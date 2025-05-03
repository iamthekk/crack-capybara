using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Task
{
	public sealed class Tasks : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<Tasks> Parser
		{
			get
			{
				return Tasks._parser;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<TaskDto> DailyTask
		{
			get
			{
				return this.dailyTask_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<TaskDto> Achievements
		{
			get
			{
				return this.achievements_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.dailyTask_.WriteTo(output, Tasks._repeated_dailyTask_codec);
			this.achievements_.WriteTo(output, Tasks._repeated_achievements_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			return 0 + this.dailyTask_.CalculateSize(Tasks._repeated_dailyTask_codec) + this.achievements_.CalculateSize(Tasks._repeated_achievements_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						this.achievements_.AddEntriesFrom(input, Tasks._repeated_achievements_codec);
					}
				}
				else
				{
					this.dailyTask_.AddEntriesFrom(input, Tasks._repeated_dailyTask_codec);
				}
			}
		}

		private static readonly MessageParser<Tasks> _parser = new MessageParser<Tasks>(() => new Tasks());

		public const int DailyTaskFieldNumber = 1;

		private static readonly FieldCodec<TaskDto> _repeated_dailyTask_codec = FieldCodec.ForMessage<TaskDto>(10U, TaskDto.Parser);

		private readonly RepeatedField<TaskDto> dailyTask_ = new RepeatedField<TaskDto>();

		public const int AchievementsFieldNumber = 2;

		private static readonly FieldCodec<TaskDto> _repeated_achievements_codec = FieldCodec.ForMessage<TaskDto>(18U, TaskDto.Parser);

		private readonly RepeatedField<TaskDto> achievements_ = new RepeatedField<TaskDto>();
	}
}
