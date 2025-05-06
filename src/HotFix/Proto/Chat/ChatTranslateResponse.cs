using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Chat
{
	public sealed class ChatTranslateResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChatTranslateResponse> Parser
		{
			get
			{
				return ChatTranslateResponse._parser;
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
		public string Context
		{
			get
			{
				return this.context_;
			}
			set
			{
				this.context_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.Context.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Context);
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
			if (this.Context.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Context);
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
						this.Context = input.ReadString();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<ChatTranslateResponse> _parser = new MessageParser<ChatTranslateResponse>(() => new ChatTranslateResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int ContextFieldNumber = 2;

		private string context_ = "";
	}
}
