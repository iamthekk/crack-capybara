using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class ChapterActiveWheelInfo : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChapterActiveWheelInfo> Parser
		{
			get
			{
				return ChapterActiveWheelInfo._parser;
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
		public int ActiveId
		{
			get
			{
				return this.activeId_;
			}
			set
			{
				this.activeId_ = value;
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
		public int RewardScore
		{
			get
			{
				return this.rewardScore_;
			}
			set
			{
				this.rewardScore_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int FreeNum
		{
			get
			{
				return this.freeNum_;
			}
			set
			{
				this.freeNum_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int FreeRate
		{
			get
			{
				return this.freeRate_;
			}
			set
			{
				this.freeRate_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int PlayTimes
		{
			get
			{
				return this.playTimes_;
			}
			set
			{
				this.playTimes_ = value;
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
			if (this.ActiveId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.ActiveId);
			}
			if (this.StartTime != 0L)
			{
				output.WriteRawTag(32);
				output.WriteInt64(this.StartTime);
			}
			if (this.EndTime != 0L)
			{
				output.WriteRawTag(40);
				output.WriteInt64(this.EndTime);
			}
			if (this.Score != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.Score);
			}
			if (this.RewardScore != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.RewardScore);
			}
			if (this.FreeNum != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.FreeNum);
			}
			if (this.FreeRate != 0)
			{
				output.WriteRawTag(72);
				output.WriteInt32(this.FreeRate);
			}
			if (this.PlayTimes != 0)
			{
				output.WriteRawTag(80);
				output.WriteInt32(this.PlayTimes);
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
			if (this.ActiveId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ActiveId);
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
			if (this.RewardScore != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.RewardScore);
			}
			if (this.FreeNum != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.FreeNum);
			}
			if (this.FreeRate != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.FreeRate);
			}
			if (this.PlayTimes != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.PlayTimes);
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
					if (num <= 24U)
					{
						if (num == 8U)
						{
							this.RowId = input.ReadInt64();
							continue;
						}
						if (num == 24U)
						{
							this.ActiveId = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 32U)
						{
							this.StartTime = input.ReadInt64();
							continue;
						}
						if (num == 40U)
						{
							this.EndTime = input.ReadInt64();
							continue;
						}
					}
				}
				else if (num <= 56U)
				{
					if (num == 48U)
					{
						this.Score = input.ReadInt32();
						continue;
					}
					if (num == 56U)
					{
						this.RewardScore = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 64U)
					{
						this.FreeNum = input.ReadInt32();
						continue;
					}
					if (num == 72U)
					{
						this.FreeRate = input.ReadInt32();
						continue;
					}
					if (num == 80U)
					{
						this.PlayTimes = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChapterActiveWheelInfo> _parser = new MessageParser<ChapterActiveWheelInfo>(() => new ChapterActiveWheelInfo());

		public const int RowIdFieldNumber = 1;

		private long rowId_;

		public const int ActiveIdFieldNumber = 3;

		private int activeId_;

		public const int StartTimeFieldNumber = 4;

		private long startTime_;

		public const int EndTimeFieldNumber = 5;

		private long endTime_;

		public const int ScoreFieldNumber = 6;

		private int score_;

		public const int RewardScoreFieldNumber = 7;

		private int rewardScore_;

		public const int FreeNumFieldNumber = 8;

		private int freeNum_;

		public const int FreeRateFieldNumber = 9;

		private int freeRate_;

		public const int PlayTimesFieldNumber = 10;

		private int playTimes_;
	}
}
