using System;
using System.IO;
using System.Text;

namespace Google.Protobuf
{
	public sealed class CodedOutputStream : IDisposable
	{
		public static int ComputeDoubleSize(double value)
		{
			return 8;
		}

		public static int ComputeFloatSize(float value)
		{
			return 4;
		}

		public static int ComputeUInt64Size(ulong value)
		{
			return CodedOutputStream.ComputeRawVarint64Size(value);
		}

		public static int ComputeInt64Size(long value)
		{
			return CodedOutputStream.ComputeRawVarint64Size((ulong)value);
		}

		public static int ComputeInt32Size(int value)
		{
			if (value >= 0)
			{
				return CodedOutputStream.ComputeRawVarint32Size((uint)value);
			}
			return 10;
		}

		public static int ComputeFixed64Size(ulong value)
		{
			return 8;
		}

		public static int ComputeFixed32Size(uint value)
		{
			return 4;
		}

		public static int ComputeBoolSize(bool value)
		{
			return 1;
		}

		public static int ComputeStringSize(string value)
		{
			int byteCount = CodedOutputStream.Utf8Encoding.GetByteCount(value);
			return CodedOutputStream.ComputeLengthSize(byteCount) + byteCount;
		}

		public static int ComputeGroupSize(IMessage value)
		{
			return value.CalculateSize();
		}

		public static int ComputeMessageSize(IMessage value)
		{
			int num = value.CalculateSize();
			return CodedOutputStream.ComputeLengthSize(num) + num;
		}

		public static int ComputeBytesSize(ByteString value)
		{
			return CodedOutputStream.ComputeLengthSize(value.Length) + value.Length;
		}

		public static int ComputeUInt32Size(uint value)
		{
			return CodedOutputStream.ComputeRawVarint32Size(value);
		}

		public static int ComputeEnumSize(int value)
		{
			return CodedOutputStream.ComputeInt32Size(value);
		}

		public static int ComputeSFixed32Size(int value)
		{
			return 4;
		}

		public static int ComputeSFixed64Size(long value)
		{
			return 8;
		}

		public static int ComputeSInt32Size(int value)
		{
			return CodedOutputStream.ComputeRawVarint32Size(CodedOutputStream.EncodeZigZag32(value));
		}

		public static int ComputeSInt64Size(long value)
		{
			return CodedOutputStream.ComputeRawVarint64Size(CodedOutputStream.EncodeZigZag64(value));
		}

		public static int ComputeLengthSize(int length)
		{
			return CodedOutputStream.ComputeRawVarint32Size((uint)length);
		}

		public static int ComputeRawVarint32Size(uint value)
		{
			if ((value & 4294967168U) == 0U)
			{
				return 1;
			}
			if ((value & 4294950912U) == 0U)
			{
				return 2;
			}
			if ((value & 4292870144U) == 0U)
			{
				return 3;
			}
			if ((value & 4026531840U) == 0U)
			{
				return 4;
			}
			return 5;
		}

		public static int ComputeRawVarint64Size(ulong value)
		{
			if ((value & 18446744073709551488UL) == 0UL)
			{
				return 1;
			}
			if ((value & 18446744073709535232UL) == 0UL)
			{
				return 2;
			}
			if ((value & 18446744073707454464UL) == 0UL)
			{
				return 3;
			}
			if ((value & 18446744073441116160UL) == 0UL)
			{
				return 4;
			}
			if ((value & 18446744039349813248UL) == 0UL)
			{
				return 5;
			}
			if ((value & 18446739675663040512UL) == 0UL)
			{
				return 6;
			}
			if ((value & 18446181123756130304UL) == 0UL)
			{
				return 7;
			}
			if ((value & 18374686479671623680UL) == 0UL)
			{
				return 8;
			}
			if ((value & 9223372036854775808UL) == 0UL)
			{
				return 9;
			}
			return 10;
		}

		public static int ComputeTagSize(int fieldNumber)
		{
			return CodedOutputStream.ComputeRawVarint32Size(WireFormat.MakeTag(fieldNumber, WireFormat.WireType.Varint));
		}

		public CodedOutputStream(byte[] flatArray)
			: this(flatArray, 0, flatArray.Length)
		{
		}

		public CodedOutputStream(byte[] buffer, int offset, int length)
		{
			this.output = null;
			this.buffer = buffer;
			this.position = offset;
			this.limit = offset + length;
			this.leaveOpen = true;
		}

		private CodedOutputStream(Stream output, byte[] buffer, bool leaveOpen)
		{
			this.output = ProtoPreconditions.CheckNotNull<Stream>(output, "output");
			this.buffer = buffer;
			this.position = 0;
			this.limit = buffer.Length;
			this.leaveOpen = leaveOpen;
		}

		public CodedOutputStream(Stream output)
			: this(output, CodedOutputStream.DefaultBufferSize, false)
		{
		}

		public CodedOutputStream(Stream output, int bufferSize)
			: this(output, new byte[bufferSize], false)
		{
		}

		public CodedOutputStream(Stream output, bool leaveOpen)
			: this(output, CodedOutputStream.DefaultBufferSize, leaveOpen)
		{
		}

		public CodedOutputStream(Stream output, int bufferSize, bool leaveOpen)
			: this(output, new byte[bufferSize], leaveOpen)
		{
		}

		public long Position
		{
			get
			{
				if (this.output != null)
				{
					return this.output.Position + (long)this.position;
				}
				return (long)this.position;
			}
		}

		public void WriteDouble(double value)
		{
			this.WriteRawLittleEndian64((ulong)BitConverter.DoubleToInt64Bits(value));
		}

		public void WriteFloat(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			if (!BitConverter.IsLittleEndian)
			{
				ByteArray.Reverse(bytes);
			}
			if (this.limit - this.position >= 4)
			{
				byte[] array = this.buffer;
				int num = this.position;
				this.position = num + 1;
				array[num] = bytes[0];
				byte[] array2 = this.buffer;
				num = this.position;
				this.position = num + 1;
				array2[num] = bytes[1];
				byte[] array3 = this.buffer;
				num = this.position;
				this.position = num + 1;
				array3[num] = bytes[2];
				byte[] array4 = this.buffer;
				num = this.position;
				this.position = num + 1;
				array4[num] = bytes[3];
				return;
			}
			this.WriteRawBytes(bytes, 0, 4);
		}

		public void WriteUInt64(ulong value)
		{
			this.WriteRawVarint64(value);
		}

		public void WriteInt64(long value)
		{
			this.WriteRawVarint64((ulong)value);
		}

		public void WriteInt32(int value)
		{
			if (value >= 0)
			{
				this.WriteRawVarint32((uint)value);
				return;
			}
			this.WriteRawVarint64((ulong)((long)value));
		}

		public void WriteFixed64(ulong value)
		{
			this.WriteRawLittleEndian64(value);
		}

		public void WriteFixed32(uint value)
		{
			this.WriteRawLittleEndian32(value);
		}

		public void WriteBool(bool value)
		{
			this.WriteRawByte(value ? 1 : 0);
		}

		public void WriteString(string value)
		{
			int byteCount = CodedOutputStream.Utf8Encoding.GetByteCount(value);
			this.WriteLength(byteCount);
			if (this.limit - this.position >= byteCount)
			{
				if (byteCount == value.Length)
				{
					for (int i = 0; i < byteCount; i++)
					{
						this.buffer[this.position + i] = (byte)value[i];
					}
				}
				else
				{
					CodedOutputStream.Utf8Encoding.GetBytes(value, 0, value.Length, this.buffer, this.position);
				}
				this.position += byteCount;
				return;
			}
			byte[] bytes = CodedOutputStream.Utf8Encoding.GetBytes(value);
			this.WriteRawBytes(bytes);
		}

		public void WriteMessage(IMessage value)
		{
			this.WriteLength(value.CalculateSize());
			value.WriteTo(this);
		}

		public void WriteBytes(ByteString value)
		{
			this.WriteLength(value.Length);
			value.WriteRawBytesTo(this);
		}

		public void WriteUInt32(uint value)
		{
			this.WriteRawVarint32(value);
		}

		public void WriteEnum(int value)
		{
			this.WriteInt32(value);
		}

		public void WriteSFixed32(int value)
		{
			this.WriteRawLittleEndian32((uint)value);
		}

		public void WriteSFixed64(long value)
		{
			this.WriteRawLittleEndian64((ulong)value);
		}

		public void WriteSInt32(int value)
		{
			this.WriteRawVarint32(CodedOutputStream.EncodeZigZag32(value));
		}

		public void WriteSInt64(long value)
		{
			this.WriteRawVarint64(CodedOutputStream.EncodeZigZag64(value));
		}

		public void WriteLength(int length)
		{
			this.WriteRawVarint32((uint)length);
		}

		public void WriteTag(int fieldNumber, WireFormat.WireType type)
		{
			this.WriteRawVarint32(WireFormat.MakeTag(fieldNumber, type));
		}

		public void WriteTag(uint tag)
		{
			this.WriteRawVarint32(tag);
		}

		public void WriteRawTag(byte b1)
		{
			this.WriteRawByte(b1);
		}

		public void WriteRawTag(byte b1, byte b2)
		{
			this.WriteRawByte(b1);
			this.WriteRawByte(b2);
		}

		public void WriteRawTag(byte b1, byte b2, byte b3)
		{
			this.WriteRawByte(b1);
			this.WriteRawByte(b2);
			this.WriteRawByte(b3);
		}

		public void WriteRawTag(byte b1, byte b2, byte b3, byte b4)
		{
			this.WriteRawByte(b1);
			this.WriteRawByte(b2);
			this.WriteRawByte(b3);
			this.WriteRawByte(b4);
		}

		public void WriteRawTag(byte b1, byte b2, byte b3, byte b4, byte b5)
		{
			this.WriteRawByte(b1);
			this.WriteRawByte(b2);
			this.WriteRawByte(b3);
			this.WriteRawByte(b4);
			this.WriteRawByte(b5);
		}

		internal void WriteRawVarint32(uint value)
		{
			if (value < 128U && this.position < this.limit)
			{
				byte[] array = this.buffer;
				int num = this.position;
				this.position = num + 1;
				array[num] = (byte)value;
				return;
			}
			while (value > 127U)
			{
				if (this.position >= this.limit)
				{
					break;
				}
				byte[] array2 = this.buffer;
				int num = this.position;
				this.position = num + 1;
				array2[num] = (byte)((value & 127U) | 128U);
				value >>= 7;
			}
			while (value > 127U)
			{
				this.WriteRawByte((byte)((value & 127U) | 128U));
				value >>= 7;
			}
			if (this.position < this.limit)
			{
				byte[] array3 = this.buffer;
				int num = this.position;
				this.position = num + 1;
				array3[num] = (byte)value;
				return;
			}
			this.WriteRawByte((byte)value);
		}

		internal void WriteRawVarint64(ulong value)
		{
			while (value > 127UL)
			{
				if (this.position >= this.limit)
				{
					break;
				}
				byte[] array = this.buffer;
				int num = this.position;
				this.position = num + 1;
				array[num] = (byte)((value & 127UL) | 128UL);
				value >>= 7;
			}
			while (value > 127UL)
			{
				this.WriteRawByte((byte)((value & 127UL) | 128UL));
				value >>= 7;
			}
			if (this.position < this.limit)
			{
				byte[] array2 = this.buffer;
				int num = this.position;
				this.position = num + 1;
				array2[num] = (byte)value;
				return;
			}
			this.WriteRawByte((byte)value);
		}

		internal void WriteRawLittleEndian32(uint value)
		{
			if (this.position + 4 > this.limit)
			{
				this.WriteRawByte((byte)value);
				this.WriteRawByte((byte)(value >> 8));
				this.WriteRawByte((byte)(value >> 16));
				this.WriteRawByte((byte)(value >> 24));
				return;
			}
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			array[num] = (byte)value;
			byte[] array2 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array2[num] = (byte)(value >> 8);
			byte[] array3 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array3[num] = (byte)(value >> 16);
			byte[] array4 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array4[num] = (byte)(value >> 24);
		}

		internal void WriteRawLittleEndian64(ulong value)
		{
			if (this.position + 8 > this.limit)
			{
				this.WriteRawByte((byte)value);
				this.WriteRawByte((byte)(value >> 8));
				this.WriteRawByte((byte)(value >> 16));
				this.WriteRawByte((byte)(value >> 24));
				this.WriteRawByte((byte)(value >> 32));
				this.WriteRawByte((byte)(value >> 40));
				this.WriteRawByte((byte)(value >> 48));
				this.WriteRawByte((byte)(value >> 56));
				return;
			}
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			array[num] = (byte)value;
			byte[] array2 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array2[num] = (byte)(value >> 8);
			byte[] array3 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array3[num] = (byte)(value >> 16);
			byte[] array4 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array4[num] = (byte)(value >> 24);
			byte[] array5 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array5[num] = (byte)(value >> 32);
			byte[] array6 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array6[num] = (byte)(value >> 40);
			byte[] array7 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array7[num] = (byte)(value >> 48);
			byte[] array8 = this.buffer;
			num = this.position;
			this.position = num + 1;
			array8[num] = (byte)(value >> 56);
		}

		internal void WriteRawByte(byte value)
		{
			if (this.position == this.limit)
			{
				this.RefreshBuffer();
			}
			byte[] array = this.buffer;
			int num = this.position;
			this.position = num + 1;
			array[num] = value;
		}

		internal void WriteRawByte(uint value)
		{
			this.WriteRawByte((byte)value);
		}

		internal void WriteRawBytes(byte[] value)
		{
			this.WriteRawBytes(value, 0, value.Length);
		}

		internal void WriteRawBytes(byte[] value, int offset, int length)
		{
			if (this.limit - this.position >= length)
			{
				ByteArray.Copy(value, offset, this.buffer, this.position, length);
				this.position += length;
				return;
			}
			int num = this.limit - this.position;
			ByteArray.Copy(value, offset, this.buffer, this.position, num);
			offset += num;
			length -= num;
			this.position = this.limit;
			this.RefreshBuffer();
			if (length <= this.limit)
			{
				ByteArray.Copy(value, offset, this.buffer, 0, length);
				this.position = length;
				return;
			}
			this.output.Write(value, offset, length);
		}

		internal static uint EncodeZigZag32(int n)
		{
			return (uint)((n << 1) ^ (n >> 31));
		}

		internal static ulong EncodeZigZag64(long n)
		{
			return (ulong)((n << 1) ^ (n >> 63));
		}

		private void RefreshBuffer()
		{
			if (this.output == null)
			{
				throw new CodedOutputStream.OutOfSpaceException();
			}
			this.output.Write(this.buffer, 0, this.position);
			this.position = 0;
		}

		public void Dispose()
		{
			this.Flush();
			if (!this.leaveOpen)
			{
				this.output.Dispose();
			}
		}

		public void Flush()
		{
			if (this.output != null)
			{
				this.RefreshBuffer();
			}
		}

		public void CheckNoSpaceLeft()
		{
			if (this.SpaceLeft != 0)
			{
				throw new InvalidOperationException("Did not write as much data as expected.");
			}
		}

		public int SpaceLeft
		{
			get
			{
				if (this.output == null)
				{
					return this.limit - this.position;
				}
				throw new InvalidOperationException("SpaceLeft can only be called on CodedOutputStreams that are writing to a flat array.");
			}
		}

		private const int LittleEndian64Size = 8;

		private const int LittleEndian32Size = 4;

		internal static readonly Encoding Utf8Encoding = Encoding.UTF8;

		public static readonly int DefaultBufferSize = 4096;

		private readonly bool leaveOpen;

		private readonly byte[] buffer;

		private readonly int limit;

		private int position;

		private readonly Stream output;

		public sealed class OutOfSpaceException : IOException
		{
			internal OutOfSpaceException()
				: base("CodedOutputStream was writing to a flat byte array and ran out of space.")
			{
			}
		}
	}
}
