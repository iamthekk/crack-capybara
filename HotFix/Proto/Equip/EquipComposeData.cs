using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Equip
{
	public sealed class EquipComposeData : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<EquipComposeData> Parser
		{
			get
			{
				return EquipComposeData._parser;
			}
		}

		[DebuggerNonUserCode]
		public ulong MainRowId
		{
			get
			{
				return this.mainRowId_;
			}
			set
			{
				this.mainRowId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ulong> RowIds
		{
			get
			{
				return this.rowIds_;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.MainRowId != 0UL)
			{
				output.WriteRawTag(8);
				output.WriteUInt64(this.MainRowId);
			}
			this.rowIds_.WriteTo(output, EquipComposeData._repeated_rowIds_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.MainRowId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.MainRowId);
			}
			return num + this.rowIds_.CalculateSize(EquipComposeData._repeated_rowIds_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 16U && num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						this.rowIds_.AddEntriesFrom(input, EquipComposeData._repeated_rowIds_codec);
					}
				}
				else
				{
					this.MainRowId = input.ReadUInt64();
				}
			}
		}

		private static readonly MessageParser<EquipComposeData> _parser = new MessageParser<EquipComposeData>(() => new EquipComposeData());

		public const int MainRowIdFieldNumber = 1;

		private ulong mainRowId_;

		public const int RowIdsFieldNumber = 2;

		private static readonly FieldCodec<ulong> _repeated_rowIds_codec = FieldCodec.ForUInt64(18U);

		private readonly RepeatedField<ulong> rowIds_ = new RepeatedField<ulong>();
	}
}
