using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Pet
{
	public sealed class PetDrawResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PetDrawResponse> Parser
		{
			get
			{
				return PetDrawResponse._parser;
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
		public int Type
		{
			get
			{
				return this.type_;
			}
			set
			{
				this.type_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int FreeTimes
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
		public int FreeRemindTime
		{
			get
			{
				return this.freeRemindTime_;
			}
			set
			{
				this.freeRemindTime_ = value;
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
		public RepeatedField<PetDto> AddPet
		{
			get
			{
				return this.addPet_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<PetDto> ShowPet
		{
			get
			{
				return this.showPet_;
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
		public AdDataDto AdDataDto
		{
			get
			{
				return this.adDataDto_;
			}
			set
			{
				this.adDataDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public PetInfo PetInfo
		{
			get
			{
				return this.petInfo_;
			}
			set
			{
				this.petInfo_ = value;
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
			if (this.Type != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Type);
			}
			if (this.FreeTimes != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.FreeTimes);
			}
			if (this.FreeRemindTime != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.FreeRemindTime);
			}
			this.dayDrawTimes_.WriteTo(output, PetDrawResponse._map_dayDrawTimes_codec);
			this.addPet_.WriteTo(output, PetDrawResponse._repeated_addPet_codec);
			this.showPet_.WriteTo(output, PetDrawResponse._repeated_showPet_codec);
			if (this.commonData_ != null)
			{
				output.WriteRawTag(66);
				output.WriteMessage(this.CommonData);
			}
			if (this.Level != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.Level);
			}
			if (this.Exp != 0U)
			{
				output.WriteRawTag(80);
				output.WriteUInt32(this.Exp);
			}
			if (this.adDataDto_ != null)
			{
				output.WriteRawTag(90);
				output.WriteMessage(this.AdDataDto);
			}
			if (this.petInfo_ != null)
			{
				output.WriteRawTag(98);
				output.WriteMessage(this.PetInfo);
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
			if (this.Type != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Type);
			}
			if (this.FreeTimes != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.FreeTimes);
			}
			if (this.FreeRemindTime != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.FreeRemindTime);
			}
			num += this.dayDrawTimes_.CalculateSize(PetDrawResponse._map_dayDrawTimes_codec);
			num += this.addPet_.CalculateSize(PetDrawResponse._repeated_addPet_codec);
			num += this.showPet_.CalculateSize(PetDrawResponse._repeated_showPet_codec);
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			if (this.Level != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Level);
			}
			if (this.Exp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Exp);
			}
			if (this.adDataDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.AdDataDto);
			}
			if (this.petInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.PetInfo);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 50U)
				{
					if (num <= 24U)
					{
						if (num == 8U)
						{
							this.Code = input.ReadInt32();
							continue;
						}
						if (num == 16U)
						{
							this.Type = input.ReadInt32();
							continue;
						}
						if (num == 24U)
						{
							this.FreeTimes = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 32U)
						{
							this.FreeRemindTime = input.ReadInt32();
							continue;
						}
						if (num == 42U)
						{
							this.dayDrawTimes_.AddEntriesFrom(input, PetDrawResponse._map_dayDrawTimes_codec);
							continue;
						}
						if (num == 50U)
						{
							this.addPet_.AddEntriesFrom(input, PetDrawResponse._repeated_addPet_codec);
							continue;
						}
					}
				}
				else if (num <= 72U)
				{
					if (num == 58U)
					{
						this.showPet_.AddEntriesFrom(input, PetDrawResponse._repeated_showPet_codec);
						continue;
					}
					if (num == 66U)
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
					if (num == 72U)
					{
						this.Level = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 80U)
					{
						this.Exp = input.ReadUInt32();
						continue;
					}
					if (num == 90U)
					{
						if (this.adDataDto_ == null)
						{
							this.adDataDto_ = new AdDataDto();
						}
						input.ReadMessage(this.adDataDto_);
						continue;
					}
					if (num == 98U)
					{
						if (this.petInfo_ == null)
						{
							this.petInfo_ = new PetInfo();
						}
						input.ReadMessage(this.petInfo_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<PetDrawResponse> _parser = new MessageParser<PetDrawResponse>(() => new PetDrawResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int TypeFieldNumber = 2;

		private int type_;

		public const int FreeTimesFieldNumber = 3;

		private int freeTimes_;

		public const int FreeRemindTimeFieldNumber = 4;

		private int freeRemindTime_;

		public const int DayDrawTimesFieldNumber = 5;

		private static readonly MapField<uint, uint>.Codec _map_dayDrawTimes_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 42U);

		private readonly MapField<uint, uint> dayDrawTimes_ = new MapField<uint, uint>();

		public const int AddPetFieldNumber = 6;

		private static readonly FieldCodec<PetDto> _repeated_addPet_codec = FieldCodec.ForMessage<PetDto>(50U, PetDto.Parser);

		private readonly RepeatedField<PetDto> addPet_ = new RepeatedField<PetDto>();

		public const int ShowPetFieldNumber = 7;

		private static readonly FieldCodec<PetDto> _repeated_showPet_codec = FieldCodec.ForMessage<PetDto>(58U, PetDto.Parser);

		private readonly RepeatedField<PetDto> showPet_ = new RepeatedField<PetDto>();

		public const int CommonDataFieldNumber = 8;

		private CommonData commonData_;

		public const int LevelFieldNumber = 9;

		private uint level_;

		public const int ExpFieldNumber = 10;

		private uint exp_;

		public const int AdDataDtoFieldNumber = 11;

		private AdDataDto adDataDto_;

		public const int PetInfoFieldNumber = 12;

		private PetInfo petInfo_;
	}
}
