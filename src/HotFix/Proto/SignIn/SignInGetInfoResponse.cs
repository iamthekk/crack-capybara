using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.SignIn
{
	public sealed class SignInGetInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SignInGetInfoResponse> Parser
		{
			get
			{
				return SignInGetInfoResponse._parser;
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
		public SignInData SignInData
		{
			get
			{
				return this.signInData_;
			}
			set
			{
				this.signInData_ = value;
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
			if (this.signInData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.SignInData);
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
			if (this.signInData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.SignInData);
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
						if (this.signInData_ == null)
						{
							this.signInData_ = new SignInData();
						}
						input.ReadMessage(this.signInData_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<SignInGetInfoResponse> _parser = new MessageParser<SignInGetInfoResponse>(() => new SignInGetInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int SignInDataFieldNumber = 2;

		private SignInData signInData_;
	}
}
