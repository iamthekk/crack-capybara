using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Pay
{
	public sealed class PayPreOrderRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PayPreOrderRequest> Parser
		{
			get
			{
				return PayPreOrderRequest._parser;
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
		public string KuaiShouChannelID
		{
			get
			{
				return this.kuaiShouChannelID_;
			}
			set
			{
				this.kuaiShouChannelID_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint YybType
		{
			get
			{
				return this.yybType_;
			}
			set
			{
				this.yybType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string YybOpenId
		{
			get
			{
				return this.yybOpenId_;
			}
			set
			{
				this.yybOpenId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string YybOpenkey
		{
			get
			{
				return this.yybOpenkey_;
			}
			set
			{
				this.yybOpenkey_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string YybPf
		{
			get
			{
				return this.yybPf_;
			}
			set
			{
				this.yybPf_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string YybPfKey
		{
			get
			{
				return this.yybPfKey_;
			}
			set
			{
				this.yybPfKey_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string DouyinRiskControlInfo
		{
			get
			{
				return this.douyinRiskControlInfo_;
			}
			set
			{
				this.douyinRiskControlInfo_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string ChannelParams
		{
			get
			{
				return this.channelParams_;
			}
			set
			{
				this.channelParams_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Custom
		{
			get
			{
				return this.custom_;
			}
			set
			{
				this.custom_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.ProductId.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.ProductId);
			}
			if (this.ExtraInfo.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.ExtraInfo);
			}
			if (this.PreOrderId != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.PreOrderId);
			}
			if (this.ExtraType != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.ExtraType);
			}
			if (this.ChannelId != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.ChannelId);
			}
			if (this.KuaiShouChannelID.Length != 0)
			{
				output.WriteRawTag(58);
				output.WriteString(this.KuaiShouChannelID);
			}
			if (this.YybType != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.YybType);
			}
			if (this.YybOpenId.Length != 0)
			{
				output.WriteRawTag(74);
				output.WriteString(this.YybOpenId);
			}
			if (this.YybOpenkey.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.YybOpenkey);
			}
			if (this.YybPf.Length != 0)
			{
				output.WriteRawTag(90);
				output.WriteString(this.YybPf);
			}
			if (this.YybPfKey.Length != 0)
			{
				output.WriteRawTag(98);
				output.WriteString(this.YybPfKey);
			}
			if (this.DouyinRiskControlInfo.Length != 0)
			{
				output.WriteRawTag(106);
				output.WriteString(this.DouyinRiskControlInfo);
			}
			if (this.ChannelParams.Length != 0)
			{
				output.WriteRawTag(114);
				output.WriteString(this.ChannelParams);
			}
			if (this.Custom.Length != 0)
			{
				output.WriteRawTag(122);
				output.WriteString(this.Custom);
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
			if (this.ProductId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ProductId);
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
			if (this.KuaiShouChannelID.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.KuaiShouChannelID);
			}
			if (this.YybType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.YybType);
			}
			if (this.YybOpenId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.YybOpenId);
			}
			if (this.YybOpenkey.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.YybOpenkey);
			}
			if (this.YybPf.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.YybPf);
			}
			if (this.YybPfKey.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.YybPfKey);
			}
			if (this.DouyinRiskControlInfo.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.DouyinRiskControlInfo);
			}
			if (this.ChannelParams.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ChannelParams);
			}
			if (this.Custom.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Custom);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 58U)
				{
					if (num <= 26U)
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
						if (num == 18U)
						{
							this.ProductId = input.ReadString();
							continue;
						}
						if (num == 26U)
						{
							this.ExtraInfo = input.ReadString();
							continue;
						}
					}
					else if (num <= 40U)
					{
						if (num == 32U)
						{
							this.PreOrderId = input.ReadUInt64();
							continue;
						}
						if (num == 40U)
						{
							this.ExtraType = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 48U)
						{
							this.ChannelId = input.ReadUInt32();
							continue;
						}
						if (num == 58U)
						{
							this.KuaiShouChannelID = input.ReadString();
							continue;
						}
					}
				}
				else if (num <= 90U)
				{
					if (num <= 74U)
					{
						if (num == 64U)
						{
							this.YybType = input.ReadUInt32();
							continue;
						}
						if (num == 74U)
						{
							this.YybOpenId = input.ReadString();
							continue;
						}
					}
					else
					{
						if (num == 82U)
						{
							this.YybOpenkey = input.ReadString();
							continue;
						}
						if (num == 90U)
						{
							this.YybPf = input.ReadString();
							continue;
						}
					}
				}
				else if (num <= 106U)
				{
					if (num == 98U)
					{
						this.YybPfKey = input.ReadString();
						continue;
					}
					if (num == 106U)
					{
						this.DouyinRiskControlInfo = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 114U)
					{
						this.ChannelParams = input.ReadString();
						continue;
					}
					if (num == 122U)
					{
						this.Custom = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<PayPreOrderRequest> _parser = new MessageParser<PayPreOrderRequest>(() => new PayPreOrderRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ProductIdFieldNumber = 2;

		private string productId_ = "";

		public const int ExtraInfoFieldNumber = 3;

		private string extraInfo_ = "";

		public const int PreOrderIdFieldNumber = 4;

		private ulong preOrderId_;

		public const int ExtraTypeFieldNumber = 5;

		private uint extraType_;

		public const int ChannelIdFieldNumber = 6;

		private uint channelId_;

		public const int KuaiShouChannelIDFieldNumber = 7;

		private string kuaiShouChannelID_ = "";

		public const int YybTypeFieldNumber = 8;

		private uint yybType_;

		public const int YybOpenIdFieldNumber = 9;

		private string yybOpenId_ = "";

		public const int YybOpenkeyFieldNumber = 10;

		private string yybOpenkey_ = "";

		public const int YybPfFieldNumber = 11;

		private string yybPf_ = "";

		public const int YybPfKeyFieldNumber = 12;

		private string yybPfKey_ = "";

		public const int DouyinRiskControlInfoFieldNumber = 13;

		private string douyinRiskControlInfo_ = "";

		public const int ChannelParamsFieldNumber = 14;

		private string channelParams_ = "";

		public const int CustomFieldNumber = 15;

		private string custom_ = "";
	}
}
