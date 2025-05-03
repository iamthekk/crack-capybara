using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class ShopGacheWishRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopGacheWishRequest> Parser
		{
			get
			{
				return ShopGacheWishRequest._parser;
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
		public uint GachaId
		{
			get
			{
				return this.gachaId_;
			}
			set
			{
				this.gachaId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<uint> HeroIds
		{
			get
			{
				return this.heroIds_;
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
			if (this.GachaId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.GachaId);
			}
			this.heroIds_.WriteTo(output, ShopGacheWishRequest._repeated_heroIds_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.GachaId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GachaId);
			}
			return num + this.heroIds_.CalculateSize(ShopGacheWishRequest._repeated_heroIds_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 10U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 16U)
					{
						this.GachaId = input.ReadUInt32();
						continue;
					}
				}
				else if (num == 24U || num == 26U)
				{
					this.heroIds_.AddEntriesFrom(input, ShopGacheWishRequest._repeated_heroIds_codec);
					continue;
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ShopGacheWishRequest> _parser = new MessageParser<ShopGacheWishRequest>(() => new ShopGacheWishRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int GachaIdFieldNumber = 2;

		private uint gachaId_;

		public const int HeroIdsFieldNumber = 3;

		private static readonly FieldCodec<uint> _repeated_heroIds_codec = FieldCodec.ForUInt32(26U);

		private readonly RepeatedField<uint> heroIds_ = new RepeatedField<uint>();
	}
}
