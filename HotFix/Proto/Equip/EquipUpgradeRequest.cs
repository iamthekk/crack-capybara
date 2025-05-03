using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Equip
{
	public sealed class EquipUpgradeRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<EquipUpgradeRequest> Parser
		{
			get
			{
				return EquipUpgradeRequest._parser;
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
		public uint Count
		{
			get
			{
				return this.count_;
			}
			set
			{
				this.count_ = value;
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
			if (this.Count != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Count);
			}
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
			if (this.Count != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Count);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U)
					{
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.Count = input.ReadUInt32();
						}
					}
					else
					{
						this.RowId = input.ReadUInt64();
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

		private static readonly MessageParser<EquipUpgradeRequest> _parser = new MessageParser<EquipUpgradeRequest>(() => new EquipUpgradeRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int RowIdFieldNumber = 2;

		private ulong rowId_;

		public const int CountFieldNumber = 3;

		private uint count_;
	}
}
