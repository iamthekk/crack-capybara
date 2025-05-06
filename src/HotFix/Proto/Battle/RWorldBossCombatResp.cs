using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Battle
{
	public sealed class RWorldBossCombatResp : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RWorldBossCombatResp> Parser
		{
			get
			{
				return RWorldBossCombatResp._parser;
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
		public long Power
		{
			get
			{
				return this.power_;
			}
			set
			{
				this.power_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long Hurt
		{
			get
			{
				return this.hurt_;
			}
			set
			{
				this.hurt_ = value;
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
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ConfigId);
			}
			if (this.Result != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Result);
			}
			if (this.Seed != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Seed);
			}
			this.startUnits_.WriteTo(output, RWorldBossCombatResp._repeated_startUnits_codec);
			this.endUnits_.WriteTo(output, RWorldBossCombatResp._repeated_endUnits_codec);
			if (this.Power != 0L)
			{
				output.WriteRawTag(56);
				output.WriteInt64(this.Power);
			}
			if (this.Hurt != 0L)
			{
				output.WriteRawTag(64);
				output.WriteInt64(this.Hurt);
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
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.ConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ConfigId);
			}
			if (this.Result != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Result);
			}
			if (this.Seed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seed);
			}
			num += this.startUnits_.CalculateSize(RWorldBossCombatResp._repeated_startUnits_codec);
			num += this.endUnits_.CalculateSize(RWorldBossCombatResp._repeated_endUnits_codec);
			if (this.Power != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Power);
			}
			if (this.Hurt != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Hurt);
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
				if (num <= 42U)
				{
					if (num <= 16U)
					{
						if (num == 8U)
						{
							this.Code = input.ReadInt32();
							continue;
						}
						if (num == 16U)
						{
							this.ConfigId = input.ReadInt32();
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
							this.Seed = input.ReadInt32();
							continue;
						}
						if (num == 42U)
						{
							this.startUnits_.AddEntriesFrom(input, RWorldBossCombatResp._repeated_startUnits_codec);
							continue;
						}
					}
				}
				else if (num <= 56U)
				{
					if (num == 50U)
					{
						this.endUnits_.AddEntriesFrom(input, RWorldBossCombatResp._repeated_endUnits_codec);
						continue;
					}
					if (num == 56U)
					{
						this.Power = input.ReadInt64();
						continue;
					}
				}
				else
				{
					if (num == 64U)
					{
						this.Hurt = input.ReadInt64();
						continue;
					}
					if (num == 74U)
					{
						this.BattleServerLogId = input.ReadString();
						continue;
					}
					if (num == 82U)
					{
						this.BattleServerLogData = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<RWorldBossCombatResp> _parser = new MessageParser<RWorldBossCombatResp>(() => new RWorldBossCombatResp());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int ConfigIdFieldNumber = 2;

		private int configId_;

		public const int ResultFieldNumber = 3;

		private int result_;

		public const int SeedFieldNumber = 4;

		private int seed_;

		public const int StartUnitsFieldNumber = 5;

		private static readonly FieldCodec<CombatUnitDto> _repeated_startUnits_codec = FieldCodec.ForMessage<CombatUnitDto>(42U, CombatUnitDto.Parser);

		private readonly RepeatedField<CombatUnitDto> startUnits_ = new RepeatedField<CombatUnitDto>();

		public const int EndUnitsFieldNumber = 6;

		private static readonly FieldCodec<CombatUnitDto> _repeated_endUnits_codec = FieldCodec.ForMessage<CombatUnitDto>(50U, CombatUnitDto.Parser);

		private readonly RepeatedField<CombatUnitDto> endUnits_ = new RepeatedField<CombatUnitDto>();

		public const int PowerFieldNumber = 7;

		private long power_;

		public const int HurtFieldNumber = 8;

		private long hurt_;

		public const int BattleServerLogIdFieldNumber = 9;

		private string battleServerLogId_ = "";

		public const int BattleServerLogDataFieldNumber = 10;

		private string battleServerLogData_ = "";
	}
}
