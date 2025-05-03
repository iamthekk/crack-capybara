using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Equip
{
	public sealed class EquipLevelResetRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<EquipLevelResetRequest> Parser
		{
			get
			{
				return EquipLevelResetRequest._parser;
			}
		}

		[DebuggerNonUserCode]
		public CommonParams CommonParams
		{
			get
			{
				return this.commonParams_;
			}
			set
			{
				this.commonParams_ = value;
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
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.CommonParams);
			}
			this.rowIds_.WriteTo(output, EquipLevelResetRequest._repeated_rowIds_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			return num + this.rowIds_.CalculateSize(EquipLevelResetRequest._repeated_rowIds_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U && num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						this.rowIds_.AddEntriesFrom(input, EquipLevelResetRequest._repeated_rowIds_codec);
					}
				}
				else
				{
					if (this.commonParams_ == null)
					{
						this.commonParams_ = new CommonParams();
					}
					input.ReadMessage(this.commonParams_);
				}
			}
		}

		private static readonly MessageParser<EquipLevelResetRequest> _parser = new MessageParser<EquipLevelResetRequest>(() => new EquipLevelResetRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int RowIdsFieldNumber = 2;

		private static readonly FieldCodec<ulong> _repeated_rowIds_codec = FieldCodec.ForUInt64(18U);

		private readonly RepeatedField<ulong> rowIds_ = new RepeatedField<ulong>();
	}
}
