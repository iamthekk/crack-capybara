using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class ShopBuyIAPItemResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopBuyIAPItemResponse> Parser
		{
			get
			{
				return ShopBuyIAPItemResponse._parser;
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
		public MapField<uint, uint> RechargeIds
		{
			get
			{
				return this.rechargeIds_;
			}
		}

		[DebuggerNonUserCode]
		public IAPDto IapInfo
		{
			get
			{
				return this.iapInfo_;
			}
			set
			{
				this.iapInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public TGAInfoDto TgaInfoDto
		{
			get
			{
				return this.tgaInfoDto_;
			}
			set
			{
				this.tgaInfoDto_ = value;
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
			this.rechargeIds_.WriteTo(output, ShopBuyIAPItemResponse._map_rechargeIds_codec);
			if (this.iapInfo_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.IapInfo);
			}
			if (this.tgaInfoDto_ != null)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.TgaInfoDto);
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
			num += this.rechargeIds_.CalculateSize(ShopBuyIAPItemResponse._map_rechargeIds_codec);
			if (this.iapInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.IapInfo);
			}
			if (this.tgaInfoDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.TgaInfoDto);
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
						this.rechargeIds_.AddEntriesFrom(input, ShopBuyIAPItemResponse._map_rechargeIds_codec);
						continue;
					}
					if (num == 34U)
					{
						if (this.iapInfo_ == null)
						{
							this.iapInfo_ = new IAPDto();
						}
						input.ReadMessage(this.iapInfo_);
						continue;
					}
					if (num == 42U)
					{
						if (this.tgaInfoDto_ == null)
						{
							this.tgaInfoDto_ = new TGAInfoDto();
						}
						input.ReadMessage(this.tgaInfoDto_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ShopBuyIAPItemResponse> _parser = new MessageParser<ShopBuyIAPItemResponse>(() => new ShopBuyIAPItemResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int RechargeIdsFieldNumber = 3;

		private static readonly MapField<uint, uint>.Codec _map_rechargeIds_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 26U);

		private readonly MapField<uint, uint> rechargeIds_ = new MapField<uint, uint>();

		public const int IapInfoFieldNumber = 4;

		private IAPDto iapInfo_;

		public const int TgaInfoDtoFieldNumber = 5;

		private TGAInfoDto tgaInfoDto_;
	}
}
