using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Pay
{
	public sealed class WeChatOrderDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<WeChatOrderDto> Parser
		{
			get
			{
				return WeChatOrderDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public string PrePayId
		{
			get
			{
				return this.prePayId_;
			}
			set
			{
				this.prePayId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string NonceStr
		{
			get
			{
				return this.nonceStr_;
			}
			set
			{
				this.nonceStr_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public ulong Timestamp
		{
			get
			{
				return this.timestamp_;
			}
			set
			{
				this.timestamp_ = value;
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
			if (this.PrePayId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.PrePayId);
			}
			if (this.NonceStr.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.NonceStr);
			}
			if (this.Timestamp != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.Timestamp);
			}
			if (this.Sign.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.Sign);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.PrePayId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.PrePayId);
			}
			if (this.NonceStr.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.NonceStr);
			}
			if (this.Timestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Timestamp);
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
				if (num <= 18U)
				{
					if (num == 10U)
					{
						this.PrePayId = input.ReadString();
						continue;
					}
					if (num == 18U)
					{
						this.NonceStr = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Timestamp = input.ReadUInt64();
						continue;
					}
					if (num == 34U)
					{
						this.Sign = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<WeChatOrderDto> _parser = new MessageParser<WeChatOrderDto>(() => new WeChatOrderDto());

		public const int PrePayIdFieldNumber = 1;

		private string prePayId_ = "";

		public const int NonceStrFieldNumber = 2;

		private string nonceStr_ = "";

		public const int TimestampFieldNumber = 3;

		private ulong timestamp_;

		public const int SignFieldNumber = 4;

		private string sign_ = "";
	}
}
