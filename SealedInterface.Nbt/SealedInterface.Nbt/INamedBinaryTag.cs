using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt
{
	public interface INamedBinaryTag
	{
		ETagType TagType
		{ get; }

		string Name
		{ get; }

		object UnderlyingValue
		{ get; }

		string ToTreeString(int depth = 0);
	}
}
