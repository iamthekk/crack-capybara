using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Social
{
	public sealed class SocialPowerRankResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SocialPowerRankResponse> Parser
		{
			get
			{
				return SocialPowerRankResponse._parser;
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
		public RepeatedField<PowerRankDto> Rank
		{
			get
			{
				return this.rank_;
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
			this.rank_.WriteTo(output, SocialPowerRankResponse._repeated_rank_codec);
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
			return num + this.rank_.CalculateSize(SocialPowerRankResponse._repeated_rank_codec);
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
							this.rank_.AddEntriesFrom(input, SocialPowerRankResponse._repeated_rank_codec);
						}
					}
					else
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<SocialPowerRankResponse> _parser = new MessageParser<SocialPowerRankResponse>(() => new SocialPowerRankResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RankFieldNumber = 3;

		private static readonly FieldCodec<PowerRankDto> _repeated_rank_codec = FieldCodec.ForMessage<PowerRankDto>(26U, PowerRankDto.Parser);

		private readonly RepeatedField<PowerRankDto> rank_ = new RepeatedField<PowerRankDto>();
	}
}
