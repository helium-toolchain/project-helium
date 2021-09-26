namespace Helium.Network.Api.Vanilla;

using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Helium.Api.Mojang;

/// <summary>
/// Network	stream for minecraft packet serialization. Implements read/write methods for all basic network types required by the protocol.
/// <para>
///		Packets should expose a method for this class to serialize.
/// </para>
/// <para>
///		Entities intended to be passed over the mojang protocol should expose an extension method for this class to serialize.
/// </para>
/// </summary>
public class PacketStream : Stream
{
	/// <summary>
	/// Base memory stream.
	/// </summary>
	public MemoryStream BaseStream { get; protected set; }

	/// <summary>
	/// Thread lock ensuring the stream can never be accessed from two distinct threads.
	/// </summary>
	public SemaphoreSlim SemaphoreLock { get; protected set; } = new(1, 1);

	/// <summary>
	/// Gets whether this stream supports seeking.
	/// </summary>
	public override Boolean CanSeek => BaseStream.CanSeek;

	/// <summary>
	/// Gets whether this stream supports reading.
	/// </summary>
	public override Boolean CanRead => BaseStream.CanRead;

	/// <summary>
	/// Gets whether this stream can time out.
	/// </summary>
	public override Boolean CanTimeout => BaseStream.CanTimeout;

	/// <summary>
	/// Gets whether this stream supports writing.
	/// </summary>
	public override Boolean CanWrite => BaseStream.CanWrite;

	/// <summary>
	/// Gets the length of this stream in bytes.
	/// </summary>
	public override Int64 Length => BaseStream.Length;

	/// <summary>
	/// Gets or sets the position within this current stream.
	/// </summary>
	public override Int64 Position
	{
		get => BaseStream.Position;
		set => BaseStream.Position = value;
	}

	#region Stream implementation

	/// <summary>
	/// Flushes this stream in its entirety.
	/// </summary>
	public override void Flush()
	{
		BaseStream.Flush();
	}

	/// <summary>
	/// Asynchronously flushes this stream.
	/// </summary>
	/// <param name="token">Optional CancellationToken for this operation.</param>
	public override async Task FlushAsync(CancellationToken token = default)
	{
		await BaseStream.FlushAsync(token);
	}

	/// <summary>
	/// Asynchronously disposes of this stream.
	/// </summary>
	public override ValueTask DisposeAsync()
	{
		GC.SuppressFinalize(this);
		return base.DisposeAsync();
	}

	/// <summary>
	/// Writes a sequence of bytes to this stream and advances the pointer by the amount of bytes written.
	/// </summary>
	public override void Write(Byte[] buffer, Int32 offset, Int32 count)
	{
		BaseStream.Write(buffer, offset, count);
	}

	/// <summary>
	/// Writes a sequence of bytes to this stream and advances the pointer by the amount of bytes written.
	/// </summary>
	public override void Write(ReadOnlySpan<Byte> buffer)
	{
		BaseStream.Write(buffer);
	}

	/// <inheritdoc/>
	public override void WriteByte(Byte value)
	{
		BaseStream.WriteByte(value);
	}

	/// <inheritdoc/>
	public override void Close()
	{
		BaseStream.Close();
		base.Close();
	}

	/// <summary>
	/// Sets the length of the current stream.
	/// </summary>
	public override void SetLength(Int64 value)
	{
		BaseStream.SetLength(value);
	}

	/// <summary>
	/// Sets the position within the current stream.
	/// </summary>
	public override Int64 Seek(Int64 offset, SeekOrigin origin)
	{
		return BaseStream.Seek(offset, origin);
	}

	/// <summary>
	/// Reads a specified amount of bytes from the current stream and advances the stream by the amount.
	/// </summary>
	public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
	{
		return BaseStream.Read(buffer, offset, count);
	}

	/// <summary>
	/// Reads a sequence of bytes from the current stream and advances the stream by the amount.
	/// </summary>
	public override Int32 Read(Span<Byte> buffer)
	{
		return BaseStream.Read(buffer);
	}

	/// <summary>
	/// Reads one singular byte from the stream.
	/// </summary>
	public override Int32 ReadByte()
	{
		return BaseStream.ReadByte();
	}

	#endregion

	#region Debug functionality

#if DEBUG
	/// <summary>
	/// Writes the contents of the current stream to a file.
	/// </summary>
	/// <param name="path">File path. The directory must exist. The file name should be passed without extension and will be appended a guid to.</param>
	public void Dump(String path)
	{
		FileStream stream = File.Create($"{path}-{Guid.NewGuid()}.hdump");

		BaseStream.CopyTo(stream);
		stream.Position = 0;

		stream.Flush();
		stream.Close();
	}

	/// <summary>
	/// Writes the contents of the current stream to a file asynchronously.
	/// </summary>
	/// <param name="path">File path. The directory must exist. The file name should be passed without extension and will be appended a guid to.</param>
	public async Task DumpAsync(String path)
	{
		FileStream stream = File.Create($"{path}-{Guid.NewGuid()}.hdump");

		BaseStream.CopyTo(stream);
		stream.Position = 0;

		await stream.FlushAsync();
		stream.Close();
	}
#endif

	#endregion

	#region Reading Primitives

	/// <summary>
	/// Reads one unsigned byte from the current stream.
	/// </summary>
	public Byte ReadUnsignedByte()
	{
		Span<Byte> buffer = stackalloc Byte[0];
		BaseStream.Read(buffer);
		return buffer[0];
	}

	/// <summary>
	/// Reads one unsigned byte from the current stream asynchronously.
	/// </summary>
	public async ValueTask<Byte> ReadUnsignedByteAsync()
	{
		Byte[] buffer = new Byte[1];
		await BaseStream.ReadAsync(buffer);
		return buffer[0];
	}

	/// <summary>
	/// Reads one signed byte from the current stream.
	/// </summary>
	public SByte ReadSignedByte()
	{
		return (SByte)this.ReadUnsignedByte();
	}

	/// <summary>
	/// Reads one signed byte from the current stream asynchronously.
	/// </summary>
	public async ValueTask<SByte> ReadSignedByteAsync()
	{
		return (SByte)await this.ReadUnsignedByteAsync();
	}

	/// <summary>
	/// Reads one boolean from the current stream.
	/// </summary>
	public Boolean ReadBoolean()
	{
		return this.ReadUnsignedByte() == 0x01;
	}

	/// <summary>
	/// Reads one boolean from the current stream asynchronously.
	/// </summary>
	public async ValueTask<Boolean> ReadBooleanAsync()
	{
		return await this.ReadUnsignedByteAsync() == 0x01; 
	}

	/// <summary>
	/// Reads an Int16 from the current stream.
	/// </summary>
	public Int16 ReadInt16()
	{
		Span<Byte> buffer = stackalloc Byte[2];
		BaseStream.Read(buffer);
		return BinaryPrimitives.ReadInt16BigEndian(buffer);
	}

	/// <summary>
	/// Reads an Int16 from the current stream asynchronously.
	/// </summary>
	public async ValueTask<Int16> ReadInt16Async()
	{
		Byte[] buffer = new Byte[2];
		await BaseStream.ReadAsync(buffer);
		return BinaryPrimitives.ReadInt16BigEndian(buffer);
	}

	/// <summary>
	/// Reads an UInt16 from the current stream.
	/// </summary>
	public UInt16 ReadUInt16()
	{
		Span<Byte> buffer = stackalloc Byte[2];
		BaseStream.Read(buffer);
		return BinaryPrimitives.ReadUInt16BigEndian(buffer);
	}

	/// <summary>
	/// Reads an UInt16 from the current stream asynchronously.
	/// </summary>
	public async ValueTask<UInt16> ReadUInt16Async()
	{
		Byte[] buffer = new Byte[2];
		await BaseStream.ReadAsync(buffer);
		return BinaryPrimitives.ReadUInt16BigEndian(buffer);
	}

	/// <summary>
	/// Reads an Int32 from the current stream.
	/// </summary>
	public Int32 ReadInt32()
	{
		Span<Byte> buffer = stackalloc Byte[4];
		BaseStream.Read(buffer);
		return BinaryPrimitives.ReadInt32BigEndian(buffer);
	}
	
	/// <summary>
	/// Reads an Int32 from the current stream asynchronously.
	/// </summary>
	public async ValueTask<Int32> ReadInt32Async()
	{
		Byte[] buffer = new Byte[4];
		await BaseStream.ReadAsync(buffer);
		return BinaryPrimitives.ReadInt32BigEndian(buffer);
	}

	/// <summary>
	/// Reads an UInt32 from the current stream.
	/// </summary>
	public UInt32 ReadUInt32()
	{
		Span<Byte> buffer = stackalloc Byte[4];
		BaseStream.Read(buffer);
		return BinaryPrimitives.ReadUInt32BigEndian(buffer);
	}

	/// <summary>
	/// Reads an UInt32 from the current stream asynchronously.
	/// </summary>
	public async ValueTask<UInt32> ReadUInt32Async()
	{
		Byte[] buffer = new Byte[4];
		await BaseStream.ReadAsync(buffer);
		return BinaryPrimitives.ReadUInt32BigEndian(buffer);
	}

	/// <summary>
	/// Reads an Int64 from the current stream.
	/// </summary>
	public Int64 ReadInt64()
	{
		Span<Byte> buffer = stackalloc Byte[8];
		BaseStream.Read(buffer);
		return BinaryPrimitives.ReadInt64BigEndian(buffer);
	}

	/// <summary>
	/// Reads an Int64 from the current stream asynchronously.
	/// </summary>
	public async ValueTask<Int64> ReadInt64Async()
	{
		Byte[] buffer = new Byte[8];
		await BaseStream.ReadAsync(buffer);
		return BinaryPrimitives.ReadInt64BigEndian(buffer);
	}

	/// <summary>
	/// Reads an UInt64 from the current stream.
	/// </summary>
	public UInt64 ReadUInt64()
	{
		Span<Byte> buffer = stackalloc Byte[8];
		BaseStream.Read(buffer);
		return BinaryPrimitives.ReadUInt64BigEndian(buffer);
	}

	/// <summary>
	/// Reads an UInt64 from the current stream asynchronously.
	/// </summary>
	public async ValueTask<UInt64> ReadUInt64Async()
	{
		Byte[] buffer = new Byte[8];
		await BaseStream.ReadAsync(buffer);
		return BinaryPrimitives.ReadUInt64BigEndian(buffer);
	}

	/// <summary>
	/// Reads a <see cref="VarInt"/> from the current stream.
	/// </summary>
	public VarInt ReadVarInt()
	{
		VarInt val = new();
		val.Read(BaseStream);
		return val;
	}

	/// <summary>
	/// Reads a <see cref="VarInt"/> from the current stream, wrapped into an awaitable.
	/// </summary>
	public ValueTask<VarInt> ReadVarIntAsync()
	{
		return ValueTask.FromResult(this.ReadVarInt());
	}

	/// <summary>
	/// Reads a <see cref="VarLong"/> from the current stream.
	/// </summary>
	public VarLong ReadVarLong()
	{
		VarLong val = new();
		val.Read(BaseStream);
		return val;
	}

	/// <summary>
	/// Reads a <see cref="VarLong"/> from the current stream, wrapped into an awaitable.
	/// </summary>
	public ValueTask<VarLong> ReadVarLongAsync()
	{
		return ValueTask.FromResult(this.ReadVarLong());
	}

	/// <summary>
	/// Reads a Single-precision floating point number from the current stream.
	/// </summary>
	public Single ReadSingle()
	{
		Span<Byte> buffer = stackalloc Byte[4];
		BaseStream.Read(buffer);
		return BinaryPrimitives.ReadSingleBigEndian(buffer);
	}

	/// <summary>
	/// Reads a Single-precision floating point number from the current stream asynchronously.
	/// </summary>
	public async ValueTask<Single> ReadSingleAsync()
	{
		Byte[] buffer = new Byte[4];
		await BaseStream.ReadAsync(buffer);
		return BinaryPrimitives.ReadSingleBigEndian(buffer);
	}

	/// <summary>
	/// Reads a Double-precision floating point number from the current stream.
	/// </summary>
	public Double ReadDouble()
	{
		Span<Byte> buffer = stackalloc Byte[8];
		BaseStream.Read(buffer);
		return BinaryPrimitives.ReadDoubleBigEndian(buffer);
	}

	/// <summary>
	/// Reads a Double-precision floating point number from the current stream asynchronously.
	/// </summary>
	public async ValueTask<Double> ReadDoubleAsync()
	{
		Byte[] buffer = new Byte[8];
		await BaseStream.ReadAsync(buffer);
		return BinaryPrimitives.ReadDoubleBigEndian(buffer);
	}

	/// <summary>
	/// Reads a Decimal floating-point number from the current stream.
	/// </summary>
	public Decimal ReadDecimal()
	{
		return (Decimal)this.ReadDouble();
	}

	/// <summary>
	/// Reads a Decimal floating-point number from the current stream.
	/// </summary>
	public async ValueTask<Decimal> ReadDecimalAsync()
	{
		return (Decimal)await this.ReadDoubleAsync();
	}

	/// <summary>
	/// Reads a String from the current stream.
	/// </summary>
	/// <param name="maxLength">Maximum string length. This should never exceed <see cref="Int16.MaxValue"/>, outside of Chat packets.</param>
	/// <exception cref="ArgumentException"/>
	public String ReadString(Int32 maxLength = Int16.MaxValue)
	{
		VarInt length = this.ReadVarInt();

		if(length > maxLength)
		{
			throw new ArgumentException($"Specified length of {maxLength} was lower than specified protocol length, aborting stream reading operation.");
		}

		Byte[] buffer = new Byte[length];
		this.Read(buffer, 0, length);

		return Encoding.UTF8.GetString(buffer);
	}

	/// <summary>
	/// Reads a String from the current stream asynchronously.
	/// </summary>
	/// <param name="maxLength">Maximum string length. This should never exceed <see cref="Int16.MaxValue"/>, outside of Chat packets.</param>
	/// <exception cref="ArgumentException"/>
	public async Task<String> ReadStringAsync(Int32 maxLength = Int16.MaxValue)
	{
		VarInt length = await this.ReadVarIntAsync();

		if (length > maxLength)
		{
			throw new ArgumentException($"Specified length of {maxLength} was lower than specified protocol length, aborting stream reading operation.");
		}

		Byte[] buffer = new Byte[length];
		await this.ReadAsync(buffer);

		return Encoding.UTF8.GetString(buffer);
	}

	#endregion

	#region Reading complex types

	/* Complex types as listed at https://wiki.vg/Protocol#Data_types. 
	 * 
	 * EntityMetadata should be handled separately, via extension methods, as they vary from protocol version to protocol version
	 * SlotData should be handled separately in some way, shape or form.
	 * NBT should be handled separately in some way, shape or form. ReadString should suffice here
	 * Position, see Helium.Api.Mojang.Position & Helium.Api.Moang.OldPosition
	 * Angle - unsigned Byte
	 * UUID - convert to System.Guid
	 * Array of X
	 * X Enum
	 * Byte Array
	*/

	/// <summary>
	/// Reads a <see cref="Helium.Api.Mojang.Position"/> from the current stream.
	/// </summary>
	public Position ReadPosition()
	{
		Span<Byte> val = stackalloc Byte[8];
		BaseStream.Read(val);
		return MemoryMarshal.Cast<Byte, Position>(val)[0];
	}

	/// <summary>
	/// Reads a <see cref="Helium.Api.Mojang.Position"/> from the current stream asynchronously.
	/// </summary>
	public async ValueTask<Position> ReadPositionAsync()
	{
		Byte[] val = new Byte[8];
		await BaseStream.ReadAsync(val);
		return MemoryMarshal.Cast<Byte, Position>(val)[0];
	}

	/// <summary>
	/// Reads a <see cref="OldPosition"/> from the current stream.
	/// </summary>
	public OldPosition ReadOldPosition()
	{
		Span<Byte> val = stackalloc Byte[8];
		BaseStream.Read(val);
		return MemoryMarshal.Cast<Byte, OldPosition>(val)[0];
	}

	/// <summary>
	/// Reads a <see cref="OldPosition"/> from the current stream asynchronously.
	/// </summary>
	public async ValueTask<OldPosition> ReadOldPositionAsync()
	{
		Byte[] val = new Byte[8];
		await BaseStream.ReadAsync(val);
		return MemoryMarshal.Cast<Byte, OldPosition>(val)[0];
	}

	/// <summary>
	/// Reads a Guid from the current stream; referred to by Mojang as UUID.
	/// </summary>
	public Guid ReadGuid()
	{
		Span<Byte> buffer = stackalloc Byte[16];
		BaseStream.Read(buffer);
		return new(buffer);
	}

	/// <summary>
	/// Reads a Guid from the current stream asynchronously; referred to by Mojang as UUID.
	/// </summary>
	public async ValueTask<Guid> ReadGuidAsync()
	{
		Byte[] buffer = new Byte[16];
		await BaseStream.ReadAsync(buffer);
		return new(buffer);
	}

	/// <summary>
	/// Reads a Byte array from the current stream.
	/// </summary>
	/// <param name="limit">Array length. This should be specified by preceding packet data or the context.</param>
	public Byte[] ReadByteArray(Int32 limit)
	{
		Byte[] buffer = new Byte[limit];
		BaseStream.Read(buffer);
		return buffer;
	}

	/// <summary>
	/// Reads an Int16 array from the current stream.
	/// </summary>
	/// <param name="limit">Array length. This should be specified by preceding packet data or the context.</param>
	public Int16[] ReadInt16Array(Int32 limit)
	{
		Byte[] buffer = new Byte[limit * 2];
		BaseStream.Read(buffer);
		return MemoryMarshal.Cast<Byte, Int16>(buffer).ToArray();
	}

	/// <summary>
	/// Reads an Int32 array from the current stream.
	/// </summary>
	/// <param name="limit">Array length. This should be specified by preceding packet data or the context.</param>
	public Int32[] ReadInt32Array(Int32 limit)
	{
		Byte[] buffer = new Byte[limit * 4];
		BaseStream.Read(buffer);
		return MemoryMarshal.Cast<Byte, Int32>(buffer).ToArray();
	}

	/// <summary>
	/// Reads an Int64 array from the current stream.
	/// </summary>
	/// <param name="limit">Array length. This should be specified by preceding packet data or the context.</param>
	public Int64[] ReadInt64Array(Int64 limit)
	{
		Byte[] buffer = new Byte[limit * 8];
		BaseStream.Read(buffer);
		return MemoryMarshal.Cast<Byte, Int64>(buffer).ToArray();
	}

	/// <summary>
	/// Reads an angle from the current stream.
	/// </summary>
	public Single ReadAngle()
	{
		return this.ReadByte() / 256 * 360;
	}

	/// <summary>
	/// Reads an angle from the current stream asynchronously.
	/// </summary>
	public async ValueTask<Single> ReadAngleAsync()
	{
		return await this.ReadUnsignedByteAsync() / 256 * 360;
	}

	/// <summary>
	/// Reads a raw string of specified length from the current stream.
	/// </summary>
	public String ReadRawString(Int32 length)
	{
		Span<Byte> buffer = new Byte[length];
		BaseStream.Read(buffer);
		return Encoding.UTF8.GetString(buffer);
	}

	/// <summary>
	/// Reads a raw string of specified length from the current stream asynchronously.
	/// </summary>
	public async Task<String> ReadRawStringAsync(Int32 length)
	{
		Byte[] buffer = new Byte[length];
		await BaseStream.ReadAsync(buffer);
		return Encoding.UTF8.GetString(buffer);
	}

	#endregion

	#region Writing primitives

	/// <summary>
	/// Writes an unsigned byte to the current stream.
	/// </summary>
	public void WriteUnsignedByte(Byte val)
	{
		BaseStream.WriteByte(val);
	}

	/// <summary>
	/// Writes an unsigned byte to the current stream asynchronously.
	/// </summary>
	public ValueTask WriteUnsignedByteAsync(Byte val)
	{
		BaseStream.WriteByte(val);
		return ValueTask.CompletedTask;
	}

	/// <summary>
	/// Writes a signed byte to the current stream.
	/// </summary>
	public void WriteSignedByte(SByte val)
	{
		BaseStream.WriteByte((Byte)val);
	}

	/// <summary>
	/// Writes a signed byte to the current stream asynchronously.
	/// </summary>
	public ValueTask WriteSignedByteAsync(SByte val)
	{
		BaseStream.WriteByte((Byte)val);
		return ValueTask.CompletedTask;
	}

	/// <summary>
	/// Writes an Int16 to the current stream.
	/// </summary>
	public void WriteInt16(Int16 val)
	{
		Span<Byte> buffer = stackalloc Byte[2];
		BinaryPrimitives.WriteInt16BigEndian(buffer, val);
		BaseStream.Write(buffer);
	}

	/// <summary>
	/// Writes an Int16 to the current stream asynchronously.
	/// </summary>
	public async ValueTask WriteInt16Async(Int16 val)
	{
		Byte[] buffer = new Byte[2];
		BinaryPrimitives.WriteInt16BigEndian(buffer, val);
		await BaseStream.WriteAsync(buffer);
	}

	/// <summary>
	/// Writes an UInt16 to the current stream.
	/// </summary>
	public void WriteUInt16(UInt16 val)
	{
		Span<Byte> buffer = stackalloc Byte[2];
		BinaryPrimitives.WriteUInt16BigEndian(buffer, val);
		BaseStream.Write(buffer);
	}

	/// <summary>
	/// Writes an UInt16 to the current stream asynchronously.
	/// </summary>
	public async ValueTask WriteUInt16Async(UInt16 val)
	{
		Byte[] buffer = new Byte[2];
		BinaryPrimitives.WriteUInt16BigEndian(buffer, val);
		await BaseStream.WriteAsync(buffer);
	}

	/// <summary>
	/// Writes an Int32 to the current stream.
	/// </summary>
	public void WriteInt32(Int32 val)
	{
		Span<Byte> buffer = stackalloc Byte[4];
		BinaryPrimitives.WriteInt32BigEndian(buffer, val);
		BaseStream.Write(buffer);
	}

	/// <summary>
	/// Writes an Int32 to the current stream asynchronously.
	/// </summary>
	public async ValueTask WriteInt32Async(Int32 val)
	{
		Byte[] buffer = new Byte[4];
		BinaryPrimitives.WriteInt32BigEndian(buffer, val);
		await BaseStream.WriteAsync(buffer);
	}

	/// <summary>
	/// Writes an UInt32 to the current stream.
	/// </summary>
	public void WriteUInt32(UInt32 val)
	{
		Span<Byte> buffer = stackalloc Byte[4];
		BinaryPrimitives.WriteUInt32BigEndian(buffer, val);
		BaseStream.Write(buffer);
	}

	/// <summary>
	/// Writes an UInt32 to the current stream asynchronously.
	/// </summary>
	public async ValueTask WriteUInt32Async(UInt32 val)
	{
		Byte[] buffer = new Byte[4];
		BinaryPrimitives.WriteUInt32BigEndian(buffer, val);
		await BaseStream.WriteAsync(buffer);
	}

	/// <summary>
	/// Writes an Int64 to the current stream.
	/// </summary>
	public void WriteInt64(Int64 val)
	{
		Span<Byte> buffer = stackalloc Byte[8];
		BinaryPrimitives.WriteInt64BigEndian(buffer, val);
		BaseStream.Write(buffer);
	}

	/// <summary>
	/// Writes an Int64 to the current stream asynchronously.
	/// </summary>
	public async ValueTask WriteInt64Async(Int64 val)
	{
		Byte[] buffer = new Byte[8];
		BinaryPrimitives.WriteInt64BigEndian(buffer, val);
		await BaseStream.WriteAsync(buffer);
	}

	/// <summary>
	/// Writes an UInt64 to the current stream.
	/// </summary>
	public void WriteUInt64(UInt64 val)
	{
		Span<Byte> buffer = stackalloc Byte[8];
		BinaryPrimitives.WriteUInt64BigEndian(buffer, val);
		BaseStream.Write(buffer);
	}

	/// <summary>
	/// Writes an UInt64 to the current stream asynchronously.
	/// </summary>
	public async ValueTask WriteUInt64Async(UInt64 val)
	{
		Byte[] buffer = new Byte[8];
		BinaryPrimitives.WriteUInt64BigEndian(buffer, val);
		await BaseStream.WriteAsync(buffer);
	}

	/// <summary>
	/// Writes a Single-precision floating point number to the current stream.
	/// </summary>
	public void WriteSingle(Single val)
	{
		Span<Byte> buffer = stackalloc Byte[4];
		BinaryPrimitives.WriteSingleBigEndian(buffer, val);
		BaseStream.Write(buffer);
	}

	/// <summary>
	/// Writes a Single-precision floating point number to the current stream asynchronously.
	/// </summary>
	public async ValueTask WriteSingleAsync(Single val)
	{
		Byte[] buffer = new Byte[4];
		BinaryPrimitives.WriteSingleBigEndian(buffer, val);
		await BaseStream.WriteAsync(buffer);
	}

	/// <summary>
	/// Writes a Double-precision floating point number to the current stream.
	/// </summary>
	public void WriteDouble(Double val)
	{
		Span<Byte> buffer = stackalloc Byte[8];
		BinaryPrimitives.WriteDoubleBigEndian(buffer, val);
		BaseStream.Write(buffer);
	}

	/// <summary>
	/// Writes a Double-precision floating point number to the current stream asynchronously.
	/// </summary>
	public async ValueTask WriteDoubleAsync(Double val)
	{
		Byte[] buffer = new Byte[8];
		BinaryPrimitives.WriteDoubleBigEndian(buffer, val);
		await BaseStream.WriteAsync(buffer);
	}

	/// <summary>
	/// Writes a <see cref="VarInt"/> to the current stream.
	/// </summary>
	public void WriteVarInt(VarInt val)
	{
		val.Write(BaseStream);
	}

	/// <summary>
	/// Writes a <see cref="VarInt"/> to the current stream.
	/// </summary>
	public ValueTask WriteVarIntAsync(VarInt val)
	{
		val.Write(BaseStream);
		return ValueTask.CompletedTask;
	}

	/// <summary>
	/// Writes a <see cref="VarLong"/> to the current stream.
	/// </summary>
	public void WriteVarLong(VarLong val)
	{
		val.Write(BaseStream);
	}

	/// <summary>
	/// Writes a <see cref="VarLong"/> to the current stream.
	/// </summary>
	public ValueTask WriteVarLongAsync(VarLong val)
	{
		val.Write(BaseStream);
		return ValueTask.CompletedTask;
	}

	/// <summary>
	/// Writes a String to the current stream, prefixed with its length.
	/// </summary>
	public void WriteString(String val)
	{
		VarInt length = val.Length;
		length.Write(BaseStream);

		Span<Byte> buffer = Encoding.UTF8.GetBytes(val);
		BaseStream.Write(buffer);
	}

	/// <summary>
	/// Writes a String to the current stream asynchronously, prefixed with its length.
	/// </summary>
	public async Task WriteStringAsync(String val)
	{
		VarInt length = val.Length;
		length.Write(BaseStream);

		Byte[] buffer = Encoding.UTF8.GetBytes(val);
		await BaseStream.WriteAsync(buffer);
	}

	#endregion

	#region Writing complex types

	/// <summary>
	/// Writes a <see cref="Helium.Api.Mojang.Position"/> to the current stream.
	/// </summary>
	public void WritePosition(Position val)
	{
		Span<Byte> buffer = MemoryMarshal.Cast<Position, Byte>(MemoryMarshal.CreateSpan(ref val, 1));
		BaseStream.Write(buffer);
	}

	/// <summary>
	/// Writes a <see cref="Helium.Api.Mojang.Position"/> to the current stream.
	/// </summary>
	public async ValueTask WritePositionAsync(Position val)
	{
		Byte[] buffer = MemoryMarshal.Cast<Position, Byte>(MemoryMarshal.CreateSpan(ref val, 1)).ToArray();
		await BaseStream.WriteAsync(buffer);
	}

	/// <summary>
	/// Writes a <see cref="OldPosition"/> to the current stream.
	/// </summary>
	public void WriteOldPosition(OldPosition val)
	{
		Span<Byte> buffer = MemoryMarshal.Cast<OldPosition, Byte>(MemoryMarshal.CreateSpan(ref val, 1));
		BaseStream.Write(buffer);
	}

	/// <summary>
	/// Writes a <see cref="OldPosition"/> to the current stream.
	/// </summary>
	public async ValueTask WriteOldPositionAsync(OldPosition val)
	{
		Byte[] buffer = MemoryMarshal.Cast<OldPosition, Byte>(MemoryMarshal.CreateSpan(ref val, 1)).ToArray();
		await BaseStream.WriteAsync(buffer);
	}

	/// <summary>
	/// Writes a Guid to the current stream.
	/// </summary>
	public void WriteGuid(Guid val)
	{
		BaseStream.Write(val.ToByteArray());
	}

	/// <summary>
	/// Writes a Guid to the current stream asynchronously.
	/// </summary>
	public async ValueTask WriteGuidAsync(Guid val)
	{
		await BaseStream.WriteAsync(val.ToByteArray());
	}

	/// <summary>
	/// Writes an angle to the current stream.
	/// </summary>
	public void WriteAngle(Single val)
	{
		BaseStream.WriteByte((Byte)(val / 360 * 256));
	}

	/// <summary>
	/// Writes an angle to the current stream asynchronously.
	/// </summary>
	public async ValueTask WriteAngleAsync(Single val)
	{
		await BaseStream.WriteAsync(new Byte[] { (Byte)(val / 360 * 256) });
	}

	/// <summary>
	/// Writes a byte array to the current stream.
	/// </summary>
	public void WriteByteArray(Byte[] val)
	{
		BaseStream.Write(val);
	}

	/// <summary>
	/// Writes a byte array to the current stream asynchronously.
	/// </summary>
	public async Task WriteByteArrayAsync(Byte[] val)
	{
		await BaseStream.WriteAsync(val);
	}

	/// <summary>
	/// Writes an Int16 array to the current stream.
	/// </summary>
	public void WriteInt16Array(Int16[] val)
	{
		BaseStream.Write(MemoryMarshal.Cast<Int16, Byte>(val.AsSpan()));
	}

	/// <summary>
	/// Writes an Int16 array to the current stream asynchronously.
	/// </summary>
	public Task WriteInt16ArrayAsync(Int16[] val)
	{
		BaseStream.Write(MemoryMarshal.Cast<Int16, Byte>(val.AsSpan()));
		return Task.CompletedTask;
	}

	/// <summary>
	/// Writes an Int32 array to the current stream.
	/// </summary>
	public void WriteInt32Array(Int32[] val)
	{
		BaseStream.Write(MemoryMarshal.Cast<Int32, Byte>(val.AsSpan()));
	}

	/// <summary>
	/// Writes an Int32 array to the current stream asynchronously.
	/// </summary>
	public Task WriteInt32ArrayAsync(Int32[] val)
	{
		BaseStream.Write(MemoryMarshal.Cast<Int32, Byte>(val.AsSpan()));
		return Task.CompletedTask;
	}

	/// <summary>
	/// Writes an Int64 array to the current stream.
	/// </summary>
	public void WriteInt64Array(Int64[] val)
	{
		BaseStream.Write(MemoryMarshal.Cast<Int64, Byte>(val.AsSpan()));
	}

	/// <summary>
	/// Writes an Int64 array to the current stream asynchronously.
	/// </summary>
	public Task WriteInt64ArrayAsync(Int64[] val)
	{
		BaseStream.Write(MemoryMarshal.Cast<Int64, Byte>(val.AsSpan()));
		return Task.CompletedTask;
	}

	/// <summary>
	/// Writes a Single array to the current stream.
	/// </summary>
	public void WriteSingleArray(Single[] val)
	{
		BaseStream.Write(MemoryMarshal.Cast<Single, Byte>(val.AsSpan()));
	}

	/// <summary>
	/// Writes a Single array to the current stream asynchronously.
	/// </summary>
	public Task WriteSingleArrayAsync(Single[] val)
	{
		BaseStream.Write(MemoryMarshal.Cast<Single, Byte>(val.AsSpan()));
		return Task.CompletedTask;
	}

	/// <summary>
	/// Writes a Double array to the current stream.
	/// </summary>
	public void WriteDoubleArray(Double[] val)
	{
		BaseStream.Write(MemoryMarshal.Cast<Double, Byte>(val.AsSpan()));
	}

	/// <summary>
	/// Writes a Double array to the current stream asynchronously.
	/// </summary>
	public Task WriteDoubleArrayAsync(Double[] val)
	{
		BaseStream.Write(MemoryMarshal.Cast<Double, Byte>(val.AsSpan()));
		return Task.CompletedTask;
	}

	/// <summary>
	/// Writes a raw string to the current stream.
	/// </summary>
	public void WriteRawString(String val)
	{
		Span<Byte> buffer = Encoding.UTF8.GetBytes(val);
		BaseStream.Write(buffer);
	}

	/// <summary>
	/// Writes a raw string to the current stream asynchronously.
	/// </summary>
	public async Task WriteRawStringAsync(String val)
	{
		Byte[] buffer = Encoding.UTF8.GetBytes(val);
		await BaseStream.WriteAsync(buffer);
	}

	#endregion
}
