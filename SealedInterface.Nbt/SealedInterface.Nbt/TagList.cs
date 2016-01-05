using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt
{
	public sealed class TagList : INamedBinaryTag, IList<INamedBinaryTag>
	{
		public string Name
		{ get; private set; }

		public ETagType TagType => ETagType.List;

		public int Count => Values.Count;

		public bool IsReadOnly => false;

		public readonly List<INamedBinaryTag> Values = new List<INamedBinaryTag>();

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
	}
}
