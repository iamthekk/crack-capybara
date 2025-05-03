using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Task
{
	public sealed class TaskRewardAchieveResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TaskRewardAchieveResponse> Parser
		{
			get
			{
				return TaskRewardAchieveResponse._parser;
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
		public uint DeleteTaskDtoId
		{
			get
			{
				return this.deleteTaskDtoId_;
			}
			set
			{
				this.deleteTaskDtoId_ = value;
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
			if (this.commonData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonData);
			}
			if (this.updateTaskDto_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.UpdateTaskDto);
			}
			if (this.DeleteTaskDtoId != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.DeleteTaskDtoId);
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
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			if (this.updateTaskDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UpdateTaskDto);
			}
			if (this.DeleteTaskDtoId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.DeleteTaskDtoId);
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
				if (num <= 18U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 18U)
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						if (this.updateTaskDto_ == null)
						{
							this.updateTaskDto_ = new TaskDto();
						}
						input.ReadMessage(this.updateTaskDto_);
						continue;
					}
					if (num == 32U)
					{
						this.DeleteTaskDtoId = input.ReadUInt32();
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

		private static readonly MessageParser<TaskRewardAchieveResponse> _parser = new MessageParser<TaskRewardAchieveResponse>(() => new TaskRewardAchieveResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int UpdateTaskDtoFieldNumber = 3;

		private TaskDto updateTaskDto_;

		public const int DeleteTaskDtoIdFieldNumber = 4;

		private uint deleteTaskDtoId_;

		public const int TasksFieldNumber = 5;

		private Tasks tasks_;
	}
}
