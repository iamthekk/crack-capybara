using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Tower
{
	public sealed class HellDoChallengeResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<HellDoChallengeResponse> Parser
		{
			get
			{
				return HellDoChallengeResponse._parser;
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
		public int Result
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
		public uint WaveRate
		{
			get
			{
				return this.waveRate_;
			}
			set
			{
				this.waveRate_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long Hp
		{
			get
			{
				return this.hp_;
			}
			set
			{
				this.hp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int RevertCount
		{
			get
			{
				return this.revertCount_;
			}
			set
			{
				this.revertCount_ = value;
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
		public uint Stage
		{
			get
			{
				return this.stage_;
			}
			set
			{
				this.stage_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint RoundPassStage
		{
			get
			{
				return this.roundPassStage_;
			}
			set
			{
				this.roundPassStage_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint StageSeed
		{
			get
			{
				return this.stageSeed_;
			}
			set
			{
				this.stageSeed_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint EventId
		{
			get
			{
				return this.eventId_;
			}
			set
			{
				this.eventId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> MonsterCfgId
		{
			get
			{
				return this.monsterCfgId_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> MonsterCfgIdNext
		{
			get
			{
				return this.monsterCfgIdNext_;
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
			if (this.Result != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Result);
			}
			if (this.WaveRate != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.WaveRate);
			}
			if (this.Hp != 0L)
			{
				output.WriteRawTag(40);
				output.WriteInt64(this.Hp);
			}
			if (this.RevertCount != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.RevertCount);
			}
			if (this.Seed != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.Seed);
			}
			this.startUnits_.WriteTo(output, HellDoChallengeResponse._repeated_startUnits_codec);
			this.endUnits_.WriteTo(output, HellDoChallengeResponse._repeated_endUnits_codec);
			if (this.userInfo_ != null)
			{
				output.WriteRawTag(82);
				output.WriteMessage(this.UserInfo);
			}
			if (this.Stage != 0U)
			{
				output.WriteRawTag(88);
				output.WriteUInt32(this.Stage);
			}
			if (this.RoundPassStage != 0U)
			{
				output.WriteRawTag(96);
				output.WriteUInt32(this.RoundPassStage);
			}
			if (this.StageSeed != 0U)
			{
				output.WriteRawTag(104);
				output.WriteUInt32(this.StageSeed);
			}
			if (this.EventId != 0U)
			{
				output.WriteRawTag(112);
				output.WriteUInt32(this.EventId);
			}
			this.monsterCfgId_.WriteTo(output, HellDoChallengeResponse._repeated_monsterCfgId_codec);
			this.monsterCfgIdNext_.WriteTo(output, HellDoChallengeResponse._repeated_monsterCfgIdNext_codec);
			if (this.BattleServerLogId.Length != 0)
			{
				output.WriteRawTag(138, 1);
				output.WriteString(this.BattleServerLogId);
			}
			if (this.BattleServerLogData.Length != 0)
			{
				output.WriteRawTag(146, 1);
				output.WriteString(this.BattleServerLogData);
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
			if (this.Result != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Result);
			}
			if (this.WaveRate != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.WaveRate);
			}
			if (this.Hp != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Hp);
			}
			if (this.RevertCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.RevertCount);
			}
			if (this.Seed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seed);
			}
			num += this.startUnits_.CalculateSize(HellDoChallengeResponse._repeated_startUnits_codec);
			num += this.endUnits_.CalculateSize(HellDoChallengeResponse._repeated_endUnits_codec);
			if (this.userInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserInfo);
			}
			if (this.Stage != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Stage);
			}
			if (this.RoundPassStage != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.RoundPassStage);
			}
			if (this.StageSeed != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.StageSeed);
			}
			if (this.EventId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.EventId);
			}
			num += this.monsterCfgId_.CalculateSize(HellDoChallengeResponse._repeated_monsterCfgId_codec);
			num += this.monsterCfgIdNext_.CalculateSize(HellDoChallengeResponse._repeated_monsterCfgIdNext_codec);
			if (this.BattleServerLogId.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.BattleServerLogId);
			}
			if (this.BattleServerLogData.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.BattleServerLogData);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num > 82U)
				{
					if (num > 120U)
					{
						if (num <= 128U)
						{
							if (num == 122U)
							{
								goto IL_0207;
							}
							if (num != 128U)
							{
								goto IL_00ED;
							}
						}
						else if (num != 130U)
						{
							if (num == 138U)
							{
								this.BattleServerLogId = input.ReadString();
								continue;
							}
							if (num != 146U)
							{
								goto IL_00ED;
							}
							this.BattleServerLogData = input.ReadString();
							continue;
						}
						this.monsterCfgIdNext_.AddEntriesFrom(input, HellDoChallengeResponse._repeated_monsterCfgIdNext_codec);
						continue;
					}
					if (num <= 96U)
					{
						if (num == 88U)
						{
							this.Stage = input.ReadUInt32();
							continue;
						}
						if (num != 96U)
						{
							goto IL_00ED;
						}
						this.RoundPassStage = input.ReadUInt32();
						continue;
					}
					else
					{
						if (num == 104U)
						{
							this.StageSeed = input.ReadUInt32();
							continue;
						}
						if (num == 112U)
						{
							this.EventId = input.ReadUInt32();
							continue;
						}
						if (num != 120U)
						{
							goto IL_00ED;
						}
					}
					IL_0207:
					this.monsterCfgId_.AddEntriesFrom(input, HellDoChallengeResponse._repeated_monsterCfgId_codec);
					continue;
				}
				if (num <= 40U)
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
							this.Result = input.ReadInt32();
							continue;
						}
						if (num == 32U)
						{
							this.WaveRate = input.ReadUInt32();
							continue;
						}
						if (num == 40U)
						{
							this.Hp = input.ReadInt64();
							continue;
						}
					}
				}
				else if (num <= 56U)
				{
					if (num == 48U)
					{
						this.RevertCount = input.ReadInt32();
						continue;
					}
					if (num == 56U)
					{
						this.Seed = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 66U)
					{
						this.startUnits_.AddEntriesFrom(input, HellDoChallengeResponse._repeated_startUnits_codec);
						continue;
					}
					if (num == 74U)
					{
						this.endUnits_.AddEntriesFrom(input, HellDoChallengeResponse._repeated_endUnits_codec);
						continue;
					}
					if (num == 82U)
					{
						if (this.userInfo_ == null)
						{
							this.userInfo_ = new BattleUserDto();
						}
						input.ReadMessage(this.userInfo_);
						continue;
					}
				}
				IL_00ED:
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<HellDoChallengeResponse> _parser = new MessageParser<HellDoChallengeResponse>(() => new HellDoChallengeResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ResultFieldNumber = 3;

		private int result_;

		public const int WaveRateFieldNumber = 4;

		private uint waveRate_;

		public const int HpFieldNumber = 5;

		private long hp_;

		public const int RevertCountFieldNumber = 6;

		private int revertCount_;

		public const int SeedFieldNumber = 7;

		private int seed_;

		public const int StartUnitsFieldNumber = 8;

		private static readonly FieldCodec<CombatUnitDto> _repeated_startUnits_codec = FieldCodec.ForMessage<CombatUnitDto>(66U, CombatUnitDto.Parser);

		private readonly RepeatedField<CombatUnitDto> startUnits_ = new RepeatedField<CombatUnitDto>();

		public const int EndUnitsFieldNumber = 9;

		private static readonly FieldCodec<CombatUnitDto> _repeated_endUnits_codec = FieldCodec.ForMessage<CombatUnitDto>(74U, CombatUnitDto.Parser);

		private readonly RepeatedField<CombatUnitDto> endUnits_ = new RepeatedField<CombatUnitDto>();

		public const int UserInfoFieldNumber = 10;

		private BattleUserDto userInfo_;

		public const int StageFieldNumber = 11;

		private uint stage_;

		public const int RoundPassStageFieldNumber = 12;

		private uint roundPassStage_;

		public const int StageSeedFieldNumber = 13;

		private uint stageSeed_;

		public const int EventIdFieldNumber = 14;

		private uint eventId_;

		public const int MonsterCfgIdFieldNumber = 15;

		private static readonly FieldCodec<int> _repeated_monsterCfgId_codec = FieldCodec.ForInt32(122U);

		private readonly RepeatedField<int> monsterCfgId_ = new RepeatedField<int>();

		public const int MonsterCfgIdNextFieldNumber = 16;

		private static readonly FieldCodec<int> _repeated_monsterCfgIdNext_codec = FieldCodec.ForInt32(130U);

		private readonly RepeatedField<int> monsterCfgIdNext_ = new RepeatedField<int>();

		public const int BattleServerLogIdFieldNumber = 17;

		private string battleServerLogId_ = "";

		public const int BattleServerLogDataFieldNumber = 18;

		private string battleServerLogData_ = "";
	}
}
