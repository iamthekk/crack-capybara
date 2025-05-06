using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.ActTime
{
	public sealed class ActTimeRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ActTimeRewardResponse> Parser
		{
			get
			{
				return ActTimeRewardResponse._parser;
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
		public RepeatedField<Consume> ConsumeData
		{
			get
			{
				return this.consumeData_;
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
			this.consumeData_.WriteTo(output, ActTimeRewardResponse._repeated_consumeData_codec);
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
			return num + this.consumeData_.CalculateSize(ActTimeRewardResponse._repeated_consumeData_codec);
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
							this.consumeData_.AddEntriesFrom(input, ActTimeRewardResponse._repeated_consumeData_codec);
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

		private static readonly MessageParser<ActTimeRewardResponse> _parser = new MessageParser<ActTimeRewardResponse>(() => new ActTimeRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ConsumeDataFieldNumber = 3;

		private static readonly FieldCodec<Consume> _repeated_consumeData_codec = FieldCodec.ForMessage<Consume>(26U, Consume.Parser);

		private readonly RepeatedField<Consume> consumeData_ = new RepeatedField<Consume>();
	}
}
