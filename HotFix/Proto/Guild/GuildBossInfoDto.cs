using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Guild
{
	public sealed class GuildBossInfoDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildBossInfoDto> Parser
		{
			get
			{
				return GuildBossInfoDto._parser;
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
		public uint BuyCntByDiamonds
		{
			get
			{
				return this.buyCntByDiamonds_;
			}
			set
			{
				this.buyCntByDiamonds_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint MaxBuyCntByDiamonds
		{
			get
			{
				return this.maxBuyCntByDiamonds_;
			}
			set
			{
				this.maxBuyCntByDiamonds_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint BuyCntCostByDiamonds
		{
			get
			{
				return this.buyCntCostByDiamonds_;
			}
			set
			{
				this.buyCntCostByDiamonds_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint BuyCntByCoins
		{
			get
			{
				return this.buyCntByCoins_;
			}
			set
			{
				this.buyCntByCoins_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint MaxBuyCntByCoins
		{
			get
			{
				return this.maxBuyCntByCoins_;
			}
			set
			{
				this.maxBuyCntByCoins_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint BuyCntCostByCoins
		{
			get
			{
				return this.buyCntCostByCoins_;
			}
			set
			{
				this.buyCntCostByCoins_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ChallengeCntRecoverySeconds
		{
			get
			{
				return this.challengeCntRecoverySeconds_;
			}
			set
			{
				this.challengeCntRecoverySeconds_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ChallengeCntRecoveryPerTime
		{
			get
			{
				return this.challengeCntRecoveryPerTime_;
			}
			set
			{
				this.challengeCntRecoveryPerTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong NextChallengeCntRecoveryTime
		{
			get
			{
				return this.nextChallengeCntRecoveryTime_;
			}
			set
			{
				this.nextChallengeCntRecoveryTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint MaxRecoveryCnt
		{
			get
			{
				return this.maxRecoveryCnt_;
			}
			set
			{
				this.maxRecoveryCnt_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong TotalPersonalDamage
		{
			get
			{
				return this.totalPersonalDamage_;
			}
			set
			{
				this.totalPersonalDamage_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong TotalGuildDamage
		{
			get
			{
				return this.totalGuildDamage_;
			}
			set
			{
				this.totalGuildDamage_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint KillBossCnt
		{
			get
			{
				return this.killBossCnt_;
			}
			set
			{
				this.killBossCnt_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong BossRefreshTimestamp
		{
			get
			{
				return this.bossRefreshTimestamp_;
			}
			set
			{
				this.bossRefreshTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong DailyRefreshTimestamp
		{
			get
			{
				return this.dailyRefreshTimestamp_;
			}
			set
			{
				this.dailyRefreshTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public GuildBossConfigDto BossConfig
		{
			get
			{
				return this.bossConfig_;
			}
			set
			{
				this.bossConfig_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<GuildBossTaskDto> BossTask
		{
			get
			{
				return this.bossTask_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<GuildBossKillBoxDto> KillBox
		{
			get
			{
				return this.killBox_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<string> ChallengeRecords
		{
			get
			{
				return this.challengeRecords_;
			}
		}

		[DebuggerNonUserCode]
		public uint DayPartCount
		{
			get
			{
				return this.dayPartCount_;
			}
			set
			{
				this.dayPartCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint GuildDan
		{
			get
			{
				return this.guildDan_;
			}
			set
			{
				this.guildDan_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint GuildSeason
		{
			get
			{
				return this.guildSeason_;
			}
			set
			{
				this.guildSeason_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int GuildRank
		{
			get
			{
				return this.guildRank_;
			}
			set
			{
				this.guildRank_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int PersonRank
		{
			get
			{
				return this.personRank_;
			}
			set
			{
				this.personRank_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int LastPersonRank
		{
			get
			{
				return this.lastPersonRank_;
			}
			set
			{
				this.lastPersonRank_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> KilledBossList
		{
			get
			{
				return this.killedBossList_;
			}
		}

		[DebuggerNonUserCode]
		public ulong SeasonEndTime
		{
			get
			{
				return this.seasonEndTime_;
			}
			set
			{
				this.seasonEndTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int LastGuildRank
		{
			get
			{
				return this.lastGuildRank_;
			}
			set
			{
				this.lastGuildRank_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ChallengeCnt != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.ChallengeCnt);
			}
			if (this.BuyCntByDiamonds != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.BuyCntByDiamonds);
			}
			if (this.MaxBuyCntByDiamonds != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.MaxBuyCntByDiamonds);
			}
			if (this.BuyCntCostByDiamonds != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.BuyCntCostByDiamonds);
			}
			if (this.BuyCntByCoins != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.BuyCntByCoins);
			}
			if (this.MaxBuyCntByCoins != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.MaxBuyCntByCoins);
			}
			if (this.BuyCntCostByCoins != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.BuyCntCostByCoins);
			}
			if (this.ChallengeCntRecoverySeconds != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.ChallengeCntRecoverySeconds);
			}
			if (this.ChallengeCntRecoveryPerTime != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.ChallengeCntRecoveryPerTime);
			}
			if (this.NextChallengeCntRecoveryTime != 0UL)
			{
				output.WriteRawTag(80);
				output.WriteUInt64(this.NextChallengeCntRecoveryTime);
			}
			if (this.MaxRecoveryCnt != 0U)
			{
				output.WriteRawTag(88);
				output.WriteUInt32(this.MaxRecoveryCnt);
			}
			if (this.TotalPersonalDamage != 0UL)
			{
				output.WriteRawTag(96);
				output.WriteUInt64(this.TotalPersonalDamage);
			}
			if (this.TotalGuildDamage != 0UL)
			{
				output.WriteRawTag(104);
				output.WriteUInt64(this.TotalGuildDamage);
			}
			if (this.KillBossCnt != 0U)
			{
				output.WriteRawTag(112);
				output.WriteUInt32(this.KillBossCnt);
			}
			if (this.BossRefreshTimestamp != 0UL)
			{
				output.WriteRawTag(120);
				output.WriteUInt64(this.BossRefreshTimestamp);
			}
			if (this.DailyRefreshTimestamp != 0UL)
			{
				output.WriteRawTag(128, 1);
				output.WriteUInt64(this.DailyRefreshTimestamp);
			}
			if (this.bossConfig_ != null)
			{
				output.WriteRawTag(138, 1);
				output.WriteMessage(this.BossConfig);
			}
			this.bossTask_.WriteTo(output, GuildBossInfoDto._repeated_bossTask_codec);
			this.killBox_.WriteTo(output, GuildBossInfoDto._repeated_killBox_codec);
			this.challengeRecords_.WriteTo(output, GuildBossInfoDto._repeated_challengeRecords_codec);
			if (this.DayPartCount != 0U)
			{
				output.WriteRawTag(168, 1);
				output.WriteUInt32(this.DayPartCount);
			}
			if (this.GuildDan != 0U)
			{
				output.WriteRawTag(176, 1);
				output.WriteUInt32(this.GuildDan);
			}
			if (this.GuildSeason != 0U)
			{
				output.WriteRawTag(184, 1);
				output.WriteUInt32(this.GuildSeason);
			}
			if (this.GuildRank != 0)
			{
				output.WriteRawTag(192, 1);
				output.WriteInt32(this.GuildRank);
			}
			if (this.PersonRank != 0)
			{
				output.WriteRawTag(200, 1);
				output.WriteInt32(this.PersonRank);
			}
			if (this.LastPersonRank != 0)
			{
				output.WriteRawTag(208, 1);
				output.WriteInt32(this.LastPersonRank);
			}
			this.killedBossList_.WriteTo(output, GuildBossInfoDto._repeated_killedBossList_codec);
			if (this.SeasonEndTime != 0UL)
			{
				output.WriteRawTag(224, 1);
				output.WriteUInt64(this.SeasonEndTime);
			}
			if (this.LastGuildRank != 0)
			{
				output.WriteRawTag(232, 1);
				output.WriteInt32(this.LastGuildRank);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ChallengeCnt != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ChallengeCnt);
			}
			if (this.BuyCntByDiamonds != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BuyCntByDiamonds);
			}
			if (this.MaxBuyCntByDiamonds != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.MaxBuyCntByDiamonds);
			}
			if (this.BuyCntCostByDiamonds != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BuyCntCostByDiamonds);
			}
			if (this.BuyCntByCoins != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BuyCntByCoins);
			}
			if (this.MaxBuyCntByCoins != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.MaxBuyCntByCoins);
			}
			if (this.BuyCntCostByCoins != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BuyCntCostByCoins);
			}
			if (this.ChallengeCntRecoverySeconds != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ChallengeCntRecoverySeconds);
			}
			if (this.ChallengeCntRecoveryPerTime != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ChallengeCntRecoveryPerTime);
			}
			if (this.NextChallengeCntRecoveryTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.NextChallengeCntRecoveryTime);
			}
			if (this.MaxRecoveryCnt != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.MaxRecoveryCnt);
			}
			if (this.TotalPersonalDamage != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.TotalPersonalDamage);
			}
			if (this.TotalGuildDamage != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.TotalGuildDamage);
			}
			if (this.KillBossCnt != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.KillBossCnt);
			}
			if (this.BossRefreshTimestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.BossRefreshTimestamp);
			}
			if (this.DailyRefreshTimestamp != 0UL)
			{
				num += 2 + CodedOutputStream.ComputeUInt64Size(this.DailyRefreshTimestamp);
			}
			if (this.bossConfig_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.BossConfig);
			}
			num += this.bossTask_.CalculateSize(GuildBossInfoDto._repeated_bossTask_codec);
			num += this.killBox_.CalculateSize(GuildBossInfoDto._repeated_killBox_codec);
			num += this.challengeRecords_.CalculateSize(GuildBossInfoDto._repeated_challengeRecords_codec);
			if (this.DayPartCount != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.DayPartCount);
			}
			if (this.GuildDan != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.GuildDan);
			}
			if (this.GuildSeason != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.GuildSeason);
			}
			if (this.GuildRank != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.GuildRank);
			}
			if (this.PersonRank != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.PersonRank);
			}
			if (this.LastPersonRank != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.LastPersonRank);
			}
			num += this.killedBossList_.CalculateSize(GuildBossInfoDto._repeated_killedBossList_codec);
			if (this.SeasonEndTime != 0UL)
			{
				num += 2 + CodedOutputStream.ComputeUInt64Size(this.SeasonEndTime);
			}
			if (this.LastGuildRank != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.LastGuildRank);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 120U)
				{
					if (num <= 56U)
					{
						if (num <= 24U)
						{
							if (num == 8U)
							{
								this.ChallengeCnt = input.ReadUInt32();
								continue;
							}
							if (num == 16U)
							{
								this.BuyCntByDiamonds = input.ReadUInt32();
								continue;
							}
							if (num == 24U)
							{
								this.MaxBuyCntByDiamonds = input.ReadUInt32();
								continue;
							}
						}
						else if (num <= 40U)
						{
							if (num == 32U)
							{
								this.BuyCntCostByDiamonds = input.ReadUInt32();
								continue;
							}
							if (num == 40U)
							{
								this.BuyCntByCoins = input.ReadUInt32();
								continue;
							}
						}
						else
						{
							if (num == 48U)
							{
								this.MaxBuyCntByCoins = input.ReadUInt32();
								continue;
							}
							if (num == 56U)
							{
								this.BuyCntCostByCoins = input.ReadUInt32();
								continue;
							}
						}
					}
					else if (num <= 88U)
					{
						if (num <= 72U)
						{
							if (num == 64U)
							{
								this.ChallengeCntRecoverySeconds = input.ReadUInt32();
								continue;
							}
							if (num == 72U)
							{
								this.ChallengeCntRecoveryPerTime = input.ReadUInt32();
								continue;
							}
						}
						else
						{
							if (num == 80U)
							{
								this.NextChallengeCntRecoveryTime = input.ReadUInt64();
								continue;
							}
							if (num == 88U)
							{
								this.MaxRecoveryCnt = input.ReadUInt32();
								continue;
							}
						}
					}
					else if (num <= 104U)
					{
						if (num == 96U)
						{
							this.TotalPersonalDamage = input.ReadUInt64();
							continue;
						}
						if (num == 104U)
						{
							this.TotalGuildDamage = input.ReadUInt64();
							continue;
						}
					}
					else
					{
						if (num == 112U)
						{
							this.KillBossCnt = input.ReadUInt32();
							continue;
						}
						if (num == 120U)
						{
							this.BossRefreshTimestamp = input.ReadUInt64();
							continue;
						}
					}
				}
				else if (num <= 176U)
				{
					if (num <= 146U)
					{
						if (num == 128U)
						{
							this.DailyRefreshTimestamp = input.ReadUInt64();
							continue;
						}
						if (num == 138U)
						{
							if (this.bossConfig_ == null)
							{
								this.bossConfig_ = new GuildBossConfigDto();
							}
							input.ReadMessage(this.bossConfig_);
							continue;
						}
						if (num == 146U)
						{
							this.bossTask_.AddEntriesFrom(input, GuildBossInfoDto._repeated_bossTask_codec);
							continue;
						}
					}
					else if (num <= 162U)
					{
						if (num == 154U)
						{
							this.killBox_.AddEntriesFrom(input, GuildBossInfoDto._repeated_killBox_codec);
							continue;
						}
						if (num == 162U)
						{
							this.challengeRecords_.AddEntriesFrom(input, GuildBossInfoDto._repeated_challengeRecords_codec);
							continue;
						}
					}
					else
					{
						if (num == 168U)
						{
							this.DayPartCount = input.ReadUInt32();
							continue;
						}
						if (num == 176U)
						{
							this.GuildDan = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 208U)
				{
					if (num <= 192U)
					{
						if (num == 184U)
						{
							this.GuildSeason = input.ReadUInt32();
							continue;
						}
						if (num == 192U)
						{
							this.GuildRank = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 200U)
						{
							this.PersonRank = input.ReadInt32();
							continue;
						}
						if (num == 208U)
						{
							this.LastPersonRank = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 218U)
				{
					if (num == 216U || num == 218U)
					{
						this.killedBossList_.AddEntriesFrom(input, GuildBossInfoDto._repeated_killedBossList_codec);
						continue;
					}
				}
				else
				{
					if (num == 224U)
					{
						this.SeasonEndTime = input.ReadUInt64();
						continue;
					}
					if (num == 232U)
					{
						this.LastGuildRank = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildBossInfoDto> _parser = new MessageParser<GuildBossInfoDto>(() => new GuildBossInfoDto());

		public const int ChallengeCntFieldNumber = 1;

		private uint challengeCnt_;

		public const int BuyCntByDiamondsFieldNumber = 2;

		private uint buyCntByDiamonds_;

		public const int MaxBuyCntByDiamondsFieldNumber = 3;

		private uint maxBuyCntByDiamonds_;

		public const int BuyCntCostByDiamondsFieldNumber = 4;

		private uint buyCntCostByDiamonds_;

		public const int BuyCntByCoinsFieldNumber = 5;

		private uint buyCntByCoins_;

		public const int MaxBuyCntByCoinsFieldNumber = 6;

		private uint maxBuyCntByCoins_;

		public const int BuyCntCostByCoinsFieldNumber = 7;

		private uint buyCntCostByCoins_;

		public const int ChallengeCntRecoverySecondsFieldNumber = 8;

		private uint challengeCntRecoverySeconds_;

		public const int ChallengeCntRecoveryPerTimeFieldNumber = 9;

		private uint challengeCntRecoveryPerTime_;

		public const int NextChallengeCntRecoveryTimeFieldNumber = 10;

		private ulong nextChallengeCntRecoveryTime_;

		public const int MaxRecoveryCntFieldNumber = 11;

		private uint maxRecoveryCnt_;

		public const int TotalPersonalDamageFieldNumber = 12;

		private ulong totalPersonalDamage_;

		public const int TotalGuildDamageFieldNumber = 13;

		private ulong totalGuildDamage_;

		public const int KillBossCntFieldNumber = 14;

		private uint killBossCnt_;

		public const int BossRefreshTimestampFieldNumber = 15;

		private ulong bossRefreshTimestamp_;

		public const int DailyRefreshTimestampFieldNumber = 16;

		private ulong dailyRefreshTimestamp_;

		public const int BossConfigFieldNumber = 17;

		private GuildBossConfigDto bossConfig_;

		public const int BossTaskFieldNumber = 18;

		private static readonly FieldCodec<GuildBossTaskDto> _repeated_bossTask_codec = FieldCodec.ForMessage<GuildBossTaskDto>(146U, GuildBossTaskDto.Parser);

		private readonly RepeatedField<GuildBossTaskDto> bossTask_ = new RepeatedField<GuildBossTaskDto>();

		public const int KillBoxFieldNumber = 19;

		private static readonly FieldCodec<GuildBossKillBoxDto> _repeated_killBox_codec = FieldCodec.ForMessage<GuildBossKillBoxDto>(154U, GuildBossKillBoxDto.Parser);

		private readonly RepeatedField<GuildBossKillBoxDto> killBox_ = new RepeatedField<GuildBossKillBoxDto>();

		public const int ChallengeRecordsFieldNumber = 20;

		private static readonly FieldCodec<string> _repeated_challengeRecords_codec = FieldCodec.ForString(162U);

		private readonly RepeatedField<string> challengeRecords_ = new RepeatedField<string>();

		public const int DayPartCountFieldNumber = 21;

		private uint dayPartCount_;

		public const int GuildDanFieldNumber = 22;

		private uint guildDan_;

		public const int GuildSeasonFieldNumber = 23;

		private uint guildSeason_;

		public const int GuildRankFieldNumber = 24;

		private int guildRank_;

		public const int PersonRankFieldNumber = 25;

		private int personRank_;

		public const int LastPersonRankFieldNumber = 26;

		private int lastPersonRank_;

		public const int KilledBossListFieldNumber = 27;

		private static readonly FieldCodec<int> _repeated_killedBossList_codec = FieldCodec.ForInt32(218U);

		private readonly RepeatedField<int> killedBossList_ = new RepeatedField<int>();

		public const int SeasonEndTimeFieldNumber = 28;

		private ulong seasonEndTime_;

		public const int LastGuildRankFieldNumber = 29;

		private int lastGuildRank_;
	}
}
