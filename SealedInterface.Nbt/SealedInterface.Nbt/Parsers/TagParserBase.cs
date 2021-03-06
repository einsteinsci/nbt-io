﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace SealedInterface.Nbt.Parsers
{
	// singleton
	public abstract class TagParserBase
	{
		public static readonly Dictionary<ETagType, TagParserBase> Parsers = new Dictionary<ETagType, TagParserBase>()
		{
			{ ETagType.End, new EndTagParser() },
			{ ETagType.Byte, new ByteTagParser() },
			{ ETagType.Short, new ShortTagParser() },
			{ ETagType.Int, new IntTagParser() },
			{ ETagType.Long, new LongTagParser() },
			{ ETagType.Float, new FloatTagParser() },
			{ ETagType.Double, new DoubleTagParser() },
			{ ETagType.Byte_Array, new ByteArrayTagParser() },
			{ ETagType.String, new StringTagParser() },
			{ ETagType.List, new ListTagParser() },
			{ ETagType.Compound, new CompoundTagParser() },
			{ ETagType.Int_Array, new IntArrayTagParser() }
		};

		public static void WriteTag(Stream stream, INamedBinaryTag tag)
		{
			Parsers[tag.TagType].WriteFullTag(stream, tag);
		}

		public static TagCompound ParseCompound(Stream stream)
		{
			return Parsers[ETagType.Compound].Parse(stream) as TagCompound;
		}

		public virtual void WriteFullTag(Stream stream, INamedBinaryTag tag)
		{
			WriteHeader(stream, tag);
			WritePayload(stream, tag);
		}

		public virtual void WriteHeader(Stream stream, INamedBinaryTag tag)
		{
			stream.WriteByte((byte)tag.TagType);

			byte[] namebytes = new byte[0];
			short namelen = 0;
			if (tag.TagType != ETagType.End)
			{
				namebytes = Encoding.UTF8.GetBytes(tag.Name);
				namelen = (short)namebytes.Length;
			}
			byte[] namelenbytes = BitConverter.GetBytes(namelen);
			namelenbytes = namelenbytes.ReverseIfLittleEndian();

			stream.Write(namelenbytes, 0, 2);

			if (tag.TagType != ETagType.End)
			{
				stream.Write(namebytes, 0, namelen);
			}
		}

		public abstract void WritePayload(Stream stream, INamedBinaryTag iTag);

		// probably shouldn't override this.
		public virtual INamedBinaryTag Parse(Stream stream)
		{
			byte b = stream.ReadSingleByte();
			ETagType type = (ETagType)b;

			string name = ParseName(stream);
			INamedBinaryTag result = type.MakeTag(name);

			ParsePayload(stream, result);

			return result;
		}

		// returned object is payload, which should be attached to tag.
		public abstract object ParsePayload(Stream stream, INamedBinaryTag iTag);

		public virtual string ParseName(Stream stream)
		{
			byte[] buf = new byte[2];
			stream.Read(buf, 0, 2);
			buf = buf.ReverseIfLittleEndian();
			ushort namelen = BitConverter.ToUInt16(buf, 0); // I'm guessing the length is in bytes, not chars\

			if (namelen > 0)
			{
				buf = new byte[namelen];
				stream.Read(buf, 0, namelen);
				string name = Encoding.UTF8.GetString(buf);

				return name;
			}

			return string.Empty;
		}
	}
}
