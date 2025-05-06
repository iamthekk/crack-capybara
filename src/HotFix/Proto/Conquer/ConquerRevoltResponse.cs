using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Conquer
{
	public sealed class ConquerRevoltResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ConquerRevoltResponse> Parser
		{
			get
			{
				return ConquerRevoltResponse._parser;
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
		public long UserId
		{
			get
			{
				return this.userId_;
			}
			set
			{
				this.userId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public LordDto Lord
		{
			get
			{
				return this.lord_;
			}
			set
			{
				this.lord_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint SlaveCount
		{
			get
			{
				return this.slaveCount_;
			}
			set
			{
				this.slaveCount_ = value;
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
			if (this.UserId != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.UserId);
			}
			if (this.lord_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.Lord);
			}
			if (this.SlaveCount != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.SlaveCount);
			}
			if (this.record_ != null)
			{
				output.WriteRawTag(50);
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
			if (this.UserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId);
			}
			if (this.lord_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Lord);
			}
			if (this.SlaveCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SlaveCount);
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
				if (num <= 24U)
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
					if (num == 24U)
					{
						this.UserId = input.ReadInt64();
						continue;
					}
				}
				else
				{
					if (num == 34U)
					{
						if (this.lord_ == null)
						{
							this.lord_ = new LordDto();
						}
						input.ReadMessage(this.lord_);
						continue;
					}
					if (num == 40U)
					{
						this.SlaveCount = input.ReadUInt32();
						continue;
					}
					if (num == 50U)
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

		private static readonly MessageParser<ConquerRevoltResponse> _parser = new MessageParser<ConquerRevoltResponse>(() => new ConquerRevoltResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int UserIdFieldNumber = 3;

		private long userId_;

		public const int LordFieldNumber = 4;

		private LordDto lord_;

		public const int SlaveCountFieldNumber = 5;

		private uint slaveCount_;

		public const int RecordFieldNumber = 6;

		private PVPRecordDto record_;
	}
}
