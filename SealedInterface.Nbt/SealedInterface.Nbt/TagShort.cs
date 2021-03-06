﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt
{
	public sealed class TagShort : INamedBinaryTag
	{
		public string Name
		{ get; private set; }

		public short Value
		{ get; set; }

		public object UnderlyingValue => Value;

		public ETagType TagType => ETagType.Short;

		public List<INamedBinaryTag> Children => null;

		public TagShort(string name, short val = 0)
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
			res += "{" + TagType.GetNotchName() + "}";

			if (Name != null)
			{
				res += " " + Name + ": ";
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
