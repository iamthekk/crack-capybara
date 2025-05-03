using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class UpdateUserCurrency : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UpdateUserCurrency> Parser
		{
			get
			{
				return UpdateUserCurrency._parser;
			}
		}

		[DebuggerNonUserCode]
		public bool IsChange
		{
			get
			{
				return this.isChange_;
			}
			set
			{
				this.isChange_ = value;
			}
		}

		[DebuggerNonUserCode]
		public UserCurrency UserCurrency
		{
			get
			{
				return this.userCurrency_;
			}
			set
			{
				this.userCurrency_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.IsChange)
			{
				output.WriteRawTag(8);
				output.WriteBool(this.IsChange);
			}
			if (this.userCurrency_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.UserCurrency);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.IsChange)
			{
				num += 2;
			}
			if (this.userCurrency_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserCurrency);
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
						if (this.userCurrency_ == null)
						{
							this.userCurrency_ = new UserCurrency();
						}
						input.ReadMessage(this.userCurrency_);
					}
				}
				else
				{
					this.IsChange = input.ReadBool();
				}
			}
		}

		private static readonly MessageParser<UpdateUserCurrency> _parser = new MessageParser<UpdateUserCurrency>(() => new UpdateUserCurrency());

		public const int IsChangeFieldNumber = 1;

		private bool isChange_;

		public const int UserCurrencyFieldNumber = 2;

		private UserCurrency userCurrency_;
	}
}
