using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Mining
{
	public sealed class SetMiningOptionRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SetMiningOptionRequest> Parser
		{
			get
			{
				return SetMiningOptionRequest._parser;
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
		public ulong AutoOpt
		{
			get
			{
				return this.autoOpt_;
			}
			set
			{
				this.autoOpt_ = value;
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
			if (this.AutoOpt != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.AutoOpt);
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
			if (this.AutoOpt != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.AutoOpt);
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
						this.AutoOpt = input.ReadUInt64();
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

		private static readonly MessageParser<SetMiningOptionRequest> _parser = new MessageParser<SetMiningOptionRequest>(() => new SetMiningOptionRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int AutoOptFieldNumber = 2;

		private ulong autoOpt_;
	}
}
