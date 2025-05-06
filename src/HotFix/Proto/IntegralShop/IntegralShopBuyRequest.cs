using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.IntegralShop
{
	public sealed class IntegralShopBuyRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<IntegralShopBuyRequest> Parser
		{
			get
			{
				return IntegralShopBuyRequest._parser;
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
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.CommonParams);
			}
			if (this.ShopConfigId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ShopConfigId);
			}
			if (this.GoodsConfigId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.GoodsConfigId);
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
				if (num != 10U)
				{
					if (num != 16U)
					{
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.GoodsConfigId = input.ReadInt32();
						}
					}
					else
					{
						this.ShopConfigId = input.ReadInt32();
					}
				}
				else
				{
					if (this.commonParams_ == null)
					{
						this.commonParams_ = new CommonParams();
					}
					input.ReadMessage(this.commonParams_);
				}
			}
		}

		private static readonly MessageParser<IntegralShopBuyRequest> _parser = new MessageParser<IntegralShopBuyRequest>(() => new IntegralShopBuyRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ShopConfigIdFieldNumber = 2;

		private int shopConfigId_;

		public const int GoodsConfigIdFieldNumber = 3;

		private int goodsConfigId_;
	}
}
