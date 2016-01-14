using System;
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

		public int Count => Values.Count;

		public bool IsReadOnly => false;

		public readonly List<INamedBinaryTag> Values = new List<INamedBinaryTag>();

		public object UnderlyingValue => this;

		public ETagType GenericType
		{ get; internal set; }

		public INamedBinaryTag this[int i]
		{
			get
			{
				return Values[i];
			}
			set
			{
				Values[i] = value;
			}
		}

		public TagList(string name, ETagType genericType)
		{
			Name = name;
			GenericType = genericType;
		}

		internal TagList(string name)
		{
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

			foreach (INamedBinaryTag tag in Values)
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
			res += Values.Count.ToString() + " Items";

			return res;
		}

		#region IList
		public int IndexOf(INamedBinaryTag item)
		{
			return Values.IndexOf(item);
		}

		public void Insert(int index, INamedBinaryTag item)
		{
			Values.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			Values.RemoveAt(index);
		}

		public void Add(INamedBinaryTag item)
		{
			Values.Add(item);
		}

		public void AddRange(IEnumerable<INamedBinaryTag> items)
		{
			Values.AddRange(items);
		}

		public void AddRange(params INamedBinaryTag[] items)
		{
			Values.AddRange(items);
		}

		public void Clear()
		{
			Values.Clear();
		}

		public bool Contains(INamedBinaryTag item)
		{
			return Values.Contains(item);
		}

		public void CopyTo(INamedBinaryTag[] array, int arrayIndex)
		{
			Values.CopyTo(array, arrayIndex);
		}

		public bool Remove(INamedBinaryTag item)
		{
			return Values.Remove(item);
		}

		public IEnumerator<INamedBinaryTag> GetEnumerator()
		{
			return Values.GetEnumerator();
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

			result = Values[i].UnderlyingValue;
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

				Values[i] = nbt;
				return true;
			} 
			else if (value is byte && GenericType == ETagType.Byte)
			{
				Values[i] = new TagByte(null, (byte)value);
				return true;
			}
			else if (value is sbyte && GenericType == ETagType.Byte)
			{
				Values[i] = new TagByte(null, (sbyte)value);
				return true;
			}
			else if (value is short && GenericType == ETagType.Short)
			{
				Values[i] = new TagShort(null, (short)value);
				return true;
			}
			else if (value is int && GenericType == ETagType.Int)
			{
				Values[i] = new TagInt(null, (int)value);
				return true;
			}
			else if (value is long && GenericType == ETagType.Long)
			{
				Values[i] = new TagLong(null, (long)value);
				return true;
			}
			else if (value is float && GenericType == ETagType.Float)
			{
				Values[i] = new TagFloat(null, (float)value);
				return true;
			}
			else if (value is double && GenericType == ETagType.Double)
			{
				Values[i] = new TagDouble(null, (double)value);
				return true;
			}
			else if (value is string && GenericType == ETagType.String)
			{
				Values[i] = new TagString(null, value as string);
				return true;
			}
			else if (value is IEnumerable<byte> && GenericType == ETagType.Byte_Array)
			{
				TagByteArray ba = new TagByteArray(null);
				ba.AddRange(value as IEnumerable<byte>);
				Values[i] = ba;
				return true;
			}
			else if (value is IEnumerable<sbyte> && GenericType == ETagType.Byte_Array)
			{
				TagByteArray ba = new TagByteArray(null);
				ba.AddRange(value as IEnumerable<sbyte>);
				Values[i] = ba;
				return true;
			}
			else if (value is IEnumerable<int> && GenericType == ETagType.Int_Array)
			{
				TagIntArray ia = new TagIntArray(null);
				ia.AddRange(value as IEnumerable<int>);
				Values[i] = ia;
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
