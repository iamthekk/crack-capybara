﻿using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.User
{
	public sealed class AccountSignInResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<AccountSignInResponse> Parser
		{
			get
			{
				return AccountSignInResponse._parser;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
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
					input.SkipLastField();
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<AccountSignInResponse> _parser = new MessageParser<AccountSignInResponse>(() => new AccountSignInResponse());

		public const int CodeFieldNumber = 1;

		private int code_;
	}
}
