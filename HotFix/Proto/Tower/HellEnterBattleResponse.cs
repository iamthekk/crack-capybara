using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Tower
{
	public sealed class HellEnterBattleResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<HellEnterBattleResponse> Parser
		{
			get
			{
				return HellEnterBattleResponse._parser;
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
		public RepeatedField<int> MonsterCfgId
		{
			get
			{
				return this.monsterCfgId_;
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
		public MapField<string, int> AttrMap
		{
			get
			{
				return this.attrMap_;
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
			if (this.StageSeed != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.StageSeed);
			}
			this.monsterCfgId_.WriteTo(output, HellEnterBattleResponse._repeated_monsterCfgId_codec);
			this.monsterSkillList_.WriteTo(output, HellEnterBattleResponse._repeated_monsterSkillList_codec);
			this.attrMap_.WriteTo(output, HellEnterBattleResponse._map_attrMap_codec);
			if (this.WaveRate != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.WaveRate);
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
			if (this.StageSeed != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.StageSeed);
			}
			num += this.monsterCfgId_.CalculateSize(HellEnterBattleResponse._repeated_monsterCfgId_codec);
			num += this.monsterSkillList_.CalculateSize(HellEnterBattleResponse._repeated_monsterSkillList_codec);
			num += this.attrMap_.CalculateSize(HellEnterBattleResponse._map_attrMap_codec);
			if (this.WaveRate != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.WaveRate);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num > 40U)
				{
					if (num <= 48U)
					{
						if (num == 42U)
						{
							goto IL_00B0;
						}
						if (num != 48U)
						{
							goto IL_0057;
						}
					}
					else if (num != 50U)
					{
						if (num == 58U)
						{
							this.attrMap_.AddEntriesFrom(input, HellEnterBattleResponse._map_attrMap_codec);
							continue;
						}
						if (num != 64U)
						{
							goto IL_0057;
						}
						this.WaveRate = input.ReadUInt32();
						continue;
					}
					this.monsterSkillList_.AddEntriesFrom(input, HellEnterBattleResponse._repeated_monsterSkillList_codec);
					continue;
				}
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
						this.StageSeed = input.ReadUInt32();
						continue;
					}
					if (num == 40U)
					{
						goto IL_00B0;
					}
				}
				IL_0057:
				input.SkipLastField();
				continue;
				IL_00B0:
				this.monsterCfgId_.AddEntriesFrom(input, HellEnterBattleResponse._repeated_monsterCfgId_codec);
			}
		}

		private static readonly MessageParser<HellEnterBattleResponse> _parser = new MessageParser<HellEnterBattleResponse>(() => new HellEnterBattleResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int CurStageFieldNumber = 3;

		private uint curStage_;

		public const int StageSeedFieldNumber = 4;

		private uint stageSeed_;

		public const int MonsterCfgIdFieldNumber = 5;

		private static readonly FieldCodec<int> _repeated_monsterCfgId_codec = FieldCodec.ForInt32(42U);

		private readonly RepeatedField<int> monsterCfgId_ = new RepeatedField<int>();

		public const int MonsterSkillListFieldNumber = 6;

		private static readonly FieldCodec<int> _repeated_monsterSkillList_codec = FieldCodec.ForInt32(50U);

		private readonly RepeatedField<int> monsterSkillList_ = new RepeatedField<int>();

		public const int AttrMapFieldNumber = 7;

		private static readonly MapField<string, int>.Codec _map_attrMap_codec = new MapField<string, int>.Codec(FieldCodec.ForString(10U), FieldCodec.ForInt32(16U), 58U);

		private readonly MapField<string, int> attrMap_ = new MapField<string, int>();

		public const int WaveRateFieldNumber = 8;

		private uint waveRate_;
	}
}
