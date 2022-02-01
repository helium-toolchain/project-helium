namespace Helium.Data.Nbt.Serialization;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;

using Helium.Data.Abstraction;

/// <summary>
/// Provides a way to read a <see cref="NbtRootToken"/> from a string.
/// </summary>
[RequiresPreviewFeatures]
#pragma warning disable CA1822
public sealed class StringifiedNbtReader
{
	/// <summary>
	/// Deserializes a <see cref="NbtRootToken"/> from stringified NBT data. Requires the given data to be valid.
	/// </summary>
	public NbtRootToken Deserialize(String data)
	{
		if(!data.StartsWith('{') || !data.EndsWith('}'))
		{
			ThrowHelper.ThrowStringifiedNotWrappedIntoCompound();
		}

		MemoryStream stream = new(Encoding.UTF8.GetBytes(data));

		return this.readRoot(stream);
	}

	private NbtRootToken readRoot(MemoryStream stream)
	{
		NbtRootToken root = new()
		{
			Name = String.Empty,
			ParentToken = null!
		};

		root.RootToken = root;

		_ = stream.ReadByte();
		stream.SkipWhitespace();
		
		while(stream.Peek() != '}')
		{
			String name = this.readString(stream);

			if(!stream.Expect(':'))
			{
				ThrowHelper.ThrowStringifiedIdentifierNotFollowedByColon();
			}

			root.AddChildToken(this.readToken(name, stream));
		}

		return root;
	}

	private String readString(MemoryStream stream)
	{
		if(stream.Peek() == '\'' || stream.Peek() == '\"')
		{
			return this.readQuotedString((Char)stream.Peek(), stream);
		}
		else
		{
			return this.readUnquotedString(stream);
		}
	}

	private String readQuotedString(Char openingQuotation, MemoryStream stream)
	{
		_ = stream.ReadByte();

		MemoryStream buffer = new();
		Byte last = 0x00, current;

		while(stream.Peek() != openingQuotation && last != '\\')
		{
			current = (Byte)stream.ReadByte();

			buffer.WriteByte(current);

			last = current;
		}

		return Encoding.UTF8.GetString(buffer.ToArray());
	}

	private String readUnquotedString(MemoryStream stream)
	{
		MemoryStream buffer = new();

		while(stream.Peek() != ':')
		{
			if(stream.Peek() == '\'' || stream.Peek() == '\"')
			{
				ThrowHelper.ThrowStringifiedUnquotedStringContainsQuotations();
			}

			if(stream.Peek() == ' ')
			{
				if(!stream.Expect(':'))
				{
					ThrowHelper.ThrowStringifiedIdentifierNotFollowedByColon();
				}

				return Encoding.UTF8.GetString(buffer.ToArray());
			}

			buffer.WriteByte((Byte)stream.ReadByte());
		}

		return Encoding.UTF8.GetString(buffer.ToArray());
	}

	private IDataToken readToken(String name, MemoryStream stream)
	{
		stream.SkipWhitespace();

		if(stream.Peek() == '{')
		{
			return this.readCompound(name, stream);
		}
		else if(stream.Peek() == '[')
		{
			return this.readEnumerable(name, stream);
		}
		else
		{
			return this.readValue(name, stream);
		}
	}

	private NbtCompoundToken readCompound(String name, MemoryStream stream)
	{
		NbtCompoundToken root = new()
		{
			Name = name
		};

		_ = stream.ReadByte();
		stream.SkipWhitespace();

		while(stream.Peek() != '}')
		{
			String childName = this.readString(stream);

			if(!stream.Expect(':'))
			{
				ThrowHelper.ThrowStringifiedIdentifierNotFollowedByColon();
			}

			root.AddChildToken(this.readToken(childName, stream));
		}

		return root;
	}

	private IDataToken readEnumerable(String name, MemoryStream stream)
	{
		stream.SkipWhitespace();
		return stream.Peek(3) == ';' ? this.readArray(name, stream) : this.readList(name, stream);
	}

	private IDataToken readArray(String name, MemoryStream stream)
	{
		return (Char)stream.Peek(2) switch
		{
			'b' or 'B' => this.readByteArray(name, stream),
			'i' or 'I' => this.readInt32Array(name, stream),
			'l' or 'L' => this.readInt64Array(name, stream),
			_ => ThrowHelper.ThrowStringifiedInvalidArrayIdentifier()
		};
	}

	private NbtSByteArrayToken readByteArray(String name, MemoryStream stream)
	{
		List<SByte> values = new();
		stream.Skip(3);
		Span<Byte> buffer = stackalloc Byte[4];

		for(Int32 i = 0; ; i++)
		{
			Byte temp = (Byte)stream.ReadByte();

			if(temp.IsNumber() || temp == '-')
			{
				buffer[i] = temp;
			}
			else if(temp == ',')
			{
				if(!SByte.TryParse(Encoding.ASCII.GetString(buffer), out SByte value))
				{
					ThrowHelper.ThrowInvalidIntegerArrayElement("byte array");
				}

				values.Add(value);

				buffer.Clear();
				i = 0;

				stream.SkipWhitespace();
			}
			else if(temp == ' ')
			{
				stream.SkipWhitespace();

				if(stream.Peek() == ',')
				{
					if(!SByte.TryParse(Encoding.ASCII.GetString(buffer), out SByte value))
					{
						ThrowHelper.ThrowInvalidIntegerArrayElement("byte array");
					}

					values.Add(value);

					buffer.Clear();
					i = 0;

					stream.Position++;
				}
				else if(stream.Peek() == ']')
				{
					NbtSByteArrayToken ret = new()
					{
						Name = name
					};

					ret.SetChildren(CollectionsMarshal.AsSpan(values));

					return ret;
				}
				else
				{
					ThrowHelper.ThrowStringifiedInvalidIntegerArray();
				}
			}
			else if(temp == ']')
			{
				NbtSByteArrayToken ret = new()
				{
					Name = name
				};

				ret.SetChildren(CollectionsMarshal.AsSpan(values));

				return ret;
			}
			else
			{
				ThrowHelper.ThrowStringifiedInvalidIntegerArray();
			}
		}
	}

	private NbtInt32ArrayToken readInt32Array(String name, MemoryStream stream)
	{
		List<Int32> values = new();
		stream.Skip(3);
		Span<Byte> buffer = stackalloc Byte[11];

		for(Int32 i = 0; ; i++)
		{
			Byte temp = (Byte)stream.ReadByte();

			if(temp.IsNumber() || temp == '-')
			{
				buffer[i] = temp;
			}
			else if(temp == ',')
			{
				if(!Int32.TryParse(Encoding.ASCII.GetString(buffer), out Int32 value))
				{
					ThrowHelper.ThrowInvalidIntegerArrayElement("byte array");
				}

				values.Add(value);

				buffer.Clear();
				i = 0;

				stream.SkipWhitespace();
			}
			else if(temp == ' ')
			{
				stream.SkipWhitespace();

				if(stream.Peek() == ',')
				{
					if(!Int32.TryParse(Encoding.ASCII.GetString(buffer), out Int32 value))
					{
						ThrowHelper.ThrowInvalidIntegerArrayElement("byte array");
					}

					values.Add(value);

					buffer.Clear();
					i = 0;

					stream.Position++;
				}
				else if(stream.Peek() == ']')
				{
					NbtInt32ArrayToken ret = new()
					{
						Name = name
					};

					ret.SetChildren(CollectionsMarshal.AsSpan(values));

					return ret;
				}
				else
				{
					ThrowHelper.ThrowStringifiedInvalidIntegerArray();
				}
			}
			else if(temp == ']')
			{
				NbtInt32ArrayToken ret = new()
				{
					Name = name
				};

				ret.SetChildren(CollectionsMarshal.AsSpan(values));

				return ret;
			}
			else
			{
				ThrowHelper.ThrowStringifiedInvalidIntegerArray();
			}
		}
	}

	private NbtInt64ArrayToken readInt64Array(String name, MemoryStream stream)
	{
		List<Int64> values = new();
		stream.Skip(3);
		Span<Byte> buffer = stackalloc Byte[20];

		for(Int32 i = 0; ; i++)
		{
			Byte temp = (Byte)stream.ReadByte();

			if(temp.IsNumber() || temp == '-')
			{
				buffer[i] = temp;
			}
			else if(temp == ',')
			{
				if(!Int64.TryParse(Encoding.ASCII.GetString(buffer), out Int64 value))
				{
					ThrowHelper.ThrowInvalidIntegerArrayElement("byte array");
				}

				values.Add(value);

				buffer.Clear();
				i = 0;

				stream.SkipWhitespace();
			}
			else if(temp == ' ')
			{
				stream.SkipWhitespace();

				if(stream.Peek() == ',')
				{
					if(!Int64.TryParse(Encoding.ASCII.GetString(buffer), out Int64 value))
					{
						ThrowHelper.ThrowInvalidIntegerArrayElement("byte array");
					}

					values.Add(value);

					buffer.Clear();
					i = 0;

					stream.Position++;
				}
				else if(stream.Peek() == ']')
				{
					NbtInt64ArrayToken ret = new()
					{
						Name = name
					};

					ret.SetChildren(CollectionsMarshal.AsSpan(values));

					return ret;
				}
				else
				{
					ThrowHelper.ThrowStringifiedInvalidIntegerArray();
				}
			}
			else if(temp == ']')
			{
				NbtInt64ArrayToken ret = new()
				{
					Name = name
				};

				ret.SetChildren(CollectionsMarshal.AsSpan(values));

				return ret;
			}
			else
			{
				ThrowHelper.ThrowStringifiedInvalidIntegerArray();
			}
		}
	}

	private IListToken readList(String name, MemoryStream stream)
	{
		List<IDataToken> children = new();
		stream.Skip(1);
		stream.SkipWhitespace();

		while(stream.Peek() != ']')
		{
			children.Add(this.readToken(String.Empty, stream));
		}

		if(children.Count == 0)
		{
			return new NbtListToken()
			{
				Name = name,
				ListTypeDeclarator = (Byte)NbtTokenType.End
			};
		}

		NbtListToken token = new()
		{
			Name = name,
			ListTypeDeclarator = children[0].RefDeclarator
		};

		token.children = children;

		return token;
	}

	private IDataToken readValue(String name, MemoryStream stream)
	{
		stream.SkipWhitespace();

		if(stream.Peek().IsAlphanumeric())
		{
			return new NbtStringToken()
			{
				Name = name,
				Value = this.readString(stream)
			};
		}

		String value = this.readUnquotedString(stream);

		if((value.EndsWith('b') || value.EndsWith('B')) && SByte.TryParse(value.AsSpan()[..(value.Length - 1)], out SByte b))
		{
			return new NbtSByteToken()
			{
				Name = name,
				Value = b
			};
		}
		else if((value.EndsWith('s') || value.EndsWith('S')) && Int16.TryParse(value.AsSpan()[..(value.Length - 1)], out Int16 s))
		{
			return new NbtInt16Token()
			{
				Name = name,
				Value = s
			};
		}
		else if((value.EndsWith('l') || value.EndsWith('L')) && Int64.TryParse(value.AsSpan()[..(value.Length - 1)], out Int64 l))
		{
			return new NbtInt64Token()
			{
				Name = name,
				Value = l
			};
		}
		else if((value.EndsWith('f') || value.EndsWith('F')) && Single.TryParse(value.AsSpan()[..(value.Length - 1)], out Single f))
		{
			return new NbtSingleToken()
			{
				Name = name,
				Value = f
			};
		}
		else if((value.EndsWith('d') || value.EndsWith('D')) && Double.TryParse(value.AsSpan()[..(value.Length - 1)], out Double d))
		{
			return new NbtDoubleToken()
			{
				Name = name,
				Value = d
			};
		}
		else if(Int32.TryParse(value, out Int32 i))
		{
			return new NbtInt32Token()
			{
				Name = name,
				Value = i
			};
		}
		else
		{
			throw new ArgumentException("Invalid value token encountered.");
		}
	}
}
#pragma warning restore CA1822
