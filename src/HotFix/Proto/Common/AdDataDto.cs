using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class AdDataDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<AdDataDto> Parser
		{
			get
			{
				return AdDataDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, uint> AdDataMap
		{
			get
			{
				return this.adDataMap_;
			}
		}

		[DebuggerNonUserCode]
		public ulong ResetTime
		{
			get
			{
				return this.resetTime_;
			}
			set
			{
				this.resetTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, ulong> LastAdvTime
		{
			get
			{
				return this.lastAdvTime_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.adDataMap_.WriteTo(output, AdDataDto._map_adDataMap_codec);
			if (this.ResetTime != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.ResetTime);
			}
			this.lastAdvTime_.WriteTo(output, AdDataDto._map_lastAdvTime_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.adDataMap_.CalculateSize(AdDataDto._map_adDataMap_codec);
			if (this.ResetTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.ResetTime);
			}
			return num + this.lastAdvTime_.CalculateSize(AdDataDto._map_lastAdvTime_codec);
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
							this.lastAdvTime_.AddEntriesFrom(input, AdDataDto._map_lastAdvTime_codec);
						}
					}
					else
					{
						this.ResetTime = input.ReadUInt64();
					}
				}
				else
				{
					this.adDataMap_.AddEntriesFrom(input, AdDataDto._map_adDataMap_codec);
				}
			}
		}

		private static readonly MessageParser<AdDataDto> _parser = new MessageParser<AdDataDto>(() => new AdDataDto());

		public const int AdDataMapFieldNumber = 1;

		private static readonly MapField<uint, uint>.Codec _map_adDataMap_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 10U);

		private readonly MapField<uint, uint> adDataMap_ = new MapField<uint, uint>();

		public const int ResetTimeFieldNumber = 2;

		private ulong resetTime_;

		public const int LastAdvTimeFieldNumber = 3;

		private static readonly MapField<uint, ulong>.Codec _map_lastAdvTime_codec = new MapField<uint, ulong>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt64(16U), 26U);

		private readonly MapField<uint, ulong> lastAdvTime_ = new MapField<uint, ulong>();
	}
}
