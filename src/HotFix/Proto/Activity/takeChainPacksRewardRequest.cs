using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Activity
{
	public sealed class takeChainPacksRewardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<takeChainPacksRewardRequest> Parser
		{
			get
			{
				return takeChainPacksRewardRequest._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Id
		{
			get
			{
				return this.id_;
			}
			set
			{
				this.id_ = value;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Id != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Id);
			}
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonParams);
			}
			if (this.ActId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.ActId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Id != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Id);
			}
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.ActId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ActId);
			}
			return num;
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
						if (num != 24U)
						{
							input.SkipLastField();
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
				else
				{
					this.Id = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<takeChainPacksRewardRequest> _parser = new MessageParser<takeChainPacksRewardRequest>(() => new takeChainPacksRewardRequest());

		public const int IdFieldNumber = 1;

		private int id_;

		public const int CommonParamsFieldNumber = 2;

		private CommonParams commonParams_;

		public const int ActIdFieldNumber = 3;

		private int actId_;
	}
}
