using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class IAPBattlePassDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<IAPBattlePassDto> Parser
		{
			get
			{
				return IAPBattlePassDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint BattlePassId
		{
			get
			{
				return this.battlePassId_;
			}
			set
			{
				this.battlePassId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Buy
		{
			get
			{
				return this.buy_;
			}
			set
			{
				this.buy_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Score
		{
			get
			{
				return this.score_;
			}
			set
			{
				this.score_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<uint> FreeRewardIdList
		{
			get
			{
				return this.freeRewardIdList_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<uint> BattlePassRewardIdList
		{
			get
			{
				return this.battlePassRewardIdList_;
			}
		}

		[DebuggerNonUserCode]
		public uint CanRewardFinalCount
		{
			get
			{
				return this.canRewardFinalCount_;
			}
			set
			{
				this.canRewardFinalCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint RewardFinalCount
		{
			get
			{
				return this.rewardFinalCount_;
			}
			set
			{
				this.rewardFinalCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.BattlePassId != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.BattlePassId);
			}
			if (this.Buy != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Buy);
			}
			if (this.Score != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Score);
			}
			this.freeRewardIdList_.WriteTo(output, IAPBattlePassDto._repeated_freeRewardIdList_codec);
			this.battlePassRewardIdList_.WriteTo(output, IAPBattlePassDto._repeated_battlePassRewardIdList_codec);
			if (this.CanRewardFinalCount != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.CanRewardFinalCount);
			}
			if (this.RewardFinalCount != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.RewardFinalCount);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.BattlePassId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BattlePassId);
			}
			if (this.Buy != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Buy);
			}
			if (this.Score != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Score);
			}
			num += this.freeRewardIdList_.CalculateSize(IAPBattlePassDto._repeated_freeRewardIdList_codec);
			num += this.battlePassRewardIdList_.CalculateSize(IAPBattlePassDto._repeated_battlePassRewardIdList_codec);
			if (this.CanRewardFinalCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.CanRewardFinalCount);
			}
			if (this.RewardFinalCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.RewardFinalCount);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num > 32U)
				{
					if (num <= 40U)
					{
						if (num == 34U)
						{
							goto IL_0078;
						}
						if (num != 40U)
						{
							goto IL_0046;
						}
					}
					else if (num != 42U)
					{
						if (num == 48U)
						{
							this.CanRewardFinalCount = input.ReadUInt32();
							continue;
						}
						if (num != 56U)
						{
							goto IL_0046;
						}
						this.RewardFinalCount = input.ReadUInt32();
						continue;
					}
					this.battlePassRewardIdList_.AddEntriesFrom(input, IAPBattlePassDto._repeated_battlePassRewardIdList_codec);
					continue;
				}
				if (num <= 16U)
				{
					if (num == 8U)
					{
						this.BattlePassId = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.Buy = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Score = input.ReadUInt32();
						continue;
					}
					if (num == 32U)
					{
						goto IL_0078;
					}
				}
				IL_0046:
				input.SkipLastField();
				continue;
				IL_0078:
				this.freeRewardIdList_.AddEntriesFrom(input, IAPBattlePassDto._repeated_freeRewardIdList_codec);
			}
		}

		private static readonly MessageParser<IAPBattlePassDto> _parser = new MessageParser<IAPBattlePassDto>(() => new IAPBattlePassDto());

		public const int BattlePassIdFieldNumber = 1;

		private uint battlePassId_;

		public const int BuyFieldNumber = 2;

		private uint buy_;

		public const int ScoreFieldNumber = 3;

		private uint score_;

		public const int FreeRewardIdListFieldNumber = 4;

		private static readonly FieldCodec<uint> _repeated_freeRewardIdList_codec = FieldCodec.ForUInt32(34U);

		private readonly RepeatedField<uint> freeRewardIdList_ = new RepeatedField<uint>();

		public const int BattlePassRewardIdListFieldNumber = 5;

		private static readonly FieldCodec<uint> _repeated_battlePassRewardIdList_codec = FieldCodec.ForUInt32(42U);

		private readonly RepeatedField<uint> battlePassRewardIdList_ = new RepeatedField<uint>();

		public const int CanRewardFinalCountFieldNumber = 6;

		private uint canRewardFinalCount_;

		public const int RewardFinalCountFieldNumber = 7;

		private uint rewardFinalCount_;
	}
}
