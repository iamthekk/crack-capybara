using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Chat
{
	public sealed class ChatTranslateRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChatTranslateRequest> Parser
		{
			get
			{
				return ChatTranslateRequest._parser;
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
		public uint SourceLanguageId
		{
			get
			{
				return this.sourceLanguageId_;
			}
			set
			{
				this.sourceLanguageId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TargetLanguageId
		{
			get
			{
				return this.targetLanguageId_;
			}
			set
			{
				this.targetLanguageId_ = value;
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
			if (this.SourceLanguageId != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.SourceLanguageId);
			}
			if (this.TargetLanguageId != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.TargetLanguageId);
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
			if (this.SourceLanguageId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SourceLanguageId);
			}
			if (this.TargetLanguageId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TargetLanguageId);
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
						this.Context = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.SourceLanguageId = input.ReadUInt32();
						continue;
					}
					if (num == 32U)
					{
						this.TargetLanguageId = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChatTranslateRequest> _parser = new MessageParser<ChatTranslateRequest>(() => new ChatTranslateRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ContextFieldNumber = 2;

		private string context_ = "";

		public const int SourceLanguageIdFieldNumber = 3;

		private uint sourceLanguageId_;

		public const int TargetLanguageIdFieldNumber = 4;

		private uint targetLanguageId_;
	}
}
