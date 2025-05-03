using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserLoginRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserLoginRequest> Parser
		{
			get
			{
				return UserLoginRequest._parser;
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
		public uint ChannelId
		{
			get
			{
				return this.channelId_;
			}
			set
			{
				this.channelId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string AccountId2
		{
			get
			{
				return this.accountId2_;
			}
			set
			{
				this.accountId2_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Verification
		{
			get
			{
				return this.verification_;
			}
			set
			{
				this.verification_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.ChannelId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.ChannelId);
			}
			if (this.AccountId2.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.AccountId2);
			}
			if (this.Verification.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.Verification);
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
			if (this.ChannelId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ChannelId);
			}
			if (this.AccountId2.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AccountId2);
			}
			if (this.Verification.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Verification);
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
						this.ChannelId = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.AccountId2 = input.ReadString();
						continue;
					}
					if (num == 34U)
					{
						this.Verification = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UserLoginRequest> _parser = new MessageParser<UserLoginRequest>(() => new UserLoginRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ChannelIdFieldNumber = 2;

		private uint channelId_;

		public const int AccountId2FieldNumber = 3;

		private string accountId2_ = "";

		public const int VerificationFieldNumber = 4;

		private string verification_ = "";
	}
}
