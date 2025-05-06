using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Socket.Guild
{
	public sealed class SocketLoginRepeatMessage : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SocketLoginRepeatMessage> Parser
		{
			get
			{
				return SocketLoginRepeatMessage._parser;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			return 0;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			while (input.ReadTag() != 0U)
			{
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<SocketLoginRepeatMessage> _parser = new MessageParser<SocketLoginRepeatMessage>(() => new SocketLoginRepeatMessage());
	}
}
