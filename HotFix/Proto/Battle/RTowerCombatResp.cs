using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Battle
{
	public sealed class RTowerCombatResp : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RTowerCombatResp> Parser
		{
			get
			{
				return RTowerCombatResp._parser;
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
		public int Tower
		{
			get
			{
				return this.tower_;
			}
			set
			{
				this.tower_ = value;
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
			if (this.Tower != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Tower);
			}
			if (this.Result != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Result);
			}
			if (this.Seed != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.Seed);
			}
			this.startUnits_.WriteTo(output, RTowerCombatResp._repeated_startUnits_codec);
			this.endUnits_.WriteTo(output, RTowerCombatResp._repeated_endUnits_codec);
			if (this.Power != 0L)
			{
				output.WriteRawTag(72);
				output.WriteInt64(this.Power);
			}
			if (this.BattleServerLogId.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.BattleServerLogId);
			}
			if (this.BattleServerLogData.Length != 0)
			{
				output.WriteRawTag(90);
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
			if (this.Tower != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Tower);
			}
			if (this.Result != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Result);
			}
			if (this.Seed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seed);
			}
			num += this.startUnits_.CalculateSize(RTowerCombatResp._repeated_startUnits_codec);
			num += this.endUnits_.CalculateSize(RTowerCombatResp._repeated_endUnits_codec);
			if (this.Power != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Power);
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
					if (num <= 16U)
					{
						if (num == 8U)
						{
							this.Code = input.ReadInt32();
							continue;
						}
						if (num == 16U)
						{
							this.Tower = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 32U)
						{
							this.Result = input.ReadInt32();
							continue;
						}
						if (num == 48U)
						{
							this.Seed = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 66U)
				{
					if (num == 58U)
					{
						this.startUnits_.AddEntriesFrom(input, RTowerCombatResp._repeated_startUnits_codec);
						continue;
					}
					if (num == 66U)
					{
						this.endUnits_.AddEntriesFrom(input, RTowerCombatResp._repeated_endUnits_codec);
						continue;
					}
				}
				else
				{
					if (num == 72U)
					{
						this.Power = input.ReadInt64();
						continue;
					}
					if (num == 82U)
					{
						this.BattleServerLogId = input.ReadString();
						continue;
					}
					if (num == 90U)
					{
						this.BattleServerLogData = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<RTowerCombatResp> _parser = new MessageParser<RTowerCombatResp>(() => new RTowerCombatResp());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int TowerFieldNumber = 2;

		private int tower_;

		public const int ResultFieldNumber = 4;

		private int result_;

		public const int SeedFieldNumber = 6;

		private int seed_;

		public const int StartUnitsFieldNumber = 7;

		private static readonly FieldCodec<CombatUnitDto> _repeated_startUnits_codec = FieldCodec.ForMessage<CombatUnitDto>(58U, CombatUnitDto.Parser);

		private readonly RepeatedField<CombatUnitDto> startUnits_ = new RepeatedField<CombatUnitDto>();

		public const int EndUnitsFieldNumber = 8;

		private static readonly FieldCodec<CombatUnitDto> _repeated_endUnits_codec = FieldCodec.ForMessage<CombatUnitDto>(66U, CombatUnitDto.Parser);

		private readonly RepeatedField<CombatUnitDto> endUnits_ = new RepeatedField<CombatUnitDto>();

		public const int PowerFieldNumber = 9;

		private long power_;

		public const int BattleServerLogIdFieldNumber = 10;

		private string battleServerLogId_ = "";

		public const int BattleServerLogDataFieldNumber = 11;

		private string battleServerLogData_ = "";
	}
}
