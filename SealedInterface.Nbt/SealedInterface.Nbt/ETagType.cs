﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt
{
	public enum ETagType : byte
	{
		End = 0,
		Byte = 1,
		Short = 2,
		Int = 3,
		Long = 4,
		Float = 5,
		Double = 6,
		Byte_Array = 7,
		String = 8,
		List = 9,
		Compound = 10,
		Int_Array = 11,
	}

	public static class TagTypes
	{
		public static Type GetUnderlyingType(this ETagType tagType)
		{
			switch (tagType)
			{
			case ETagType.End:
				return typeof(void);
			case ETagType.Byte:
				return typeof(sbyte);
			case ETagType.Short:
				return typeof(short);
			case ETagType.Int:
				return typeof(int);
			case ETagType.Long:
				return typeof(long);
			case ETagType.Float:
				return typeof(float);
			case ETagType.Double:
				return typeof(double);
			case ETagType.Byte_Array:
				return typeof(byte[]);
			case ETagType.String:
				return typeof(string);
			case ETagType.List:
				return typeof(object[]);
			case ETagType.Compound:
				return typeof(Dictionary<string, INamedBinaryTag>);
			case ETagType.Int_Array:
				return typeof(int[]);
			default:
				return null;
			}
		}

		public static string GetNotchName(this ETagType type)
		{
			return "TAG_" + type.ToString();
		}

		public static INamedBinaryTag MakeTag(this ETagType type, string name)
		{
			switch (type)
			{
			case ETagType.End:
				return new TagEnd();
			case ETagType.Byte:
				return new TagByte(name);
			case ETagType.Short:
				return new TagShort(name);
			case ETagType.Int:
				return new TagInt(name);
			case ETagType.Long:
				return new TagLong(name);
			case ETagType.Float:
				return new TagFloat(name);
			case ETagType.Double:
				return new TagDouble(name);
			case ETagType.Byte_Array:
				return null;
			case ETagType.String:
				return null;
			case ETagType.List:
				return new TagList(name);
			case ETagType.Compound:
				return new TagCompound(name);
			case ETagType.Int_Array:
				return null;
			default:
				return null;
			}
		}
	}
}
