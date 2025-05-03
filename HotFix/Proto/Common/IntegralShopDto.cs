using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class IntegralShopDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<IntegralShopDto> Parser
		{
			get
			{
				return IntegralShopDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint ShopConfigId
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
		public RepeatedField<uint> GoodsConfigId
		{
			get
			{
				return this.goodsConfigId_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<uint> BuyConfigId
		{
			get
			{
				return this.buyConfigId_;
			}
		}

		[DebuggerNonUserCode]
		public uint Round
		{
			get
			{
				return this.round_;
			}
			set
			{
				this.round_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long RefTimeDay
		{
			get
			{
				return this.refTimeDay_;
			}
			set
			{
				this.refTimeDay_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long RefTimeWeek
		{
			get
			{
				return this.refTimeWeek_;
			}
			set
			{
				this.refTimeWeek_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long RefTimeMonth
		{
			get
			{
				return this.refTimeMonth_;
			}
			set
			{
				this.refTimeMonth_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ShopConfigId != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.ShopConfigId);
			}
			this.goodsConfigId_.WriteTo(output, IntegralShopDto._repeated_goodsConfigId_codec);
			this.buyConfigId_.WriteTo(output, IntegralShopDto._repeated_buyConfigId_codec);
			if (this.Round != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.Round);
			}
			if (this.RefTimeDay != 0L)
			{
				output.WriteRawTag(40);
				output.WriteInt64(this.RefTimeDay);
			}
			if (this.RefTimeWeek != 0L)
			{
				output.WriteRawTag(48);
				output.WriteInt64(this.RefTimeWeek);
			}
			if (this.RefTimeMonth != 0L)
			{
				output.WriteRawTag(56);
				output.WriteInt64(this.RefTimeMonth);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ShopConfigId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ShopConfigId);
			}
			num += this.goodsConfigId_.CalculateSize(IntegralShopDto._repeated_goodsConfigId_codec);
			num += this.buyConfigId_.CalculateSize(IntegralShopDto._repeated_buyConfigId_codec);
			if (this.Round != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Round);
			}
			if (this.RefTimeDay != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.RefTimeDay);
			}
			if (this.RefTimeWeek != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.RefTimeWeek);
			}
			if (this.RefTimeMonth != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.RefTimeMonth);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 24U)
				{
					if (num <= 16U)
					{
						if (num == 8U)
						{
							this.ShopConfigId = input.ReadUInt32();
							continue;
						}
						if (num != 16U)
						{
							goto IL_0046;
						}
					}
					else if (num != 18U)
					{
						if (num != 24U)
						{
							goto IL_0046;
						}
						goto IL_006F;
					}
					this.goodsConfigId_.AddEntriesFrom(input, IntegralShopDto._repeated_goodsConfigId_codec);
					continue;
				}
				if (num <= 32U)
				{
					if (num == 26U)
					{
						goto IL_006F;
					}
					if (num == 32U)
					{
						this.Round = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 40U)
					{
						this.RefTimeDay = input.ReadInt64();
						continue;
					}
					if (num == 48U)
					{
						this.RefTimeWeek = input.ReadInt64();
						continue;
					}
					if (num == 56U)
					{
						this.RefTimeMonth = input.ReadInt64();
						continue;
					}
				}
				IL_0046:
				input.SkipLastField();
				continue;
				IL_006F:
				this.buyConfigId_.AddEntriesFrom(input, IntegralShopDto._repeated_buyConfigId_codec);
			}
		}

		private static readonly MessageParser<IntegralShopDto> _parser = new MessageParser<IntegralShopDto>(() => new IntegralShopDto());

		public const int ShopConfigIdFieldNumber = 1;

		private uint shopConfigId_;

		public const int GoodsConfigIdFieldNumber = 2;

		private static readonly FieldCodec<uint> _repeated_goodsConfigId_codec = FieldCodec.ForUInt32(18U);

		private readonly RepeatedField<uint> goodsConfigId_ = new RepeatedField<uint>();

		public const int BuyConfigIdFieldNumber = 3;

		private static readonly FieldCodec<uint> _repeated_buyConfigId_codec = FieldCodec.ForUInt32(26U);

		private readonly RepeatedField<uint> buyConfigId_ = new RepeatedField<uint>();

		public const int RoundFieldNumber = 4;

		private uint round_;

		public const int RefTimeDayFieldNumber = 5;

		private long refTimeDay_;

		public const int RefTimeWeekFieldNumber = 6;

		private long refTimeWeek_;

		public const int RefTimeMonthFieldNumber = 7;

		private long refTimeMonth_;
	}
}
