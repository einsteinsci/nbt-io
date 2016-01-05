using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt.Parsers
{
	public sealed class EndTagParser : TagParserBase
	{
		public override object ParsePayload(Stream stream, INamedBinaryTag iTag)
		{
			return null;
		}

		public override string ParseName(Stream stream)
		{
			return null;
		}

		public override void WriteHeader(Stream stream, INamedBinaryTag tag)
		{
			stream.WriteByte((byte)tag.TagType);

			// no name length or name
		}

		public override void WritePayload(Stream stream, INamedBinaryTag iTag)
		{ }
	}
}
