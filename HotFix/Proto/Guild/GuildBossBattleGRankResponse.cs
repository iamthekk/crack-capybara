using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildBossBattleGRankResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildBossBattleGRankResponse> Parser
		{
			get
			{
				return GuildBossBattleGRankResponse._parser;
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
		public RepeatedField<GuildSimpleDto> Dtos
		{
			get
			{
				return this.dtos_;
			}
		}

		[DebuggerNonUserCode]
		public GuildSimpleDto MyGuildDto
		{
			get
			{
				return this.myGuildDto_;
			}
			set
			{
				this.myGuildDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long MyRank
		{
			get
			{
				return this.myRank_;
			}
			set
			{
				this.myRank_ = value;
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
			this.dtos_.WriteTo(output, GuildBossBattleGRankResponse._repeated_dtos_codec);
			if (this.myGuildDto_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.MyGuildDto);
			}
			if (this.MyRank != 0L)
			{
				output.WriteRawTag(40);
				output.WriteInt64(this.MyRank);
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
			num += this.dtos_.CalculateSize(GuildBossBattleGRankResponse._repeated_dtos_codec);
			if (this.myGuildDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.MyGuildDto);
			}
			if (this.MyRank != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.MyRank);
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
						this.dtos_.AddEntriesFrom(input, GuildBossBattleGRankResponse._repeated_dtos_codec);
						continue;
					}
					if (num == 34U)
					{
						if (this.myGuildDto_ == null)
						{
							this.myGuildDto_ = new GuildSimpleDto();
						}
						input.ReadMessage(this.myGuildDto_);
						continue;
					}
					if (num == 40U)
					{
						this.MyRank = input.ReadInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildBossBattleGRankResponse> _parser = new MessageParser<GuildBossBattleGRankResponse>(() => new GuildBossBattleGRankResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int DtosFieldNumber = 3;

		private static readonly FieldCodec<GuildSimpleDto> _repeated_dtos_codec = FieldCodec.ForMessage<GuildSimpleDto>(26U, GuildSimpleDto.Parser);

		private readonly RepeatedField<GuildSimpleDto> dtos_ = new RepeatedField<GuildSimpleDto>();

		public const int MyGuildDtoFieldNumber = 4;

		private GuildSimpleDto myGuildDto_;

		public const int MyRankFieldNumber = 5;

		private long myRank_;
	}
}
