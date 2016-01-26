using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt
{
	public sealed class TagByteArray : INamedBinaryTag, IList<sbyte>
	{
		public List<sbyte> Values
		{ get; private set; }

		public string Name
		{ get; private set; }

		public object UnderlyingValue => this;

		public ETagType TagType => ETagType.Byte_Array;

		public int Count => Values.Count;

		public bool IsReadOnly => ((IList<sbyte>)Values).IsReadOnly;

		public List<INamedBinaryTag> Children
		{
			get
			{
				List<INamedBinaryTag> res = new List<INamedBinaryTag>();
				foreach (sbyte s in Values)
				{
					res.Add(new TagByte(null, s));
				}

				return res;
			}
		}

		public sbyte this[int index]
		{
			get
			{
				return Values[index];
			}

			set
			{
				Values[index] = value;
			}
		}

		public TagByteArray(string name)
		{
			Name = name;
			Values = new List<sbyte>();
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

			foreach (sbyte b in Values)
			{
				for (int i = 0; i < depth + 1; i++)
				{
					res += "  ";
				}
				res += b.ToString("X2") + Environment.NewLine;
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
			res += Values.Count.ToString() + " Bytes";

			return res;
		}

		#region IList
		public int IndexOf(sbyte item)
		{
			return ((IList<sbyte>)Values).IndexOf(item);
		}

		public void Insert(int index, sbyte item)
		{
			((IList<sbyte>)Values).Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			((IList<sbyte>)Values).RemoveAt(index);
		}

		public void Add(sbyte item)
		{
			((IList<sbyte>)Values).Add(item);
		}
		public void Add(byte item)
		{
			Values.Add(unchecked((sbyte)item));
		}

		public void AddRange(IEnumerable<sbyte> vals)
		{
			foreach (sbyte b in vals)
			{
				Add(b);
			}
		}
		public void AddRange(params sbyte[] vals)
		{
			AddRange((IEnumerable<sbyte>)vals);
		}
		public void AddRange(IEnumerable<byte> vals)
		{
			foreach (byte b in vals)
			{
				Add(b);
			}
		}
		public void AddRange(params byte[] vals)
		{
			AddRange((IEnumerable<byte>)vals);
		}

		public void Clear()
		{
			((IList<sbyte>)Values).Clear();
		}

		public bool Contains(sbyte item)
		{
			return ((IList<sbyte>)Values).Contains(item);
		}

		public void CopyTo(sbyte[] array, int arrayIndex)
		{
			((IList<sbyte>)Values).CopyTo(array, arrayIndex);
		}

		public bool Remove(sbyte item)
		{
			return ((IList<sbyte>)Values).Remove(item);
		}

		public IEnumerator<sbyte> GetEnumerator()
		{
			return ((IList<sbyte>)Values).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<sbyte>)Values).GetEnumerator();
		}
		#endregion IList
	}
}
