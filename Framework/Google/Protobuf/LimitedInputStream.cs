using System;
using System.IO;

namespace Google.Protobuf
{
	internal sealed class LimitedInputStream : Stream
	{
		internal LimitedInputStream(Stream proxied, int size)
		{
			this.proxied = proxied;
			this.bytesLeft = size;
		}

		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		public override void Flush()
		{
		}

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.bytesLeft > 0)
			{
				int num = this.proxied.Read(buffer, offset, Math.Min(this.bytesLeft, count));
				this.bytesLeft -= num;
				return num;
			}
			return 0;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		private readonly Stream proxied;

		private int bytesLeft;
	}
}
