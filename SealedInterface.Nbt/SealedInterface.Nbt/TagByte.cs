using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SealedInterface.Nbt.Json;

namespace SealedInterface.Nbt
{
	//[JsonConverter(typeof(IntegerTagConverter))]
	[JsonObject(MemberSerialization.OptIn)]
	public sealed class TagByte : INamedBinaryTag
	{
		public string Name
		{ get; private set; }

		[JsonProperty]
		public sbyte Value
		{ get; set; }

		public object UnderlyingValue => Value;

		public bool Boolean
		{
			get
			{
				return Value != 0;
			}
			set
			{
				Value = (sbyte)(value ? 1 : 0);
			}
		}

		public List<INamedBinaryTag> Children => null;

		public ETagType TagType => ETagType.Byte;

		public TagByte(string name, byte val = 0)
		{
			Name = name;
			Value = unchecked((sbyte)val);
		}
		public TagByte(string name, sbyte val)
		{
			Name = name;
			Value = val;
		}
		public TagByte(string name, bool val)
		{
			Name = name;
			Boolean = val;
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

			res += "0x" + Value.ToString("X2") + " (" + Value.ToString() + ")";

			return res;
		}

		public override string ToString()
		{
			string res = "{" + TagType.GetNotchName() + "} ";
			if (Name != null)
			{
				res += Name + ": ";
			}
			res += "0x" + Value.ToString("X2") + " (" + Value.ToString() + ")";

			return res;
		}
	}
}
