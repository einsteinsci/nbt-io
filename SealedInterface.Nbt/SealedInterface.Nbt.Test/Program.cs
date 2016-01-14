using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SealedInterface.Nbt.Parsers;

namespace SealedInterface.Nbt.test
{
	public class Program
	{
		public static void Main(string[] args)
		{
			MemoryStream stream = new MemoryStream();

			TagCompound tag = new TagCompound("ABCD");
			tag.SetShort("EF", 4095);
			TagCompound tag2 = new TagCompound("GHI");
			tag2.SetFloat("JK", 0.5f);
			tag2.SetString("TXT", "Hello World!");

			TagList list = new TagList("ZZZZ", ETagType.Compound);
			list.Add(tag);
			list.Add(tag2);

			dynamic root = new TagCompound("_");
			root.DYN = new int[] { 5, -6 };
			root.Set(list);

			TagParserBase.WriteTag(stream, root);
			byte[] data = stream.ToArray();

			Console.WriteLine(root.ToTreeString());

			Console.WriteLine("\nSerialized to:");
			Console.WriteLine(ToHex(data));
			Console.WriteLine();

			stream.Position = 0;
			TagCompound parsed = TagParserBase.ParseCompound(stream);
			Console.WriteLine(parsed.ToTreeString());

			Console.ReadKey();
		}

		public static string ToHex(byte[] bytes)
		{
			string res = "";
			foreach (byte b in bytes)
			{
				res += b.ToString("X2") + " ";
			}
			res = res.TrimEnd(' ');

			return res;
		}
	}
}
