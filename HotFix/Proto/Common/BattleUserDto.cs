using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class BattleUserDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<BattleUserDto> Parser
		{
			get
			{
				return BattleUserDto._parser;
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
		public int Level
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
		public int Avatar
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
		public int AvatarFrame
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
		public long Power
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
		public long ActorRowId
		{
			get
			{
				return this.actorRowId_;
			}
			set
			{
				this.actorRowId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public LordDto Extra
		{
			get
			{
				return this.extra_;
			}
			set
			{
				this.extra_ = value;
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
		public RepeatedField<HeroDto> Heros
		{
			get
			{
				return this.heros_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<TalentSystemDto> Talents
		{
			get
			{
				return this.talents_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RelicDto> Relics
		{
			get
			{
				return this.relics_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<PetDto> Pets
		{
			get
			{
				return this.pets_;
			}
		}

		[DebuggerNonUserCode]
		public CollectionInfo CollectionInfo
		{
			get
			{
				return this.collectionInfo_;
			}
			set
			{
				this.collectionInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public TalentsInfo TalentsInfo
		{
			get
			{
				return this.talentsInfo_;
			}
			set
			{
				this.talentsInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ArtifactInfo ArtifactInfo
		{
			get
			{
				return this.artifactInfo_;
			}
			set
			{
				this.artifactInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ArtifactItemDto> ArtifactItemDtos
		{
			get
			{
				return this.artifactItemDtos_;
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
		public RepeatedField<MountItemDto> MountItemDtos
		{
			get
			{
				return this.mountItemDtos_;
			}
		}

		[DebuggerNonUserCode]
		public UserStatisticInfo UserStatisticInfo
		{
			get
			{
				return this.userStatisticInfo_;
			}
			set
			{
				this.userStatisticInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<uint> Fetters
		{
			get
			{
				return this.fetters_;
			}
		}

		[DebuggerNonUserCode]
		public string Attributes
		{
			get
			{
				return this.attributes_;
			}
			set
			{
				this.attributes_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> SkillIds
		{
			get
			{
				return this.skillIds_;
			}
		}

		[DebuggerNonUserCode]
		public int SkinHeaddressId
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
		public int SkinBodyId
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
		public int SkinAccessoryId
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
		public TalentLegacyInfoDto TalentLegacyInfo
		{
			get
			{
				return this.talentLegacyInfo_;
			}
			set
			{
				this.talentLegacyInfo_ = value;
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
			if (this.Level != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Level);
			}
			if (this.Avatar != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Avatar);
			}
			if (this.AvatarFrame != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.AvatarFrame);
			}
			if (this.Power != 0L)
			{
				output.WriteRawTag(56);
				output.WriteInt64(this.Power);
			}
			if (this.ActorRowId != 0L)
			{
				output.WriteRawTag(64);
				output.WriteInt64(this.ActorRowId);
			}
			if (this.extra_ != null)
			{
				output.WriteRawTag(74);
				output.WriteMessage(this.Extra);
			}
			this.equips_.WriteTo(output, BattleUserDto._repeated_equips_codec);
			this.heros_.WriteTo(output, BattleUserDto._repeated_heros_codec);
			this.talents_.WriteTo(output, BattleUserDto._repeated_talents_codec);
			this.relics_.WriteTo(output, BattleUserDto._repeated_relics_codec);
			this.pets_.WriteTo(output, BattleUserDto._repeated_pets_codec);
			if (this.collectionInfo_ != null)
			{
				output.WriteRawTag(122);
				output.WriteMessage(this.CollectionInfo);
			}
			if (this.talentsInfo_ != null)
			{
				output.WriteRawTag(130, 1);
				output.WriteMessage(this.TalentsInfo);
			}
			if (this.artifactInfo_ != null)
			{
				output.WriteRawTag(138, 1);
				output.WriteMessage(this.ArtifactInfo);
			}
			this.artifactItemDtos_.WriteTo(output, BattleUserDto._repeated_artifactItemDtos_codec);
			if (this.mountInfo_ != null)
			{
				output.WriteRawTag(154, 1);
				output.WriteMessage(this.MountInfo);
			}
			this.mountItemDtos_.WriteTo(output, BattleUserDto._repeated_mountItemDtos_codec);
			if (this.userStatisticInfo_ != null)
			{
				output.WriteRawTag(170, 1);
				output.WriteMessage(this.UserStatisticInfo);
			}
			this.fetters_.WriteTo(output, BattleUserDto._repeated_fetters_codec);
			if (this.Attributes.Length != 0)
			{
				output.WriteRawTag(186, 1);
				output.WriteString(this.Attributes);
			}
			this.skillIds_.WriteTo(output, BattleUserDto._repeated_skillIds_codec);
			if (this.SkinHeaddressId != 0)
			{
				output.WriteRawTag(200, 1);
				output.WriteInt32(this.SkinHeaddressId);
			}
			if (this.SkinBodyId != 0)
			{
				output.WriteRawTag(208, 1);
				output.WriteInt32(this.SkinBodyId);
			}
			if (this.SkinAccessoryId != 0)
			{
				output.WriteRawTag(216, 1);
				output.WriteInt32(this.SkinAccessoryId);
			}
			if (this.talentLegacyInfo_ != null)
			{
				output.WriteRawTag(226, 1);
				output.WriteMessage(this.TalentLegacyInfo);
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
			if (this.Level != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Level);
			}
			if (this.Avatar != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Avatar);
			}
			if (this.AvatarFrame != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.AvatarFrame);
			}
			if (this.Power != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Power);
			}
			if (this.ActorRowId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.ActorRowId);
			}
			if (this.extra_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Extra);
			}
			num += this.equips_.CalculateSize(BattleUserDto._repeated_equips_codec);
			num += this.heros_.CalculateSize(BattleUserDto._repeated_heros_codec);
			num += this.talents_.CalculateSize(BattleUserDto._repeated_talents_codec);
			num += this.relics_.CalculateSize(BattleUserDto._repeated_relics_codec);
			num += this.pets_.CalculateSize(BattleUserDto._repeated_pets_codec);
			if (this.collectionInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CollectionInfo);
			}
			if (this.talentsInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.TalentsInfo);
			}
			if (this.artifactInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.ArtifactInfo);
			}
			num += this.artifactItemDtos_.CalculateSize(BattleUserDto._repeated_artifactItemDtos_codec);
			if (this.mountInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.MountInfo);
			}
			num += this.mountItemDtos_.CalculateSize(BattleUserDto._repeated_mountItemDtos_codec);
			if (this.userStatisticInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.UserStatisticInfo);
			}
			num += this.fetters_.CalculateSize(BattleUserDto._repeated_fetters_codec);
			if (this.Attributes.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.Attributes);
			}
			num += this.skillIds_.CalculateSize(BattleUserDto._repeated_skillIds_codec);
			if (this.SkinHeaddressId != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.SkinHeaddressId);
			}
			if (this.SkinBodyId != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.SkinBodyId);
			}
			if (this.SkinAccessoryId != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.SkinAccessoryId);
			}
			if (this.talentLegacyInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.TalentLegacyInfo);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num > 122U)
				{
					if (num <= 176U)
					{
						if (num <= 146U)
						{
							if (num == 130U)
							{
								if (this.talentsInfo_ == null)
								{
									this.talentsInfo_ = new TalentsInfo();
								}
								input.ReadMessage(this.talentsInfo_);
								continue;
							}
							if (num == 138U)
							{
								if (this.artifactInfo_ == null)
								{
									this.artifactInfo_ = new ArtifactInfo();
								}
								input.ReadMessage(this.artifactInfo_);
								continue;
							}
							if (num != 146U)
							{
								goto IL_019A;
							}
							this.artifactItemDtos_.AddEntriesFrom(input, BattleUserDto._repeated_artifactItemDtos_codec);
							continue;
						}
						else if (num <= 162U)
						{
							if (num == 154U)
							{
								if (this.mountInfo_ == null)
								{
									this.mountInfo_ = new MountInfo();
								}
								input.ReadMessage(this.mountInfo_);
								continue;
							}
							if (num != 162U)
							{
								goto IL_019A;
							}
							this.mountItemDtos_.AddEntriesFrom(input, BattleUserDto._repeated_mountItemDtos_codec);
							continue;
						}
						else
						{
							if (num == 170U)
							{
								if (this.userStatisticInfo_ == null)
								{
									this.userStatisticInfo_ = new UserStatisticInfo();
								}
								input.ReadMessage(this.userStatisticInfo_);
								continue;
							}
							if (num != 176U)
							{
								goto IL_019A;
							}
						}
					}
					else if (num <= 194U)
					{
						if (num <= 186U)
						{
							if (num != 178U)
							{
								if (num != 186U)
								{
									goto IL_019A;
								}
								this.Attributes = input.ReadString();
								continue;
							}
						}
						else
						{
							if (num != 192U && num != 194U)
							{
								goto IL_019A;
							}
							this.skillIds_.AddEntriesFrom(input, BattleUserDto._repeated_skillIds_codec);
							continue;
						}
					}
					else if (num <= 208U)
					{
						if (num == 200U)
						{
							this.SkinHeaddressId = input.ReadInt32();
							continue;
						}
						if (num != 208U)
						{
							goto IL_019A;
						}
						this.SkinBodyId = input.ReadInt32();
						continue;
					}
					else
					{
						if (num == 216U)
						{
							this.SkinAccessoryId = input.ReadInt32();
							continue;
						}
						if (num != 226U)
						{
							goto IL_019A;
						}
						if (this.talentLegacyInfo_ == null)
						{
							this.talentLegacyInfo_ = new TalentLegacyInfoDto();
						}
						input.ReadMessage(this.talentLegacyInfo_);
						continue;
					}
					this.fetters_.AddEntriesFrom(input, BattleUserDto._repeated_fetters_codec);
					continue;
				}
				if (num <= 64U)
				{
					if (num <= 24U)
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
						if (num == 24U)
						{
							this.Level = input.ReadInt32();
							continue;
						}
					}
					else if (num <= 40U)
					{
						if (num == 32U)
						{
							this.Avatar = input.ReadInt32();
							continue;
						}
						if (num == 40U)
						{
							this.AvatarFrame = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 56U)
						{
							this.Power = input.ReadInt64();
							continue;
						}
						if (num == 64U)
						{
							this.ActorRowId = input.ReadInt64();
							continue;
						}
					}
				}
				else if (num <= 90U)
				{
					if (num == 74U)
					{
						if (this.extra_ == null)
						{
							this.extra_ = new LordDto();
						}
						input.ReadMessage(this.extra_);
						continue;
					}
					if (num == 82U)
					{
						this.equips_.AddEntriesFrom(input, BattleUserDto._repeated_equips_codec);
						continue;
					}
					if (num == 90U)
					{
						this.heros_.AddEntriesFrom(input, BattleUserDto._repeated_heros_codec);
						continue;
					}
				}
				else if (num <= 106U)
				{
					if (num == 98U)
					{
						this.talents_.AddEntriesFrom(input, BattleUserDto._repeated_talents_codec);
						continue;
					}
					if (num == 106U)
					{
						this.relics_.AddEntriesFrom(input, BattleUserDto._repeated_relics_codec);
						continue;
					}
				}
				else
				{
					if (num == 114U)
					{
						this.pets_.AddEntriesFrom(input, BattleUserDto._repeated_pets_codec);
						continue;
					}
					if (num == 122U)
					{
						if (this.collectionInfo_ == null)
						{
							this.collectionInfo_ = new CollectionInfo();
						}
						input.ReadMessage(this.collectionInfo_);
						continue;
					}
				}
				IL_019A:
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<BattleUserDto> _parser = new MessageParser<BattleUserDto>(() => new BattleUserDto());

		public const int UserIdFieldNumber = 1;

		private long userId_;

		public const int NickNameFieldNumber = 2;

		private string nickName_ = "";

		public const int LevelFieldNumber = 3;

		private int level_;

		public const int AvatarFieldNumber = 4;

		private int avatar_;

		public const int AvatarFrameFieldNumber = 5;

		private int avatarFrame_;

		public const int PowerFieldNumber = 7;

		private long power_;

		public const int ActorRowIdFieldNumber = 8;

		private long actorRowId_;

		public const int ExtraFieldNumber = 9;

		private LordDto extra_;

		public const int EquipsFieldNumber = 10;

		private static readonly FieldCodec<EquipmentDto> _repeated_equips_codec = FieldCodec.ForMessage<EquipmentDto>(82U, EquipmentDto.Parser);

		private readonly RepeatedField<EquipmentDto> equips_ = new RepeatedField<EquipmentDto>();

		public const int HerosFieldNumber = 11;

		private static readonly FieldCodec<HeroDto> _repeated_heros_codec = FieldCodec.ForMessage<HeroDto>(90U, HeroDto.Parser);

		private readonly RepeatedField<HeroDto> heros_ = new RepeatedField<HeroDto>();

		public const int TalentsFieldNumber = 12;

		private static readonly FieldCodec<TalentSystemDto> _repeated_talents_codec = FieldCodec.ForMessage<TalentSystemDto>(98U, TalentSystemDto.Parser);

		private readonly RepeatedField<TalentSystemDto> talents_ = new RepeatedField<TalentSystemDto>();

		public const int RelicsFieldNumber = 13;

		private static readonly FieldCodec<RelicDto> _repeated_relics_codec = FieldCodec.ForMessage<RelicDto>(106U, RelicDto.Parser);

		private readonly RepeatedField<RelicDto> relics_ = new RepeatedField<RelicDto>();

		public const int PetsFieldNumber = 14;

		private static readonly FieldCodec<PetDto> _repeated_pets_codec = FieldCodec.ForMessage<PetDto>(114U, PetDto.Parser);

		private readonly RepeatedField<PetDto> pets_ = new RepeatedField<PetDto>();

		public const int CollectionInfoFieldNumber = 15;

		private CollectionInfo collectionInfo_;

		public const int TalentsInfoFieldNumber = 16;

		private TalentsInfo talentsInfo_;

		public const int ArtifactInfoFieldNumber = 17;

		private ArtifactInfo artifactInfo_;

		public const int ArtifactItemDtosFieldNumber = 18;

		private static readonly FieldCodec<ArtifactItemDto> _repeated_artifactItemDtos_codec = FieldCodec.ForMessage<ArtifactItemDto>(146U, ArtifactItemDto.Parser);

		private readonly RepeatedField<ArtifactItemDto> artifactItemDtos_ = new RepeatedField<ArtifactItemDto>();

		public const int MountInfoFieldNumber = 19;

		private MountInfo mountInfo_;

		public const int MountItemDtosFieldNumber = 20;

		private static readonly FieldCodec<MountItemDto> _repeated_mountItemDtos_codec = FieldCodec.ForMessage<MountItemDto>(162U, MountItemDto.Parser);

		private readonly RepeatedField<MountItemDto> mountItemDtos_ = new RepeatedField<MountItemDto>();

		public const int UserStatisticInfoFieldNumber = 21;

		private UserStatisticInfo userStatisticInfo_;

		public const int FettersFieldNumber = 22;

		private static readonly FieldCodec<uint> _repeated_fetters_codec = FieldCodec.ForUInt32(178U);

		private readonly RepeatedField<uint> fetters_ = new RepeatedField<uint>();

		public const int AttributesFieldNumber = 23;

		private string attributes_ = "";

		public const int SkillIdsFieldNumber = 24;

		private static readonly FieldCodec<int> _repeated_skillIds_codec = FieldCodec.ForInt32(194U);

		private readonly RepeatedField<int> skillIds_ = new RepeatedField<int>();

		public const int SkinHeaddressIdFieldNumber = 25;

		private int skinHeaddressId_;

		public const int SkinBodyIdFieldNumber = 26;

		private int skinBodyId_;

		public const int SkinAccessoryIdFieldNumber = 27;

		private int skinAccessoryId_;

		public const int TalentLegacyInfoFieldNumber = 28;

		private TalentLegacyInfoDto talentLegacyInfo_;
	}
}
