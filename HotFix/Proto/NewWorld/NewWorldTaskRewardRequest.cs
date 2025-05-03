using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.NewWorld
{
	public sealed class NewWorldTaskRewardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<NewWorldTaskRewardRequest> Parser
		{
			get
			{
				return NewWorldTaskRewardRequest._parser;
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
		public int ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
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
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ConfigId);
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
			if (this.ConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ConfigId);
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
						this.ConfigId = input.ReadInt32();
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

		private static readonly MessageParser<NewWorldTaskRewardRequest> _parser = new MessageParser<NewWorldTaskRewardRequest>(() => new NewWorldTaskRewardRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ConfigIdFieldNumber = 2;

		private int configId_;
	}
}
