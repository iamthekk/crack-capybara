using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Pay
{
	public sealed class BattlePassRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<BattlePassRewardResponse> Parser
		{
			get
			{
				return BattlePassRewardResponse._parser;
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
		public IAPBattlePassDto BattlePassDto
		{
			get
			{
				return this.battlePassDto_;
			}
			set
			{
				this.battlePassDto_ = value;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.battlePassDto_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.BattlePassDto);
			}
			if (this.commonData_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.CommonData);
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
			if (this.battlePassDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.BattlePassDto);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			return num;
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
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							if (this.commonData_ == null)
							{
								this.commonData_ = new CommonData();
							}
							input.ReadMessage(this.commonData_);
						}
					}
					else
					{
						if (this.battlePassDto_ == null)
						{
							this.battlePassDto_ = new IAPBattlePassDto();
						}
						input.ReadMessage(this.battlePassDto_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<BattlePassRewardResponse> _parser = new MessageParser<BattlePassRewardResponse>(() => new BattlePassRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int BattlePassDtoFieldNumber = 2;

		private IAPBattlePassDto battlePassDto_;

		public const int CommonDataFieldNumber = 3;

		private CommonData commonData_;
	}
}
