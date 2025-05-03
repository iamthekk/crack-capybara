using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Social
{
	public sealed class InteractDetailResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<InteractDetailResponse> Parser
		{
			get
			{
				return InteractDetailResponse._parser;
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
		public PVPRecordDto Record
		{
			get
			{
				return this.record_;
			}
			set
			{
				this.record_ = value;
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
			if (this.RowId != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.RowId);
			}
			if (this.record_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.Record);
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
			if (this.RowId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.RowId);
			}
			if (this.record_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Record);
			}
			return num;
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
						this.RowId = input.ReadInt64();
						continue;
					}
					if (num == 34U)
					{
						if (this.record_ == null)
						{
							this.record_ = new PVPRecordDto();
						}
						input.ReadMessage(this.record_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<InteractDetailResponse> _parser = new MessageParser<InteractDetailResponse>(() => new InteractDetailResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RowIdFieldNumber = 3;

		private long rowId_;

		public const int RecordFieldNumber = 4;

		private PVPRecordDto record_;
	}
}
