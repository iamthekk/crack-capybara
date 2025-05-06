using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Tower
{
	public sealed class HellGetPanelInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<HellGetPanelInfoResponse> Parser
		{
			get
			{
				return HellGetPanelInfoResponse._parser;
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
		public uint CurStage
		{
			get
			{
				return this.curStage_;
			}
			set
			{
				this.curStage_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint BattleStatus
		{
			get
			{
				return this.battleStatus_;
			}
			set
			{
				this.battleStatus_ = value;
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
		public RepeatedField<int> MonsterSkillList
		{
			get
			{
				return this.monsterSkillList_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> RoundSkillList
		{
			get
			{
				return this.roundSkillList_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<int, SkillListDto> StageSkillMap
		{
			get
			{
				return this.stageSkillMap_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<string, int> AttrMap
		{
			get
			{
				return this.attrMap_;
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
		public RepeatedField<int> AllSkillList
		{
			get
			{
				return this.allSkillList_;
			}
		}

		[DebuggerNonUserCode]
		public uint IsEnterSkill
		{
			get
			{
				return this.isEnterSkill_;
			}
			set
			{
				this.isEnterSkill_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint IsRoundSkill
		{
			get
			{
				return this.isRoundSkill_;
			}
			set
			{
				this.isRoundSkill_ = value;
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
			if (this.CurStage != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.CurStage);
			}
			if (this.BattleStatus != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.BattleStatus);
			}
			if (this.StageSeed != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.StageSeed);
			}
			if (this.RoundPassStage != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.RoundPassStage);
			}
			this.monsterSkillList_.WriteTo(output, HellGetPanelInfoResponse._repeated_monsterSkillList_codec);
			this.roundSkillList_.WriteTo(output, HellGetPanelInfoResponse._repeated_roundSkillList_codec);
			this.stageSkillMap_.WriteTo(output, HellGetPanelInfoResponse._map_stageSkillMap_codec);
			this.attrMap_.WriteTo(output, HellGetPanelInfoResponse._map_attrMap_codec);
			this.monsterCfgId_.WriteTo(output, HellGetPanelInfoResponse._repeated_monsterCfgId_codec);
			if (this.Hp != 0L)
			{
				output.WriteRawTag(96);
				output.WriteInt64(this.Hp);
			}
			if (this.RevertCount != 0)
			{
				output.WriteRawTag(104);
				output.WriteInt32(this.RevertCount);
			}
			if (this.WaveRate != 0U)
			{
				output.WriteRawTag(112);
				output.WriteUInt32(this.WaveRate);
			}
			if (this.EventId != 0U)
			{
				output.WriteRawTag(120);
				output.WriteUInt32(this.EventId);
			}
			this.allSkillList_.WriteTo(output, HellGetPanelInfoResponse._repeated_allSkillList_codec);
			if (this.IsEnterSkill != 0U)
			{
				output.WriteRawTag(136, 1);
				output.WriteUInt32(this.IsEnterSkill);
			}
			if (this.IsRoundSkill != 0U)
			{
				output.WriteRawTag(144, 1);
				output.WriteUInt32(this.IsRoundSkill);
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
			if (this.CurStage != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.CurStage);
			}
			if (this.BattleStatus != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BattleStatus);
			}
			if (this.StageSeed != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.StageSeed);
			}
			if (this.RoundPassStage != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.RoundPassStage);
			}
			num += this.monsterSkillList_.CalculateSize(HellGetPanelInfoResponse._repeated_monsterSkillList_codec);
			num += this.roundSkillList_.CalculateSize(HellGetPanelInfoResponse._repeated_roundSkillList_codec);
			num += this.stageSkillMap_.CalculateSize(HellGetPanelInfoResponse._map_stageSkillMap_codec);
			num += this.attrMap_.CalculateSize(HellGetPanelInfoResponse._map_attrMap_codec);
			num += this.monsterCfgId_.CalculateSize(HellGetPanelInfoResponse._repeated_monsterCfgId_codec);
			if (this.Hp != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Hp);
			}
			if (this.RevertCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.RevertCount);
			}
			if (this.WaveRate != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.WaveRate);
			}
			if (this.EventId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.EventId);
			}
			num += this.allSkillList_.CalculateSize(HellGetPanelInfoResponse._repeated_allSkillList_codec);
			if (this.IsEnterSkill != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.IsEnterSkill);
			}
			if (this.IsRoundSkill != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.IsRoundSkill);
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
								this.CurStage = input.ReadUInt32();
								continue;
							}
							if (num == 32U)
							{
								this.BattleStatus = input.ReadUInt32();
								continue;
							}
							if (num == 40U)
							{
								this.StageSeed = input.ReadUInt32();
								continue;
							}
						}
					}
					else if (num <= 58U)
					{
						if (num == 48U)
						{
							this.RoundPassStage = input.ReadUInt32();
							continue;
						}
						if (num == 56U || num == 58U)
						{
							this.monsterSkillList_.AddEntriesFrom(input, HellGetPanelInfoResponse._repeated_monsterSkillList_codec);
							continue;
						}
					}
					else
					{
						if (num == 64U || num == 66U)
						{
							this.roundSkillList_.AddEntriesFrom(input, HellGetPanelInfoResponse._repeated_roundSkillList_codec);
							continue;
						}
						if (num == 74U)
						{
							this.stageSkillMap_.AddEntriesFrom(input, HellGetPanelInfoResponse._map_stageSkillMap_codec);
							continue;
						}
					}
				}
				else
				{
					if (num <= 104U)
					{
						if (num <= 88U)
						{
							if (num == 82U)
							{
								this.attrMap_.AddEntriesFrom(input, HellGetPanelInfoResponse._map_attrMap_codec);
								continue;
							}
							if (num != 88U)
							{
								goto IL_00FD;
							}
						}
						else if (num != 90U)
						{
							if (num == 96U)
							{
								this.Hp = input.ReadInt64();
								continue;
							}
							if (num != 104U)
							{
								goto IL_00FD;
							}
							this.RevertCount = input.ReadInt32();
							continue;
						}
						this.monsterCfgId_.AddEntriesFrom(input, HellGetPanelInfoResponse._repeated_monsterCfgId_codec);
						continue;
					}
					if (num <= 128U)
					{
						if (num == 112U)
						{
							this.WaveRate = input.ReadUInt32();
							continue;
						}
						if (num == 120U)
						{
							this.EventId = input.ReadUInt32();
							continue;
						}
						if (num != 128U)
						{
							goto IL_00FD;
						}
					}
					else if (num != 130U)
					{
						if (num == 136U)
						{
							this.IsEnterSkill = input.ReadUInt32();
							continue;
						}
						if (num != 144U)
						{
							goto IL_00FD;
						}
						this.IsRoundSkill = input.ReadUInt32();
						continue;
					}
					this.allSkillList_.AddEntriesFrom(input, HellGetPanelInfoResponse._repeated_allSkillList_codec);
					continue;
				}
				IL_00FD:
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<HellGetPanelInfoResponse> _parser = new MessageParser<HellGetPanelInfoResponse>(() => new HellGetPanelInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int CurStageFieldNumber = 3;

		private uint curStage_;

		public const int BattleStatusFieldNumber = 4;

		private uint battleStatus_;

		public const int StageSeedFieldNumber = 5;

		private uint stageSeed_;

		public const int RoundPassStageFieldNumber = 6;

		private uint roundPassStage_;

		public const int MonsterSkillListFieldNumber = 7;

		private static readonly FieldCodec<int> _repeated_monsterSkillList_codec = FieldCodec.ForInt32(58U);

		private readonly RepeatedField<int> monsterSkillList_ = new RepeatedField<int>();

		public const int RoundSkillListFieldNumber = 8;

		private static readonly FieldCodec<int> _repeated_roundSkillList_codec = FieldCodec.ForInt32(66U);

		private readonly RepeatedField<int> roundSkillList_ = new RepeatedField<int>();

		public const int StageSkillMapFieldNumber = 9;

		private static readonly MapField<int, SkillListDto>.Codec _map_stageSkillMap_codec = new MapField<int, SkillListDto>.Codec(FieldCodec.ForInt32(8U), FieldCodec.ForMessage<SkillListDto>(18U, SkillListDto.Parser), 74U);

		private readonly MapField<int, SkillListDto> stageSkillMap_ = new MapField<int, SkillListDto>();

		public const int AttrMapFieldNumber = 10;

		private static readonly MapField<string, int>.Codec _map_attrMap_codec = new MapField<string, int>.Codec(FieldCodec.ForString(10U), FieldCodec.ForInt32(16U), 82U);

		private readonly MapField<string, int> attrMap_ = new MapField<string, int>();

		public const int MonsterCfgIdFieldNumber = 11;

		private static readonly FieldCodec<int> _repeated_monsterCfgId_codec = FieldCodec.ForInt32(90U);

		private readonly RepeatedField<int> monsterCfgId_ = new RepeatedField<int>();

		public const int HpFieldNumber = 12;

		private long hp_;

		public const int RevertCountFieldNumber = 13;

		private int revertCount_;

		public const int WaveRateFieldNumber = 14;

		private uint waveRate_;

		public const int EventIdFieldNumber = 15;

		private uint eventId_;

		public const int AllSkillListFieldNumber = 16;

		private static readonly FieldCodec<int> _repeated_allSkillList_codec = FieldCodec.ForInt32(130U);

		private readonly RepeatedField<int> allSkillList_ = new RepeatedField<int>();

		public const int IsEnterSkillFieldNumber = 17;

		private uint isEnterSkill_;

		public const int IsRoundSkillFieldNumber = 18;

		private uint isRoundSkill_;
	}
}
