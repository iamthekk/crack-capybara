using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class IAPDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<IAPDto> Parser
		{
			get
			{
				return IAPDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, uint> PacksBuyCount
		{
			get
			{
				return this.packsBuyCount_;
			}
		}

		[DebuggerNonUserCode]
		public ulong PacksResetTimeDay
		{
			get
			{
				return this.packsResetTimeDay_;
			}
			set
			{
				this.packsResetTimeDay_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong PacksResetTimeWeek
		{
			get
			{
				return this.packsResetTimeWeek_;
			}
			set
			{
				this.packsResetTimeWeek_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong PacksResetTimeMonth
		{
			get
			{
				return this.packsResetTimeMonth_;
			}
			set
			{
				this.packsResetTimeMonth_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, IAPMonthCardDto> MonthCardMap
		{
			get
			{
				return this.monthCardMap_;
			}
		}

		[DebuggerNonUserCode]
		public IAPBattlePassDto BattlePassInfo
		{
			get
			{
				return this.battlePassInfo_;
			}
			set
			{
				this.battlePassInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool FirstRechargeReward
		{
			get
			{
				return this.firstRechargeReward_;
			}
			set
			{
				this.firstRechargeReward_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TotalRecharge
		{
			get
			{
				return this.totalRecharge_;
			}
			set
			{
				this.totalRecharge_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<uint> BuyOpenServerGiftId
		{
			get
			{
				return this.buyOpenServerGiftId_;
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
		public RepeatedField<uint> BuyLevelFundGroupId
		{
			get
			{
				return this.buyLevelFundGroupId_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, IntegerArray> LevelFundReward
		{
			get
			{
				return this.levelFundReward_;
			}
		}

		[DebuggerNonUserCode]
		public int IsHeadFrameActive
		{
			get
			{
				return this.isHeadFrameActive_;
			}
			set
			{
				this.isHeadFrameActive_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, IntegerArray> FreeLevelFundReward
		{
			get
			{
				return this.freeLevelFundReward_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<uint> BuyFirstChargeGiftId
		{
			get
			{
				return this.buyFirstChargeGiftId_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, ulong> FirstChargeGiftPassTime
		{
			get
			{
				return this.firstChargeGiftPassTime_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<int, int> TurntablePayCount
		{
			get
			{
				return this.turntablePayCount_;
			}
		}

		[DebuggerNonUserCode]
		public uint TotalRechargeMonth
		{
			get
			{
				return this.totalRechargeMonth_;
			}
			set
			{
				this.totalRechargeMonth_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<int, ulong> BuyFirstChargeGiftInfo
		{
			get
			{
				return this.buyFirstChargeGiftInfo_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<int, uint> FirstChargeGiftReward
		{
			get
			{
				return this.firstChargeGiftReward_;
			}
		}

		[DebuggerNonUserCode]
		public long FirstChargeGiftEndTime
		{
			get
			{
				return this.firstChargeGiftEndTime_;
			}
			set
			{
				this.firstChargeGiftEndTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ChapterBattlePassDto ChapterBattlePassDto
		{
			get
			{
				return this.chapterBattlePassDto_;
			}
			set
			{
				this.chapterBattlePassDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.packsBuyCount_.WriteTo(output, IAPDto._map_packsBuyCount_codec);
			if (this.PacksResetTimeDay != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.PacksResetTimeDay);
			}
			if (this.PacksResetTimeWeek != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.PacksResetTimeWeek);
			}
			if (this.PacksResetTimeMonth != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.PacksResetTimeMonth);
			}
			this.monthCardMap_.WriteTo(output, IAPDto._map_monthCardMap_codec);
			if (this.battlePassInfo_ != null)
			{
				output.WriteRawTag(50);
				output.WriteMessage(this.BattlePassInfo);
			}
			if (this.FirstRechargeReward)
			{
				output.WriteRawTag(56);
				output.WriteBool(this.FirstRechargeReward);
			}
			if (this.TotalRecharge != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.TotalRecharge);
			}
			this.buyOpenServerGiftId_.WriteTo(output, IAPDto._repeated_buyOpenServerGiftId_codec);
			this.chapterGiftTime_.WriteTo(output, IAPDto._map_chapterGiftTime_codec);
			this.buyLevelFundGroupId_.WriteTo(output, IAPDto._repeated_buyLevelFundGroupId_codec);
			this.levelFundReward_.WriteTo(output, IAPDto._map_levelFundReward_codec);
			if (this.IsHeadFrameActive != 0)
			{
				output.WriteRawTag(104);
				output.WriteInt32(this.IsHeadFrameActive);
			}
			this.freeLevelFundReward_.WriteTo(output, IAPDto._map_freeLevelFundReward_codec);
			this.buyFirstChargeGiftId_.WriteTo(output, IAPDto._repeated_buyFirstChargeGiftId_codec);
			this.firstChargeGiftPassTime_.WriteTo(output, IAPDto._map_firstChargeGiftPassTime_codec);
			this.turntablePayCount_.WriteTo(output, IAPDto._map_turntablePayCount_codec);
			if (this.TotalRechargeMonth != 0U)
			{
				output.WriteRawTag(144, 1);
				output.WriteUInt32(this.TotalRechargeMonth);
			}
			this.buyFirstChargeGiftInfo_.WriteTo(output, IAPDto._map_buyFirstChargeGiftInfo_codec);
			this.firstChargeGiftReward_.WriteTo(output, IAPDto._map_firstChargeGiftReward_codec);
			if (this.FirstChargeGiftEndTime != 0L)
			{
				output.WriteRawTag(168, 1);
				output.WriteInt64(this.FirstChargeGiftEndTime);
			}
			if (this.chapterBattlePassDto_ != null)
			{
				output.WriteRawTag(178, 1);
				output.WriteMessage(this.ChapterBattlePassDto);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.packsBuyCount_.CalculateSize(IAPDto._map_packsBuyCount_codec);
			if (this.PacksResetTimeDay != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.PacksResetTimeDay);
			}
			if (this.PacksResetTimeWeek != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.PacksResetTimeWeek);
			}
			if (this.PacksResetTimeMonth != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.PacksResetTimeMonth);
			}
			num += this.monthCardMap_.CalculateSize(IAPDto._map_monthCardMap_codec);
			if (this.battlePassInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.BattlePassInfo);
			}
			if (this.FirstRechargeReward)
			{
				num += 2;
			}
			if (this.TotalRecharge != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TotalRecharge);
			}
			num += this.buyOpenServerGiftId_.CalculateSize(IAPDto._repeated_buyOpenServerGiftId_codec);
			num += this.chapterGiftTime_.CalculateSize(IAPDto._map_chapterGiftTime_codec);
			num += this.buyLevelFundGroupId_.CalculateSize(IAPDto._repeated_buyLevelFundGroupId_codec);
			num += this.levelFundReward_.CalculateSize(IAPDto._map_levelFundReward_codec);
			if (this.IsHeadFrameActive != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.IsHeadFrameActive);
			}
			num += this.freeLevelFundReward_.CalculateSize(IAPDto._map_freeLevelFundReward_codec);
			num += this.buyFirstChargeGiftId_.CalculateSize(IAPDto._repeated_buyFirstChargeGiftId_codec);
			num += this.firstChargeGiftPassTime_.CalculateSize(IAPDto._map_firstChargeGiftPassTime_codec);
			num += this.turntablePayCount_.CalculateSize(IAPDto._map_turntablePayCount_codec);
			if (this.TotalRechargeMonth != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.TotalRechargeMonth);
			}
			num += this.buyFirstChargeGiftInfo_.CalculateSize(IAPDto._map_buyFirstChargeGiftInfo_codec);
			num += this.firstChargeGiftReward_.CalculateSize(IAPDto._map_firstChargeGiftReward_codec);
			if (this.FirstChargeGiftEndTime != 0L)
			{
				num += 2 + CodedOutputStream.ComputeInt64Size(this.FirstChargeGiftEndTime);
			}
			if (this.chapterBattlePassDto_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.ChapterBattlePassDto);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 88U)
				{
					if (num > 50U)
					{
						if (num <= 72U)
						{
							if (num == 56U)
							{
								this.FirstRechargeReward = input.ReadBool();
								continue;
							}
							if (num == 64U)
							{
								this.TotalRecharge = input.ReadUInt32();
								continue;
							}
							if (num != 72U)
							{
								goto IL_012F;
							}
						}
						else if (num != 74U)
						{
							if (num == 82U)
							{
								this.chapterGiftTime_.AddEntriesFrom(input, IAPDto._map_chapterGiftTime_codec);
								continue;
							}
							if (num != 88U)
							{
								goto IL_012F;
							}
							goto IL_020B;
						}
						this.buyOpenServerGiftId_.AddEntriesFrom(input, IAPDto._repeated_buyOpenServerGiftId_codec);
						continue;
					}
					if (num <= 24U)
					{
						if (num == 10U)
						{
							this.packsBuyCount_.AddEntriesFrom(input, IAPDto._map_packsBuyCount_codec);
							continue;
						}
						if (num == 16U)
						{
							this.PacksResetTimeDay = input.ReadUInt64();
							continue;
						}
						if (num == 24U)
						{
							this.PacksResetTimeWeek = input.ReadUInt64();
							continue;
						}
					}
					else
					{
						if (num == 32U)
						{
							this.PacksResetTimeMonth = input.ReadUInt64();
							continue;
						}
						if (num == 42U)
						{
							this.monthCardMap_.AddEntriesFrom(input, IAPDto._map_monthCardMap_codec);
							continue;
						}
						if (num == 50U)
						{
							if (this.battlePassInfo_ == null)
							{
								this.battlePassInfo_ = new IAPBattlePassDto();
							}
							input.ReadMessage(this.battlePassInfo_);
							continue;
						}
					}
				}
				else if (num <= 122U)
				{
					if (num <= 104U)
					{
						if (num == 90U)
						{
							goto IL_020B;
						}
						if (num == 98U)
						{
							this.levelFundReward_.AddEntriesFrom(input, IAPDto._map_levelFundReward_codec);
							continue;
						}
						if (num == 104U)
						{
							this.IsHeadFrameActive = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 114U)
						{
							this.freeLevelFundReward_.AddEntriesFrom(input, IAPDto._map_freeLevelFundReward_codec);
							continue;
						}
						if (num == 120U || num == 122U)
						{
							this.buyFirstChargeGiftId_.AddEntriesFrom(input, IAPDto._repeated_buyFirstChargeGiftId_codec);
							continue;
						}
					}
				}
				else if (num <= 144U)
				{
					if (num == 130U)
					{
						this.firstChargeGiftPassTime_.AddEntriesFrom(input, IAPDto._map_firstChargeGiftPassTime_codec);
						continue;
					}
					if (num == 138U)
					{
						this.turntablePayCount_.AddEntriesFrom(input, IAPDto._map_turntablePayCount_codec);
						continue;
					}
					if (num == 144U)
					{
						this.TotalRechargeMonth = input.ReadUInt32();
						continue;
					}
				}
				else if (num <= 162U)
				{
					if (num == 154U)
					{
						this.buyFirstChargeGiftInfo_.AddEntriesFrom(input, IAPDto._map_buyFirstChargeGiftInfo_codec);
						continue;
					}
					if (num == 162U)
					{
						this.firstChargeGiftReward_.AddEntriesFrom(input, IAPDto._map_firstChargeGiftReward_codec);
						continue;
					}
				}
				else
				{
					if (num == 168U)
					{
						this.FirstChargeGiftEndTime = input.ReadInt64();
						continue;
					}
					if (num == 178U)
					{
						if (this.chapterBattlePassDto_ == null)
						{
							this.chapterBattlePassDto_ = new ChapterBattlePassDto();
						}
						input.ReadMessage(this.chapterBattlePassDto_);
						continue;
					}
				}
				IL_012F:
				input.SkipLastField();
				continue;
				IL_020B:
				this.buyLevelFundGroupId_.AddEntriesFrom(input, IAPDto._repeated_buyLevelFundGroupId_codec);
			}
		}

		private static readonly MessageParser<IAPDto> _parser = new MessageParser<IAPDto>(() => new IAPDto());

		public const int PacksBuyCountFieldNumber = 1;

		private static readonly MapField<uint, uint>.Codec _map_packsBuyCount_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 10U);

		private readonly MapField<uint, uint> packsBuyCount_ = new MapField<uint, uint>();

		public const int PacksResetTimeDayFieldNumber = 2;

		private ulong packsResetTimeDay_;

		public const int PacksResetTimeWeekFieldNumber = 3;

		private ulong packsResetTimeWeek_;

		public const int PacksResetTimeMonthFieldNumber = 4;

		private ulong packsResetTimeMonth_;

		public const int MonthCardMapFieldNumber = 5;

		private static readonly MapField<uint, IAPMonthCardDto>.Codec _map_monthCardMap_codec = new MapField<uint, IAPMonthCardDto>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForMessage<IAPMonthCardDto>(18U, IAPMonthCardDto.Parser), 42U);

		private readonly MapField<uint, IAPMonthCardDto> monthCardMap_ = new MapField<uint, IAPMonthCardDto>();

		public const int BattlePassInfoFieldNumber = 6;

		private IAPBattlePassDto battlePassInfo_;

		public const int FirstRechargeRewardFieldNumber = 7;

		private bool firstRechargeReward_;

		public const int TotalRechargeFieldNumber = 8;

		private uint totalRecharge_;

		public const int BuyOpenServerGiftIdFieldNumber = 9;

		private static readonly FieldCodec<uint> _repeated_buyOpenServerGiftId_codec = FieldCodec.ForUInt32(74U);

		private readonly RepeatedField<uint> buyOpenServerGiftId_ = new RepeatedField<uint>();

		public const int ChapterGiftTimeFieldNumber = 10;

		private static readonly MapField<uint, ulong>.Codec _map_chapterGiftTime_codec = new MapField<uint, ulong>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt64(16U), 82U);

		private readonly MapField<uint, ulong> chapterGiftTime_ = new MapField<uint, ulong>();

		public const int BuyLevelFundGroupIdFieldNumber = 11;

		private static readonly FieldCodec<uint> _repeated_buyLevelFundGroupId_codec = FieldCodec.ForUInt32(90U);

		private readonly RepeatedField<uint> buyLevelFundGroupId_ = new RepeatedField<uint>();

		public const int LevelFundRewardFieldNumber = 12;

		private static readonly MapField<uint, IntegerArray>.Codec _map_levelFundReward_codec = new MapField<uint, IntegerArray>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForMessage<IntegerArray>(18U, IntegerArray.Parser), 98U);

		private readonly MapField<uint, IntegerArray> levelFundReward_ = new MapField<uint, IntegerArray>();

		public const int IsHeadFrameActiveFieldNumber = 13;

		private int isHeadFrameActive_;

		public const int FreeLevelFundRewardFieldNumber = 14;

		private static readonly MapField<uint, IntegerArray>.Codec _map_freeLevelFundReward_codec = new MapField<uint, IntegerArray>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForMessage<IntegerArray>(18U, IntegerArray.Parser), 114U);

		private readonly MapField<uint, IntegerArray> freeLevelFundReward_ = new MapField<uint, IntegerArray>();

		public const int BuyFirstChargeGiftIdFieldNumber = 15;

		private static readonly FieldCodec<uint> _repeated_buyFirstChargeGiftId_codec = FieldCodec.ForUInt32(122U);

		private readonly RepeatedField<uint> buyFirstChargeGiftId_ = new RepeatedField<uint>();

		public const int FirstChargeGiftPassTimeFieldNumber = 16;

		private static readonly MapField<uint, ulong>.Codec _map_firstChargeGiftPassTime_codec = new MapField<uint, ulong>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt64(16U), 130U);

		private readonly MapField<uint, ulong> firstChargeGiftPassTime_ = new MapField<uint, ulong>();

		public const int TurntablePayCountFieldNumber = 17;

		private static readonly MapField<int, int>.Codec _map_turntablePayCount_codec = new MapField<int, int>.Codec(FieldCodec.ForInt32(8U), FieldCodec.ForInt32(16U), 138U);

		private readonly MapField<int, int> turntablePayCount_ = new MapField<int, int>();

		public const int TotalRechargeMonthFieldNumber = 18;

		private uint totalRechargeMonth_;

		public const int BuyFirstChargeGiftInfoFieldNumber = 19;

		private static readonly MapField<int, ulong>.Codec _map_buyFirstChargeGiftInfo_codec = new MapField<int, ulong>.Codec(FieldCodec.ForInt32(8U), FieldCodec.ForUInt64(16U), 154U);

		private readonly MapField<int, ulong> buyFirstChargeGiftInfo_ = new MapField<int, ulong>();

		public const int FirstChargeGiftRewardFieldNumber = 20;

		private static readonly MapField<int, uint>.Codec _map_firstChargeGiftReward_codec = new MapField<int, uint>.Codec(FieldCodec.ForInt32(8U), FieldCodec.ForUInt32(16U), 162U);

		private readonly MapField<int, uint> firstChargeGiftReward_ = new MapField<int, uint>();

		public const int FirstChargeGiftEndTimeFieldNumber = 21;

		private long firstChargeGiftEndTime_;

		public const int ChapterBattlePassDtoFieldNumber = 22;

		private ChapterBattlePassDto chapterBattlePassDto_;
	}
}
