using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Develop
{
	public sealed class DevelopChangeResourceRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<DevelopChangeResourceRequest> Parser
		{
			get
			{
				return DevelopChangeResourceRequest._parser;
			}
		}

		[DebuggerNonUserCode]
		public CommonParams CommonParams
		{
			get
			{
				return this.commonParams_;
			}
			set
			{
				this.commonParams_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ResType
		{
			get
			{
				return this.resType_;
			}
			set
			{
				this.resType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long ResNum
		{
			get
			{
				return this.resNum_;
			}
			set
			{
				this.resNum_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ItemId
		{
			get
			{
				return this.itemId_;
			}
			set
			{
				this.itemId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string OtherData
		{
			get
			{
				return this.otherData_;
			}
			set
			{
				this.otherData_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.CommonParams);
			}
			if (this.ResType != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ResType);
			}
			if (this.ResNum != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.ResNum);
			}
			if (this.ItemId != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.ItemId);
			}
			if (this.OtherData.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(this.OtherData);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.ResType != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ResType);
			}
			if (this.ResNum != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.ResNum);
			}
			if (this.ItemId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ItemId);
			}
			if (this.OtherData.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.OtherData);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 10U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 16U)
					{
						this.ResType = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.ResNum = input.ReadInt64();
						continue;
					}
					if (num == 32U)
					{
						this.ItemId = input.ReadInt32();
						continue;
					}
					if (num == 42U)
					{
						this.OtherData = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<DevelopChangeResourceRequest> _parser = new MessageParser<DevelopChangeResourceRequest>(() => new DevelopChangeResourceRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ResTypeFieldNumber = 2;

		private int resType_;

		public const int ResNumFieldNumber = 3;

		private long resNum_;

		public const int ItemIdFieldNumber = 4;

		private int itemId_;

		public const int OtherDataFieldNumber = 5;

		private string otherData_ = "";
	}
}
