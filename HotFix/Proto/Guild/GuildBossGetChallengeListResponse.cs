using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Guild
{
	public sealed class GuildBossGetChallengeListResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildBossGetChallengeListResponse> Parser
		{
			get
			{
				return GuildBossGetChallengeListResponse._parser;
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
		public RepeatedField<GuildBossRankDto> BossRankList
		{
			get
			{
				return this.bossRankList_;
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
			this.bossRankList_.WriteTo(output, GuildBossGetChallengeListResponse._repeated_bossRankList_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.bossRankList_.CalculateSize(GuildBossGetChallengeListResponse._repeated_bossRankList_codec);
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
						input.SkipLastField();
					}
					else
					{
						this.bossRankList_.AddEntriesFrom(input, GuildBossGetChallengeListResponse._repeated_bossRankList_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<GuildBossGetChallengeListResponse> _parser = new MessageParser<GuildBossGetChallengeListResponse>(() => new GuildBossGetChallengeListResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int BossRankListFieldNumber = 2;

		private static readonly FieldCodec<GuildBossRankDto> _repeated_bossRankList_codec = FieldCodec.ForMessage<GuildBossRankDto>(18U, GuildBossRankDto.Parser);

		private readonly RepeatedField<GuildBossRankDto> bossRankList_ = new RepeatedField<GuildBossRankDto>();
	}
}
