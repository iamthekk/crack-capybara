using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Socket.Guild
{
	public sealed class SocketReconnectMessage : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SocketReconnectMessage> Parser
		{
			get
			{
				return SocketReconnectMessage._parser;
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

		private static readonly MessageParser<SocketReconnectMessage> _parser = new MessageParser<SocketReconnectMessage>(() => new SocketReconnectMessage());
	}
}
