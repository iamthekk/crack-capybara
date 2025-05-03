using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildTaskRefreshRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildTaskRefreshRequest> Parser
		{
			get
			{
				return GuildTaskRefreshRequest._parser;
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
		public uint TaskId
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
			if (this.TaskId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.TaskId);
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
			if (this.TaskId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TaskId);
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
						this.TaskId = input.ReadUInt32();
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

		private static readonly MessageParser<GuildTaskRefreshRequest> _parser = new MessageParser<GuildTaskRefreshRequest>(() => new GuildTaskRefreshRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int TaskIdFieldNumber = 2;

		private uint taskId_;
	}
}
