namespace Helium.Nbt.Serialization;

using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.Versioning;
using System.Text;

using Helium.Nbt.Exceptions;

/// <summary>
/// Provides methods for reading a <see cref="NbtCompoundToken"/> from a string.
/// </summary>
[RequiresPreviewFeatures]
public class StringifiedNbtReader
{
	private readonly StringifiedReaderTypeHandling typeHandling;
	private MemoryStream dataStream;

	public StringifiedNbtReader(StringifiedReaderTypeHandling typeHandling)
	{
		this.typeHandling = typeHandling;
		this.dataStream = null!;
	}

	public NbtCompoundToken ReadCompound(String data)
	{
		if(!data.StartsWith('{') || !data.EndsWith('}'))
		{
			throw new MalformedSNbtException("The data was not wrapped into a root compound");
		}

		return this.ReadCompound(Encoding.UTF8.GetBytes(data));
	}

	public NbtCompoundToken ReadCompound(Byte[] data)
	{
		dataStream = new MemoryStream(data);

		return this.ReadRootCompoundToken();
	}

	public NbtCompoundToken ReadRootCompoundToken()
	{
		_ = this.dataStream.ReadByte();

		NbtCompoundToken root = new();
		Byte t;

		while((t = (Byte)this.dataStream.ReadByte()) != 0x7D) // check whether the next byte matches '}'
		{
			if(t != 0x2C)
			{
				continue; // the head instruction should read a ',' 
			}

			root.AddChild(this.ReadNextToken());
		}

		return root;
	}

	public unsafe INbtToken ReadNextToken()
	{
		Byte temp, preceding;
		Boolean enclosed;
		Byte* ptr = stackalloc Byte[256];
		Span<Byte> buffer = new(ptr, 256);

		while ((temp = (Byte)this.dataStream.ReadByte()) == 0x20) // get rid of any additional spaces
		{
			continue;
		}

		if (temp == 0x22 || temp == 0x27) // check for " and '
		{
			temp = (Byte)this.dataStream.ReadByte();
			enclosed = true;
		}

		*ptr = temp;
		ptr++;

		while (true)
		{

		}
	}
}
