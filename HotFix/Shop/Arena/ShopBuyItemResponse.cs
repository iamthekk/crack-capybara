using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class ShopBuyItemResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopBuyItemResponse> Parser
		{
			get
			{
				return ShopBuyItemResponse._parser;
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
		public AdDataDto AdData
		{
			get
			{
				return this.adData_;
			}
			set
			{
				this.adData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ShopDrawDto ShopDrawDto
		{
			get
			{
				return this.shopDrawDto_;
			}
			set
			{
				this.shopDrawDto_ = value;
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
			if (this.adData_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.AdData);
			}
			if (this.shopDrawDto_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.ShopDrawDto);
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
			if (this.adData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.AdData);
			}
			if (this.shopDrawDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.ShopDrawDto);
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
						if (this.adData_ == null)
						{
							this.adData_ = new AdDataDto();
						}
						input.ReadMessage(this.adData_);
						continue;
					}
					if (num == 34U)
					{
						if (this.shopDrawDto_ == null)
						{
							this.shopDrawDto_ = new ShopDrawDto();
						}
						input.ReadMessage(this.shopDrawDto_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ShopBuyItemResponse> _parser = new MessageParser<ShopBuyItemResponse>(() => new ShopBuyItemResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int AdDataFieldNumber = 3;

		private AdDataDto adData_;

		public const int ShopDrawDtoFieldNumber = 4;

		private ShopDrawDto shopDrawDto_;
	}
}
