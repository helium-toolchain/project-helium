namespace Helium.Data.Nbt.Serialization;

using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Versioning;
using System.Text;

using Helium.Data.Abstraction;

/// <summary>
/// Provides functionality for parsing NBT from binary data.
/// </summary>
[RequiresPreviewFeatures]
public sealed class BinaryNbtReader
{
	/// <summary>
	/// Stores how deep a data structure is allowed to be. 0 causes this value to be ignored.
	/// Defaults to 512 to mimic the NBT specification behaviour.
	/// </summary>
	public Int32 MaximumDepth { get; set; } = 512;

	/// <summary>
	/// Controls throwing exceptions. This is ignored in Debug builds.
	/// </summary>
	public Boolean ThrowExceptions { get; set; } = true;

	/// <summary>
	/// Reads NBT from a string and returns the deserialized NBT tree.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public NbtRootToken Deserialize(String data)
	{
		return this.Deserialize(new MemoryStream(Encoding.UTF8.GetBytes(data)));
	}

	/// <summary>
	/// Reads NBT from a byte array and returns the deserialized NBT tree.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public NbtRootToken Deserialize(Byte[] data)
	{
		return this.Deserialize(new MemoryStream(data));
	}

	/// <summary>
	/// Reads NBT from a MemoryStream and returns the deserialized NBT tree.
	/// </summary>
	/// <exception cref="ArgumentException">Thrown if the passed NBT data is invalid in one way or another.</exception>
	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public NbtRootToken Deserialize(MemoryStream data)
	{
		Stack<CurrentTokenState> stateStack = new();

		String currentName;
		NbtRootToken root = new();
		IDataToken activeToken = root, currentToken = root;
		NbtTokenType currentConstructedType = NbtTokenType.End;

		Int16 stringLength = 0;
		Int32 arrayLength = 0, arrayLengthMod4 = 0, arrayLengthMod2 = 0, arrayLengthMod8 = 0;

		// the runtime optimizes these into registers on x64 and aarch64. speeeeeed.
		Span<Byte> oneLengthSpan = stackalloc Byte[1];
		Span<Byte> twoLengthSpan = stackalloc Byte[2];
		Span<Byte> fourLengthSpan = stackalloc Byte[4];
		Span<Byte> eightLengthSpan = stackalloc Byte[8];

		stateStack.Push(new()
		{
			CurrentTokenType = CurrentTokenType.Root,
			RemainingChildren = -1,
			ListTokenType = NbtTokenType.End
		});

		while(true)
		{
			if(stateStack.Count > this.MaximumDepth && this.MaximumDepth != 0)
			{
#if !DEBUG
				if(this.ThrowExceptions)
				{
#endif
					ThrowHelper.ThrowMaximumDepthExceeded(stateStack.Count, this.MaximumDepth);
#if !DEBUG
				}
				else
				{
					return null!;
				}
#endif
			}

			if(stateStack.Peek().CurrentTokenType != CurrentTokenType.List)
			{
				currentConstructedType = (NbtTokenType)(Byte)data.ReadByte();

				data.Read(twoLengthSpan);
				stringLength = BinaryPrimitives.ReadInt16BigEndian(twoLengthSpan);

				Byte[] name = data.ReadBytes(stringLength);

				currentName = Encoding.UTF8.GetString(name);

				switch(currentConstructedType)
				{
					case NbtTokenType.SByte:
						data.Read(oneLengthSpan);
						currentToken = new NbtSByteToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
							Value = (SByte)oneLengthSpan[0]
						};
						break;

					case NbtTokenType.Int16:
						data.Read(twoLengthSpan);
						currentToken = new NbtInt16Token()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
							Value = BinaryPrimitives.ReadInt16BigEndian(twoLengthSpan)
						};
						break;

					case NbtTokenType.Int32:
						data.Read(fourLengthSpan);
						currentToken = new NbtInt32Token()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
							Value = BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan)
						};
						break;

					case NbtTokenType.Int64:
						data.Read(eightLengthSpan);
						currentToken = new NbtInt64Token()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
							Value = BinaryPrimitives.ReadInt64BigEndian(eightLengthSpan)
						};
						break;

					case NbtTokenType.Single:
						data.Read(fourLengthSpan);
						currentToken = new NbtSingleToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
							Value = BinaryPrimitives.ReadSingleBigEndian(fourLengthSpan)
						};
						break;

					case NbtTokenType.Double:
						data.Read(eightLengthSpan);
						currentToken = new NbtDoubleToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
							Value = BinaryPrimitives.ReadDoubleBigEndian(eightLengthSpan)
						};
						break;

					case NbtTokenType.String:
						data.Read(twoLengthSpan);
						stringLength = BinaryPrimitives.ReadInt16BigEndian(twoLengthSpan);

						currentToken = new NbtStringToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
							Value = Encoding.UTF8.GetString(data.ReadBytes(stringLength))
						};
						break;

					case NbtTokenType.SByteArray:
						data.Read(fourLengthSpan);
						arrayLength = BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan);

						Span<Byte> t1 = new(data.ReadBytes(arrayLength));

						currentToken = new NbtSByteArrayToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
						};

						(currentToken as NbtSByteArrayToken)!.SetChildren(MemoryMarshal.Cast<Byte, SByte>(t1));
						break;

					case NbtTokenType.Int32Array:

						data.Read(fourLengthSpan);
						arrayLength = BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan);

						Span<Byte> t2 = new(data.ReadBytes(arrayLength * 4));

						currentToken = new NbtInt32ArrayToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root
						};

						// we can switch the endianness up
						if(RuntimeInformation.ProcessArchitecture == Architecture.X64 && Avx2.IsSupported &&
							arrayLength >= 8) // avx2 places a hard dependency on avx, we dont need to check that
						{
							unsafe
							{
								Vector256<Byte> i32Mask = Vector256.Create(
									(Byte)28, 29, 30, 31, 24, 25, 26, 27, 20, 21, 22, 23, 16, 17, 18, 19,
									12, 13, 14, 15, 8, 9, 10, 11, 4, 5, 6, 7, 0, 1, 2, 3);

								arrayLengthMod8 = arrayLength - (arrayLength % 8);
								Span<Byte> finalInt32 = new Byte[arrayLengthMod8 * 4];

								// keep the GC from being a prick
								fixed(Byte* reshuffled = finalInt32)
								{
									for(Int32 i = 0; i < arrayLengthMod8; i += 32)
									{
										/* Load 32 bytes from our raw origin span
										 * -
										 * Re-shuffle the bytes according to the mask defined above; inversing their endianness.
										 * -
										 * Store this into our pointer and proceed with the next iteration
										 * -
										 * This is available in AVX-512 but .NET does not support AVX-512 yet either; it is not
										 * widely adopted and therefore a low priority for the .NET team. This code area should be
										 * updated once those are implemented in System.Runtime.Intrinsics.
										 */
										Avx.Store(reshuffled, Avx2.Shuffle(Unsafe.As<Byte, Vector256<Byte>>(ref MemoryMarshal.GetReference(t2.Slice(i, 32))), i32Mask));
									}
								}

								Span<Int32> littleEndianInt32 = MemoryMarshal.Cast<Byte, Int32>(finalInt32);

								(currentToken as NbtInt32ArrayToken)!.SetChildren(littleEndianInt32);
							}

							for(Int32 i = 0; i < arrayLength % 4; i++)
							{
								data.Read(fourLengthSpan);
								(currentToken as NbtInt32ArrayToken)!.Add(BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan));
							}
						}
						// oh woo, we can switch it up, but differently
						else if(RuntimeInformation.ProcessArchitecture == Architecture.X64 && Ssse3.IsSupported &&
							Sse2.IsSupported && arrayLength >= 8) // its only faster when doing this twice
						{
							unsafe
							{
								Vector128<Byte> i32Mask = 
									Vector128.Create((Byte)12, 13, 14, 15, 8, 9, 10, 11, 4, 5, 6, 7, 0, 1, 2, 3);

								arrayLengthMod4 = arrayLength - (arrayLength % 4);
								Span<Byte> finalInt32 = new Byte[arrayLengthMod4 * 4];

								// keep the GC from doing evil things
								fixed(Byte* reshuffled = finalInt32)
								{
									for(Int32 i = 0; i < arrayLengthMod4; i += 16)
									{
										/* Load 16 bytes from our raw origin span
										 * -
										 * ref T Span<T>#GetPinnableReference() is actually hidden from IntelliSense and should not
										 * be 'called by user code', well, how about we do it anyway.
										 * -
										 * Re-shuffle the bytes according to the mask defined above; inversing their endianness.
										 * -
										 * Store this into our pointer and proceed with the next iteration
										 * -
										 * This is available in AVX-512 but .NET does not support AVX-512 yet either; it is not
										 * widely adopted and therefore a low priority for the .NET team. This code area should be
										 * updated once those are implemented in System.Runtime.Intrinsics.
										 */
										Sse2.Store(reshuffled, Ssse3.Shuffle(Unsafe.As<Byte, Vector128<Byte>>(ref MemoryMarshal.GetReference(t2.Slice(i, 16))), i32Mask));
									}
								}

								Span<Int32> littleEndianInt32 = MemoryMarshal.Cast<Byte, Int32>(finalInt32);

								(currentToken as NbtInt32ArrayToken)!.SetChildren(littleEndianInt32);
							}

							for(Int32 i = 0; i < arrayLength % 4; i++)
							{
								data.Read(fourLengthSpan);
								(currentToken as NbtInt32ArrayToken)!.Add(BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan));
							}
						}
						else
						{
							for(Int32 i = 0; i < arrayLength; i++)
							{
								data.Read(fourLengthSpan);
								(currentToken as NbtInt32ArrayToken)!.Add(BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan));
							}
						}
						break;

					case NbtTokenType.Int64Array:
						data.Read(fourLengthSpan);
						arrayLength = BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan);

						Span<Byte> t3 = new(data.ReadBytes(arrayLength * 8));

						currentToken = new NbtInt64ArrayToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root
						};

						// we can switch the endianness up
						if(RuntimeInformation.ProcessArchitecture == Architecture.X64 && Avx2.IsSupported &&
							arrayLength >= 4) // avx2 places a hard dependency on avx, we dont need to check that
						{
							unsafe
							{
								Vector256<Byte> i64Mask = Vector256.Create(
									(Byte)24, 25, 26, 27, 28, 29, 30, 31, 16, 17, 18, 19, 20, 21, 22, 23,
									8, 9, 10, 11, 12, 13, 14, 15, 0, 1, 2, 3, 4, 5, 6, 7);

								arrayLengthMod4 = arrayLength - (arrayLength % 4);
								Span<Byte> finalInt64 = new Byte[arrayLengthMod4 * 8];

								// keep the GC from being a prick
								fixed(Byte* reshuffled = finalInt64)
								{
									for(Int32 i = 0; i < arrayLengthMod8; i += 32)
									{
										/* Load 32 bytes from our raw origin span
										 * -
										 * Re-shuffle the bytes according to the mask defined above; inversing their endianness.
										 * -
										 * Store this into our pointer and proceed with the next iteration
										 * -
										 * .NET does not support aarch64-neon sufficiently yet. A different approach might be possible.
										 * This is available in AVX-512 but .NET does not support AVX-512 yet either; it is not
										 * widely adopted and therefore a low priority for the .NET team. This code area should be
										 * updated once those are implemented in System.Runtime.Intrinsics.
										 */
										Avx.Store(reshuffled, Avx2.Shuffle(Unsafe.As<Byte, Vector256<Byte>>(ref MemoryMarshal.GetReference(t3.Slice(i, 32))), i64Mask));
									}
								}

								Span<Int64> littleEndianInt64 = MemoryMarshal.Cast<Byte, Int64>(finalInt64);

								(currentToken as NbtInt64ArrayToken)!.SetChildren(littleEndianInt64);
							}

							for(Int32 i = 0; i < arrayLength % 4; i++)
							{
								data.Read(fourLengthSpan);
								(currentToken as NbtInt64ArrayToken)!.Add(BinaryPrimitives.ReadInt64BigEndian(fourLengthSpan));
							}
						}
						// oh woo, we can switch it up, but differently
						else if(RuntimeInformation.ProcessArchitecture == Architecture.X64 && Ssse3.IsSupported &&
							Sse2.IsSupported && arrayLength >= 4) // its only faster when doing this twice
						{
							unsafe
							{
								Vector128<Byte> i64Mask =
									Vector128.Create((Byte)8, 9, 10, 11, 12, 13, 14, 15, 0, 1, 2, 3, 4, 5, 6, 7);

								arrayLengthMod2 = arrayLength - (arrayLength % 2);
								Span<Byte> finalInt64 = new Byte[arrayLengthMod2 * 8];

								// keep the GC from doing evil things
								fixed(Byte* reshuffled = finalInt64)
								{
									for(Int32 i = 0; i < arrayLengthMod4; i += 16)
									{
										/* Load 16 bytes from our raw origin span
										 * -
										 * ref T Span<T>#GetPinnableReference() is actually hidden from IntelliSense and should not
										 * be 'called by user code', well, how about we do it anyway.
										 * -
										 * Re-shuffle the bytes according to the mask defined above; inversing their endianness.
										 * -
										 * Store this into our pointer and proceed with the next iteration
										 * -
										 * This is available in AVX-512 but .NET does not support AVX-512 yet either; it is not
										 * widely adopted and therefore a low priority for the .NET team. This code area should be
										 * updated once those are implemented in System.Runtime.Intrinsics.
										 */
										Sse2.Store(reshuffled, Ssse3.Shuffle(Unsafe.As<Byte, Vector128<Byte>>(ref MemoryMarshal.GetReference(t3.Slice(i, 16))), i64Mask));
									}
								}

								Span<Int64> littleEndianInt64 = MemoryMarshal.Cast<Byte, Int64>(finalInt64);

								(currentToken as NbtInt64ArrayToken)!.SetChildren(littleEndianInt64);
							}

							for(Int32 i = 0; i < arrayLength % 4; i++)
							{
								data.Read(eightLengthSpan);
								(currentToken as NbtInt64ArrayToken)!.Add(BinaryPrimitives.ReadInt64BigEndian(eightLengthSpan));
							}
						}
						else
						{
							for(Int32 i = 0; i < arrayLength; i++)
							{
								data.Read(eightLengthSpan);
								(currentToken as NbtInt64ArrayToken)!.Add(BinaryPrimitives.ReadInt64BigEndian(eightLengthSpan));
							}
						}
						break;

					case NbtTokenType.List:
						NbtTokenType listType = (NbtTokenType)(Byte)data.ReadByte();
						data.Read(fourLengthSpan);

						arrayLength = BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan);

						if(listType == NbtTokenType.End && arrayLength > 0
#if !DEBUG
							&& this.ThrowExceptions
#endif
							)
						{
							ThrowHelper.ThrowInvalidListTokenType(NbtTokenType.End);
						}

						currentToken = new NbtListToken()
						{
							ListTypeDeclarator = (Byte)listType,
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root
						};

						(activeToken as ICompoundToken)!.AddChildToken(currentToken);
						activeToken = currentToken;

						stateStack.Push(new()
						{
							CurrentTokenType = CurrentTokenType.List,
							ListTokenType = listType,
							RemainingChildren = arrayLength
						});
						continue;

					case NbtTokenType.Compound:
						currentToken = new NbtCompoundToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root
						};

						(activeToken as ICompoundToken)!.AddChildToken(currentToken);
						activeToken = currentToken;

						stateStack.Push(new()
						{
							CurrentTokenType = CurrentTokenType.Compound
						});

						continue;

					case NbtTokenType.End:

#pragma warning disable CS0252 // very much intended reference comparison
						if(activeToken == root)
#pragma warning restore CS0252
						{
							goto LOOP_END;
						}

						activeToken = activeToken.ParentToken!;

						stateStack.Pop();
						continue;
				}

				(activeToken as ICompoundToken)?.AddChildToken(currentToken);
			}
			else
			{
				if(stateStack.Peek().RemainingChildren <= 0)
				{
					activeToken = activeToken.ParentToken!;

					stateStack.Pop();
					continue;
				}

				CurrentTokenState state = stateStack.Pop();
				state.RemainingChildren--;
				stateStack.Push(state);

				currentName = "List member";

				switch(stateStack.Peek().ListTokenType)
				{
					case NbtTokenType.SByte:
						data.Read(oneLengthSpan);
						currentToken = new NbtSByteToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
							Value = (SByte)oneLengthSpan[0]
						};
						break;

					case NbtTokenType.Int16:
						data.Read(twoLengthSpan);
						currentToken = new NbtInt16Token()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
							Value = BinaryPrimitives.ReadInt16BigEndian(twoLengthSpan)
						};
						break;

					case NbtTokenType.Int32:
						data.Read(fourLengthSpan);
						currentToken = new NbtInt32Token()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
							Value = BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan)
						};
						break;

					case NbtTokenType.Int64:
						data.Read(eightLengthSpan);
						currentToken = new NbtInt64Token()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
							Value = BinaryPrimitives.ReadInt64BigEndian(eightLengthSpan)
						};
						break;

					case NbtTokenType.Single:
						data.Read(fourLengthSpan);
						currentToken = new NbtSingleToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
							Value = BinaryPrimitives.ReadSingleBigEndian(fourLengthSpan)
						};
						break;

					case NbtTokenType.Double:
						data.Read(eightLengthSpan);
						currentToken = new NbtDoubleToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
							Value = BinaryPrimitives.ReadDoubleBigEndian(eightLengthSpan)
						};
						break;

					case NbtTokenType.String:
						data.Read(twoLengthSpan);
						stringLength = BinaryPrimitives.ReadInt16BigEndian(twoLengthSpan);

						currentToken = new NbtStringToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
							Value = Encoding.UTF8.GetString(data.ReadBytes(stringLength))
						};
						break;

					case NbtTokenType.SByteArray:
						data.Read(fourLengthSpan);
						arrayLength = BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan);

						Span<Byte> t1 = new(data.ReadBytes(arrayLength));

						currentToken = new NbtSByteArrayToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root,
						};

						(currentToken as NbtSByteArrayToken)!.SetChildren(MemoryMarshal.Cast<Byte, SByte>(t1));
						break;

					case NbtTokenType.Int32Array:

						data.Read(fourLengthSpan);
						arrayLength = BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan);

						Span<Byte> t2 = new(data.ReadBytes(arrayLength * 4));

						currentToken = new NbtInt32ArrayToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root
						};

						// we can switch the endianness up
						if(RuntimeInformation.ProcessArchitecture == Architecture.X64 && Avx2.IsSupported &&
							arrayLength > 8) // avx2 places a hard dependency on avx, we dont need to check that
						{
							unsafe
							{
								Vector256<Byte> i32Mask = Vector256.Create(
									(Byte)28, 29, 30, 31, 24, 25, 26, 27, 20, 21, 22, 23, 16, 17, 18, 19,
									12, 13, 14, 15, 8, 9, 10, 11, 4, 5, 6, 7, 0, 1, 2, 3);

								arrayLengthMod8 = arrayLength - (arrayLength % 8);
								Span<Byte> finalInt32 = new Byte[arrayLengthMod8 * 4];

								// keep the GC from being a prick
								fixed(Byte* reshuffled = finalInt32)
								{
									for(Int32 i = 0; i < arrayLengthMod8; i += 32)
									{
										/* Load 32 bytes from our raw origin span
										 * -
										 * Re-shuffle the bytes according to the mask defined above; inversing their endianness.
										 * -
										 * Store this into our pointer and proceed with the next iteration
										 * -
										 * This is available in AVX-512 but .NET does not support AVX-512 yet either; it is not
										 * widely adopted and therefore a low priority for the .NET team. This code area should be
										 * updated once those are implemented in System.Runtime.Intrinsics.
										 */
										Avx.Store(reshuffled, Avx2.Shuffle(Avx.LoadVector256(
											(Byte*)Unsafe.AsPointer(ref t2.Slice(i, 32).GetPinnableReference())),
											i32Mask));
									}
								}

								Span<Int32> littleEndianInt32 = MemoryMarshal.Cast<Byte, Int32>(finalInt32);

								(currentToken as NbtInt32ArrayToken)!.SetChildren(littleEndianInt32);
							}

							for(Int32 i = 0; i < arrayLength % 4; i++)
							{
								data.Read(fourLengthSpan);
								(currentToken as NbtInt32ArrayToken)!.Add(BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan));
							}
						}
						// oh woo, we can switch it up, but differently
						else if(RuntimeInformation.ProcessArchitecture == Architecture.X64 && Ssse3.IsSupported &&
							Sse2.IsSupported && arrayLength > 8) // its only faster when doing this twice
						{
							unsafe
							{
								Vector128<Byte> i32Mask =
									Vector128.Create((Byte)12, 13, 14, 15, 8, 9, 10, 11, 4, 5, 6, 7, 0, 1, 2, 3);

								arrayLengthMod4 = arrayLength - (arrayLength % 4);
								Span<Byte> finalInt32 = new Byte[arrayLengthMod4 * 4];

								// keep the GC from doing evil things
								fixed(Byte* reshuffled = finalInt32)
								{
									for(Int32 i = 0; i < arrayLengthMod4; i += 16)
									{
										/* Load 16 bytes from our raw origin span
										 * -
										 * ref T Span<T>#GetPinnableReference() is actually hidden from IntelliSense and should not
										 * be 'called by user code', well, how about we do it anyway.
										 * -
										 * Re-shuffle the bytes according to the mask defined above; inversing their endianness.
										 * -
										 * Store this into our pointer and proceed with the next iteration
										 * -
										 * This is available in AVX-512 but .NET does not support AVX-512 yet either; it is not
										 * widely adopted and therefore a low priority for the .NET team. This code area should be
										 * updated once those are implemented in System.Runtime.Intrinsics.
										 */
										Sse2.Store(reshuffled, Ssse3.Shuffle(Sse2.LoadVector128(
											(Byte*)Unsafe.AsPointer(ref t2.Slice(i, 16).GetPinnableReference())),
											i32Mask));
									}
								}

								Span<Int32> littleEndianInt32 = MemoryMarshal.Cast<Byte, Int32>(finalInt32);

								(currentToken as NbtInt32ArrayToken)!.SetChildren(littleEndianInt32);
							}

							for(Int32 i = 0; i < arrayLength % 4; i++)
							{
								data.Read(fourLengthSpan);
								(currentToken as NbtInt32ArrayToken)!.Add(BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan));
							}
						}
						else if(RuntimeInformation.ProcessArchitecture == Architecture.Arm64 && AdvSimd.IsSupported &&
							arrayLength > 8) // arm woo! this isnt worth it for int64 though
						{
							unsafe
							{
								arrayLengthMod4 = arrayLength - (arrayLength % 4);
								Span<Int32> finalInt32 = new Int32[arrayLengthMod4];

								// keep the GC from doing evil things
								fixed(Int32* reshuffled = finalInt32)
								{
									for(Int32 i = 0; i < arrayLengthMod4; i += 16)
									{
										/* Load 16 bytes from our raw origin span
										 * -
										 * ref T Span<T>#GetPinnableReference() is actually hidden from IntelliSense and should not
										 * be 'called by user code', well, how about we do it anyway.
										 * -
										 * Use the ARM hardware intrinsics to perform a endianness reverse. no need for pshufb 
										 * (or its AVX2 equivalent) here.
										 * -
										 * Store this into our pointer and proceed with the next iteration
										 * -
										 * This is available in AVX-512 but .NET does not support AVX-512 yet either; it is not
										 * widely adopted and therefore a low priority for the .NET team. This code area should be
										 * updated once those are implemented in System.Runtime.Intrinsics.
										 */
										AdvSimd.Store(reshuffled, AdvSimd.ReverseElement16(AdvSimd.LoadVector128(
											(Int32*)Unsafe.AsPointer(ref t2.Slice(i, 16).GetPinnableReference()))));
									}
								}

								(currentToken as NbtInt32ArrayToken)!.SetChildren(finalInt32);
							}

							for(Int32 i = 0; i < arrayLength % 4; i++)
							{
								data.Read(fourLengthSpan);
								(currentToken as NbtInt32ArrayToken)!.Add(BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan));
							}
						}
						else
						{
							for(Int32 i = 0; i < arrayLength; i++)
							{
								data.Read(fourLengthSpan);
								(currentToken as NbtInt32ArrayToken)!.Add(BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan));
							}
						}
						break;

					case NbtTokenType.Int64Array:
						data.Read(fourLengthSpan);
						arrayLength = BinaryPrimitives.ReadInt32BigEndian(fourLengthSpan);

						Span<Byte> t3 = new(data.ReadBytes(arrayLength * 8));

						currentToken = new NbtInt64ArrayToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root
						};

						// we can switch the endianness up
						if(RuntimeInformation.ProcessArchitecture == Architecture.X64 && Avx2.IsSupported &&
							arrayLength > 4) // avx2 places a hard dependency on avx, we dont need to check that
						{
							unsafe
							{
								Vector256<Byte> i64Mask = Vector256.Create(
									(Byte)24, 25, 26, 27, 28, 29, 30, 31, 16, 17, 18, 19, 20, 21, 22, 23,
									8, 9, 10, 11, 12, 13, 14, 15, 0, 1, 2, 3, 4, 5, 6, 7);

								arrayLengthMod4 = arrayLength - (arrayLength % 4);
								Span<Byte> finalInt64 = new Byte[arrayLengthMod4 * 8];

								// keep the GC from being a prick
								fixed(Byte* reshuffled = finalInt64)
								{
									for(Int32 i = 0; i < arrayLengthMod8; i += 32)
									{
										/* Load 32 bytes from our raw origin span
										 * -
										 * Re-shuffle the bytes according to the mask defined above; inversing their endianness.
										 * -
										 * Store this into our pointer and proceed with the next iteration
										 * -
										 * .NET does not support aarch64-neon sufficiently yet. A different approach might be possible.
										 * This is available in AVX-512 but .NET does not support AVX-512 yet either; it is not
										 * widely adopted and therefore a low priority for the .NET team. This code area should be
										 * updated once those are implemented in System.Runtime.Intrinsics.
										 */
										Avx.Store(reshuffled, Avx2.Shuffle(Avx.LoadVector256(
											(Byte*)Unsafe.AsPointer(ref t3.Slice(i, 32).GetPinnableReference())),
											i64Mask));
									}
								}

								Span<Int64> littleEndianInt64 = MemoryMarshal.Cast<Byte, Int64>(finalInt64);

								(currentToken as NbtInt64ArrayToken)!.SetChildren(littleEndianInt64);
							}

							for(Int32 i = 0; i < arrayLength % 4; i++)
							{
								data.Read(fourLengthSpan);
								(currentToken as NbtInt64ArrayToken)!.Add(BinaryPrimitives.ReadInt64BigEndian(fourLengthSpan));
							}
						}
						// oh woo, we can switch it up, but differently
						else if(RuntimeInformation.ProcessArchitecture == Architecture.X64 && Ssse3.IsSupported &&
							Sse2.IsSupported && arrayLength > 32) // its only faster when doing this twice
						{
							unsafe
							{
								Vector128<Byte> i64Mask =
									Vector128.Create((Byte)8, 9, 10, 11, 12, 13, 14, 15, 0, 1, 2, 3, 4, 5, 6, 7);

								arrayLengthMod2 = arrayLength - (arrayLength % 2);
								Span<Byte> finalInt64 = new Byte[arrayLengthMod2 * 8];

								// keep the GC from doing evil things
								fixed(Byte* reshuffled = finalInt64)
								{
									for(Int32 i = 0; i < arrayLengthMod4; i += 16)
									{
										/* Load 16 bytes from our raw origin span
										 * -
										 * ref T Span<T>#GetPinnableReference() is actually hidden from IntelliSense and should not
										 * be 'called by user code', well, how about we do it anyway.
										 * -
										 * Re-shuffle the bytes according to the mask defined above; inversing their endianness.
										 * -
										 * Store this into our pointer and proceed with the next iteration
										 * -
										 * This is available in AVX-512 but .NET does not support AVX-512 yet either; it is not
										 * widely adopted and therefore a low priority for the .NET team. This code area should be
										 * updated once those are implemented in System.Runtime.Intrinsics.
										 */
										Sse2.Store(reshuffled, Ssse3.Shuffle(Sse2.LoadVector128(
											(Byte*)Unsafe.AsPointer(ref t3.Slice(i, 16).GetPinnableReference())),
											i64Mask));
									}
								}

								Span<Int64> littleEndianInt64 = MemoryMarshal.Cast<Byte, Int64>(finalInt64);

								(currentToken as NbtInt64ArrayToken)!.SetChildren(littleEndianInt64);
							}

							for(Int32 i = 0; i < arrayLength % 4; i++)
							{
								data.Read(eightLengthSpan);
								(currentToken as NbtInt64ArrayToken)!.Add(BinaryPrimitives.ReadInt64BigEndian(eightLengthSpan));
							}
						}
						else
						{
							for(Int32 i = 0; i < arrayLength; i++)
							{
								data.Read(eightLengthSpan);
								(currentToken as NbtInt64ArrayToken)!.Add(BinaryPrimitives.ReadInt64BigEndian(eightLengthSpan));
							}
						}
						break;

					case NbtTokenType.List:
						ThrowHelper.ThrowInvalidListTokenType(NbtTokenType.List);
						break;

					case NbtTokenType.Compound:
						currentToken = new NbtCompoundToken()
						{
							Name = currentName,
							ParentToken = activeToken,
							RootToken = root
						};

						(activeToken as IListToken)!.AddChildToken(currentToken);
						activeToken = currentToken;

						stateStack.Push(new()
						{
							CurrentTokenType = CurrentTokenType.Compound
						});

						continue;

					case NbtTokenType.End:
						ThrowHelper.ThrowInvalidListTokenType(NbtTokenType.End);
						break;
				}

				(activeToken as IListToken)!.AddChildToken(currentToken);
			}

			break;
		}

	LOOP_END:

		return root;
	}
}
