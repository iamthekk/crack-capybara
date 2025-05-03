using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.LeaderBoard
{
	public sealed class LeaderBoardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<LeaderBoardResponse> Parser
		{
			get
			{
				return LeaderBoardResponse._parser;
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
		public RepeatedField<RankUserDto> Ranks
		{
			get
			{
				return this.ranks_;
			}
		}

		[DebuggerNonUserCode]
		public int Page
		{
			get
			{
				return this.page_;
			}
			set
			{
				this.page_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int TotalCount
		{
			get
			{
				return this.totalCount_;
			}
			set
			{
				this.totalCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RankUserDto Self
		{
			get
			{
				return this.self_;
			}
			set
			{
				this.self_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RankUserDto> Top3
		{
			get
			{
				return this.top3_;
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
			this.ranks_.WriteTo(output, LeaderBoardResponse._repeated_ranks_codec);
			if (this.Page != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Page);
			}
			if (this.TotalCount != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.TotalCount);
			}
			if (this.self_ != null)
			{
				output.WriteRawTag(50);
				output.WriteMessage(this.Self);
			}
			this.top3_.WriteTo(output, LeaderBoardResponse._repeated_top3_codec);
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
			num += this.ranks_.CalculateSize(LeaderBoardResponse._repeated_ranks_codec);
			if (this.Page != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Page);
			}
			if (this.TotalCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TotalCount);
			}
			if (this.self_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Self);
			}
			return num + this.top3_.CalculateSize(LeaderBoardResponse._repeated_top3_codec);
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
						this.ranks_.AddEntriesFrom(input, LeaderBoardResponse._repeated_ranks_codec);
						continue;
					}
				}
				else if (num <= 40U)
				{
					if (num == 32U)
					{
						this.Page = input.ReadInt32();
						continue;
					}
					if (num == 40U)
					{
						this.TotalCount = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 50U)
					{
						if (this.self_ == null)
						{
							this.self_ = new RankUserDto();
						}
						input.ReadMessage(this.self_);
						continue;
					}
					if (num == 58U)
					{
						this.top3_.AddEntriesFrom(input, LeaderBoardResponse._repeated_top3_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<LeaderBoardResponse> _parser = new MessageParser<LeaderBoardResponse>(() => new LeaderBoardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RanksFieldNumber = 3;

		private static readonly FieldCodec<RankUserDto> _repeated_ranks_codec = FieldCodec.ForMessage<RankUserDto>(26U, RankUserDto.Parser);

		private readonly RepeatedField<RankUserDto> ranks_ = new RepeatedField<RankUserDto>();

		public const int PageFieldNumber = 4;

		private int page_;

		public const int TotalCountFieldNumber = 5;

		private int totalCount_;

		public const int SelfFieldNumber = 6;

		private RankUserDto self_;

		public const int Top3FieldNumber = 7;

		private static readonly FieldCodec<RankUserDto> _repeated_top3_codec = FieldCodec.ForMessage<RankUserDto>(58U, RankUserDto.Parser);

		private readonly RepeatedField<RankUserDto> top3_ = new RepeatedField<RankUserDto>();
	}
}
