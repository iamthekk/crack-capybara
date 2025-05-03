using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class ChapterBattlePassScoreRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChapterBattlePassScoreRequest> Parser
		{
			get
			{
				return ChapterBattlePassScoreRequest._parser;
			}
		}

		[DebuggerNonUserCode]
		public CommonParams CommonParams
		{
			get
			{
				return this.commonParams_;
			}
			set
			{
				this.commonParams_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Day
		{
			get
			{
				return this.day_;
			}
			set
			{
				this.day_ = value;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.CommonParams);
			}
			if (this.Day != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Day);
			}
			if (this.RowId != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.RowId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.Day != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Day);
			}
			if (this.RowId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.RowId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U)
					{
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.RowId = input.ReadInt64();
						}
					}
					else
					{
						this.Day = input.ReadInt32();
					}
				}
				else
				{
					if (this.commonParams_ == null)
					{
						this.commonParams_ = new CommonParams();
					}
					input.ReadMessage(this.commonParams_);
				}
			}
		}

		private static readonly MessageParser<ChapterBattlePassScoreRequest> _parser = new MessageParser<ChapterBattlePassScoreRequest>(() => new ChapterBattlePassScoreRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int DayFieldNumber = 2;

		private int day_;

		public const int RowIdFieldNumber = 3;

		private long rowId_;
	}
}
