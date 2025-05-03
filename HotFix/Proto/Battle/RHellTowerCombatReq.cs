using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Battle
{
	public sealed class RHellTowerCombatReq : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RHellTowerCombatReq> Parser
		{
			get
			{
				return RHellTowerCombatReq._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint StageId
		{
			get
			{
				return this.stageId_;
			}
			set
			{
				this.stageId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint PassStage
		{
			get
			{
				return this.passStage_;
			}
			set
			{
				this.passStage_ = value;
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
		public long CurHp
		{
			get
			{
				return this.curHp_;
			}
			set
			{
				this.curHp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ReviveCount
		{
			get
			{
				return this.reviveCount_;
			}
			set
			{
				this.reviveCount_ = value;
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
		public string ClientVersion
		{
			get
			{
				return this.clientVersion_;
			}
			set
			{
				this.clientVersion_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.StageId != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.StageId);
			}
			if (this.PassStage != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.PassStage);
			}
			this.monsterCfgId_.WriteTo(output, RHellTowerCombatReq._repeated_monsterCfgId_codec);
			this.monsterSkillList_.WriteTo(output, RHellTowerCombatReq._repeated_monsterSkillList_codec);
			if (this.userInfo_ != null)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.UserInfo);
			}
			if (this.Seed != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.Seed);
			}
			if (this.CurHp != 0L)
			{
				output.WriteRawTag(56);
				output.WriteInt64(this.CurHp);
			}
			if (this.ReviveCount != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.ReviveCount);
			}
			if (this.BattleServerLogId.Length != 0)
			{
				output.WriteRawTag(74);
				output.WriteString(this.BattleServerLogId);
			}
			if (this.BattleServerLogData.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.BattleServerLogData);
			}
			if (this.ClientVersion.Length != 0)
			{
				output.WriteRawTag(90);
				output.WriteString(this.ClientVersion);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.StageId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.StageId);
			}
			if (this.PassStage != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.PassStage);
			}
			num += this.monsterCfgId_.CalculateSize(RHellTowerCombatReq._repeated_monsterCfgId_codec);
			num += this.monsterSkillList_.CalculateSize(RHellTowerCombatReq._repeated_monsterSkillList_codec);
			if (this.userInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserInfo);
			}
			if (this.Seed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seed);
			}
			if (this.CurHp != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.CurHp);
			}
			if (this.ReviveCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ReviveCount);
			}
			if (this.BattleServerLogId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.BattleServerLogId);
			}
			if (this.BattleServerLogData.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.BattleServerLogData);
			}
			if (this.ClientVersion.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ClientVersion);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 34U)
				{
					if (num <= 24U)
					{
						if (num == 8U)
						{
							this.StageId = input.ReadUInt32();
							continue;
						}
						if (num == 16U)
						{
							this.PassStage = input.ReadUInt32();
							continue;
						}
						if (num != 24U)
						{
							goto IL_0085;
						}
					}
					else if (num != 26U)
					{
						if (num != 32U && num != 34U)
						{
							goto IL_0085;
						}
						this.monsterSkillList_.AddEntriesFrom(input, RHellTowerCombatReq._repeated_monsterSkillList_codec);
						continue;
					}
					this.monsterCfgId_.AddEntriesFrom(input, RHellTowerCombatReq._repeated_monsterCfgId_codec);
					continue;
				}
				if (num <= 56U)
				{
					if (num == 42U)
					{
						if (this.userInfo_ == null)
						{
							this.userInfo_ = new BattleUserDto();
						}
						input.ReadMessage(this.userInfo_);
						continue;
					}
					if (num == 48U)
					{
						this.Seed = input.ReadInt32();
						continue;
					}
					if (num == 56U)
					{
						this.CurHp = input.ReadInt64();
						continue;
					}
				}
				else if (num <= 74U)
				{
					if (num == 64U)
					{
						this.ReviveCount = input.ReadInt32();
						continue;
					}
					if (num == 74U)
					{
						this.BattleServerLogId = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 82U)
					{
						this.BattleServerLogData = input.ReadString();
						continue;
					}
					if (num == 90U)
					{
						this.ClientVersion = input.ReadString();
						continue;
					}
				}
				IL_0085:
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<RHellTowerCombatReq> _parser = new MessageParser<RHellTowerCombatReq>(() => new RHellTowerCombatReq());

		public const int StageIdFieldNumber = 1;

		private uint stageId_;

		public const int PassStageFieldNumber = 2;

		private uint passStage_;

		public const int MonsterCfgIdFieldNumber = 3;

		private static readonly FieldCodec<int> _repeated_monsterCfgId_codec = FieldCodec.ForInt32(26U);

		private readonly RepeatedField<int> monsterCfgId_ = new RepeatedField<int>();

		public const int MonsterSkillListFieldNumber = 4;

		private static readonly FieldCodec<int> _repeated_monsterSkillList_codec = FieldCodec.ForInt32(34U);

		private readonly RepeatedField<int> monsterSkillList_ = new RepeatedField<int>();

		public const int UserInfoFieldNumber = 5;

		private BattleUserDto userInfo_;

		public const int SeedFieldNumber = 6;

		private int seed_;

		public const int CurHpFieldNumber = 7;

		private long curHp_;

		public const int ReviveCountFieldNumber = 8;

		private int reviveCount_;

		public const int BattleServerLogIdFieldNumber = 9;

		private string battleServerLogId_ = "";

		public const int BattleServerLogDataFieldNumber = 10;

		private string battleServerLogData_ = "";

		public const int ClientVersionFieldNumber = 11;

		private string clientVersion_ = "";
	}
}
