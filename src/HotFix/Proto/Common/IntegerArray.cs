using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class IntegerArray : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<IntegerArray> Parser
		{
			get
			{
				return IntegerArray._parser;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> IntegerArray_
		{
			get
			{
				return this.integerArray_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.integerArray_.WriteTo(output, IntegerArray._repeated_integerArray_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			return 0 + this.integerArray_.CalculateSize(IntegerArray._repeated_integerArray_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U && num != 10U)
				{
					input.SkipLastField();
				}
				else
				{
					this.integerArray_.AddEntriesFrom(input, IntegerArray._repeated_integerArray_codec);
				}
			}
		}

		private static readonly MessageParser<IntegerArray> _parser = new MessageParser<IntegerArray>(() => new IntegerArray());

		public const int IntegerArray_FieldNumber = 1;

		private static readonly FieldCodec<int> _repeated_integerArray_codec = FieldCodec.ForInt32(10U);

		private readonly RepeatedField<int> integerArray_ = new RepeatedField<int>();
	}
}
