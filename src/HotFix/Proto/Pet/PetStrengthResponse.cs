using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Pet
{
	public sealed class PetStrengthResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PetStrengthResponse> Parser
		{
			get
			{
				return PetStrengthResponse._parser;
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
			if (this.commonData_ != null)
			{
				output.WriteRawTag(26);
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
				if (num != 8U)
				{
					if (num != 18U)
					{
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							if (this.commonData_ == null)
							{
								this.commonData_ = new CommonData();
							}
							input.ReadMessage(this.commonData_);
						}
					}
					else
					{
						if (this.petDto_ == null)
						{
							this.petDto_ = new PetDto();
						}
						input.ReadMessage(this.petDto_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<PetStrengthResponse> _parser = new MessageParser<PetStrengthResponse>(() => new PetStrengthResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int PetDtoFieldNumber = 2;

		private PetDto petDto_;

		public const int CommonDataFieldNumber = 3;

		private CommonData commonData_;
	}
}
