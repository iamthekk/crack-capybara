using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class Uint32List : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<Uint32List> Parser
		{
			get
			{
				return Uint32List._parser;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<uint> Values
		{
			get
			{
				return this.values_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.values_.WriteTo(output, Uint32List._repeated_values_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			return 0 + this.values_.CalculateSize(Uint32List._repeated_values_codec);
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
					this.values_.AddEntriesFrom(input, Uint32List._repeated_values_codec);
				}
			}
		}

		private static readonly MessageParser<Uint32List> _parser = new MessageParser<Uint32List>(() => new Uint32List());

		public const int ValuesFieldNumber = 1;

		private static readonly FieldCodec<uint> _repeated_values_codec = FieldCodec.ForUInt32(10U);

		private readonly RepeatedField<uint> values_ = new RepeatedField<uint>();
	}
}
