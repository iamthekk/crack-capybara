using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.User
{
	public sealed class AccountLoginResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<AccountLoginResponse> Parser
		{
			get
			{
				return AccountLoginResponse._parser;
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
		public int Age
		{
			get
			{
				return this.age_;
			}
			set
			{
				this.age_ = value;
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
			if (this.Age != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Age);
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
			if (this.Age != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Age);
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
					if (num != 16U)
					{
						input.SkipLastField();
					}
					else
					{
						this.Age = input.ReadInt32();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<AccountLoginResponse> _parser = new MessageParser<AccountLoginResponse>(() => new AccountLoginResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int AgeFieldNumber = 2;

		private int age_;
	}
}
