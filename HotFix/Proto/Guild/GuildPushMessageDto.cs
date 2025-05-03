using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Guild
{
	public sealed class GuildPushMessageDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildPushMessageDto> Parser
		{
			get
			{
				return GuildPushMessageDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint MessageType
		{
			get
			{
				return this.messageType_;
			}
			set
			{
				this.messageType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string MessageContent
		{
			get
			{
				return this.messageContent_;
			}
			set
			{
				this.messageContent_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.MessageType != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.MessageType);
			}
			if (this.MessageContent.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.MessageContent);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.MessageType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.MessageType);
			}
			if (this.MessageContent.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.MessageContent);
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
						this.MessageContent = input.ReadString();
					}
				}
				else
				{
					this.MessageType = input.ReadUInt32();
				}
			}
		}

		private static readonly MessageParser<GuildPushMessageDto> _parser = new MessageParser<GuildPushMessageDto>(() => new GuildPushMessageDto());

		public const int MessageTypeFieldNumber = 1;

		private uint messageType_;

		public const int MessageContentFieldNumber = 2;

		private string messageContent_ = "";
	}
}
