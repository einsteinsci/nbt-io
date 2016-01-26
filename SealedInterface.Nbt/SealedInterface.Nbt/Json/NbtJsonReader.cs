using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SealedInterface.Nbt.Json
{
	public static class NbtJsonReader
	{
		public static TagCompound Parse(string text)
		{
			TagCompound res = null;

			using (JsonReader reader = new JsonTextReader(new StringReader(text)))
			{
				if (!reader.Read())
				{
					throw new JsonSerializationException("No tokens found in string '" + text + "'");
				}

				if (reader.TokenType != JsonToken.StartObject)
				{
					throw new JsonSerializationException("No opening brace found for JSON object.");
				}

				res = ParseCompound(reader);
			}

			return res;
		}

		public static TagCompound ParseCompound(JsonReader reader, string rootName = "")
		{
			TagCompound res = new TagCompound(rootName);
			string tagName = null;
			while (reader.Read())
			{
				if (reader.TokenType == JsonToken.PropertyName)
				{
					tagName = reader.Value as string;
				}

				if (reader.TokenType == JsonToken.Boolean)
				{
					bool b = (bool)reader.Value;
					TagByte tag = new TagByte(tagName, b);
					res.Set(tag);
				}
				else if (reader.TokenType == JsonToken.Integer)
				{
					long l = (long)reader.Value;
					TagLong tag = new TagLong(tagName, l);
					res.Set(tag);
				}
				else if (reader.TokenType == JsonToken.Float)
				{
					double d = (double)reader.Value;
					TagDouble tag = new TagDouble(tagName, d);
					res.Set(tag);
				}
				else if (reader.TokenType == JsonToken.String)
				{
					string s = reader.Value as string;
					TagString tag = new TagString(tagName, s);
					res.Set(tag);
				}
				else if (reader.TokenType == JsonToken.StartObject)
				{
					TagCompound tag = ParseCompound(reader);
					res.Set(tag);
				}
				else if (reader.TokenType == JsonToken.StartArray)
				{
					TagList list = ParseList(reader, tagName);
					res.Set(list);
				}
				else if (reader.TokenType == JsonToken.EndObject)
				{
					return res;
				}
			}

			return res;
		}

		public static TagList ParseList(JsonReader reader, string rootName)
		{
			TagList list = new TagList(rootName);
			bool foundGeneric = false;
			while (reader.Read())
			{
				if (reader.TokenType == JsonToken.Boolean)
				{
					if (!foundGeneric)
					{
						foundGeneric = true;
						list.GenericType = ETagType.Byte;
					}

					bool b = (bool)reader.Value;
					TagByte tag = new TagByte(null, b);
					list.Add(tag);
				}
				else if (reader.TokenType == JsonToken.Integer)
				{
					if (!foundGeneric)
					{
						foundGeneric = true;
						list.GenericType = ETagType.Long;
					}

					long l = (long)reader.Value;
					TagLong tag = new TagLong(null, l);
					list.Add(tag);
				}
				else if (reader.TokenType == JsonToken.Float)
				{
					if (!foundGeneric)
					{
						foundGeneric = true;
						list.GenericType = ETagType.Float;
					}
					else if (list.GenericType == ETagType.Long)
					{
						List<TagDouble> buf = new List<TagDouble>();
						foreach (TagLong tl in list)
						{
							buf.Add(new TagDouble(tl.Name, tl.Value));
						}
						list.Clear();
						list.GenericType = ETagType.Double;
						foreach (TagDouble td in buf)
						{
							list.Add(td);
						}
					}

					double d = (double)reader.Value;
					TagDouble tag = new TagDouble(null, d);
					list.Add(tag);
				}
				else if (reader.TokenType == JsonToken.String)
				{
					if (!foundGeneric)
					{
						foundGeneric = true;
						list.GenericType = ETagType.String;
					}

					string s = reader.Value as string;
					TagString tag = new TagString(null, s);
					list.Add(tag);
				}
				else if (reader.TokenType == JsonToken.StartObject)
				{
					if (!foundGeneric)
					{
						foundGeneric = true;
						list.GenericType = ETagType.Compound;
					}

					TagCompound tag = ParseCompound(reader, null);
					list.Add(tag);
				}
				else if (reader.TokenType == JsonToken.StartArray)
				{
					if (!foundGeneric)
					{
						foundGeneric = true;
						list.GenericType = ETagType.List;
					}

					TagList inner = ParseList(reader, null);
					list.Add(inner);
				}
				else if (reader.TokenType == JsonToken.EndArray)
				{
					return list;
				}
				else
				{
					throw new NotImplementedException("Currently no handling for this type of JSON token: " + reader.TokenType.ToString());
				}
			}

			return list;
		}

		public static void Serialize(JsonWriter writer, TagCompound root)
		{
			foreach (INamedBinaryTag tag in root)
			{
				SerializeTag(writer, tag);
			}
		}

		public static void Serialize(JsonWriter writer, TagList list)
		{
			writer.WriteStartArray();
			foreach (INamedBinaryTag tag in list)
			{
				if (tag == null)
				{
					writer.WriteNull();
				}
				else
				{
					SerializeTag(writer, tag);
				}
			}
			writer.WriteEndArray();
		}

		public static void Serialize(JsonWriter writer, TagByteArray array)
		{
			writer.WriteStartArray();
			foreach (sbyte sb in array)
			{
				writer.WriteValue(sb);
			}
			writer.WriteEndArray();
		}

		public static void Serialize(JsonWriter writer, TagIntArray array)
		{
			writer.WriteStartArray();
			foreach (int n in array)
			{
				writer.WriteValue(n);
			}
			writer.WriteEndArray();
		}

		public static void SerializeTag(JsonWriter writer, INamedBinaryTag tag)
		{
			switch (tag.TagType)
			{
			case ETagType.Byte:
				writer.WritePropertyName(tag.Name, false);
				writer.WriteValue((tag as TagByte).Value);
				break;
			case ETagType.Short:
				writer.WritePropertyName(tag.Name, false);
				writer.WriteValue((tag as TagShort).Value);
				break;
			case ETagType.Int:
				writer.WritePropertyName(tag.Name, false);
				writer.WriteValue((tag as TagInt).Value);
				break;
			case ETagType.Long:
				writer.WritePropertyName(tag.Name, false);
				writer.WriteValue((tag as TagLong).Value);
				break;
			case ETagType.Float:
				writer.WritePropertyName(tag.Name, false);
				writer.WriteValue((tag as TagFloat).Value);
				break;
			case ETagType.Double:
				writer.WritePropertyName(tag.Name, false);
				writer.WriteValue((tag as TagDouble).Value);
				break;
			case ETagType.Byte_Array:
				break;
			case ETagType.String:
				writer.WritePropertyName(tag.Name, false);
				writer.WriteValue((tag as TagString).Text);
				break;
			case ETagType.List:
				Serialize(writer, tag as TagList);
				break;
			case ETagType.Compound:
				Serialize(writer, tag as TagCompound);
				break;
			case ETagType.Int_Array:
				break;
			}
		}
	}
}
