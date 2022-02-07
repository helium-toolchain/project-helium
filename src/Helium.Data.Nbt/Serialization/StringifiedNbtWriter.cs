namespace Helium.Data.Nbt.Serialization;

using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;

using Helium.Data.Abstraction;

/// <summary>
/// Provides a method to write sNBT data to a string.
/// </summary>
[RequiresPreviewFeatures]
#pragma warning disable CA1822
public sealed class StringifiedNbtWriter
{
	/// <summary>
	/// Serializes a given <see cref="NbtRootToken"/> into sNBT.
	/// </summary>
	public String Serialize(NbtRootToken root)
	{
		MemoryStream stream = new();
		stream.WriteByte((Byte)'{');

		for(Int32 i = 0; i < root.Children.Count(); i++)
		{
			this.writeToken(root.Children.ElementAt(i), stream);

			if(i < root.Children.Count() - 1)
			{
				stream.WriteByte((Byte)',');
			}
		}

		stream.WriteByte((Byte)'}');

		return Encoding.UTF8.GetString(stream.ToArray());
	}

	private void writeToken(IDataToken token, MemoryStream stream)
	{
		this.writeString(token.Name, stream);
		stream.WriteByte((Byte)':');
		this.writeTokenPayload(token, stream);
	}

	private void writeString(String value, MemoryStream stream)
	{
		stream.WriteByte((Byte)'\"');
		stream.Write(Encoding.UTF8.GetBytes(value));
		stream.WriteByte((Byte)'\"');
	}

	private void writeTokenPayload(IDataToken token, MemoryStream stream)
	{
		switch(token)
		{
			case NbtSByteArrayToken:
				this.writeSByteArray((NbtSByteArrayToken)token, stream);
				break;
			case NbtSByteToken:
				this.writeSByte((NbtSByteToken)token, stream);
				break;
			case NbtCompoundToken:
				this.writeCompound((NbtCompoundToken)token, stream);
				break;
			case NbtDoubleToken:
				this.writeDouble((NbtDoubleToken)token, stream);
				break;
			case NbtInt16Token:
				this.writeInt16((NbtInt16Token)token, stream);
				break;
			case NbtInt32ArrayToken:
				this.writeInt32Array((NbtInt32ArrayToken)token, stream);
				break;
			case NbtInt32Token:
				this.writeInt32((NbtInt32Token)token, stream);
				break;
			case NbtInt64ArrayToken:
				this.writeInt64Array((NbtInt64ArrayToken)token, stream);
				break;
			case NbtInt64Token:
				this.writeInt64((NbtInt64Token)token, stream);
				break;
			case NbtListToken:
				this.writeList((NbtListToken)token, stream);
				break;
			case NbtSingleToken:
				this.writeSingle((NbtSingleToken)token, stream);
				break;
			case NbtStringToken:
				this.writeString(((NbtStringToken)token).Value, stream);
				break;
			default: 
				throw new ArgumentException("Could not write the data blob to sNBT");
		};
	}

	private void writeSByteArray(NbtSByteArrayToken token, MemoryStream stream)
	{
		stream.Write(new Byte[] { 0x5B, 0x42, 0x3B });

		for(Int32 i = 0; i < token.children.Count; i++)
		{
			stream.Write(Encoding.ASCII.GetBytes(token.children[i].ToString()));

			if(i < token.children.Count - 1)
			{
				stream.WriteByte(0x2C);
			}
		}

		stream.WriteByte(0x5D);
	}

	private void writeInt32Array(NbtInt32ArrayToken token, MemoryStream stream)
	{
		stream.Write(new Byte[] { 0x5B, 0x49, 0x3B });

		for(Int32 i = 0; i < token.children.Count; i++)
		{
			stream.Write(Encoding.ASCII.GetBytes(token.children[i].ToString()));

			if(i < token.children.Count - 1)
			{
				stream.WriteByte(0x2C);
			}
		}

		stream.WriteByte(0x5D);
	}

	private void writeInt64Array(NbtInt64ArrayToken token, MemoryStream stream)
	{
		stream.Write(new Byte[] { 0x5B, 0x4C, 0x3B });

		for(Int32 i = 0; i < token.children.Count; i++)
		{
			stream.Write(Encoding.ASCII.GetBytes(token.children[i].ToString()));

			if(i < token.children.Count - 1)
			{
				stream.WriteByte(0x2C);
			}
		}

		stream.WriteByte(0x5D);
	}

	private void writeSByte(NbtSByteToken token, MemoryStream stream)
	{
		stream.WriteByte((Byte)token.Value);
		stream.WriteByte(0x62);
	}

	private void writeCompound(NbtCompoundToken token, MemoryStream stream)
	{
		stream.WriteByte(0x7B);

		for(Int32 i = 0; i < token.Children.Count(); i++)
		{
			this.writeToken(token.Children.ElementAt(i), stream);

			if(i < token.Children.Count() - 1)
			{
				stream.WriteByte(0x2C);
			}
		}

		stream.WriteByte(0x7D);
	}

	private void writeDouble(NbtDoubleToken token, MemoryStream stream)
	{
		stream.Write(Encoding.ASCII.GetBytes(token.Value.ToString()));
		stream.WriteByte(0x64);
	}

	private void writeInt16(NbtInt16Token token, MemoryStream stream)
	{
		stream.Write(Encoding.ASCII.GetBytes(token.Value.ToString()));
		stream.WriteByte(0x73);
	}

	private void writeInt32(NbtInt32Token token, MemoryStream stream)
	{
		stream.Write(Encoding.ASCII.GetBytes(token.Value.ToString()));
	}

	private void writeInt64(NbtInt64Token token, MemoryStream stream)
	{
		stream.Write(Encoding.ASCII.GetBytes(token.Value.ToString()));
		stream.WriteByte(0x6C);
	}

	private void writeSingle(NbtSingleToken token, MemoryStream stream)
	{
		stream.Write(Encoding.ASCII.GetBytes(token.Value.ToString()));
		stream.WriteByte(0x66);
	}

	private void writeList(NbtListToken token, MemoryStream stream)
	{
		if(token.ListTypeDeclarator != (Byte)NbtTokenType.End)
		{
			stream.WriteByte(0x5B);

			for(Int32 i = 0; i < token.children.Count; i++)
			{
				this.writeTokenPayload(token.children[i], stream);

				if(i < token.children.Count - 1)
				{
					stream.WriteByte(0x2C);
				}
			}

			stream.WriteByte(0x5D);
		}
		else
		{
			stream.WriteByte(0x5B);
			stream.WriteByte(0x5D);
		}
	}
}
#pragma warning restore CA1822
