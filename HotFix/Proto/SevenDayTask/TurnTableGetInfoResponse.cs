using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.SevenDayTask
{
	public sealed class TurnTableGetInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TurnTableGetInfoResponse> Parser
		{
			get
			{
				return TurnTableGetInfoResponse._parser;
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
		public long ActivityTimeLeft
		{
			get
			{
				return this.activityTimeLeft_;
			}
			set
			{
				this.activityTimeLeft_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Count
		{
			get
			{
				return this.count_;
			}
			set
			{
				this.count_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int BigGuaranteeItemConfigId
		{
			get
			{
				return this.bigGuaranteeItemConfigId_;
			}
			set
			{
				this.bigGuaranteeItemConfigId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint BigGuaranteeItemNum
		{
			get
			{
				return this.bigGuaranteeItemNum_;
			}
			set
			{
				this.bigGuaranteeItemNum_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> TurntableRewardIds
		{
			get
			{
				return this.turntableRewardIds_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<int, int> SmallGuaranteeCount
		{
			get
			{
				return this.smallGuaranteeCount_;
			}
		}

		[DebuggerNonUserCode]
		public int TurntableId
		{
			get
			{
				return this.turntableId_;
			}
			set
			{
				this.turntableId_ = value;
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
		public int BigRewardCount
		{
			get
			{
				return this.bigRewardCount_;
			}
			set
			{
				this.bigRewardCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int BigGuaranteeCount
		{
			get
			{
				return this.bigGuaranteeCount_;
			}
			set
			{
				this.bigGuaranteeCount_ = value;
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
			if (this.ActivityTimeLeft != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.ActivityTimeLeft);
			}
			if (this.Count != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.Count);
			}
			if (this.BigGuaranteeItemConfigId != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.BigGuaranteeItemConfigId);
			}
			if (this.BigGuaranteeItemNum != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.BigGuaranteeItemNum);
			}
			this.turntableRewardIds_.WriteTo(output, TurnTableGetInfoResponse._repeated_turntableRewardIds_codec);
			this.smallGuaranteeCount_.WriteTo(output, TurnTableGetInfoResponse._map_smallGuaranteeCount_codec);
			if (this.TurntableId != 0)
			{
				output.WriteRawTag(72);
				output.WriteInt32(this.TurntableId);
			}
			this.taskData_.WriteTo(output, TurnTableGetInfoResponse._map_taskData_codec);
			this.finishedTaskId_.WriteTo(output, TurnTableGetInfoResponse._repeated_finishedTaskId_codec);
			if (this.BigRewardCount != 0)
			{
				output.WriteRawTag(96);
				output.WriteInt32(this.BigRewardCount);
			}
			if (this.BigGuaranteeCount != 0)
			{
				output.WriteRawTag(104);
				output.WriteInt32(this.BigGuaranteeCount);
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
			if (this.ActivityTimeLeft != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.ActivityTimeLeft);
			}
			if (this.Count != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Count);
			}
			if (this.BigGuaranteeItemConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.BigGuaranteeItemConfigId);
			}
			if (this.BigGuaranteeItemNum != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BigGuaranteeItemNum);
			}
			num += this.turntableRewardIds_.CalculateSize(TurnTableGetInfoResponse._repeated_turntableRewardIds_codec);
			num += this.smallGuaranteeCount_.CalculateSize(TurnTableGetInfoResponse._map_smallGuaranteeCount_codec);
			if (this.TurntableId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TurntableId);
			}
			num += this.taskData_.CalculateSize(TurnTableGetInfoResponse._map_taskData_codec);
			num += this.finishedTaskId_.CalculateSize(TurnTableGetInfoResponse._repeated_finishedTaskId_codec);
			if (this.BigRewardCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.BigRewardCount);
			}
			if (this.BigGuaranteeCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.BigGuaranteeCount);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 56U)
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
							if (this.commonData_ == null)
							{
								this.commonData_ = new CommonData();
							}
							input.ReadMessage(this.commonData_);
							continue;
						}
						if (num == 24U)
						{
							this.ActivityTimeLeft = input.ReadInt64();
							continue;
						}
					}
					else if (num <= 40U)
					{
						if (num == 32U)
						{
							this.Count = input.ReadUInt32();
							continue;
						}
						if (num == 40U)
						{
							this.BigGuaranteeItemConfigId = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 48U)
						{
							this.BigGuaranteeItemNum = input.ReadUInt32();
							continue;
						}
						if (num == 56U)
						{
							goto IL_0127;
						}
					}
				}
				else if (num <= 82U)
				{
					if (num <= 66U)
					{
						if (num == 58U)
						{
							goto IL_0127;
						}
						if (num == 66U)
						{
							this.smallGuaranteeCount_.AddEntriesFrom(input, TurnTableGetInfoResponse._map_smallGuaranteeCount_codec);
							continue;
						}
					}
					else
					{
						if (num == 72U)
						{
							this.TurntableId = input.ReadInt32();
							continue;
						}
						if (num == 82U)
						{
							this.taskData_.AddEntriesFrom(input, TurnTableGetInfoResponse._map_taskData_codec);
							continue;
						}
					}
				}
				else if (num <= 90U)
				{
					if (num == 88U || num == 90U)
					{
						this.finishedTaskId_.AddEntriesFrom(input, TurnTableGetInfoResponse._repeated_finishedTaskId_codec);
						continue;
					}
				}
				else
				{
					if (num == 96U)
					{
						this.BigRewardCount = input.ReadInt32();
						continue;
					}
					if (num == 104U)
					{
						this.BigGuaranteeCount = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
				continue;
				IL_0127:
				this.turntableRewardIds_.AddEntriesFrom(input, TurnTableGetInfoResponse._repeated_turntableRewardIds_codec);
			}
		}

		private static readonly MessageParser<TurnTableGetInfoResponse> _parser = new MessageParser<TurnTableGetInfoResponse>(() => new TurnTableGetInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ActivityTimeLeftFieldNumber = 3;

		private long activityTimeLeft_;

		public const int CountFieldNumber = 4;

		private uint count_;

		public const int BigGuaranteeItemConfigIdFieldNumber = 5;

		private int bigGuaranteeItemConfigId_;

		public const int BigGuaranteeItemNumFieldNumber = 6;

		private uint bigGuaranteeItemNum_;

		public const int TurntableRewardIdsFieldNumber = 7;

		private static readonly FieldCodec<int> _repeated_turntableRewardIds_codec = FieldCodec.ForInt32(58U);

		private readonly RepeatedField<int> turntableRewardIds_ = new RepeatedField<int>();

		public const int SmallGuaranteeCountFieldNumber = 8;

		private static readonly MapField<int, int>.Codec _map_smallGuaranteeCount_codec = new MapField<int, int>.Codec(FieldCodec.ForInt32(8U), FieldCodec.ForInt32(16U), 66U);

		private readonly MapField<int, int> smallGuaranteeCount_ = new MapField<int, int>();

		public const int TurntableIdFieldNumber = 9;

		private int turntableId_;

		public const int TaskDataFieldNumber = 10;

		private static readonly MapField<int, int>.Codec _map_taskData_codec = new MapField<int, int>.Codec(FieldCodec.ForInt32(8U), FieldCodec.ForInt32(16U), 82U);

		private readonly MapField<int, int> taskData_ = new MapField<int, int>();

		public const int FinishedTaskIdFieldNumber = 11;

		private static readonly FieldCodec<int> _repeated_finishedTaskId_codec = FieldCodec.ForInt32(90U);

		private readonly RepeatedField<int> finishedTaskId_ = new RepeatedField<int>();

		public const int BigRewardCountFieldNumber = 12;

		private int bigRewardCount_;

		public const int BigGuaranteeCountFieldNumber = 13;

		private int bigGuaranteeCount_;
	}
}
