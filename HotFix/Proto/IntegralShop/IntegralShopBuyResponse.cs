using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.IntegralShop
{
	public sealed class IntegralShopBuyResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<IntegralShopBuyResponse> Parser
		{
			get
			{
				return IntegralShopBuyResponse._parser;
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
		public int ShopConfigId
		{
			get
			{
				return this.shopConfigId_;
			}
			set
			{
				this.shopConfigId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int GoodsConfigId
		{
			get
			{
				return this.goodsConfigId_;
			}
			set
			{
				this.goodsConfigId_ = value;
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
			if (this.ShopConfigId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.ShopConfigId);
			}
			if (this.GoodsConfigId != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.GoodsConfigId);
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
			if (this.ShopConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ShopConfigId);
			}
			if (this.GoodsConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.GoodsConfigId);
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
						this.ShopConfigId = input.ReadInt32();
						continue;
					}
					if (num == 32U)
					{
						this.GoodsConfigId = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<IntegralShopBuyResponse> _parser = new MessageParser<IntegralShopBuyResponse>(() => new IntegralShopBuyResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ShopConfigIdFieldNumber = 3;

		private int shopConfigId_;

		public const int GoodsConfigIdFieldNumber = 4;

		private int goodsConfigId_;
	}
}
