using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class IAPMonthCardDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<IAPMonthCardDto> Parser
		{
			get
			{
				return IAPMonthCardDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint ConfigId
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
		public ulong EndTime
		{
			get
			{
				return this.endTime_;
			}
			set
			{
				this.endTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong LastRewardTime
		{
			get
			{
				return this.lastRewardTime_;
			}
			set
			{
				this.lastRewardTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint CanReward
		{
			get
			{
				return this.canReward_;
			}
			set
			{
				this.canReward_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong NextRewardTime
		{
			get
			{
				return this.nextRewardTime_;
			}
			set
			{
				this.nextRewardTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ConfigId != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.ConfigId);
			}
			if (this.EndTime != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.EndTime);
			}
			if (this.LastRewardTime != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.LastRewardTime);
			}
			if (this.CanReward != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.CanReward);
			}
			if (this.NextRewardTime != 0UL)
			{
				output.WriteRawTag(40);
				output.WriteUInt64(this.NextRewardTime);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ConfigId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ConfigId);
			}
			if (this.EndTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.EndTime);
			}
			if (this.LastRewardTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.LastRewardTime);
			}
			if (this.CanReward != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.CanReward);
			}
			if (this.NextRewardTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.NextRewardTime);
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
						this.ConfigId = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.EndTime = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.LastRewardTime = input.ReadUInt64();
						continue;
					}
					if (num == 32U)
					{
						this.CanReward = input.ReadUInt32();
						continue;
					}
					if (num == 40U)
					{
						this.NextRewardTime = input.ReadUInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<IAPMonthCardDto> _parser = new MessageParser<IAPMonthCardDto>(() => new IAPMonthCardDto());

		public const int ConfigIdFieldNumber = 1;

		private uint configId_;

		public const int EndTimeFieldNumber = 2;

		private ulong endTime_;

		public const int LastRewardTimeFieldNumber = 3;

		private ulong lastRewardTime_;

		public const int CanRewardFieldNumber = 4;

		private uint canReward_;

		public const int NextRewardTimeFieldNumber = 5;

		private ulong nextRewardTime_;
	}
}
