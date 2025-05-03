using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class EquipmentDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<EquipmentDto> Parser
		{
			get
			{
				return EquipmentDto._parser;
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
		public uint EquipId
		{
			get
			{
				return this.equipId_;
			}
			set
			{
				this.equipId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Level
		{
			get
			{
				return this.level_;
			}
			set
			{
				this.level_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Exp
		{
			get
			{
				return this.exp_;
			}
			set
			{
				this.exp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ComposeId
		{
			get
			{
				return this.composeId_;
			}
			set
			{
				this.composeId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Evolution
		{
			get
			{
				return this.evolution_;
			}
			set
			{
				this.evolution_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RowId != 0UL)
			{
				output.WriteRawTag(8);
				output.WriteUInt64(this.RowId);
			}
			if (this.EquipId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.EquipId);
			}
			if (this.Level != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Level);
			}
			if (this.Exp != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.Exp);
			}
			if (this.ComposeId != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.ComposeId);
			}
			if (this.Evolution != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.Evolution);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RowId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RowId);
			}
			if (this.EquipId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.EquipId);
			}
			if (this.Level != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Level);
			}
			if (this.Exp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Exp);
			}
			if (this.ComposeId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ComposeId);
			}
			if (this.Evolution != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Evolution);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 24U)
				{
					if (num == 8U)
					{
						this.RowId = input.ReadUInt64();
						continue;
					}
					if (num == 16U)
					{
						this.EquipId = input.ReadUInt32();
						continue;
					}
					if (num == 24U)
					{
						this.Level = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 32U)
					{
						this.Exp = input.ReadUInt32();
						continue;
					}
					if (num == 40U)
					{
						this.ComposeId = input.ReadUInt32();
						continue;
					}
					if (num == 48U)
					{
						this.Evolution = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<EquipmentDto> _parser = new MessageParser<EquipmentDto>(() => new EquipmentDto());

		public const int RowIdFieldNumber = 1;

		private ulong rowId_;

		public const int EquipIdFieldNumber = 2;

		private uint equipId_;

		public const int LevelFieldNumber = 3;

		private uint level_;

		public const int ExpFieldNumber = 4;

		private uint exp_;

		public const int ComposeIdFieldNumber = 5;

		private uint composeId_;

		public const int EvolutionFieldNumber = 6;

		private uint evolution_;
	}
}
