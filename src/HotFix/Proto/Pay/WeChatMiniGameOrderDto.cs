using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Pay
{
	public sealed class WeChatMiniGameOrderDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<WeChatMiniGameOrderDto> Parser
		{
			get
			{
				return WeChatMiniGameOrderDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public string SignData
		{
			get
			{
				return this.signData_;
			}
			set
			{
				this.signData_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string PaySig
		{
			get
			{
				return this.paySig_;
			}
			set
			{
				this.paySig_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Signature
		{
			get
			{
				return this.signature_;
			}
			set
			{
				this.signature_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.SignData.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.SignData);
			}
			if (this.PaySig.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.PaySig);
			}
			if (this.Signature.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Signature);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.SignData.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.SignData);
			}
			if (this.PaySig.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.PaySig);
			}
			if (this.Signature.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Signature);
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
							this.Signature = input.ReadString();
						}
					}
					else
					{
						this.PaySig = input.ReadString();
					}
				}
				else
				{
					this.SignData = input.ReadString();
				}
			}
		}

		private static readonly MessageParser<WeChatMiniGameOrderDto> _parser = new MessageParser<WeChatMiniGameOrderDto>(() => new WeChatMiniGameOrderDto());

		public const int SignDataFieldNumber = 1;

		private string signData_ = "";

		public const int PaySigFieldNumber = 2;

		private string paySig_ = "";

		public const int SignatureFieldNumber = 3;

		private string signature_ = "";
	}
}
