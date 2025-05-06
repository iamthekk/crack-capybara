using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class IntegralShopItemDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<IntegralShopItemDto> Parser
		{
			get
			{
				return IntegralShopItemDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Type
		{
			get
			{
				return this.type_;
			}
			set
			{
				this.type_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RewardDto Reward
		{
			get
			{
				return this.reward_;
			}
			set
			{
				this.reward_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint PriceType
		{
			get
			{
				return this.priceType_;
			}
			set
			{
				this.priceType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Price
		{
			get
			{
				return this.price_;
			}
			set
			{
				this.price_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Discount
		{
			get
			{
				return this.discount_;
			}
			set
			{
				this.discount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Sort
		{
			get
			{
				return this.sort_;
			}
			set
			{
				this.sort_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ConfigId != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.ConfigId);
			}
			if (this.Type != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Type);
			}
			if (this.reward_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.Reward);
			}
			if (this.PriceType != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.PriceType);
			}
			if (this.Price != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.Price);
			}
			if (this.Discount != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.Discount);
			}
			if (this.Sort != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.Sort);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ConfigId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ConfigId);
			}
			if (this.Type != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Type);
			}
			if (this.reward_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Reward);
			}
			if (this.PriceType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.PriceType);
			}
			if (this.Price != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Price);
			}
			if (this.Discount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Discount);
			}
			if (this.Sort != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Sort);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 26U)
				{
					if (num == 8U)
					{
						this.ConfigId = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.Type = input.ReadUInt32();
						continue;
					}
					if (num == 26U)
					{
						if (this.reward_ == null)
						{
							this.reward_ = new RewardDto();
						}
						input.ReadMessage(this.reward_);
						continue;
					}
				}
				else if (num <= 40U)
				{
					if (num == 32U)
					{
						this.PriceType = input.ReadUInt32();
						continue;
					}
					if (num == 40U)
					{
						this.Price = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 48U)
					{
						this.Discount = input.ReadUInt32();
						continue;
					}
					if (num == 56U)
					{
						this.Sort = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<IntegralShopItemDto> _parser = new MessageParser<IntegralShopItemDto>(() => new IntegralShopItemDto());

		public const int ConfigIdFieldNumber = 1;

		private uint configId_;

		public const int TypeFieldNumber = 2;

		private uint type_;

		public const int RewardFieldNumber = 3;

		private RewardDto reward_;

		public const int PriceTypeFieldNumber = 4;

		private uint priceType_;

		public const int PriceFieldNumber = 5;

		private uint price_;

		public const int DiscountFieldNumber = 6;

		private uint discount_;

		public const int SortFieldNumber = 7;

		private uint sort_;
	}
}
