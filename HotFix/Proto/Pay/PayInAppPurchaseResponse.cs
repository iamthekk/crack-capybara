using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Pay
{
	public sealed class PayInAppPurchaseResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PayInAppPurchaseResponse> Parser
		{
			get
			{
				return PayInAppPurchaseResponse._parser;
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
		public string IAPTransID
		{
			get
			{
				return this.iAPTransID_;
			}
			set
			{
				this.iAPTransID_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint RechargeId
		{
			get
			{
				return this.rechargeId_;
			}
			set
			{
				this.rechargeId_ = value;
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
		public bool IsSandBox
		{
			get
			{
				return this.isSandBox_;
			}
			set
			{
				this.isSandBox_ = value;
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
			if (this.IAPTransID.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.IAPTransID);
			}
			if (this.RechargeId != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.RechargeId);
			}
			this.rechargeIds_.WriteTo(output, PayInAppPurchaseResponse._map_rechargeIds_codec);
			if (this.iapInfo_ != null)
			{
				output.WriteRawTag(50);
				output.WriteMessage(this.IapInfo);
			}
			if (this.IsSandBox)
			{
				output.WriteRawTag(56);
				output.WriteBool(this.IsSandBox);
			}
			if (this.tgaInfoDto_ != null)
			{
				output.WriteRawTag(66);
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
			if (this.IAPTransID.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.IAPTransID);
			}
			if (this.RechargeId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.RechargeId);
			}
			num += this.rechargeIds_.CalculateSize(PayInAppPurchaseResponse._map_rechargeIds_codec);
			if (this.iapInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.IapInfo);
			}
			if (this.IsSandBox)
			{
				num += 2;
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
				if (num <= 32U)
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
							this.IAPTransID = input.ReadString();
							continue;
						}
						if (num == 32U)
						{
							this.RechargeId = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 50U)
				{
					if (num == 42U)
					{
						this.rechargeIds_.AddEntriesFrom(input, PayInAppPurchaseResponse._map_rechargeIds_codec);
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
					if (num == 56U)
					{
						this.IsSandBox = input.ReadBool();
						continue;
					}
					if (num == 66U)
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

		private static readonly MessageParser<PayInAppPurchaseResponse> _parser = new MessageParser<PayInAppPurchaseResponse>(() => new PayInAppPurchaseResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int IAPTransIDFieldNumber = 3;

		private string iAPTransID_ = "";

		public const int RechargeIdFieldNumber = 4;

		private uint rechargeId_;

		public const int RechargeIdsFieldNumber = 5;

		private static readonly MapField<uint, uint>.Codec _map_rechargeIds_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 42U);

		private readonly MapField<uint, uint> rechargeIds_ = new MapField<uint, uint>();

		public const int IapInfoFieldNumber = 6;

		private IAPDto iapInfo_;

		public const int IsSandBoxFieldNumber = 7;

		private bool isSandBox_;

		public const int TgaInfoDtoFieldNumber = 8;

		private TGAInfoDto tgaInfoDto_;
	}
}
