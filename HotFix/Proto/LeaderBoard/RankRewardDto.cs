using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.LeaderBoard
{
	public sealed class RankRewardDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RankRewardDto> Parser
		{
			get
			{
				return RankRewardDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Id
		{
			get
			{
				return this.id_;
			}
			set
			{
				this.id_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> RankReward
		{
			get
			{
				return this.rankReward_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> SeasonReward
		{
			get
			{
				return this.seasonReward_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Id != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Id);
			}
			this.rankReward_.WriteTo(output, RankRewardDto._repeated_rankReward_codec);
			this.seasonReward_.WriteTo(output, RankRewardDto._repeated_seasonReward_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Id != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Id);
			}
			num += this.rankReward_.CalculateSize(RankRewardDto._repeated_rankReward_codec);
			return num + this.seasonReward_.CalculateSize(RankRewardDto._repeated_seasonReward_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 18U)
					{
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							this.seasonReward_.AddEntriesFrom(input, RankRewardDto._repeated_seasonReward_codec);
						}
					}
					else
					{
						this.rankReward_.AddEntriesFrom(input, RankRewardDto._repeated_rankReward_codec);
					}
				}
				else
				{
					this.Id = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<RankRewardDto> _parser = new MessageParser<RankRewardDto>(() => new RankRewardDto());

		public const int IdFieldNumber = 1;

		private int id_;

		public const int RankRewardFieldNumber = 2;

		private static readonly FieldCodec<RewardDto> _repeated_rankReward_codec = FieldCodec.ForMessage<RewardDto>(18U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> rankReward_ = new RepeatedField<RewardDto>();

		public const int SeasonRewardFieldNumber = 3;

		private static readonly FieldCodec<RewardDto> _repeated_seasonReward_codec = FieldCodec.ForMessage<RewardDto>(26U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> seasonReward_ = new RepeatedField<RewardDto>();
	}
}
