using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class ShopIntegralBuyItemResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ShopIntegralBuyItemResponse> Parser
		{
			get
			{
				return ShopIntegralBuyItemResponse._parser;
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
		public RepeatedField<uint> BuyLog
		{
			get
			{
				return this.buyLog_;
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
			this.buyLog_.WriteTo(output, ShopIntegralBuyItemResponse._repeated_buyLog_codec);
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
			return num + this.buyLog_.CalculateSize(ShopIntegralBuyItemResponse._repeated_buyLog_codec);
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
				else if (num == 24U || num == 26U)
				{
					this.buyLog_.AddEntriesFrom(input, ShopIntegralBuyItemResponse._repeated_buyLog_codec);
					continue;
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ShopIntegralBuyItemResponse> _parser = new MessageParser<ShopIntegralBuyItemResponse>(() => new ShopIntegralBuyItemResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int BuyLogFieldNumber = 3;

		private static readonly FieldCodec<uint> _repeated_buyLog_codec = FieldCodec.ForUInt32(26U);

		private readonly RepeatedField<uint> buyLog_ = new RepeatedField<uint>();
	}
}
