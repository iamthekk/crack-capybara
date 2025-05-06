using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class ShopGetInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopGetInfoResponse> Parser
		{
			get
			{
				return ShopGetInfoResponse._parser;
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
		public ShopAllDataDto ShopAllDataDto
		{
			get
			{
				return this.shopAllDataDto_;
			}
			set
			{
				this.shopAllDataDto_ = value;
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
			if (this.shopAllDataDto_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.ShopAllDataDto);
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
			if (this.shopAllDataDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.ShopAllDataDto);
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
						input.SkipLastField();
					}
					else
					{
						if (this.shopAllDataDto_ == null)
						{
							this.shopAllDataDto_ = new ShopAllDataDto();
						}
						input.ReadMessage(this.shopAllDataDto_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<ShopGetInfoResponse> _parser = new MessageParser<ShopGetInfoResponse>(() => new ShopGetInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int ShopAllDataDtoFieldNumber = 2;

		private ShopAllDataDto shopAllDataDto_;
	}
}
