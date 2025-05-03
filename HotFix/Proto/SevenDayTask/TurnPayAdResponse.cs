using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.SevenDayTask
{
	public sealed class TurnPayAdResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TurnPayAdResponse> Parser
		{
			get
			{
				return TurnPayAdResponse._parser;
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
		public MapField<int, int> TurntablePayCount
		{
			get
			{
				return this.turntablePayCount_;
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
			this.turntablePayCount_.WriteTo(output, TurnPayAdResponse._map_turntablePayCount_codec);
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
			return num + this.turntablePayCount_.CalculateSize(TurnPayAdResponse._map_turntablePayCount_codec);
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
						if (num != 138U)
						{
							input.SkipLastField();
						}
						else
						{
							this.turntablePayCount_.AddEntriesFrom(input, TurnPayAdResponse._map_turntablePayCount_codec);
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

		private static readonly MessageParser<TurnPayAdResponse> _parser = new MessageParser<TurnPayAdResponse>(() => new TurnPayAdResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int TurntablePayCountFieldNumber = 17;

		private static readonly MapField<int, int>.Codec _map_turntablePayCount_codec = new MapField<int, int>.Codec(FieldCodec.ForInt32(8U), FieldCodec.ForInt32(16U), 138U);

		private readonly MapField<int, int> turntablePayCount_ = new MapField<int, int>();
	}
}
