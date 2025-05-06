using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class CombatUnitDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CombatUnitDto> Parser
		{
			get
			{
				return CombatUnitDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public long RowId
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
		public int InstanceID
		{
			get
			{
				return this.instanceID_;
			}
			set
			{
				this.instanceID_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long Hp
		{
			get
			{
				return this.hp_;
			}
			set
			{
				this.hp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long Energy
		{
			get
			{
				return this.energy_;
			}
			set
			{
				this.energy_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RowId != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.RowId);
			}
			if (this.InstanceID != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.InstanceID);
			}
			if (this.Hp != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.Hp);
			}
			if (this.Energy != 0L)
			{
				output.WriteRawTag(32);
				output.WriteInt64(this.Energy);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RowId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.RowId);
			}
			if (this.InstanceID != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.InstanceID);
			}
			if (this.Hp != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Hp);
			}
			if (this.Energy != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Energy);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 8U)
					{
						this.RowId = input.ReadInt64();
						continue;
					}
					if (num == 16U)
					{
						this.InstanceID = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Hp = input.ReadInt64();
						continue;
					}
					if (num == 32U)
					{
						this.Energy = input.ReadInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<CombatUnitDto> _parser = new MessageParser<CombatUnitDto>(() => new CombatUnitDto());

		public const int RowIdFieldNumber = 1;

		private long rowId_;

		public const int InstanceIDFieldNumber = 2;

		private int instanceID_;

		public const int HpFieldNumber = 3;

		private long hp_;

		public const int EnergyFieldNumber = 4;

		private long energy_;
	}
}
