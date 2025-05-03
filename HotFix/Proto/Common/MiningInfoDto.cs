using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class MiningInfoDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MiningInfoDto> Parser
		{
			get
			{
				return MiningInfoDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Stage
		{
			get
			{
				return this.stage_;
			}
			set
			{
				this.stage_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int OpenKey
		{
			get
			{
				return this.openKey_;
			}
			set
			{
				this.openKey_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong AutoOpt
		{
			get
			{
				return this.autoOpt_;
			}
			set
			{
				this.autoOpt_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<GridDto> Grids
		{
			get
			{
				return this.grids_;
			}
		}

		[DebuggerNonUserCode]
		public int OpenTreasure
		{
			get
			{
				return this.openTreasure_;
			}
			set
			{
				this.openTreasure_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int TreasureResId
		{
			get
			{
				return this.treasureResId_;
			}
			set
			{
				this.treasureResId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> TreasureUpGradeInfo
		{
			get
			{
				return this.treasureUpGradeInfo_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Stage != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Stage);
			}
			if (this.OpenKey != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.OpenKey);
			}
			if (this.AutoOpt != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.AutoOpt);
			}
			this.grids_.WriteTo(output, MiningInfoDto._repeated_grids_codec);
			if (this.OpenTreasure != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.OpenTreasure);
			}
			if (this.TreasureResId != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.TreasureResId);
			}
			this.treasureUpGradeInfo_.WriteTo(output, MiningInfoDto._repeated_treasureUpGradeInfo_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Stage != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Stage);
			}
			if (this.OpenKey != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.OpenKey);
			}
			if (this.AutoOpt != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.AutoOpt);
			}
			num += this.grids_.CalculateSize(MiningInfoDto._repeated_grids_codec);
			if (this.OpenTreasure != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.OpenTreasure);
			}
			if (this.TreasureResId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TreasureResId);
			}
			return num + this.treasureUpGradeInfo_.CalculateSize(MiningInfoDto._repeated_treasureUpGradeInfo_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 34U)
				{
					if (num <= 16U)
					{
						if (num == 8U)
						{
							this.Stage = input.ReadInt32();
							continue;
						}
						if (num == 16U)
						{
							this.OpenKey = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.AutoOpt = input.ReadUInt64();
							continue;
						}
						if (num == 34U)
						{
							this.grids_.AddEntriesFrom(input, MiningInfoDto._repeated_grids_codec);
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 40U)
					{
						this.OpenTreasure = input.ReadInt32();
						continue;
					}
					if (num == 48U)
					{
						this.TreasureResId = input.ReadInt32();
						continue;
					}
				}
				else if (num == 56U || num == 58U)
				{
					this.treasureUpGradeInfo_.AddEntriesFrom(input, MiningInfoDto._repeated_treasureUpGradeInfo_codec);
					continue;
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<MiningInfoDto> _parser = new MessageParser<MiningInfoDto>(() => new MiningInfoDto());

		public const int StageFieldNumber = 1;

		private int stage_;

		public const int OpenKeyFieldNumber = 2;

		private int openKey_;

		public const int AutoOptFieldNumber = 3;

		private ulong autoOpt_;

		public const int GridsFieldNumber = 4;

		private static readonly FieldCodec<GridDto> _repeated_grids_codec = FieldCodec.ForMessage<GridDto>(34U, GridDto.Parser);

		private readonly RepeatedField<GridDto> grids_ = new RepeatedField<GridDto>();

		public const int OpenTreasureFieldNumber = 5;

		private int openTreasure_;

		public const int TreasureResIdFieldNumber = 6;

		private int treasureResId_;

		public const int TreasureUpGradeInfoFieldNumber = 7;

		private static readonly FieldCodec<int> _repeated_treasureUpGradeInfo_codec = FieldCodec.ForInt32(58U);

		private readonly RepeatedField<int> treasureUpGradeInfo_ = new RepeatedField<int>();
	}
}
