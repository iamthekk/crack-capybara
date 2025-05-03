using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Mining
{
	public sealed class MiningBoxUpgradeRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MiningBoxUpgradeRewardResponse> Parser
		{
			get
			{
				return MiningBoxUpgradeRewardResponse._parser;
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
		public MiningInfoDto MiningInfoDto
		{
			get
			{
				return this.miningInfoDto_;
			}
			set
			{
				this.miningInfoDto_ = value;
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
			if (this.miningInfoDto_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.MiningInfoDto);
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
			if (this.miningInfoDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.MiningInfoDto);
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
							if (this.miningInfoDto_ == null)
							{
								this.miningInfoDto_ = new MiningInfoDto();
							}
							input.ReadMessage(this.miningInfoDto_);
						}
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
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<MiningBoxUpgradeRewardResponse> _parser = new MessageParser<MiningBoxUpgradeRewardResponse>(() => new MiningBoxUpgradeRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int MiningInfoDtoFieldNumber = 3;

		private MiningInfoDto miningInfoDto_;
	}
}
