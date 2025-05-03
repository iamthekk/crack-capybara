using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.NewWorld
{
	public sealed class NewWorldTaskRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<NewWorldTaskRewardResponse> Parser
		{
			get
			{
				return NewWorldTaskRewardResponse._parser;
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
		public RepeatedField<int> RewardTasks
		{
			get
			{
				return this.rewardTasks_;
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
			this.rewardTasks_.WriteTo(output, NewWorldTaskRewardResponse._repeated_rewardTasks_codec);
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
			return num + this.rewardTasks_.CalculateSize(NewWorldTaskRewardResponse._repeated_rewardTasks_codec);
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
				else if (num == 24U || num == 26U)
				{
					this.rewardTasks_.AddEntriesFrom(input, NewWorldTaskRewardResponse._repeated_rewardTasks_codec);
					continue;
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<NewWorldTaskRewardResponse> _parser = new MessageParser<NewWorldTaskRewardResponse>(() => new NewWorldTaskRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RewardTasksFieldNumber = 3;

		private static readonly FieldCodec<int> _repeated_rewardTasks_codec = FieldCodec.ForInt32(26U);

		private readonly RepeatedField<int> rewardTasks_ = new RepeatedField<int>();
	}
}
