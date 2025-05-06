using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Mission
{
	public sealed class EndWorldBossResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<EndWorldBossResponse> Parser
		{
			get
			{
				return EndWorldBossResponse._parser;
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
		public WorldBossDto WorldBossInfo
		{
			get
			{
				return this.worldBossInfo_;
			}
			set
			{
				this.worldBossInfo_ = value;
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
			this.startUnits_.WriteTo(output, EndWorldBossResponse._repeated_startUnits_codec);
			this.endUnits_.WriteTo(output, EndWorldBossResponse._repeated_endUnits_codec);
			if (this.userInfo_ != null)
			{
				output.WriteRawTag(90);
				output.WriteMessage(this.UserInfo);
			}
			if (this.worldBossInfo_ != null)
			{
				output.WriteRawTag(98);
				output.WriteMessage(this.WorldBossInfo);
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
			num += this.startUnits_.CalculateSize(EndWorldBossResponse._repeated_startUnits_codec);
			num += this.endUnits_.CalculateSize(EndWorldBossResponse._repeated_endUnits_codec);
			if (this.userInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserInfo);
			}
			if (this.worldBossInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.WorldBossInfo);
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
				if (num <= 56U)
				{
					if (num <= 32U)
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
						if (num == 32U)
						{
							this.Damage = input.ReadUInt64();
							continue;
						}
					}
					else
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
						if (num == 56U)
						{
							this.ConfigId = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 82U)
				{
					if (num == 64U)
					{
						this.Seed = input.ReadInt32();
						continue;
					}
					if (num == 74U)
					{
						this.startUnits_.AddEntriesFrom(input, EndWorldBossResponse._repeated_startUnits_codec);
						continue;
					}
					if (num == 82U)
					{
						this.endUnits_.AddEntriesFrom(input, EndWorldBossResponse._repeated_endUnits_codec);
						continue;
					}
				}
				else if (num <= 98U)
				{
					if (num == 90U)
					{
						if (this.userInfo_ == null)
						{
							this.userInfo_ = new BattleUserDto();
						}
						input.ReadMessage(this.userInfo_);
						continue;
					}
					if (num == 98U)
					{
						if (this.worldBossInfo_ == null)
						{
							this.worldBossInfo_ = new WorldBossDto();
						}
						input.ReadMessage(this.worldBossInfo_);
						continue;
					}
				}
				else
				{
					if (num == 106U)
					{
						this.BattleServerLogId = input.ReadString();
						continue;
					}
					if (num == 114U)
					{
						this.BattleServerLogData = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<EndWorldBossResponse> _parser = new MessageParser<EndWorldBossResponse>(() => new EndWorldBossResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

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

		public const int UserInfoFieldNumber = 11;

		private BattleUserDto userInfo_;

		public const int WorldBossInfoFieldNumber = 12;

		private WorldBossDto worldBossInfo_;

		public const int BattleServerLogIdFieldNumber = 13;

		private string battleServerLogId_ = "";

		public const int BattleServerLogDataFieldNumber = 14;

		private string battleServerLogData_ = "";
	}
}
