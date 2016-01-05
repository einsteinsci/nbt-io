using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt
{
	internal static class Util
	{
		internal static byte ReadSingleByte(this Stream stream)
		{
			int n = stream.ReadByte();
			if (n == -1)
			{
				throw new IOException("Could not read byte.");
			}
			byte b = (byte)n;
			return b;
		}

		internal static byte[] ReverseIfLittleEndian(this byte[] original)
		{
			byte[] copy = new byte[original.Length];
			original.CopyTo(copy, 0);

			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(copy);
			}

			return copy;
		}
	}
}
