using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class CommonData : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CommonData> Parser
		{
			get
			{
				return CommonData._parser;
			}
		}

		[DebuggerNonUserCode]
		public UpdateTransId UpdateTransId
		{
			get
			{
				return this.updateTransId_;
			}
			set
			{
				this.updateTransId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public UpdateUserCurrency UpdateUserCurrency
		{
			get
			{
				return this.updateUserCurrency_;
			}
			set
			{
				this.updateUserCurrency_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> Reward
		{
			get
			{
				return this.reward_;
			}
		}

		[DebuggerNonUserCode]
		public UpdateUserLevel UpdateUserLevel
		{
			get
			{
				return this.updateUserLevel_;
			}
			set
			{
				this.updateUserLevel_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool TaskRedPoint
		{
			get
			{
				return this.taskRedPoint_;
			}
			set
			{
				this.taskRedPoint_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<EquipmentDto> Equipment
		{
			get
			{
				return this.equipment_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<ulong, ItemDto> Items
		{
			get
			{
				return this.items_;
			}
		}

		[DebuggerNonUserCode]
		public UpdateAdData UpdateAdData
		{
			get
			{
				return this.updateAdData_;
			}
			set
			{
				this.updateAdData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool GuildTaskRedPoint
		{
			get
			{
				return this.guildTaskRedPoint_;
			}
			set
			{
				this.guildTaskRedPoint_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ulong> DeleteEquipRowIds
		{
			get
			{
				return this.deleteEquipRowIds_;
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
		public RepeatedField<TalentSystemDto> TalentSystems
		{
			get
			{
				return this.talentSystems_;
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
		public UpdateUserVipLevel UpdateUserVipLevel
		{
			get
			{
				return this.updateUserVipLevel_;
			}
			set
			{
				this.updateUserVipLevel_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<TaskDto> UpdateTasks
		{
			get
			{
				return this.updateTasks_;
			}
		}

		[DebuggerNonUserCode]
		public bool SevenDayTaskRedPoint
		{
			get
			{
				return this.sevenDayTaskRedPoint_;
			}
			set
			{
				this.sevenDayTaskRedPoint_ = value;
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
		public RepeatedField<SevenDayTaskDto> UpdateTaskDto
		{
			get
			{
				return this.updateTaskDto_;
			}
		}

		[DebuggerNonUserCode]
		public uint BattlePassScore
		{
			get
			{
				return this.battlePassScore_;
			}
			set
			{
				this.battlePassScore_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<PetDto> PetDto
		{
			get
			{
				return this.petDto_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<CollectionDto> CollectionDto
		{
			get
			{
				return this.collectionDto_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<TalentsInfo> TalentsInfo
		{
			get
			{
				return this.talentsInfo_;
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
		public RepeatedField<Consume> ConsumeData
		{
			get
			{
				return this.consumeData_;
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
		public RepeatedField<TurnTableTaskDto> TurnTableTasks
		{
			get
			{
				return this.turnTableTasks_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<CostDto> CostDto
		{
			get
			{
				return this.costDto_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> CloseFunctionId
		{
			get
			{
				return this.closeFunctionId_;
			}
		}

		[DebuggerNonUserCode]
		public bool SetCloseFunction
		{
			get
			{
				return this.setCloseFunction_;
			}
			set
			{
				this.setCloseFunction_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ChainActvDto> ChainActvDto
		{
			get
			{
				return this.chainActvDto_;
			}
		}

		[DebuggerNonUserCode]
		public uint IsChainActv
		{
			get
			{
				return this.isChainActv_;
			}
			set
			{
				this.isChainActv_ = value;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.updateTransId_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.UpdateTransId);
			}
			if (this.updateUserCurrency_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.UpdateUserCurrency);
			}
			this.reward_.WriteTo(output, CommonData._repeated_reward_codec);
			if (this.updateUserLevel_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.UpdateUserLevel);
			}
			if (this.TaskRedPoint)
			{
				output.WriteRawTag(40);
				output.WriteBool(this.TaskRedPoint);
			}
			this.equipment_.WriteTo(output, CommonData._repeated_equipment_codec);
			this.items_.WriteTo(output, CommonData._map_items_codec);
			if (this.updateAdData_ != null)
			{
				output.WriteRawTag(66);
				output.WriteMessage(this.UpdateAdData);
			}
			if (this.GuildTaskRedPoint)
			{
				output.WriteRawTag(72);
				output.WriteBool(this.GuildTaskRedPoint);
			}
			this.deleteEquipRowIds_.WriteTo(output, CommonData._repeated_deleteEquipRowIds_codec);
			this.heros_.WriteTo(output, CommonData._repeated_heros_codec);
			if (this.actor_ != null)
			{
				output.WriteRawTag(98);
				output.WriteMessage(this.Actor);
			}
			this.talentSystems_.WriteTo(output, CommonData._repeated_talentSystems_codec);
			this.relics_.WriteTo(output, CommonData._repeated_relics_codec);
			if (this.updateUserVipLevel_ != null)
			{
				output.WriteRawTag(122);
				output.WriteMessage(this.UpdateUserVipLevel);
			}
			this.updateTasks_.WriteTo(output, CommonData._repeated_updateTasks_codec);
			if (this.SevenDayTaskRedPoint)
			{
				output.WriteRawTag(136, 1);
				output.WriteBool(this.SevenDayTaskRedPoint);
			}
			this.userTickets_.WriteTo(output, CommonData._repeated_userTickets_codec);
			this.updateTaskDto_.WriteTo(output, CommonData._repeated_updateTaskDto_codec);
			if (this.BattlePassScore != 0U)
			{
				output.WriteRawTag(160, 1);
				output.WriteUInt32(this.BattlePassScore);
			}
			this.petDto_.WriteTo(output, CommonData._repeated_petDto_codec);
			this.collectionDto_.WriteTo(output, CommonData._repeated_collectionDto_codec);
			this.talentsInfo_.WriteTo(output, CommonData._repeated_talentsInfo_codec);
			this.artifactItemDtos_.WriteTo(output, CommonData._repeated_artifactItemDtos_codec);
			this.mountItemDtos_.WriteTo(output, CommonData._repeated_mountItemDtos_codec);
			if (this.userStatisticInfo_ != null)
			{
				output.WriteRawTag(210, 1);
				output.WriteMessage(this.UserStatisticInfo);
			}
			if (this.adData_ != null)
			{
				output.WriteRawTag(218, 1);
				output.WriteMessage(this.AdData);
			}
			if (this.userInfoDto_ != null)
			{
				output.WriteRawTag(226, 1);
				output.WriteMessage(this.UserInfoDto);
			}
			this.consumeData_.WriteTo(output, CommonData._repeated_consumeData_codec);
			if (this.pushIapDto_ != null)
			{
				output.WriteRawTag(242, 1);
				output.WriteMessage(this.PushIapDto);
			}
			this.turnTableTasks_.WriteTo(output, CommonData._repeated_turnTableTasks_codec);
			this.costDto_.WriteTo(output, CommonData._repeated_costDto_codec);
			this.closeFunctionId_.WriteTo(output, CommonData._repeated_closeFunctionId_codec);
			if (this.SetCloseFunction)
			{
				output.WriteRawTag(144, 2);
				output.WriteBool(this.SetCloseFunction);
			}
			this.chainActvDto_.WriteTo(output, CommonData._repeated_chainActvDto_codec);
			if (this.IsChainActv != 0U)
			{
				output.WriteRawTag(160, 2);
				output.WriteUInt32(this.IsChainActv);
			}
			this.pushChainDto_.WriteTo(output, CommonData._repeated_pushChainDto_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.updateTransId_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UpdateTransId);
			}
			if (this.updateUserCurrency_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UpdateUserCurrency);
			}
			num += this.reward_.CalculateSize(CommonData._repeated_reward_codec);
			if (this.updateUserLevel_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UpdateUserLevel);
			}
			if (this.TaskRedPoint)
			{
				num += 2;
			}
			num += this.equipment_.CalculateSize(CommonData._repeated_equipment_codec);
			num += this.items_.CalculateSize(CommonData._map_items_codec);
			if (this.updateAdData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UpdateAdData);
			}
			if (this.GuildTaskRedPoint)
			{
				num += 2;
			}
			num += this.deleteEquipRowIds_.CalculateSize(CommonData._repeated_deleteEquipRowIds_codec);
			num += this.heros_.CalculateSize(CommonData._repeated_heros_codec);
			if (this.actor_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Actor);
			}
			num += this.talentSystems_.CalculateSize(CommonData._repeated_talentSystems_codec);
			num += this.relics_.CalculateSize(CommonData._repeated_relics_codec);
			if (this.updateUserVipLevel_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UpdateUserVipLevel);
			}
			num += this.updateTasks_.CalculateSize(CommonData._repeated_updateTasks_codec);
			if (this.SevenDayTaskRedPoint)
			{
				num += 3;
			}
			num += this.userTickets_.CalculateSize(CommonData._repeated_userTickets_codec);
			num += this.updateTaskDto_.CalculateSize(CommonData._repeated_updateTaskDto_codec);
			if (this.BattlePassScore != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.BattlePassScore);
			}
			num += this.petDto_.CalculateSize(CommonData._repeated_petDto_codec);
			num += this.collectionDto_.CalculateSize(CommonData._repeated_collectionDto_codec);
			num += this.talentsInfo_.CalculateSize(CommonData._repeated_talentsInfo_codec);
			num += this.artifactItemDtos_.CalculateSize(CommonData._repeated_artifactItemDtos_codec);
			num += this.mountItemDtos_.CalculateSize(CommonData._repeated_mountItemDtos_codec);
			if (this.userStatisticInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.UserStatisticInfo);
			}
			if (this.adData_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.AdData);
			}
			if (this.userInfoDto_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.UserInfoDto);
			}
			num += this.consumeData_.CalculateSize(CommonData._repeated_consumeData_codec);
			if (this.pushIapDto_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.PushIapDto);
			}
			num += this.turnTableTasks_.CalculateSize(CommonData._repeated_turnTableTasks_codec);
			num += this.costDto_.CalculateSize(CommonData._repeated_costDto_codec);
			num += this.closeFunctionId_.CalculateSize(CommonData._repeated_closeFunctionId_codec);
			if (this.SetCloseFunction)
			{
				num += 3;
			}
			num += this.chainActvDto_.CalculateSize(CommonData._repeated_chainActvDto_codec);
			if (this.IsChainActv != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.IsChainActv);
			}
			return num + this.pushChainDto_.CalculateSize(CommonData._repeated_pushChainDto_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 146U)
				{
					if (num <= 72U)
					{
						if (num <= 34U)
						{
							if (num <= 18U)
							{
								if (num == 10U)
								{
									if (this.updateTransId_ == null)
									{
										this.updateTransId_ = new UpdateTransId();
									}
									input.ReadMessage(this.updateTransId_);
									continue;
								}
								if (num == 18U)
								{
									if (this.updateUserCurrency_ == null)
									{
										this.updateUserCurrency_ = new UpdateUserCurrency();
									}
									input.ReadMessage(this.updateUserCurrency_);
									continue;
								}
							}
							else
							{
								if (num == 26U)
								{
									this.reward_.AddEntriesFrom(input, CommonData._repeated_reward_codec);
									continue;
								}
								if (num == 34U)
								{
									if (this.updateUserLevel_ == null)
									{
										this.updateUserLevel_ = new UpdateUserLevel();
									}
									input.ReadMessage(this.updateUserLevel_);
									continue;
								}
							}
						}
						else if (num <= 50U)
						{
							if (num == 40U)
							{
								this.TaskRedPoint = input.ReadBool();
								continue;
							}
							if (num == 50U)
							{
								this.equipment_.AddEntriesFrom(input, CommonData._repeated_equipment_codec);
								continue;
							}
						}
						else
						{
							if (num == 58U)
							{
								this.items_.AddEntriesFrom(input, CommonData._map_items_codec);
								continue;
							}
							if (num == 66U)
							{
								if (this.updateAdData_ == null)
								{
									this.updateAdData_ = new UpdateAdData();
								}
								input.ReadMessage(this.updateAdData_);
								continue;
							}
							if (num == 72U)
							{
								this.GuildTaskRedPoint = input.ReadBool();
								continue;
							}
						}
					}
					else if (num <= 106U)
					{
						if (num <= 82U)
						{
							if (num == 80U || num == 82U)
							{
								this.deleteEquipRowIds_.AddEntriesFrom(input, CommonData._repeated_deleteEquipRowIds_codec);
								continue;
							}
						}
						else
						{
							if (num == 90U)
							{
								this.heros_.AddEntriesFrom(input, CommonData._repeated_heros_codec);
								continue;
							}
							if (num == 98U)
							{
								if (this.actor_ == null)
								{
									this.actor_ = new ActorDto();
								}
								input.ReadMessage(this.actor_);
								continue;
							}
							if (num == 106U)
							{
								this.talentSystems_.AddEntriesFrom(input, CommonData._repeated_talentSystems_codec);
								continue;
							}
						}
					}
					else if (num <= 122U)
					{
						if (num == 114U)
						{
							this.relics_.AddEntriesFrom(input, CommonData._repeated_relics_codec);
							continue;
						}
						if (num == 122U)
						{
							if (this.updateUserVipLevel_ == null)
							{
								this.updateUserVipLevel_ = new UpdateUserVipLevel();
							}
							input.ReadMessage(this.updateUserVipLevel_);
							continue;
						}
					}
					else
					{
						if (num == 130U)
						{
							this.updateTasks_.AddEntriesFrom(input, CommonData._repeated_updateTasks_codec);
							continue;
						}
						if (num == 136U)
						{
							this.SevenDayTaskRedPoint = input.ReadBool();
							continue;
						}
						if (num == 146U)
						{
							this.userTickets_.AddEntriesFrom(input, CommonData._repeated_userTickets_codec);
							continue;
						}
					}
				}
				else
				{
					if (num > 226U)
					{
						if (num <= 264U)
						{
							if (num <= 242U)
							{
								if (num == 234U)
								{
									this.consumeData_.AddEntriesFrom(input, CommonData._repeated_consumeData_codec);
									continue;
								}
								if (num != 242U)
								{
									goto IL_022D;
								}
								if (this.pushIapDto_ == null)
								{
									this.pushIapDto_ = new PushIapDto();
								}
								input.ReadMessage(this.pushIapDto_);
								continue;
							}
							else
							{
								if (num == 250U)
								{
									this.turnTableTasks_.AddEntriesFrom(input, CommonData._repeated_turnTableTasks_codec);
									continue;
								}
								if (num == 258U)
								{
									this.costDto_.AddEntriesFrom(input, CommonData._repeated_costDto_codec);
									continue;
								}
								if (num != 264U)
								{
									goto IL_022D;
								}
							}
						}
						else if (num <= 272U)
						{
							if (num != 266U)
							{
								if (num != 272U)
								{
									goto IL_022D;
								}
								this.SetCloseFunction = input.ReadBool();
								continue;
							}
						}
						else
						{
							if (num == 282U)
							{
								this.chainActvDto_.AddEntriesFrom(input, CommonData._repeated_chainActvDto_codec);
								continue;
							}
							if (num == 288U)
							{
								this.IsChainActv = input.ReadUInt32();
								continue;
							}
							if (num != 298U)
							{
								goto IL_022D;
							}
							this.pushChainDto_.AddEntriesFrom(input, CommonData._repeated_pushChainDto_codec);
							continue;
						}
						this.closeFunctionId_.AddEntriesFrom(input, CommonData._repeated_closeFunctionId_codec);
						continue;
					}
					if (num <= 186U)
					{
						if (num <= 160U)
						{
							if (num == 154U)
							{
								this.updateTaskDto_.AddEntriesFrom(input, CommonData._repeated_updateTaskDto_codec);
								continue;
							}
							if (num == 160U)
							{
								this.BattlePassScore = input.ReadUInt32();
								continue;
							}
						}
						else
						{
							if (num == 170U)
							{
								this.petDto_.AddEntriesFrom(input, CommonData._repeated_petDto_codec);
								continue;
							}
							if (num == 178U)
							{
								this.collectionDto_.AddEntriesFrom(input, CommonData._repeated_collectionDto_codec);
								continue;
							}
							if (num == 186U)
							{
								this.talentsInfo_.AddEntriesFrom(input, CommonData._repeated_talentsInfo_codec);
								continue;
							}
						}
					}
					else if (num <= 202U)
					{
						if (num == 194U)
						{
							this.artifactItemDtos_.AddEntriesFrom(input, CommonData._repeated_artifactItemDtos_codec);
							continue;
						}
						if (num == 202U)
						{
							this.mountItemDtos_.AddEntriesFrom(input, CommonData._repeated_mountItemDtos_codec);
							continue;
						}
					}
					else
					{
						if (num == 210U)
						{
							if (this.userStatisticInfo_ == null)
							{
								this.userStatisticInfo_ = new UserStatisticInfo();
							}
							input.ReadMessage(this.userStatisticInfo_);
							continue;
						}
						if (num == 218U)
						{
							if (this.adData_ == null)
							{
								this.adData_ = new AdDataDto();
							}
							input.ReadMessage(this.adData_);
							continue;
						}
						if (num == 226U)
						{
							if (this.userInfoDto_ == null)
							{
								this.userInfoDto_ = new UserInfoDto();
							}
							input.ReadMessage(this.userInfoDto_);
							continue;
						}
					}
				}
				IL_022D:
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<CommonData> _parser = new MessageParser<CommonData>(() => new CommonData());

		public const int UpdateTransIdFieldNumber = 1;

		private UpdateTransId updateTransId_;

		public const int UpdateUserCurrencyFieldNumber = 2;

		private UpdateUserCurrency updateUserCurrency_;

		public const int RewardFieldNumber = 3;

		private static readonly FieldCodec<RewardDto> _repeated_reward_codec = FieldCodec.ForMessage<RewardDto>(26U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> reward_ = new RepeatedField<RewardDto>();

		public const int UpdateUserLevelFieldNumber = 4;

		private UpdateUserLevel updateUserLevel_;

		public const int TaskRedPointFieldNumber = 5;

		private bool taskRedPoint_;

		public const int EquipmentFieldNumber = 6;

		private static readonly FieldCodec<EquipmentDto> _repeated_equipment_codec = FieldCodec.ForMessage<EquipmentDto>(50U, EquipmentDto.Parser);

		private readonly RepeatedField<EquipmentDto> equipment_ = new RepeatedField<EquipmentDto>();

		public const int ItemsFieldNumber = 7;

		private static readonly MapField<ulong, ItemDto>.Codec _map_items_codec = new MapField<ulong, ItemDto>.Codec(FieldCodec.ForUInt64(8U), FieldCodec.ForMessage<ItemDto>(18U, ItemDto.Parser), 58U);

		private readonly MapField<ulong, ItemDto> items_ = new MapField<ulong, ItemDto>();

		public const int UpdateAdDataFieldNumber = 8;

		private UpdateAdData updateAdData_;

		public const int GuildTaskRedPointFieldNumber = 9;

		private bool guildTaskRedPoint_;

		public const int DeleteEquipRowIdsFieldNumber = 10;

		private static readonly FieldCodec<ulong> _repeated_deleteEquipRowIds_codec = FieldCodec.ForUInt64(82U);

		private readonly RepeatedField<ulong> deleteEquipRowIds_ = new RepeatedField<ulong>();

		public const int HerosFieldNumber = 11;

		private static readonly FieldCodec<HeroDto> _repeated_heros_codec = FieldCodec.ForMessage<HeroDto>(90U, HeroDto.Parser);

		private readonly RepeatedField<HeroDto> heros_ = new RepeatedField<HeroDto>();

		public const int ActorFieldNumber = 12;

		private ActorDto actor_;

		public const int TalentSystemsFieldNumber = 13;

		private static readonly FieldCodec<TalentSystemDto> _repeated_talentSystems_codec = FieldCodec.ForMessage<TalentSystemDto>(106U, TalentSystemDto.Parser);

		private readonly RepeatedField<TalentSystemDto> talentSystems_ = new RepeatedField<TalentSystemDto>();

		public const int RelicsFieldNumber = 14;

		private static readonly FieldCodec<RelicDto> _repeated_relics_codec = FieldCodec.ForMessage<RelicDto>(114U, RelicDto.Parser);

		private readonly RepeatedField<RelicDto> relics_ = new RepeatedField<RelicDto>();

		public const int UpdateUserVipLevelFieldNumber = 15;

		private UpdateUserVipLevel updateUserVipLevel_;

		public const int UpdateTasksFieldNumber = 16;

		private static readonly FieldCodec<TaskDto> _repeated_updateTasks_codec = FieldCodec.ForMessage<TaskDto>(130U, TaskDto.Parser);

		private readonly RepeatedField<TaskDto> updateTasks_ = new RepeatedField<TaskDto>();

		public const int SevenDayTaskRedPointFieldNumber = 17;

		private bool sevenDayTaskRedPoint_;

		public const int UserTicketsFieldNumber = 18;

		private static readonly FieldCodec<UserTicket> _repeated_userTickets_codec = FieldCodec.ForMessage<UserTicket>(146U, UserTicket.Parser);

		private readonly RepeatedField<UserTicket> userTickets_ = new RepeatedField<UserTicket>();

		public const int UpdateTaskDtoFieldNumber = 19;

		private static readonly FieldCodec<SevenDayTaskDto> _repeated_updateTaskDto_codec = FieldCodec.ForMessage<SevenDayTaskDto>(154U, SevenDayTaskDto.Parser);

		private readonly RepeatedField<SevenDayTaskDto> updateTaskDto_ = new RepeatedField<SevenDayTaskDto>();

		public const int BattlePassScoreFieldNumber = 20;

		private uint battlePassScore_;

		public const int PetDtoFieldNumber = 21;

		private static readonly FieldCodec<PetDto> _repeated_petDto_codec = FieldCodec.ForMessage<PetDto>(170U, Proto.Common.PetDto.Parser);

		private readonly RepeatedField<PetDto> petDto_ = new RepeatedField<PetDto>();

		public const int CollectionDtoFieldNumber = 22;

		private static readonly FieldCodec<CollectionDto> _repeated_collectionDto_codec = FieldCodec.ForMessage<CollectionDto>(178U, Proto.Common.CollectionDto.Parser);

		private readonly RepeatedField<CollectionDto> collectionDto_ = new RepeatedField<CollectionDto>();

		public const int TalentsInfoFieldNumber = 23;

		private static readonly FieldCodec<TalentsInfo> _repeated_talentsInfo_codec = FieldCodec.ForMessage<TalentsInfo>(186U, Proto.Common.TalentsInfo.Parser);

		private readonly RepeatedField<TalentsInfo> talentsInfo_ = new RepeatedField<TalentsInfo>();

		public const int ArtifactItemDtosFieldNumber = 24;

		private static readonly FieldCodec<ArtifactItemDto> _repeated_artifactItemDtos_codec = FieldCodec.ForMessage<ArtifactItemDto>(194U, ArtifactItemDto.Parser);

		private readonly RepeatedField<ArtifactItemDto> artifactItemDtos_ = new RepeatedField<ArtifactItemDto>();

		public const int MountItemDtosFieldNumber = 25;

		private static readonly FieldCodec<MountItemDto> _repeated_mountItemDtos_codec = FieldCodec.ForMessage<MountItemDto>(202U, MountItemDto.Parser);

		private readonly RepeatedField<MountItemDto> mountItemDtos_ = new RepeatedField<MountItemDto>();

		public const int UserStatisticInfoFieldNumber = 26;

		private UserStatisticInfo userStatisticInfo_;

		public const int AdDataFieldNumber = 27;

		private AdDataDto adData_;

		public const int UserInfoDtoFieldNumber = 28;

		private UserInfoDto userInfoDto_;

		public const int ConsumeDataFieldNumber = 29;

		private static readonly FieldCodec<Consume> _repeated_consumeData_codec = FieldCodec.ForMessage<Consume>(234U, Consume.Parser);

		private readonly RepeatedField<Consume> consumeData_ = new RepeatedField<Consume>();

		public const int PushIapDtoFieldNumber = 30;

		private PushIapDto pushIapDto_;

		public const int TurnTableTasksFieldNumber = 31;

		private static readonly FieldCodec<TurnTableTaskDto> _repeated_turnTableTasks_codec = FieldCodec.ForMessage<TurnTableTaskDto>(250U, TurnTableTaskDto.Parser);

		private readonly RepeatedField<TurnTableTaskDto> turnTableTasks_ = new RepeatedField<TurnTableTaskDto>();

		public const int CostDtoFieldNumber = 32;

		private static readonly FieldCodec<CostDto> _repeated_costDto_codec = FieldCodec.ForMessage<CostDto>(258U, Proto.Common.CostDto.Parser);

		private readonly RepeatedField<CostDto> costDto_ = new RepeatedField<CostDto>();

		public const int CloseFunctionIdFieldNumber = 33;

		private static readonly FieldCodec<int> _repeated_closeFunctionId_codec = FieldCodec.ForInt32(266U);

		private readonly RepeatedField<int> closeFunctionId_ = new RepeatedField<int>();

		public const int SetCloseFunctionFieldNumber = 34;

		private bool setCloseFunction_;

		public const int ChainActvDtoFieldNumber = 35;

		private static readonly FieldCodec<ChainActvDto> _repeated_chainActvDto_codec = FieldCodec.ForMessage<ChainActvDto>(282U, Proto.Common.ChainActvDto.Parser);

		private readonly RepeatedField<ChainActvDto> chainActvDto_ = new RepeatedField<ChainActvDto>();

		public const int IsChainActvFieldNumber = 36;

		private uint isChainActv_;

		public const int PushChainDtoFieldNumber = 37;

		private static readonly FieldCodec<ChainActvDto> _repeated_pushChainDto_codec = FieldCodec.ForMessage<ChainActvDto>(298U, Proto.Common.ChainActvDto.Parser);

		private readonly RepeatedField<ChainActvDto> pushChainDto_ = new RepeatedField<ChainActvDto>();
	}
}
