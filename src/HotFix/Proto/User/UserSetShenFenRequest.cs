using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserSetShenFenRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserSetShenFenRequest> Parser
		{
			get
			{
				return UserSetShenFenRequest._parser;
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
		public string Shenfenzheng
		{
			get
			{
				return this.shenfenzheng_;
			}
			set
			{
				this.shenfenzheng_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.Shenfenzheng.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Shenfenzheng);
			}
			if (this.Age != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Age);
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
			if (this.Shenfenzheng.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Shenfenzheng);
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
				if (num <= 18U)
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
					if (num == 18U)
					{
						this.Account = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.Shenfenzheng = input.ReadString();
						continue;
					}
					if (num == 32U)
					{
						this.Age = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UserSetShenFenRequest> _parser = new MessageParser<UserSetShenFenRequest>(() => new UserSetShenFenRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int AccountFieldNumber = 2;

		private string account_ = "";

		public const int ShenfenzhengFieldNumber = 3;

		private string shenfenzheng_ = "";

		public const int AgeFieldNumber = 4;

		private int age_;
	}
}
