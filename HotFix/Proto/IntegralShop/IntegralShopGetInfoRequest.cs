using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.IntegralShop
{
	public sealed class IntegralShopGetInfoRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<IntegralShopGetInfoRequest> Parser
		{
			get
			{
				return IntegralShopGetInfoRequest._parser;
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
						input.SkipLastField();
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

		private static readonly MessageParser<IntegralShopGetInfoRequest> _parser = new MessageParser<IntegralShopGetInfoRequest>(() => new IntegralShopGetInfoRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ShopConfigIdFieldNumber = 2;

		private int shopConfigId_;
	}
}
