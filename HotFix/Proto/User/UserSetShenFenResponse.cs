using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.User
{
	public sealed class UserSetShenFenResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserSetShenFenResponse> Parser
		{
			get
			{
				return UserSetShenFenResponse._parser;
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
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.Shenfenzheng.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Shenfenzheng);
			}
			if (this.Age != 0)
			{
				output.WriteRawTag(24);
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
				if (num != 8U)
				{
					if (num != 18U)
					{
						if (num != 24U)
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
						this.Shenfenzheng = input.ReadString();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<UserSetShenFenResponse> _parser = new MessageParser<UserSetShenFenResponse>(() => new UserSetShenFenResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int ShenfenzhengFieldNumber = 2;

		private string shenfenzheng_ = "";

		public const int AgeFieldNumber = 3;

		private int age_;
	}
}
