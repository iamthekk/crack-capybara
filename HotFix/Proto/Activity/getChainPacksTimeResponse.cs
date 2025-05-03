using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Activity
{
	public sealed class getChainPacksTimeResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<getChainPacksTimeResponse> Parser
		{
			get
			{
				return getChainPacksTimeResponse._parser;
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
		public RepeatedField<ChainActvDto> ChainActvDto
		{
			get
			{
				return this.chainActvDto_;
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
			this.chainActvDto_.WriteTo(output, getChainPacksTimeResponse._repeated_chainActvDto_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.chainActvDto_.CalculateSize(getChainPacksTimeResponse._repeated_chainActvDto_codec);
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
						this.chainActvDto_.AddEntriesFrom(input, getChainPacksTimeResponse._repeated_chainActvDto_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<getChainPacksTimeResponse> _parser = new MessageParser<getChainPacksTimeResponse>(() => new getChainPacksTimeResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int ChainActvDtoFieldNumber = 2;

		private static readonly FieldCodec<ChainActvDto> _repeated_chainActvDto_codec = FieldCodec.ForMessage<ChainActvDto>(18U, Proto.Common.ChainActvDto.Parser);

		private readonly RepeatedField<ChainActvDto> chainActvDto_ = new RepeatedField<ChainActvDto>();
	}
}
