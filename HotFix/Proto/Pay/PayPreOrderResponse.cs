using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Pay
{
	public sealed class PayPreOrderResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PayPreOrderResponse> Parser
		{
			get
			{
				return PayPreOrderResponse._parser;
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
		public string NotifyUrl
		{
			get
			{
				return this.notifyUrl_;
			}
			set
			{
				this.notifyUrl_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public WeChatOrderDto WeChatOrderDto
		{
			get
			{
				return this.weChatOrderDto_;
			}
			set
			{
				this.weChatOrderDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string AliOrderData
		{
			get
			{
				return this.aliOrderData_;
			}
			set
			{
				this.aliOrderData_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public UcOrderDto UcOrderDto
		{
			get
			{
				return this.ucOrderDto_;
			}
			set
			{
				this.ucOrderDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public BiliOrderDto BiliOrderDto
		{
			get
			{
				return this.biliOrderDto_;
			}
			set
			{
				this.biliOrderDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string PassBackParams
		{
			get
			{
				return this.passBackParams_;
			}
			set
			{
				this.passBackParams_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public KuaiShouOrderDto KusiShouOrderDto
		{
			get
			{
				return this.kusiShouOrderDto_;
			}
			set
			{
				this.kusiShouOrderDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string CpOrderId
		{
			get
			{
				return this.cpOrderId_;
			}
			set
			{
				this.cpOrderId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint Amount
		{
			get
			{
				return this.amount_;
			}
			set
			{
				this.amount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public VivoOrderDto VivoOrderDto
		{
			get
			{
				return this.vivoOrderDto_;
			}
			set
			{
				this.vivoOrderDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public YybOrderDto YybOrderDto
		{
			get
			{
				return this.yybOrderDto_;
			}
			set
			{
				this.yybOrderDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public DouYinOrderDto DouYinOrderDto
		{
			get
			{
				return this.douYinOrderDto_;
			}
			set
			{
				this.douYinOrderDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public WeChatMiniGameOrderDto WeChatMiniGameOrderDto
		{
			get
			{
				return this.weChatMiniGameOrderDto_;
			}
			set
			{
				this.weChatMiniGameOrderDto_ = value;
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
			if (this.PreOrderId != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.PreOrderId);
			}
			if (this.NotifyUrl.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.NotifyUrl);
			}
			if (this.weChatOrderDto_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.WeChatOrderDto);
			}
			if (this.AliOrderData.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(this.AliOrderData);
			}
			if (this.ucOrderDto_ != null)
			{
				output.WriteRawTag(50);
				output.WriteMessage(this.UcOrderDto);
			}
			if (this.biliOrderDto_ != null)
			{
				output.WriteRawTag(58);
				output.WriteMessage(this.BiliOrderDto);
			}
			if (this.PassBackParams.Length != 0)
			{
				output.WriteRawTag(66);
				output.WriteString(this.PassBackParams);
			}
			if (this.kusiShouOrderDto_ != null)
			{
				output.WriteRawTag(74);
				output.WriteMessage(this.KusiShouOrderDto);
			}
			if (this.CpOrderId.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.CpOrderId);
			}
			if (this.Amount != 0U)
			{
				output.WriteRawTag(88);
				output.WriteUInt32(this.Amount);
			}
			if (this.vivoOrderDto_ != null)
			{
				output.WriteRawTag(98);
				output.WriteMessage(this.VivoOrderDto);
			}
			if (this.yybOrderDto_ != null)
			{
				output.WriteRawTag(106);
				output.WriteMessage(this.YybOrderDto);
			}
			if (this.douYinOrderDto_ != null)
			{
				output.WriteRawTag(114);
				output.WriteMessage(this.DouYinOrderDto);
			}
			if (this.weChatMiniGameOrderDto_ != null)
			{
				output.WriteRawTag(122);
				output.WriteMessage(this.WeChatMiniGameOrderDto);
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
			if (this.PreOrderId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.PreOrderId);
			}
			if (this.NotifyUrl.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.NotifyUrl);
			}
			if (this.weChatOrderDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.WeChatOrderDto);
			}
			if (this.AliOrderData.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AliOrderData);
			}
			if (this.ucOrderDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UcOrderDto);
			}
			if (this.biliOrderDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.BiliOrderDto);
			}
			if (this.PassBackParams.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.PassBackParams);
			}
			if (this.kusiShouOrderDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.KusiShouOrderDto);
			}
			if (this.CpOrderId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.CpOrderId);
			}
			if (this.Amount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Amount);
			}
			if (this.vivoOrderDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.VivoOrderDto);
			}
			if (this.yybOrderDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.YybOrderDto);
			}
			if (this.douYinOrderDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.DouYinOrderDto);
			}
			if (this.weChatMiniGameOrderDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.WeChatMiniGameOrderDto);
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
						if (num == 8U)
						{
							this.Code = input.ReadInt32();
							continue;
						}
						if (num == 16U)
						{
							this.PreOrderId = input.ReadUInt64();
							continue;
						}
						if (num == 26U)
						{
							this.NotifyUrl = input.ReadString();
							continue;
						}
					}
					else if (num <= 42U)
					{
						if (num == 34U)
						{
							if (this.weChatOrderDto_ == null)
							{
								this.weChatOrderDto_ = new WeChatOrderDto();
							}
							input.ReadMessage(this.weChatOrderDto_);
							continue;
						}
						if (num == 42U)
						{
							this.AliOrderData = input.ReadString();
							continue;
						}
					}
					else
					{
						if (num == 50U)
						{
							if (this.ucOrderDto_ == null)
							{
								this.ucOrderDto_ = new UcOrderDto();
							}
							input.ReadMessage(this.ucOrderDto_);
							continue;
						}
						if (num == 58U)
						{
							if (this.biliOrderDto_ == null)
							{
								this.biliOrderDto_ = new BiliOrderDto();
							}
							input.ReadMessage(this.biliOrderDto_);
							continue;
						}
					}
				}
				else if (num <= 88U)
				{
					if (num <= 74U)
					{
						if (num == 66U)
						{
							this.PassBackParams = input.ReadString();
							continue;
						}
						if (num == 74U)
						{
							if (this.kusiShouOrderDto_ == null)
							{
								this.kusiShouOrderDto_ = new KuaiShouOrderDto();
							}
							input.ReadMessage(this.kusiShouOrderDto_);
							continue;
						}
					}
					else
					{
						if (num == 82U)
						{
							this.CpOrderId = input.ReadString();
							continue;
						}
						if (num == 88U)
						{
							this.Amount = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 106U)
				{
					if (num == 98U)
					{
						if (this.vivoOrderDto_ == null)
						{
							this.vivoOrderDto_ = new VivoOrderDto();
						}
						input.ReadMessage(this.vivoOrderDto_);
						continue;
					}
					if (num == 106U)
					{
						if (this.yybOrderDto_ == null)
						{
							this.yybOrderDto_ = new YybOrderDto();
						}
						input.ReadMessage(this.yybOrderDto_);
						continue;
					}
				}
				else
				{
					if (num == 114U)
					{
						if (this.douYinOrderDto_ == null)
						{
							this.douYinOrderDto_ = new DouYinOrderDto();
						}
						input.ReadMessage(this.douYinOrderDto_);
						continue;
					}
					if (num == 122U)
					{
						if (this.weChatMiniGameOrderDto_ == null)
						{
							this.weChatMiniGameOrderDto_ = new WeChatMiniGameOrderDto();
						}
						input.ReadMessage(this.weChatMiniGameOrderDto_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<PayPreOrderResponse> _parser = new MessageParser<PayPreOrderResponse>(() => new PayPreOrderResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int PreOrderIdFieldNumber = 2;

		private ulong preOrderId_;

		public const int NotifyUrlFieldNumber = 3;

		private string notifyUrl_ = "";

		public const int WeChatOrderDtoFieldNumber = 4;

		private WeChatOrderDto weChatOrderDto_;

		public const int AliOrderDataFieldNumber = 5;

		private string aliOrderData_ = "";

		public const int UcOrderDtoFieldNumber = 6;

		private UcOrderDto ucOrderDto_;

		public const int BiliOrderDtoFieldNumber = 7;

		private BiliOrderDto biliOrderDto_;

		public const int PassBackParamsFieldNumber = 8;

		private string passBackParams_ = "";

		public const int KusiShouOrderDtoFieldNumber = 9;

		private KuaiShouOrderDto kusiShouOrderDto_;

		public const int CpOrderIdFieldNumber = 10;

		private string cpOrderId_ = "";

		public const int AmountFieldNumber = 11;

		private uint amount_;

		public const int VivoOrderDtoFieldNumber = 12;

		private VivoOrderDto vivoOrderDto_;

		public const int YybOrderDtoFieldNumber = 13;

		private YybOrderDto yybOrderDto_;

		public const int DouYinOrderDtoFieldNumber = 14;

		private DouYinOrderDto douYinOrderDto_;

		public const int WeChatMiniGameOrderDtoFieldNumber = 15;

		private WeChatMiniGameOrderDto weChatMiniGameOrderDto_;
	}
}
