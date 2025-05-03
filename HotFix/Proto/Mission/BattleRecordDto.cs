using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Mission
{
	public sealed class BattleRecordDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<BattleRecordDto> Parser
		{
			get
			{
				return BattleRecordDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public long Tm
		{
			get
			{
				return this.tm_;
			}
			set
			{
				this.tm_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long Damage
		{
			get
			{
				return this.damage_;
			}
			set
			{
				this.damage_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Tm != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.Tm);
			}
			if (this.Damage != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.Damage);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Tm != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Tm);
			}
			if (this.Damage != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Damage);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 16U)
					{
						input.SkipLastField();
					}
					else
					{
						this.Damage = input.ReadInt64();
					}
				}
				else
				{
					this.Tm = input.ReadInt64();
				}
			}
		}

		private static readonly MessageParser<BattleRecordDto> _parser = new MessageParser<BattleRecordDto>(() => new BattleRecordDto());

		public const int TmFieldNumber = 1;

		private long tm_;

		public const int DamageFieldNumber = 2;

		private long damage_;
	}
}
