using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt
{
	public sealed class TagDouble : INamedBinaryTag
	{
		public double Value
		{ get; set; }

		public string Name
		{ get; private set; }

		public object UnderlyingValue => Value;

		public ETagType TagType => ETagType.Double;

		public List<INamedBinaryTag> Children => null;

		public TagDouble(string name, double val = 0)
		{
			Name = name;
			Value = val;
		}

		public string ToTreeString(int depth = 0)
		{
			string res = "";
			for (int i = 0; i < depth; i++)
			{
				res += "  ";
			}
			res += "{" + TagType.GetNotchName() + "} ";

			if (Name != null)
			{
				res += Name + ": ";
			}

			res += Value.ToString();

			return res;
		}

		public override string ToString()
		{
			string res = "{" + TagType.GetNotchName() + "} ";
			if (Name != null)
			{
				res += Name + ": ";
			}
			res += Value.ToString();

			return res;
		}
	}
}
