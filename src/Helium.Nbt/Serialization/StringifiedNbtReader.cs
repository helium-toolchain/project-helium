namespace Helium.Nbt.Serialization;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

using Helium.Nbt.Exceptions;

/// <summary>
/// Provides methods for reading a <see cref="NbtCompoundToken"/> from a string. Only ASCII characters are supported.
/// </summary>
[RequiresPreviewFeatures]
public class StringifiedNbtReader
{
	private MemoryStream data;

	/// <summary>
	/// Creates a new instance.
	/// </summary>
	public StringifiedNbtReader()
	{
		this.data = null!;
	}

	/// <summary>
	/// Reads a compound from a string.
	/// </summary>
	/// <exception cref="MalformedSNbtException">Thrown if any data is invalid</exception>
	public NbtCompoundToken ReadCompound(String data)
	{
		if(!data.StartsWith('{') || !data.EndsWith('}'))
		{
			throw new MalformedSNbtException("Data was not wrapped into a root compound");
		}

		return this.ReadCompound(Encoding.UTF8.GetBytes(data));
	}

	/// <summary>
	/// Reads a compound from a byte array
	/// </summary>
	/// <exception cref="MalformedSNbtException">Thrown if any data is invalid</exception>
	public NbtCompoundToken ReadCompound(Byte[] data)
	{
		this.data = new MemoryStream(data);

		return this.ReadRootCompoundToken();
	}

	/// <summary>
	/// Reads the root compound token from the current stream.
	/// </summary>
	public NbtCompoundToken ReadRootCompoundToken()
	{
		return ReadCompoundToken(Array.Empty<Byte>());
	}

	/// <summary>
	/// Reads a named compound token from the current stream.
	/// </summary>
	public NbtCompoundToken ReadCompoundToken(Byte[] name)
	{
		NbtCompoundToken compound = new(name, new());

		if(!this.data.Expect(0x7B))
		{
			throw new MalformedSNbtException("The given data blob did not start with an opening curly bracket.");
		}

		this.data.SkipWhitespace();

		while(this.data.Peek() != 0x7D)
		{
			Byte[] nextName = this.ReadString();

			if(!this.data.Expect(0x3A))
			{
				throw new MalformedSNbtException("No value following the given name.");
			}

			compound.AddChild(this.ReadValue(nextName));
		}

		return compound;
	}

	/// <summary>
	/// Reads a string, quoted or unquoted, from the current stream.
	/// </summary>
	public Byte[] ReadString()
	{
		if(this.data.Peek() == 0x22 || this.data.Peek() == 0x27)
		{
			return this.ReadQuotedString();
		}
		else
		{
			return this.ReadUnquotedString();
		}
	}

	/// <summary>
	/// Reads a quoted string from the current stream.
	/// </summary>
	public Byte[] ReadQuotedString()
	{
		_ = this.data.ReadByte();

		MemoryStream buffer = new();

		while(this.data.Peek() != 0x22 && this.data.Peek() != 0x27)
		{
			buffer.WriteByte((Byte)this.data.ReadByte());
		}

		return buffer.ToArray();
	}

	/// <summary>
	/// Reads an unquoted string from the current stream.
	/// </summary>
	public Byte[] ReadUnquotedString()
	{
		MemoryStream buffer = new();
		
		while(this.data.Peek() != 0x3A)
		{
			if(this.data.Peek() == 0x22 || this.data.Peek() == 0x27)
			{
				throw new MalformedSNbtException("Unquoted strings cannot contain any quotation marks.");
			}

			if(this.data.Peek() == 0x20)
			{
				if(!this.data.Expect(0x3A))
				{
					throw new MalformedSNbtException("Invalid unquoted string");
				}

				return buffer.ToArray();
			}

			buffer.WriteByte((Byte)this.data.ReadByte());
		}

		return buffer.ToArray();
	}

	/// <summary>
	/// Reads any nbt token from the current stream.
	/// </summary>
	public INbtToken ReadValue(Byte[] name)
	{
		this.data.SkipWhitespace();

		Byte val = this.data.Peek();

		if(val == 0x7B)
		{
			return this.ReadCompoundToken(name);
		}
		else if(val == 0x5B)
		{
			return this.ReadEnumerableToken(name);
		}
		else
		{
			return this.ReadValueToken(name);
		}
	}

	/// <summary>
	/// Reads any array/list token from the current stream.
	/// </summary>
	public INbtToken ReadEnumerableToken(Byte[] name)
	{
		this.data.SkipWhitespace();
		return this.data.Peek(3) == 0x3B ? this.ReadArrayToken(name) : this.ReadListToken(name);
	}

	/// <summary>
	/// Reads any array token from the current stream.
	/// </summary>
	public INbtToken ReadArrayToken(Byte[] name)
	{
		return this.data.Peek(2) switch
		{
			0x43 or 0x63 => ReadByteArrayToken(name),
			0x49 or 0x69 => ReadIntArrayToken(name),
			0x4C or 0x6C => ReadLongArrayToken(name),
			_ => throw new MalformedSNbtException("Invalid array identifier.")
		};
	}

	/// <summary>
	/// Reads a byte array token from the current stream.
	/// </summary>
	public INbtToken ReadByteArrayToken(Byte[] name)
	{
		List<SByte> bytes = new();
		this.data.Skip(3);

		Span<Byte> buffer = stackalloc Byte[4];

		for(Int32 i = 0; ; i++)
		{
			Byte temp = (Byte)this.data.ReadByte();

			if(temp.IsNumber() || temp == 0x2D)
			{
				buffer[i] = temp;
			}
			else if(temp == 0x2C)
			{
				if(!SByte.TryParse(Encoding.ASCII.GetString(buffer), out SByte addition))
				{
					throw new MalformedSNbtException("Invalid value found in byte array Token");
				}

				bytes.Add(addition);

				buffer.Clear();
				i = 0;
			}
			else if(temp == 0x5D)
			{
				return new NbtByteArrayToken(name, CollectionsMarshal.AsSpan(bytes));
			}
			else
			{
				throw new MalformedSNbtException("Invalid character found in byte array Token");
			}
		}
	}

	/// <summary>
	/// Reads an int array token from the current stream.
	/// </summary>
	public INbtToken ReadIntArrayToken(Byte[] name)
	{
		List<Int32> ints = new();
		this.data.Skip(3);

		Span<Byte> buffer = stackalloc Byte[11];

		for(Int32 i = 0; ; i++)
		{
			Byte temp = (Byte)this.data.ReadByte();

			if(temp.IsNumber() || temp == 0x2D)
			{
				buffer[i] = temp;
			}
			else if(temp == 0x2C)
			{
				if(!Int32.TryParse(Encoding.ASCII.GetString(buffer), out Int32 addition))
				{
					throw new MalformedSNbtException("Invalid value found in int array Token");
				}

				ints.Add(addition);

				buffer.Clear();
				i = 0;
			}
			else if(temp == 0x5D)
			{
				return new NbtInt32ArrayToken(name, ints);
			}
			else
			{
				throw new MalformedSNbtException("Invalid character found in int array Token");
			}
		}
	}

	/// <summary>
	/// Reads a long array token from the current stream.
	/// </summary>
	public INbtToken ReadLongArrayToken(Byte[] name)
	{
		List<Int64> longs = new();
		this.data.Skip(3);

		Span<Byte> buffer = stackalloc Byte[20];

		for(Int32 i = 0; ; i++)
		{
			Byte temp = (Byte)this.data.ReadByte();

			if(temp.IsNumber() || temp == 0x2D)
			{
				buffer[i] = temp;
			}
			else if(temp == 0x2C)
			{
				if(!Int64.TryParse(Encoding.ASCII.GetString(buffer), out Int64 addition))
				{
					throw new MalformedSNbtException("Invalid value found in long array Token");
				}

				longs.Add(addition);

				buffer.Clear();
				i = 0;
			}
			else if(temp == 0x5D)
			{
				return new NbtInt64ArrayToken(name, longs);
			}
			else
			{
				throw new MalformedSNbtException("Invalid character found in long array Token");
			}
		}
	}

	/// <summary>
	/// Reads a list token from the current stream.
	/// </summary>
	public INbtToken ReadListToken(Byte[] name)
	{
		List<INbtToken> children = new();

		this.data.Skip(1);
		this.data.SkipWhitespace();

		while(this.data.Peek() != 0x5D)
		{
			children.Add(this.ReadValue(Array.Empty<Byte>()));
		}

		if(children.Count == 0)
		{
			return new NbtListToken(name, children, 0, NbtTokenType.End);
		}

		return children[0] switch
		{
			NbtByteArrayToken => new NbtListToken(name, children, children.Count, NbtTokenType.ByteArray),
			NbtByteToken => new NbtListToken(name, children, children.Count, NbtTokenType.Byte),
			NbtCompoundToken => new NbtListToken(name, children, children.Count, NbtTokenType.Compound),
			NbtDoubleToken => new NbtListToken(name, children, children.Count, NbtTokenType.Double),
			NbtInt16Token => new NbtListToken(name, children, children.Count, NbtTokenType.Short),
			NbtInt32ArrayToken => new NbtListToken(name, children, children.Count, NbtTokenType.IntArray),
			NbtInt32Token => new NbtListToken(name, children, children.Count, NbtTokenType.Int),
			NbtInt64ArrayToken => new NbtListToken(name, children, children.Count, NbtTokenType.LongArray),
			NbtInt64Token => new NbtListToken(name, children, children.Count, NbtTokenType.Long),
			NbtSingleToken => new NbtListToken(name, children, children.Count, NbtTokenType.Float),
			NbtStringToken => new NbtListToken(name, children, children.Count, NbtTokenType.String),
			_ => throw new MalformedSNbtException("Invalid list Token contents")
		};
	}

	/// <summary>
	/// Reads any value token from the current stream; of the form <c>name:value</c>
	/// </summary>
	public INbtToken ReadValueToken(Byte[] name)
	{
		this.data.SkipWhitespace();

		if(this.data.Peek().IsAlphanumeric())
		{
			return new NbtStringToken(name, Encoding.ASCII.GetString(this.ReadString()));
		}

		String value = Encoding.ASCII.GetString(this.ReadUnquotedString());

		if(value.EndsWith('b') && SByte.TryParse(value.AsSpan()[..(value.Length - 1)], out SByte b))
		{
			return new NbtByteToken(name, b);
		}
		else if(value.EndsWith('s') && Int16.TryParse(value.AsSpan()[..(value.Length -1)], out Int16 s))
		{
			return new NbtInt16Token(name, s);
		}
		else if(value.EndsWith('l') && Int64.TryParse(value.AsSpan()[..(value.Length - 1)], out Int64 l))
		{
			return new NbtInt64Token(name, l);
		}
		else if(value.EndsWith('f') && Single.TryParse(value.AsSpan()[..(value.Length - 1)], out Single f))
		{
			return new NbtSingleToken(name, f);
		}
		else if(value.EndsWith('d') && Double.TryParse(value.AsSpan()[..(value.Length - 1)], out Double d))
		{
			return new NbtDoubleToken(name, d);
		}
		else if(Int32.TryParse(value, out Int32 i))
		{
			return new NbtInt32Token(name, i);
		}
		else
		{
			throw new MalformedSNbtException("Invalid value token encountered");
		}
	}
}
