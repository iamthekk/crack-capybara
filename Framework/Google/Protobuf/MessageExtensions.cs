using System;
using System.IO;

namespace Google.Protobuf
{
	public static class MessageExtensions
	{
		public static void MergeFrom(this IMessage message, byte[] data)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "m_message");
			ProtoPreconditions.CheckNotNull<byte[]>(data, "data");
			CodedInputStream codedInputStream = new CodedInputStream(data);
			message.MergeFrom(codedInputStream);
			codedInputStream.CheckReadEndOfStreamTag();
		}

		public static void MergeFrom(this IMessage message, ByteString data)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "m_message");
			ProtoPreconditions.CheckNotNull<ByteString>(data, "data");
			CodedInputStream codedInputStream = data.CreateCodedInput();
			message.MergeFrom(codedInputStream);
			codedInputStream.CheckReadEndOfStreamTag();
		}

		public static void MergeFrom(this IMessage message, Stream input)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "m_message");
			ProtoPreconditions.CheckNotNull<Stream>(input, "input");
			CodedInputStream codedInputStream = new CodedInputStream(input);
			message.MergeFrom(codedInputStream);
			codedInputStream.CheckReadEndOfStreamTag();
		}

		public static void MergeDelimitedFrom(this IMessage message, Stream input)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "m_message");
			ProtoPreconditions.CheckNotNull<Stream>(input, "input");
			int num = (int)CodedInputStream.ReadRawVarint32(input);
			Stream stream = new LimitedInputStream(input, num);
			message.MergeFrom(stream);
		}

		public static byte[] ToByteArray(this IMessage message)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "m_message");
			byte[] array = new byte[message.CalculateSize()];
			CodedOutputStream codedOutputStream = new CodedOutputStream(array);
			message.WriteTo(codedOutputStream);
			codedOutputStream.CheckNoSpaceLeft();
			return array;
		}

		public static void WriteTo(this IMessage message, Stream output)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "m_message");
			ProtoPreconditions.CheckNotNull<Stream>(output, "output");
			CodedOutputStream codedOutputStream = new CodedOutputStream(output);
			message.WriteTo(codedOutputStream);
			codedOutputStream.Flush();
		}

		public static void WriteDelimitedTo(this IMessage message, Stream output)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "m_message");
			ProtoPreconditions.CheckNotNull<Stream>(output, "output");
			CodedOutputStream codedOutputStream = new CodedOutputStream(output);
			codedOutputStream.WriteRawVarint32((uint)message.CalculateSize());
			message.WriteTo(codedOutputStream);
			codedOutputStream.Flush();
		}

		public static ByteString ToByteString(this IMessage message)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "m_message");
			return ByteString.AttachBytes(message.ToByteArray());
		}
	}
}
