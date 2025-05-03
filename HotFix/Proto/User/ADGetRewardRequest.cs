using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.User
{
	public sealed class ADGetRewardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ADGetRewardRequest> Parser
		{
			get
			{
				return ADGetRewardRequest._parser;
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
		public int AdType
		{
			get
			{
				return this.adType_;
			}
			set
			{
				this.adType_ = value;
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
			if (this.AdType != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.AdType);
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
			if (this.AdType != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.AdType);
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
						this.AdType = input.ReadInt32();
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

		private static readonly MessageParser<ADGetRewardRequest> _parser = new MessageParser<ADGetRewardRequest>(() => new ADGetRewardRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int AdTypeFieldNumber = 2;

		private int adType_;
	}
}
