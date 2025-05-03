using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Mining
{
	public sealed class BounDrawResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<BounDrawResponse> Parser
		{
			get
			{
				return BounDrawResponse._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Code
		{
			get
			{
				return this.code_;
			}
			set
			{
				this.code_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CommonData CommonData
		{
			get
			{
				return this.commonData_;
			}
			set
			{
				this.commonData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MiningDrawDto DrawInfo
		{
			get
			{
				return this.drawInfo_;
			}
			set
			{
				this.drawInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> ShowRewards
		{
			get
			{
				return this.showRewards_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> RewardsIndex
		{
			get
			{
				return this.rewardsIndex_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.commonData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonData);
			}
			if (this.drawInfo_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.DrawInfo);
			}
			this.showRewards_.WriteTo(output, BounDrawResponse._repeated_showRewards_codec);
			this.rewardsIndex_.WriteTo(output, BounDrawResponse._repeated_rewardsIndex_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			if (this.drawInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.DrawInfo);
			}
			num += this.showRewards_.CalculateSize(BounDrawResponse._repeated_showRewards_codec);
			return num + this.rewardsIndex_.CalculateSize(BounDrawResponse._repeated_rewardsIndex_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 26U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 18U)
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
					if (num == 26U)
					{
						if (this.drawInfo_ == null)
						{
							this.drawInfo_ = new MiningDrawDto();
						}
						input.ReadMessage(this.drawInfo_);
						continue;
					}
				}
				else
				{
					if (num == 34U)
					{
						this.showRewards_.AddEntriesFrom(input, BounDrawResponse._repeated_showRewards_codec);
						continue;
					}
					if (num == 40U || num == 42U)
					{
						this.rewardsIndex_.AddEntriesFrom(input, BounDrawResponse._repeated_rewardsIndex_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<BounDrawResponse> _parser = new MessageParser<BounDrawResponse>(() => new BounDrawResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int DrawInfoFieldNumber = 3;

		private MiningDrawDto drawInfo_;

		public const int ShowRewardsFieldNumber = 4;

		private static readonly FieldCodec<RewardDto> _repeated_showRewards_codec = FieldCodec.ForMessage<RewardDto>(34U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> showRewards_ = new RepeatedField<RewardDto>();

		public const int RewardsIndexFieldNumber = 5;

		private static readonly FieldCodec<int> _repeated_rewardsIndex_codec = FieldCodec.ForInt32(42U);

		private readonly RepeatedField<int> rewardsIndex_ = new RepeatedField<int>();
	}
}
