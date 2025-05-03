using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class UserInfoDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserInfoDto> Parser
		{
			get
			{
				return UserInfoDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public long UserId
		{
			get
			{
				return this.userId_;
			}
			set
			{
				this.userId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string NickName
		{
			get
			{
				return this.nickName_;
			}
			set
			{
				this.nickName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint Avatar
		{
			get
			{
				return this.avatar_;
			}
			set
			{
				this.avatar_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint AvatarFrame
		{
			get
			{
				return this.avatarFrame_;
			}
			set
			{
				this.avatarFrame_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Level
		{
			get
			{
				return this.level_;
			}
			set
			{
				this.level_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong Power
		{
			get
			{
				return this.power_;
			}
			set
			{
				this.power_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint SkinHeaddressId
		{
			get
			{
				return this.skinHeaddressId_;
			}
			set
			{
				this.skinHeaddressId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint SkinBodyId
		{
			get
			{
				return this.skinBodyId_;
			}
			set
			{
				this.skinBodyId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint SkinAccessoryId
		{
			get
			{
				return this.skinAccessoryId_;
			}
			set
			{
				this.skinAccessoryId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint BackGround
		{
			get
			{
				return this.backGround_;
			}
			set
			{
				this.backGround_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<UserUnlockAvatarDto> UnlockAvatarList
		{
			get
			{
				return this.unlockAvatarList_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<UserUnlockAvatarDto> UnlockAvatarFrameList
		{
			get
			{
				return this.unlockAvatarFrameList_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<UserUnlockAvatarDto> UnlockSkinHeaddressList
		{
			get
			{
				return this.unlockSkinHeaddressList_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<UserUnlockAvatarDto> UnlockSkinBodyList
		{
			get
			{
				return this.unlockSkinBodyList_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<UserUnlockAvatarDto> UnlockSkinAccessoryList
		{
			get
			{
				return this.unlockSkinAccessoryList_;
			}
		}

		[DebuggerNonUserCode]
		public GuildDto GuildDto
		{
			get
			{
				return this.guildDto_;
			}
			set
			{
				this.guildDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<UserUnlockAvatarDto> UnlockBackGroundList
		{
			get
			{
				return this.unlockBackGroundList_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<UserUnlockAvatarDto> UnlockTitleList
		{
			get
			{
				return this.unlockTitleList_;
			}
		}

		[DebuggerNonUserCode]
		public uint TitleId
		{
			get
			{
				return this.titleId_;
			}
			set
			{
				this.titleId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<EquipmentDto> Equips
		{
			get
			{
				return this.equips_;
			}
		}

		[DebuggerNonUserCode]
		public MountInfo MountInfo
		{
			get
			{
				return this.mountInfo_;
			}
			set
			{
				this.mountInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.UserId != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.UserId);
			}
			if (this.NickName.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.NickName);
			}
			if (this.Avatar != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Avatar);
			}
			if (this.AvatarFrame != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.AvatarFrame);
			}
			if (this.Level != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.Level);
			}
			if (this.Power != 0UL)
			{
				output.WriteRawTag(48);
				output.WriteUInt64(this.Power);
			}
			if (this.SkinHeaddressId != 0U)
			{
				output.WriteRawTag(80);
				output.WriteUInt32(this.SkinHeaddressId);
			}
			if (this.SkinBodyId != 0U)
			{
				output.WriteRawTag(88);
				output.WriteUInt32(this.SkinBodyId);
			}
			if (this.SkinAccessoryId != 0U)
			{
				output.WriteRawTag(96);
				output.WriteUInt32(this.SkinAccessoryId);
			}
			if (this.BackGround != 0U)
			{
				output.WriteRawTag(104);
				output.WriteUInt32(this.BackGround);
			}
			this.unlockAvatarList_.WriteTo(output, UserInfoDto._repeated_unlockAvatarList_codec);
			this.unlockAvatarFrameList_.WriteTo(output, UserInfoDto._repeated_unlockAvatarFrameList_codec);
			this.unlockSkinHeaddressList_.WriteTo(output, UserInfoDto._repeated_unlockSkinHeaddressList_codec);
			this.unlockSkinBodyList_.WriteTo(output, UserInfoDto._repeated_unlockSkinBodyList_codec);
			this.unlockSkinAccessoryList_.WriteTo(output, UserInfoDto._repeated_unlockSkinAccessoryList_codec);
			if (this.guildDto_ != null)
			{
				output.WriteRawTag(154, 1);
				output.WriteMessage(this.GuildDto);
			}
			this.unlockBackGroundList_.WriteTo(output, UserInfoDto._repeated_unlockBackGroundList_codec);
			this.unlockTitleList_.WriteTo(output, UserInfoDto._repeated_unlockTitleList_codec);
			if (this.TitleId != 0U)
			{
				output.WriteRawTag(176, 1);
				output.WriteUInt32(this.TitleId);
			}
			this.equips_.WriteTo(output, UserInfoDto._repeated_equips_codec);
			if (this.mountInfo_ != null)
			{
				output.WriteRawTag(194, 1);
				output.WriteMessage(this.MountInfo);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.UserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId);
			}
			if (this.NickName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.NickName);
			}
			if (this.Avatar != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Avatar);
			}
			if (this.AvatarFrame != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.AvatarFrame);
			}
			if (this.Level != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Level);
			}
			if (this.Power != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Power);
			}
			if (this.SkinHeaddressId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SkinHeaddressId);
			}
			if (this.SkinBodyId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SkinBodyId);
			}
			if (this.SkinAccessoryId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SkinAccessoryId);
			}
			if (this.BackGround != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BackGround);
			}
			num += this.unlockAvatarList_.CalculateSize(UserInfoDto._repeated_unlockAvatarList_codec);
			num += this.unlockAvatarFrameList_.CalculateSize(UserInfoDto._repeated_unlockAvatarFrameList_codec);
			num += this.unlockSkinHeaddressList_.CalculateSize(UserInfoDto._repeated_unlockSkinHeaddressList_codec);
			num += this.unlockSkinBodyList_.CalculateSize(UserInfoDto._repeated_unlockSkinBodyList_codec);
			num += this.unlockSkinAccessoryList_.CalculateSize(UserInfoDto._repeated_unlockSkinAccessoryList_codec);
			if (this.guildDto_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.GuildDto);
			}
			num += this.unlockBackGroundList_.CalculateSize(UserInfoDto._repeated_unlockBackGroundList_codec);
			num += this.unlockTitleList_.CalculateSize(UserInfoDto._repeated_unlockTitleList_codec);
			if (this.TitleId != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.TitleId);
			}
			num += this.equips_.CalculateSize(UserInfoDto._repeated_equips_codec);
			if (this.mountInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.MountInfo);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 104U)
				{
					if (num <= 40U)
					{
						if (num <= 18U)
						{
							if (num == 8U)
							{
								this.UserId = input.ReadInt64();
								continue;
							}
							if (num == 18U)
							{
								this.NickName = input.ReadString();
								continue;
							}
						}
						else
						{
							if (num == 24U)
							{
								this.Avatar = input.ReadUInt32();
								continue;
							}
							if (num == 32U)
							{
								this.AvatarFrame = input.ReadUInt32();
								continue;
							}
							if (num == 40U)
							{
								this.Level = input.ReadUInt32();
								continue;
							}
						}
					}
					else if (num <= 80U)
					{
						if (num == 48U)
						{
							this.Power = input.ReadUInt64();
							continue;
						}
						if (num == 80U)
						{
							this.SkinHeaddressId = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 88U)
						{
							this.SkinBodyId = input.ReadUInt32();
							continue;
						}
						if (num == 96U)
						{
							this.SkinAccessoryId = input.ReadUInt32();
							continue;
						}
						if (num == 104U)
						{
							this.BackGround = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 146U)
				{
					if (num <= 122U)
					{
						if (num == 114U)
						{
							this.unlockAvatarList_.AddEntriesFrom(input, UserInfoDto._repeated_unlockAvatarList_codec);
							continue;
						}
						if (num == 122U)
						{
							this.unlockAvatarFrameList_.AddEntriesFrom(input, UserInfoDto._repeated_unlockAvatarFrameList_codec);
							continue;
						}
					}
					else
					{
						if (num == 130U)
						{
							this.unlockSkinHeaddressList_.AddEntriesFrom(input, UserInfoDto._repeated_unlockSkinHeaddressList_codec);
							continue;
						}
						if (num == 138U)
						{
							this.unlockSkinBodyList_.AddEntriesFrom(input, UserInfoDto._repeated_unlockSkinBodyList_codec);
							continue;
						}
						if (num == 146U)
						{
							this.unlockSkinAccessoryList_.AddEntriesFrom(input, UserInfoDto._repeated_unlockSkinAccessoryList_codec);
							continue;
						}
					}
				}
				else if (num <= 170U)
				{
					if (num == 154U)
					{
						if (this.guildDto_ == null)
						{
							this.guildDto_ = new GuildDto();
						}
						input.ReadMessage(this.guildDto_);
						continue;
					}
					if (num == 162U)
					{
						this.unlockBackGroundList_.AddEntriesFrom(input, UserInfoDto._repeated_unlockBackGroundList_codec);
						continue;
					}
					if (num == 170U)
					{
						this.unlockTitleList_.AddEntriesFrom(input, UserInfoDto._repeated_unlockTitleList_codec);
						continue;
					}
				}
				else
				{
					if (num == 176U)
					{
						this.TitleId = input.ReadUInt32();
						continue;
					}
					if (num == 186U)
					{
						this.equips_.AddEntriesFrom(input, UserInfoDto._repeated_equips_codec);
						continue;
					}
					if (num == 194U)
					{
						if (this.mountInfo_ == null)
						{
							this.mountInfo_ = new MountInfo();
						}
						input.ReadMessage(this.mountInfo_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UserInfoDto> _parser = new MessageParser<UserInfoDto>(() => new UserInfoDto());

		public const int UserIdFieldNumber = 1;

		private long userId_;

		public const int NickNameFieldNumber = 2;

		private string nickName_ = "";

		public const int AvatarFieldNumber = 3;

		private uint avatar_;

		public const int AvatarFrameFieldNumber = 4;

		private uint avatarFrame_;

		public const int LevelFieldNumber = 5;

		private uint level_;

		public const int PowerFieldNumber = 6;

		private ulong power_;

		public const int SkinHeaddressIdFieldNumber = 10;

		private uint skinHeaddressId_;

		public const int SkinBodyIdFieldNumber = 11;

		private uint skinBodyId_;

		public const int SkinAccessoryIdFieldNumber = 12;

		private uint skinAccessoryId_;

		public const int BackGroundFieldNumber = 13;

		private uint backGround_;

		public const int UnlockAvatarListFieldNumber = 14;

		private static readonly FieldCodec<UserUnlockAvatarDto> _repeated_unlockAvatarList_codec = FieldCodec.ForMessage<UserUnlockAvatarDto>(114U, UserUnlockAvatarDto.Parser);

		private readonly RepeatedField<UserUnlockAvatarDto> unlockAvatarList_ = new RepeatedField<UserUnlockAvatarDto>();

		public const int UnlockAvatarFrameListFieldNumber = 15;

		private static readonly FieldCodec<UserUnlockAvatarDto> _repeated_unlockAvatarFrameList_codec = FieldCodec.ForMessage<UserUnlockAvatarDto>(122U, UserUnlockAvatarDto.Parser);

		private readonly RepeatedField<UserUnlockAvatarDto> unlockAvatarFrameList_ = new RepeatedField<UserUnlockAvatarDto>();

		public const int UnlockSkinHeaddressListFieldNumber = 16;

		private static readonly FieldCodec<UserUnlockAvatarDto> _repeated_unlockSkinHeaddressList_codec = FieldCodec.ForMessage<UserUnlockAvatarDto>(130U, UserUnlockAvatarDto.Parser);

		private readonly RepeatedField<UserUnlockAvatarDto> unlockSkinHeaddressList_ = new RepeatedField<UserUnlockAvatarDto>();

		public const int UnlockSkinBodyListFieldNumber = 17;

		private static readonly FieldCodec<UserUnlockAvatarDto> _repeated_unlockSkinBodyList_codec = FieldCodec.ForMessage<UserUnlockAvatarDto>(138U, UserUnlockAvatarDto.Parser);

		private readonly RepeatedField<UserUnlockAvatarDto> unlockSkinBodyList_ = new RepeatedField<UserUnlockAvatarDto>();

		public const int UnlockSkinAccessoryListFieldNumber = 18;

		private static readonly FieldCodec<UserUnlockAvatarDto> _repeated_unlockSkinAccessoryList_codec = FieldCodec.ForMessage<UserUnlockAvatarDto>(146U, UserUnlockAvatarDto.Parser);

		private readonly RepeatedField<UserUnlockAvatarDto> unlockSkinAccessoryList_ = new RepeatedField<UserUnlockAvatarDto>();

		public const int GuildDtoFieldNumber = 19;

		private GuildDto guildDto_;

		public const int UnlockBackGroundListFieldNumber = 20;

		private static readonly FieldCodec<UserUnlockAvatarDto> _repeated_unlockBackGroundList_codec = FieldCodec.ForMessage<UserUnlockAvatarDto>(162U, UserUnlockAvatarDto.Parser);

		private readonly RepeatedField<UserUnlockAvatarDto> unlockBackGroundList_ = new RepeatedField<UserUnlockAvatarDto>();

		public const int UnlockTitleListFieldNumber = 21;

		private static readonly FieldCodec<UserUnlockAvatarDto> _repeated_unlockTitleList_codec = FieldCodec.ForMessage<UserUnlockAvatarDto>(170U, UserUnlockAvatarDto.Parser);

		private readonly RepeatedField<UserUnlockAvatarDto> unlockTitleList_ = new RepeatedField<UserUnlockAvatarDto>();

		public const int TitleIdFieldNumber = 22;

		private uint titleId_;

		public const int EquipsFieldNumber = 23;

		private static readonly FieldCodec<EquipmentDto> _repeated_equips_codec = FieldCodec.ForMessage<EquipmentDto>(186U, EquipmentDto.Parser);

		private readonly RepeatedField<EquipmentDto> equips_ = new RepeatedField<EquipmentDto>();

		public const int MountInfoFieldNumber = 24;

		private MountInfo mountInfo_;
	}
}
