namespace Helium.Data.Nbt;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;

/// <summary>
/// Represents a NBT root token.
/// </summary>
[RequiresPreviewFeatures]
public sealed record NbtRootToken : IRootToken
{
	private readonly Dictionary<String, IDataToken> children;

	public IDataToken this[String key]
	{
		get => children[key];
		set => children[key] = value;
	}

	/// <summary>
	/// Stores the binary marker for this token type.
	/// </summary>
	public static Byte Declarator => 0x0A;

	/// <summary>
	/// Stores the binary format for this type.
	/// </summary>
	public static String DataFormat => "nbt";

	/// <summary>
	/// Stores all child tokens to this token.
	/// </summary>
	public IEnumerable<IDataToken> Children => children.Values;

	/// <summary>
	/// Represents the name of this token. May be <c>null</c> if your token is an element of a <see cref="NbtListToken"/>.
	/// </summary>
	public String Name { get; set; } = null!;

	/// <summary>
	/// Represents the root of this token.
	/// </summary>
	public IRootToken? RootToken { get; }

	/// <summary>
	/// Represents the direct parent of this token.
	/// </summary>
	public IDataToken? ParentToken { get; }

	public ICollection<String> Keys => children.Keys;

	public ICollection<IDataToken> Values => children.Values;

	public Int32 Count { get; }

	public Boolean IsReadOnly { get; }

	public NbtRootToken()
	{
		this.children = new();
	}

	public NbtRootToken(String name)
	{
		this.children = new();
		this.Name = name;
	}

	public NbtRootToken(Dictionary<String, IDataToken> tokens)
	{
		this.children = tokens;
	}

	public NbtRootToken(String name, Dictionary<String, IDataToken> tokens)
	{
		this.children = tokens;
		this.Name = name;
	}

	public NbtRootToken(IEnumerable<IDataToken> tokens)
	{
		this.children = new();
		foreach(IDataToken token in tokens)
		{
			children.Add(token.Name, token);
		}
	}

	public NbtRootToken(String name, IEnumerable<IDataToken> tokens)
	{
		this.children = new();
		foreach(IDataToken token in tokens)
		{
			children.Add(token.Name, token);
		}
		this.Name = name;
	}

	public NbtCompoundToken ToCompound()
	{
		return (NbtCompoundToken)(ICompoundToken)this;
	}

	public void Add(String key, IDataToken value)
	{
		this.children.Add(key, value);
	}

	public void Add(KeyValuePair<String, IDataToken> item)
	{
		this.children.Add(item.Key, item.Value);
	}

	public void AddChildToken(IDataToken token)
	{
		this.children.Add(token.Name, token);
	}

	public void Clear()
	{
		this.children.Clear();
	}

	public Boolean Contains(KeyValuePair<String, IDataToken> item)
	{
		return this.children.ContainsKey(item.Key) && this.children[item.Key] == item.Value;
	}

	public Boolean ContainsKey(String key)
	{
		return this.children.ContainsKey(key);
	}

	public void CopyTo(KeyValuePair<String, IDataToken>[] array, Int32 arrayIndex)
	{
		List<KeyValuePair<String, IDataToken>> underlying = new(this.Count);

		foreach(String s in this.Keys)
		{
			underlying.Add(new(s, this[s]));
		}

		underlying.CopyTo(array, arrayIndex);
	}

	public IEnumerator<KeyValuePair<String, IDataToken>> GetEnumerator()
	{
		return this.children.GetEnumerator();
	}

	public Boolean Remove(String key)
	{
		return this.children.Remove(key);
	}

	public Boolean Remove(KeyValuePair<String, IDataToken> item)
	{
		if(this.Contains(item))
		{
			return this.children.Remove(item.Key);
		}
		return false;
	}

	public Boolean TryGetValue(String key,

		[MaybeNullWhen(false)]
		out IDataToken value)
	{
		return this.children.TryGetValue(key, out value);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.children.GetEnumerator();
	}
}
