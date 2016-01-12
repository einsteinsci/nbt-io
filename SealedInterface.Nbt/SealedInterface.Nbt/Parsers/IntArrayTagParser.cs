using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt.Parsers
{
	public sealed class IntArrayTagParser : TagParserBase
	{
		public override object ParsePayload(Stream stream, INamedBinaryTag iTag)
		{
			TagIntArray tag = iTag as TagIntArray;
			byte[] count_bin = new byte[4];
			stream.Read(count_bin, 0, 4);
			count_bin = count_bin.ReverseIfLittleEndian();
			int num_count = BitConverter.ToInt32(count_bin, 0);
			int byte_count = num_count * 4;

			for (int i = 0; i < num_count; i++)
			{
				byte[] num_bin = new byte[4];
				stream.Read(num_bin, 0, 4);
				num_bin = num_bin.ReverseIfLittleEndian();
				int num = BitConverter.ToInt32(num_bin, 0);
				tag.Add(num);
			}

			return tag.Values;
		}

		public override void WritePayload(Stream stream, INamedBinaryTag iTag)
		{
			TagIntArray tag = iTag as TagIntArray;
			byte[] count_bin = BitConverter.GetBytes(tag.Count);
			count_bin = count_bin.ReverseIfLittleEndian();
			stream.Write(count_bin, 0, 4);

			foreach (int n in tag)
			{
				byte[] num_bin = BitConverter.GetBytes(n);
				num_bin = num_bin.ReverseIfLittleEndian();
				stream.Write(num_bin, 0, 4);
			}
		}
	}
}
