using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Socket.Guild
{
	public sealed class SocketHeartBeatRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SocketHeartBeatRequest> Parser
		{
			get
			{
				return SocketHeartBeatRequest._parser;
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

		private static readonly MessageParser<SocketHeartBeatRequest> _parser = new MessageParser<SocketHeartBeatRequest>(() => new SocketHeartBeatRequest());
	}
}
