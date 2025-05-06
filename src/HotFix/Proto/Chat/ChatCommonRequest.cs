using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Chat
{
	public sealed class ChatCommonRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChatCommonRequest> Parser
		{
			get
			{
				return ChatCommonRequest._parser;
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
		public uint GroupType
		{
			get
			{
				return this.groupType_;
			}
			set
			{
				this.groupType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string GroupId
		{
			get
			{
				return this.groupId_;
			}
			set
			{
				this.groupId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.GroupType != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.GroupType);
			}
			if (this.GroupId.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(this.GroupId);
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
			if (this.GroupType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GroupType);
			}
			if (this.GroupId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GroupId);
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
						this.LanguageId = input.ReadUInt32();
						continue;
					}
					if (num == 32U)
					{
						this.GroupType = input.ReadUInt32();
						continue;
					}
					if (num == 42U)
					{
						this.GroupId = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChatCommonRequest> _parser = new MessageParser<ChatCommonRequest>(() => new ChatCommonRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ContextFieldNumber = 2;

		private string context_ = "";

		public const int LanguageIdFieldNumber = 3;

		private uint languageId_;

		public const int GroupTypeFieldNumber = 4;

		private uint groupType_;

		public const int GroupIdFieldNumber = 5;

		private string groupId_ = "";
	}
}
