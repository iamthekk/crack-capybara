using System;
using System.IO;

namespace Google.Protobuf.Compatibility
{
	public static class StreamExtensions
	{
		public static void CopyTo(this Stream source, Stream destination)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			byte[] array = new byte[81920];
			int num;
			while ((num = source.Read(array, 0, array.Length)) > 0)
			{
				destination.Write(array, 0, num);
			}
		}

		private const int BUFFER_SIZE = 81920;
	}
}
