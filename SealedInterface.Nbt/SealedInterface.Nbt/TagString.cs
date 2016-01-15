using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt
{
	public sealed class TagString : INamedBinaryTag
	{
		public string Name
		{ get; private set; }

		public string Text
		{ get; set; }

		public object UnderlyingValue => Text;

		public ETagType TagType => ETagType.String;

		public TagString(string name, string text = "")
		{
			Name = name;
			Text = text;
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
				res += " " + Name + ": ";
			}

			res += '"' + Text + '"';

			return res;
		}

		public override string ToString()
		{
			string res = "{" + TagType.GetNotchName() + "} ";
			if (Name != null)
			{
				res += Name + ": ";
			}
			res += "'" + Text + "'";

			return res;
		}
	}
}
