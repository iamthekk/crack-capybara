using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Pay
{
	public sealed class DouYinOrderDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<DouYinOrderDto> Parser
		{
			get
			{
				return DouYinOrderDto._parser;
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
		public string SdkParam
		{
			get
			{
				return this.sdkParam_;
			}
			set
			{
				this.sdkParam_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Message
		{
			get
			{
				return this.message_;
			}
			set
			{
				this.message_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.SdkParam.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.SdkParam);
			}
			if (this.Message.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Message);
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
			if (this.SdkParam.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.SdkParam);
			}
			if (this.Message.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Message);
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
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							this.Message = input.ReadString();
						}
					}
					else
					{
						this.SdkParam = input.ReadString();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<DouYinOrderDto> _parser = new MessageParser<DouYinOrderDto>(() => new DouYinOrderDto());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int SdkParamFieldNumber = 2;

		private string sdkParam_ = "";

		public const int MessageFieldNumber = 3;

		private string message_ = "";
	}
}
