using System;
using System.IO;

namespace SealedInterface.Nbt.Parsers
{
	public sealed class ByteTagParser : TagParserBase
	{
		public override object ParsePayload(Stream stream, INamedBinaryTag tagBase)
		{
			byte datum = stream.ReadSingleByte();
			sbyte val = (sbyte)datum;
			TagByte tag = tagBase as TagByte;
			if (tag == null)
			{
				throw new InvalidCastException("Wrong NBT type! Expected TagByte, found " + tagBase.GetType().Name);
			}

			tag.Value = val;
			return val;
		}

		public override void WritePayload(Stream stream, INamedBinaryTag itag)
		{
			TagByte tag = itag as TagByte;
			byte datum = unchecked((byte)tag.Value);

			stream.WriteByte(datum);
		}
	}
}