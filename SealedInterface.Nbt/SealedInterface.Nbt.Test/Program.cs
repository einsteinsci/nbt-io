using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SealedInterface.Nbt.Json;
using SealedInterface.Nbt.Parsers;

namespace SealedInterface.Nbt.test
{
	public class Program
	{
		const string JSON = @"{""STUFF"":64}";

		public class TestJson
		{
			public TagByte STUFF;
		}

		public static void Main(string[] args)
		{
			ReadJson();

			Console.ReadKey();
		}

		public static void ReadJson()
		{
			TagCompound tag = NbtJsonReader.Parse(JSON);
			Console.WriteLine(tag.ToTreeString());
		}

		public static void ReadRealFile()
		{
			const string LEVEL_DAT_PATH = @"D:\Minecraft\saves\§9Creative Test\playerdata\b9d55d02-b553-4d8d-ac4e-4cc1b151ade9.dat";
			using (GZipStream stream = new GZipStream(new FileStream(LEVEL_DAT_PATH, FileMode.Open), CompressionMode.Decompress))
			{
				TagCompound parsed = NbtIO.Parse(stream);

				Console.WriteLine("NBT Data from '{0}':", LEVEL_DAT_PATH.ToUpper());
				Console.WriteLine();
				Console.WriteLine(parsed.ToTreeString());
			}
		}

		public static void TestFileStream()
		{
			using (FileStream stream = new FileStream("D:\\Test\\level.dat", FileMode.Create))
			{
				TagCompound root = MakeRootTag();

				NbtIO.WriteToStream(stream, root);

				Console.WriteLine("Input NBT:");
				Console.WriteLine(root.ToTreeString());

				Console.WriteLine("\nSaved {0} bytes.", stream.Length);
				Console.WriteLine();
			}

			using (FileStream stream = new FileStream("D:\\Test\\level.dat", FileMode.Open))
			{
				TagCompound parsed = NbtIO.Parse(stream);

				Console.WriteLine("Read NBT:");
				Console.WriteLine(parsed.ToTreeString());
			}
		}

		public static void TestMemoryStream()
		{
			TagCompound root = MakeRootTag();
			
			byte[] data = NbtIO.WriteToBytes(root);

			Console.WriteLine(root.ToTreeString());

			Console.WriteLine("\nSerialized to:");
			Console.WriteLine(ToHex(data));
			Console.WriteLine();

			TagCompound parsed = NbtIO.ParseBytes(data);

			Console.WriteLine(parsed.ToTreeString());
		}

		private static TagCompound MakeRootTag()
		{
			TagList list = new TagList("ZZZZ", ETagType.Compound);
			TagCompound tag = list.AddCompound();
			tag.SetShort("EF", 4095);
			TagCompound tag2 = list.AddCompound();
			tag2.SetFloat("JK", 0.5f);
			tag2.SetString("TXT", "Hello World!");

			list.Add(tag);
			list.Add(tag2);

			dynamic root = new TagCompound("ROOT");
			root.Set(list);
			root.DYN = new int[] { 5, -6 };
			root.ZZZZ[1].TEST = 0.1;
			return root;
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
