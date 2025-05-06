using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.CrossArena
{
	public sealed class CrossArenaRecordResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CrossArenaRecordResponse> Parser
		{
			get
			{
				return CrossArenaRecordResponse._parser;
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
		public RepeatedField<CrossArenaRecordDto> Records
		{
			get
			{
				return this.records_;
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
			this.records_.WriteTo(output, CrossArenaRecordResponse._repeated_records_codec);
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
			return num + this.records_.CalculateSize(CrossArenaRecordResponse._repeated_records_codec);
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
						if (num != 34U)
						{
							input.SkipLastField();
						}
						else
						{
							this.records_.AddEntriesFrom(input, CrossArenaRecordResponse._repeated_records_codec);
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

		private static readonly MessageParser<CrossArenaRecordResponse> _parser = new MessageParser<CrossArenaRecordResponse>(() => new CrossArenaRecordResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RecordsFieldNumber = 4;

		private static readonly FieldCodec<CrossArenaRecordDto> _repeated_records_codec = FieldCodec.ForMessage<CrossArenaRecordDto>(34U, CrossArenaRecordDto.Parser);

		private readonly RepeatedField<CrossArenaRecordDto> records_ = new RepeatedField<CrossArenaRecordDto>();
	}
}
