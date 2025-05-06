using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class TaskDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TaskDto> Parser
		{
			get
			{
				return TaskDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint Id
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
		public uint Process
		{
			get
			{
				return this.process_;
			}
			set
			{
				this.process_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool IsFinish
		{
			get
			{
				return this.isFinish_;
			}
			set
			{
				this.isFinish_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool IsReceive
		{
			get
			{
				return this.isReceive_;
			}
			set
			{
				this.isReceive_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TaskType
		{
			get
			{
				return this.taskType_;
			}
			set
			{
				this.taskType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Id != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.Id);
			}
			if (this.Process != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Process);
			}
			if (this.IsFinish)
			{
				output.WriteRawTag(24);
				output.WriteBool(this.IsFinish);
			}
			if (this.IsReceive)
			{
				output.WriteRawTag(32);
				output.WriteBool(this.IsReceive);
			}
			if (this.TaskType != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.TaskType);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Id != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Id);
			}
			if (this.Process != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Process);
			}
			if (this.IsFinish)
			{
				num += 2;
			}
			if (this.IsReceive)
			{
				num += 2;
			}
			if (this.TaskType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TaskType);
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
						this.Id = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.Process = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.IsFinish = input.ReadBool();
						continue;
					}
					if (num == 32U)
					{
						this.IsReceive = input.ReadBool();
						continue;
					}
					if (num == 40U)
					{
						this.TaskType = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TaskDto> _parser = new MessageParser<TaskDto>(() => new TaskDto());

		public const int IdFieldNumber = 1;

		private uint id_;

		public const int ProcessFieldNumber = 2;

		private uint process_;

		public const int IsFinishFieldNumber = 3;

		private bool isFinish_;

		public const int IsReceiveFieldNumber = 4;

		private bool isReceive_;

		public const int TaskTypeFieldNumber = 5;

		private uint taskType_;
	}
}
