using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Guild
{
	public sealed class GuildSearchResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildSearchResponse> Parser
		{
			get
			{
				return GuildSearchResponse._parser;
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
		public RepeatedField<GuildInfoDto> GuildInfoDtos
		{
			get
			{
				return this.guildInfoDtos_;
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
			this.guildInfoDtos_.WriteTo(output, GuildSearchResponse._repeated_guildInfoDtos_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.guildInfoDtos_.CalculateSize(GuildSearchResponse._repeated_guildInfoDtos_codec);
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
						this.guildInfoDtos_.AddEntriesFrom(input, GuildSearchResponse._repeated_guildInfoDtos_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<GuildSearchResponse> _parser = new MessageParser<GuildSearchResponse>(() => new GuildSearchResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int GuildInfoDtosFieldNumber = 2;

		private static readonly FieldCodec<GuildInfoDto> _repeated_guildInfoDtos_codec = FieldCodec.ForMessage<GuildInfoDto>(18U, GuildInfoDto.Parser);

		private readonly RepeatedField<GuildInfoDto> guildInfoDtos_ = new RepeatedField<GuildInfoDto>();
	}
}
