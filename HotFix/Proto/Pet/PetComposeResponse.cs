using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Pet
{
	public sealed class PetComposeResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PetComposeResponse> Parser
		{
			get
			{
				return PetComposeResponse._parser;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			this.petDto_.WriteTo(output, PetComposeResponse._repeated_petDto_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.petDto_.CalculateSize(PetComposeResponse._repeated_petDto_codec);
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
						input.SkipLastField();
					}
					else
					{
						this.petDto_.AddEntriesFrom(input, PetComposeResponse._repeated_petDto_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<PetComposeResponse> _parser = new MessageParser<PetComposeResponse>(() => new PetComposeResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int PetDtoFieldNumber = 2;

		private static readonly FieldCodec<PetDto> _repeated_petDto_codec = FieldCodec.ForMessage<PetDto>(18U, Proto.Common.PetDto.Parser);

		private readonly RepeatedField<PetDto> petDto_ = new RepeatedField<PetDto>();
	}
}
