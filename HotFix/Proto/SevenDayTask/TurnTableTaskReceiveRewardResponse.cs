using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.SevenDayTask
{
	public sealed class TurnTableTaskReceiveRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TurnTableTaskReceiveRewardResponse> Parser
		{
			get
			{
				return TurnTableTaskReceiveRewardResponse._parser;
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
		public MapField<int, int> TaskData
		{
			get
			{
				return this.taskData_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> FinishedTaskId
		{
			get
			{
				return this.finishedTaskId_;
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
			this.taskData_.WriteTo(output, TurnTableTaskReceiveRewardResponse._map_taskData_codec);
			this.finishedTaskId_.WriteTo(output, TurnTableTaskReceiveRewardResponse._repeated_finishedTaskId_codec);
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
			num += this.taskData_.CalculateSize(TurnTableTaskReceiveRewardResponse._map_taskData_codec);
			return num + this.finishedTaskId_.CalculateSize(TurnTableTaskReceiveRewardResponse._repeated_finishedTaskId_codec);
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
						this.taskData_.AddEntriesFrom(input, TurnTableTaskReceiveRewardResponse._map_taskData_codec);
						continue;
					}
					if (num == 32U || num == 34U)
					{
						this.finishedTaskId_.AddEntriesFrom(input, TurnTableTaskReceiveRewardResponse._repeated_finishedTaskId_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TurnTableTaskReceiveRewardResponse> _parser = new MessageParser<TurnTableTaskReceiveRewardResponse>(() => new TurnTableTaskReceiveRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int TaskDataFieldNumber = 3;

		private static readonly MapField<int, int>.Codec _map_taskData_codec = new MapField<int, int>.Codec(FieldCodec.ForInt32(8U), FieldCodec.ForInt32(16U), 26U);

		private readonly MapField<int, int> taskData_ = new MapField<int, int>();

		public const int FinishedTaskIdFieldNumber = 4;

		private static readonly FieldCodec<int> _repeated_finishedTaskId_codec = FieldCodec.ForInt32(34U);

		private readonly RepeatedField<int> finishedTaskId_ = new RepeatedField<int>();
	}
}
