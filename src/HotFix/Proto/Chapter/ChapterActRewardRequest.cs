using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class ChapterActRewardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChapterActRewardRequest> Parser
		{
			get
			{
				return ChapterActRewardRequest._parser;
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
		public RepeatedField<ulong> RowIds
		{
			get
			{
				return this.rowIds_;
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
			this.rowIds_.WriteTo(output, ChapterActRewardRequest._repeated_rowIds_codec);
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
			return num + this.rowIds_.CalculateSize(ChapterActRewardRequest._repeated_rowIds_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 10U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 16U)
					{
						this.Day = input.ReadInt32();
						continue;
					}
				}
				else if (num == 24U || num == 26U)
				{
					this.rowIds_.AddEntriesFrom(input, ChapterActRewardRequest._repeated_rowIds_codec);
					continue;
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChapterActRewardRequest> _parser = new MessageParser<ChapterActRewardRequest>(() => new ChapterActRewardRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int DayFieldNumber = 2;

		private int day_;

		public const int RowIdsFieldNumber = 3;

		private static readonly FieldCodec<ulong> _repeated_rowIds_codec = FieldCodec.ForUInt64(26U);

		private readonly RepeatedField<ulong> rowIds_ = new RepeatedField<ulong>();
	}
}
