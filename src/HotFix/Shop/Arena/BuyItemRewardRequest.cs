using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class BuyItemRewardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<BuyItemRewardRequest> Parser
		{
			get
			{
				return BuyItemRewardRequest._parser;
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
		public int ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int EquipId
		{
			get
			{
				return this.equipId_;
			}
			set
			{
				this.equipId_ = value;
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
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ConfigId);
			}
			if (this.EquipId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.EquipId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.ConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ConfigId);
			}
			if (this.EquipId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.EquipId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U)
					{
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.EquipId = input.ReadInt32();
						}
					}
					else
					{
						this.ConfigId = input.ReadInt32();
					}
				}
				else
				{
					if (this.commonParams_ == null)
					{
						this.commonParams_ = new CommonParams();
					}
					input.ReadMessage(this.commonParams_);
				}
			}
		}

		private static readonly MessageParser<BuyItemRewardRequest> _parser = new MessageParser<BuyItemRewardRequest>(() => new BuyItemRewardRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ConfigIdFieldNumber = 2;

		private int configId_;

		public const int EquipIdFieldNumber = 3;

		private int equipId_;
	}
}
