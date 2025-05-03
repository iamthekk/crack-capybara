using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Pet
{
	public sealed class PetTrainResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PetTrainResponse> Parser
		{
			get
			{
				return PetTrainResponse._parser;
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
		public PetDto PetDto
		{
			get
			{
				return this.petDto_;
			}
			set
			{
				this.petDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int TrainingLevel
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
		public int TrainingExp
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.petDto_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.PetDto);
			}
			if (this.TrainingLevel != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.TrainingLevel);
			}
			if (this.TrainingExp != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.TrainingExp);
			}
			if (this.commonData_ != null)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.CommonData);
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
			if (this.petDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.PetDto);
			}
			if (this.TrainingLevel != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TrainingLevel);
			}
			if (this.TrainingExp != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TrainingExp);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 18U)
					{
						if (this.petDto_ == null)
						{
							this.petDto_ = new PetDto();
						}
						input.ReadMessage(this.petDto_);
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.TrainingLevel = input.ReadInt32();
						continue;
					}
					if (num == 32U)
					{
						this.TrainingExp = input.ReadInt32();
						continue;
					}
					if (num == 42U)
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<PetTrainResponse> _parser = new MessageParser<PetTrainResponse>(() => new PetTrainResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int PetDtoFieldNumber = 2;

		private PetDto petDto_;

		public const int TrainingLevelFieldNumber = 3;

		private int trainingLevel_;

		public const int TrainingExpFieldNumber = 4;

		private int trainingExp_;

		public const int CommonDataFieldNumber = 5;

		private CommonData commonData_;
	}
}
