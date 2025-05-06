using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class ShopBuyIAPItemRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopBuyIAPItemRequest> Parser
		{
			get
			{
				return ShopBuyIAPItemRequest._parser;
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
		public uint ConfigId
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
		public string ExtraInfo
		{
			get
			{
				return this.extraInfo_;
			}
			set
			{
				this.extraInfo_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint ExtraType
		{
			get
			{
				return this.extraType_;
			}
			set
			{
				this.extraType_ = value;
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
			if (this.ConfigId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.ConfigId);
			}
			if (this.ExtraInfo.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.ExtraInfo);
			}
			if (this.ExtraType != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.ExtraType);
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
			if (this.ConfigId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ConfigId);
			}
			if (this.ExtraInfo.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ExtraInfo);
			}
			if (this.ExtraType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ExtraType);
			}
			return num;
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
						this.ConfigId = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.ExtraInfo = input.ReadString();
						continue;
					}
					if (num == 32U)
					{
						this.ExtraType = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ShopBuyIAPItemRequest> _parser = new MessageParser<ShopBuyIAPItemRequest>(() => new ShopBuyIAPItemRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ConfigIdFieldNumber = 2;

		private uint configId_;

		public const int ExtraInfoFieldNumber = 3;

		private string extraInfo_ = "";

		public const int ExtraTypeFieldNumber = 4;

		private uint extraType_;
	}
}
