using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Pet
{
	public sealed class PetResetResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PetResetResponse> Parser
		{
			get
			{
				return PetResetResponse._parser;
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
		public RepeatedField<PetDto> PetDto
		{
			get
			{
				return this.petDto_;
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
			this.petDto_.WriteTo(output, PetResetResponse._repeated_petDto_codec);
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
			num += this.petDto_.CalculateSize(PetResetResponse._repeated_petDto_codec);
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
						this.petDto_.AddEntriesFrom(input, PetResetResponse._repeated_petDto_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<PetResetResponse> _parser = new MessageParser<PetResetResponse>(() => new PetResetResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int PetDtoFieldNumber = 2;

		private static readonly FieldCodec<PetDto> _repeated_petDto_codec = FieldCodec.ForMessage<PetDto>(18U, Proto.Common.PetDto.Parser);

		private readonly RepeatedField<PetDto> petDto_ = new RepeatedField<PetDto>();

		public const int CommonDataFieldNumber = 3;

		private CommonData commonData_;
	}
}
