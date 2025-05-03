using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class PetDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PetDto> Parser
		{
			get
			{
				return PetDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public ulong RowId
		{
			get
			{
				return this.rowId_;
			}
			set
			{
				this.rowId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ConfigId
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
		public uint PetType
		{
			get
			{
				return this.petType_;
			}
			set
			{
				this.petType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint PetCount
		{
			get
			{
				return this.petCount_;
			}
			set
			{
				this.petCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint PetLv
		{
			get
			{
				return this.petLv_;
			}
			set
			{
				this.petLv_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint FormationPos
		{
			get
			{
				return this.formationPos_;
			}
			set
			{
				this.formationPos_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint IsShow
		{
			get
			{
				return this.isShow_;
			}
			set
			{
				this.isShow_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong CreateTime
		{
			get
			{
				return this.createTime_;
			}
			set
			{
				this.createTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> TrainingAttributeIds
		{
			get
			{
				return this.trainingAttributeIds_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> TrainingAttributeValues
		{
			get
			{
				return this.trainingAttributeValues_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> TrainingAttributeIdsTemp
		{
			get
			{
				return this.trainingAttributeIdsTemp_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> TrainingAttributeValuesTemp
		{
			get
			{
				return this.trainingAttributeValuesTemp_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> TrainLock
		{
			get
			{
				return this.trainLock_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RowId != 0UL)
			{
				output.WriteRawTag(8);
				output.WriteUInt64(this.RowId);
			}
			if (this.ConfigId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.ConfigId);
			}
			if (this.PetType != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.PetType);
			}
			if (this.PetCount != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.PetCount);
			}
			if (this.PetLv != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.PetLv);
			}
			if (this.FormationPos != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.FormationPos);
			}
			if (this.IsShow != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.IsShow);
			}
			if (this.CreateTime != 0UL)
			{
				output.WriteRawTag(72);
				output.WriteUInt64(this.CreateTime);
			}
			this.trainingAttributeIds_.WriteTo(output, PetDto._repeated_trainingAttributeIds_codec);
			this.trainingAttributeValues_.WriteTo(output, PetDto._repeated_trainingAttributeValues_codec);
			this.trainingAttributeIdsTemp_.WriteTo(output, PetDto._repeated_trainingAttributeIdsTemp_codec);
			this.trainingAttributeValuesTemp_.WriteTo(output, PetDto._repeated_trainingAttributeValuesTemp_codec);
			this.trainLock_.WriteTo(output, PetDto._repeated_trainLock_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RowId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RowId);
			}
			if (this.ConfigId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ConfigId);
			}
			if (this.PetType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.PetType);
			}
			if (this.PetCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.PetCount);
			}
			if (this.PetLv != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.PetLv);
			}
			if (this.FormationPos != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.FormationPos);
			}
			if (this.IsShow != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.IsShow);
			}
			if (this.CreateTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.CreateTime);
			}
			num += this.trainingAttributeIds_.CalculateSize(PetDto._repeated_trainingAttributeIds_codec);
			num += this.trainingAttributeValues_.CalculateSize(PetDto._repeated_trainingAttributeValues_codec);
			num += this.trainingAttributeIdsTemp_.CalculateSize(PetDto._repeated_trainingAttributeIdsTemp_codec);
			num += this.trainingAttributeValuesTemp_.CalculateSize(PetDto._repeated_trainingAttributeValuesTemp_codec);
			return num + this.trainLock_.CalculateSize(PetDto._repeated_trainLock_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num > 80U)
				{
					if (num <= 96U)
					{
						if (num <= 88U)
						{
							if (num == 82U)
							{
								goto IL_0155;
							}
							if (num != 88U)
							{
								goto IL_00CB;
							}
						}
						else if (num != 90U)
						{
							if (num != 96U)
							{
								goto IL_00CB;
							}
							goto IL_017B;
						}
						this.trainingAttributeValues_.AddEntriesFrom(input, PetDto._repeated_trainingAttributeValues_codec);
						continue;
					}
					if (num <= 104U)
					{
						if (num == 98U)
						{
							goto IL_017B;
						}
						if (num != 104U)
						{
							goto IL_00CB;
						}
					}
					else if (num != 106U)
					{
						if (num != 112U && num != 114U)
						{
							goto IL_00CB;
						}
						this.trainLock_.AddEntriesFrom(input, PetDto._repeated_trainLock_codec);
						continue;
					}
					this.trainingAttributeValuesTemp_.AddEntriesFrom(input, PetDto._repeated_trainingAttributeValuesTemp_codec);
					continue;
					IL_017B:
					this.trainingAttributeIdsTemp_.AddEntriesFrom(input, PetDto._repeated_trainingAttributeIdsTemp_codec);
					continue;
				}
				if (num <= 32U)
				{
					if (num <= 16U)
					{
						if (num == 8U)
						{
							this.RowId = input.ReadUInt64();
							continue;
						}
						if (num == 16U)
						{
							this.ConfigId = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.PetType = input.ReadUInt32();
							continue;
						}
						if (num == 32U)
						{
							this.PetCount = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 56U)
				{
					if (num == 40U)
					{
						this.PetLv = input.ReadUInt32();
						continue;
					}
					if (num == 56U)
					{
						this.FormationPos = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 64U)
					{
						this.IsShow = input.ReadUInt32();
						continue;
					}
					if (num == 72U)
					{
						this.CreateTime = input.ReadUInt64();
						continue;
					}
					if (num == 80U)
					{
						goto IL_0155;
					}
				}
				IL_00CB:
				input.SkipLastField();
				continue;
				IL_0155:
				this.trainingAttributeIds_.AddEntriesFrom(input, PetDto._repeated_trainingAttributeIds_codec);
			}
		}

		private static readonly MessageParser<PetDto> _parser = new MessageParser<PetDto>(() => new PetDto());

		public const int RowIdFieldNumber = 1;

		private ulong rowId_;

		public const int ConfigIdFieldNumber = 2;

		private uint configId_;

		public const int PetTypeFieldNumber = 3;

		private uint petType_;

		public const int PetCountFieldNumber = 4;

		private uint petCount_;

		public const int PetLvFieldNumber = 5;

		private uint petLv_;

		public const int FormationPosFieldNumber = 7;

		private uint formationPos_;

		public const int IsShowFieldNumber = 8;

		private uint isShow_;

		public const int CreateTimeFieldNumber = 9;

		private ulong createTime_;

		public const int TrainingAttributeIdsFieldNumber = 10;

		private static readonly FieldCodec<int> _repeated_trainingAttributeIds_codec = FieldCodec.ForInt32(82U);

		private readonly RepeatedField<int> trainingAttributeIds_ = new RepeatedField<int>();

		public const int TrainingAttributeValuesFieldNumber = 11;

		private static readonly FieldCodec<int> _repeated_trainingAttributeValues_codec = FieldCodec.ForInt32(90U);

		private readonly RepeatedField<int> trainingAttributeValues_ = new RepeatedField<int>();

		public const int TrainingAttributeIdsTempFieldNumber = 12;

		private static readonly FieldCodec<int> _repeated_trainingAttributeIdsTemp_codec = FieldCodec.ForInt32(98U);

		private readonly RepeatedField<int> trainingAttributeIdsTemp_ = new RepeatedField<int>();

		public const int TrainingAttributeValuesTempFieldNumber = 13;

		private static readonly FieldCodec<int> _repeated_trainingAttributeValuesTemp_codec = FieldCodec.ForInt32(106U);

		private readonly RepeatedField<int> trainingAttributeValuesTemp_ = new RepeatedField<int>();

		public const int TrainLockFieldNumber = 14;

		private static readonly FieldCodec<int> _repeated_trainLock_codec = FieldCodec.ForInt32(114U);

		private readonly RepeatedField<int> trainLock_ = new RepeatedField<int>();
	}
}
