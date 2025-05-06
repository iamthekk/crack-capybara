using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class PVPRecordDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PVPRecordDto> Parser
		{
			get
			{
				return PVPRecordDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public BattleUserDto OwnerUser
		{
			get
			{
				return this.ownerUser_;
			}
			set
			{
				this.ownerUser_ = value;
			}
		}

		[DebuggerNonUserCode]
		public BattleUserDto OtherUser
		{
			get
			{
				return this.otherUser_;
			}
			set
			{
				this.otherUser_ = value;
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
		public long ReportRowId
		{
			get
			{
				return this.reportRowId_;
			}
			set
			{
				this.reportRowId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long Time
		{
			get
			{
				return this.time_;
			}
			set
			{
				this.time_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int TargetChangeScore
		{
			get
			{
				return this.targetChangeScore_;
			}
			set
			{
				this.targetChangeScore_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int OwnerChangeScore
		{
			get
			{
				return this.ownerChangeScore_;
			}
			set
			{
				this.ownerChangeScore_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Dan
		{
			get
			{
				return this.dan_;
			}
			set
			{
				this.dan_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ownerUser_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.OwnerUser);
			}
			if (this.otherUser_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.OtherUser);
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
			this.startUnits_.WriteTo(output, PVPRecordDto._repeated_startUnits_codec);
			this.endUnits_.WriteTo(output, PVPRecordDto._repeated_endUnits_codec);
			if (this.ReportRowId != 0L)
			{
				output.WriteRawTag(56);
				output.WriteInt64(this.ReportRowId);
			}
			if (this.Time != 0L)
			{
				output.WriteRawTag(64);
				output.WriteInt64(this.Time);
			}
			if (this.TargetChangeScore != 0)
			{
				output.WriteRawTag(72);
				output.WriteInt32(this.TargetChangeScore);
			}
			if (this.OwnerChangeScore != 0)
			{
				output.WriteRawTag(80);
				output.WriteInt32(this.OwnerChangeScore);
			}
			if (this.Dan != 0)
			{
				output.WriteRawTag(88);
				output.WriteInt32(this.Dan);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ownerUser_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.OwnerUser);
			}
			if (this.otherUser_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.OtherUser);
			}
			if (this.Result != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Result);
			}
			if (this.Seed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seed);
			}
			num += this.startUnits_.CalculateSize(PVPRecordDto._repeated_startUnits_codec);
			num += this.endUnits_.CalculateSize(PVPRecordDto._repeated_endUnits_codec);
			if (this.ReportRowId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.ReportRowId);
			}
			if (this.Time != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Time);
			}
			if (this.TargetChangeScore != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TargetChangeScore);
			}
			if (this.OwnerChangeScore != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.OwnerChangeScore);
			}
			if (this.Dan != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Dan);
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
					if (num <= 18U)
					{
						if (num == 10U)
						{
							if (this.ownerUser_ == null)
							{
								this.ownerUser_ = new BattleUserDto();
							}
							input.ReadMessage(this.ownerUser_);
							continue;
						}
						if (num == 18U)
						{
							if (this.otherUser_ == null)
							{
								this.otherUser_ = new BattleUserDto();
							}
							input.ReadMessage(this.otherUser_);
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
							this.startUnits_.AddEntriesFrom(input, PVPRecordDto._repeated_startUnits_codec);
							continue;
						}
					}
				}
				else if (num <= 64U)
				{
					if (num == 50U)
					{
						this.endUnits_.AddEntriesFrom(input, PVPRecordDto._repeated_endUnits_codec);
						continue;
					}
					if (num == 56U)
					{
						this.ReportRowId = input.ReadInt64();
						continue;
					}
					if (num == 64U)
					{
						this.Time = input.ReadInt64();
						continue;
					}
				}
				else
				{
					if (num == 72U)
					{
						this.TargetChangeScore = input.ReadInt32();
						continue;
					}
					if (num == 80U)
					{
						this.OwnerChangeScore = input.ReadInt32();
						continue;
					}
					if (num == 88U)
					{
						this.Dan = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<PVPRecordDto> _parser = new MessageParser<PVPRecordDto>(() => new PVPRecordDto());

		public const int OwnerUserFieldNumber = 1;

		private BattleUserDto ownerUser_;

		public const int OtherUserFieldNumber = 2;

		private BattleUserDto otherUser_;

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

		public const int ReportRowIdFieldNumber = 7;

		private long reportRowId_;

		public const int TimeFieldNumber = 8;

		private long time_;

		public const int TargetChangeScoreFieldNumber = 9;

		private int targetChangeScore_;

		public const int OwnerChangeScoreFieldNumber = 10;

		private int ownerChangeScore_;

		public const int DanFieldNumber = 11;

		private int dan_;
	}
}
