using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Task
{
	public sealed class TaskRewardDailyResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TaskRewardDailyResponse> Parser
		{
			get
			{
				return TaskRewardDailyResponse._parser;
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
		public uint ActiveDaily
		{
			get
			{
				return this.activeDaily_;
			}
			set
			{
				this.activeDaily_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ActiveWeekly
		{
			get
			{
				return this.activeWeekly_;
			}
			set
			{
				this.activeWeekly_ = value;
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
		public TaskDto UpdateTaskDto
		{
			get
			{
				return this.updateTaskDto_;
			}
			set
			{
				this.updateTaskDto_ = value;
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
			if (this.ActiveDaily != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.ActiveDaily);
			}
			if (this.ActiveWeekly != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.ActiveWeekly);
			}
			if (this.commonData_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.CommonData);
			}
			if (this.updateTaskDto_ != null)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.UpdateTaskDto);
			}
			if (this.tasks_ != null)
			{
				output.WriteRawTag(50);
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
			if (this.ActiveDaily != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ActiveDaily);
			}
			if (this.ActiveWeekly != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ActiveWeekly);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			if (this.updateTaskDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UpdateTaskDto);
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
				if (num <= 24U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 16U)
					{
						this.ActiveDaily = input.ReadUInt32();
						continue;
					}
					if (num == 24U)
					{
						this.ActiveWeekly = input.ReadUInt32();
						continue;
					}
				}
				else
				{
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
						if (this.updateTaskDto_ == null)
						{
							this.updateTaskDto_ = new TaskDto();
						}
						input.ReadMessage(this.updateTaskDto_);
						continue;
					}
					if (num == 50U)
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

		private static readonly MessageParser<TaskRewardDailyResponse> _parser = new MessageParser<TaskRewardDailyResponse>(() => new TaskRewardDailyResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int ActiveDailyFieldNumber = 2;

		private uint activeDaily_;

		public const int ActiveWeeklyFieldNumber = 3;

		private uint activeWeekly_;

		public const int CommonDataFieldNumber = 4;

		private CommonData commonData_;

		public const int UpdateTaskDtoFieldNumber = 5;

		private TaskDto updateTaskDto_;

		public const int TasksFieldNumber = 6;

		private Tasks tasks_;
	}
}
