using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.ActTime
{
	public sealed class ActTimeRankResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ActTimeRankResponse> Parser
		{
			get
			{
				return ActTimeRankResponse._parser;
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
		public int RankIndex
		{
			get
			{
				return this.rankIndex_;
			}
			set
			{
				this.rankIndex_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ActTimeRankDto> Rank
		{
			get
			{
				return this.rank_;
			}
		}

		[DebuggerNonUserCode]
		public int RankScore
		{
			get
			{
				return this.rankScore_;
			}
			set
			{
				this.rankScore_ = value;
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
			if (this.RankIndex != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.RankIndex);
			}
			this.rank_.WriteTo(output, ActTimeRankResponse._repeated_rank_codec);
			if (this.RankScore != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.RankScore);
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
			if (this.RankIndex != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.RankIndex);
			}
			num += this.rank_.CalculateSize(ActTimeRankResponse._repeated_rank_codec);
			if (this.RankScore != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.RankScore);
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
					if (num == 24U)
					{
						this.RankIndex = input.ReadInt32();
						continue;
					}
					if (num == 34U)
					{
						this.rank_.AddEntriesFrom(input, ActTimeRankResponse._repeated_rank_codec);
						continue;
					}
					if (num == 40U)
					{
						this.RankScore = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ActTimeRankResponse> _parser = new MessageParser<ActTimeRankResponse>(() => new ActTimeRankResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RankIndexFieldNumber = 3;

		private int rankIndex_;

		public const int RankFieldNumber = 4;

		private static readonly FieldCodec<ActTimeRankDto> _repeated_rank_codec = FieldCodec.ForMessage<ActTimeRankDto>(34U, ActTimeRankDto.Parser);

		private readonly RepeatedField<ActTimeRankDto> rank_ = new RepeatedField<ActTimeRankDto>();

		public const int RankScoreFieldNumber = 5;

		private int rankScore_;
	}
}
