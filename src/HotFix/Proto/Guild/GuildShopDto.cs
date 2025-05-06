using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildShopDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildShopDto> Parser
		{
			get
			{
				return GuildShopDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint ShopId
		{
			get
			{
				return this.shopId_;
			}
			set
			{
				this.shopId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Position
		{
			get
			{
				return this.position_;
			}
			set
			{
				this.position_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Count
		{
			get
			{
				return this.count_;
			}
			set
			{
				this.count_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Limit
		{
			get
			{
				return this.limit_;
			}
			set
			{
				this.limit_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint NeedItemId
		{
			get
			{
				return this.needItemId_;
			}
			set
			{
				this.needItemId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint NeedItemCount
		{
			get
			{
				return this.needItemCount_;
			}
			set
			{
				this.needItemCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<RewardDto> Rewards
		{
			get
			{
				return this.rewards_;
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
		public uint FreeCnt
		{
			get
			{
				return this.freeCnt_;
			}
			set
			{
				this.freeCnt_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ShopId != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.ShopId);
			}
			if (this.Position != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Position);
			}
			if (this.Count != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Count);
			}
			if (this.Limit != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.Limit);
			}
			if (this.NeedItemId != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.NeedItemId);
			}
			if (this.NeedItemCount != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.NeedItemCount);
			}
			this.rewards_.WriteTo(output, GuildShopDto._repeated_rewards_codec);
			if (this.Discount != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.Discount);
			}
			if (this.FreeCnt != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.FreeCnt);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ShopId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ShopId);
			}
			if (this.Position != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Position);
			}
			if (this.Count != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Count);
			}
			if (this.Limit != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Limit);
			}
			if (this.NeedItemId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.NeedItemId);
			}
			if (this.NeedItemCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.NeedItemCount);
			}
			num += this.rewards_.CalculateSize(GuildShopDto._repeated_rewards_codec);
			if (this.Discount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Discount);
			}
			if (this.FreeCnt != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.FreeCnt);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 32U)
				{
					if (num <= 16U)
					{
						if (num == 8U)
						{
							this.ShopId = input.ReadUInt32();
							continue;
						}
						if (num == 16U)
						{
							this.Position = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.Count = input.ReadUInt32();
							continue;
						}
						if (num == 32U)
						{
							this.Limit = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 40U)
					{
						this.NeedItemId = input.ReadUInt32();
						continue;
					}
					if (num == 48U)
					{
						this.NeedItemCount = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 58U)
					{
						this.rewards_.AddEntriesFrom(input, GuildShopDto._repeated_rewards_codec);
						continue;
					}
					if (num == 64U)
					{
						this.Discount = input.ReadUInt32();
						continue;
					}
					if (num == 72U)
					{
						this.FreeCnt = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildShopDto> _parser = new MessageParser<GuildShopDto>(() => new GuildShopDto());

		public const int ShopIdFieldNumber = 1;

		private uint shopId_;

		public const int PositionFieldNumber = 2;

		private uint position_;

		public const int CountFieldNumber = 3;

		private uint count_;

		public const int LimitFieldNumber = 4;

		private uint limit_;

		public const int NeedItemIdFieldNumber = 5;

		private uint needItemId_;

		public const int NeedItemCountFieldNumber = 6;

		private uint needItemCount_;

		public const int RewardsFieldNumber = 7;

		private static readonly FieldCodec<RewardDto> _repeated_rewards_codec = FieldCodec.ForMessage<RewardDto>(58U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> rewards_ = new RepeatedField<RewardDto>();

		public const int DiscountFieldNumber = 8;

		private uint discount_;

		public const int FreeCntFieldNumber = 9;

		private uint freeCnt_;
	}
}
