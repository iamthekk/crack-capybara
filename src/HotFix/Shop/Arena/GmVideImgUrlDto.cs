using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Shop.Arena
{
	public sealed class GmVideImgUrlDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GmVideImgUrlDto> Parser
		{
			get
			{
				return GmVideImgUrlDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public string Url
		{
			get
			{
				return this.url_;
			}
			set
			{
				this.url_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Lang
		{
			get
			{
				return this.lang_;
			}
			set
			{
				this.lang_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Url.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Url);
			}
			if (this.Lang.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Lang);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Url.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Url);
			}
			if (this.Lang.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Lang);
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
						this.Lang = input.ReadString();
					}
				}
				else
				{
					this.Url = input.ReadString();
				}
			}
		}

		private static readonly MessageParser<GmVideImgUrlDto> _parser = new MessageParser<GmVideImgUrlDto>(() => new GmVideImgUrlDto());

		public const int UrlFieldNumber = 1;

		private string url_ = "";

		public const int LangFieldNumber = 2;

		private string lang_ = "";
	}
}
