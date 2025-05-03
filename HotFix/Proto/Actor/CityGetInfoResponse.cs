using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Actor
{
	public sealed class CityGetInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CityGetInfoResponse> Parser
		{
			get
			{
				return CityGetInfoResponse._parser;
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
		public LordDto Extra
		{
			get
			{
				return this.extra_;
			}
			set
			{
				this.extra_ = value;
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
			if (this.extra_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.Extra);
			}
			if (this.SlaveCount != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.SlaveCount);
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
			if (this.extra_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Extra);
			}
			if (this.SlaveCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SlaveCount);
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
					if (num == 26U)
					{
						if (this.extra_ == null)
						{
							this.extra_ = new LordDto();
						}
						input.ReadMessage(this.extra_);
						continue;
					}
					if (num == 32U)
					{
						this.SlaveCount = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<CityGetInfoResponse> _parser = new MessageParser<CityGetInfoResponse>(() => new CityGetInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ExtraFieldNumber = 3;

		private LordDto extra_;

		public const int SlaveCountFieldNumber = 4;

		private uint slaveCount_;
	}
}
