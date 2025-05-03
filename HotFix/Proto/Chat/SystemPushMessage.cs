using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Chat
{
	public sealed class SystemPushMessage : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SystemPushMessage> Parser
		{
			get
			{
				return SystemPushMessage._parser;
			}
		}

		[DebuggerNonUserCode]
		public long Cmd
		{
			get
			{
				return this.cmd_;
			}
			set
			{
				this.cmd_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string Body
		{
			get
			{
				return this.body_;
			}
			set
			{
				this.body_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Cmd != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.Cmd);
			}
			if (this.Body.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Body);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Cmd != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Cmd);
			}
			if (this.Body.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Body);
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
						this.Body = input.ReadString();
					}
				}
				else
				{
					this.Cmd = input.ReadInt64();
				}
			}
		}

		private static readonly MessageParser<SystemPushMessage> _parser = new MessageParser<SystemPushMessage>(() => new SystemPushMessage());

		public const int CmdFieldNumber = 1;

		private long cmd_;

		public const int BodyFieldNumber = 2;

		private string body_ = "";
	}
}
