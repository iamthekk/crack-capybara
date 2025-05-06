using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class ChapterBattlePassDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChapterBattlePassDto> Parser
		{
			get
			{
				return ChapterBattlePassDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public long RowId
		{
			get
			{
				return this.rowId_;
			}
			set
			{
				this.rowId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long StartTime
		{
			get
			{
				return this.startTime_;
			}
			set
			{
				this.startTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long EndTime
		{
			get
			{
				return this.endTime_;
			}
			set
			{
				this.endTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Score
		{
			get
			{
				return this.score_;
			}
			set
			{
				this.score_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Buy
		{
			get
			{
				return this.buy_;
			}
			set
			{
				this.buy_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> FreeRewardList
		{
			get
			{
				return this.freeRewardList_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> PayRewardList
		{
			get
			{
				return this.payRewardList_;
			}
		}

		[DebuggerNonUserCode]
		public int FinalRewardCount
		{
			get
			{
				return this.finalRewardCount_;
			}
			set
			{
				this.finalRewardCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RowId != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.RowId);
			}
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ConfigId);
			}
			if (this.StartTime != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.StartTime);
			}
			if (this.EndTime != 0L)
			{
				output.WriteRawTag(32);
				output.WriteInt64(this.EndTime);
			}
			if (this.Score != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.Score);
			}
			if (this.Buy != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.Buy);
			}
			this.freeRewardList_.WriteTo(output, ChapterBattlePassDto._repeated_freeRewardList_codec);
			this.payRewardList_.WriteTo(output, ChapterBattlePassDto._repeated_payRewardList_codec);
			if (this.FinalRewardCount != 0)
			{
				output.WriteRawTag(72);
				output.WriteInt32(this.FinalRewardCount);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RowId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.RowId);
			}
			if (this.ConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ConfigId);
			}
			if (this.StartTime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.StartTime);
			}
			if (this.EndTime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.EndTime);
			}
			if (this.Score != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Score);
			}
			if (this.Buy != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Buy);
			}
			num += this.freeRewardList_.CalculateSize(ChapterBattlePassDto._repeated_freeRewardList_codec);
			num += this.payRewardList_.CalculateSize(ChapterBattlePassDto._repeated_payRewardList_codec);
			if (this.FinalRewardCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.FinalRewardCount);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 40U)
				{
					if (num <= 16U)
					{
						if (num == 8U)
						{
							this.RowId = input.ReadInt64();
							continue;
						}
						if (num == 16U)
						{
							this.ConfigId = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.StartTime = input.ReadInt64();
							continue;
						}
						if (num == 32U)
						{
							this.EndTime = input.ReadInt64();
							continue;
						}
						if (num == 40U)
						{
							this.Score = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 58U)
				{
					if (num == 48U)
					{
						this.Buy = input.ReadInt32();
						continue;
					}
					if (num == 56U || num == 58U)
					{
						this.freeRewardList_.AddEntriesFrom(input, ChapterBattlePassDto._repeated_freeRewardList_codec);
						continue;
					}
				}
				else
				{
					if (num == 64U || num == 66U)
					{
						this.payRewardList_.AddEntriesFrom(input, ChapterBattlePassDto._repeated_payRewardList_codec);
						continue;
					}
					if (num == 72U)
					{
						this.FinalRewardCount = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChapterBattlePassDto> _parser = new MessageParser<ChapterBattlePassDto>(() => new ChapterBattlePassDto());

		public const int RowIdFieldNumber = 1;

		private long rowId_;

		public const int ConfigIdFieldNumber = 2;

		private int configId_;

		public const int StartTimeFieldNumber = 3;

		private long startTime_;

		public const int EndTimeFieldNumber = 4;

		private long endTime_;

		public const int ScoreFieldNumber = 5;

		private int score_;

		public const int BuyFieldNumber = 6;

		private int buy_;

		public const int FreeRewardListFieldNumber = 7;

		private static readonly FieldCodec<int> _repeated_freeRewardList_codec = FieldCodec.ForInt32(58U);

		private readonly RepeatedField<int> freeRewardList_ = new RepeatedField<int>();

		public const int PayRewardListFieldNumber = 8;

		private static readonly FieldCodec<int> _repeated_payRewardList_codec = FieldCodec.ForInt32(66U);

		private readonly RepeatedField<int> payRewardList_ = new RepeatedField<int>();

		public const int FinalRewardCountFieldNumber = 9;

		private int finalRewardCount_;
	}
}
