using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Mission
{
	public sealed class WorldBossBoxRewardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<WorldBossBoxRewardRequest> Parser
		{
			get
			{
				return WorldBossBoxRewardRequest._parser;
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
		public int BoxRewardId
		{
			get
			{
				return this.boxRewardId_;
			}
			set
			{
				this.boxRewardId_ = value;
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
			if (this.BoxRewardId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.BoxRewardId);
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
			if (this.BoxRewardId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.BoxRewardId);
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
						this.BoxRewardId = input.ReadInt32();
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

		private static readonly MessageParser<WorldBossBoxRewardRequest> _parser = new MessageParser<WorldBossBoxRewardRequest>(() => new WorldBossBoxRewardRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int BoxRewardIdFieldNumber = 2;

		private int boxRewardId_;
	}
}
