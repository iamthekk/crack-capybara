using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class TimeBase : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TimeBase> Parser
		{
			get
			{
				return TimeBase._parser;
			}
		}

		[DebuggerNonUserCode]
		public int ActId
		{
			get
			{
				return this.actId_;
			}
			set
			{
				this.actId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> EntryId
		{
			get
			{
				return this.entryId_;
			}
		}

		[DebuggerNonUserCode]
		public int Round
		{
			get
			{
				return this.round_;
			}
			set
			{
				this.round_ = value;
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
		public long RoundRewardTimes
		{
			get
			{
				return this.roundRewardTimes_;
			}
			set
			{
				this.roundRewardTimes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int RewardRankIndex
		{
			get
			{
				return this.rewardRankIndex_;
			}
			set
			{
				this.rewardRankIndex_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool HasGetRankReward
		{
			get
			{
				return this.hasGetRankReward_;
			}
			set
			{
				this.hasGetRankReward_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ActId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.ActId);
			}
			this.entryId_.WriteTo(output, TimeBase._repeated_entryId_codec);
			if (this.Round != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Round);
			}
			if (this.STime != 0L)
			{
				output.WriteRawTag(40);
				output.WriteInt64(this.STime);
			}
			if (this.ETime != 0L)
			{
				output.WriteRawTag(48);
				output.WriteInt64(this.ETime);
			}
			if (this.RoundRewardTimes != 0L)
			{
				output.WriteRawTag(56);
				output.WriteInt64(this.RoundRewardTimes);
			}
			if (this.RewardRankIndex != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.RewardRankIndex);
			}
			if (this.HasGetRankReward)
			{
				output.WriteRawTag(72);
				output.WriteBool(this.HasGetRankReward);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ActId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ActId);
			}
			num += this.entryId_.CalculateSize(TimeBase._repeated_entryId_codec);
			if (this.Round != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Round);
			}
			if (this.STime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.STime);
			}
			if (this.ETime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.ETime);
			}
			if (this.RoundRewardTimes != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.RoundRewardTimes);
			}
			if (this.RewardRankIndex != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.RewardRankIndex);
			}
			if (this.HasGetRankReward)
			{
				num += 2;
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
							this.ActId = input.ReadInt32();
							continue;
						}
						if (num != 16U)
						{
							goto IL_0046;
						}
					}
					else if (num != 18U)
					{
						if (num != 32U)
						{
							goto IL_0046;
						}
						this.Round = input.ReadInt32();
						continue;
					}
					this.entryId_.AddEntriesFrom(input, TimeBase._repeated_entryId_codec);
					continue;
				}
				if (num <= 48U)
				{
					if (num == 40U)
					{
						this.STime = input.ReadInt64();
						continue;
					}
					if (num == 48U)
					{
						this.ETime = input.ReadInt64();
						continue;
					}
				}
				else
				{
					if (num == 56U)
					{
						this.RoundRewardTimes = input.ReadInt64();
						continue;
					}
					if (num == 64U)
					{
						this.RewardRankIndex = input.ReadInt32();
						continue;
					}
					if (num == 72U)
					{
						this.HasGetRankReward = input.ReadBool();
						continue;
					}
				}
				IL_0046:
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TimeBase> _parser = new MessageParser<TimeBase>(() => new TimeBase());

		public const int ActIdFieldNumber = 1;

		private int actId_;

		public const int EntryIdFieldNumber = 2;

		private static readonly FieldCodec<int> _repeated_entryId_codec = FieldCodec.ForInt32(18U);

		private readonly RepeatedField<int> entryId_ = new RepeatedField<int>();

		public const int RoundFieldNumber = 4;

		private int round_;

		public const int STimeFieldNumber = 5;

		private long sTime_;

		public const int ETimeFieldNumber = 6;

		private long eTime_;

		public const int RoundRewardTimesFieldNumber = 7;

		private long roundRewardTimes_;

		public const int RewardRankIndexFieldNumber = 8;

		private int rewardRankIndex_;

		public const int HasGetRankRewardFieldNumber = 9;

		private bool hasGetRankReward_;
	}
}
