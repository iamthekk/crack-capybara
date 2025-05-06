using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class TGAInfoDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TGAInfoDto> Parser
		{
			get
			{
				return TGAInfoDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public int TotalLoginDays
		{
			get
			{
				return this.totalLoginDays_;
			}
			set
			{
				this.totalLoginDays_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int TotalPayTimes
		{
			get
			{
				return this.totalPayTimes_;
			}
			set
			{
				this.totalPayTimes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int TotalAdvTimes
		{
			get
			{
				return this.totalAdvTimes_;
			}
			set
			{
				this.totalAdvTimes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long PreLoginTime
		{
			get
			{
				return this.preLoginTime_;
			}
			set
			{
				this.preLoginTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long CreateTime
		{
			get
			{
				return this.createTime_;
			}
			set
			{
				this.createTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public float TotalCharge
		{
			get
			{
				return this.totalCharge_;
			}
			set
			{
				this.totalCharge_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long FirstPayTime
		{
			get
			{
				return this.firstPayTime_;
			}
			set
			{
				this.firstPayTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string TotalChargeStr
		{
			get
			{
				return this.totalChargeStr_;
			}
			set
			{
				this.totalChargeStr_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.TotalLoginDays != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.TotalLoginDays);
			}
			if (this.TotalPayTimes != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.TotalPayTimes);
			}
			if (this.TotalAdvTimes != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.TotalAdvTimes);
			}
			if (this.PreLoginTime != 0L)
			{
				output.WriteRawTag(32);
				output.WriteInt64(this.PreLoginTime);
			}
			if (this.CreateTime != 0L)
			{
				output.WriteRawTag(40);
				output.WriteInt64(this.CreateTime);
			}
			if (this.TotalCharge != 0f)
			{
				output.WriteRawTag(53);
				output.WriteFloat(this.TotalCharge);
			}
			if (this.FirstPayTime != 0L)
			{
				output.WriteRawTag(56);
				output.WriteInt64(this.FirstPayTime);
			}
			if (this.TotalChargeStr.Length != 0)
			{
				output.WriteRawTag(66);
				output.WriteString(this.TotalChargeStr);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.TotalLoginDays != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TotalLoginDays);
			}
			if (this.TotalPayTimes != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TotalPayTimes);
			}
			if (this.TotalAdvTimes != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TotalAdvTimes);
			}
			if (this.PreLoginTime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.PreLoginTime);
			}
			if (this.CreateTime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.CreateTime);
			}
			if (this.TotalCharge != 0f)
			{
				num += 5;
			}
			if (this.FirstPayTime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.FirstPayTime);
			}
			if (this.TotalChargeStr.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.TotalChargeStr);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 32U)
				{
					if (num <= 16U)
					{
						if (num == 8U)
						{
							this.TotalLoginDays = input.ReadInt32();
							continue;
						}
						if (num == 16U)
						{
							this.TotalPayTimes = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.TotalAdvTimes = input.ReadInt32();
							continue;
						}
						if (num == 32U)
						{
							this.PreLoginTime = input.ReadInt64();
							continue;
						}
					}
				}
				else if (num <= 53U)
				{
					if (num == 40U)
					{
						this.CreateTime = input.ReadInt64();
						continue;
					}
					if (num == 53U)
					{
						this.TotalCharge = input.ReadFloat();
						continue;
					}
				}
				else
				{
					if (num == 56U)
					{
						this.FirstPayTime = input.ReadInt64();
						continue;
					}
					if (num == 66U)
					{
						this.TotalChargeStr = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TGAInfoDto> _parser = new MessageParser<TGAInfoDto>(() => new TGAInfoDto());

		public const int TotalLoginDaysFieldNumber = 1;

		private int totalLoginDays_;

		public const int TotalPayTimesFieldNumber = 2;

		private int totalPayTimes_;

		public const int TotalAdvTimesFieldNumber = 3;

		private int totalAdvTimes_;

		public const int PreLoginTimeFieldNumber = 4;

		private long preLoginTime_;

		public const int CreateTimeFieldNumber = 5;

		private long createTime_;

		public const int TotalChargeFieldNumber = 6;

		private float totalCharge_;

		public const int FirstPayTimeFieldNumber = 7;

		private long firstPayTime_;

		public const int TotalChargeStrFieldNumber = 8;

		private string totalChargeStr_ = "";
	}
}
