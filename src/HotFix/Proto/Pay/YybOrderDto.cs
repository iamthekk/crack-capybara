using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Pay
{
	public sealed class YybOrderDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<YybOrderDto> Parser
		{
			get
			{
				return YybOrderDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public string ZoneId
		{
			get
			{
				return this.zoneId_;
			}
			set
			{
				this.zoneId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string GoodsTokenUrl
		{
			get
			{
				return this.goodsTokenUrl_;
			}
			set
			{
				this.goodsTokenUrl_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ZoneId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.ZoneId);
			}
			if (this.GoodsTokenUrl.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.GoodsTokenUrl);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ZoneId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ZoneId);
			}
			if (this.GoodsTokenUrl.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GoodsTokenUrl);
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
						input.SkipLastField();
					}
					else
					{
						this.GoodsTokenUrl = input.ReadString();
					}
				}
				else
				{
					this.ZoneId = input.ReadString();
				}
			}
		}

		private static readonly MessageParser<YybOrderDto> _parser = new MessageParser<YybOrderDto>(() => new YybOrderDto());

		public const int ZoneIdFieldNumber = 1;

		private string zoneId_ = "";

		public const int GoodsTokenUrlFieldNumber = 2;

		private string goodsTokenUrl_ = "";
	}
}
