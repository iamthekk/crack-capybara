using System;
using System.IO;

namespace Google.Protobuf
{
	public class MessageParser
	{
		internal MessageParser(Func<IMessage> factory)
		{
			this.factory = factory;
		}

		internal IMessage CreateTemplate()
		{
			return this.factory();
		}

		public IMessage ParseFrom(byte[] data)
		{
			ProtoPreconditions.CheckNotNull<byte[]>(data, "data");
			IMessage message = this.factory();
			message.MergeFrom(data);
			return message;
		}

		public IMessage ParseFrom(ByteString data)
		{
			ProtoPreconditions.CheckNotNull<ByteString>(data, "data");
			IMessage message = this.factory();
			message.MergeFrom(data);
			return message;
		}

		public IMessage ParseFrom(Stream input)
		{
			IMessage message = this.factory();
			message.MergeFrom(input);
			return message;
		}

		public IMessage ParseDelimitedFrom(Stream input)
		{
			IMessage message = this.factory();
			message.MergeDelimitedFrom(input);
			return message;
		}

		public IMessage ParseFrom(CodedInputStream input)
		{
			IMessage message = this.factory();
			message.MergeFrom(input);
			return message;
		}

		private Func<IMessage> factory;
	}
}
