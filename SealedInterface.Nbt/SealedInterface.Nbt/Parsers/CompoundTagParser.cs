using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt.Parsers
{
	public sealed class CompoundTagParser : TagParserBase
	{
		public override object ParsePayload(Stream stream, INamedBinaryTag iTag)
		{
			TagCompound tag = iTag as TagCompound;

			while (true)
			{
				byte b = stream.ReadSingleByte();
				ETagType type = (ETagType)b;

				if (type == ETagType.End)
				{
					break;
				}

				string key = ParseName(stream);
				INamedBinaryTag inner = type.MakeTag(key);

				TagParserBase parser = Parsers[type];
				parser.ParsePayload(stream, inner);

				tag.Set(key, inner);
			}

			return tag.Values;
		}

		public override void WritePayload(Stream stream, INamedBinaryTag iTag)
		{
			TagCompound tag = iTag as TagCompound;

			foreach (KeyValuePair<string, INamedBinaryTag> kvp in tag.Values)
			{
				if (kvp.Key != kvp.Value.Name)
				{
					throw new InvalidDataException(string.Format(
						"Dictionary name ({0}) does not match tag name ({1})!", kvp.Key, kvp.Value.Name));
				}

				TagParserBase parser = Parsers[kvp.Value.TagType];
				parser.WriteFullTag(stream, kvp.Value);
			}

			WriteTag(stream, new TagEnd());
		}
	}
}
