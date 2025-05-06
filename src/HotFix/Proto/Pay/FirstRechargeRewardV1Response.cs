﻿using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Pay
{
	public sealed class FirstRechargeRewardV1Response : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<FirstRechargeRewardV1Response> Parser
		{
			get
			{
				return FirstRechargeRewardV1Response._parser;
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
		public MapField<int, uint> FirstChargeGiftReward
		{
			get
			{
				return this.firstChargeGiftReward_;
			}
		}

		[DebuggerNonUserCode]
		public uint TotalRecharge
		{
			get
			{
				return this.totalRecharge_;
			}
			set
			{
				this.totalRecharge_ = value;
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
			this.firstChargeGiftReward_.WriteTo(output, FirstRechargeRewardV1Response._map_firstChargeGiftReward_codec);
			if (this.TotalRecharge != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.TotalRecharge);
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
			num += this.firstChargeGiftReward_.CalculateSize(FirstRechargeRewardV1Response._map_firstChargeGiftReward_codec);
			if (this.TotalRecharge != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TotalRecharge);
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
						this.firstChargeGiftReward_.AddEntriesFrom(input, FirstRechargeRewardV1Response._map_firstChargeGiftReward_codec);
						continue;
					}
					if (num == 32U)
					{
						this.TotalRecharge = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<FirstRechargeRewardV1Response> _parser = new MessageParser<FirstRechargeRewardV1Response>(() => new FirstRechargeRewardV1Response());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int FirstChargeGiftRewardFieldNumber = 3;

		private static readonly MapField<int, uint>.Codec _map_firstChargeGiftReward_codec = new MapField<int, uint>.Codec(FieldCodec.ForInt32(8U), FieldCodec.ForUInt32(16U), 26U);

		private readonly MapField<int, uint> firstChargeGiftReward_ = new MapField<int, uint>();

		public const int TotalRechargeFieldNumber = 4;

		private uint totalRecharge_;
	}
}
