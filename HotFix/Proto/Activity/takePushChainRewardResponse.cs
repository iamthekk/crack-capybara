using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Activity
{
	public sealed class takePushChainRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<takePushChainRewardResponse> Parser
		{
			get
			{
				return takePushChainRewardResponse._parser;
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
			if (this.commonData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonData);
			}
			this.chainActvDto_.WriteTo(output, takePushChainRewardResponse._repeated_chainActvDto_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			return num + this.chainActvDto_.CalculateSize(takePushChainRewardResponse._repeated_chainActvDto_codec);
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
							this.chainActvDto_.AddEntriesFrom(input, takePushChainRewardResponse._repeated_chainActvDto_codec);
						}
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
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<takePushChainRewardResponse> _parser = new MessageParser<takePushChainRewardResponse>(() => new takePushChainRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ChainActvDtoFieldNumber = 3;

		private static readonly FieldCodec<ChainActvDto> _repeated_chainActvDto_codec = FieldCodec.ForMessage<ChainActvDto>(26U, Proto.Common.ChainActvDto.Parser);

		private readonly RepeatedField<ChainActvDto> chainActvDto_ = new RepeatedField<ChainActvDto>();
	}
}
