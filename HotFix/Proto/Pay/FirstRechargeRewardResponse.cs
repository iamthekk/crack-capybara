using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Pay
{
	public sealed class FirstRechargeRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<FirstRechargeRewardResponse> Parser
		{
			get
			{
				return FirstRechargeRewardResponse._parser;
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
		public bool FirstRechargeReward
		{
			get
			{
				return this.firstRechargeReward_;
			}
			set
			{
				this.firstRechargeReward_ = value;
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
			if (this.FirstRechargeReward)
			{
				output.WriteRawTag(24);
				output.WriteBool(this.FirstRechargeReward);
			}
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
			if (this.FirstRechargeReward)
			{
				num += 2;
			}
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
					if (num == 24U)
					{
						this.FirstRechargeReward = input.ReadBool();
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

		private static readonly MessageParser<FirstRechargeRewardResponse> _parser = new MessageParser<FirstRechargeRewardResponse>(() => new FirstRechargeRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int FirstRechargeRewardFieldNumber = 3;

		private bool firstRechargeReward_;

		public const int TotalRechargeFieldNumber = 4;

		private uint totalRecharge_;
	}
}
