using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Mining
{
	public sealed class BounDrawRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<BounDrawRequest> Parser
		{
			get
			{
				return BounDrawRequest._parser;
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
		public int Rate
		{
			get
			{
				return this.rate_;
			}
			set
			{
				this.rate_ = value;
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
			if (this.Rate != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Rate);
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
			if (this.Rate != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Rate);
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
						this.Rate = input.ReadInt32();
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

		private static readonly MessageParser<BounDrawRequest> _parser = new MessageParser<BounDrawRequest>(() => new BounDrawRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int RateFieldNumber = 2;

		private int rate_;
	}
}
