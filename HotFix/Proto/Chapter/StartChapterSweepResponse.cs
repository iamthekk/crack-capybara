using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class StartChapterSweepResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<StartChapterSweepResponse> Parser
		{
			get
			{
				return StartChapterSweepResponse._parser;
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
		public int ChapterSeed
		{
			get
			{
				return this.chapterSeed_;
			}
			set
			{
				this.chapterSeed_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, EventDetail> EventMap
		{
			get
			{
				return this.eventMap_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, EventDetail> ActiveMap
		{
			get
			{
				return this.activeMap_;
			}
		}

		[DebuggerNonUserCode]
		public uint Rate
		{
			get
			{
				return this.rate_;
			}
			set
			{
				this.rate_ = value;
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
			if (this.ChapterSeed != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.ChapterSeed);
			}
			this.eventMap_.WriteTo(output, StartChapterSweepResponse._map_eventMap_codec);
			this.activeMap_.WriteTo(output, StartChapterSweepResponse._map_activeMap_codec);
			if (this.Rate != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.Rate);
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
			if (this.ChapterSeed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ChapterSeed);
			}
			num += this.eventMap_.CalculateSize(StartChapterSweepResponse._map_eventMap_codec);
			num += this.activeMap_.CalculateSize(StartChapterSweepResponse._map_activeMap_codec);
			if (this.Rate != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Rate);
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
							this.ChapterId = input.ReadInt32();
							continue;
						}
						if (num == 32U)
						{
							this.WaveIndex = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 50U)
				{
					if (num == 40U)
					{
						this.ChapterSeed = input.ReadInt32();
						continue;
					}
					if (num == 50U)
					{
						this.eventMap_.AddEntriesFrom(input, StartChapterSweepResponse._map_eventMap_codec);
						continue;
					}
				}
				else
				{
					if (num == 58U)
					{
						this.activeMap_.AddEntriesFrom(input, StartChapterSweepResponse._map_activeMap_codec);
						continue;
					}
					if (num == 64U)
					{
						this.Rate = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<StartChapterSweepResponse> _parser = new MessageParser<StartChapterSweepResponse>(() => new StartChapterSweepResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ChapterIdFieldNumber = 3;

		private int chapterId_;

		public const int WaveIndexFieldNumber = 4;

		private int waveIndex_;

		public const int ChapterSeedFieldNumber = 5;

		private int chapterSeed_;

		public const int EventMapFieldNumber = 6;

		private static readonly MapField<uint, EventDetail>.Codec _map_eventMap_codec = new MapField<uint, EventDetail>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForMessage<EventDetail>(18U, EventDetail.Parser), 50U);

		private readonly MapField<uint, EventDetail> eventMap_ = new MapField<uint, EventDetail>();

		public const int ActiveMapFieldNumber = 7;

		private static readonly MapField<uint, EventDetail>.Codec _map_activeMap_codec = new MapField<uint, EventDetail>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForMessage<EventDetail>(18U, EventDetail.Parser), 58U);

		private readonly MapField<uint, EventDetail> activeMap_ = new MapField<uint, EventDetail>();

		public const int RateFieldNumber = 8;

		private uint rate_;
	}
}
