using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Socket.Guild
{
	public sealed class SocketPushMessage : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SocketPushMessage> Parser
		{
			get
			{
				return SocketPushMessage._parser;
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
		public ulong MessageSeq
		{
			get
			{
				return this.messageSeq_;
			}
			set
			{
				this.messageSeq_ = value;
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
			if (this.MessageSeq != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.MessageSeq);
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
			if (this.MessageSeq != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.MessageSeq);
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
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.MessageSeq = input.ReadUInt64();
						}
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

		private static readonly MessageParser<SocketPushMessage> _parser = new MessageParser<SocketPushMessage>(() => new SocketPushMessage());

		public const int MessageTypeFieldNumber = 1;

		private uint messageType_;

		public const int MessageContentFieldNumber = 2;

		private string messageContent_ = "";

		public const int MessageSeqFieldNumber = 3;

		private ulong messageSeq_;
	}
}
