﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealedInterface.Nbt.Parsers
{
	public sealed class DoubleTagParser : TagParserBase
	{
		public override object ParsePayload(Stream stream, INamedBinaryTag tagBase)
		{
			byte[] data = new byte[8];
			if (stream.Read(data, 0, 8) < 8)
			{
				throw new EndOfStreamException("End of stream reached inside of tag. Put those bytes back!");
			}

			data = data.ReverseIfLittleEndian();
			double val = BitConverter.ToDouble(data, 0);

			TagDouble tag = tagBase as TagDouble;
			if (tag == null)
			{
				throw new InvalidCastException("Wrong NBT type! Expected TagDouble, found " + tagBase.GetType().Name);
			}
			tag.Value = val;
			return val;
		}

		public override void WritePayload(Stream stream, INamedBinaryTag iTag)
		{
			TagDouble tag = iTag as TagDouble;
			byte[] data = BitConverter.GetBytes(tag.Value);
			data = data.ReverseIfLittleEndian();
			stream.Write(data, 0, 8);
		}
	}
}
