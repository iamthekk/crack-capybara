using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Guild
{
	public sealed class GuildAgreeJoinResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildAgreeJoinResponse> Parser
		{
			get
			{
				return GuildAgreeJoinResponse._parser;
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
		public RepeatedField<GuildMemberInfoDto> GuildMemberInfoDto
		{
			get
			{
				return this.guildMemberInfoDto_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ulong> JoinOtherGuildUserIds
		{
			get
			{
				return this.joinOtherGuildUserIds_;
			}
		}

		[DebuggerNonUserCode]
		public uint ApplyCount
		{
			get
			{
				return this.applyCount_;
			}
			set
			{
				this.applyCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<long> TimeLimitGuildUserIds
		{
			get
			{
				return this.timeLimitGuildUserIds_;
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
			this.guildMemberInfoDto_.WriteTo(output, GuildAgreeJoinResponse._repeated_guildMemberInfoDto_codec);
			this.joinOtherGuildUserIds_.WriteTo(output, GuildAgreeJoinResponse._repeated_joinOtherGuildUserIds_codec);
			if (this.ApplyCount != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.ApplyCount);
			}
			this.timeLimitGuildUserIds_.WriteTo(output, GuildAgreeJoinResponse._repeated_timeLimitGuildUserIds_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			num += this.guildMemberInfoDto_.CalculateSize(GuildAgreeJoinResponse._repeated_guildMemberInfoDto_codec);
			num += this.joinOtherGuildUserIds_.CalculateSize(GuildAgreeJoinResponse._repeated_joinOtherGuildUserIds_codec);
			if (this.ApplyCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ApplyCount);
			}
			return num + this.timeLimitGuildUserIds_.CalculateSize(GuildAgreeJoinResponse._repeated_timeLimitGuildUserIds_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 24U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 18U)
					{
						this.guildMemberInfoDto_.AddEntriesFrom(input, GuildAgreeJoinResponse._repeated_guildMemberInfoDto_codec);
						continue;
					}
					if (num == 24U)
					{
						goto IL_005E;
					}
				}
				else if (num <= 32U)
				{
					if (num == 26U)
					{
						goto IL_005E;
					}
					if (num == 32U)
					{
						this.ApplyCount = input.ReadUInt32();
						continue;
					}
				}
				else if (num == 40U || num == 42U)
				{
					this.timeLimitGuildUserIds_.AddEntriesFrom(input, GuildAgreeJoinResponse._repeated_timeLimitGuildUserIds_codec);
					continue;
				}
				input.SkipLastField();
				continue;
				IL_005E:
				this.joinOtherGuildUserIds_.AddEntriesFrom(input, GuildAgreeJoinResponse._repeated_joinOtherGuildUserIds_codec);
			}
		}

		private static readonly MessageParser<GuildAgreeJoinResponse> _parser = new MessageParser<GuildAgreeJoinResponse>(() => new GuildAgreeJoinResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int GuildMemberInfoDtoFieldNumber = 2;

		private static readonly FieldCodec<GuildMemberInfoDto> _repeated_guildMemberInfoDto_codec = FieldCodec.ForMessage<GuildMemberInfoDto>(18U, Proto.Guild.GuildMemberInfoDto.Parser);

		private readonly RepeatedField<GuildMemberInfoDto> guildMemberInfoDto_ = new RepeatedField<GuildMemberInfoDto>();

		public const int JoinOtherGuildUserIdsFieldNumber = 3;

		private static readonly FieldCodec<ulong> _repeated_joinOtherGuildUserIds_codec = FieldCodec.ForUInt64(26U);

		private readonly RepeatedField<ulong> joinOtherGuildUserIds_ = new RepeatedField<ulong>();

		public const int ApplyCountFieldNumber = 4;

		private uint applyCount_;

		public const int TimeLimitGuildUserIdsFieldNumber = 5;

		private static readonly FieldCodec<long> _repeated_timeLimitGuildUserIds_codec = FieldCodec.ForInt64(42U);

		private readonly RepeatedField<long> timeLimitGuildUserIds_ = new RepeatedField<long>();
	}
}
