using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class ActiveInfo : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ActiveInfo> Parser
		{
			get
			{
				return ActiveInfo._parser;
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
		public uint ActiveType
		{
			get
			{
				return this.activeType_;
			}
			set
			{
				this.activeType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ActiveId
		{
			get
			{
				return this.activeId_;
			}
			set
			{
				this.activeId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong StartTime
		{
			get
			{
				return this.startTime_;
			}
			set
			{
				this.startTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong EndTime
		{
			get
			{
				return this.endTime_;
			}
			set
			{
				this.endTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Socre
		{
			get
			{
				return this.socre_;
			}
			set
			{
				this.socre_ = value;
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
			if (this.ActiveType != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.ActiveType);
			}
			if (this.ActiveId != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.ActiveId);
			}
			if (this.StartTime != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.StartTime);
			}
			if (this.EndTime != 0UL)
			{
				output.WriteRawTag(40);
				output.WriteUInt64(this.EndTime);
			}
			if (this.Socre != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.Socre);
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
			if (this.ActiveType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ActiveType);
			}
			if (this.ActiveId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ActiveId);
			}
			if (this.StartTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.StartTime);
			}
			if (this.EndTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.EndTime);
			}
			if (this.Socre != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Socre);
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
						this.ActiveType = input.ReadUInt32();
						continue;
					}
					if (num == 24U)
					{
						this.ActiveId = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 32U)
					{
						this.StartTime = input.ReadUInt64();
						continue;
					}
					if (num == 40U)
					{
						this.EndTime = input.ReadUInt64();
						continue;
					}
					if (num == 48U)
					{
						this.Socre = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ActiveInfo> _parser = new MessageParser<ActiveInfo>(() => new ActiveInfo());

		public const int RowIdFieldNumber = 1;

		private ulong rowId_;

		public const int ActiveTypeFieldNumber = 2;

		private uint activeType_;

		public const int ActiveIdFieldNumber = 3;

		private uint activeId_;

		public const int StartTimeFieldNumber = 4;

		private ulong startTime_;

		public const int EndTimeFieldNumber = 5;

		private ulong endTime_;

		public const int SocreFieldNumber = 6;

		private uint socre_;
	}
}
