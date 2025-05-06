using System;
using System.Collections.Generic;

namespace Google.Protobuf
{
	public sealed class FieldCodec<T>
	{
		static FieldCodec()
		{
			if (typeof(T) == typeof(string))
			{
				FieldCodec<T>.DefaultDefault = (T)((object)"");
				return;
			}
			if (typeof(T) == typeof(ByteString))
			{
				FieldCodec<T>.DefaultDefault = (T)((object)ByteString.Empty);
			}
		}

		internal static bool IsPackedRepeatedField(uint tag)
		{
			return FieldCodec<T>.TypeSupportsPacking && WireFormat.GetTagWireType(tag) == WireFormat.WireType.LengthDelimited;
		}

		internal FieldCodec(Func<CodedInputStream, T> reader, Action<CodedOutputStream, T> writer, int fixedSize, uint tag)
			: this(reader, writer, (T _) => fixedSize, tag)
		{
			this.FixedSize = fixedSize;
		}

		internal FieldCodec(Func<CodedInputStream, T> reader, Action<CodedOutputStream, T> writer, Func<T, int> sizeCalculator, uint tag)
			: this(reader, writer, sizeCalculator, tag, FieldCodec<T>.DefaultDefault)
		{
		}

		internal FieldCodec(Func<CodedInputStream, T> reader, Action<CodedOutputStream, T> writer, Func<T, int> sizeCalculator, uint tag, T defaultValue)
		{
			this.ValueReader = reader;
			this.ValueWriter = writer;
			this.ValueSizeCalculator = sizeCalculator;
			this.FixedSize = 0;
			this.Tag = tag;
			this.DefaultValue = defaultValue;
			this.tagSize = CodedOutputStream.ComputeRawVarint32Size(tag);
			this.PackedRepeatedField = FieldCodec<T>.IsPackedRepeatedField(tag);
		}

		public void WriteTagAndValue(CodedOutputStream output, T value)
		{
			if (!this.IsDefault(value))
			{
				output.WriteTag(this.Tag);
				this.ValueWriter(output, value);
			}
		}

		public T Read(CodedInputStream input)
		{
			return this.ValueReader(input);
		}

		public int CalculateSizeWithTag(T value)
		{
			if (!this.IsDefault(value))
			{
				return this.ValueSizeCalculator(value) + this.tagSize;
			}
			return 0;
		}

		private bool IsDefault(T value)
		{
			return EqualityComparer<T>.Default.Equals(value, this.DefaultValue);
		}

		private static readonly T DefaultDefault;

		private static readonly bool TypeSupportsPacking = default(T) != null;

		internal readonly bool PackedRepeatedField;

		internal readonly Action<CodedOutputStream, T> ValueWriter;

		internal readonly Func<T, int> ValueSizeCalculator;

		internal readonly Func<CodedInputStream, T> ValueReader;

		internal readonly int FixedSize;

		internal readonly uint Tag;

		internal readonly T DefaultValue;

		private readonly int tagSize;
	}
}
