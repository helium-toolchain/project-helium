namespace Helium.Data.Castle.Serialization;

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

using Helium.Data.Abstraction;

public unsafe ref struct BinaryCastleReader
{
	public Int32 MaximumDepth { get; set; } = 512;

	public Span<Byte> Blob { get; init; }

	[RequiresPreviewFeatures]
	public CastleRootToken Deserialize()
	{
		CastleRootToken root = new();

		fixed(Byte* trash = Blob)	// trash is fixed, but we do pointer arithmetics from here onwards, which is why we call this 
		{							// trash and have a second pointer. fuck off, .NET GC. wish i could do this in C++...
			Byte* handle = trash;

			handle += 5;			// the first byte is 00, if it isnt, the data is invalid anyway, but error handling isnt the task of this code block
									// the next four bytes denote the name array length. we need to read that thing. we skip the length decl.
									// okay. now that we are looking at the actual Castle data, lets proceed...

			// set up some things for the main loop
			Stack<UInt16> stateStack = new();

			ICastleParentToken activeToken = root;
			IDataToken currentToken = root;

			CastleTokenType currentConstructedType = CastleTokenType.Root;
			UInt16 stringLength = 0, stringId = 0, arrayLength = 0;
			Int16 shortValue = 0;
			Int32 intValue = 0;
			Int64 longValue = 0; // general purpose values needed for exactly three tokens :YEP:

			Span<Byte> oneLengthSpan;
			Span<Byte> twoLengthSpan;
			Span<Byte> fourLengthSpan;
			Span<Byte> eightLengthSpan;
			Span<Byte> buffer;

			// setup done, lets move on to the name array

			while(true)
			{
				twoLengthSpan = new(handle, 2);				// assign the next length to our two-length span
				handle += 2;                                // and increment the pointer because it doesnt do that automatically

				stringLength = BinaryPrimitives.ReadUInt16LittleEndian(twoLengthSpan);

				if(stringLength == 0)	// cool, this is the name deduplication ID of the root token. proceed to parse.
				{
					break; 
				}

				buffer = new(handle, stringLength); // same procedure as above
				handle += stringLength;

				root.TokenNames.Add(Encoding.ASCII.GetString(buffer));
			}

			twoLengthSpan = new(handle, 2);
			handle += 2;

			// id like to know when we're done here

			stateStack.Push(Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan)));

			// time to parse :concern:

			while(true)
			{
				oneLengthSpan = new(handle, 1);		// by now we understand how these work. i hope so, anyway
				handle++;

				currentConstructedType = (CastleTokenType)oneLengthSpan[0];

				if(currentConstructedType == CastleTokenType.Compound || currentConstructedType == CastleTokenType.List)
				{
					handle += 4; // if that is the case, we have a length declarator to skip
				}

				if(activeToken.RefDeclarator != (Byte)CastleTokenType.List)
				{
					twoLengthSpan = new(handle, 2);     // load the string ID
					handle += 2;

					stringId = BinaryPrimitives.ReadUInt16LittleEndian(twoLengthSpan); // name deduplication ID acquired
				}
				else
				{
					stringId = 0;		// dummy name deduplication ID. we know 0 exists, so it wont cause issues.
										// 0 exists in any compound token that contains a list, for the record, since the list
				}						// will need a name, so therefore even if there are no other tokens, 0 will be assigned.

				switch(currentConstructedType)
				{
					case CastleTokenType.Byte:

						oneLengthSpan = new(handle, 1);
						handle++;

						currentToken = new CastleByteToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = oneLengthSpan[0]
						};
						break;

					case CastleTokenType.SByte:

						oneLengthSpan = new(handle, 1);
						handle++;

						currentToken = new CastleSByteToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = (SByte)oneLengthSpan[0]
						};
						break;

					case CastleTokenType.Int16:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						currentToken = new CastleInt16Token()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = Unsafe.As<Byte, Int16>(ref MemoryMarshal.GetReference(twoLengthSpan))
						};
						break;

						// yes, this Unsafe.As hell could easily be written as MemoryMarshal.Cast, but SPEED.
						// Unsafe.As compiles to `ldarg.0; ret` in CIL, MemoryMarshal.Cast to substantially more instructions
						// additionally, since this code doesnt have to deal with mojank... ever... we can afford
						// removing some safety checking provided by MemoryMarshal.Cast

					case CastleTokenType.UInt16:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						currentToken = new CastleUInt16Token()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan))
						};
						break;

					case CastleTokenType.Int32:

						fourLengthSpan = new(handle, 4);
						handle += 4;

						currentToken = new CastleInt32Token()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = Unsafe.As<Byte, Int32>(ref MemoryMarshal.GetReference(fourLengthSpan))
						};
						break;

					case CastleTokenType.UInt32:

						fourLengthSpan = new(handle, 4);
						handle += 4;

						currentToken = new CastleUInt32Token()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = Unsafe.As<Byte, UInt32>(ref MemoryMarshal.GetReference(fourLengthSpan))
						};
						break;

					case CastleTokenType.Int64:

						eightLengthSpan = new(handle, 8);
						handle += 8;

						currentToken = new CastleInt64Token()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = Unsafe.As<Byte, Int64>(ref MemoryMarshal.GetReference(eightLengthSpan))
						};
						break;

					case CastleTokenType.UInt64:

						eightLengthSpan = new(handle, 8);
						handle += 8;

						currentToken = new CastleUInt64Token()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = Unsafe.As<Byte, UInt64>(ref MemoryMarshal.GetReference(eightLengthSpan))
						};
						break;

					case CastleTokenType.Half:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						currentToken = new CastleHalfToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = Unsafe.As<Byte, Half>(ref MemoryMarshal.GetReference(twoLengthSpan))
						};
						break;

					case CastleTokenType.Single:

						fourLengthSpan = new(handle, 4);
						handle += 4;

						currentToken = new CastleSingleToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = Unsafe.As<Byte, Single>(ref MemoryMarshal.GetReference(fourLengthSpan))
						};
						break;

					case CastleTokenType.Double:

						eightLengthSpan = new(handle, 8);
						handle += 8;

						currentToken = new CastleDoubleToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = Unsafe.As<Byte, Double>(ref MemoryMarshal.GetReference(eightLengthSpan))
						};
						break;

					case CastleTokenType.Guid:

						buffer = new(handle, 16);	// this cant be a "sixteenLengthSpan" because there are no 128-bit
						handle += 16;                       // general purpose registers on x64/aarch64. and thats what we use these for.

						currentToken = new CastleGuidToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = Unsafe.As<Byte, Guid>(ref MemoryMarshal.GetReference(buffer))
						};
						break;

					case CastleTokenType.ByteArray:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						arrayLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						buffer = new(handle, arrayLength);
						handle += arrayLength;

						currentToken = new CastleByteArrayToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Children = buffer.ToArray().ToList()
						};
						break;

					case CastleTokenType.SByteArray:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						arrayLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						buffer = new(handle, arrayLength);
						handle += arrayLength;

						currentToken = new CastleSByteArrayToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Children = MemoryMarshal.Cast<Byte, SByte>(buffer).ToArray().ToList()
						};
						break;

					case CastleTokenType.Int16Array:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						arrayLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						buffer = new(handle, arrayLength * 2);
						handle += arrayLength * 2;

						currentToken = new CastleInt16ArrayToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Children = MemoryMarshal.Cast<Byte, Int16>(buffer).ToArray().ToList()
						};
						break;

					case CastleTokenType.UInt16Array:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						arrayLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						buffer = new(handle, arrayLength * 2);
						handle += arrayLength * 2;

						currentToken = new CastleUInt16ArrayToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Children = MemoryMarshal.Cast<Byte, UInt16>(buffer).ToArray().ToList()
						};
						break;

					case CastleTokenType.Int32Array:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						arrayLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						buffer = new(handle, arrayLength * 4);
						handle += arrayLength * 4;

						currentToken = new CastleInt32ArrayToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Children = MemoryMarshal.Cast<Byte, Int32>(buffer).ToArray().ToList()
						};
						break;

					case CastleTokenType.UInt32Array:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						arrayLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						buffer = new(handle, arrayLength * 4);
						handle += arrayLength * 4;

						currentToken = new CastleUInt32ArrayToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Children = MemoryMarshal.Cast<Byte, UInt32>(buffer).ToArray().ToList()
						};
						break;

					case CastleTokenType.Int64Array:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						arrayLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						buffer = new(handle, arrayLength * 8);
						handle += arrayLength * 8;

						currentToken = new CastleInt64ArrayToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Children = MemoryMarshal.Cast<Byte, Int64>(buffer).ToArray().ToList()
						};
						break;

					case CastleTokenType.UInt64Array:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						arrayLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						buffer = new(handle, arrayLength * 8);
						handle += arrayLength * 8;

						currentToken = new CastleUInt64ArrayToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Children = MemoryMarshal.Cast<Byte, UInt64>(buffer).ToArray().ToList()
						};
						break;

					case CastleTokenType.HalfArray:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						arrayLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						buffer = new(handle, arrayLength * 2);
						handle += arrayLength * 2;

						currentToken = new CastleHalfArrayToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Children = MemoryMarshal.Cast<Byte, Half>(buffer).ToArray().ToList()
						};
						break;

					case CastleTokenType.SingleArray:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						arrayLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						buffer = new(handle, arrayLength * 4);
						handle += arrayLength * 4;

						currentToken = new CastleSingleArrayToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Children = MemoryMarshal.Cast<Byte, Single>(buffer).ToArray().ToList()
						};
						break;

					case CastleTokenType.DoubleArray:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						arrayLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						buffer = new(handle, arrayLength * 8);
						handle += arrayLength * 8;

						currentToken = new CastleDoubleArrayToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Children = MemoryMarshal.Cast<Byte, Double>(buffer).ToArray().ToList()
						};
						break;

					case CastleTokenType.String:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						stringLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						buffer = new(handle, stringLength);
						handle += stringLength;

						currentToken = new CastleStringToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = Encoding.UTF8.GetString(buffer)
						};
						break;

					case CastleTokenType.UTF16String:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						stringLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						buffer = new(handle, stringLength * 2);
						handle += stringLength * 2;

						currentToken = new CastleString16Token()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = Encoding.Unicode.GetString(buffer) // fuck knows why its called Unicode and not UTF16
						};
						break;

					case CastleTokenType.DateTime:

						eightLengthSpan = new(handle, 8);
						handle += 8;

						longValue = Unsafe.As<Byte, Int64>(ref MemoryMarshal.GetReference(eightLengthSpan));

						twoLengthSpan = new(handle, 2);
						handle += 2;

						shortValue = Unsafe.As<Byte, Int16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						currentToken = new CastleDateTimeToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = new(longValue, TimeSpan.FromMinutes(shortValue))
						};
						break;

					case CastleTokenType.Date:

						fourLengthSpan = new(handle, 4);
						handle += 4;

						intValue = Unsafe.As<Byte, Int32>(ref MemoryMarshal.GetReference(fourLengthSpan));

						currentToken = new CastleDateToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = DateOnly.FromDayNumber(intValue)
						};
						break;

					case CastleTokenType.Time:

						eightLengthSpan = new(handle, 8);
						handle += 8;

						longValue = Unsafe.As<Byte, Int64>(ref MemoryMarshal.GetReference(eightLengthSpan));

						currentToken = new CastleTimeToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root,
							Value = new(longValue)
						};
						break;

					case CastleTokenType.List:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						arrayLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						oneLengthSpan = new(handle, 1);
						handle++;

						currentToken = new CastleListToken()
						{
							ListTypeDeclarator = oneLengthSpan[0],
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root
						};

						activeToken.AddChildToken(currentToken);

						activeToken = (ICastleParentToken)currentToken;

						stateStack.Push(arrayLength);
						continue;

					case CastleTokenType.Compound:

						twoLengthSpan = new(handle, 2);
						handle += 2;

						arrayLength = Unsafe.As<Byte, UInt16>(ref MemoryMarshal.GetReference(twoLengthSpan));

						currentToken = new CastleCompoundToken()
						{
							NameId = stringId,
							ParentToken = activeToken,
							RootToken = root
						};

						activeToken.AddChildToken(currentToken);

						activeToken = (ICastleParentToken)currentToken;

						stateStack.Push(arrayLength);
						continue;

				}

				activeToken.AddChildToken(currentToken);

				if(activeToken is CastleCompoundToken compound)
				{
					if(stateStack.Peek() == compound.Count)
					{
						stateStack.Pop();
						activeToken = (ICastleParentToken)activeToken.ParentToken!;
					}
				}
				else if(activeToken is CastleListToken list)
				{
					if(stateStack.Peek() == list.Count)
					{
						stateStack.Pop();
						activeToken = (ICastleParentToken)activeToken.ParentToken!;
					}
				}
				else if(activeToken is CastleRootToken rootToken)
				{
					if(stateStack.Peek() == rootToken.Count)
					{
						goto RETURN;
					}
				}
			}
		}

		RETURN:

		return root;
	}
}
