using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserLoginResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserLoginResponse> Parser
		{
			get
			{
				return UserLoginResponse._parser;
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
		public string AccessToken
		{
			get
			{
				return this.accessToken_;
			}
			set
			{
				this.accessToken_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public ulong Timestamp
		{
			get
			{
				return this.timestamp_;
			}
			set
			{
				this.timestamp_ = value;
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
		public UserCurrency UserCurrency
		{
			get
			{
				return this.userCurrency_;
			}
			set
			{
				this.userCurrency_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong SystemMask
		{
			get
			{
				return this.systemMask_;
			}
			set
			{
				this.systemMask_ = value;
			}
		}

		[DebuggerNonUserCode]
		public UserLevel UserLevel
		{
			get
			{
				return this.userLevel_;
			}
			set
			{
				this.userLevel_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong GuideMask
		{
			get
			{
				return this.guideMask_;
			}
			set
			{
				this.guideMask_ = value;
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
		public ulong RegisterTimestamp
		{
			get
			{
				return this.registerTimestamp_;
			}
			set
			{
				this.registerTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ItemDto> Items
		{
			get
			{
				return this.items_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<EquipmentDto> Equipments
		{
			get
			{
				return this.equipments_;
			}
		}

		[DebuggerNonUserCode]
		public UserMission UserMission
		{
			get
			{
				return this.userMission_;
			}
			set
			{
				this.userMission_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ulong> WearEquipRowIds
		{
			get
			{
				return this.wearEquipRowIds_;
			}
		}

		[DebuggerNonUserCode]
		public ulong TransId
		{
			get
			{
				return this.transId_;
			}
			set
			{
				this.transId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public AdDataDto AdData
		{
			get
			{
				return this.adData_;
			}
			set
			{
				this.adData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public UserInfoDto UserInfoDto
		{
			get
			{
				return this.userInfoDto_;
			}
			set
			{
				this.userInfoDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CityDto City
		{
			get
			{
				return this.city_;
			}
			set
			{
				this.city_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ChapterDto Chapter
		{
			get
			{
				return this.chapter_;
			}
			set
			{
				this.chapter_ = value;
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
		public ActorDto Actor
		{
			get
			{
				return this.actor_;
			}
			set
			{
				this.actor_ = value;
			}
		}

		[DebuggerNonUserCode]
		public LordDto Lord
		{
			get
			{
				return this.lord_;
			}
			set
			{
				this.lord_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint SlaveCount
		{
			get
			{
				return this.slaveCount_;
			}
			set
			{
				this.slaveCount_ = value;
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
		public IntegralShopDto IntegralShops
		{
			get
			{
				return this.integralShops_;
			}
			set
			{
				this.integralShops_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Tower
		{
			get
			{
				return this.tower_;
			}
			set
			{
				this.tower_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TowerReward
		{
			get
			{
				return this.towerReward_;
			}
			set
			{
				this.towerReward_ = value;
			}
		}

		[DebuggerNonUserCode]
		public UserVipLevel UserVipLevel
		{
			get
			{
				return this.userVipLevel_;
			}
			set
			{
				this.userVipLevel_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<UserTicket> UserTickets
		{
			get
			{
				return this.userTickets_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<uint> OpenModelIdList
		{
			get
			{
				return this.openModelIdList_;
			}
		}

		[DebuggerNonUserCode]
		public PetInfo PetInfo
		{
			get
			{
				return this.petInfo_;
			}
			set
			{
				this.petInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public TalentInfo TalentInfo
		{
			get
			{
				return this.talentInfo_;
			}
			set
			{
				this.talentInfo_ = value;
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
		public RepeatedField<ActiveInfo> ActiveInfo
		{
			get
			{
				return this.activeInfo_;
			}
		}

		[DebuggerNonUserCode]
		public ShopDrawDto ShopDrawDto
		{
			get
			{
				return this.shopDrawDto_;
			}
			set
			{
				this.shopDrawDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ChapterActiveRankInfo RewardInfo
		{
			get
			{
				return this.rewardInfo_;
			}
			set
			{
				this.rewardInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ChestInfo ChestInfo
		{
			get
			{
				return this.chestInfo_;
			}
			set
			{
				this.chestInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<DungeonInfo> DungeonInfo
		{
			get
			{
				return this.dungeonInfo_;
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
		public bool IsNewUser
		{
			get
			{
				return this.isNewUser_;
			}
			set
			{
				this.isNewUser_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong DataRefreshTimestamp
		{
			get
			{
				return this.dataRefreshTimestamp_;
			}
			set
			{
				this.dataRefreshTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string TimeZone
		{
			get
			{
				return this.timeZone_;
			}
			set
			{
				this.timeZone_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public ShopAllDataDto ShopAllDataDto
		{
			get
			{
				return this.shopAllDataDto_;
			}
			set
			{
				this.shopAllDataDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public HungUpInfoDto HungUpInfoDto
		{
			get
			{
				return this.hungUpInfoDto_;
			}
			set
			{
				this.hungUpInfoDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public PushIapDto PushIapDto
		{
			get
			{
				return this.pushIapDto_;
			}
			set
			{
				this.pushIapDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ServerId
		{
			get
			{
				return this.serverId_;
			}
			set
			{
				this.serverId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint HellStage
		{
			get
			{
				return this.hellStage_;
			}
			set
			{
				this.hellStage_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint HellBattleStatus
		{
			get
			{
				return this.hellBattleStatus_;
			}
			set
			{
				this.hellBattleStatus_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong OpenServerTime
		{
			get
			{
				return this.openServerTime_;
			}
			set
			{
				this.openServerTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong OpenServerResetTime
		{
			get
			{
				return this.openServerResetTime_;
			}
			set
			{
				this.openServerResetTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string AccountKey
		{
			get
			{
				return this.accountKey_;
			}
			set
			{
				this.accountKey_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public TGAInfoDto TgaInfoDto
		{
			get
			{
				return this.tgaInfoDto_;
			}
			set
			{
				this.tgaInfoDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool IsForceJump
		{
			get
			{
				return this.isForceJump_;
			}
			set
			{
				this.isForceJump_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint AbVersion
		{
			get
			{
				return this.abVersion_;
			}
			set
			{
				this.abVersion_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool HabbyMailBind
		{
			get
			{
				return this.habbyMailBind_;
			}
			set
			{
				this.habbyMailBind_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool HabbyMailReward
		{
			get
			{
				return this.habbyMailReward_;
			}
			set
			{
				this.habbyMailReward_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string ServerIMGroupId
		{
			get
			{
				return this.serverIMGroupId_;
			}
			set
			{
				this.serverIMGroupId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public int EnterNewWorld
		{
			get
			{
				return this.enterNewWorld_;
			}
			set
			{
				this.enterNewWorld_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string HabbyId
		{
			get
			{
				return this.habbyId_;
			}
			set
			{
				this.habbyId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string CrossServerIMGroupId
		{
			get
			{
				return this.crossServerIMGroupId_;
			}
			set
			{
				this.crossServerIMGroupId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public long FreeAdLifeRefreshtime
		{
			get
			{
				return this.freeAdLifeRefreshtime_;
			}
			set
			{
				this.freeAdLifeRefreshtime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ChapterActiveWheelInfo WheelInfo
		{
			get
			{
				return this.wheelInfo_;
			}
			set
			{
				this.wheelInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ChainActvDto> PushChainDto
		{
			get
			{
				return this.pushChainDto_;
			}
		}

		[DebuggerNonUserCode]
		public int ShopSupCount
		{
			get
			{
				return this.shopSupCount_;
			}
			set
			{
				this.shopSupCount_ = value;
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
			if (this.AccessToken.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.AccessToken);
			}
			if (this.Timestamp != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.Timestamp);
			}
			if (this.UserId != 0L)
			{
				output.WriteRawTag(32);
				output.WriteInt64(this.UserId);
			}
			if (this.userCurrency_ != null)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.UserCurrency);
			}
			if (this.SystemMask != 0UL)
			{
				output.WriteRawTag(48);
				output.WriteUInt64(this.SystemMask);
			}
			if (this.userLevel_ != null)
			{
				output.WriteRawTag(58);
				output.WriteMessage(this.UserLevel);
			}
			if (this.GuideMask != 0UL)
			{
				output.WriteRawTag(64);
				output.WriteUInt64(this.GuideMask);
			}
			if (this.commonData_ != null)
			{
				output.WriteRawTag(74);
				output.WriteMessage(this.CommonData);
			}
			if (this.RegisterTimestamp != 0UL)
			{
				output.WriteRawTag(80);
				output.WriteUInt64(this.RegisterTimestamp);
			}
			this.items_.WriteTo(output, UserLoginResponse._repeated_items_codec);
			this.equipments_.WriteTo(output, UserLoginResponse._repeated_equipments_codec);
			if (this.userMission_ != null)
			{
				output.WriteRawTag(106);
				output.WriteMessage(this.UserMission);
			}
			this.wearEquipRowIds_.WriteTo(output, UserLoginResponse._repeated_wearEquipRowIds_codec);
			if (this.TransId != 0UL)
			{
				output.WriteRawTag(120);
				output.WriteUInt64(this.TransId);
			}
			if (this.adData_ != null)
			{
				output.WriteRawTag(130, 1);
				output.WriteMessage(this.AdData);
			}
			if (this.userInfoDto_ != null)
			{
				output.WriteRawTag(138, 1);
				output.WriteMessage(this.UserInfoDto);
			}
			if (this.city_ != null)
			{
				output.WriteRawTag(146, 1);
				output.WriteMessage(this.City);
			}
			if (this.chapter_ != null)
			{
				output.WriteRawTag(154, 1);
				output.WriteMessage(this.Chapter);
			}
			this.heros_.WriteTo(output, UserLoginResponse._repeated_heros_codec);
			if (this.actor_ != null)
			{
				output.WriteRawTag(170, 1);
				output.WriteMessage(this.Actor);
			}
			if (this.lord_ != null)
			{
				output.WriteRawTag(186, 1);
				output.WriteMessage(this.Lord);
			}
			if (this.SlaveCount != 0U)
			{
				output.WriteRawTag(192, 1);
				output.WriteUInt32(this.SlaveCount);
			}
			this.relics_.WriteTo(output, UserLoginResponse._repeated_relics_codec);
			if (this.integralShops_ != null)
			{
				output.WriteRawTag(210, 1);
				output.WriteMessage(this.IntegralShops);
			}
			if (this.Tower != 0U)
			{
				output.WriteRawTag(216, 1);
				output.WriteUInt32(this.Tower);
			}
			if (this.TowerReward != 0U)
			{
				output.WriteRawTag(224, 1);
				output.WriteUInt32(this.TowerReward);
			}
			if (this.userVipLevel_ != null)
			{
				output.WriteRawTag(234, 1);
				output.WriteMessage(this.UserVipLevel);
			}
			this.userTickets_.WriteTo(output, UserLoginResponse._repeated_userTickets_codec);
			this.openModelIdList_.WriteTo(output, UserLoginResponse._repeated_openModelIdList_codec);
			if (this.petInfo_ != null)
			{
				output.WriteRawTag(130, 2);
				output.WriteMessage(this.PetInfo);
			}
			if (this.talentInfo_ != null)
			{
				output.WriteRawTag(138, 2);
				output.WriteMessage(this.TalentInfo);
			}
			if (this.collectionInfo_ != null)
			{
				output.WriteRawTag(146, 2);
				output.WriteMessage(this.CollectionInfo);
			}
			if (this.talentsInfo_ != null)
			{
				output.WriteRawTag(154, 2);
				output.WriteMessage(this.TalentsInfo);
			}
			this.activeInfo_.WriteTo(output, UserLoginResponse._repeated_activeInfo_codec);
			if (this.shopDrawDto_ != null)
			{
				output.WriteRawTag(170, 2);
				output.WriteMessage(this.ShopDrawDto);
			}
			if (this.rewardInfo_ != null)
			{
				output.WriteRawTag(178, 2);
				output.WriteMessage(this.RewardInfo);
			}
			if (this.chestInfo_ != null)
			{
				output.WriteRawTag(186, 2);
				output.WriteMessage(this.ChestInfo);
			}
			this.dungeonInfo_.WriteTo(output, UserLoginResponse._repeated_dungeonInfo_codec);
			if (this.artifactInfo_ != null)
			{
				output.WriteRawTag(202, 2);
				output.WriteMessage(this.ArtifactInfo);
			}
			this.artifactItemDtos_.WriteTo(output, UserLoginResponse._repeated_artifactItemDtos_codec);
			if (this.mountInfo_ != null)
			{
				output.WriteRawTag(218, 2);
				output.WriteMessage(this.MountInfo);
			}
			this.mountItemDtos_.WriteTo(output, UserLoginResponse._repeated_mountItemDtos_codec);
			if (this.userStatisticInfo_ != null)
			{
				output.WriteRawTag(234, 2);
				output.WriteMessage(this.UserStatisticInfo);
			}
			if (this.IsNewUser)
			{
				output.WriteRawTag(240, 2);
				output.WriteBool(this.IsNewUser);
			}
			if (this.DataRefreshTimestamp != 0UL)
			{
				output.WriteRawTag(248, 2);
				output.WriteUInt64(this.DataRefreshTimestamp);
			}
			if (this.TimeZone.Length != 0)
			{
				output.WriteRawTag(130, 3);
				output.WriteString(this.TimeZone);
			}
			if (this.shopAllDataDto_ != null)
			{
				output.WriteRawTag(138, 3);
				output.WriteMessage(this.ShopAllDataDto);
			}
			if (this.hungUpInfoDto_ != null)
			{
				output.WriteRawTag(146, 3);
				output.WriteMessage(this.HungUpInfoDto);
			}
			if (this.pushIapDto_ != null)
			{
				output.WriteRawTag(154, 3);
				output.WriteMessage(this.PushIapDto);
			}
			if (this.ServerId != 0U)
			{
				output.WriteRawTag(176, 3);
				output.WriteUInt32(this.ServerId);
			}
			if (this.HellStage != 0U)
			{
				output.WriteRawTag(184, 3);
				output.WriteUInt32(this.HellStage);
			}
			if (this.HellBattleStatus != 0U)
			{
				output.WriteRawTag(192, 3);
				output.WriteUInt32(this.HellBattleStatus);
			}
			if (this.OpenServerTime != 0UL)
			{
				output.WriteRawTag(208, 3);
				output.WriteUInt64(this.OpenServerTime);
			}
			if (this.OpenServerResetTime != 0UL)
			{
				output.WriteRawTag(216, 3);
				output.WriteUInt64(this.OpenServerResetTime);
			}
			if (this.AccountKey.Length != 0)
			{
				output.WriteRawTag(226, 3);
				output.WriteString(this.AccountKey);
			}
			if (this.tgaInfoDto_ != null)
			{
				output.WriteRawTag(234, 3);
				output.WriteMessage(this.TgaInfoDto);
			}
			if (this.IsForceJump)
			{
				output.WriteRawTag(240, 3);
				output.WriteBool(this.IsForceJump);
			}
			if (this.AbVersion != 0U)
			{
				output.WriteRawTag(248, 3);
				output.WriteUInt32(this.AbVersion);
			}
			if (this.HabbyMailBind)
			{
				output.WriteRawTag(128, 4);
				output.WriteBool(this.HabbyMailBind);
			}
			if (this.HabbyMailReward)
			{
				output.WriteRawTag(136, 4);
				output.WriteBool(this.HabbyMailReward);
			}
			if (this.ServerIMGroupId.Length != 0)
			{
				output.WriteRawTag(146, 4);
				output.WriteString(this.ServerIMGroupId);
			}
			if (this.EnterNewWorld != 0)
			{
				output.WriteRawTag(152, 4);
				output.WriteInt32(this.EnterNewWorld);
			}
			if (this.HabbyId.Length != 0)
			{
				output.WriteRawTag(162, 4);
				output.WriteString(this.HabbyId);
			}
			if (this.CrossServerIMGroupId.Length != 0)
			{
				output.WriteRawTag(170, 4);
				output.WriteString(this.CrossServerIMGroupId);
			}
			if (this.FreeAdLifeRefreshtime != 0L)
			{
				output.WriteRawTag(176, 4);
				output.WriteInt64(this.FreeAdLifeRefreshtime);
			}
			if (this.wheelInfo_ != null)
			{
				output.WriteRawTag(186, 4);
				output.WriteMessage(this.WheelInfo);
			}
			this.pushChainDto_.WriteTo(output, UserLoginResponse._repeated_pushChainDto_codec);
			if (this.ShopSupCount != 0)
			{
				output.WriteRawTag(200, 4);
				output.WriteInt32(this.ShopSupCount);
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
			if (this.AccessToken.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AccessToken);
			}
			if (this.Timestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Timestamp);
			}
			if (this.UserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId);
			}
			if (this.userCurrency_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserCurrency);
			}
			if (this.SystemMask != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.SystemMask);
			}
			if (this.userLevel_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserLevel);
			}
			if (this.GuideMask != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.GuideMask);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			if (this.RegisterTimestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RegisterTimestamp);
			}
			num += this.items_.CalculateSize(UserLoginResponse._repeated_items_codec);
			num += this.equipments_.CalculateSize(UserLoginResponse._repeated_equipments_codec);
			if (this.userMission_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserMission);
			}
			num += this.wearEquipRowIds_.CalculateSize(UserLoginResponse._repeated_wearEquipRowIds_codec);
			if (this.TransId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.TransId);
			}
			if (this.adData_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.AdData);
			}
			if (this.userInfoDto_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.UserInfoDto);
			}
			if (this.city_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.City);
			}
			if (this.chapter_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.Chapter);
			}
			num += this.heros_.CalculateSize(UserLoginResponse._repeated_heros_codec);
			if (this.actor_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.Actor);
			}
			if (this.lord_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.Lord);
			}
			if (this.SlaveCount != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.SlaveCount);
			}
			num += this.relics_.CalculateSize(UserLoginResponse._repeated_relics_codec);
			if (this.integralShops_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.IntegralShops);
			}
			if (this.Tower != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.Tower);
			}
			if (this.TowerReward != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.TowerReward);
			}
			if (this.userVipLevel_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.UserVipLevel);
			}
			num += this.userTickets_.CalculateSize(UserLoginResponse._repeated_userTickets_codec);
			num += this.openModelIdList_.CalculateSize(UserLoginResponse._repeated_openModelIdList_codec);
			if (this.petInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.PetInfo);
			}
			if (this.talentInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.TalentInfo);
			}
			if (this.collectionInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.CollectionInfo);
			}
			if (this.talentsInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.TalentsInfo);
			}
			num += this.activeInfo_.CalculateSize(UserLoginResponse._repeated_activeInfo_codec);
			if (this.shopDrawDto_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.ShopDrawDto);
			}
			if (this.rewardInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.RewardInfo);
			}
			if (this.chestInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.ChestInfo);
			}
			num += this.dungeonInfo_.CalculateSize(UserLoginResponse._repeated_dungeonInfo_codec);
			if (this.artifactInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.ArtifactInfo);
			}
			num += this.artifactItemDtos_.CalculateSize(UserLoginResponse._repeated_artifactItemDtos_codec);
			if (this.mountInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.MountInfo);
			}
			num += this.mountItemDtos_.CalculateSize(UserLoginResponse._repeated_mountItemDtos_codec);
			if (this.userStatisticInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.UserStatisticInfo);
			}
			if (this.IsNewUser)
			{
				num += 3;
			}
			if (this.DataRefreshTimestamp != 0UL)
			{
				num += 2 + CodedOutputStream.ComputeUInt64Size(this.DataRefreshTimestamp);
			}
			if (this.TimeZone.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.TimeZone);
			}
			if (this.shopAllDataDto_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.ShopAllDataDto);
			}
			if (this.hungUpInfoDto_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.HungUpInfoDto);
			}
			if (this.pushIapDto_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.PushIapDto);
			}
			if (this.ServerId != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.ServerId);
			}
			if (this.HellStage != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.HellStage);
			}
			if (this.HellBattleStatus != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.HellBattleStatus);
			}
			if (this.OpenServerTime != 0UL)
			{
				num += 2 + CodedOutputStream.ComputeUInt64Size(this.OpenServerTime);
			}
			if (this.OpenServerResetTime != 0UL)
			{
				num += 2 + CodedOutputStream.ComputeUInt64Size(this.OpenServerResetTime);
			}
			if (this.AccountKey.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.AccountKey);
			}
			if (this.tgaInfoDto_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.TgaInfoDto);
			}
			if (this.IsForceJump)
			{
				num += 3;
			}
			if (this.AbVersion != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.AbVersion);
			}
			if (this.HabbyMailBind)
			{
				num += 3;
			}
			if (this.HabbyMailReward)
			{
				num += 3;
			}
			if (this.ServerIMGroupId.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.ServerIMGroupId);
			}
			if (this.EnterNewWorld != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.EnterNewWorld);
			}
			if (this.HabbyId.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.HabbyId);
			}
			if (this.CrossServerIMGroupId.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.CrossServerIMGroupId);
			}
			if (this.FreeAdLifeRefreshtime != 0L)
			{
				num += 2 + CodedOutputStream.ComputeInt64Size(this.FreeAdLifeRefreshtime);
			}
			if (this.wheelInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.WheelInfo);
			}
			num += this.pushChainDto_.CalculateSize(UserLoginResponse._repeated_pushChainDto_codec);
			if (this.ShopSupCount != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.ShopSupCount);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 274U)
				{
					if (num <= 130U)
					{
						if (num <= 64U)
						{
							if (num <= 32U)
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
										this.AccessToken = input.ReadString();
										continue;
									}
								}
								else
								{
									if (num == 24U)
									{
										this.Timestamp = input.ReadUInt64();
										continue;
									}
									if (num == 32U)
									{
										this.UserId = input.ReadInt64();
										continue;
									}
								}
							}
							else if (num <= 48U)
							{
								if (num == 42U)
								{
									if (this.userCurrency_ == null)
									{
										this.userCurrency_ = new UserCurrency();
									}
									input.ReadMessage(this.userCurrency_);
									continue;
								}
								if (num == 48U)
								{
									this.SystemMask = input.ReadUInt64();
									continue;
								}
							}
							else
							{
								if (num == 58U)
								{
									if (this.userLevel_ == null)
									{
										this.userLevel_ = new UserLevel();
									}
									input.ReadMessage(this.userLevel_);
									continue;
								}
								if (num == 64U)
								{
									this.GuideMask = input.ReadUInt64();
									continue;
								}
							}
						}
						else
						{
							if (num > 98U)
							{
								if (num <= 112U)
								{
									if (num == 106U)
									{
										if (this.userMission_ == null)
										{
											this.userMission_ = new UserMission();
										}
										input.ReadMessage(this.userMission_);
										continue;
									}
									if (num != 112U)
									{
										goto IL_0468;
									}
								}
								else if (num != 114U)
								{
									if (num == 120U)
									{
										this.TransId = input.ReadUInt64();
										continue;
									}
									if (num != 130U)
									{
										goto IL_0468;
									}
									if (this.adData_ == null)
									{
										this.adData_ = new AdDataDto();
									}
									input.ReadMessage(this.adData_);
									continue;
								}
								this.wearEquipRowIds_.AddEntriesFrom(input, UserLoginResponse._repeated_wearEquipRowIds_codec);
								continue;
							}
							if (num <= 80U)
							{
								if (num == 74U)
								{
									if (this.commonData_ == null)
									{
										this.commonData_ = new CommonData();
									}
									input.ReadMessage(this.commonData_);
									continue;
								}
								if (num == 80U)
								{
									this.RegisterTimestamp = input.ReadUInt64();
									continue;
								}
							}
							else
							{
								if (num == 90U)
								{
									this.items_.AddEntriesFrom(input, UserLoginResponse._repeated_items_codec);
									continue;
								}
								if (num == 98U)
								{
									this.equipments_.AddEntriesFrom(input, UserLoginResponse._repeated_equipments_codec);
									continue;
								}
							}
						}
					}
					else if (num <= 210U)
					{
						if (num <= 162U)
						{
							if (num <= 146U)
							{
								if (num == 138U)
								{
									if (this.userInfoDto_ == null)
									{
										this.userInfoDto_ = new UserInfoDto();
									}
									input.ReadMessage(this.userInfoDto_);
									continue;
								}
								if (num == 146U)
								{
									if (this.city_ == null)
									{
										this.city_ = new CityDto();
									}
									input.ReadMessage(this.city_);
									continue;
								}
							}
							else
							{
								if (num == 154U)
								{
									if (this.chapter_ == null)
									{
										this.chapter_ = new ChapterDto();
									}
									input.ReadMessage(this.chapter_);
									continue;
								}
								if (num == 162U)
								{
									this.heros_.AddEntriesFrom(input, UserLoginResponse._repeated_heros_codec);
									continue;
								}
							}
						}
						else if (num <= 186U)
						{
							if (num == 170U)
							{
								if (this.actor_ == null)
								{
									this.actor_ = new ActorDto();
								}
								input.ReadMessage(this.actor_);
								continue;
							}
							if (num == 186U)
							{
								if (this.lord_ == null)
								{
									this.lord_ = new LordDto();
								}
								input.ReadMessage(this.lord_);
								continue;
							}
						}
						else
						{
							if (num == 192U)
							{
								this.SlaveCount = input.ReadUInt32();
								continue;
							}
							if (num == 202U)
							{
								this.relics_.AddEntriesFrom(input, UserLoginResponse._repeated_relics_codec);
								continue;
							}
							if (num == 210U)
							{
								if (this.integralShops_ == null)
								{
									this.integralShops_ = new IntegralShopDto();
								}
								input.ReadMessage(this.integralShops_);
								continue;
							}
						}
					}
					else if (num <= 242U)
					{
						if (num <= 224U)
						{
							if (num == 216U)
							{
								this.Tower = input.ReadUInt32();
								continue;
							}
							if (num == 224U)
							{
								this.TowerReward = input.ReadUInt32();
								continue;
							}
						}
						else
						{
							if (num == 234U)
							{
								if (this.userVipLevel_ == null)
								{
									this.userVipLevel_ = new UserVipLevel();
								}
								input.ReadMessage(this.userVipLevel_);
								continue;
							}
							if (num == 242U)
							{
								this.userTickets_.AddEntriesFrom(input, UserLoginResponse._repeated_userTickets_codec);
								continue;
							}
						}
					}
					else if (num <= 250U)
					{
						if (num == 248U || num == 250U)
						{
							this.openModelIdList_.AddEntriesFrom(input, UserLoginResponse._repeated_openModelIdList_codec);
							continue;
						}
					}
					else
					{
						if (num == 258U)
						{
							if (this.petInfo_ == null)
							{
								this.petInfo_ = new PetInfo();
							}
							input.ReadMessage(this.petInfo_);
							continue;
						}
						if (num == 266U)
						{
							if (this.talentInfo_ == null)
							{
								this.talentInfo_ = new TalentInfo();
							}
							input.ReadMessage(this.talentInfo_);
							continue;
						}
						if (num == 274U)
						{
							if (this.collectionInfo_ == null)
							{
								this.collectionInfo_ = new CollectionInfo();
							}
							input.ReadMessage(this.collectionInfo_);
							continue;
						}
					}
				}
				else if (num <= 432U)
				{
					if (num <= 346U)
					{
						if (num <= 306U)
						{
							if (num <= 290U)
							{
								if (num == 282U)
								{
									if (this.talentsInfo_ == null)
									{
										this.talentsInfo_ = new TalentsInfo();
									}
									input.ReadMessage(this.talentsInfo_);
									continue;
								}
								if (num == 290U)
								{
									this.activeInfo_.AddEntriesFrom(input, UserLoginResponse._repeated_activeInfo_codec);
									continue;
								}
							}
							else
							{
								if (num == 298U)
								{
									if (this.shopDrawDto_ == null)
									{
										this.shopDrawDto_ = new ShopDrawDto();
									}
									input.ReadMessage(this.shopDrawDto_);
									continue;
								}
								if (num == 306U)
								{
									if (this.rewardInfo_ == null)
									{
										this.rewardInfo_ = new ChapterActiveRankInfo();
									}
									input.ReadMessage(this.rewardInfo_);
									continue;
								}
							}
						}
						else if (num <= 322U)
						{
							if (num == 314U)
							{
								if (this.chestInfo_ == null)
								{
									this.chestInfo_ = new ChestInfo();
								}
								input.ReadMessage(this.chestInfo_);
								continue;
							}
							if (num == 322U)
							{
								this.dungeonInfo_.AddEntriesFrom(input, UserLoginResponse._repeated_dungeonInfo_codec);
								continue;
							}
						}
						else
						{
							if (num == 330U)
							{
								if (this.artifactInfo_ == null)
								{
									this.artifactInfo_ = new ArtifactInfo();
								}
								input.ReadMessage(this.artifactInfo_);
								continue;
							}
							if (num == 338U)
							{
								this.artifactItemDtos_.AddEntriesFrom(input, UserLoginResponse._repeated_artifactItemDtos_codec);
								continue;
							}
							if (num == 346U)
							{
								if (this.mountInfo_ == null)
								{
									this.mountInfo_ = new MountInfo();
								}
								input.ReadMessage(this.mountInfo_);
								continue;
							}
						}
					}
					else if (num <= 376U)
					{
						if (num <= 362U)
						{
							if (num == 354U)
							{
								this.mountItemDtos_.AddEntriesFrom(input, UserLoginResponse._repeated_mountItemDtos_codec);
								continue;
							}
							if (num == 362U)
							{
								if (this.userStatisticInfo_ == null)
								{
									this.userStatisticInfo_ = new UserStatisticInfo();
								}
								input.ReadMessage(this.userStatisticInfo_);
								continue;
							}
						}
						else
						{
							if (num == 368U)
							{
								this.IsNewUser = input.ReadBool();
								continue;
							}
							if (num == 376U)
							{
								this.DataRefreshTimestamp = input.ReadUInt64();
								continue;
							}
						}
					}
					else if (num <= 394U)
					{
						if (num == 386U)
						{
							this.TimeZone = input.ReadString();
							continue;
						}
						if (num == 394U)
						{
							if (this.shopAllDataDto_ == null)
							{
								this.shopAllDataDto_ = new ShopAllDataDto();
							}
							input.ReadMessage(this.shopAllDataDto_);
							continue;
						}
					}
					else
					{
						if (num == 402U)
						{
							if (this.hungUpInfoDto_ == null)
							{
								this.hungUpInfoDto_ = new HungUpInfoDto();
							}
							input.ReadMessage(this.hungUpInfoDto_);
							continue;
						}
						if (num == 410U)
						{
							if (this.pushIapDto_ == null)
							{
								this.pushIapDto_ = new PushIapDto();
							}
							input.ReadMessage(this.pushIapDto_);
							continue;
						}
						if (num == 432U)
						{
							this.ServerId = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 512U)
				{
					if (num <= 472U)
					{
						if (num <= 448U)
						{
							if (num == 440U)
							{
								this.HellStage = input.ReadUInt32();
								continue;
							}
							if (num == 448U)
							{
								this.HellBattleStatus = input.ReadUInt32();
								continue;
							}
						}
						else
						{
							if (num == 464U)
							{
								this.OpenServerTime = input.ReadUInt64();
								continue;
							}
							if (num == 472U)
							{
								this.OpenServerResetTime = input.ReadUInt64();
								continue;
							}
						}
					}
					else if (num <= 490U)
					{
						if (num == 482U)
						{
							this.AccountKey = input.ReadString();
							continue;
						}
						if (num == 490U)
						{
							if (this.tgaInfoDto_ == null)
							{
								this.tgaInfoDto_ = new TGAInfoDto();
							}
							input.ReadMessage(this.tgaInfoDto_);
							continue;
						}
					}
					else
					{
						if (num == 496U)
						{
							this.IsForceJump = input.ReadBool();
							continue;
						}
						if (num == 504U)
						{
							this.AbVersion = input.ReadUInt32();
							continue;
						}
						if (num == 512U)
						{
							this.HabbyMailBind = input.ReadBool();
							continue;
						}
					}
				}
				else if (num <= 546U)
				{
					if (num <= 530U)
					{
						if (num == 520U)
						{
							this.HabbyMailReward = input.ReadBool();
							continue;
						}
						if (num == 530U)
						{
							this.ServerIMGroupId = input.ReadString();
							continue;
						}
					}
					else
					{
						if (num == 536U)
						{
							this.EnterNewWorld = input.ReadInt32();
							continue;
						}
						if (num == 546U)
						{
							this.HabbyId = input.ReadString();
							continue;
						}
					}
				}
				else if (num <= 560U)
				{
					if (num == 554U)
					{
						this.CrossServerIMGroupId = input.ReadString();
						continue;
					}
					if (num == 560U)
					{
						this.FreeAdLifeRefreshtime = input.ReadInt64();
						continue;
					}
				}
				else
				{
					if (num == 570U)
					{
						if (this.wheelInfo_ == null)
						{
							this.wheelInfo_ = new ChapterActiveWheelInfo();
						}
						input.ReadMessage(this.wheelInfo_);
						continue;
					}
					if (num == 578U)
					{
						this.pushChainDto_.AddEntriesFrom(input, UserLoginResponse._repeated_pushChainDto_codec);
						continue;
					}
					if (num == 584U)
					{
						this.ShopSupCount = input.ReadInt32();
						continue;
					}
				}
				IL_0468:
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UserLoginResponse> _parser = new MessageParser<UserLoginResponse>(() => new UserLoginResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int AccessTokenFieldNumber = 2;

		private string accessToken_ = "";

		public const int TimestampFieldNumber = 3;

		private ulong timestamp_;

		public const int UserIdFieldNumber = 4;

		private long userId_;

		public const int UserCurrencyFieldNumber = 5;

		private UserCurrency userCurrency_;

		public const int SystemMaskFieldNumber = 6;

		private ulong systemMask_;

		public const int UserLevelFieldNumber = 7;

		private UserLevel userLevel_;

		public const int GuideMaskFieldNumber = 8;

		private ulong guideMask_;

		public const int CommonDataFieldNumber = 9;

		private CommonData commonData_;

		public const int RegisterTimestampFieldNumber = 10;

		private ulong registerTimestamp_;

		public const int ItemsFieldNumber = 11;

		private static readonly FieldCodec<ItemDto> _repeated_items_codec = FieldCodec.ForMessage<ItemDto>(90U, ItemDto.Parser);

		private readonly RepeatedField<ItemDto> items_ = new RepeatedField<ItemDto>();

		public const int EquipmentsFieldNumber = 12;

		private static readonly FieldCodec<EquipmentDto> _repeated_equipments_codec = FieldCodec.ForMessage<EquipmentDto>(98U, EquipmentDto.Parser);

		private readonly RepeatedField<EquipmentDto> equipments_ = new RepeatedField<EquipmentDto>();

		public const int UserMissionFieldNumber = 13;

		private UserMission userMission_;

		public const int WearEquipRowIdsFieldNumber = 14;

		private static readonly FieldCodec<ulong> _repeated_wearEquipRowIds_codec = FieldCodec.ForUInt64(114U);

		private readonly RepeatedField<ulong> wearEquipRowIds_ = new RepeatedField<ulong>();

		public const int TransIdFieldNumber = 15;

		private ulong transId_;

		public const int AdDataFieldNumber = 16;

		private AdDataDto adData_;

		public const int UserInfoDtoFieldNumber = 17;

		private UserInfoDto userInfoDto_;

		public const int CityFieldNumber = 18;

		private CityDto city_;

		public const int ChapterFieldNumber = 19;

		private ChapterDto chapter_;

		public const int HerosFieldNumber = 20;

		private static readonly FieldCodec<HeroDto> _repeated_heros_codec = FieldCodec.ForMessage<HeroDto>(162U, HeroDto.Parser);

		private readonly RepeatedField<HeroDto> heros_ = new RepeatedField<HeroDto>();

		public const int ActorFieldNumber = 21;

		private ActorDto actor_;

		public const int LordFieldNumber = 23;

		private LordDto lord_;

		public const int SlaveCountFieldNumber = 24;

		private uint slaveCount_;

		public const int RelicsFieldNumber = 25;

		private static readonly FieldCodec<RelicDto> _repeated_relics_codec = FieldCodec.ForMessage<RelicDto>(202U, RelicDto.Parser);

		private readonly RepeatedField<RelicDto> relics_ = new RepeatedField<RelicDto>();

		public const int IntegralShopsFieldNumber = 26;

		private IntegralShopDto integralShops_;

		public const int TowerFieldNumber = 27;

		private uint tower_;

		public const int TowerRewardFieldNumber = 28;

		private uint towerReward_;

		public const int UserVipLevelFieldNumber = 29;

		private UserVipLevel userVipLevel_;

		public const int UserTicketsFieldNumber = 30;

		private static readonly FieldCodec<UserTicket> _repeated_userTickets_codec = FieldCodec.ForMessage<UserTicket>(242U, UserTicket.Parser);

		private readonly RepeatedField<UserTicket> userTickets_ = new RepeatedField<UserTicket>();

		public const int OpenModelIdListFieldNumber = 31;

		private static readonly FieldCodec<uint> _repeated_openModelIdList_codec = FieldCodec.ForUInt32(250U);

		private readonly RepeatedField<uint> openModelIdList_ = new RepeatedField<uint>();

		public const int PetInfoFieldNumber = 32;

		private PetInfo petInfo_;

		public const int TalentInfoFieldNumber = 33;

		private TalentInfo talentInfo_;

		public const int CollectionInfoFieldNumber = 34;

		private CollectionInfo collectionInfo_;

		public const int TalentsInfoFieldNumber = 35;

		private TalentsInfo talentsInfo_;

		public const int ActiveInfoFieldNumber = 36;

		private static readonly FieldCodec<ActiveInfo> _repeated_activeInfo_codec = FieldCodec.ForMessage<ActiveInfo>(290U, Proto.Common.ActiveInfo.Parser);

		private readonly RepeatedField<ActiveInfo> activeInfo_ = new RepeatedField<ActiveInfo>();

		public const int ShopDrawDtoFieldNumber = 37;

		private ShopDrawDto shopDrawDto_;

		public const int RewardInfoFieldNumber = 38;

		private ChapterActiveRankInfo rewardInfo_;

		public const int ChestInfoFieldNumber = 39;

		private ChestInfo chestInfo_;

		public const int DungeonInfoFieldNumber = 40;

		private static readonly FieldCodec<DungeonInfo> _repeated_dungeonInfo_codec = FieldCodec.ForMessage<DungeonInfo>(322U, Proto.Common.DungeonInfo.Parser);

		private readonly RepeatedField<DungeonInfo> dungeonInfo_ = new RepeatedField<DungeonInfo>();

		public const int ArtifactInfoFieldNumber = 41;

		private ArtifactInfo artifactInfo_;

		public const int ArtifactItemDtosFieldNumber = 42;

		private static readonly FieldCodec<ArtifactItemDto> _repeated_artifactItemDtos_codec = FieldCodec.ForMessage<ArtifactItemDto>(338U, ArtifactItemDto.Parser);

		private readonly RepeatedField<ArtifactItemDto> artifactItemDtos_ = new RepeatedField<ArtifactItemDto>();

		public const int MountInfoFieldNumber = 43;

		private MountInfo mountInfo_;

		public const int MountItemDtosFieldNumber = 44;

		private static readonly FieldCodec<MountItemDto> _repeated_mountItemDtos_codec = FieldCodec.ForMessage<MountItemDto>(354U, MountItemDto.Parser);

		private readonly RepeatedField<MountItemDto> mountItemDtos_ = new RepeatedField<MountItemDto>();

		public const int UserStatisticInfoFieldNumber = 45;

		private UserStatisticInfo userStatisticInfo_;

		public const int IsNewUserFieldNumber = 46;

		private bool isNewUser_;

		public const int DataRefreshTimestampFieldNumber = 47;

		private ulong dataRefreshTimestamp_;

		public const int TimeZoneFieldNumber = 48;

		private string timeZone_ = "";

		public const int ShopAllDataDtoFieldNumber = 49;

		private ShopAllDataDto shopAllDataDto_;

		public const int HungUpInfoDtoFieldNumber = 50;

		private HungUpInfoDto hungUpInfoDto_;

		public const int PushIapDtoFieldNumber = 51;

		private PushIapDto pushIapDto_;

		public const int ServerIdFieldNumber = 54;

		private uint serverId_;

		public const int HellStageFieldNumber = 55;

		private uint hellStage_;

		public const int HellBattleStatusFieldNumber = 56;

		private uint hellBattleStatus_;

		public const int OpenServerTimeFieldNumber = 58;

		private ulong openServerTime_;

		public const int OpenServerResetTimeFieldNumber = 59;

		private ulong openServerResetTime_;

		public const int AccountKeyFieldNumber = 60;

		private string accountKey_ = "";

		public const int TgaInfoDtoFieldNumber = 61;

		private TGAInfoDto tgaInfoDto_;

		public const int IsForceJumpFieldNumber = 62;

		private bool isForceJump_;

		public const int AbVersionFieldNumber = 63;

		private uint abVersion_;

		public const int HabbyMailBindFieldNumber = 64;

		private bool habbyMailBind_;

		public const int HabbyMailRewardFieldNumber = 65;

		private bool habbyMailReward_;

		public const int ServerIMGroupIdFieldNumber = 66;

		private string serverIMGroupId_ = "";

		public const int EnterNewWorldFieldNumber = 67;

		private int enterNewWorld_;

		public const int HabbyIdFieldNumber = 68;

		private string habbyId_ = "";

		public const int CrossServerIMGroupIdFieldNumber = 69;

		private string crossServerIMGroupId_ = "";

		public const int FreeAdLifeRefreshtimeFieldNumber = 70;

		private long freeAdLifeRefreshtime_;

		public const int WheelInfoFieldNumber = 71;

		private ChapterActiveWheelInfo wheelInfo_;

		public const int PushChainDtoFieldNumber = 72;

		private static readonly FieldCodec<ChainActvDto> _repeated_pushChainDto_codec = FieldCodec.ForMessage<ChainActvDto>(578U, ChainActvDto.Parser);

		private readonly RepeatedField<ChainActvDto> pushChainDto_ = new RepeatedField<ChainActvDto>();

		public const int ShopSupCountFieldNumber = 73;

		private int shopSupCount_;
	}
}
