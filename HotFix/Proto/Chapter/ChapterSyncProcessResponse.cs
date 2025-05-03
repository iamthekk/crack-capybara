using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class ChapterSyncProcessResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChapterSyncProcessResponse> Parser
		{
			get
			{
				return ChapterSyncProcessResponse._parser;
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
		public RepeatedField<int> CanRewardList
		{
			get
			{
				return this.canRewardList_;
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
			this.canRewardList_.WriteTo(output, ChapterSyncProcessResponse._repeated_canRewardList_codec);
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
			return num + this.canRewardList_.CalculateSize(ChapterSyncProcessResponse._repeated_canRewardList_codec);
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
						this.ChapterId = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 32U)
					{
						this.WaveIndex = input.ReadInt32();
						continue;
					}
					if (num == 40U || num == 42U)
					{
						this.canRewardList_.AddEntriesFrom(input, ChapterSyncProcessResponse._repeated_canRewardList_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChapterSyncProcessResponse> _parser = new MessageParser<ChapterSyncProcessResponse>(() => new ChapterSyncProcessResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ChapterIdFieldNumber = 3;

		private int chapterId_;

		public const int WaveIndexFieldNumber = 4;

		private int waveIndex_;

		public const int CanRewardListFieldNumber = 5;

		private static readonly FieldCodec<int> _repeated_canRewardList_codec = FieldCodec.ForInt32(42U);

		private readonly RepeatedField<int> canRewardList_ = new RepeatedField<int>();
	}
}
