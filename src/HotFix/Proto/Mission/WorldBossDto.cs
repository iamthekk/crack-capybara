using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.LeaderBoard;

namespace Proto.Mission
{
	public sealed class WorldBossDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<WorldBossDto> Parser
		{
			get
			{
				return WorldBossDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Id
		{
			get
			{
				return this.id_;
			}
			set
			{
				this.id_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Cost
		{
			get
			{
				return this.cost_;
			}
			set
			{
				this.cost_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long RefreshTimestamp
		{
			get
			{
				return this.refreshTimestamp_;
			}
			set
			{
				this.refreshTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long StartTimestamp
		{
			get
			{
				return this.startTimestamp_;
			}
			set
			{
				this.startTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long EndTimestamp
		{
			get
			{
				return this.endTimestamp_;
			}
			set
			{
				this.endTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Round
		{
			get
			{
				return this.round_;
			}
			set
			{
				this.round_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long RoundEndTimestamp
		{
			get
			{
				return this.roundEndTimestamp_;
			}
			set
			{
				this.roundEndTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int MissionId
		{
			get
			{
				return this.missionId_;
			}
			set
			{
				this.missionId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int GroupIndex
		{
			get
			{
				return this.groupIndex_;
			}
			set
			{
				this.groupIndex_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int BoxRewardId
		{
			get
			{
				return this.boxRewardId_;
			}
			set
			{
				this.boxRewardId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long Damage
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
		public RepeatedField<RankRewardDto> RankRewards
		{
			get
			{
				return this.rankRewards_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RankUserDto> RankList
		{
			get
			{
				return this.rankList_;
			}
		}

		[DebuggerNonUserCode]
		public int FreeCount
		{
			get
			{
				return this.freeCount_;
			}
			set
			{
				this.freeCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ChallengeCnt
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
		public int SelfRank
		{
			get
			{
				return this.selfRank_;
			}
			set
			{
				this.selfRank_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long NextOpenTimestamp
		{
			get
			{
				return this.nextOpenTimestamp_;
			}
			set
			{
				this.nextOpenTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int RankLvl
		{
			get
			{
				return this.rankLvl_;
			}
			set
			{
				this.rankLvl_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int GroupType
		{
			get
			{
				return this.groupType_;
			}
			set
			{
				this.groupType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int LastRank
		{
			get
			{
				return this.lastRank_;
			}
			set
			{
				this.lastRank_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int LastRankLvl
		{
			get
			{
				return this.lastRankLvl_;
			}
			set
			{
				this.lastRankLvl_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<BattleRecordDto> Records
		{
			get
			{
				return this.records_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Id != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Id);
			}
			if (this.Cost != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Cost);
			}
			if (this.RefreshTimestamp != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.RefreshTimestamp);
			}
			if (this.StartTimestamp != 0L)
			{
				output.WriteRawTag(32);
				output.WriteInt64(this.StartTimestamp);
			}
			if (this.EndTimestamp != 0L)
			{
				output.WriteRawTag(40);
				output.WriteInt64(this.EndTimestamp);
			}
			if (this.Round != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.Round);
			}
			if (this.RoundEndTimestamp != 0L)
			{
				output.WriteRawTag(56);
				output.WriteInt64(this.RoundEndTimestamp);
			}
			if (this.MissionId != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.MissionId);
			}
			if (this.GroupIndex != 0)
			{
				output.WriteRawTag(72);
				output.WriteInt32(this.GroupIndex);
			}
			if (this.BoxRewardId != 0)
			{
				output.WriteRawTag(80);
				output.WriteInt32(this.BoxRewardId);
			}
			if (this.Damage != 0L)
			{
				output.WriteRawTag(88);
				output.WriteInt64(this.Damage);
			}
			this.rankRewards_.WriteTo(output, WorldBossDto._repeated_rankRewards_codec);
			this.rankList_.WriteTo(output, WorldBossDto._repeated_rankList_codec);
			if (this.FreeCount != 0)
			{
				output.WriteRawTag(112);
				output.WriteInt32(this.FreeCount);
			}
			if (this.ChallengeCnt != 0)
			{
				output.WriteRawTag(120);
				output.WriteInt32(this.ChallengeCnt);
			}
			if (this.SelfRank != 0)
			{
				output.WriteRawTag(128, 1);
				output.WriteInt32(this.SelfRank);
			}
			if (this.NextOpenTimestamp != 0L)
			{
				output.WriteRawTag(136, 1);
				output.WriteInt64(this.NextOpenTimestamp);
			}
			if (this.RankLvl != 0)
			{
				output.WriteRawTag(144, 1);
				output.WriteInt32(this.RankLvl);
			}
			if (this.GroupType != 0)
			{
				output.WriteRawTag(152, 1);
				output.WriteInt32(this.GroupType);
			}
			if (this.LastRank != 0)
			{
				output.WriteRawTag(160, 1);
				output.WriteInt32(this.LastRank);
			}
			if (this.LastRankLvl != 0)
			{
				output.WriteRawTag(168, 1);
				output.WriteInt32(this.LastRankLvl);
			}
			this.records_.WriteTo(output, WorldBossDto._repeated_records_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Id != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Id);
			}
			if (this.Cost != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Cost);
			}
			if (this.RefreshTimestamp != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.RefreshTimestamp);
			}
			if (this.StartTimestamp != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.StartTimestamp);
			}
			if (this.EndTimestamp != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.EndTimestamp);
			}
			if (this.Round != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Round);
			}
			if (this.RoundEndTimestamp != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.RoundEndTimestamp);
			}
			if (this.MissionId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.MissionId);
			}
			if (this.GroupIndex != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.GroupIndex);
			}
			if (this.BoxRewardId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.BoxRewardId);
			}
			if (this.Damage != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Damage);
			}
			num += this.rankRewards_.CalculateSize(WorldBossDto._repeated_rankRewards_codec);
			num += this.rankList_.CalculateSize(WorldBossDto._repeated_rankList_codec);
			if (this.FreeCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.FreeCount);
			}
			if (this.ChallengeCnt != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ChallengeCnt);
			}
			if (this.SelfRank != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.SelfRank);
			}
			if (this.NextOpenTimestamp != 0L)
			{
				num += 2 + CodedOutputStream.ComputeInt64Size(this.NextOpenTimestamp);
			}
			if (this.RankLvl != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.RankLvl);
			}
			if (this.GroupType != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.GroupType);
			}
			if (this.LastRank != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.LastRank);
			}
			if (this.LastRankLvl != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.LastRankLvl);
			}
			return num + this.records_.CalculateSize(WorldBossDto._repeated_records_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 88U)
				{
					if (num <= 40U)
					{
						if (num <= 16U)
						{
							if (num == 8U)
							{
								this.Id = input.ReadInt32();
								continue;
							}
							if (num == 16U)
							{
								this.Cost = input.ReadInt32();
								continue;
							}
						}
						else
						{
							if (num == 24U)
							{
								this.RefreshTimestamp = input.ReadInt64();
								continue;
							}
							if (num == 32U)
							{
								this.StartTimestamp = input.ReadInt64();
								continue;
							}
							if (num == 40U)
							{
								this.EndTimestamp = input.ReadInt64();
								continue;
							}
						}
					}
					else if (num <= 64U)
					{
						if (num == 48U)
						{
							this.Round = input.ReadInt32();
							continue;
						}
						if (num == 56U)
						{
							this.RoundEndTimestamp = input.ReadInt64();
							continue;
						}
						if (num == 64U)
						{
							this.MissionId = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 72U)
						{
							this.GroupIndex = input.ReadInt32();
							continue;
						}
						if (num == 80U)
						{
							this.BoxRewardId = input.ReadInt32();
							continue;
						}
						if (num == 88U)
						{
							this.Damage = input.ReadInt64();
							continue;
						}
					}
				}
				else if (num <= 128U)
				{
					if (num <= 106U)
					{
						if (num == 98U)
						{
							this.rankRewards_.AddEntriesFrom(input, WorldBossDto._repeated_rankRewards_codec);
							continue;
						}
						if (num == 106U)
						{
							this.rankList_.AddEntriesFrom(input, WorldBossDto._repeated_rankList_codec);
							continue;
						}
					}
					else
					{
						if (num == 112U)
						{
							this.FreeCount = input.ReadInt32();
							continue;
						}
						if (num == 120U)
						{
							this.ChallengeCnt = input.ReadInt32();
							continue;
						}
						if (num == 128U)
						{
							this.SelfRank = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 152U)
				{
					if (num == 136U)
					{
						this.NextOpenTimestamp = input.ReadInt64();
						continue;
					}
					if (num == 144U)
					{
						this.RankLvl = input.ReadInt32();
						continue;
					}
					if (num == 152U)
					{
						this.GroupType = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 160U)
					{
						this.LastRank = input.ReadInt32();
						continue;
					}
					if (num == 168U)
					{
						this.LastRankLvl = input.ReadInt32();
						continue;
					}
					if (num == 178U)
					{
						this.records_.AddEntriesFrom(input, WorldBossDto._repeated_records_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<WorldBossDto> _parser = new MessageParser<WorldBossDto>(() => new WorldBossDto());

		public const int IdFieldNumber = 1;

		private int id_;

		public const int CostFieldNumber = 2;

		private int cost_;

		public const int RefreshTimestampFieldNumber = 3;

		private long refreshTimestamp_;

		public const int StartTimestampFieldNumber = 4;

		private long startTimestamp_;

		public const int EndTimestampFieldNumber = 5;

		private long endTimestamp_;

		public const int RoundFieldNumber = 6;

		private int round_;

		public const int RoundEndTimestampFieldNumber = 7;

		private long roundEndTimestamp_;

		public const int MissionIdFieldNumber = 8;

		private int missionId_;

		public const int GroupIndexFieldNumber = 9;

		private int groupIndex_;

		public const int BoxRewardIdFieldNumber = 10;

		private int boxRewardId_;

		public const int DamageFieldNumber = 11;

		private long damage_;

		public const int RankRewardsFieldNumber = 12;

		private static readonly FieldCodec<RankRewardDto> _repeated_rankRewards_codec = FieldCodec.ForMessage<RankRewardDto>(98U, RankRewardDto.Parser);

		private readonly RepeatedField<RankRewardDto> rankRewards_ = new RepeatedField<RankRewardDto>();

		public const int RankListFieldNumber = 13;

		private static readonly FieldCodec<RankUserDto> _repeated_rankList_codec = FieldCodec.ForMessage<RankUserDto>(106U, RankUserDto.Parser);

		private readonly RepeatedField<RankUserDto> rankList_ = new RepeatedField<RankUserDto>();

		public const int FreeCountFieldNumber = 14;

		private int freeCount_;

		public const int ChallengeCntFieldNumber = 15;

		private int challengeCnt_;

		public const int SelfRankFieldNumber = 16;

		private int selfRank_;

		public const int NextOpenTimestampFieldNumber = 17;

		private long nextOpenTimestamp_;

		public const int RankLvlFieldNumber = 18;

		private int rankLvl_;

		public const int GroupTypeFieldNumber = 19;

		private int groupType_;

		public const int LastRankFieldNumber = 20;

		private int lastRank_;

		public const int LastRankLvlFieldNumber = 21;

		private int lastRankLvl_;

		public const int RecordsFieldNumber = 22;

		private static readonly FieldCodec<BattleRecordDto> _repeated_records_codec = FieldCodec.ForMessage<BattleRecordDto>(178U, BattleRecordDto.Parser);

		private readonly RepeatedField<BattleRecordDto> records_ = new RepeatedField<BattleRecordDto>();
	}
}
