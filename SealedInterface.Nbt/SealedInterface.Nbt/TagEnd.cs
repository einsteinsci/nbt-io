using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt
{
	public class TagEnd : INamedBinaryTag
	{
		public string Name => null;

		public ETagType TagType => ETagType.End;

		public string ToTreeString(int depth = 0)
		{
			return "{END}";
		}

		public override string ToString()
		{
			return ToTreeString();
		}
	}
}
