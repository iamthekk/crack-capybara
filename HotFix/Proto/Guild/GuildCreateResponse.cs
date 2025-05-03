using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildCreateResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildCreateResponse> Parser
		{
			get
			{
				return GuildCreateResponse._parser;
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
			if (this.commonData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonData);
			}
			if (this.guildDetailInfoDto_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.GuildDetailInfoDto);
			}
			if (this.guildFeaturesDto_ != null)
			{
				output.WriteRawTag(34);
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
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			if (this.guildDetailInfoDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GuildDetailInfoDto);
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
						if (this.guildDetailInfoDto_ == null)
						{
							this.guildDetailInfoDto_ = new GuildDetailInfoDto();
						}
						input.ReadMessage(this.guildDetailInfoDto_);
						continue;
					}
					if (num == 34U)
					{
						if (this.guildFeaturesDto_ == null)
						{
							this.guildFeaturesDto_ = new GuildFeaturesDto();
						}
						input.ReadMessage(this.guildFeaturesDto_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildCreateResponse> _parser = new MessageParser<GuildCreateResponse>(() => new GuildCreateResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int GuildDetailInfoDtoFieldNumber = 3;

		private GuildDetailInfoDto guildDetailInfoDto_;

		public const int GuildFeaturesDtoFieldNumber = 4;

		private GuildFeaturesDto guildFeaturesDto_;
	}
}
