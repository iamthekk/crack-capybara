using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Guild
{
	public sealed class GuildGetDetailResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildGetDetailResponse> Parser
		{
			get
			{
				return GuildGetDetailResponse._parser;
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
		public GuildDetailInfoDto GuildDetailInfoDto
		{
			get
			{
				return this.guildDetailInfoDto_;
			}
			set
			{
				this.guildDetailInfoDto_ = value;
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
			if (this.guildDetailInfoDto_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.GuildDetailInfoDto);
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
			if (this.guildDetailInfoDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GuildDetailInfoDto);
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
						if (this.guildDetailInfoDto_ == null)
						{
							this.guildDetailInfoDto_ = new GuildDetailInfoDto();
						}
						input.ReadMessage(this.guildDetailInfoDto_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<GuildGetDetailResponse> _parser = new MessageParser<GuildGetDetailResponse>(() => new GuildGetDetailResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int GuildDetailInfoDtoFieldNumber = 2;

		private GuildDetailInfoDto guildDetailInfoDto_;
	}
}
