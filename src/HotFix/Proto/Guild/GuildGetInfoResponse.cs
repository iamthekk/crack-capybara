using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildGetInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildGetInfoResponse> Parser
		{
			get
			{
				return GuildGetInfoResponse._parser;
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
		public bool IsJoined
		{
			get
			{
				return this.isJoined_;
			}
			set
			{
				this.isJoined_ = value;
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
		public bool IsLevelUp
		{
			get
			{
				return this.isLevelUp_;
			}
			set
			{
				this.isLevelUp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public BeKickedOutDto BeKickedOutDto
		{
			get
			{
				return this.beKickedOutDto_;
			}
			set
			{
				this.beKickedOutDto_ = value;
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
		public RepeatedField<RewardDto> DonationReward
		{
			get
			{
				return this.donationReward_;
			}
		}

		[DebuggerNonUserCode]
		public long QuitGuildTimeStamp
		{
			get
			{
				return this.quitGuildTimeStamp_;
			}
			set
			{
				this.quitGuildTimeStamp_ = value;
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
			if (this.IsJoined)
			{
				output.WriteRawTag(16);
				output.WriteBool(this.IsJoined);
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
			if (this.IsLevelUp)
			{
				output.WriteRawTag(40);
				output.WriteBool(this.IsLevelUp);
			}
			if (this.beKickedOutDto_ != null)
			{
				output.WriteRawTag(50);
				output.WriteMessage(this.BeKickedOutDto);
			}
			if (this.commonData_ != null)
			{
				output.WriteRawTag(58);
				output.WriteMessage(this.CommonData);
			}
			this.donationReward_.WriteTo(output, GuildGetInfoResponse._repeated_donationReward_codec);
			if (this.QuitGuildTimeStamp != 0L)
			{
				output.WriteRawTag(72);
				output.WriteInt64(this.QuitGuildTimeStamp);
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
			if (this.IsJoined)
			{
				num += 2;
			}
			if (this.guildDetailInfoDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GuildDetailInfoDto);
			}
			if (this.guildFeaturesDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GuildFeaturesDto);
			}
			if (this.IsLevelUp)
			{
				num += 2;
			}
			if (this.beKickedOutDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.BeKickedOutDto);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			num += this.donationReward_.CalculateSize(GuildGetInfoResponse._repeated_donationReward_codec);
			if (this.QuitGuildTimeStamp != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.QuitGuildTimeStamp);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 34U)
				{
					if (num <= 16U)
					{
						if (num == 8U)
						{
							this.Code = input.ReadInt32();
							continue;
						}
						if (num == 16U)
						{
							this.IsJoined = input.ReadBool();
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
				}
				else if (num <= 50U)
				{
					if (num == 40U)
					{
						this.IsLevelUp = input.ReadBool();
						continue;
					}
					if (num == 50U)
					{
						if (this.beKickedOutDto_ == null)
						{
							this.beKickedOutDto_ = new BeKickedOutDto();
						}
						input.ReadMessage(this.beKickedOutDto_);
						continue;
					}
				}
				else
				{
					if (num == 58U)
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
					if (num == 66U)
					{
						this.donationReward_.AddEntriesFrom(input, GuildGetInfoResponse._repeated_donationReward_codec);
						continue;
					}
					if (num == 72U)
					{
						this.QuitGuildTimeStamp = input.ReadInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildGetInfoResponse> _parser = new MessageParser<GuildGetInfoResponse>(() => new GuildGetInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int IsJoinedFieldNumber = 2;

		private bool isJoined_;

		public const int GuildDetailInfoDtoFieldNumber = 3;

		private GuildDetailInfoDto guildDetailInfoDto_;

		public const int GuildFeaturesDtoFieldNumber = 4;

		private GuildFeaturesDto guildFeaturesDto_;

		public const int IsLevelUpFieldNumber = 5;

		private bool isLevelUp_;

		public const int BeKickedOutDtoFieldNumber = 6;

		private BeKickedOutDto beKickedOutDto_;

		public const int CommonDataFieldNumber = 7;

		private CommonData commonData_;

		public const int DonationRewardFieldNumber = 8;

		private static readonly FieldCodec<RewardDto> _repeated_donationReward_codec = FieldCodec.ForMessage<RewardDto>(66U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> donationReward_ = new RepeatedField<RewardDto>();

		public const int QuitGuildTimeStampFieldNumber = 9;

		private long quitGuildTimeStamp_;
	}
}
