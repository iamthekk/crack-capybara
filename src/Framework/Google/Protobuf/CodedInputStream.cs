using System;
using System.Collections.Generic;
using System.IO;
using Framework;

namespace Google.Protobuf
{
	public sealed class CodedInputStream : IDisposable
	{
		public CodedInputStream(byte[] buffer)
			: this(null, ProtoPreconditions.CheckNotNull<byte[]>(buffer, "buffer"), 0, buffer.Length, true)
		{
		}

		public CodedInputStream(byte[] buffer, int offset, int length)
			: this(null, ProtoPreconditions.CheckNotNull<byte[]>(buffer, "buffer"), offset, offset + length, true)
		{
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset", "Offset must be within the buffer");
			}
			if (length < 0 || offset + length > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("length", "Length must be non-negative and within the buffer");
			}
		}

		public CodedInputStream(Stream input)
			: this(input, false)
		{
		}

		public CodedInputStream(Stream input, bool leaveOpen)
			: this(ProtoPreconditions.CheckNotNull<Stream>(input, "input"), new byte[4096], 0, 0, leaveOpen)
		{
		}

		internal CodedInputStream(Stream input, byte[] buffer, int bufferPos, int bufferSize, bool leaveOpen)
		{
			this.input = input;
			this.buffer = buffer;
			this.bufferPos = bufferPos;
			this.bufferSize = bufferSize;
			this.sizeLimit = 67108864;
			this.recursionLimit = 64;
			this.leaveOpen = leaveOpen;
		}

		internal CodedInputStream(Stream input, byte[] buffer, int bufferPos, int bufferSize, int sizeLimit, int recursionLimit, bool leaveOpen)
			: this(input, buffer, bufferPos, bufferSize, leaveOpen)
		{
			if (sizeLimit <= 0)
			{
				throw new ArgumentOutOfRangeException("sizeLimit", "Size limit must be positive");
			}
			if (recursionLimit <= 0)
			{
				throw new ArgumentOutOfRangeException("recursionLimit!", "Recursion limit must be positive");
			}
			this.sizeLimit = sizeLimit;
			this.recursionLimit = recursionLimit;
		}

		public static CodedInputStream CreateWithLimits(Stream input, int sizeLimit, int recursionLimit)
		{
			return new CodedInputStream(input, new byte[4096], 0, 0, sizeLimit, recursionLimit, false);
		}

		public long Position
		{
			get
			{
				if (this.input != null)
				{
					return this.input.Position - (long)(this.bufferSize + this.bufferSizeAfterLimit - this.bufferPos);
				}
				return (long)this.bufferPos;
			}
		}

		internal uint LastTag
		{
			get
			{
				return this.lastTag;
			}
		}

		public int SizeLimit
		{
			get
			{
				return this.sizeLimit;
			}
		}

		public int RecursionLimit
		{
			get
			{
				return this.recursionLimit;
			}
		}

		public void Dispose()
		{
			if (!this.leaveOpen)
			{
				this.input.Dispose();
			}
		}

		internal void CheckReadEndOfStreamTag()
		{
			if (this.lastTag != 0U)
			{
				throw InvalidProtocolBufferException.MoreDataAvailable();
			}
		}

		public uint PeekTag()
		{
			if (this.hasNextTag)
			{
				return this.nextTag;
			}
			uint num = this.lastTag;
			this.nextTag = this.ReadTag();
			this.hasNextTag = true;
			this.lastTag = num;
			return this.nextTag;
		}

		public uint ReadTag()
		{
			if (this.hasNextTag)
			{
				this.lastTag = this.nextTag;
				this.hasNextTag = false;
				return this.lastTag;
			}
			if (this.bufferPos + 2 <= this.bufferSize)
			{
				byte[] array = this.buffer;
				int num = this.bufferPos;
				this.bufferPos = num + 1;
				int num2 = array[num];
				if (num2 < 128)
				{
					this.lastTag = (uint)num2;
				}
				else
				{
					int num3 = num2 & 127;
					byte[] array2 = this.buffer;
					num = this.bufferPos;
					this.bufferPos = num + 1;
					if ((num2 = array2[num]) < 128)
					{
						num3 |= num2 << 7;
						this.lastTag = (uint)num3;
					}
					else
					{
						this.bufferPos -= 2;
						this.lastTag = this.ReadRawVarint32();
					}
				}
			}
			else
			{
				if (this.IsAtEnd)
				{
					this.lastTag = 0U;
					return 0U;
				}
				this.lastTag = this.ReadRawVarint32();
			}
			if (this.lastTag == 0U)
			{
				throw InvalidProtocolBufferException.InvalidTag();
			}
			return this.lastTag;
		}

		public void SkipLastField()
		{
			if (this.lastTag == 0U)
			{
				throw new InvalidOperationException("SkipLastField cannot be called at the end of a stream");
			}
			switch (WireFormat.GetTagWireType(this.lastTag))
			{
			case WireFormat.WireType.Varint:
				this.ReadRawVarint32();
				return;
			case WireFormat.WireType.Fixed64:
				this.ReadFixed64();
				return;
			case WireFormat.WireType.LengthDelimited:
			{
				int num = this.ReadLength();
				this.SkipRawBytes(num);
				return;
			}
			case WireFormat.WireType.StartGroup:
				this.SkipGroup(this.lastTag);
				return;
			case WireFormat.WireType.EndGroup:
				throw new InvalidProtocolBufferException("SkipLastField called on an end-group tag, indicating that the corresponding start-group was missing");
			case WireFormat.WireType.Fixed32:
				this.ReadFixed32();
				return;
			default:
				return;
			}
		}

		private void SkipGroup(uint startGroupTag)
		{
			this.recursionDepth++;
			if (this.recursionDepth >= this.recursionLimit)
			{
				throw InvalidProtocolBufferException.RecursionLimitExceeded();
			}
			uint num;
			for (;;)
			{
				num = this.ReadTag();
				if (num == 0U)
				{
					break;
				}
				if (WireFormat.GetTagWireType(num) == WireFormat.WireType.EndGroup)
				{
					goto IL_0043;
				}
				this.SkipLastField();
			}
			throw InvalidProtocolBufferException.TruncatedMessage();
			IL_0043:
			int tagFieldNumber = WireFormat.GetTagFieldNumber(startGroupTag);
			int tagFieldNumber2 = WireFormat.GetTagFieldNumber(num);
			if (tagFieldNumber != tagFieldNumber2)
			{
				throw new InvalidProtocolBufferException("Mismatched end-group tag. Started with field " + tagFieldNumber.ToString() + "; ended with field " + tagFieldNumber2.ToString());
			}
			this.recursionDepth--;
		}

		public double ReadDouble()
		{
			return BitConverter.Int64BitsToDouble((long)this.ReadRawLittleEndian64());
		}

		public float ReadFloat()
		{
			if (BitConverter.IsLittleEndian && 4 <= this.bufferSize - this.bufferPos)
			{
				float num = BitConverter.ToSingle(this.buffer, this.bufferPos);
				this.bufferPos += 4;
				return num;
			}
			byte[] array = this.ReadRawBytes(4);
			if (!BitConverter.IsLittleEndian)
			{
				ByteArray.Reverse(array);
			}
			return BitConverter.ToSingle(array, 0);
		}

		public ulong ReadUInt64()
		{
			return this.ReadRawVarint64();
		}

		public long ReadInt64()
		{
			return (long)this.ReadRawVarint64();
		}

		public int ReadInt32()
		{
			return (int)this.ReadRawVarint32();
		}

		public ulong ReadFixed64()
		{
			return this.ReadRawLittleEndian64();
		}

		public uint ReadFixed32()
		{
			return this.ReadRawLittleEndian32();
		}

		public bool ReadBool()
		{
			return this.ReadRawVarint32() > 0U;
		}

		public string ReadString()
		{
			int num = this.ReadLength();
			if (num == 0)
			{
				return "";
			}
			if (num <= this.bufferSize - this.bufferPos)
			{
				string @string = CodedOutputStream.Utf8Encoding.GetString(this.buffer, this.bufferPos, num);
				this.bufferPos += num;
				return @string;
			}
			return CodedOutputStream.Utf8Encoding.GetString(this.ReadRawBytes(num), 0, num);
		}

		public void ReadMessage(IMessage builder)
		{
			int num = this.ReadLength();
			if (this.recursionDepth >= this.recursionLimit)
			{
				throw InvalidProtocolBufferException.RecursionLimitExceeded();
			}
			int num2 = this.PushLimit(num);
			this.recursionDepth++;
			builder.MergeFrom(this);
			GameApp.NetWork.HandleCommonData(builder);
			this.CheckReadEndOfStreamTag();
			if (!this.ReachedLimit)
			{
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			this.recursionDepth--;
			this.PopLimit(num2);
		}

		public ByteString ReadBytes()
		{
			int num = this.ReadLength();
			if (num <= this.bufferSize - this.bufferPos && num > 0)
			{
				ByteString byteString = ByteString.CopyFrom(this.buffer, this.bufferPos, num);
				this.bufferPos += num;
				return byteString;
			}
			return ByteString.AttachBytes(this.ReadRawBytes(num));
		}

		public uint ReadUInt32()
		{
			return this.ReadRawVarint32();
		}

		public int ReadEnum()
		{
			return (int)this.ReadRawVarint32();
		}

		public int ReadSFixed32()
		{
			return (int)this.ReadRawLittleEndian32();
		}

		public long ReadSFixed64()
		{
			return (long)this.ReadRawLittleEndian64();
		}

		public int ReadSInt32()
		{
			return CodedInputStream.DecodeZigZag32(this.ReadRawVarint32());
		}

		public long ReadSInt64()
		{
			return CodedInputStream.DecodeZigZag64(this.ReadRawVarint64());
		}

		public int ReadLength()
		{
			return (int)this.ReadRawVarint32();
		}

		public bool MaybeConsumeTag(uint tag)
		{
			if (this.PeekTag() == tag)
			{
				this.hasNextTag = false;
				return true;
			}
			return false;
		}

		private uint SlowReadRawVarint32()
		{
			int num = (int)this.ReadRawByte();
			if (num < 128)
			{
				return (uint)num;
			}
			int num2 = num & 127;
			if ((num = (int)this.ReadRawByte()) < 128)
			{
				num2 |= num << 7;
			}
			else
			{
				num2 |= (num & 127) << 7;
				if ((num = (int)this.ReadRawByte()) < 128)
				{
					num2 |= num << 14;
				}
				else
				{
					num2 |= (num & 127) << 14;
					if ((num = (int)this.ReadRawByte()) < 128)
					{
						num2 |= num << 21;
					}
					else
					{
						num2 |= (num & 127) << 21;
						num2 |= (num = (int)this.ReadRawByte()) << 28;
						if (num >= 128)
						{
							for (int i = 0; i < 5; i++)
							{
								if (this.ReadRawByte() < 128)
								{
									return (uint)num2;
								}
							}
							throw InvalidProtocolBufferException.MalformedVarint();
						}
					}
				}
			}
			return (uint)num2;
		}

		internal uint ReadRawVarint32()
		{
			if (this.bufferPos + 5 > this.bufferSize)
			{
				return this.SlowReadRawVarint32();
			}
			byte[] array = this.buffer;
			int num = this.bufferPos;
			this.bufferPos = num + 1;
			int num2 = array[num];
			if (num2 < 128)
			{
				return (uint)num2;
			}
			int num3 = num2 & 127;
			byte[] array2 = this.buffer;
			num = this.bufferPos;
			this.bufferPos = num + 1;
			if ((num2 = array2[num]) < 128)
			{
				num3 |= num2 << 7;
			}
			else
			{
				num3 |= (num2 & 127) << 7;
				byte[] array3 = this.buffer;
				num = this.bufferPos;
				this.bufferPos = num + 1;
				if ((num2 = array3[num]) < 128)
				{
					num3 |= num2 << 14;
				}
				else
				{
					num3 |= (num2 & 127) << 14;
					byte[] array4 = this.buffer;
					num = this.bufferPos;
					this.bufferPos = num + 1;
					if ((num2 = array4[num]) < 128)
					{
						num3 |= num2 << 21;
					}
					else
					{
						num3 |= (num2 & 127) << 21;
						int num4 = num3;
						byte[] array5 = this.buffer;
						num = this.bufferPos;
						this.bufferPos = num + 1;
						num3 = num4 | ((num2 = array5[num]) << 28);
						if (num2 >= 128)
						{
							for (int i = 0; i < 5; i++)
							{
								if (this.ReadRawByte() < 128)
								{
									return (uint)num3;
								}
							}
							throw InvalidProtocolBufferException.MalformedVarint();
						}
					}
				}
			}
			return (uint)num3;
		}

		internal static uint ReadRawVarint32(Stream input)
		{
			int num = 0;
			int i;
			for (i = 0; i < 32; i += 7)
			{
				int num2 = input.ReadByte();
				if (num2 == -1)
				{
					throw InvalidProtocolBufferException.TruncatedMessage();
				}
				num |= (num2 & 127) << i;
				if ((num2 & 128) == 0)
				{
					return (uint)num;
				}
			}
			while (i < 64)
			{
				int num3 = input.ReadByte();
				if (num3 == -1)
				{
					throw InvalidProtocolBufferException.TruncatedMessage();
				}
				if ((num3 & 128) == 0)
				{
					return (uint)num;
				}
				i += 7;
			}
			throw InvalidProtocolBufferException.MalformedVarint();
		}

		internal ulong ReadRawVarint64()
		{
			int i = 0;
			ulong num = 0UL;
			while (i < 64)
			{
				byte b = this.ReadRawByte();
				num |= (ulong)((ulong)((long)(b & 127)) << i);
				if ((b & 128) == 0)
				{
					return num;
				}
				i += 7;
			}
			throw InvalidProtocolBufferException.MalformedVarint();
		}

		internal uint ReadRawLittleEndian32()
		{
			uint num = (uint)this.ReadRawByte();
			uint num2 = (uint)this.ReadRawByte();
			uint num3 = (uint)this.ReadRawByte();
			uint num4 = (uint)this.ReadRawByte();
			return num | (num2 << 8) | (num3 << 16) | (num4 << 24);
		}

		internal ulong ReadRawLittleEndian64()
		{
			ulong num = (ulong)this.ReadRawByte();
			ulong num2 = (ulong)this.ReadRawByte();
			ulong num3 = (ulong)this.ReadRawByte();
			ulong num4 = (ulong)this.ReadRawByte();
			ulong num5 = (ulong)this.ReadRawByte();
			ulong num6 = (ulong)this.ReadRawByte();
			ulong num7 = (ulong)this.ReadRawByte();
			ulong num8 = (ulong)this.ReadRawByte();
			return num | (num2 << 8) | (num3 << 16) | (num4 << 24) | (num5 << 32) | (num6 << 40) | (num7 << 48) | (num8 << 56);
		}

		internal static int DecodeZigZag32(uint n)
		{
			return (int)((n >> 1) ^ -(int)(n & 1U));
		}

		internal static long DecodeZigZag64(ulong n)
		{
			return (long)((n >> 1) ^ -(long)(n & 1UL));
		}

		internal int PushLimit(int byteLimit)
		{
			if (byteLimit < 0)
			{
				throw InvalidProtocolBufferException.NegativeSize();
			}
			byteLimit += this.totalBytesRetired + this.bufferPos;
			int num = this.currentLimit;
			if (byteLimit > num)
			{
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			this.currentLimit = byteLimit;
			this.RecomputeBufferSizeAfterLimit();
			return num;
		}

		private void RecomputeBufferSizeAfterLimit()
		{
			this.bufferSize += this.bufferSizeAfterLimit;
			int num = this.totalBytesRetired + this.bufferSize;
			if (num > this.currentLimit)
			{
				this.bufferSizeAfterLimit = num - this.currentLimit;
				this.bufferSize -= this.bufferSizeAfterLimit;
				return;
			}
			this.bufferSizeAfterLimit = 0;
		}

		internal void PopLimit(int oldLimit)
		{
			this.currentLimit = oldLimit;
			this.RecomputeBufferSizeAfterLimit();
		}

		internal bool ReachedLimit
		{
			get
			{
				return this.currentLimit != int.MaxValue && this.totalBytesRetired + this.bufferPos >= this.currentLimit;
			}
		}

		public bool IsAtEnd
		{
			get
			{
				return this.bufferPos == this.bufferSize && !this.RefillBuffer(false);
			}
		}

		private bool RefillBuffer(bool mustSucceed)
		{
			if (this.bufferPos < this.bufferSize)
			{
				throw new InvalidOperationException("RefillBuffer() called when buffer wasn't empty.");
			}
			if (this.totalBytesRetired + this.bufferSize == this.currentLimit)
			{
				if (mustSucceed)
				{
					throw InvalidProtocolBufferException.TruncatedMessage();
				}
				return false;
			}
			else
			{
				this.totalBytesRetired += this.bufferSize;
				this.bufferPos = 0;
				this.bufferSize = ((this.input == null) ? 0 : this.input.Read(this.buffer, 0, this.buffer.Length));
				if (this.bufferSize < 0)
				{
					throw new InvalidOperationException("Stream.Read returned a negative count");
				}
				if (this.bufferSize == 0)
				{
					if (mustSucceed)
					{
						throw InvalidProtocolBufferException.TruncatedMessage();
					}
					return false;
				}
				else
				{
					this.RecomputeBufferSizeAfterLimit();
					int num = this.totalBytesRetired + this.bufferSize + this.bufferSizeAfterLimit;
					if (num > this.sizeLimit || num < 0)
					{
						throw InvalidProtocolBufferException.SizeLimitExceeded();
					}
					return true;
				}
			}
		}

		internal byte ReadRawByte()
		{
			if (this.bufferPos == this.bufferSize)
			{
				this.RefillBuffer(true);
			}
			byte[] array = this.buffer;
			int num = this.bufferPos;
			this.bufferPos = num + 1;
			return array[num];
		}

		internal byte[] ReadRawBytes(int size)
		{
			if (size < 0)
			{
				throw InvalidProtocolBufferException.NegativeSize();
			}
			if (this.totalBytesRetired + this.bufferPos + size > this.currentLimit)
			{
				this.SkipRawBytes(this.currentLimit - this.totalBytesRetired - this.bufferPos);
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			if (size <= this.bufferSize - this.bufferPos)
			{
				byte[] array = new byte[size];
				ByteArray.Copy(this.buffer, this.bufferPos, array, 0, size);
				this.bufferPos += size;
				return array;
			}
			if (size < this.buffer.Length)
			{
				byte[] array2 = new byte[size];
				int num = this.bufferSize - this.bufferPos;
				ByteArray.Copy(this.buffer, this.bufferPos, array2, 0, num);
				this.bufferPos = this.bufferSize;
				this.RefillBuffer(true);
				while (size - num > this.bufferSize)
				{
					Buffer.BlockCopy(this.buffer, 0, array2, num, this.bufferSize);
					num += this.bufferSize;
					this.bufferPos = this.bufferSize;
					this.RefillBuffer(true);
				}
				ByteArray.Copy(this.buffer, 0, array2, num, size - num);
				this.bufferPos = size - num;
				return array2;
			}
			int num2 = this.bufferPos;
			int num3 = this.bufferSize;
			this.totalBytesRetired += this.bufferSize;
			this.bufferPos = 0;
			this.bufferSize = 0;
			int i = size - (num3 - num2);
			List<byte[]> list = new List<byte[]>();
			while (i > 0)
			{
				byte[] array3 = new byte[Math.Min(i, this.buffer.Length)];
				int num4;
				for (int j = 0; j < array3.Length; j += num4)
				{
					num4 = ((this.input == null) ? (-1) : this.input.Read(array3, j, array3.Length - j));
					if (num4 <= 0)
					{
						throw InvalidProtocolBufferException.TruncatedMessage();
					}
					this.totalBytesRetired += num4;
				}
				i -= array3.Length;
				list.Add(array3);
			}
			byte[] array4 = new byte[size];
			int num5 = num3 - num2;
			ByteArray.Copy(this.buffer, num2, array4, 0, num5);
			foreach (byte[] array5 in list)
			{
				Buffer.BlockCopy(array5, 0, array4, num5, array5.Length);
				num5 += array5.Length;
			}
			return array4;
		}

		private void SkipRawBytes(int size)
		{
			if (size < 0)
			{
				throw InvalidProtocolBufferException.NegativeSize();
			}
			if (this.totalBytesRetired + this.bufferPos + size > this.currentLimit)
			{
				this.SkipRawBytes(this.currentLimit - this.totalBytesRetired - this.bufferPos);
				throw InvalidProtocolBufferException.TruncatedMessage();
			}
			if (size <= this.bufferSize - this.bufferPos)
			{
				this.bufferPos += size;
				return;
			}
			int num = this.bufferSize - this.bufferPos;
			this.totalBytesRetired += this.bufferSize;
			this.bufferPos = 0;
			this.bufferSize = 0;
			if (num < size)
			{
				if (this.input == null)
				{
					throw InvalidProtocolBufferException.TruncatedMessage();
				}
				this.SkipImpl(size - num);
				this.totalBytesRetired += size - num;
			}
		}

		private void SkipImpl(int amountToSkip)
		{
			if (this.input.CanSeek)
			{
				long position = this.input.Position;
				this.input.Position += (long)amountToSkip;
				if (this.input.Position != position + (long)amountToSkip)
				{
					throw InvalidProtocolBufferException.TruncatedMessage();
				}
			}
			else
			{
				byte[] array = new byte[Math.Min(1024, amountToSkip)];
				while (amountToSkip > 0)
				{
					int num = this.input.Read(array, 0, Math.Min(array.Length, amountToSkip));
					if (num <= 0)
					{
						throw InvalidProtocolBufferException.TruncatedMessage();
					}
					amountToSkip -= num;
				}
			}
		}

		private readonly bool leaveOpen;

		private readonly byte[] buffer;

		private int bufferSize;

		private int bufferSizeAfterLimit;

		private int bufferPos;

		private readonly Stream input;

		private uint lastTag;

		private uint nextTag;

		private bool hasNextTag;

		internal const int DefaultRecursionLimit = 64;

		internal const int DefaultSizeLimit = 67108864;

		internal const int BufferSize = 4096;

		private int totalBytesRetired;

		private int currentLimit = int.MaxValue;

		private int recursionDepth;

		private readonly int recursionLimit;

		private readonly int sizeLimit;
	}
}
