using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class ChapterWheelSpineResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChapterWheelSpineResponse> Parser
		{
			get
			{
				return ChapterWheelSpineResponse._parser;
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
		public long RewardId
		{
			get
			{
				return this.rewardId_;
			}
			set
			{
				this.rewardId_ = value;
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
			if (this.RewardId != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.RewardId);
			}
			if (this.Score != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Score);
			}
			if (this.RewardScore != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.RewardScore);
			}
			if (this.FreeNum != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.FreeNum);
			}
			if (this.FreeRate != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.FreeRate);
			}
			if (this.PlayTimes != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.PlayTimes);
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
			if (this.RewardId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.RewardId);
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
				if (num <= 32U)
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
						if (num == 24U)
						{
							this.RewardId = input.ReadInt64();
							continue;
						}
						if (num == 32U)
						{
							this.Score = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 40U)
					{
						this.RewardScore = input.ReadInt32();
						continue;
					}
					if (num == 48U)
					{
						this.FreeNum = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 56U)
					{
						this.FreeRate = input.ReadInt32();
						continue;
					}
					if (num == 64U)
					{
						this.PlayTimes = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChapterWheelSpineResponse> _parser = new MessageParser<ChapterWheelSpineResponse>(() => new ChapterWheelSpineResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RewardIdFieldNumber = 3;

		private long rewardId_;

		public const int ScoreFieldNumber = 4;

		private int score_;

		public const int RewardScoreFieldNumber = 5;

		private int rewardScore_;

		public const int FreeNumFieldNumber = 6;

		private int freeNum_;

		public const int FreeRateFieldNumber = 7;

		private int freeRate_;

		public const int PlayTimesFieldNumber = 8;

		private int playTimes_;
	}
}
