using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Pet
{
	public sealed class PetFormationResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PetFormationResponse> Parser
		{
			get
			{
				return PetFormationResponse._parser;
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
		public RepeatedField<PetDto> PetDtos
		{
			get
			{
				return this.petDtos_;
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
			this.petDtos_.WriteTo(output, PetFormationResponse._repeated_petDtos_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.petDtos_.CalculateSize(PetFormationResponse._repeated_petDtos_codec);
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
						this.petDtos_.AddEntriesFrom(input, PetFormationResponse._repeated_petDtos_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<PetFormationResponse> _parser = new MessageParser<PetFormationResponse>(() => new PetFormationResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int PetDtosFieldNumber = 2;

		private static readonly FieldCodec<PetDto> _repeated_petDtos_codec = FieldCodec.ForMessage<PetDto>(18U, PetDto.Parser);

		private readonly RepeatedField<PetDto> petDtos_ = new RepeatedField<PetDto>();
	}
}
