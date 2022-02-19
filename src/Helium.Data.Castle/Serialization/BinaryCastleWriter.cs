namespace Helium.Data.Castle.Serialization;

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

[RequiresPreviewFeatures]
public unsafe ref struct BinaryCastleWriter
{
	private MemoryHandle bufferGCHandle;
	private Byte* bufferPointer;
	private Int32 bufferLength;

	private Memory<Byte> underlyingBuffer;
	private readonly CastleRootToken rootToken;

	public BinaryCastleWriter(CastleRootToken root, Int32? resizeIncrement = null)
	{
		this.bufferGCHandle = default!;
		this.bufferPointer = default!;
		this.bufferLength = 0;
		this.underlyingBuffer = default!;
		this.rootToken = root;

		if(resizeIncrement != null && resizeIncrement >= 1024) // ensure its never smaller than one memory page on x86
		{
			this.ResizeIncrement = (Int32)resizeIncrement;
		}

		this.initializeBuffer();
	}

	public Span<Byte> Buffer => underlyingBuffer.Span;

	public Int32 ResizeIncrement { get; private set; } = RuntimeInformation.ProcessArchitecture switch
	{
		Architecture.X86 => 1024,
		Architecture.X64 => 4096,
		Architecture.S390x => 4096,
		Architecture.Arm => 4096,
		Architecture.Arm64 => 65536,
		_ => 1024
	};

	private void incrementBuffer()
	{
		Memory<Byte> newBuffer = new Byte[this.bufferLength + this.ResizeIncrement];
		newBuffer.Span.Clear();
		this.bufferLength += this.ResizeIncrement;

		this.underlyingBuffer.CopyTo(newBuffer);

		this.bufferGCHandle.Dispose();

		this.underlyingBuffer = newBuffer;
		this.bufferPointer = (Byte*)MemoryMarshal.GetReference(newBuffer.Span);

		this.bufferGCHandle = underlyingBuffer.Pin();
	}

	private void incrementBufferIfNecessary(Int32 nextWrite)
	{
		// use goto to save a loop. its faster. could just use while(true) too, though... actually just think of it as while(true)
		REDO_INCREMENT_IF_NECESSARY:

		// + 7 because thats the maximum the string can be preceded by. we'd rather zero out too much
		if(this.Buffer.Trim<Byte>(0x00).Length + nextWrite + 7 > this.bufferLength)
		{
			this.incrementBuffer();
		}
		else
		{
			return;
		}

		goto REDO_INCREMENT_IF_NECESSARY;
	}

	private void initializeBuffer()
	{
		this.underlyingBuffer = new Byte[this.ResizeIncrement];
		this.underlyingBuffer.Span.Clear();
		this.bufferLength += this.ResizeIncrement;

		this.bufferGCHandle.Dispose();

		this.bufferPointer = (Byte*)MemoryMarshal.GetReference(this.underlyingBuffer.Span);

		this.bufferGCHandle = underlyingBuffer.Pin();
	}

	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	public void Serialize()
	{
		Int32 nameArrayOffset = 7, totalOffset;
		Span<Byte> blobAccess = new(this.bufferPointer, 7), valueSpan;

		// sets the first seven bytes to zero.
		// we'll be revisiting indices 1 to 4 later.
		blobAccess.Clear();

		for(UInt16 i = 0; i < this.rootToken.TokenNames.Count; i++)
		{
			// ensure we only operate on owned memory. i would not be opposed to just not doing that but this is C#
			this.incrementBufferIfNecessary(2 + this.rootToken.TokenNames[i].Length);

			// write the numeral length...
			valueSpan = new(this.bufferPointer + nameArrayOffset, 2);
			BinaryPrimitives.WriteUInt16LittleEndian(valueSpan, (UInt16)this.rootToken.TokenNames[i].Length);

			// ...and the actual string. in ASCII. 
			valueSpan = new(this.bufferPointer + nameArrayOffset + 2, this.rootToken.TokenNames[i].Length);
			Encoding.ASCII.GetBytes(this.rootToken.TokenNames[i]).CopyTo(valueSpan);

			// assemble the total length of this array
			nameArrayOffset += 2 + this.rootToken.TokenNames[i].Length;
		}

		// now that we know the length, woo! let us write it.
		// this is super cursed but its fast :COPIUM:
		blobAccess = new(this.bufferPointer + 1, 4);
		BinaryPrimitives.WriteInt32BigEndian(blobAccess, nameArrayOffset - 7); // it was intialized as 7, get rid of that

		// also write the amount of children
		blobAccess = new(this.bufferPointer + nameArrayOffset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(blobAccess, (UInt16)this.rootToken.Count);

		// prepare our reference parameter. this will store the total length, imposing a hard limit of 2GB on a Castle blob...
		// which is fine, since we're serializing in memory anyway and any library client would probably not like that much
		// memory usage. also managing that much memory from a single instance is a bit :conc: anyway, the .NET GC isnt a fan of that
		totalOffset = nameArrayOffset + 2;

		// and now... *cue Entry of the Gladiators* let the insanity begin!
		foreach(ICastleToken token in this.rootToken.Children)
		{
			this.writeToken(token, ref totalOffset);
		}

		// slice away all trailing zeroes, we dont need those
		this.underlyingBuffer = this.underlyingBuffer[..totalOffset];

		// we can now move this, now that we no longer rely on offsets
		this.bufferGCHandle.Dispose();
	}

	// one-size-fits-all writer for any given token. uses the declared token type to route this call.
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void writeToken(ICastleToken token, ref Int32 offset)
	{
		switch((CastleTokenType)token.RefDeclarator)
		{
			case CastleTokenType.Byte:
				this.writeByteToken((CastleByteToken)token, ref offset);
				break;
			case CastleTokenType.SByte:
				this.writeSByteToken((CastleSByteToken)token, ref offset);
				break;
			case CastleTokenType.Int16:
				this.writeInt16Token((CastleInt16Token)token, ref offset);
				break;
			case CastleTokenType.UInt16:
				this.writeUInt16Token((CastleUInt16Token)token, ref offset);
				break;
			case CastleTokenType.Int32:
				this.writeInt32Token((CastleInt32Token)token, ref offset);
				break;
			case CastleTokenType.UInt32:
				this.writeUInt32Token((CastleUInt32Token)token, ref offset);
				break;
			case CastleTokenType.Int64:
				this.writeInt64Token((CastleInt64Token)token, ref offset);
				break;
			case CastleTokenType.UInt64:
				this.writeUInt64Token((CastleUInt64Token)token, ref offset);
				break;
			case CastleTokenType.Half:
				this.writeHalfToken((CastleHalfToken)token, ref offset);
				break;
			case CastleTokenType.Single:
				this.writeSingleToken((CastleSingleToken)token, ref offset);
				break;
			case CastleTokenType.Double:
				this.writeDoubleToken((CastleDoubleToken)token, ref offset);
				break;
			case CastleTokenType.ByteArray:
				this.writeByteArrayToken((CastleByteArrayToken)token, ref offset);
				break;
			case CastleTokenType.SByteArray:
				this.writeSByteArrayToken((CastleSByteArrayToken)token, ref offset);
				break;
			case CastleTokenType.Int16Array:
				this.writeInt16ArrayToken((CastleInt16ArrayToken)token, ref offset);
				break;
			case CastleTokenType.UInt16Array:
				this.writeUInt16ArrayToken((CastleUInt16ArrayToken)token, ref offset);
				break;
			case CastleTokenType.Int32Array:
				this.writeInt32ArrayToken((CastleInt32ArrayToken)token, ref offset);
				break;
			case CastleTokenType.UInt32Array:
				this.writeUInt32ArrayToken((CastleUInt32ArrayToken)token, ref offset);
				break;
			case CastleTokenType.Int64Array:
				this.writeInt64ArrayToken((CastleInt64ArrayToken)token, ref offset);
				break;
			case CastleTokenType.UInt64Array:
				this.writeUInt64ArrayToken((CastleUInt64ArrayToken)token, ref offset);
				break;
			case CastleTokenType.HalfArray:
				this.writeHalfArrayToken((CastleHalfArrayToken)token, ref offset);
				break;
			case CastleTokenType.SingleArray:
				this.writeSingleArrayToken((CastleSingleArrayToken)token, ref offset);
				break;
			case CastleTokenType.DoubleArray:
				this.writeDoubleArrayToken((CastleDoubleArrayToken)token, ref offset);
				break;
			case CastleTokenType.DateTime:
				this.writeDateTimeToken((CastleDateTimeToken)token, ref offset);
				break;
			case CastleTokenType.Date:
				this.writeDateToken((CastleDateToken)token, ref offset);
				break;
			case CastleTokenType.Time:
				this.writeTimeToken((CastleTimeToken)token, ref offset);
				break;
			case CastleTokenType.String:
				this.writeStringToken((CastleStringToken)token, ref offset);
				break;
			case CastleTokenType.UTF16String:
				this.writeString16Token((CastleString16Token)token, ref offset);
				break;
			case CastleTokenType.Guid:
				this.writeGuidToken((CastleGuidToken)token, ref offset);
				break;
			case CastleTokenType.List:
				this.writeListToken((CastleListToken)token, ref offset);
				break;
			case CastleTokenType.Compound:
				this.writeCompoundToken((CastleCompoundToken)token, ref offset);
				break;
		}
	}

	private void writeByteToken(CastleByteToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(4);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> nameId = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(nameId, token.NameId);

		*(this.bufferPointer + offset + 3) = token.Value;

		offset += 3;
	}

	private void writeSByteToken(CastleSByteToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(4);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> nameId = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(nameId, token.NameId);

		*(this.bufferPointer + offset + 3) = (Byte)token.Value;

		offset += 3;
	}

	private void writeInt16Token(CastleInt16Token token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(5);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, token.Value);

		offset += 4;
	}

	private void writeUInt16Token(CastleUInt16Token token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(5);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.Value);

		offset += 4;
	}

	private void writeInt32Token(CastleInt32Token token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(7);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 4);
		BinaryPrimitives.WriteInt32LittleEndian(access, token.Value);

		offset += 6;
	}

	private void writeUInt32Token(CastleUInt32Token token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(7);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 4);
		BinaryPrimitives.WriteUInt32LittleEndian(access, token.Value);

		offset += 6;
	}

	private void writeInt64Token(CastleInt64Token token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(11);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 8);
		BinaryPrimitives.WriteInt64LittleEndian(access, token.Value);

		offset += 10;
	}

	private void writeUInt64Token(CastleUInt64Token token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(11);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 8);
		BinaryPrimitives.WriteUInt64LittleEndian(access, token.Value);

		offset += 10;
	}

	private void writeHalfToken(CastleHalfToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(5);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteHalfLittleEndian(access, token.Value);

		offset += 4;
	}

	private void writeSingleToken(CastleSingleToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(7);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 4);
		BinaryPrimitives.WriteSingleLittleEndian(access, token.Value);

		offset += 6;
	}

	private void writeDoubleToken(CastleDoubleToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(11);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 8);
		BinaryPrimitives.WriteDoubleLittleEndian(access, token.Value);

		offset += 10;
	}

	private void writeByteArrayToken(CastleByteArrayToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(5 + token.Count);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)token.Count);

		access = new(this.bufferPointer + offset + 4, token.Count);
		CollectionsMarshal.AsSpan(token.Children).CopyTo(access);

		offset += 4 + token.Count;
	}

	private void writeSByteArrayToken(CastleSByteArrayToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(5 + token.Count);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)token.Count);

		access = new(this.bufferPointer + offset + 4, token.Count);
		MemoryMarshal.Cast<SByte, Byte>(CollectionsMarshal.AsSpan(token.Children)).CopyTo(access);

		offset += 4 + token.Count;
	}

	private void writeInt16ArrayToken(CastleInt16ArrayToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(5 + token.Count * 2);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)token.Count);

		access = new(this.bufferPointer + offset + 4, token.Count);
		MemoryMarshal.Cast<Int16, Byte>(CollectionsMarshal.AsSpan(token.Children)).CopyTo(access);

		offset += 4 + token.Count * 2;
	}

	private void writeUInt16ArrayToken(CastleUInt16ArrayToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(5 + token.Count * 2);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)token.Count);

		access = new(this.bufferPointer + offset + 4, token.Count);
		MemoryMarshal.Cast<UInt16, Byte>(CollectionsMarshal.AsSpan(token.Children)).CopyTo(access);

		offset += 4 + token.Count * 2;
	}

	private void writeInt32ArrayToken(CastleInt32ArrayToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(5 + token.Count * 4);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)token.Count);

		access = new(this.bufferPointer + offset + 4, token.Count);
		MemoryMarshal.Cast<Int32, Byte>(CollectionsMarshal.AsSpan(token.Children)).CopyTo(access);

		offset += 4 + token.Count * 4;
	}

	private void writeUInt32ArrayToken(CastleUInt32ArrayToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(5 + token.Count * 4);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)token.Count);

		access = new(this.bufferPointer + offset + 4, token.Count);
		MemoryMarshal.Cast<UInt32, Byte>(CollectionsMarshal.AsSpan(token.Children)).CopyTo(access);

		offset += 4 + token.Count * 4;
	}

	private void writeInt64ArrayToken(CastleInt64ArrayToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(5 + token.Count * 8);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)token.Count);

		access = new(this.bufferPointer + offset + 4, token.Count);
		MemoryMarshal.Cast<Int64, Byte>(CollectionsMarshal.AsSpan(token.Children)).CopyTo(access);

		offset += 4 + token.Count * 8;
	}

	private void writeUInt64ArrayToken(CastleUInt64ArrayToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(5 + token.Count * 8);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)token.Count);

		access = new(this.bufferPointer + offset + 4, token.Count);
		MemoryMarshal.Cast<UInt64, Byte>(CollectionsMarshal.AsSpan(token.Children)).CopyTo(access);

		offset += 4 + token.Count * 8;
	}

	private void writeHalfArrayToken(CastleHalfArrayToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(5 + token.Count * 2);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)token.Count);

		access = new(this.bufferPointer + offset + 4, token.Count);
		MemoryMarshal.Cast<Half, Byte>(CollectionsMarshal.AsSpan(token.Children)).CopyTo(access);

		offset += 4 + token.Count * 2;
	}

	private void writeSingleArrayToken(CastleSingleArrayToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(5 + token.Count * 4);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)token.Count);

		access = new(this.bufferPointer + offset + 4, token.Count);
		MemoryMarshal.Cast<Single, Byte>(CollectionsMarshal.AsSpan(token.Children)).CopyTo(access);

		offset += 4 + token.Count * 4;
	}

	private void writeDoubleArrayToken(CastleDoubleArrayToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(5 + token.Count * 8);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)token.Count);

		access = new(this.bufferPointer + offset + 4, token.Count);
		MemoryMarshal.Cast<Double, Byte>(CollectionsMarshal.AsSpan(token.Children)).CopyTo(access);

		offset += 4 + token.Count * 8;
	}

	private void writeDateTimeToken(CastleDateTimeToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(13);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 8);
		BinaryPrimitives.WriteInt64LittleEndian(access, token.Value.Ticks);

		access = new(this.bufferPointer + offset + 10, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)token.Value.Offset.Minutes);

		offset += 12;
	}

	private void writeDateToken(CastleDateToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(7);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 4);
		BinaryPrimitives.WriteInt32LittleEndian(access, token.Value.DayNumber);

		offset += 6;
	}

	private void writeTimeToken(CastleTimeToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(11);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 8);
		BinaryPrimitives.WriteInt64LittleEndian(access, token.Value.Ticks);

		offset += 10;
	}

	private void writeStringToken(CastleStringToken token, ref Int32 offset)
	{
		Int32 byteCount = Encoding.UTF8.GetByteCount(token.Value);

		this.incrementBufferIfNecessary(3 + byteCount);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)byteCount);

		access = new(this.bufferPointer + offset + 2, byteCount);
		Encoding.UTF8.GetBytes(token.Value).AsSpan().CopyTo(access);

		offset += 2 + byteCount;
	}

	private void writeString16Token(CastleString16Token token, ref Int32 offset)
	{
		Int32 byteCount = Encoding.Unicode.GetByteCount(token.Value);

		this.incrementBufferIfNecessary(3 + byteCount);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)byteCount);

		access = new(this.bufferPointer + offset + 2, byteCount);
		Encoding.Unicode.GetBytes(token.Value).CopyTo(access);

		offset += 2 + byteCount;
	}

	private void writeGuidToken(CastleGuidToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(19);

		*(this.bufferPointer + offset + 1) = token.RefDeclarator;
		offset++;

		Span<Byte> access = new(this.bufferPointer + offset, 2);
		BinaryPrimitives.WriteUInt16LittleEndian(access, token.NameId);

		access = new(this.bufferPointer + offset + 2, 16);
		token.Value.ToByteArray().CopyTo(access);

		offset += 18;
	}

	private void writeListToken(CastleListToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(7);

		Int32 offsetSnapshot = offset + 2;
		*(this.bufferPointer + offset + 1) = CastleListToken.Declarator;

		Span<Byte> access = new(this.bufferPointer + offset + 5, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)token.Count);

		foreach(ICastleToken child in token.Children)
		{
			this.writeNamelessToken(token, ref offset);
		}

		Int32 listOffset = offset - offsetSnapshot;

		access = new(this.bufferPointer + offsetSnapshot, 4);
		BinaryPrimitives.WriteInt32LittleEndian(access, listOffset);
	}

	private void writeCompoundToken(CastleCompoundToken token, ref Int32 offset)
	{
		this.incrementBufferIfNecessary(7);

		Int32 offsetSnapshot = offset + 2;
		*(this.bufferPointer + offset + 1) = CastleCompoundToken.Declarator;

		Span<Byte> access = new(this.bufferPointer + offset + 5, 2);
		BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)token.Count);

		foreach(ICastleToken child in token.Children)
		{
			this.writeToken(token, ref offset);
		}

		Int32 listOffset = offset - offsetSnapshot;

		access = new(this.bufferPointer + offsetSnapshot, 4);
		BinaryPrimitives.WriteInt32LittleEndian(access, listOffset);
	}

	private void writeNamelessToken(ICastleToken token, ref Int32 offset)
	{
		Span<Byte> access;
		Int32 byteCount;

		switch((CastleTokenType)token.RefDeclarator)
		{
			case CastleTokenType.Byte:

				this.incrementBufferIfNecessary(1);

				*(this.bufferPointer + offset + 1) = ((CastleByteToken)token).Value;
				offset++;

				break;

			case CastleTokenType.SByte:

				this.incrementBufferIfNecessary(1);

				*(this.bufferPointer + offset + 1) = (Byte)((CastleSByteToken)token).Value;
				offset++;

				break;

			case CastleTokenType.Int16:

				this.incrementBufferIfNecessary(2);

				access = new(this.bufferPointer + offset, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, ((CastleInt16Token)token).Value);
				offset += 2;

				break;

			case CastleTokenType.UInt16:

				this.incrementBufferIfNecessary(2);

				access = new(this.bufferPointer + offset, 2);
				BinaryPrimitives.WriteUInt16LittleEndian(access, ((CastleUInt16Token)token).Value);
				offset += 2;

				break;

			case CastleTokenType.Int32:

				this.incrementBufferIfNecessary(4);

				access = new(this.bufferPointer + offset, 4);
				BinaryPrimitives.WriteInt32LittleEndian(access, ((CastleInt32Token)token).Value);
				offset += 4;

				break;

			case CastleTokenType.UInt32:

				this.incrementBufferIfNecessary(4);

				access = new(this.bufferPointer + offset, 4);
				BinaryPrimitives.WriteUInt32LittleEndian(access, ((CastleUInt32Token)token).Value);
				offset += 4;

				break;

			case CastleTokenType.Int64:

				this.incrementBufferIfNecessary(8);

				access = new(this.bufferPointer + offset, 8);
				BinaryPrimitives.WriteInt64LittleEndian(access, ((CastleInt64Token)token).Value);
				offset += 8;

				break;

			case CastleTokenType.UInt64:

				this.incrementBufferIfNecessary(8);

				access = new(this.bufferPointer + offset, 8);
				BinaryPrimitives.WriteUInt64LittleEndian(access, ((CastleUInt64Token)token).Value);
				offset += 8;

				break;

			case CastleTokenType.Half:

				this.incrementBufferIfNecessary(2);

				access = new(this.bufferPointer + offset, 2);
				BinaryPrimitives.WriteHalfLittleEndian(access, ((CastleHalfToken)token).Value);
				offset += 2;

				break;

			case CastleTokenType.Single:

				this.incrementBufferIfNecessary(4);

				access = new(this.bufferPointer + offset, 4);
				BinaryPrimitives.WriteSingleLittleEndian(access, ((CastleSingleToken)token).Value);
				offset += 4;

				break;

			case CastleTokenType.Double:

				this.incrementBufferIfNecessary(8);

				access = new(this.bufferPointer + offset, 8);
				BinaryPrimitives.WriteDoubleLittleEndian(access, ((CastleDoubleToken)token).Value);
				offset += 8;

				break;

			case CastleTokenType.ByteArray:

				CastleByteArrayToken cbat = (CastleByteArrayToken)token;
				this.incrementBufferIfNecessary(cbat.Count);

				access = new(this.bufferPointer + offset, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)cbat.Count);

				access = new(this.bufferPointer + offset + 2, cbat.Count);
				CollectionsMarshal.AsSpan(cbat.Children).CopyTo(access);
				offset += cbat.Count;

				break;

			case CastleTokenType.SByteArray:

				CastleSByteArrayToken csbat = (CastleSByteArrayToken)token;
				this.incrementBufferIfNecessary(csbat.Count);

				access = new(this.bufferPointer + offset, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)csbat.Count);

				access = new(this.bufferPointer + offset + 2, csbat.Count);
				MemoryMarshal.Cast<SByte, Byte>(CollectionsMarshal.AsSpan(csbat.Children)).CopyTo(access);
				offset += csbat.Count;

				break;

			case CastleTokenType.Int16Array:

				CastleInt16ArrayToken ci16at = (CastleInt16ArrayToken)token;
				this.incrementBufferIfNecessary(ci16at.Count * 2);

				access = new(this.bufferPointer + offset, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)ci16at.Count);

				access = new(this.bufferPointer + offset + 2, ci16at.Count);
				MemoryMarshal.Cast<Int16, Byte>(CollectionsMarshal.AsSpan(ci16at.Children)).CopyTo(access);
				offset += ci16at.Count * 2;

				break;

			case CastleTokenType.UInt16Array:

				CastleUInt16ArrayToken cui16at = (CastleUInt16ArrayToken)token;
				this.incrementBufferIfNecessary(cui16at.Count * 2);

				access = new(this.bufferPointer + offset, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)cui16at.Count);

				access = new(this.bufferPointer + offset + 2, cui16at.Count);
				MemoryMarshal.Cast<UInt16, Byte>(CollectionsMarshal.AsSpan(cui16at.Children)).CopyTo(access);
				offset += cui16at.Count * 2;

				break;

			case CastleTokenType.Int32Array:

				CastleInt32ArrayToken ci32at = (CastleInt32ArrayToken)token;
				this.incrementBufferIfNecessary(ci32at.Count * 4);

				access = new(this.bufferPointer + offset, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)ci32at.Count);

				access = new(this.bufferPointer + offset + 2, ci32at.Count);
				MemoryMarshal.Cast<Int32, Byte>(CollectionsMarshal.AsSpan(ci32at.Children)).CopyTo(access);
				offset += ci32at.Count * 4;

				break;

			case CastleTokenType.UInt32Array:

				CastleUInt32ArrayToken cui32at = (CastleUInt32ArrayToken)token;
				this.incrementBufferIfNecessary(cui32at.Count * 4);

				access = new(this.bufferPointer + offset, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)cui32at.Count);

				access = new(this.bufferPointer + offset + 2, cui32at.Count);
				MemoryMarshal.Cast<UInt32, Byte>(CollectionsMarshal.AsSpan(cui32at.Children)).CopyTo(access);
				offset += cui32at.Count * 4;

				break;

			case CastleTokenType.Int64Array:

				CastleInt64ArrayToken ci64at = (CastleInt64ArrayToken)token;
				this.incrementBufferIfNecessary(ci64at.Count * 8);

				access = new(this.bufferPointer + offset, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)ci64at.Count);

				access = new(this.bufferPointer + offset + 2, ci64at.Count);
				MemoryMarshal.Cast<Int64, Byte>(CollectionsMarshal.AsSpan(ci64at.Children)).CopyTo(access);
				offset += ci64at.Count * 8;

				break;

			case CastleTokenType.UInt64Array:

				CastleUInt64ArrayToken cui64at = (CastleUInt64ArrayToken)token;
				this.incrementBufferIfNecessary(cui64at.Count * 8);

				access = new(this.bufferPointer + offset, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)cui64at.Count);

				access = new(this.bufferPointer + offset + 2, cui64at.Count);
				MemoryMarshal.Cast<UInt64, Byte>(CollectionsMarshal.AsSpan(cui64at.Children)).CopyTo(access);
				offset += cui64at.Count * 8;

				break;

			case CastleTokenType.HalfArray:

				CastleHalfArrayToken chat = (CastleHalfArrayToken)token;
				this.incrementBufferIfNecessary(chat.Count * 2);

				access = new(this.bufferPointer + offset, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)chat.Count);

				access = new(this.bufferPointer + offset + 2, chat.Count);
				MemoryMarshal.Cast<Half, Byte>(CollectionsMarshal.AsSpan(chat.Children)).CopyTo(access);
				offset += chat.Count * 2;

				break;

			case CastleTokenType.SingleArray:

				CastleSingleArrayToken csat = (CastleSingleArrayToken)token;
				this.incrementBufferIfNecessary(csat.Count * 4);

				access = new(this.bufferPointer + offset, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)csat.Count);

				access = new(this.bufferPointer + offset + 2, csat.Count);
				MemoryMarshal.Cast<Single, Byte>(CollectionsMarshal.AsSpan(csat.Children)).CopyTo(access);
				offset += csat.Count * 4;

				break;

			case CastleTokenType.DoubleArray:

				CastleDoubleArrayToken cdat = (CastleDoubleArrayToken)token;
				this.incrementBufferIfNecessary(cdat.Count * 8);

				access = new(this.bufferPointer + offset, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)cdat.Count);

				access = new(this.bufferPointer + offset + 2, cdat.Count);
				MemoryMarshal.Cast<Double, Byte>(CollectionsMarshal.AsSpan(cdat.Children)).CopyTo(access);
				offset += cdat.Count * 8;

				break;

			case CastleTokenType.DateTime:

				CastleDateTimeToken cdtt = (CastleDateTimeToken)token;
				this.incrementBufferIfNecessary(11);

				*(this.bufferPointer + offset + 1) = token.RefDeclarator;
				offset++;

				access = new(this.bufferPointer + offset + 2, 8);
				BinaryPrimitives.WriteInt64LittleEndian(access, cdtt.Value.Ticks);

				access = new(this.bufferPointer + offset + 10, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)cdtt.Value.Offset.Minutes);
				offset += 10;

				break;

			case CastleTokenType.Date:

				CastleDateToken cdt = (CastleDateToken)token;
				this.incrementBufferIfNecessary(5);

				*(this.bufferPointer + offset + 1) = token.RefDeclarator;
				offset++;

				access = new(this.bufferPointer + offset + 2, 4);
				BinaryPrimitives.WriteInt32LittleEndian(access, cdt.Value.DayNumber);
				offset += 4;

				break;

			case CastleTokenType.Time:

				CastleTimeToken ctt = (CastleTimeToken)token;
				this.incrementBufferIfNecessary(9);

				*(this.bufferPointer + offset + 1) = token.RefDeclarator;
				offset++;

				access = new(this.bufferPointer + offset + 2, 8);
				BinaryPrimitives.WriteInt64LittleEndian(access, ctt.Value.Ticks);
				offset += 8;

				break;

			case CastleTokenType.String:

				CastleStringToken cst = (CastleStringToken)token;
				byteCount = Encoding.UTF8.GetByteCount(cst.Value);

				this.incrementBufferIfNecessary(1 + byteCount);

				*(this.bufferPointer + offset + 1) = token.RefDeclarator;
				offset++;

				access = new(this.bufferPointer + offset + 2, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)byteCount);

				access = new(this.bufferPointer + offset + 2, byteCount);
				Encoding.UTF8.GetBytes(cst.Value).AsSpan().CopyTo(access);
				offset += byteCount;

				break;

			case CastleTokenType.UTF16String:

				CastleStringToken cs16t = (CastleStringToken)token;
				byteCount = Encoding.Unicode.GetByteCount(cs16t.Value);

				this.incrementBufferIfNecessary(1 + byteCount);

				*(this.bufferPointer + offset + 1) = token.RefDeclarator;
				offset++;

				access = new(this.bufferPointer + offset + 2, 2);
				BinaryPrimitives.WriteInt16LittleEndian(access, (Int16)byteCount);

				access = new(this.bufferPointer + offset + 2, byteCount);
				Encoding.Unicode.GetBytes(cs16t.Value).AsSpan().CopyTo(access);
				offset += byteCount;

				break;

			case CastleTokenType.Guid:

				CastleGuidToken cgt = (CastleGuidToken)token;
				this.incrementBufferIfNecessary(17);

				*(this.bufferPointer + offset + 1) = token.RefDeclarator;
				offset++;

				access = new(this.bufferPointer + offset + 2, 16);
				cgt.Value.ToByteArray().CopyTo(access);
				offset += 17;

				break;

			case CastleTokenType.List: throw new ArgumentException("List token cannot contain a List as child token");

			case CastleTokenType.Compound:

				CastleCompoundToken cct = (CastleCompoundToken)token;
				this.incrementBufferIfNecessary(7);

				Int32 offsetSnapshot = offset + 2;
				*(this.bufferPointer + offset + 1) = CastleCompoundToken.Declarator;

				foreach(ICastleToken child in cct.Children)
				{
					this.writeToken(token, ref offset);
				}

				Int32 listOffset = offset - offsetSnapshot;

				access = new(this.bufferPointer + offsetSnapshot, 4);
				BinaryPrimitives.WriteInt32LittleEndian(access, listOffset);

				break;
		}
	}
}
