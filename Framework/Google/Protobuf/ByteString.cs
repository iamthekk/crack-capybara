using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Google.Protobuf
{
	public sealed class ByteString : IEnumerable<byte>, IEnumerable, IEquatable<ByteString>
	{
		internal static ByteString AttachBytes(byte[] bytes)
		{
			return new ByteString(bytes);
		}

		private ByteString(byte[] bytes)
		{
			this.bytes = bytes;
		}

		public static ByteString Empty
		{
			get
			{
				return ByteString.empty;
			}
		}

		public int Length
		{
			get
			{
				return this.bytes.Length;
			}
		}

		public bool IsEmpty
		{
			get
			{
				return this.Length == 0;
			}
		}

		public byte[] ToByteArray()
		{
			return (byte[])this.bytes.Clone();
		}

		public string ToBase64()
		{
			return Convert.ToBase64String(this.bytes);
		}

		public static ByteString FromBase64(string bytes)
		{
			if (!(bytes == ""))
			{
				return new ByteString(Convert.FromBase64String(bytes));
			}
			return ByteString.Empty;
		}

		public static ByteString FromStream(Stream stream)
		{
			ProtoPreconditions.CheckNotNull<Stream>(stream, "stream");
			MemoryStream memoryStream = new MemoryStream(stream.CanSeek ? checked((int)(stream.Length - stream.Position)) : 0);
			stream.CopyTo(memoryStream);
			return ByteString.AttachBytes((memoryStream.Length == (long)memoryStream.Capacity) ? memoryStream.GetBuffer() : memoryStream.ToArray());
		}

		public static ByteString CopyFrom(params byte[] bytes)
		{
			return new ByteString((byte[])bytes.Clone());
		}

		public static ByteString CopyFrom(byte[] bytes, int offset, int count)
		{
			byte[] array = new byte[count];
			ByteArray.Copy(bytes, offset, array, 0, count);
			return new ByteString(array);
		}

		public static ByteString CopyFrom(string text, Encoding encoding)
		{
			return new ByteString(encoding.GetBytes(text));
		}

		public static ByteString CopyFromUtf8(string text)
		{
			return ByteString.CopyFrom(text, Encoding.UTF8);
		}

		public byte this[int index]
		{
			get
			{
				return this.bytes[index];
			}
		}

		public string ToString(Encoding encoding)
		{
			return encoding.GetString(this.bytes, 0, this.bytes.Length);
		}

		public string ToStringUtf8()
		{
			return this.ToString(Encoding.UTF8);
		}

		public IEnumerator<byte> GetEnumerator()
		{
			return ((IEnumerable<byte>)this.bytes).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		public CodedInputStream CreateCodedInput()
		{
			return new CodedInputStream(this.bytes);
		}

		public static bool operator ==(ByteString lhs, ByteString rhs)
		{
			if (lhs == rhs)
			{
				return true;
			}
			if (lhs == null || rhs == null)
			{
				return false;
			}
			if (lhs.bytes.Length != rhs.bytes.Length)
			{
				return false;
			}
			for (int i = 0; i < lhs.Length; i++)
			{
				if (rhs.bytes[i] != lhs.bytes[i])
				{
					return false;
				}
			}
			return true;
		}

		public static bool operator !=(ByteString lhs, ByteString rhs)
		{
			return !(lhs == rhs);
		}

		public override bool Equals(object obj)
		{
			return this == obj as ByteString;
		}

		public override int GetHashCode()
		{
			int num = 23;
			foreach (byte b in this.bytes)
			{
				num = num * 31 + (int)b;
			}
			return num;
		}

		public bool Equals(ByteString other)
		{
			return this == other;
		}

		internal void WriteRawBytesTo(CodedOutputStream outputStream)
		{
			outputStream.WriteRawBytes(this.bytes, 0, this.bytes.Length);
		}

		public void CopyTo(byte[] array, int position)
		{
			ByteArray.Copy(this.bytes, 0, array, position, this.bytes.Length);
		}

		public void WriteTo(Stream outputStream)
		{
			outputStream.Write(this.bytes, 0, this.bytes.Length);
		}

		private static readonly ByteString empty = new ByteString(new byte[0]);

		private readonly byte[] bytes;

		internal static class Unsafe
		{
			internal static ByteString FromBytes(byte[] bytes)
			{
				return new ByteString(bytes);
			}

			internal static byte[] GetBuffer(ByteString bytes)
			{
				return bytes.bytes;
			}
		}
	}
}
