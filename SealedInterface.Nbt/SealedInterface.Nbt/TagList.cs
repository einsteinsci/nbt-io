﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt
{
	public sealed class TagList : DynamicObject, INamedBinaryTag, IList<INamedBinaryTag>
	{
		public string Name
		{ get; private set; }

		public ETagType TagType => ETagType.List;

		public int Count => Children.Count;

		public bool IsReadOnly => false;

		public List<INamedBinaryTag> Children
		{ get; private set; }

		public object UnderlyingValue => this;

		public ETagType GenericType
		{ get; internal set; }

		public INamedBinaryTag this[int i]
		{
			get
			{
				return Children[i];
			}
			set
			{
				Children[i] = value;
			}
		}

		public TagList(string name, ETagType genericType)
		{
			Children = new List<INamedBinaryTag>();
			Name = name;
			GenericType = genericType;
		}

		internal TagList(string name)
		{
			Children = new List<INamedBinaryTag>();
			Name = name;
		}

		public string ToTreeString(int depth = 0)
		{
			string res = "";
			for (int i = 0; i < depth; i++)
			{
				res += "  ";
			}
			res += "{" + TagType.GetNotchName() + "}";

			if (Name != null)
			{
				res += " " + Name + ":";
			}
			res += Environment.NewLine;

			foreach (INamedBinaryTag tag in Children)
			{
				res += tag.ToTreeString(depth + 1) + Environment.NewLine;
			}

			return res.TrimEnd('\n');
		}

		public override string ToString()
		{
			string res = "{" + TagType.GetNotchName() + "} ";
			if (Name != null)
			{
				res += Name + ": ";
			}
			res += Children.Count.ToString() + " Items";

			return res;
		}

		#region Adders

		public TagCompound AddCompound()
		{
			TagCompound t = new TagCompound(null);
			Add(t);
			return t;
		}
		public TagList AddList(ETagType genericType)
		{
			TagList l = new TagList(null, genericType);
			Add(l);
			return l;
		}
		public TagIntArray AddIntArray(IEnumerable<int> filler)
		{
			TagIntArray ia = new TagIntArray(null);
			ia.AddRange(filler);
			Add(ia);
			return ia;
		}
		public TagIntArray AddIntArray(params int[] filler)
		{
			return AddIntArray((IEnumerable<int>)filler);
		}
		public TagByteArray AddByteArray(IEnumerable<byte> filler)
		{
			TagByteArray ba = new TagByteArray(null);
			ba.AddRange(filler);
			Add(ba);
			return ba;
		}
		public TagByteArray AddByteArray(params byte[] filler)
		{
			return AddByteArray((IEnumerable<byte>)filler);
		}
		public TagByteArray AddByteArray(IEnumerable<sbyte> filler)
		{
			TagByteArray ba = new TagByteArray(null);
			ba.AddRange(filler);
			Add(ba);
			return ba;
		}
		public TagByteArray AddByteArray(params sbyte[] filler)
		{
			return AddByteArray((IEnumerable<sbyte>)filler);
		}

		public TagByte AddByte(byte val)
		{
			TagByte b = new TagByte(null, val);
			Add(b);
			return b;
		}
		public TagByte AddByte(sbyte val)
		{
			TagByte b = new TagByte(null, val);
			Add(b);
			return b;
		}
		public TagByte AddByte(bool val)
		{
			TagByte b = new TagByte(null, val);
			Add(b);
			return b;
		}

		public TagShort AddShort(short val)
		{
			TagShort s = new TagShort(null, val);
			Add(s);
			return s;
		}
		public TagInt AddInt(int val)
		{
			TagInt n = new TagInt(null, val);
			Add(n);
			return n;
		}
		public TagLong AddLong(long val)
		{
			TagLong l = new TagLong(null, val);
			Add(l);
			return l;
		}
		public TagFloat AddFloat(float val)
		{
			TagFloat f = new TagFloat(null, val);
			Add(f);
			return f;
		}
		public TagDouble AddDouble(double val)
		{
			TagDouble d = new TagDouble(null, val);
			Add(d);
			return d;
		}
		public TagString AddString(string val)
		{
			TagString s = new TagString(null, val);
			Add(s);
			return s;
		}

		#endregion Adders

		#region IList
		public int IndexOf(INamedBinaryTag item)
		{
			return Children.IndexOf(item);
		}

		public void Insert(int index, INamedBinaryTag item)
		{
			Children.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			Children.RemoveAt(index);
		}

		public void Add(INamedBinaryTag item)
		{
			Children.Add(item);
		}

		public void AddRange(IEnumerable<INamedBinaryTag> items)
		{
			Children.AddRange(items);
		}

		public void AddRange(params INamedBinaryTag[] items)
		{
			Children.AddRange(items);
		}

		public void Clear()
		{
			Children.Clear();
		}

		public bool Contains(INamedBinaryTag item)
		{
			return Children.Contains(item);
		}

		public void CopyTo(INamedBinaryTag[] array, int arrayIndex)
		{
			Children.CopyTo(array, arrayIndex);
		}

		public bool Remove(INamedBinaryTag item)
		{
			return Children.Remove(item);
		}

		public IEnumerator<INamedBinaryTag> GetEnumerator()
		{
			return Children.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion IList

		#region DynamicObject

		public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
		{
			int i = (int)indexes[0];

			if (i < 0 || i >= Count)
			{
				result = null;
				return false;
			}

			result = Children[i].UnderlyingValue;
			return true;
		}

		public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
		{
			int i = (int)indexes[0];

			if (i < 0 || i >= Count)
			{
				return false;
			}

			if (value is INamedBinaryTag)
			{
				INamedBinaryTag nbt = value as INamedBinaryTag;
				if (nbt.TagType != GenericType)
				{
					return false;
				}

				Children[i] = nbt;
				return true;
			} 
			else if (value is byte && GenericType == ETagType.Byte)
			{
				Children[i] = new TagByte(null, (byte)value);
				return true;
			}
			else if (value is sbyte && GenericType == ETagType.Byte)
			{
				Children[i] = new TagByte(null, (sbyte)value);
				return true;
			}
			else if (value is short && GenericType == ETagType.Short)
			{
				Children[i] = new TagShort(null, (short)value);
				return true;
			}
			else if (value is int && GenericType == ETagType.Int)
			{
				Children[i] = new TagInt(null, (int)value);
				return true;
			}
			else if (value is long && GenericType == ETagType.Long)
			{
				Children[i] = new TagLong(null, (long)value);
				return true;
			}
			else if (value is float && GenericType == ETagType.Float)
			{
				Children[i] = new TagFloat(null, (float)value);
				return true;
			}
			else if (value is double && GenericType == ETagType.Double)
			{
				Children[i] = new TagDouble(null, (double)value);
				return true;
			}
			else if (value is string && GenericType == ETagType.String)
			{
				Children[i] = new TagString(null, value as string);
				return true;
			}
			else if (value is IEnumerable<byte> && GenericType == ETagType.Byte_Array)
			{
				TagByteArray ba = new TagByteArray(null);
				ba.AddRange(value as IEnumerable<byte>);
				Children[i] = ba;
				return true;
			}
			else if (value is IEnumerable<sbyte> && GenericType == ETagType.Byte_Array)
			{
				TagByteArray ba = new TagByteArray(null);
				ba.AddRange(value as IEnumerable<sbyte>);
				Children[i] = ba;
				return true;
			}
			else if (value is IEnumerable<int> && GenericType == ETagType.Int_Array)
			{
				TagIntArray ia = new TagIntArray(null);
				ia.AddRange(value as IEnumerable<int>);
				Children[i] = ia;
				return true;
			}
			else if (value is IEnumerable<INamedBinaryTag> && GenericType == ETagType.List)
			{
				IEnumerable<INamedBinaryTag> coll = value as IEnumerable<INamedBinaryTag>;
				TagList list = new TagList(null);
				if (coll.Count() > 0)
				{
					list.GenericType = coll.First().TagType;
				}
				else if (Count > 0 && this.First().TagType == ETagType.List)
				{
					list.GenericType = this.First().TagType;
				}

				list.AddRange(coll);
				this[i] = list;
				return true;
			}
			else
			{
				return false;
			}
		}

		#endregion
	}
}
