using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Task
{
	public sealed class TaskActiveRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TaskActiveRewardResponse> Parser
		{
			get
			{
				return TaskActiveRewardResponse._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Code
		{
			get
			{
				return this.code_;
			}
			set
			{
				this.code_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Type
		{
			get
			{
				return this.type_;
			}
			set
			{
				this.type_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong RewardLog
		{
			get
			{
				return this.rewardLog_;
			}
			set
			{
				this.rewardLog_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CommonData CommonData
		{
			get
			{
				return this.commonData_;
			}
			set
			{
				this.commonData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public Tasks Tasks
		{
			get
			{
				return this.tasks_;
			}
			set
			{
				this.tasks_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.Type != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Type);
			}
			if (this.RewardLog != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.RewardLog);
			}
			if (this.commonData_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.CommonData);
			}
			if (this.tasks_ != null)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.Tasks);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.Type != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Type);
			}
			if (this.RewardLog != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RewardLog);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			if (this.tasks_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Tasks);
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
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 16U)
					{
						this.Type = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.RewardLog = input.ReadUInt64();
						continue;
					}
					if (num == 34U)
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
					if (num == 42U)
					{
						if (this.tasks_ == null)
						{
							this.tasks_ = new Tasks();
						}
						input.ReadMessage(this.tasks_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TaskActiveRewardResponse> _parser = new MessageParser<TaskActiveRewardResponse>(() => new TaskActiveRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int TypeFieldNumber = 2;

		private uint type_;

		public const int RewardLogFieldNumber = 3;

		private ulong rewardLog_;

		public const int CommonDataFieldNumber = 4;

		private CommonData commonData_;

		public const int TasksFieldNumber = 5;

		private Tasks tasks_;
	}
}
