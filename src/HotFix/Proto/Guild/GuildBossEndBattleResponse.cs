using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildBossEndBattleResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildBossEndBattleResponse> Parser
		{
			get
			{
				return GuildBossEndBattleResponse._parser;
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
		public uint ChallengeCnt
		{
			get
			{
				return this.challengeCnt_;
			}
			set
			{
				this.challengeCnt_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong Damage
		{
			get
			{
				return this.damage_;
			}
			set
			{
				this.damage_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong TotalDamage
		{
			get
			{
				return this.totalDamage_;
			}
			set
			{
				this.totalDamage_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Result
		{
			get
			{
				return this.result_;
			}
			set
			{
				this.result_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Seed
		{
			get
			{
				return this.seed_;
			}
			set
			{
				this.seed_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<CombatUnitDto> StartUnits
		{
			get
			{
				return this.startUnits_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<CombatUnitDto> EndUnits
		{
			get
			{
				return this.endUnits_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, ulong> ChapterGiftTime
		{
			get
			{
				return this.chapterGiftTime_;
			}
		}

		[DebuggerNonUserCode]
		public BattleUserDto UserInfo
		{
			get
			{
				return this.userInfo_;
			}
			set
			{
				this.userInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string BattleServerLogId
		{
			get
			{
				return this.battleServerLogId_;
			}
			set
			{
				this.battleServerLogId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string BattleServerLogData
		{
			get
			{
				return this.battleServerLogData_;
			}
			set
			{
				this.battleServerLogData_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> BoxReward
		{
			get
			{
				return this.boxReward_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> BattleReward
		{
			get
			{
				return this.battleReward_;
			}
		}

		[DebuggerNonUserCode]
		public GuildBossInfoDto GuildBossInfo
		{
			get
			{
				return this.guildBossInfo_;
			}
			set
			{
				this.guildBossInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long BeforeHp
		{
			get
			{
				return this.beforeHp_;
			}
			set
			{
				this.beforeHp_ = value;
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
			if (this.ChallengeCnt != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.ChallengeCnt);
			}
			if (this.Damage != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.Damage);
			}
			if (this.TotalDamage != 0UL)
			{
				output.WriteRawTag(40);
				output.WriteUInt64(this.TotalDamage);
			}
			if (this.Result != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.Result);
			}
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.ConfigId);
			}
			if (this.Seed != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.Seed);
			}
			this.startUnits_.WriteTo(output, GuildBossEndBattleResponse._repeated_startUnits_codec);
			this.endUnits_.WriteTo(output, GuildBossEndBattleResponse._repeated_endUnits_codec);
			this.chapterGiftTime_.WriteTo(output, GuildBossEndBattleResponse._map_chapterGiftTime_codec);
			if (this.userInfo_ != null)
			{
				output.WriteRawTag(98);
				output.WriteMessage(this.UserInfo);
			}
			if (this.BattleServerLogId.Length != 0)
			{
				output.WriteRawTag(106);
				output.WriteString(this.BattleServerLogId);
			}
			if (this.BattleServerLogData.Length != 0)
			{
				output.WriteRawTag(114);
				output.WriteString(this.BattleServerLogData);
			}
			this.boxReward_.WriteTo(output, GuildBossEndBattleResponse._repeated_boxReward_codec);
			this.battleReward_.WriteTo(output, GuildBossEndBattleResponse._repeated_battleReward_codec);
			if (this.guildBossInfo_ != null)
			{
				output.WriteRawTag(138, 1);
				output.WriteMessage(this.GuildBossInfo);
			}
			if (this.BeforeHp != 0L)
			{
				output.WriteRawTag(144, 1);
				output.WriteInt64(this.BeforeHp);
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
			if (this.ChallengeCnt != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ChallengeCnt);
			}
			if (this.Damage != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Damage);
			}
			if (this.TotalDamage != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.TotalDamage);
			}
			if (this.Result != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Result);
			}
			if (this.ConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ConfigId);
			}
			if (this.Seed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seed);
			}
			num += this.startUnits_.CalculateSize(GuildBossEndBattleResponse._repeated_startUnits_codec);
			num += this.endUnits_.CalculateSize(GuildBossEndBattleResponse._repeated_endUnits_codec);
			num += this.chapterGiftTime_.CalculateSize(GuildBossEndBattleResponse._map_chapterGiftTime_codec);
			if (this.userInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserInfo);
			}
			if (this.BattleServerLogId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.BattleServerLogId);
			}
			if (this.BattleServerLogData.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.BattleServerLogData);
			}
			num += this.boxReward_.CalculateSize(GuildBossEndBattleResponse._repeated_boxReward_codec);
			num += this.battleReward_.CalculateSize(GuildBossEndBattleResponse._repeated_battleReward_codec);
			if (this.guildBossInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.GuildBossInfo);
			}
			if (this.BeforeHp != 0L)
			{
				num += 2 + CodedOutputStream.ComputeInt64Size(this.BeforeHp);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 74U)
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
							if (num == 24U)
							{
								this.ChallengeCnt = input.ReadUInt32();
								continue;
							}
							if (num == 32U)
							{
								this.Damage = input.ReadUInt64();
								continue;
							}
						}
					}
					else if (num <= 48U)
					{
						if (num == 40U)
						{
							this.TotalDamage = input.ReadUInt64();
							continue;
						}
						if (num == 48U)
						{
							this.Result = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 56U)
						{
							this.ConfigId = input.ReadInt32();
							continue;
						}
						if (num == 64U)
						{
							this.Seed = input.ReadInt32();
							continue;
						}
						if (num == 74U)
						{
							this.startUnits_.AddEntriesFrom(input, GuildBossEndBattleResponse._repeated_startUnits_codec);
							continue;
						}
					}
				}
				else if (num <= 106U)
				{
					if (num <= 90U)
					{
						if (num == 82U)
						{
							this.endUnits_.AddEntriesFrom(input, GuildBossEndBattleResponse._repeated_endUnits_codec);
							continue;
						}
						if (num == 90U)
						{
							this.chapterGiftTime_.AddEntriesFrom(input, GuildBossEndBattleResponse._map_chapterGiftTime_codec);
							continue;
						}
					}
					else
					{
						if (num == 98U)
						{
							if (this.userInfo_ == null)
							{
								this.userInfo_ = new BattleUserDto();
							}
							input.ReadMessage(this.userInfo_);
							continue;
						}
						if (num == 106U)
						{
							this.BattleServerLogId = input.ReadString();
							continue;
						}
					}
				}
				else if (num <= 122U)
				{
					if (num == 114U)
					{
						this.BattleServerLogData = input.ReadString();
						continue;
					}
					if (num == 122U)
					{
						this.boxReward_.AddEntriesFrom(input, GuildBossEndBattleResponse._repeated_boxReward_codec);
						continue;
					}
				}
				else
				{
					if (num == 130U)
					{
						this.battleReward_.AddEntriesFrom(input, GuildBossEndBattleResponse._repeated_battleReward_codec);
						continue;
					}
					if (num == 138U)
					{
						if (this.guildBossInfo_ == null)
						{
							this.guildBossInfo_ = new GuildBossInfoDto();
						}
						input.ReadMessage(this.guildBossInfo_);
						continue;
					}
					if (num == 144U)
					{
						this.BeforeHp = input.ReadInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildBossEndBattleResponse> _parser = new MessageParser<GuildBossEndBattleResponse>(() => new GuildBossEndBattleResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ChallengeCntFieldNumber = 3;

		private uint challengeCnt_;

		public const int DamageFieldNumber = 4;

		private ulong damage_;

		public const int TotalDamageFieldNumber = 5;

		private ulong totalDamage_;

		public const int ResultFieldNumber = 6;

		private uint result_;

		public const int ConfigIdFieldNumber = 7;

		private int configId_;

		public const int SeedFieldNumber = 8;

		private int seed_;

		public const int StartUnitsFieldNumber = 9;

		private static readonly FieldCodec<CombatUnitDto> _repeated_startUnits_codec = FieldCodec.ForMessage<CombatUnitDto>(74U, CombatUnitDto.Parser);

		private readonly RepeatedField<CombatUnitDto> startUnits_ = new RepeatedField<CombatUnitDto>();

		public const int EndUnitsFieldNumber = 10;

		private static readonly FieldCodec<CombatUnitDto> _repeated_endUnits_codec = FieldCodec.ForMessage<CombatUnitDto>(82U, CombatUnitDto.Parser);

		private readonly RepeatedField<CombatUnitDto> endUnits_ = new RepeatedField<CombatUnitDto>();

		public const int ChapterGiftTimeFieldNumber = 11;

		private static readonly MapField<uint, ulong>.Codec _map_chapterGiftTime_codec = new MapField<uint, ulong>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt64(16U), 90U);

		private readonly MapField<uint, ulong> chapterGiftTime_ = new MapField<uint, ulong>();

		public const int UserInfoFieldNumber = 12;

		private BattleUserDto userInfo_;

		public const int BattleServerLogIdFieldNumber = 13;

		private string battleServerLogId_ = "";

		public const int BattleServerLogDataFieldNumber = 14;

		private string battleServerLogData_ = "";

		public const int BoxRewardFieldNumber = 15;

		private static readonly FieldCodec<RewardDto> _repeated_boxReward_codec = FieldCodec.ForMessage<RewardDto>(122U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> boxReward_ = new RepeatedField<RewardDto>();

		public const int BattleRewardFieldNumber = 16;

		private static readonly FieldCodec<RewardDto> _repeated_battleReward_codec = FieldCodec.ForMessage<RewardDto>(130U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> battleReward_ = new RepeatedField<RewardDto>();

		public const int GuildBossInfoFieldNumber = 17;

		private GuildBossInfoDto guildBossInfo_;

		public const int BeforeHpFieldNumber = 18;

		private long beforeHp_;
	}
}
