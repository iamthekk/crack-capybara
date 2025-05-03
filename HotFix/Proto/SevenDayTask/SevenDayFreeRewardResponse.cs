using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.SevenDayTask
{
	public sealed class SevenDayFreeRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SevenDayFreeRewardResponse> Parser
		{
			get
			{
				return SevenDayFreeRewardResponse._parser;
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
			if (this.commonData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonData);
			}
			this.freeRewardRecord_.WriteTo(output, SevenDayFreeRewardResponse._map_freeRewardRecord_codec);
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
			return num + this.freeRewardRecord_.CalculateSize(SevenDayFreeRewardResponse._map_freeRewardRecord_codec);
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
							this.freeRewardRecord_.AddEntriesFrom(input, SevenDayFreeRewardResponse._map_freeRewardRecord_codec);
						}
					}
					else
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<SevenDayFreeRewardResponse> _parser = new MessageParser<SevenDayFreeRewardResponse>(() => new SevenDayFreeRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int FreeRewardRecordFieldNumber = 3;

		private static readonly MapField<uint, uint>.Codec _map_freeRewardRecord_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 26U);

		private readonly MapField<uint, uint> freeRewardRecord_ = new MapField<uint, uint>();
	}
}
