namespace Helium.Nbt.Serialization;

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Text;

using Helium.Nbt.Exceptions;

/// <summary>
/// Provides methods for writing a <see cref="NbtCompoundToken"/> to sNBT
/// </summary>
[RequiresPreviewFeatures]
public class StringifiedNbtWriter
{
	private MemoryStream data;

	public StringifiedNbtWriter()
	{
		this.data = new();
	}

	public Byte[] WriteCompound(NbtCompoundToken compound)
	{
		this.data.WriteByte(0x7B);

		for(Int32 i = 0; i < compound.Children.Count; i++)
		{
			this.WriteToken(compound.Children[i]);

			if(i < compound.Children.Count - 1)
			{
				this.data.WriteByte(0x2C);
			}
		}

		this.data.WriteByte(0x7D);

		Byte[] vs = data.ToArray();
		this.data = new();
		return vs;
	}

	public void WriteToken(INbtToken token)
	{
		this.data.WriteByte(0x22);
		this.data.Write(token.Name);
		this.data.WriteByte(0x22);
		this.data.WriteByte(0x3A);
		this.WriteTokenPayload(token);
	}

	public void WriteTokenPayload(INbtToken token)
	{
		switch(token)
		{
			case NbtBigEndianInt32ArrayToken:
				this.WriteBigEndianInt32ArrayToken((NbtBigEndianInt32ArrayToken)token); 
				break;
			case NbtBigEndianInt64ArrayToken:
				this.WriteBigEndianInt64ArrayToken((NbtBigEndianInt64ArrayToken)token); 
				break;
			case NbtByteArrayToken:
				this.WriteByteArrayToken((NbtByteArrayToken)token); 
				break;
			case NbtByteToken:
				this.WriteByteToken((NbtByteToken)token); 
				break;
			case NbtCompoundToken: 
				this.WriteCompoundToken((NbtCompoundToken)token); 
				break;
			case NbtDoubleToken: 
				this.WriteDoubleToken((NbtDoubleToken)token); 
				break;
			case NbtInt16Token: 
				this.WriteInt16Token((NbtInt16Token)token); 
				break;
			case NbtInt32ArrayToken: 
				this.WriteInt32ArrayToken((NbtInt32ArrayToken)token); 
				break;
			case NbtInt32Token: 
				this.WriteInt32Token((NbtInt32Token)token); 
				break;
			case NbtInt64ArrayToken: 
				this.WriteInt64ArrayToken((NbtInt64ArrayToken)token); 
				break;
			case NbtInt64Token: 
				this.WriteInt64Token((NbtInt64Token)token); 
				break;
			case NbtListToken: 
				this.WriteListToken((NbtListToken)token); 
				break;
			case NbtSingleToken: 
				this.WriteSingleToken((NbtSingleToken)token); 
				break;
			case NbtStringToken: 
				this.WriteStringToken((NbtStringToken)token); 
				break;
			default: throw new MalformedDataException("Could not write the data blob to sNBT");
		};
	}

	public void WriteBigEndianInt32ArrayToken(NbtBigEndianInt32ArrayToken token)
	{
		this.data.Write(new Byte[] { 0x5B, 0x49, 0x3B });

		for(Int32 i = 0; i < token.Elements.Count; i++)
		{
			this.data.Write(Encoding.ASCII.GetBytes(token.Elements[i].AsSystemDefault().ToString()));

			if(i < token.Elements.Count - 1)
			{
				this.data.WriteByte(0x2C);
			}
		}

		this.data.WriteByte(0x5D);
	}

	public void WriteBigEndianInt64ArrayToken(NbtBigEndianInt64ArrayToken token)
	{
		this.data.Write(new Byte[] { 0x5B, 0x4C, 0x3B });

		for(Int32 i = 0; i < token.Elements.Count; i++)
		{
			this.data.Write(Encoding.ASCII.GetBytes(token.Elements[i].AsSystemDefault().ToString()));

			if(i < token.Elements.Count - 1)
			{
				this.data.WriteByte(0x2C);
			}
		}

		this.data.WriteByte(0x5D);
	}

	public void WriteByteArrayToken(NbtByteArrayToken token)
	{
		this.data.Write(new Byte[] { 0x5B, 0x42, 0x3B });

		for(Int32 i = 0; i < token.Elements.Count; i++)
		{
			this.data.Write(Encoding.ASCII.GetBytes(token.Elements[i].ToString()));

			if(i < token.Elements.Count - 1)
			{
				this.data.WriteByte(0x2C);
			}
		}

		this.data.WriteByte(0x5D);
	}

	public void WriteInt32ArrayToken(NbtInt32ArrayToken token)
	{
		this.data.Write(new Byte[] { 0x5B, 0x49, 0x3B });

		for(Int32 i = 0; i < token.Elements.Count; i++)
		{
			this.data.Write(Encoding.ASCII.GetBytes(token.Elements[i].ToString()));

			if(i < token.Elements.Count - 1)
			{
				this.data.WriteByte(0x2C);
			}
		}

		this.data.WriteByte(0x5D);
	}

	public void WriteInt64ArrayToken(NbtInt64ArrayToken token)
	{
		this.data.Write(new Byte[] { 0x5B, 0x4C, 0x3B });

		for(Int32 i = 0; i < token.Elements.Count; i++)
		{
			this.data.Write(Encoding.ASCII.GetBytes(token.Elements[i].ToString()));

			if(i < token.Elements.Count - 1)
			{
				this.data.WriteByte(0x2C);
			}
		}

		this.data.WriteByte(0x5D);
	}

	public void WriteByteToken(NbtByteToken token)
	{
		this.data.WriteByte((Byte)token.Value);
		this.data.WriteByte(0x62);
	}

	public void WriteCompoundToken(NbtCompoundToken compound)
	{
		this.data.WriteByte(0x7B);

		for(Int32 i = 0; i < compound.Children.Count; i++)
		{
			this.WriteToken(compound.Children[i]);

			if(i < compound.Children.Count - 1)
			{
				this.data.WriteByte(0x2C);
			}
		}

		this.data.WriteByte(0x7D);
	}

	public void WriteDoubleToken(NbtDoubleToken token)
	{
		this.data.Write(Encoding.ASCII.GetBytes(token.Value.ToString()));
		this.data.WriteByte(0x64);
	}

	public void WriteInt16Token(NbtInt16Token token)
	{
		this.data.Write(Encoding.ASCII.GetBytes(token.Value.ToString()));
		this.data.WriteByte(0x73);
	}

	public void WriteInt32Token(NbtInt32Token token)
	{
		this.data.Write(Encoding.ASCII.GetBytes(token.Value.ToString()));
	}

	public void WriteInt64Token(NbtInt64Token token)
	{
		this.data.Write(Encoding.ASCII.GetBytes(token.Value.ToString()));
		this.data.WriteByte(0x6C);
	}

	public void WriteSingleToken(NbtSingleToken token)
	{
		this.data.Write(Encoding.ASCII.GetBytes(token.Value.ToString()));
		this.data.WriteByte(0x66);
	}

	public void WriteStringToken(NbtStringToken token)
	{
		this.data.WriteByte(0x22);
		this.data.Write(Encoding.ASCII.GetBytes(token.Value));
		this.data.WriteByte(0x22);
	}

	public void WriteListToken(NbtListToken token)
	{
		this.data.WriteByte(0x5B);

		for(Int32 i = 0; i < token.Content.Count; i++)
		{
			this.WriteTokenPayload(token.Content[i]);

			if(i < token.Content.Count - 1)
			{
				this.data.WriteByte(0x2C);
			}
		}

		this.data.WriteByte(0x5D);
	}
}
