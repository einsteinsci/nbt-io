using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt.Parsers
{
	public sealed class ListTagParser : TagParserBase
	{
		public override object ParsePayload(Stream stream, INamedBinaryTag iTag)
		{
			TagList list = iTag as TagList;

			byte generic_b = stream.ReadSingleByte();
			ETagType generic = (ETagType)generic_b;
			list.GenericType = generic;

			if (generic == ETagType.End)
			{
				throw new Exception("TagList cannot consist End tags.");
			}

			byte[] count_b = new byte[4];
			stream.Read(count_b, 0, 4);
			count_b = count_b.ReverseIfLittleEndian();
			int count = BitConverter.ToInt32(count_b, 0);

			TagParserBase parser = Parsers[generic];

			for (int i = 0; i < count; i++)
			{
				INamedBinaryTag tag = generic.MakeTag(null);
				parser.ParsePayload(stream, tag);
				list.Add(tag);
			}

			return list.Values;
		}

		public override void WritePayload(Stream stream, INamedBinaryTag iTag)
		{
			TagList list = iTag as TagList;
			byte generic = (byte)list.GenericType;
			stream.WriteByte(generic);

			byte[] data = BitConverter.GetBytes(list.Count);
			data = data.ReverseIfLittleEndian();
			stream.Write(data, 0, 4);

			foreach (INamedBinaryTag tag in list)
			{
				TagParserBase parser = Parsers[tag.TagType];
				parser.WritePayload(stream, tag);
			}
		}
	}
}
