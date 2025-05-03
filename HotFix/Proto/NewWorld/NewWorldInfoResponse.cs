using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;
using Proto.LeaderBoard;

namespace Proto.NewWorld
{
	public sealed class NewWorldInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<NewWorldInfoResponse> Parser
		{
			get
			{
				return NewWorldInfoResponse._parser;
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
		public RepeatedField<int> LikeInfo
		{
			get
			{
				return this.likeInfo_;
			}
		}

		[DebuggerNonUserCode]
		public long LikeRefreshtime
		{
			get
			{
				return this.likeRefreshtime_;
			}
			set
			{
				this.likeRefreshtime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int EnterNewWrldCount
		{
			get
			{
				return this.enterNewWrldCount_;
			}
			set
			{
				this.enterNewWrldCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> LikeCount
		{
			get
			{
				return this.likeCount_;
			}
		}

		[DebuggerNonUserCode]
		public int IsEnterNewWorld
		{
			get
			{
				return this.isEnterNewWorld_;
			}
			set
			{
				this.isEnterNewWorld_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long NewWorldOpenTime
		{
			get
			{
				return this.newWorldOpenTime_;
			}
			set
			{
				this.newWorldOpenTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RankUserDto Top1
		{
			get
			{
				return this.top1_;
			}
			set
			{
				this.top1_ = value;
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
			this.likeInfo_.WriteTo(output, NewWorldInfoResponse._repeated_likeInfo_codec);
			if (this.LikeRefreshtime != 0L)
			{
				output.WriteRawTag(32);
				output.WriteInt64(this.LikeRefreshtime);
			}
			if (this.EnterNewWrldCount != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.EnterNewWrldCount);
			}
			this.likeCount_.WriteTo(output, NewWorldInfoResponse._repeated_likeCount_codec);
			if (this.IsEnterNewWorld != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.IsEnterNewWorld);
			}
			if (this.NewWorldOpenTime != 0L)
			{
				output.WriteRawTag(64);
				output.WriteInt64(this.NewWorldOpenTime);
			}
			if (this.top1_ != null)
			{
				output.WriteRawTag(74);
				output.WriteMessage(this.Top1);
			}
			this.rewardTasks_.WriteTo(output, NewWorldInfoResponse._repeated_rewardTasks_codec);
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
			num += this.likeInfo_.CalculateSize(NewWorldInfoResponse._repeated_likeInfo_codec);
			if (this.LikeRefreshtime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.LikeRefreshtime);
			}
			if (this.EnterNewWrldCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.EnterNewWrldCount);
			}
			num += this.likeCount_.CalculateSize(NewWorldInfoResponse._repeated_likeCount_codec);
			if (this.IsEnterNewWorld != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.IsEnterNewWorld);
			}
			if (this.NewWorldOpenTime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.NewWorldOpenTime);
			}
			if (this.top1_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Top1);
			}
			return num + this.rewardTasks_.CalculateSize(NewWorldInfoResponse._repeated_rewardTasks_codec);
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
						if (num != 24U)
						{
							goto IL_0085;
						}
					}
					else if (num != 26U)
					{
						if (num == 32U)
						{
							this.LikeRefreshtime = input.ReadInt64();
							continue;
						}
						if (num != 40U)
						{
							goto IL_0085;
						}
						this.EnterNewWrldCount = input.ReadInt32();
						continue;
					}
					this.likeInfo_.AddEntriesFrom(input, NewWorldInfoResponse._repeated_likeInfo_codec);
					continue;
				}
				if (num <= 56U)
				{
					if (num == 48U || num == 50U)
					{
						this.likeCount_.AddEntriesFrom(input, NewWorldInfoResponse._repeated_likeCount_codec);
						continue;
					}
					if (num == 56U)
					{
						this.IsEnterNewWorld = input.ReadInt32();
						continue;
					}
				}
				else if (num <= 74U)
				{
					if (num == 64U)
					{
						this.NewWorldOpenTime = input.ReadInt64();
						continue;
					}
					if (num == 74U)
					{
						if (this.top1_ == null)
						{
							this.top1_ = new RankUserDto();
						}
						input.ReadMessage(this.top1_);
						continue;
					}
				}
				else if (num == 80U || num == 82U)
				{
					this.rewardTasks_.AddEntriesFrom(input, NewWorldInfoResponse._repeated_rewardTasks_codec);
					continue;
				}
				IL_0085:
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<NewWorldInfoResponse> _parser = new MessageParser<NewWorldInfoResponse>(() => new NewWorldInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int LikeInfoFieldNumber = 3;

		private static readonly FieldCodec<int> _repeated_likeInfo_codec = FieldCodec.ForInt32(26U);

		private readonly RepeatedField<int> likeInfo_ = new RepeatedField<int>();

		public const int LikeRefreshtimeFieldNumber = 4;

		private long likeRefreshtime_;

		public const int EnterNewWrldCountFieldNumber = 5;

		private int enterNewWrldCount_;

		public const int LikeCountFieldNumber = 6;

		private static readonly FieldCodec<int> _repeated_likeCount_codec = FieldCodec.ForInt32(50U);

		private readonly RepeatedField<int> likeCount_ = new RepeatedField<int>();

		public const int IsEnterNewWorldFieldNumber = 7;

		private int isEnterNewWorld_;

		public const int NewWorldOpenTimeFieldNumber = 8;

		private long newWorldOpenTime_;

		public const int Top1FieldNumber = 9;

		private RankUserDto top1_;

		public const int RewardTasksFieldNumber = 10;

		private static readonly FieldCodec<int> _repeated_rewardTasks_codec = FieldCodec.ForInt32(82U);

		private readonly RepeatedField<int> rewardTasks_ = new RepeatedField<int>();
	}
}
