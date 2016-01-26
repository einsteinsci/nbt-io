using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SealedInterface.Nbt.Json
{
	public sealed class IntegerTagConverter : JsonConverter
	{
		public override bool CanConvert(Type t)
		{
			return t == typeof(TagByte) || t == typeof(TagShort) || t == typeof(TagInt);
		}

		public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
		{
			JObject jobj = JObject.Load(reader);
			List<JProperty> props = jobj.Properties().ToList();
			return null;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
			}

			if (value is TagByte)
			{
				writer.WriteValue((value as TagByte).Value);
			}
			else if (value is TagShort)
			{
				writer.WriteValue((value as TagShort).Value);
			}
			else if (value is TagInt)
			{
				writer.WriteValue((value as TagInt).Value);
			}

			throw new ArgumentException("Can only convert TagByte, TagShort, and TagInt. Found " + value.GetType().FullName + ".");
		}
	}
}
