using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt.Parsers
{
	public class ShortTagParser : TagParserBase
	{
		public override object ParsePayload(Stream stream, INamedBinaryTag tagBase)
		{
			byte[] data = new byte[2];
			if (stream.Read(data, 0, 2) < 2)
			{
				throw new EndOfStreamException("End of stream reached inside of tag. Put those bytes back!");
			}

			data = data.ReverseIfLittleEndian();
			short val = BitConverter.ToInt16(data, 0);

			TagShort tag = tagBase as TagShort;
			if (tag == null)
			{
				throw new InvalidCastException("Wrong NBT type! Expected TagShort, found " + tagBase.GetType().Name);
			}
			tag.Value = val;
			return val;
		}

		public override void WritePayload(Stream stream, INamedBinaryTag iTag)
		{
			TagShort tag = iTag as TagShort;
			byte[] data = BitConverter.GetBytes(tag.Value);
			data = data.ReverseIfLittleEndian();
			stream.Write(data, 0, 2);
		}
	}
}
