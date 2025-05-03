using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Battle
{
	public sealed class RChapterCombatResp : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RChapterCombatResp> Parser
		{
			get
			{
				return RChapterCombatResp._parser;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.ChapterId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ChapterId);
			}
			if (this.WaveIndex != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.WaveIndex);
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
			this.startUnits_.WriteTo(output, RChapterCombatResp._repeated_startUnits_codec);
			this.endUnits_.WriteTo(output, RChapterCombatResp._repeated_endUnits_codec);
			if (this.Power != 0L)
			{
				output.WriteRawTag(72);
				output.WriteInt64(this.Power);
			}
			if (this.ReviveCount != 0)
			{
				output.WriteRawTag(80);
				output.WriteInt32(this.ReviveCount);
			}
			if (this.BattleServerLogId.Length != 0)
			{
				output.WriteRawTag(90);
				output.WriteString(this.BattleServerLogId);
			}
			if (this.BattleServerLogData.Length != 0)
			{
				output.WriteRawTag(98);
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
			if (this.ChapterId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ChapterId);
			}
			if (this.WaveIndex != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.WaveIndex);
			}
			if (this.Result != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Result);
			}
			if (this.Seed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seed);
			}
			num += this.startUnits_.CalculateSize(RChapterCombatResp._repeated_startUnits_codec);
			num += this.endUnits_.CalculateSize(RChapterCombatResp._repeated_endUnits_codec);
			if (this.Power != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Power);
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
							this.ChapterId = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.WaveIndex = input.ReadInt32();
							continue;
						}
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
				else if (num <= 72U)
				{
					if (num == 58U)
					{
						this.startUnits_.AddEntriesFrom(input, RChapterCombatResp._repeated_startUnits_codec);
						continue;
					}
					if (num == 66U)
					{
						this.endUnits_.AddEntriesFrom(input, RChapterCombatResp._repeated_endUnits_codec);
						continue;
					}
					if (num == 72U)
					{
						this.Power = input.ReadInt64();
						continue;
					}
				}
				else
				{
					if (num == 80U)
					{
						this.ReviveCount = input.ReadInt32();
						continue;
					}
					if (num == 90U)
					{
						this.BattleServerLogId = input.ReadString();
						continue;
					}
					if (num == 98U)
					{
						this.BattleServerLogData = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<RChapterCombatResp> _parser = new MessageParser<RChapterCombatResp>(() => new RChapterCombatResp());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int ChapterIdFieldNumber = 2;

		private int chapterId_;

		public const int WaveIndexFieldNumber = 3;

		private int waveIndex_;

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

		public const int ReviveCountFieldNumber = 10;

		private int reviveCount_;

		public const int BattleServerLogIdFieldNumber = 11;

		private string battleServerLogId_ = "";

		public const int BattleServerLogDataFieldNumber = 12;

		private string battleServerLogData_ = "";
	}
}
