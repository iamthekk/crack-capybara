using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class ChapterActRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChapterActRewardResponse> Parser
		{
			get
			{
				return ChapterActRewardResponse._parser;
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
		public MapField<ulong, uint> Score
		{
			get
			{
				return this.score_;
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
			if (this.Day != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Day);
			}
			this.score_.WriteTo(output, ChapterActRewardResponse._map_score_codec);
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
			if (this.Day != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Day);
			}
			return num + this.score_.CalculateSize(ChapterActRewardResponse._map_score_codec);
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
					if (num == 24U)
					{
						this.Day = input.ReadInt32();
						continue;
					}
					if (num == 34U)
					{
						this.score_.AddEntriesFrom(input, ChapterActRewardResponse._map_score_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChapterActRewardResponse> _parser = new MessageParser<ChapterActRewardResponse>(() => new ChapterActRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int DayFieldNumber = 3;

		private int day_;

		public const int ScoreFieldNumber = 4;

		private static readonly MapField<ulong, uint>.Codec _map_score_codec = new MapField<ulong, uint>.Codec(FieldCodec.ForUInt64(8U), FieldCodec.ForUInt32(16U), 34U);

		private readonly MapField<ulong, uint> score_ = new MapField<ulong, uint>();
	}
}
