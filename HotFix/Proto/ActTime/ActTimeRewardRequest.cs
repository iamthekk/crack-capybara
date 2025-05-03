using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.ActTime
{
	public sealed class ActTimeRewardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ActTimeRewardRequest> Parser
		{
			get
			{
				return ActTimeRewardRequest._parser;
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
		public int Opt
		{
			get
			{
				return this.opt_;
			}
			set
			{
				this.opt_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int TaskId
		{
			get
			{
				return this.taskId_;
			}
			set
			{
				this.taskId_ = value;
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
			if (this.Opt != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Opt);
			}
			if (this.TaskId != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.TaskId);
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
			if (this.Opt != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Opt);
			}
			if (this.TaskId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TaskId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 10U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 16U)
					{
						this.ActId = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Opt = input.ReadInt32();
						continue;
					}
					if (num == 32U)
					{
						this.TaskId = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ActTimeRewardRequest> _parser = new MessageParser<ActTimeRewardRequest>(() => new ActTimeRewardRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ActIdFieldNumber = 2;

		private int actId_;

		public const int OptFieldNumber = 3;

		private int opt_;

		public const int TaskIdFieldNumber = 4;

		private int taskId_;
	}
}
