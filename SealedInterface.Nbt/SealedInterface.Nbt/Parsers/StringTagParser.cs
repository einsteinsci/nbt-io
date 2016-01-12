using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt.Parsers
{
	public sealed class StringTagParser : TagParserBase
	{
		public override object ParsePayload(Stream stream, INamedBinaryTag iTag)
		{
			TagString tag = iTag as TagString;
			byte[] count_b = new byte[2];
			stream.Read(count_b, 0, 2);
			count_b = count_b.ReverseIfLittleEndian();
			short len = BitConverter.ToInt16(count_b, 0);

			byte[] data = new byte[len];
			stream.Read(data, 0, len);
			tag.Text = Encoding.UTF8.GetString(data);

			return tag.Text;
		}

		public override void WritePayload(Stream stream, INamedBinaryTag iTag)
		{
			TagString tag = iTag as TagString;
			byte[] data = Encoding.UTF8.GetBytes(tag.Text);

			short len = (short)data.Length;
			byte[] count_b = BitConverter.GetBytes(len);
			count_b = count_b.ReverseIfLittleEndian();
			stream.Write(count_b, 0, 2);

			stream.Write(data, 0, len);
		}
	}
}
