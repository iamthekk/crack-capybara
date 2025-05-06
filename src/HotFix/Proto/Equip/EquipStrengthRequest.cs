using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Equip
{
	public sealed class EquipStrengthRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<EquipStrengthRequest> Parser
		{
			get
			{
				return EquipStrengthRequest._parser;
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
		public ulong RowId
		{
			get
			{
				return this.rowId_;
			}
			set
			{
				this.rowId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<ulong> EquipRowIds
		{
			get
			{
				return this.equipRowIds_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, uint> UseItems
		{
			get
			{
				return this.useItems_;
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
			if (this.RowId != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.RowId);
			}
			this.equipRowIds_.WriteTo(output, EquipStrengthRequest._repeated_equipRowIds_codec);
			this.useItems_.WriteTo(output, EquipStrengthRequest._map_useItems_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.RowId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RowId);
			}
			num += this.equipRowIds_.CalculateSize(EquipStrengthRequest._repeated_equipRowIds_codec);
			return num + this.useItems_.CalculateSize(EquipStrengthRequest._map_useItems_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 10U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 16U)
					{
						this.RowId = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 24U || num == 26U)
					{
						this.equipRowIds_.AddEntriesFrom(input, EquipStrengthRequest._repeated_equipRowIds_codec);
						continue;
					}
					if (num == 34U)
					{
						this.useItems_.AddEntriesFrom(input, EquipStrengthRequest._map_useItems_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<EquipStrengthRequest> _parser = new MessageParser<EquipStrengthRequest>(() => new EquipStrengthRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int RowIdFieldNumber = 2;

		private ulong rowId_;

		public const int EquipRowIdsFieldNumber = 3;

		private static readonly FieldCodec<ulong> _repeated_equipRowIds_codec = FieldCodec.ForUInt64(26U);

		private readonly RepeatedField<ulong> equipRowIds_ = new RepeatedField<ulong>();

		public const int UseItemsFieldNumber = 4;

		private static readonly MapField<uint, uint>.Codec _map_useItems_codec = new MapField<uint, uint>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForUInt32(16U), 34U);

		private readonly MapField<uint, uint> useItems_ = new MapField<uint, uint>();
	}
}
