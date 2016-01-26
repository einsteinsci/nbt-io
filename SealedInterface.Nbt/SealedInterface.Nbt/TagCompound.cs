using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt
{
	public sealed class TagCompound : DynamicObject, INamedBinaryTag, IEnumerable<INamedBinaryTag>
	{
		public string Name
		{ get; private set; }

		public object UnderlyingValue => this;

		public readonly Dictionary<string, INamedBinaryTag> Values = new Dictionary<string, INamedBinaryTag>();

		public ETagType TagType => ETagType.Compound;

		public List<INamedBinaryTag> Children => Values.Values.ToList();

		public TagCompound(string name)
		{
			Name = name;
		}

		internal void Set(string name, INamedBinaryTag tag)
		{
			if (Values.ContainsKey(name))
			{
				Values[name] = tag;
			}
			else
			{
				Values.Add(name, tag);
			}
		}

		public void Set(INamedBinaryTag tag)
		{
			Set(tag.Name, tag);
		}

		public void SetFromUnderlyingValue(string name, object obj)
		{
			if (obj == null)
			{
				return;
			}

			if (obj is INamedBinaryTag)
			{
				Set(name, obj as INamedBinaryTag);
			}
			else if (obj is byte)
			{
				SetByte(name, (byte)obj);
			}
			else if (obj is sbyte)
			{
				SetByte(name, (sbyte)obj);
			}
			else if (obj is short)
			{
				SetShort(name, (short)obj);
			}
			else if (obj is int)
			{
				SetInt(name, (int)obj);
			}
			else if (obj is long)
			{
				SetLong(name, (long)obj);
			}
			else if (obj is float)
			{
				SetFloat(name, (float)obj);
			}
			else if (obj is double)
			{
				SetDouble(name, (double)obj);
			}
			else if (obj is string)
			{
				SetString(name, obj as string);
			}
			else if (obj is IEnumerable<byte>)
			{
				CreateByteArray(name).AddRange(obj as IEnumerable<byte>);
			}
			else if (obj is IEnumerable<sbyte>)
			{
				CreateByteArray(name).AddRange(obj as IEnumerable<sbyte>);
			}
			else if (obj is IEnumerable<int>)
			{
				CreateIntArray(name).AddRange(obj as IEnumerable<int>);
			}
			else if (obj is IEnumerable<INamedBinaryTag>)
			{
				IEnumerable<INamedBinaryTag> coll = obj as IEnumerable<INamedBinaryTag>;
				TagList list = new TagList(name);
				if (coll.Count() > 0)
				{
					list.GenericType = coll.First().TagType;
				}
				else if (Values.ContainsKey(name) && Values[name] is TagList)
				{
					list.GenericType = (Values[name] as TagList).GenericType;
				}

				list.AddRange(coll);

				Set(list);
			}
			else
			{
				throw new ArgumentException("Invalid NBT type: " + obj.GetType().FullName);
			}
		}

		#region set shortcuts

		public void SetByte(string name, sbyte val)
		{
			Set(name, new TagByte(name, val));
		}
		public void SetByte(string name, byte val)
		{
			Set(name, new TagByte(name, val));
		}
		public void SetByte(string name, bool val)
		{
			Set(name, new TagByte(name, val));
		}

		public void SetShort(string name, short val)
		{
			Set(name, new TagShort(name, val));
		}

		public void SetInt(string name, int val)
		{
			Set(name, new TagInt(name, val));
		}

		public void SetLong(string name, long val)
		{
			Set(name, new TagLong(name, val));
		}

		public void SetFloat(string name, float val)
		{
			Set(name, new TagFloat(name, val));
		}

		public void SetDouble(string name, double val)
		{
			Set(name, new TagDouble(name, val));
		}

		public void SetString(string name, string text)
		{
			Set(name, new TagString(name, text));
		}

		#endregion set shortcuts

		#region get shortcuts

		public sbyte GetByte(string name)
		{
			TagByte b = Get<TagByte>(name);
			return b?.Value ?? 0;
		}
		public byte GetUByte(string name)
		{
			sbyte sb = GetByte(name);
			return unchecked((byte)sb);
		}

		public short GetShort(string name)
		{
			TagShort s = Get<TagShort>(name);
			return s?.Value ?? 0;
		}
		public int GetInt(string name)
		{
			TagInt n = Get<TagInt>(name);
			return n?.Value ?? 0;
		}
		public long GetLong(string name)
		{
			TagLong l = Get<TagLong>(name);
			return l?.Value ?? 0;
		}

		public float GetFloat(string name)
		{
			TagFloat f = Get<TagFloat>(name);
			return f?.Value ?? 0;
		}
		public double GetDouble(string name)
		{
			TagDouble d = Get<TagDouble>(name);
			return d?.Value ?? 0;
		}

		public string GetString(string name)
		{
			TagString s = Get<TagString>(name);
			return s?.Text;
		}

		public TagCompound GetCompound(string name)
		{
			TagCompound tag = Get<TagCompound>(name);
			return tag;
		}

		public TagList GetList(string name)
		{
			TagList list = Get<TagList>(name);
			return list;
		}

		public TagByteArray GetByteArray(string name)
		{
			TagByteArray ba = Get<TagByteArray>(name);
			return ba;
		}

		public TagIntArray GetIntArray(string name)
		{
			TagIntArray ia = Get<TagIntArray>(name);
			return ia;
		}

		#endregion get shortcuts

		public TagCompound CreateCompound(string name)
		{
			TagCompound res = new TagCompound(name);
			Set(name, res);
			return res;
		}

		public TagList CreateList(string name, ETagType type)
		{
			TagList res = new TagList(name, type);
			Set(name, res);
			return res;
		}

		public TagByteArray CreateByteArray(string name)
		{
			TagByteArray ba = new TagByteArray(name);
			Set(name, ba);
			return ba;
		}

		public TagIntArray CreateIntArray(string name)
		{
			TagIntArray ia = new TagIntArray(name);
			Set(name, ia);
			return ia;
		}

		public bool Remove(string name)
		{
			return Values.Remove(name);
		}

		public bool Rename(string oldName, string newName)
		{
			INamedBinaryTag val = Get(oldName);
			if (val == null)
			{
				return false;
			}

			Values.Remove(oldName);
			val = val.TagType.MakeTag(newName);
			Values.Add(newName, val);
			return true;
		}

		public INamedBinaryTag Get(string name)
		{
			if (Values.ContainsKey(name))
			{
				return Values[name];
			}

			return null;
		}

		public T Get<T>(string name)
			where T : class, INamedBinaryTag
		{
			return Get(name) as T;
		}

		public override string ToString()
		{
			string res = "{" + TagType.GetNotchName() + "} ";
			if (Name != null)
			{
				res += Name + ": ";
			}
			res += Values.Count.ToString() + " Members";

			return res;
		}

		// INameBasedTag
		public string ToTreeString(int depth = 0)
		{
			string res = "";
			for (int i = 0; i < depth; i++)
			{
				res += "  ";
			}
			res += "{" + TagType.GetNotchName() + "}";

			if (Name != null)
			{
				res += " " + Name + ":";
			}
			res += Environment.NewLine;

			foreach (INamedBinaryTag tag in Values.Values)
			{
				res += tag.ToTreeString(depth + 1) + Environment.NewLine;
			}

			return res.TrimEnd('\n');
		}

		#region DynamicObject
		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (Values.ContainsKey(binder.Name))
			{
				result = Get(binder.Name).UnderlyingValue;
				return true;
			}

			return base.TryGetMember(binder, out result);
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			SetFromUnderlyingValue(binder.Name, value);
			return true;
		}
		#endregion

		#region IEnumerable<INamedBinaryTag>
		public IEnumerator<INamedBinaryTag> GetEnumerator()
		{
			return Values.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion IEnumerable<INamedBinaryTag>
	}
}
