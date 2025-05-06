﻿using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class EndChapterResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<EndChapterResponse> Parser
		{
			get
			{
				return EndChapterResponse._parser;
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
		public int ChapterId
		{
			get
			{
				return this.chapterId_;
			}
			set
			{
				this.chapterId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int WaveIndex
		{
			get
			{
				return this.waveIndex_;
			}
			set
			{
				this.waveIndex_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Result
		{
			get
			{
				return this.result_;
			}
			set
			{
				this.result_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> CanRewardList
		{
			get
			{
				return this.canRewardList_;
			}
		}

		[DebuggerNonUserCode]
		public int BattleTimes
		{
			get
			{
				return this.battleTimes_;
			}
			set
			{
				this.battleTimes_ = value;
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
			if (this.ChapterId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.ChapterId);
			}
			if (this.WaveIndex != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.WaveIndex);
			}
			if (this.Result != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.Result);
			}
			this.canRewardList_.WriteTo(output, EndChapterResponse._repeated_canRewardList_codec);
			if (this.BattleTimes != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.BattleTimes);
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
			if (this.ChapterId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ChapterId);
			}
			if (this.WaveIndex != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.WaveIndex);
			}
			if (this.Result != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Result);
			}
			num += this.canRewardList_.CalculateSize(EndChapterResponse._repeated_canRewardList_codec);
			if (this.BattleTimes != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.BattleTimes);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num > 32U)
				{
					if (num <= 48U)
					{
						if (num == 40U)
						{
							this.Result = input.ReadInt32();
							continue;
						}
						if (num != 48U)
						{
							goto IL_0041;
						}
					}
					else if (num != 50U)
					{
						if (num != 56U)
						{
							goto IL_0041;
						}
						this.BattleTimes = input.ReadInt32();
						continue;
					}
					this.canRewardList_.AddEntriesFrom(input, EndChapterResponse._repeated_canRewardList_codec);
					continue;
				}
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
						this.ChapterId = input.ReadInt32();
						continue;
					}
					if (num == 32U)
					{
						this.WaveIndex = input.ReadInt32();
						continue;
					}
				}
				IL_0041:
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<EndChapterResponse> _parser = new MessageParser<EndChapterResponse>(() => new EndChapterResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ChapterIdFieldNumber = 3;

		private int chapterId_;

		public const int WaveIndexFieldNumber = 4;

		private int waveIndex_;

		public const int ResultFieldNumber = 5;

		private int result_;

		public const int CanRewardListFieldNumber = 6;

		private static readonly FieldCodec<int> _repeated_canRewardList_codec = FieldCodec.ForInt32(50U);

		private readonly RepeatedField<int> canRewardList_ = new RepeatedField<int>();

		public const int BattleTimesFieldNumber = 7;

		private int battleTimes_;
	}
}
