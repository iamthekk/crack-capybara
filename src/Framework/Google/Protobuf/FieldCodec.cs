using System;

namespace Google.Protobuf
{
	public static class FieldCodec
	{
		public static FieldCodec<string> ForString(uint tag)
		{
			return new FieldCodec<string>((CodedInputStream input) => input.ReadString(), delegate(CodedOutputStream output, string value)
			{
				output.WriteString(value);
			}, new Func<string, int>(CodedOutputStream.ComputeStringSize), tag);
		}

		public static FieldCodec<ByteString> ForBytes(uint tag)
		{
			return new FieldCodec<ByteString>((CodedInputStream input) => input.ReadBytes(), delegate(CodedOutputStream output, ByteString value)
			{
				output.WriteBytes(value);
			}, new Func<ByteString, int>(CodedOutputStream.ComputeBytesSize), tag);
		}

		public static FieldCodec<bool> ForBool(uint tag)
		{
			return new FieldCodec<bool>((CodedInputStream input) => input.ReadBool(), delegate(CodedOutputStream output, bool value)
			{
				output.WriteBool(value);
			}, new Func<bool, int>(CodedOutputStream.ComputeBoolSize), tag);
		}

		public static FieldCodec<int> ForInt32(uint tag)
		{
			return new FieldCodec<int>((CodedInputStream input) => input.ReadInt32(), delegate(CodedOutputStream output, int value)
			{
				output.WriteInt32(value);
			}, new Func<int, int>(CodedOutputStream.ComputeInt32Size), tag);
		}

		public static FieldCodec<int> ForSInt32(uint tag)
		{
			return new FieldCodec<int>((CodedInputStream input) => input.ReadSInt32(), delegate(CodedOutputStream output, int value)
			{
				output.WriteSInt32(value);
			}, new Func<int, int>(CodedOutputStream.ComputeSInt32Size), tag);
		}

		public static FieldCodec<uint> ForFixed32(uint tag)
		{
			return new FieldCodec<uint>((CodedInputStream input) => input.ReadFixed32(), delegate(CodedOutputStream output, uint value)
			{
				output.WriteFixed32(value);
			}, 4, tag);
		}

		public static FieldCodec<int> ForSFixed32(uint tag)
		{
			return new FieldCodec<int>((CodedInputStream input) => input.ReadSFixed32(), delegate(CodedOutputStream output, int value)
			{
				output.WriteSFixed32(value);
			}, 4, tag);
		}

		public static FieldCodec<uint> ForUInt32(uint tag)
		{
			return new FieldCodec<uint>((CodedInputStream input) => input.ReadUInt32(), delegate(CodedOutputStream output, uint value)
			{
				output.WriteUInt32(value);
			}, new Func<uint, int>(CodedOutputStream.ComputeUInt32Size), tag);
		}

		public static FieldCodec<long> ForInt64(uint tag)
		{
			return new FieldCodec<long>((CodedInputStream input) => input.ReadInt64(), delegate(CodedOutputStream output, long value)
			{
				output.WriteInt64(value);
			}, new Func<long, int>(CodedOutputStream.ComputeInt64Size), tag);
		}

		public static FieldCodec<long> ForSInt64(uint tag)
		{
			return new FieldCodec<long>((CodedInputStream input) => input.ReadSInt64(), delegate(CodedOutputStream output, long value)
			{
				output.WriteSInt64(value);
			}, new Func<long, int>(CodedOutputStream.ComputeSInt64Size), tag);
		}

		public static FieldCodec<ulong> ForFixed64(uint tag)
		{
			return new FieldCodec<ulong>((CodedInputStream input) => input.ReadFixed64(), delegate(CodedOutputStream output, ulong value)
			{
				output.WriteFixed64(value);
			}, 8, tag);
		}

		public static FieldCodec<long> ForSFixed64(uint tag)
		{
			return new FieldCodec<long>((CodedInputStream input) => input.ReadSFixed64(), delegate(CodedOutputStream output, long value)
			{
				output.WriteSFixed64(value);
			}, 8, tag);
		}

		public static FieldCodec<ulong> ForUInt64(uint tag)
		{
			return new FieldCodec<ulong>((CodedInputStream input) => input.ReadUInt64(), delegate(CodedOutputStream output, ulong value)
			{
				output.WriteUInt64(value);
			}, new Func<ulong, int>(CodedOutputStream.ComputeUInt64Size), tag);
		}

		public static FieldCodec<float> ForFloat(uint tag)
		{
			return new FieldCodec<float>((CodedInputStream input) => input.ReadFloat(), delegate(CodedOutputStream output, float value)
			{
				output.WriteFloat(value);
			}, new Func<float, int>(CodedOutputStream.ComputeFloatSize), tag);
		}

		public static FieldCodec<double> ForDouble(uint tag)
		{
			return new FieldCodec<double>((CodedInputStream input) => input.ReadDouble(), delegate(CodedOutputStream output, double value)
			{
				output.WriteDouble(value);
			}, new Func<double, int>(CodedOutputStream.ComputeDoubleSize), tag);
		}

		public static FieldCodec<T> ForEnum<T>(uint tag, Func<T, int> toInt32, Func<int, T> fromInt32)
		{
			return new FieldCodec<T>((CodedInputStream input) => fromInt32(input.ReadEnum()), delegate(CodedOutputStream output, T value)
			{
				output.WriteEnum(toInt32(value));
			}, (T value) => CodedOutputStream.ComputeEnumSize(toInt32(value)), tag);
		}

		public static FieldCodec<T> ForMessage<T>(uint tag, MessageParser<T> parser) where T : IMessage
		{
			return new FieldCodec<T>(delegate(CodedInputStream input)
			{
				T t = parser.CreateTemplate();
				input.ReadMessage(t);
				return t;
			}, delegate(CodedOutputStream output, T value)
			{
				output.WriteMessage(value);
			}, (T message) => CodedOutputStream.ComputeMessageSize(message), tag);
		}
	}
}
