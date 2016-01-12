using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt
{
	public sealed class TagCompound : INamedBinaryTag
	{
		public string Name
		{ get; private set; }

		public readonly Dictionary<string, INamedBinaryTag> Values = new Dictionary<string, INamedBinaryTag>();

		public ETagType TagType => ETagType.Compound;

		public TagCompound(string name)
		{
			Name = name;
		}

		internal void Set(string name, INamedBinaryTag tag)
		{
			if (Values.ContainsKey(name))
			{
				Values[name] = tag;
			}
			else
			{
				Values.Add(name, tag);
			}
		}

		public void Set(INamedBinaryTag tag)
		{
			Set(tag.Name, tag);
		}

		#region set shortcuts

		public void SetByte(string name, sbyte val)
		{
			Set(name, new TagByte(name, val));
		}
		public void SetByte(string name, byte val)
		{
			Set(name, new TagByte(name, val));
		}
		public void SetByte(string name, bool val)
		{
			Set(name, new TagByte(name, val));
		}

		public void SetShort(string name, short val)
		{
			Set(name, new TagShort(name, val));
		}

		public void SetInt(string name, int val)
		{
			Set(name, new TagInt(name, val));
		}

		public void SetLong(string name, long val)
		{
			Set(name, new TagLong(name, val));
		}

		public void SetFloat(string name, float val)
		{
			Set(name, new TagFloat(name, val));
		}

		public void SetDouble(string name, double val)
		{
			Set(name, new TagDouble(name, val));
		}

		public void SetCompound(TagCompound tag)
		{
			Set(tag.Name, tag);
		}

		public void SetString(string name, string text)
		{
			Set(name, new TagString(name, text));
		}
		
		#endregion set shortcuts

		public void Rename(string oldName, string newName)
		{
			INamedBinaryTag val = Get(oldName);
			Values.Remove(oldName);
			val = val.TagType.MakeTag(newName);
			Values.Add(newName, val);
		}

		public INamedBinaryTag Get(string name)
		{
			if (Values.ContainsKey(name))
			{
				return Values[name];
			}

			return null;
		}

		// INameBasedTag
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

			foreach (INamedBinaryTag tag in Values.Values)
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
			res += Values.Count.ToString() + " Members";

			return res;
		}
	}
}
