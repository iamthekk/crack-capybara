using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.SevenDayTask
{
	public sealed class SevenDayDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SevenDayDto> Parser
		{
			get
			{
				return SevenDayDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint Days
		{
			get
			{
				return this.days_;
			}
			set
			{
				this.days_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong TaskEndTimestamp
		{
			get
			{
				return this.taskEndTimestamp_;
			}
			set
			{
				this.taskEndTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong EndTimestamp
		{
			get
			{
				return this.endTimestamp_;
			}
			set
			{
				this.endTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong ActiveLog
		{
			get
			{
				return this.activeLog_;
			}
			set
			{
				this.activeLog_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Active
		{
			get
			{
				return this.active_;
			}
			set
			{
				this.active_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<SevenDayTaskDto> Tasks
		{
			get
			{
				return this.tasks_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Days != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.Days);
			}
			if (this.TaskEndTimestamp != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.TaskEndTimestamp);
			}
			if (this.EndTimestamp != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.EndTimestamp);
			}
			if (this.ActiveLog != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.ActiveLog);
			}
			if (this.Active != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.Active);
			}
			this.tasks_.WriteTo(output, SevenDayDto._repeated_tasks_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Days != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Days);
			}
			if (this.TaskEndTimestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.TaskEndTimestamp);
			}
			if (this.EndTimestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.EndTimestamp);
			}
			if (this.ActiveLog != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.ActiveLog);
			}
			if (this.Active != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Active);
			}
			return num + this.tasks_.CalculateSize(SevenDayDto._repeated_tasks_codec);
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
						this.Days = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.TaskEndTimestamp = input.ReadUInt64();
						continue;
					}
					if (num == 24U)
					{
						this.EndTimestamp = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 32U)
					{
						this.ActiveLog = input.ReadUInt64();
						continue;
					}
					if (num == 40U)
					{
						this.Active = input.ReadUInt32();
						continue;
					}
					if (num == 50U)
					{
						this.tasks_.AddEntriesFrom(input, SevenDayDto._repeated_tasks_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<SevenDayDto> _parser = new MessageParser<SevenDayDto>(() => new SevenDayDto());

		public const int DaysFieldNumber = 1;

		private uint days_;

		public const int TaskEndTimestampFieldNumber = 2;

		private ulong taskEndTimestamp_;

		public const int EndTimestampFieldNumber = 3;

		private ulong endTimestamp_;

		public const int ActiveLogFieldNumber = 4;

		private ulong activeLog_;

		public const int ActiveFieldNumber = 5;

		private uint active_;

		public const int TasksFieldNumber = 6;

		private static readonly FieldCodec<SevenDayTaskDto> _repeated_tasks_codec = FieldCodec.ForMessage<SevenDayTaskDto>(50U, SevenDayTaskDto.Parser);

		private readonly RepeatedField<SevenDayTaskDto> tasks_ = new RepeatedField<SevenDayTaskDto>();
	}
}
