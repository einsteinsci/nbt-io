using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt
{
	public sealed class TagIntArray : INamedBinaryTag, IList<int>
	{
		public List<int> Values
		{ get; private set; }

		public string Name
		{ get; private set; }

		public object UnderlyingValue => this;

		public ETagType TagType => ETagType.Int_Array;

		public int Count => Values.Count;

		public bool IsReadOnly => ((IList<int>)Values).IsReadOnly;

		public List<INamedBinaryTag> Children
		{
			get
			{
				List<INamedBinaryTag> res = new List<INamedBinaryTag>();
				foreach (int n in Values)
				{
					res.Add(new TagInt(null, n));
				}

				return res;
			}
		}

		public int this[int index]
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

		public TagIntArray(string name)
		{
			Name = name;
			Values = new List<int>();
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

			foreach (int n in Values)
			{
				for (int i = 0; i < depth + 1; i++)
				{
					res += "  ";
				}
				res += n.ToString() + Environment.NewLine;
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
			res += Values.Count.ToString() + " Values";

			return res;
		}

		#region IList<int>
		public int IndexOf(int item)
		{
			return ((IList<int>)Values).IndexOf(item);
		}

		public void Insert(int index, int item)
		{
			((IList<int>)Values).Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			((IList<int>)Values).RemoveAt(index);
		}

		public void Add(int item)
		{
			((IList<int>)Values).Add(item);
		}

		public void AddRange(IEnumerable<int> items)
		{
			foreach (int n in items)
			{
				Add(n);
			}
		}
		public void AddRange(params int[] vals)
		{
			AddRange((IEnumerable<int>)vals);
		}

		public void Clear()
		{
			((IList<int>)Values).Clear();
		}

		public bool Contains(int item)
		{
			return ((IList<int>)Values).Contains(item);
		}

		public void CopyTo(int[] array, int arrayIndex)
		{
			((IList<int>)Values).CopyTo(array, arrayIndex);
		}

		public bool Remove(int item)
		{
			return ((IList<int>)Values).Remove(item);
		}

		public IEnumerator<int> GetEnumerator()
		{
			return ((IList<int>)Values).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<int>)Values).GetEnumerator();
		}
		#endregion IList<int>
	}
}
