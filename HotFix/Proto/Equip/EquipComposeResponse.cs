using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Equip
{
	public sealed class EquipComposeResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<EquipComposeResponse> Parser
		{
			get
			{
				return EquipComposeResponse._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Code
		{
			get
			{
				return this.code_;
			}
			set
			{
				this.code_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<long> DelEquipRowId
		{
			get
			{
				return this.delEquipRowId_;
			}
		}

		[DebuggerNonUserCode]
		public CommonData CommonData
		{
			get
			{
				return this.commonData_;
			}
			set
			{
				this.commonData_ = value;
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
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			this.delEquipRowId_.WriteTo(output, EquipComposeResponse._repeated_delEquipRowId_codec);
			if (this.commonData_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.CommonData);
			}
			this.rowIds_.WriteTo(output, EquipComposeResponse._repeated_rowIds_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			num += this.delEquipRowId_.CalculateSize(EquipComposeResponse._repeated_delEquipRowId_codec);
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			return num + this.rowIds_.CalculateSize(EquipComposeResponse._repeated_rowIds_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 16U || num == 18U)
					{
						this.delEquipRowId_.AddEntriesFrom(input, EquipComposeResponse._repeated_delEquipRowId_codec);
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
					if (num == 32U || num == 34U)
					{
						this.rowIds_.AddEntriesFrom(input, EquipComposeResponse._repeated_rowIds_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<EquipComposeResponse> _parser = new MessageParser<EquipComposeResponse>(() => new EquipComposeResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int DelEquipRowIdFieldNumber = 2;

		private static readonly FieldCodec<long> _repeated_delEquipRowId_codec = FieldCodec.ForInt64(18U);

		private readonly RepeatedField<long> delEquipRowId_ = new RepeatedField<long>();

		public const int CommonDataFieldNumber = 3;

		private CommonData commonData_;

		public const int RowIdsFieldNumber = 4;

		private static readonly FieldCodec<ulong> _repeated_rowIds_codec = FieldCodec.ForUInt64(34U);

		private readonly RepeatedField<ulong> rowIds_ = new RepeatedField<ulong>();
	}
}
