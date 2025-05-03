using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Pay
{
	public sealed class PayInAppPurchaseRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PayInAppPurchaseRequest> Parser
		{
			get
			{
				return PayInAppPurchaseRequest._parser;
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
		public uint PlatformIndex
		{
			get
			{
				return this.platformIndex_;
			}
			set
			{
				this.platformIndex_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string ProductId
		{
			get
			{
				return this.productId_;
			}
			set
			{
				this.productId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string ReceiptData
		{
			get
			{
				return this.receiptData_;
			}
			set
			{
				this.receiptData_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string ExtraInfo
		{
			get
			{
				return this.extraInfo_;
			}
			set
			{
				this.extraInfo_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public ulong PreOrderId
		{
			get
			{
				return this.preOrderId_;
			}
			set
			{
				this.preOrderId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ExtraType
		{
			get
			{
				return this.extraType_;
			}
			set
			{
				this.extraType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ChannelId
		{
			get
			{
				return this.channelId_;
			}
			set
			{
				this.channelId_ = value;
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
			if (this.PlatformIndex != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.PlatformIndex);
			}
			if (this.ProductId.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.ProductId);
			}
			if (this.ReceiptData.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.ReceiptData);
			}
			if (this.ExtraInfo.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(this.ExtraInfo);
			}
			if (this.PreOrderId != 0UL)
			{
				output.WriteRawTag(48);
				output.WriteUInt64(this.PreOrderId);
			}
			if (this.ExtraType != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.ExtraType);
			}
			if (this.ChannelId != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.ChannelId);
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
			if (this.PlatformIndex != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.PlatformIndex);
			}
			if (this.ProductId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ProductId);
			}
			if (this.ReceiptData.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ReceiptData);
			}
			if (this.ExtraInfo.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ExtraInfo);
			}
			if (this.PreOrderId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.PreOrderId);
			}
			if (this.ExtraType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ExtraType);
			}
			if (this.ChannelId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ChannelId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 34U)
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
							this.PlatformIndex = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 26U)
						{
							this.ProductId = input.ReadString();
							continue;
						}
						if (num == 34U)
						{
							this.ReceiptData = input.ReadString();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 42U)
					{
						this.ExtraInfo = input.ReadString();
						continue;
					}
					if (num == 48U)
					{
						this.PreOrderId = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 56U)
					{
						this.ExtraType = input.ReadUInt32();
						continue;
					}
					if (num == 64U)
					{
						this.ChannelId = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<PayInAppPurchaseRequest> _parser = new MessageParser<PayInAppPurchaseRequest>(() => new PayInAppPurchaseRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int PlatformIndexFieldNumber = 2;

		private uint platformIndex_;

		public const int ProductIdFieldNumber = 3;

		private string productId_ = "";

		public const int ReceiptDataFieldNumber = 4;

		private string receiptData_ = "";

		public const int ExtraInfoFieldNumber = 5;

		private string extraInfo_ = "";

		public const int PreOrderIdFieldNumber = 6;

		private ulong preOrderId_;

		public const int ExtraTypeFieldNumber = 7;

		private uint extraType_;

		public const int ChannelIdFieldNumber = 8;

		private uint channelId_;
	}
}
