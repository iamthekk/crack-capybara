using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Task
{
	public sealed class TaskGetInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TaskGetInfoResponse> Parser
		{
			get
			{
				return TaskGetInfoResponse._parser;
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
		public uint DailyTaskActive
		{
			get
			{
				return this.dailyTaskActive_;
			}
			set
			{
				this.dailyTaskActive_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint WeeklyTaskActive
		{
			get
			{
				return this.weeklyTaskActive_;
			}
			set
			{
				this.weeklyTaskActive_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong DailyTaskResetTime
		{
			get
			{
				return this.dailyTaskResetTime_;
			}
			set
			{
				this.dailyTaskResetTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong DailyTaskRewardLog
		{
			get
			{
				return this.dailyTaskRewardLog_;
			}
			set
			{
				this.dailyTaskRewardLog_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong WeeklyTaskRewardLog
		{
			get
			{
				return this.weeklyTaskRewardLog_;
			}
			set
			{
				this.weeklyTaskRewardLog_ = value;
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
			if (this.tasks_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.Tasks);
			}
			if (this.DailyTaskActive != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.DailyTaskActive);
			}
			if (this.WeeklyTaskActive != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.WeeklyTaskActive);
			}
			if (this.DailyTaskResetTime != 0UL)
			{
				output.WriteRawTag(40);
				output.WriteUInt64(this.DailyTaskResetTime);
			}
			if (this.DailyTaskRewardLog != 0UL)
			{
				output.WriteRawTag(48);
				output.WriteUInt64(this.DailyTaskRewardLog);
			}
			if (this.WeeklyTaskRewardLog != 0UL)
			{
				output.WriteRawTag(56);
				output.WriteUInt64(this.WeeklyTaskRewardLog);
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
			if (this.tasks_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Tasks);
			}
			if (this.DailyTaskActive != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.DailyTaskActive);
			}
			if (this.WeeklyTaskActive != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.WeeklyTaskActive);
			}
			if (this.DailyTaskResetTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.DailyTaskResetTime);
			}
			if (this.DailyTaskRewardLog != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.DailyTaskRewardLog);
			}
			if (this.WeeklyTaskRewardLog != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.WeeklyTaskRewardLog);
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
					if (num == 18U)
					{
						if (this.tasks_ == null)
						{
							this.tasks_ = new Tasks();
						}
						input.ReadMessage(this.tasks_);
						continue;
					}
					if (num == 24U)
					{
						this.DailyTaskActive = input.ReadUInt32();
						continue;
					}
				}
				else if (num <= 40U)
				{
					if (num == 32U)
					{
						this.WeeklyTaskActive = input.ReadUInt32();
						continue;
					}
					if (num == 40U)
					{
						this.DailyTaskResetTime = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 48U)
					{
						this.DailyTaskRewardLog = input.ReadUInt64();
						continue;
					}
					if (num == 56U)
					{
						this.WeeklyTaskRewardLog = input.ReadUInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TaskGetInfoResponse> _parser = new MessageParser<TaskGetInfoResponse>(() => new TaskGetInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int TasksFieldNumber = 2;

		private Tasks tasks_;

		public const int DailyTaskActiveFieldNumber = 3;

		private uint dailyTaskActive_;

		public const int WeeklyTaskActiveFieldNumber = 4;

		private uint weeklyTaskActive_;

		public const int DailyTaskResetTimeFieldNumber = 5;

		private ulong dailyTaskResetTime_;

		public const int DailyTaskRewardLogFieldNumber = 6;

		private ulong dailyTaskRewardLog_;

		public const int WeeklyTaskRewardLogFieldNumber = 7;

		private ulong weeklyTaskRewardLog_;
	}
}
