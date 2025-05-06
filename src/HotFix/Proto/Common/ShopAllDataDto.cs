using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class ShopAllDataDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopAllDataDto> Parser
		{
			get
			{
				return ShopAllDataDto._parser;
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
		public RepeatedField<RewardDto> GachaChestReward
		{
			get
			{
				return this.gachaChestReward_;
			}
		}

		[DebuggerNonUserCode]
		public uint GachaChestCount
		{
			get
			{
				return this.gachaChestCount_;
			}
			set
			{
				this.gachaChestCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint GachaChestNeedCount
		{
			get
			{
				return this.gachaChestNeedCount_;
			}
			set
			{
				this.gachaChestNeedCount_ = value;
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
		public ShopActDto ShopAct
		{
			get
			{
				return this.shopAct_;
			}
			set
			{
				this.shopAct_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.rechargeIds_.WriteTo(output, ShopAllDataDto._map_rechargeIds_codec);
			this.gachaChestReward_.WriteTo(output, ShopAllDataDto._repeated_gachaChestReward_codec);
			if (this.GachaChestCount != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.GachaChestCount);
			}
			if (this.GachaChestNeedCount != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.GachaChestNeedCount);
			}
			this.wishData_.WriteTo(output, ShopAllDataDto._map_wishData_codec);
			if (this.iapInfo_ != null)
			{
				output.WriteRawTag(50);
				output.WriteMessage(this.IapInfo);
			}
			if (this.shopDrawDto_ != null)
			{
				output.WriteRawTag(58);
				output.WriteMessage(this.ShopDrawDto);
			}
			if (this.adData_ != null)
			{
				output.WriteRawTag(66);
				output.WriteMessage(this.AdData);
			}
			if (this.shopAct_ != null)
			{
				output.WriteRawTag(74);
				output.WriteMessage(this.ShopAct);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.rechargeIds_.CalculateSize(ShopAllDataDto._map_rechargeIds_codec);
			num += this.gachaChestReward_.CalculateSize(ShopAllDataDto._repeated_gachaChestReward_codec);
			if (this.GachaChestCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GachaChestCount);
			}
			if (this.GachaChestNeedCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GachaChestNeedCount);
			}
			num += this.wishData_.CalculateSize(ShopAllDataDto._map_wishData_codec);
			if (this.iapInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.IapInfo);
			}
			if (this.shopDrawDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.ShopDrawDto);
			}
			if (this.adData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.AdData);
			}
			if (this.shopAct_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.ShopAct);
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
					if (num <= 18U)
					{
						if (num == 10U)
						{
							this.rechargeIds_.AddEntriesFrom(input, ShopAllDataDto._map_rechargeIds_codec);
							continue;
						}
						if (num == 18U)
						{
							this.gachaChestReward_.AddEntriesFrom(input, ShopAllDataDto._repeated_gachaChestReward_codec);
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.GachaChestCount = input.ReadUInt32();
							continue;
						}
						if (num == 32U)
						{
							this.GachaChestNeedCount = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 50U)
				{
					if (num == 42U)
					{
						this.wishData_.AddEntriesFrom(input, ShopAllDataDto._map_wishData_codec);
						continue;
					}
					if (num == 50U)
					{
						if (this.iapInfo_ == null)
						{
							this.iapInfo_ = new IAPDto();
						}
						input.ReadMessage(this.iapInfo_);
						continue;
					}
				}
				else
				{
					if (num == 58U)
					{
						if (this.shopDrawDto_ == null)
						{
							this.shopDrawDto_ = new ShopDrawDto();
						}
						input.ReadMessage(this.shopDrawDto_);
						continue;
					}
					if (num == 66U)
					{
						if (this.adData_ == null)
						{
							this.adData_ = new AdDataDto();
						}
						input.ReadMessage(this.adData_);
						continue;
					}
					if (num == 74U)
					{
						if (this.shopAct_ == null)
						{
							this.shopAct_ = new ShopActDto();
						}
						input.ReadMessage(this.shopAct_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ShopAllDataDto> _parser = new MessageParser<ShopAllDataDto>(() => new ShopAllDataDto());

		public const int RechargeIdsFieldNumber = 1;

		private static readonly MapField<uint, uint>.Codec _map_rechargeIds_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 10U);

		private readonly MapField<uint, uint> rechargeIds_ = new MapField<uint, uint>();

		public const int GachaChestRewardFieldNumber = 2;

		private static readonly FieldCodec<RewardDto> _repeated_gachaChestReward_codec = FieldCodec.ForMessage<RewardDto>(18U, RewardDto.Parser);

		private readonly RepeatedField<RewardDto> gachaChestReward_ = new RepeatedField<RewardDto>();

		public const int GachaChestCountFieldNumber = 3;

		private uint gachaChestCount_;

		public const int GachaChestNeedCountFieldNumber = 4;

		private uint gachaChestNeedCount_;

		public const int WishDataFieldNumber = 5;

		private static readonly MapField<uint, Uint32List>.Codec _map_wishData_codec = new MapField<uint, Uint32List>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForMessage<Uint32List>(18U, Uint32List.Parser), 42U);

		private readonly MapField<uint, Uint32List> wishData_ = new MapField<uint, Uint32List>();

		public const int IapInfoFieldNumber = 6;

		private IAPDto iapInfo_;

		public const int ShopDrawDtoFieldNumber = 7;

		private ShopDrawDto shopDrawDto_;

		public const int AdDataFieldNumber = 8;

		private AdDataDto adData_;

		public const int ShopActFieldNumber = 9;

		private ShopActDto shopAct_;
	}
}
