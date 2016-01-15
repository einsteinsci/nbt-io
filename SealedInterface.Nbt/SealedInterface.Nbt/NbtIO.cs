using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SealedInterface.Nbt.Parsers;

namespace SealedInterface.Nbt
{
	public static class NbtIO
	{
		public static TagCompound ParseBytes(byte[] rawData)
		{
			TagCompound res = null;

			using (MemoryStream stream = new MemoryStream(rawData, false))
			{
				res = Parse(stream);
			}

			return res;
		}
		public static T ParseBytes<T>(byte[] rawData)
			where T : class, INamedBinaryTag
		{
			return ParseBytes(rawData, typeof(T)) as T;
		}
		public static INamedBinaryTag ParseBytes(byte[] rawData, Type t)
		{
			return ParseBytes(rawData, TagTypes.GetETagTypeFromType(t));
		}
		public static INamedBinaryTag ParseBytes(byte[] rawData, ETagType rootTagType)
		{
			INamedBinaryTag nbt = null;

			using (MemoryStream stream = new MemoryStream(rawData, false))
			{
				nbt = TagParserBase.Parsers[rootTagType].Parse(stream);
			}

			return nbt;
		}

		public static TagCompound Parse(Stream stream)
		{
			return TagParserBase.ParseCompound(stream);
		}
		public static T Parse<T>(Stream stream)
			where T : class, INamedBinaryTag
		{
			return Parse(stream, typeof(T)) as T;
		}
		public static INamedBinaryTag Parse(Stream stream, Type t)
		{
			return Parse(stream, TagTypes.GetETagTypeFromType(t));
		}
		public static INamedBinaryTag Parse(Stream stream, ETagType rootTagType)
		{
			return TagParserBase.Parsers[rootTagType].Parse(stream);
		}

		public static byte[] WriteToBytes(INamedBinaryTag nbt)
		{
			byte[] result = new byte[0];
			using (MemoryStream stream = new MemoryStream())
			{
				TagParserBase.WriteTag(stream, nbt);
				result = stream.ToArray();
			}
			
			return result;
		}

		public static void WriteToStream(Stream stream, INamedBinaryTag nbt)
		{
			TagParserBase.WriteTag(stream, nbt);
		}
	}
}
