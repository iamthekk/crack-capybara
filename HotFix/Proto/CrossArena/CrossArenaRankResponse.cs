using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;
using Proto.User;

namespace Proto.CrossArena
{
	public sealed class CrossArenaRankResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CrossArenaRankResponse> Parser
		{
			get
			{
				return CrossArenaRankResponse._parser;
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
		public RepeatedField<CrossArenaRankDto> Rank
		{
			get
			{
				return this.rank_;
			}
		}

		[DebuggerNonUserCode]
		public uint TotalCount
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
		public RepeatedField<PlayerInfoDto> Info
		{
			get
			{
				return this.info_;
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
			this.rank_.WriteTo(output, CrossArenaRankResponse._repeated_rank_codec);
			if (this.TotalCount != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.TotalCount);
			}
			this.info_.WriteTo(output, CrossArenaRankResponse._repeated_info_codec);
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
			num += this.rank_.CalculateSize(CrossArenaRankResponse._repeated_rank_codec);
			if (this.TotalCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TotalCount);
			}
			return num + this.info_.CalculateSize(CrossArenaRankResponse._repeated_info_codec);
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
						this.rank_.AddEntriesFrom(input, CrossArenaRankResponse._repeated_rank_codec);
						continue;
					}
					if (num == 32U)
					{
						this.TotalCount = input.ReadUInt32();
						continue;
					}
					if (num == 42U)
					{
						this.info_.AddEntriesFrom(input, CrossArenaRankResponse._repeated_info_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<CrossArenaRankResponse> _parser = new MessageParser<CrossArenaRankResponse>(() => new CrossArenaRankResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RankFieldNumber = 3;

		private static readonly FieldCodec<CrossArenaRankDto> _repeated_rank_codec = FieldCodec.ForMessage<CrossArenaRankDto>(26U, CrossArenaRankDto.Parser);

		private readonly RepeatedField<CrossArenaRankDto> rank_ = new RepeatedField<CrossArenaRankDto>();

		public const int TotalCountFieldNumber = 4;

		private uint totalCount_;

		public const int InfoFieldNumber = 5;

		private static readonly FieldCodec<PlayerInfoDto> _repeated_info_codec = FieldCodec.ForMessage<PlayerInfoDto>(42U, PlayerInfoDto.Parser);

		private readonly RepeatedField<PlayerInfoDto> info_ = new RepeatedField<PlayerInfoDto>();
	}
}
