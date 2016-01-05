using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt.Parsers
{
	public sealed class FloatTagParser : TagParserBase
	{
		public override object ParsePayload(Stream stream, INamedBinaryTag tagBase)
		{
			byte[] data = new byte[4];
			if (stream.Read(data, 0, 4) < 4)
			{
				throw new EndOfStreamException("End of stream reached inside of tag. Put those bytes back!");
			}

			data = data.ReverseIfLittleEndian();
			float val = BitConverter.ToSingle(data, 0);

			TagFloat tag = tagBase as TagFloat;
			if (tag == null)
			{
				throw new InvalidCastException("Wrong NBT type! Expected TagFloat, found " + tagBase.GetType().Name);
			}
			tag.Value = val;
			return val;
		}

		public override void WritePayload(Stream stream, INamedBinaryTag iTag)
		{
			TagFloat tag = iTag as TagFloat;
			byte[] data = BitConverter.GetBytes(tag.Value);
			data = data.ReverseIfLittleEndian();
			stream.Write(data, 0, 4);
		}
	}
}
