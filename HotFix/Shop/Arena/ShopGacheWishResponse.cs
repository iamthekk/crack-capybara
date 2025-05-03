using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class ShopGacheWishResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopGacheWishResponse> Parser
		{
			get
			{
				return ShopGacheWishResponse._parser;
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
		public MapField<uint, Uint32List> WishData
		{
			get
			{
				return this.wishData_;
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
			this.wishData_.WriteTo(output, ShopGacheWishResponse._map_wishData_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.wishData_.CalculateSize(ShopGacheWishResponse._map_wishData_codec);
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
						this.wishData_.AddEntriesFrom(input, ShopGacheWishResponse._map_wishData_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<ShopGacheWishResponse> _parser = new MessageParser<ShopGacheWishResponse>(() => new ShopGacheWishResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int WishDataFieldNumber = 2;

		private static readonly MapField<uint, Uint32List>.Codec _map_wishData_codec = new MapField<uint, Uint32List>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForMessage<Uint32List>(18U, Uint32List.Parser), 18U);

		private readonly MapField<uint, Uint32List> wishData_ = new MapField<uint, Uint32List>();
	}
}
