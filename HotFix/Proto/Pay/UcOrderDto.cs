using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Pay
{
	public sealed class UcOrderDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UcOrderDto> Parser
		{
			get
			{
				return UcOrderDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public string CallbackInfo
		{
			get
			{
				return this.callbackInfo_;
			}
			set
			{
				this.callbackInfo_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Amount
		{
			get
			{
				return this.amount_;
			}
			set
			{
				this.amount_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string NotifyUrl
		{
			get
			{
				return this.notifyUrl_;
			}
			set
			{
				this.notifyUrl_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string CpOrderId
		{
			get
			{
				return this.cpOrderId_;
			}
			set
			{
				this.cpOrderId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string AccountId
		{
			get
			{
				return this.accountId_;
			}
			set
			{
				this.accountId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string SignType
		{
			get
			{
				return this.signType_;
			}
			set
			{
				this.signType_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Sign
		{
			get
			{
				return this.sign_;
			}
			set
			{
				this.sign_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.CallbackInfo.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.CallbackInfo);
			}
			if (this.Amount.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Amount);
			}
			if (this.NotifyUrl.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.NotifyUrl);
			}
			if (this.CpOrderId.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.CpOrderId);
			}
			if (this.AccountId.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(this.AccountId);
			}
			if (this.SignType.Length != 0)
			{
				output.WriteRawTag(50);
				output.WriteString(this.SignType);
			}
			if (this.Sign.Length != 0)
			{
				output.WriteRawTag(58);
				output.WriteString(this.Sign);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.CallbackInfo.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.CallbackInfo);
			}
			if (this.Amount.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Amount);
			}
			if (this.NotifyUrl.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.NotifyUrl);
			}
			if (this.CpOrderId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.CpOrderId);
			}
			if (this.AccountId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AccountId);
			}
			if (this.SignType.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.SignType);
			}
			if (this.Sign.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Sign);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 26U)
				{
					if (num == 10U)
					{
						this.CallbackInfo = input.ReadString();
						continue;
					}
					if (num == 18U)
					{
						this.Amount = input.ReadString();
						continue;
					}
					if (num == 26U)
					{
						this.NotifyUrl = input.ReadString();
						continue;
					}
				}
				else if (num <= 42U)
				{
					if (num == 34U)
					{
						this.CpOrderId = input.ReadString();
						continue;
					}
					if (num == 42U)
					{
						this.AccountId = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 50U)
					{
						this.SignType = input.ReadString();
						continue;
					}
					if (num == 58U)
					{
						this.Sign = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UcOrderDto> _parser = new MessageParser<UcOrderDto>(() => new UcOrderDto());

		public const int CallbackInfoFieldNumber = 1;

		private string callbackInfo_ = "";

		public const int AmountFieldNumber = 2;

		private string amount_ = "";

		public const int NotifyUrlFieldNumber = 3;

		private string notifyUrl_ = "";

		public const int CpOrderIdFieldNumber = 4;

		private string cpOrderId_ = "";

		public const int AccountIdFieldNumber = 5;

		private string accountId_ = "";

		public const int SignTypeFieldNumber = 6;

		private string signType_ = "";

		public const int SignFieldNumber = 7;

		private string sign_ = "";
	}
}
