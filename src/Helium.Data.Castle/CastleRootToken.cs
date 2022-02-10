namespace Helium.Data.Castle;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Versioning;

using Helium.Data.Abstraction;

[RequiresPreviewFeatures]
public record CastleRootToken : ICastleToken, IRootToken
{
	internal List<String> TokenNames { get; set; } = new();

	public IDataToken this[String key]
	{
		get => this.Children.Where(xm => xm.Name == key).FirstOrDefault()!;
		set => Children = Children.Where(xm => xm.Name != key).Append(value);
	}

	public static String DataFormat => "castle";

	public static Byte Declarator => 0x00;

	public IEnumerable<IDataToken> Children { get; internal set; } = new List<IDataToken>();

	public Byte RefDeclarator => Declarator;

	public String Name => String.Empty;

	public IRootToken? RootToken => this;

	public IDataToken? ParentToken => null!;

	public ICollection<String> Keys => (ICollection<String>)this.Children.Select(xm => xm.Name);

	public ICollection<IDataToken> Values => (ICollection<IDataToken>)this.Children;

	public Int32 Count => this.Children.Count();

	public Boolean IsReadOnly => (this.Children as IList)?.IsReadOnly ?? true;

	public void Add(String key, IDataToken value)
	{
		(this.Children as List<IDataToken>)?.Add(value);
	}

	public void Add(KeyValuePair<String, IDataToken> item)
	{
		(this.Children as List<IDataToken>)?.Add(item.Value);
	}

	public void AddChildToken(IDataToken token)
	{
		(this.Children as List<IDataToken>)?.Add(token);
	}

	public void Clear()
	{
		this.Children = new List<IDataToken>();
	}

	public Boolean Contains(KeyValuePair<String, IDataToken> item)
	{
		return this.Children.Any(xm => xm.Name == item.Key);
	}

	public Boolean ContainsKey(String key)
	{
		return this.Children.Any(xm => xm.Name == key);
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
		return (IEnumerator<KeyValuePair<String, IDataToken>>)this.Children.GetEnumerator();
	}

	public Boolean Remove(String key)
	{
		return (this.Children as List<IDataToken>)?.Remove(this.Children.First(xm => xm.Name == key)) ?? false;
	}

	public Boolean Remove(KeyValuePair<String, IDataToken> item)
	{
		return (this.Children as List<IDataToken>)?.Remove(item.Value) ?? false;
	}

	public IDataToken ToNbtToken()
	{
		throw new NotImplementedException();
	}

	public Boolean TryGetValue(String key, [MaybeNullWhen(false)] out IDataToken value)
	{
		if(!this.Children.Any(xm => xm.Name == key))
		{
			value = null!;
			return false;
		}
		value = this.Children.First(xm => xm.Name == key);
		return true;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.Children.GetEnumerator();
	}
}
