using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.ActTime
{
	public sealed class ActTimePayFreeRewardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ActTimePayFreeRewardRequest> Parser
		{
			get
			{
				return ActTimePayFreeRewardRequest._parser;
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
		public int ActId
		{
			get
			{
				return this.actId_;
			}
			set
			{
				this.actId_ = value;
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
			if (this.ActId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ActId);
			}
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(24);
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
			if (this.ActId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ActId);
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
						if (num != 24U)
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
						this.ActId = input.ReadInt32();
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

		private static readonly MessageParser<ActTimePayFreeRewardRequest> _parser = new MessageParser<ActTimePayFreeRewardRequest>(() => new ActTimePayFreeRewardRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ActIdFieldNumber = 2;

		private int actId_;

		public const int ConfigIdFieldNumber = 3;

		private int configId_;
	}
}
