using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class PushIapDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PushIapDto> Parser
		{
			get
			{
				return PushIapDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<PushIapItemDto> PushIapItemDto
		{
			get
			{
				return this.pushIapItemDto_;
			}
		}

		[DebuggerNonUserCode]
		public long DayResetTime
		{
			get
			{
				return this.dayResetTime_;
			}
			set
			{
				this.dayResetTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, uint> DayPayTimes
		{
			get
			{
				return this.dayPayTimes_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.pushIapItemDto_.WriteTo(output, PushIapDto._repeated_pushIapItemDto_codec);
			if (this.DayResetTime != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.DayResetTime);
			}
			this.dayPayTimes_.WriteTo(output, PushIapDto._map_dayPayTimes_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.pushIapItemDto_.CalculateSize(PushIapDto._repeated_pushIapItemDto_codec);
			if (this.DayResetTime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.DayResetTime);
			}
			return num + this.dayPayTimes_.CalculateSize(PushIapDto._map_dayPayTimes_codec);
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
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							this.dayPayTimes_.AddEntriesFrom(input, PushIapDto._map_dayPayTimes_codec);
						}
					}
					else
					{
						this.DayResetTime = input.ReadInt64();
					}
				}
				else
				{
					this.pushIapItemDto_.AddEntriesFrom(input, PushIapDto._repeated_pushIapItemDto_codec);
				}
			}
		}

		private static readonly MessageParser<PushIapDto> _parser = new MessageParser<PushIapDto>(() => new PushIapDto());

		public const int PushIapItemDtoFieldNumber = 1;

		private static readonly FieldCodec<PushIapItemDto> _repeated_pushIapItemDto_codec = FieldCodec.ForMessage<PushIapItemDto>(10U, Proto.Common.PushIapItemDto.Parser);

		private readonly RepeatedField<PushIapItemDto> pushIapItemDto_ = new RepeatedField<PushIapItemDto>();

		public const int DayResetTimeFieldNumber = 2;

		private long dayResetTime_;

		public const int DayPayTimesFieldNumber = 3;

		private static readonly MapField<uint, uint>.Codec _map_dayPayTimes_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 26U);

		private readonly MapField<uint, uint> dayPayTimes_ = new MapField<uint, uint>();
	}
}
