using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class ChapterActRankResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChapterActRankResponse> Parser
		{
			get
			{
				return ChapterActRankResponse._parser;
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
		public RepeatedField<ChapterActRankDto> RankList
		{
			get
			{
				return this.rankList_;
			}
		}

		[DebuggerNonUserCode]
		public ChapterActiveRankInfo RewardInfo
		{
			get
			{
				return this.rewardInfo_;
			}
			set
			{
				this.rewardInfo_ = value;
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
			this.rankList_.WriteTo(output, ChapterActRankResponse._repeated_rankList_codec);
			if (this.rewardInfo_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.RewardInfo);
			}
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
			num += this.rankList_.CalculateSize(ChapterActRankResponse._repeated_rankList_codec);
			if (this.rewardInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.RewardInfo);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
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
				}
				else
				{
					if (num == 26U)
					{
						this.rankList_.AddEntriesFrom(input, ChapterActRankResponse._repeated_rankList_codec);
						continue;
					}
					if (num == 34U)
					{
						if (this.rewardInfo_ == null)
						{
							this.rewardInfo_ = new ChapterActiveRankInfo();
						}
						input.ReadMessage(this.rewardInfo_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChapterActRankResponse> _parser = new MessageParser<ChapterActRankResponse>(() => new ChapterActRankResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RankListFieldNumber = 3;

		private static readonly FieldCodec<ChapterActRankDto> _repeated_rankList_codec = FieldCodec.ForMessage<ChapterActRankDto>(26U, ChapterActRankDto.Parser);

		private readonly RepeatedField<ChapterActRankDto> rankList_ = new RepeatedField<ChapterActRankDto>();

		public const int RewardInfoFieldNumber = 4;

		private ChapterActiveRankInfo rewardInfo_;
	}
}
