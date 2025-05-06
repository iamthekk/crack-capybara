using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildTaskDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildTaskDto> Parser
		{
			get
			{
				return GuildTaskDto._parser;
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
		public uint Progress
		{
			get
			{
				return this.progress_;
			}
			set
			{
				this.progress_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Need
		{
			get
			{
				return this.need_;
			}
			set
			{
				this.need_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> Rewards
		{
			get
			{
				return this.rewards_;
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
		public string LanguageId
		{
			get
			{
				return this.languageId_;
			}
			set
			{
				this.languageId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.TaskId != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.TaskId);
			}
			if (this.Progress != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Progress);
			}
			if (this.Need != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Need);
			}
			this.rewards_.WriteTo(output, GuildTaskDto._repeated_rewards_codec);
			if (this.IsFinish)
			{
				output.WriteRawTag(40);
				output.WriteBool(this.IsFinish);
			}
			if (this.IsReceive)
			{
				output.WriteRawTag(48);
				output.WriteBool(this.IsReceive);
			}
			if (this.LanguageId.Length != 0)
			{
				output.WriteRawTag(58);
				output.WriteString(this.LanguageId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.TaskId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TaskId);
			}
			if (this.Progress != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Progress);
			}
			if (this.Need != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Need);
			}
			num += this.rewards_.CalculateSize(GuildTaskDto._repeated_rewards_codec);
			if (this.IsFinish)
			{
				num += 2;
			}
			if (this.IsReceive)
			{
				num += 2;
			}
			if (this.LanguageId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.LanguageId);
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
						this.TaskId = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.Progress = input.ReadUInt32();
						continue;
					}
					if (num == 24U)
					{
						this.Need = input.ReadUInt32();
						continue;
					}
				}
				else if (num <= 40U)
				{
					if (num == 34U)
					{
						this.rewards_.AddEntriesFrom(input, GuildTaskDto._repeated_rewards_codec);
						continue;
					}
					if (num == 40U)
					{
						this.IsFinish = input.ReadBool();
						continue;
					}
				}
				else
				{
					if (num == 48U)
					{
						this.IsReceive = input.ReadBool();
						continue;
					}
					if (num == 58U)
					{
						this.LanguageId = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildTaskDto> _parser = new MessageParser<GuildTaskDto>(() => new GuildTaskDto());

		public const int TaskIdFieldNumber = 1;

		private uint taskId_;

		public const int ProgressFieldNumber = 2;

		private uint progress_;

		public const int NeedFieldNumber = 3;

		private uint need_;

		public const int RewardsFieldNumber = 4;

		private static readonly FieldCodec<RewardDto> _repeated_rewards_codec = FieldCodec.ForMessage<RewardDto>(34U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> rewards_ = new RepeatedField<RewardDto>();

		public const int IsFinishFieldNumber = 5;

		private bool isFinish_;

		public const int IsReceiveFieldNumber = 6;

		private bool isReceive_;

		public const int LanguageIdFieldNumber = 7;

		private string languageId_ = "";
	}
}
