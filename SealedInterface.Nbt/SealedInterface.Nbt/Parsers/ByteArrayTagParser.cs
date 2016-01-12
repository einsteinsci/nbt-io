using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt.Parsers
{
	public sealed class ByteArrayTagParser : TagParserBase
	{
		public override object ParsePayload(Stream stream, INamedBinaryTag iTag)
		{
			TagByteArray tag = iTag as TagByteArray;
			byte[] count_b = new byte[4];
			stream.Read(count_b, 0, 4);
			count_b = count_b.ReverseIfLittleEndian();
			int count = BitConverter.ToInt32(count_b, 0);

			for (int i = 0; i < count; i++)
			{
				byte b = stream.ReadSingleByte();
				sbyte sb = unchecked((sbyte)b);
				tag.Add(sb);
			}

			return tag.Values;
		}

		public override void WritePayload(Stream stream, INamedBinaryTag iTag)
		{
			TagByteArray tag = iTag as TagByteArray;
			byte[] count_b = BitConverter.GetBytes(tag.Count);
			count_b = count_b.ReverseIfLittleEndian();
			stream.Write(count_b, 0, 4);

			foreach (sbyte sb in tag)
			{
				byte b = unchecked((byte)sb);
				stream.WriteByte(b);
			}
		}
	}
}
