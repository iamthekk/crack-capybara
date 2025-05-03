using System;

namespace Google.Protobuf
{
	internal static class ByteArray
	{
		internal static void Copy(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count)
		{
			if (count > 12)
			{
				Buffer.BlockCopy(src, srcOffset, dst, dstOffset, count);
				return;
			}
			int num = srcOffset + count;
			for (int i = srcOffset; i < num; i++)
			{
				dst[dstOffset++] = src[i];
			}
		}

		internal static void Reverse(byte[] bytes)
		{
			int i = 0;
			int num = bytes.Length - 1;
			while (i < num)
			{
				byte b = bytes[i];
				bytes[i] = bytes[num];
				bytes[num] = b;
				i++;
				num--;
			}
		}

		private const int CopyThreshold = 12;
	}
}
