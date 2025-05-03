using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class PetInfo : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PetInfo> Parser
		{
			get
			{
				return PetInfo._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint FreeTimes
		{
			get
			{
				return this.freeTimes_;
			}
			set
			{
				this.freeTimes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, uint> DayDrawTimes
		{
			get
			{
				return this.dayDrawTimes_;
			}
		}

		[DebuggerNonUserCode]
		public ulong DayResetTimeStamp
		{
			get
			{
				return this.dayResetTimeStamp_;
			}
			set
			{
				this.dayResetTimeStamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong FreeTimeRemind
		{
			get
			{
				return this.freeTimeRemind_;
			}
			set
			{
				this.freeTimeRemind_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<PetDto> Pets
		{
			get
			{
				return this.pets_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<uint> Fetters
		{
			get
			{
				return this.fetters_;
			}
		}

		[DebuggerNonUserCode]
		public uint Level
		{
			get
			{
				return this.level_;
			}
			set
			{
				this.level_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Exp
		{
			get
			{
				return this.exp_;
			}
			set
			{
				this.exp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint AdvertDrawTimes
		{
			get
			{
				return this.advertDrawTimes_;
			}
			set
			{
				this.advertDrawTimes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TrainingLevel
		{
			get
			{
				return this.trainingLevel_;
			}
			set
			{
				this.trainingLevel_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TrainingExp
		{
			get
			{
				return this.trainingExp_;
			}
			set
			{
				this.trainingExp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.FreeTimes != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.FreeTimes);
			}
			this.dayDrawTimes_.WriteTo(output, PetInfo._map_dayDrawTimes_codec);
			if (this.DayResetTimeStamp != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.DayResetTimeStamp);
			}
			if (this.FreeTimeRemind != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.FreeTimeRemind);
			}
			this.pets_.WriteTo(output, PetInfo._repeated_pets_codec);
			this.fetters_.WriteTo(output, PetInfo._repeated_fetters_codec);
			if (this.Level != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.Level);
			}
			if (this.Exp != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.Exp);
			}
			if (this.AdvertDrawTimes != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.AdvertDrawTimes);
			}
			if (this.TrainingLevel != 0U)
			{
				output.WriteRawTag(80);
				output.WriteUInt32(this.TrainingLevel);
			}
			if (this.TrainingExp != 0U)
			{
				output.WriteRawTag(88);
				output.WriteUInt32(this.TrainingExp);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.FreeTimes != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.FreeTimes);
			}
			num += this.dayDrawTimes_.CalculateSize(PetInfo._map_dayDrawTimes_codec);
			if (this.DayResetTimeStamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.DayResetTimeStamp);
			}
			if (this.FreeTimeRemind != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.FreeTimeRemind);
			}
			num += this.pets_.CalculateSize(PetInfo._repeated_pets_codec);
			num += this.fetters_.CalculateSize(PetInfo._repeated_fetters_codec);
			if (this.Level != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Level);
			}
			if (this.Exp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Exp);
			}
			if (this.AdvertDrawTimes != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.AdvertDrawTimes);
			}
			if (this.TrainingLevel != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TrainingLevel);
			}
			if (this.TrainingExp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TrainingExp);
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
							this.FreeTimes = input.ReadUInt32();
							continue;
						}
						if (num == 18U)
						{
							this.dayDrawTimes_.AddEntriesFrom(input, PetInfo._map_dayDrawTimes_codec);
							continue;
						}
						if (num == 24U)
						{
							this.DayResetTimeStamp = input.ReadUInt64();
							continue;
						}
					}
					else
					{
						if (num == 32U)
						{
							this.FreeTimeRemind = input.ReadUInt64();
							continue;
						}
						if (num == 42U)
						{
							this.pets_.AddEntriesFrom(input, PetInfo._repeated_pets_codec);
							continue;
						}
						if (num == 48U)
						{
							goto IL_00D4;
						}
					}
				}
				else if (num <= 64U)
				{
					if (num == 50U)
					{
						goto IL_00D4;
					}
					if (num == 56U)
					{
						this.Level = input.ReadUInt32();
						continue;
					}
					if (num == 64U)
					{
						this.Exp = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 72U)
					{
						this.AdvertDrawTimes = input.ReadUInt32();
						continue;
					}
					if (num == 80U)
					{
						this.TrainingLevel = input.ReadUInt32();
						continue;
					}
					if (num == 88U)
					{
						this.TrainingExp = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
				continue;
				IL_00D4:
				this.fetters_.AddEntriesFrom(input, PetInfo._repeated_fetters_codec);
			}
		}

		private static readonly MessageParser<PetInfo> _parser = new MessageParser<PetInfo>(() => new PetInfo());

		public const int FreeTimesFieldNumber = 1;

		private uint freeTimes_;

		public const int DayDrawTimesFieldNumber = 2;

		private static readonly MapField<uint, uint>.Codec _map_dayDrawTimes_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 18U);

		private readonly MapField<uint, uint> dayDrawTimes_ = new MapField<uint, uint>();

		public const int DayResetTimeStampFieldNumber = 3;

		private ulong dayResetTimeStamp_;

		public const int FreeTimeRemindFieldNumber = 4;

		private ulong freeTimeRemind_;

		public const int PetsFieldNumber = 5;

		private static readonly FieldCodec<PetDto> _repeated_pets_codec = FieldCodec.ForMessage<PetDto>(42U, PetDto.Parser);

		private readonly RepeatedField<PetDto> pets_ = new RepeatedField<PetDto>();

		public const int FettersFieldNumber = 6;

		private static readonly FieldCodec<uint> _repeated_fetters_codec = FieldCodec.ForUInt32(50U);

		private readonly RepeatedField<uint> fetters_ = new RepeatedField<uint>();

		public const int LevelFieldNumber = 7;

		private uint level_;

		public const int ExpFieldNumber = 8;

		private uint exp_;

		public const int AdvertDrawTimesFieldNumber = 9;

		private uint advertDrawTimes_;

		public const int TrainingLevelFieldNumber = 10;

		private uint trainingLevel_;

		public const int TrainingExpFieldNumber = 11;

		private uint trainingExp_;
	}
}
