using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.ActTime
{
	public sealed class ActTimePayFreeRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ActTimePayFreeRewardResponse> Parser
		{
			get
			{
				return ActTimePayFreeRewardResponse._parser;
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
		public RepeatedField<Pay> PayData
		{
			get
			{
				return this.payData_;
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
			this.payData_.WriteTo(output, ActTimePayFreeRewardResponse._repeated_payData_codec);
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
			return num + this.payData_.CalculateSize(ActTimePayFreeRewardResponse._repeated_payData_codec);
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
							this.payData_.AddEntriesFrom(input, ActTimePayFreeRewardResponse._repeated_payData_codec);
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

		private static readonly MessageParser<ActTimePayFreeRewardResponse> _parser = new MessageParser<ActTimePayFreeRewardResponse>(() => new ActTimePayFreeRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int PayDataFieldNumber = 3;

		private static readonly FieldCodec<Pay> _repeated_payData_codec = FieldCodec.ForMessage<Pay>(26U, Pay.Parser);

		private readonly RepeatedField<Pay> payData_ = new RepeatedField<Pay>();
	}
}
