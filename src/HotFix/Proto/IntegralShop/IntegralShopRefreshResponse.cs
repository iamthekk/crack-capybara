using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.IntegralShop
{
	public sealed class IntegralShopRefreshResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<IntegralShopRefreshResponse> Parser
		{
			get
			{
				return IntegralShopRefreshResponse._parser;
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
		public IntegralShopDto Shop
		{
			get
			{
				return this.shop_;
			}
			set
			{
				this.shop_ = value;
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
			if (this.shop_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.Shop);
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
			if (this.shop_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Shop);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 18U)
					{
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							if (this.shop_ == null)
							{
								this.shop_ = new IntegralShopDto();
							}
							input.ReadMessage(this.shop_);
						}
					}
					else
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<IntegralShopRefreshResponse> _parser = new MessageParser<IntegralShopRefreshResponse>(() => new IntegralShopRefreshResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ShopFieldNumber = 3;

		private IntegralShopDto shop_;
	}
}
