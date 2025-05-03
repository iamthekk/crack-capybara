using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Guild
{
	public sealed class GuildDetailInfoDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildDetailInfoDto> Parser
		{
			get
			{
				return GuildDetailInfoDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public GuildInfoDto GuildInfoDto
		{
			get
			{
				return this.guildInfoDto_;
			}
			set
			{
				this.guildInfoDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<GuildMemberInfoDto> GuildMemberInfoDtos
		{
			get
			{
				return this.guildMemberInfoDtos_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.guildInfoDto_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.GuildInfoDto);
			}
			this.guildMemberInfoDtos_.WriteTo(output, GuildDetailInfoDto._repeated_guildMemberInfoDtos_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.guildInfoDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GuildInfoDto);
			}
			return num + this.guildMemberInfoDtos_.CalculateSize(GuildDetailInfoDto._repeated_guildMemberInfoDtos_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						this.guildMemberInfoDtos_.AddEntriesFrom(input, GuildDetailInfoDto._repeated_guildMemberInfoDtos_codec);
					}
				}
				else
				{
					if (this.guildInfoDto_ == null)
					{
						this.guildInfoDto_ = new GuildInfoDto();
					}
					input.ReadMessage(this.guildInfoDto_);
				}
			}
		}

		private static readonly MessageParser<GuildDetailInfoDto> _parser = new MessageParser<GuildDetailInfoDto>(() => new GuildDetailInfoDto());

		public const int GuildInfoDtoFieldNumber = 1;

		private GuildInfoDto guildInfoDto_;

		public const int GuildMemberInfoDtosFieldNumber = 2;

		private static readonly FieldCodec<GuildMemberInfoDto> _repeated_guildMemberInfoDtos_codec = FieldCodec.ForMessage<GuildMemberInfoDto>(18U, GuildMemberInfoDto.Parser);

		private readonly RepeatedField<GuildMemberInfoDto> guildMemberInfoDtos_ = new RepeatedField<GuildMemberInfoDto>();
	}
}
