using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Guild
{
	public sealed class GuildGetFeaturesInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildGetFeaturesInfoResponse> Parser
		{
			get
			{
				return GuildGetFeaturesInfoResponse._parser;
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
		public GuildFeaturesDto GuildFeaturesDto
		{
			get
			{
				return this.guildFeaturesDto_;
			}
			set
			{
				this.guildFeaturesDto_ = value;
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
			if (this.guildFeaturesDto_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.GuildFeaturesDto);
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
			if (this.guildFeaturesDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GuildFeaturesDto);
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
						if (this.guildFeaturesDto_ == null)
						{
							this.guildFeaturesDto_ = new GuildFeaturesDto();
						}
						input.ReadMessage(this.guildFeaturesDto_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<GuildGetFeaturesInfoResponse> _parser = new MessageParser<GuildGetFeaturesInfoResponse>(() => new GuildGetFeaturesInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int GuildFeaturesDtoFieldNumber = 2;

		private GuildFeaturesDto guildFeaturesDto_;
	}
}
