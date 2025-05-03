using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.User
{
	public sealed class AccountLoginRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<AccountLoginRequest> Parser
		{
			get
			{
				return AccountLoginRequest._parser;
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
		public string Account
		{
			get
			{
				return this.account_;
			}
			set
			{
				this.account_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Password
		{
			get
			{
				return this.password_;
			}
			set
			{
				this.password_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.Account.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Account);
			}
			if (this.Password.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Password);
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
			if (this.Account.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Account);
			}
			if (this.Password.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Password);
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
					if (num != 18U)
					{
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							this.Password = input.ReadString();
						}
					}
					else
					{
						this.Account = input.ReadString();
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

		private static readonly MessageParser<AccountLoginRequest> _parser = new MessageParser<AccountLoginRequest>(() => new AccountLoginRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int AccountFieldNumber = 2;

		private string account_ = "";

		public const int PasswordFieldNumber = 3;

		private string password_ = "";
	}
}
