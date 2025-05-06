using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class UserMission : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserMission> Parser
		{
			get
			{
				return UserMission._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint MissionId
		{
			get
			{
				return this.missionId_;
			}
			set
			{
				this.missionId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong HangUpTimestamp
		{
			get
			{
				return this.hangUpTimestamp_;
			}
			set
			{
				this.hangUpTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint QuickHangUpCount
		{
			get
			{
				return this.quickHangUpCount_;
			}
			set
			{
				this.quickHangUpCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong QuickHangUpTimestamp
		{
			get
			{
				return this.quickHangUpTimestamp_;
			}
			set
			{
				this.quickHangUpTimestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.MissionId != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.MissionId);
			}
			if (this.HangUpTimestamp != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.HangUpTimestamp);
			}
			if (this.QuickHangUpCount != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.QuickHangUpCount);
			}
			if (this.QuickHangUpTimestamp != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.QuickHangUpTimestamp);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.MissionId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.MissionId);
			}
			if (this.HangUpTimestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.HangUpTimestamp);
			}
			if (this.QuickHangUpCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.QuickHangUpCount);
			}
			if (this.QuickHangUpTimestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.QuickHangUpTimestamp);
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
						this.MissionId = input.ReadUInt32();
						continue;
					}
					if (num == 16U)
					{
						this.HangUpTimestamp = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.QuickHangUpCount = input.ReadUInt32();
						continue;
					}
					if (num == 32U)
					{
						this.QuickHangUpTimestamp = input.ReadUInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UserMission> _parser = new MessageParser<UserMission>(() => new UserMission());

		public const int MissionIdFieldNumber = 1;

		private uint missionId_;

		public const int HangUpTimestampFieldNumber = 2;

		private ulong hangUpTimestamp_;

		public const int QuickHangUpCountFieldNumber = 3;

		private uint quickHangUpCount_;

		public const int QuickHangUpTimestampFieldNumber = 4;

		private ulong quickHangUpTimestamp_;
	}
}
