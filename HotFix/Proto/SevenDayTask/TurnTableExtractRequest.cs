using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.SevenDayTask
{
	public sealed class TurnTableExtractRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TurnTableExtractRequest> Parser
		{
			get
			{
				return TurnTableExtractRequest._parser;
			}
		}

		[DebuggerNonUserCode]
		public CommonParams CommonParams
		{
			get
			{
				return this.commonParams_;
			}
			set
			{
				this.commonParams_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ExtractNum
		{
			get
			{
				return this.extractNum_;
			}
			set
			{
				this.extractNum_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.CommonParams);
			}
			if (this.ExtractNum != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ExtractNum);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.ExtractNum != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ExtractNum);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U)
					{
						input.SkipLastField();
					}
					else
					{
						this.ExtractNum = input.ReadInt32();
					}
				}
				else
				{
					if (this.commonParams_ == null)
					{
						this.commonParams_ = new CommonParams();
					}
					input.ReadMessage(this.commonParams_);
				}
			}
		}

		private static readonly MessageParser<TurnTableExtractRequest> _parser = new MessageParser<TurnTableExtractRequest>(() => new TurnTableExtractRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ExtractNumFieldNumber = 2;

		private int extractNum_;
	}
}
