using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Dungeon
{
	public sealed class StartDungeonResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<StartDungeonResponse> Parser
		{
			get
			{
				return StartDungeonResponse._parser;
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
		public int DungeonId
		{
			get
			{
				return this.dungeonId_;
			}
			set
			{
				this.dungeonId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int LevelId
		{
			get
			{
				return this.levelId_;
			}
			set
			{
				this.levelId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int OptionType
		{
			get
			{
				return this.optionType_;
			}
			set
			{
				this.optionType_ = value;
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
		public RepeatedField<DungeonInfo> DungeonInfo
		{
			get
			{
				return this.dungeonInfo_;
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
			if (this.DungeonId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.DungeonId);
			}
			if (this.LevelId != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.LevelId);
			}
			if (this.OptionType != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.OptionType);
			}
			if (this.Result != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.Result);
			}
			if (this.Seed != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.Seed);
			}
			this.startUnits_.WriteTo(output, StartDungeonResponse._repeated_startUnits_codec);
			this.endUnits_.WriteTo(output, StartDungeonResponse._repeated_endUnits_codec);
			this.dungeonInfo_.WriteTo(output, StartDungeonResponse._repeated_dungeonInfo_codec);
			if (this.userInfo_ != null)
			{
				output.WriteRawTag(90);
				output.WriteMessage(this.UserInfo);
			}
			if (this.BattleServerLogId.Length != 0)
			{
				output.WriteRawTag(98);
				output.WriteString(this.BattleServerLogId);
			}
			if (this.BattleServerLogData.Length != 0)
			{
				output.WriteRawTag(106);
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
			if (this.DungeonId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.DungeonId);
			}
			if (this.LevelId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.LevelId);
			}
			if (this.OptionType != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.OptionType);
			}
			if (this.Result != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Result);
			}
			if (this.Seed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seed);
			}
			num += this.startUnits_.CalculateSize(StartDungeonResponse._repeated_startUnits_codec);
			num += this.endUnits_.CalculateSize(StartDungeonResponse._repeated_endUnits_codec);
			num += this.dungeonInfo_.CalculateSize(StartDungeonResponse._repeated_dungeonInfo_codec);
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
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 48U)
				{
					if (num <= 24U)
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
						if (num == 24U)
						{
							this.DungeonId = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 32U)
						{
							this.LevelId = input.ReadInt32();
							continue;
						}
						if (num == 40U)
						{
							this.OptionType = input.ReadInt32();
							continue;
						}
						if (num == 48U)
						{
							this.Result = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 74U)
				{
					if (num == 56U)
					{
						this.Seed = input.ReadInt32();
						continue;
					}
					if (num == 66U)
					{
						this.startUnits_.AddEntriesFrom(input, StartDungeonResponse._repeated_startUnits_codec);
						continue;
					}
					if (num == 74U)
					{
						this.endUnits_.AddEntriesFrom(input, StartDungeonResponse._repeated_endUnits_codec);
						continue;
					}
				}
				else if (num <= 90U)
				{
					if (num == 82U)
					{
						this.dungeonInfo_.AddEntriesFrom(input, StartDungeonResponse._repeated_dungeonInfo_codec);
						continue;
					}
					if (num == 90U)
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
					if (num == 98U)
					{
						this.BattleServerLogId = input.ReadString();
						continue;
					}
					if (num == 106U)
					{
						this.BattleServerLogData = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<StartDungeonResponse> _parser = new MessageParser<StartDungeonResponse>(() => new StartDungeonResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int DungeonIdFieldNumber = 3;

		private int dungeonId_;

		public const int LevelIdFieldNumber = 4;

		private int levelId_;

		public const int OptionTypeFieldNumber = 5;

		private int optionType_;

		public const int ResultFieldNumber = 6;

		private uint result_;

		public const int SeedFieldNumber = 7;

		private int seed_;

		public const int StartUnitsFieldNumber = 8;

		private static readonly FieldCodec<CombatUnitDto> _repeated_startUnits_codec = FieldCodec.ForMessage<CombatUnitDto>(66U, CombatUnitDto.Parser);

		private readonly RepeatedField<CombatUnitDto> startUnits_ = new RepeatedField<CombatUnitDto>();

		public const int EndUnitsFieldNumber = 9;

		private static readonly FieldCodec<CombatUnitDto> _repeated_endUnits_codec = FieldCodec.ForMessage<CombatUnitDto>(74U, CombatUnitDto.Parser);

		private readonly RepeatedField<CombatUnitDto> endUnits_ = new RepeatedField<CombatUnitDto>();

		public const int DungeonInfoFieldNumber = 10;

		private static readonly FieldCodec<DungeonInfo> _repeated_dungeonInfo_codec = FieldCodec.ForMessage<DungeonInfo>(82U, Proto.Common.DungeonInfo.Parser);

		private readonly RepeatedField<DungeonInfo> dungeonInfo_ = new RepeatedField<DungeonInfo>();

		public const int UserInfoFieldNumber = 11;

		private BattleUserDto userInfo_;

		public const int BattleServerLogIdFieldNumber = 12;

		private string battleServerLogId_ = "";

		public const int BattleServerLogDataFieldNumber = 13;

		private string battleServerLogData_ = "";
	}
}
