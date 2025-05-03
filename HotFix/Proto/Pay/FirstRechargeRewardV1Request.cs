using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Pay
{
	public sealed class FirstRechargeRewardV1Request : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<FirstRechargeRewardV1Request> Parser
		{
			get
			{
				return FirstRechargeRewardV1Request._parser;
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
		public int Configid
		{
			get
			{
				return this.configid_;
			}
			set
			{
				this.configid_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Products
		{
			get
			{
				return this.products_;
			}
			set
			{
				this.products_ = value;
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
			if (this.Configid != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Configid);
			}
			if (this.Products != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Products);
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
			if (this.Configid != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Configid);
			}
			if (this.Products != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Products);
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
							this.Products = input.ReadInt32();
						}
					}
					else
					{
						this.Configid = input.ReadInt32();
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

		private static readonly MessageParser<FirstRechargeRewardV1Request> _parser = new MessageParser<FirstRechargeRewardV1Request>(() => new FirstRechargeRewardV1Request());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ConfigidFieldNumber = 2;

		private int configid_;

		public const int ProductsFieldNumber = 3;

		private int products_;
	}
}
