using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Battle
{
	public sealed class RChapterCombatReq : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RChapterCombatReq> Parser
		{
			get
			{
				return RChapterCombatReq._parser;
			}
		}

		[DebuggerNonUserCode]
		public int ChapterId
		{
			get
			{
				return this.chapterId_;
			}
			set
			{
				this.chapterId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int WaveIndex
		{
			get
			{
				return this.waveIndex_;
			}
			set
			{
				this.waveIndex_ = value;
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
		public RepeatedField<int> MonsterCfgId
		{
			get
			{
				return this.monsterCfgId_;
			}
		}

		[DebuggerNonUserCode]
		public ulong CurHp
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
		public int BattleTimes
		{
			get
			{
				return this.battleTimes_;
			}
			set
			{
				this.battleTimes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ChapterId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.ChapterId);
			}
			if (this.WaveIndex != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.WaveIndex);
			}
			if (this.userInfo_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.UserInfo);
			}
			if (this.Seed != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Seed);
			}
			this.monsterCfgId_.WriteTo(output, RChapterCombatReq._repeated_monsterCfgId_codec);
			if (this.CurHp != 0UL)
			{
				output.WriteRawTag(48);
				output.WriteUInt64(this.CurHp);
			}
			if (this.ReviveCount != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.ReviveCount);
			}
			if (this.BattleServerLogId.Length != 0)
			{
				output.WriteRawTag(66);
				output.WriteString(this.BattleServerLogId);
			}
			if (this.BattleServerLogData.Length != 0)
			{
				output.WriteRawTag(74);
				output.WriteString(this.BattleServerLogData);
			}
			if (this.ClientVersion.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.ClientVersion);
			}
			if (this.BattleTimes != 0)
			{
				output.WriteRawTag(88);
				output.WriteInt32(this.BattleTimes);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ChapterId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ChapterId);
			}
			if (this.WaveIndex != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.WaveIndex);
			}
			if (this.userInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserInfo);
			}
			if (this.Seed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seed);
			}
			num += this.monsterCfgId_.CalculateSize(RChapterCombatReq._repeated_monsterCfgId_codec);
			if (this.CurHp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.CurHp);
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
			if (this.BattleTimes != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.BattleTimes);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 42U)
				{
					if (num <= 26U)
					{
						if (num == 8U)
						{
							this.ChapterId = input.ReadInt32();
							continue;
						}
						if (num == 16U)
						{
							this.WaveIndex = input.ReadInt32();
							continue;
						}
						if (num == 26U)
						{
							if (this.userInfo_ == null)
							{
								this.userInfo_ = new BattleUserDto();
							}
							input.ReadMessage(this.userInfo_);
							continue;
						}
					}
					else
					{
						if (num == 32U)
						{
							this.Seed = input.ReadInt32();
							continue;
						}
						if (num == 40U || num == 42U)
						{
							this.monsterCfgId_.AddEntriesFrom(input, RChapterCombatReq._repeated_monsterCfgId_codec);
							continue;
						}
					}
				}
				else if (num <= 66U)
				{
					if (num == 48U)
					{
						this.CurHp = input.ReadUInt64();
						continue;
					}
					if (num == 56U)
					{
						this.ReviveCount = input.ReadInt32();
						continue;
					}
					if (num == 66U)
					{
						this.BattleServerLogId = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 74U)
					{
						this.BattleServerLogData = input.ReadString();
						continue;
					}
					if (num == 82U)
					{
						this.ClientVersion = input.ReadString();
						continue;
					}
					if (num == 88U)
					{
						this.BattleTimes = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<RChapterCombatReq> _parser = new MessageParser<RChapterCombatReq>(() => new RChapterCombatReq());

		public const int ChapterIdFieldNumber = 1;

		private int chapterId_;

		public const int WaveIndexFieldNumber = 2;

		private int waveIndex_;

		public const int UserInfoFieldNumber = 3;

		private BattleUserDto userInfo_;

		public const int SeedFieldNumber = 4;

		private int seed_;

		public const int MonsterCfgIdFieldNumber = 5;

		private static readonly FieldCodec<int> _repeated_monsterCfgId_codec = FieldCodec.ForInt32(42U);

		private readonly RepeatedField<int> monsterCfgId_ = new RepeatedField<int>();

		public const int CurHpFieldNumber = 6;

		private ulong curHp_;

		public const int ReviveCountFieldNumber = 7;

		private int reviveCount_;

		public const int BattleServerLogIdFieldNumber = 8;

		private string battleServerLogId_ = "";

		public const int BattleServerLogDataFieldNumber = 9;

		private string battleServerLogData_ = "";

		public const int ClientVersionFieldNumber = 10;

		private string clientVersion_ = "";

		public const int BattleTimesFieldNumber = 11;

		private int battleTimes_;
	}
}
