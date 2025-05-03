using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Guild
{
	public sealed class GuildUpPositionResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildUpPositionResponse> Parser
		{
			get
			{
				return GuildUpPositionResponse._parser;
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
		public GuildMemberInfoDto GuildMemberInfoDto
		{
			get
			{
				return this.guildMemberInfoDto_;
			}
			set
			{
				this.guildMemberInfoDto_ = value;
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
			if (this.guildMemberInfoDto_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.GuildMemberInfoDto);
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
			if (this.guildMemberInfoDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GuildMemberInfoDto);
			}
			return num;
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
						if (this.guildMemberInfoDto_ == null)
						{
							this.guildMemberInfoDto_ = new GuildMemberInfoDto();
						}
						input.ReadMessage(this.guildMemberInfoDto_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<GuildUpPositionResponse> _parser = new MessageParser<GuildUpPositionResponse>(() => new GuildUpPositionResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int GuildMemberInfoDtoFieldNumber = 2;

		private GuildMemberInfoDto guildMemberInfoDto_;
	}
}
