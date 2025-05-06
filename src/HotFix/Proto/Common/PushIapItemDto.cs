using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class PushIapItemDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PushIapItemDto> Parser
		{
			get
			{
				return PushIapItemDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public int ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long STime
		{
			get
			{
				return this.sTime_;
			}
			set
			{
				this.sTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long ETime
		{
			get
			{
				return this.eTime_;
			}
			set
			{
				this.eTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ByTimes
		{
			get
			{
				return this.byTimes_;
			}
			set
			{
				this.byTimes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ItemId
		{
			get
			{
				return this.itemId_;
			}
			set
			{
				this.itemId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.ConfigId);
			}
			if (this.STime != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.STime);
			}
			if (this.ETime != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.ETime);
			}
			if (this.ByTimes != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.ByTimes);
			}
			if (this.ItemId != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.ItemId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ConfigId);
			}
			if (this.STime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.STime);
			}
			if (this.ETime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.ETime);
			}
			if (this.ByTimes != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ByTimes);
			}
			if (this.ItemId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ItemId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 8U)
					{
						this.ConfigId = input.ReadInt32();
						continue;
					}
					if (num == 16U)
					{
						this.STime = input.ReadInt64();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.ETime = input.ReadInt64();
						continue;
					}
					if (num == 32U)
					{
						this.ByTimes = input.ReadInt32();
						continue;
					}
					if (num == 40U)
					{
						this.ItemId = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<PushIapItemDto> _parser = new MessageParser<PushIapItemDto>(() => new PushIapItemDto());

		public const int ConfigIdFieldNumber = 1;

		private int configId_;

		public const int STimeFieldNumber = 2;

		private long sTime_;

		public const int ETimeFieldNumber = 3;

		private long eTime_;

		public const int ByTimesFieldNumber = 4;

		private int byTimes_;

		public const int ItemIdFieldNumber = 5;

		private int itemId_;
	}
}
