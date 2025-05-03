using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class ShopActDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopActDto> Parser
		{
			get
			{
				return ShopActDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ShopActDetailDto> ShopDetails
		{
			get
			{
				return this.shopDetails_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.shopDetails_.WriteTo(output, ShopActDto._repeated_shopDetails_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			return 0 + this.shopDetails_.CalculateSize(ShopActDto._repeated_shopDetails_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					input.SkipLastField();
				}
				else
				{
					this.shopDetails_.AddEntriesFrom(input, ShopActDto._repeated_shopDetails_codec);
				}
			}
		}

		private static readonly MessageParser<ShopActDto> _parser = new MessageParser<ShopActDto>(() => new ShopActDto());

		public const int ShopDetailsFieldNumber = 1;

		private static readonly FieldCodec<ShopActDetailDto> _repeated_shopDetails_codec = FieldCodec.ForMessage<ShopActDetailDto>(10U, ShopActDetailDto.Parser);

		private readonly RepeatedField<ShopActDetailDto> shopDetails_ = new RepeatedField<ShopActDetailDto>();
	}
}
