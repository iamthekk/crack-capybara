using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Chat
{
	public sealed class ChatGuildRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChatGuildRequest> Parser
		{
			get
			{
				return ChatGuildRequest._parser;
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
		public uint LanguageId
		{
			get
			{
				return this.languageId_;
			}
			set
			{
				this.languageId_ = value;
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
			if (this.Context.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Context);
			}
			if (this.LanguageId != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.LanguageId);
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
			if (this.Context.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Context);
			}
			if (this.LanguageId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.LanguageId);
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
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.LanguageId = input.ReadUInt32();
						}
					}
					else
					{
						this.Context = input.ReadString();
					}
				}
				else
				{
					if (this.commonParams_ == null)
					{
						this.commonParams_ = new CommonParams();
					}
					input.ReadMessage(this.commonParams_);
				}
			}
		}

		private static readonly MessageParser<ChatGuildRequest> _parser = new MessageParser<ChatGuildRequest>(() => new ChatGuildRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ContextFieldNumber = 2;

		private string context_ = "";

		public const int LanguageIdFieldNumber = 3;

		private uint languageId_;
	}
}
