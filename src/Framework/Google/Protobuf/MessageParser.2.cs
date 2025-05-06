using System;
using System.IO;

namespace Google.Protobuf
{
	public sealed class MessageParser<T> : MessageParser where T : IMessage
	{
		public MessageParser(Func<T> factory)
			: base(() => factory())
		{
			this.factory = factory;
		}

		internal new T CreateTemplate()
		{
			return this.factory();
		}

		public new T ParseFrom(byte[] data)
		{
			ProtoPreconditions.CheckNotNull<byte[]>(data, "data");
			T t = this.factory();
			t.MergeFrom(data);
			return t;
		}

		public new T ParseFrom(ByteString data)
		{
			ProtoPreconditions.CheckNotNull<ByteString>(data, "data");
			T t = this.factory();
			t.MergeFrom(data);
			return t;
		}

		public new T ParseFrom(Stream input)
		{
			T t = this.factory();
			t.MergeFrom(input);
			return t;
		}

		public new T ParseDelimitedFrom(Stream input)
		{
			T t = this.factory();
			t.MergeDelimitedFrom(input);
			return t;
		}

		public new T ParseFrom(CodedInputStream input)
		{
			T t = this.factory();
			t.MergeFrom(input);
			return t;
		}

		private readonly Func<T> factory;
	}
}
