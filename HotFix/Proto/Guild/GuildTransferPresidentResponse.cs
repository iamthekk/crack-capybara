﻿using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Guild
{
	public sealed class GuildTransferPresidentResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildTransferPresidentResponse> Parser
		{
			get
			{
				return GuildTransferPresidentResponse._parser;
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
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			this.guildMemberInfoDtos_.WriteTo(output, GuildTransferPresidentResponse._repeated_guildMemberInfoDtos_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.guildMemberInfoDtos_.CalculateSize(GuildTransferPresidentResponse._repeated_guildMemberInfoDtos_codec);
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
						this.guildMemberInfoDtos_.AddEntriesFrom(input, GuildTransferPresidentResponse._repeated_guildMemberInfoDtos_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<GuildTransferPresidentResponse> _parser = new MessageParser<GuildTransferPresidentResponse>(() => new GuildTransferPresidentResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int GuildMemberInfoDtosFieldNumber = 2;

		private static readonly FieldCodec<GuildMemberInfoDto> _repeated_guildMemberInfoDtos_codec = FieldCodec.ForMessage<GuildMemberInfoDto>(18U, GuildMemberInfoDto.Parser);

		private readonly RepeatedField<GuildMemberInfoDto> guildMemberInfoDtos_ = new RepeatedField<GuildMemberInfoDto>();
	}
}
