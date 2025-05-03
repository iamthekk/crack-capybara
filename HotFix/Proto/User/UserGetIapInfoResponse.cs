using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserGetIapInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserGetIapInfoResponse> Parser
		{
			get
			{
				return UserGetIapInfoResponse._parser;
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
		public UserCurrency UserCurrency
		{
			get
			{
				return this.userCurrency_;
			}
			set
			{
				this.userCurrency_ = value;
			}
		}

		[DebuggerNonUserCode]
		public UserLevel UserLevel
		{
			get
			{
				return this.userLevel_;
			}
			set
			{
				this.userLevel_ = value;
			}
		}

		[DebuggerNonUserCode]
		public UserMission UserMission
		{
			get
			{
				return this.userMission_;
			}
			set
			{
				this.userMission_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ItemDto> Items
		{
			get
			{
				return this.items_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<EquipmentDto> Equipments
		{
			get
			{
				return this.equipments_;
			}
		}

		[DebuggerNonUserCode]
		public ulong TransId
		{
			get
			{
				return this.transId_;
			}
			set
			{
				this.transId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public UserTicket UserLifeValue
		{
			get
			{
				return this.userLifeValue_;
			}
			set
			{
				this.userLifeValue_ = value;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.userCurrency_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.UserCurrency);
			}
			if (this.userLevel_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.UserLevel);
			}
			if (this.userMission_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.UserMission);
			}
			this.items_.WriteTo(output, UserGetIapInfoResponse._repeated_items_codec);
			this.equipments_.WriteTo(output, UserGetIapInfoResponse._repeated_equipments_codec);
			if (this.TransId != 0UL)
			{
				output.WriteRawTag(56);
				output.WriteUInt64(this.TransId);
			}
			if (this.userLifeValue_ != null)
			{
				output.WriteRawTag(66);
				output.WriteMessage(this.UserLifeValue);
			}
			if (this.iapInfo_ != null)
			{
				output.WriteRawTag(74);
				output.WriteMessage(this.IapInfo);
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
			if (this.userCurrency_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserCurrency);
			}
			if (this.userLevel_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserLevel);
			}
			if (this.userMission_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserMission);
			}
			num += this.items_.CalculateSize(UserGetIapInfoResponse._repeated_items_codec);
			num += this.equipments_.CalculateSize(UserGetIapInfoResponse._repeated_equipments_codec);
			if (this.TransId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.TransId);
			}
			if (this.userLifeValue_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserLifeValue);
			}
			if (this.iapInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.IapInfo);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 34U)
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
							if (this.userCurrency_ == null)
							{
								this.userCurrency_ = new UserCurrency();
							}
							input.ReadMessage(this.userCurrency_);
							continue;
						}
					}
					else
					{
						if (num == 26U)
						{
							if (this.userLevel_ == null)
							{
								this.userLevel_ = new UserLevel();
							}
							input.ReadMessage(this.userLevel_);
							continue;
						}
						if (num == 34U)
						{
							if (this.userMission_ == null)
							{
								this.userMission_ = new UserMission();
							}
							input.ReadMessage(this.userMission_);
							continue;
						}
					}
				}
				else if (num <= 50U)
				{
					if (num == 42U)
					{
						this.items_.AddEntriesFrom(input, UserGetIapInfoResponse._repeated_items_codec);
						continue;
					}
					if (num == 50U)
					{
						this.equipments_.AddEntriesFrom(input, UserGetIapInfoResponse._repeated_equipments_codec);
						continue;
					}
				}
				else
				{
					if (num == 56U)
					{
						this.TransId = input.ReadUInt64();
						continue;
					}
					if (num == 66U)
					{
						if (this.userLifeValue_ == null)
						{
							this.userLifeValue_ = new UserTicket();
						}
						input.ReadMessage(this.userLifeValue_);
						continue;
					}
					if (num == 74U)
					{
						if (this.iapInfo_ == null)
						{
							this.iapInfo_ = new IAPDto();
						}
						input.ReadMessage(this.iapInfo_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UserGetIapInfoResponse> _parser = new MessageParser<UserGetIapInfoResponse>(() => new UserGetIapInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int UserCurrencyFieldNumber = 2;

		private UserCurrency userCurrency_;

		public const int UserLevelFieldNumber = 3;

		private UserLevel userLevel_;

		public const int UserMissionFieldNumber = 4;

		private UserMission userMission_;

		public const int ItemsFieldNumber = 5;

		private static readonly FieldCodec<ItemDto> _repeated_items_codec = FieldCodec.ForMessage<ItemDto>(42U, ItemDto.Parser);

		private readonly RepeatedField<ItemDto> items_ = new RepeatedField<ItemDto>();

		public const int EquipmentsFieldNumber = 6;

		private static readonly FieldCodec<EquipmentDto> _repeated_equipments_codec = FieldCodec.ForMessage<EquipmentDto>(50U, EquipmentDto.Parser);

		private readonly RepeatedField<EquipmentDto> equipments_ = new RepeatedField<EquipmentDto>();

		public const int TransIdFieldNumber = 7;

		private ulong transId_;

		public const int UserLifeValueFieldNumber = 8;

		private UserTicket userLifeValue_;

		public const int IapInfoFieldNumber = 9;

		private IAPDto iapInfo_;
	}
}
