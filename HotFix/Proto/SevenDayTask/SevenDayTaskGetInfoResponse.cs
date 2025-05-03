using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.SevenDayTask
{
	public sealed class SevenDayTaskGetInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SevenDayTaskGetInfoResponse> Parser
		{
			get
			{
				return SevenDayTaskGetInfoResponse._parser;
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
		public SevenDayDto SevenDayDto
		{
			get
			{
				return this.sevenDayDto_;
			}
			set
			{
				this.sevenDayDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, uint> FreeRewardRecord
		{
			get
			{
				return this.freeRewardRecord_;
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
			if (this.sevenDayDto_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.SevenDayDto);
			}
			this.freeRewardRecord_.WriteTo(output, SevenDayTaskGetInfoResponse._map_freeRewardRecord_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.sevenDayDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.SevenDayDto);
			}
			return num + this.freeRewardRecord_.CalculateSize(SevenDayTaskGetInfoResponse._map_freeRewardRecord_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 18U)
					{
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							this.freeRewardRecord_.AddEntriesFrom(input, SevenDayTaskGetInfoResponse._map_freeRewardRecord_codec);
						}
					}
					else
					{
						if (this.sevenDayDto_ == null)
						{
							this.sevenDayDto_ = new SevenDayDto();
						}
						input.ReadMessage(this.sevenDayDto_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<SevenDayTaskGetInfoResponse> _parser = new MessageParser<SevenDayTaskGetInfoResponse>(() => new SevenDayTaskGetInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int SevenDayDtoFieldNumber = 2;

		private SevenDayDto sevenDayDto_;

		public const int FreeRewardRecordFieldNumber = 3;

		private static readonly MapField<uint, uint>.Codec _map_freeRewardRecord_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 26U);

		private readonly MapField<uint, uint> freeRewardRecord_ = new MapField<uint, uint>();
	}
}
