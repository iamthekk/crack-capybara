using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class ShopIntegralGetInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopIntegralGetInfoResponse> Parser
		{
			get
			{
				return ShopIntegralGetInfoResponse._parser;
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
		public RepeatedField<IntegralShopDto> IntegralShops
		{
			get
			{
				return this.integralShops_;
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
			this.integralShops_.WriteTo(output, ShopIntegralGetInfoResponse._repeated_integralShops_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.integralShops_.CalculateSize(ShopIntegralGetInfoResponse._repeated_integralShops_codec);
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
						input.SkipLastField();
					}
					else
					{
						this.integralShops_.AddEntriesFrom(input, ShopIntegralGetInfoResponse._repeated_integralShops_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<ShopIntegralGetInfoResponse> _parser = new MessageParser<ShopIntegralGetInfoResponse>(() => new ShopIntegralGetInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int IntegralShopsFieldNumber = 2;

		private static readonly FieldCodec<IntegralShopDto> _repeated_integralShops_codec = FieldCodec.ForMessage<IntegralShopDto>(18U, IntegralShopDto.Parser);

		private readonly RepeatedField<IntegralShopDto> integralShops_ = new RepeatedField<IntegralShopDto>();
	}
}
