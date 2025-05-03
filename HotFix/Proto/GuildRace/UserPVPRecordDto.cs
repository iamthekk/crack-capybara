using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.GuildRace
{
	public sealed class UserPVPRecordDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserPVPRecordDto> Parser
		{
			get
			{
				return UserPVPRecordDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public long UserId1
		{
			get
			{
				return this.userId1_;
			}
			set
			{
				this.userId1_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long UserId2
		{
			get
			{
				return this.userId2_;
			}
			set
			{
				this.userId2_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Result
		{
			get
			{
				return this.result_;
			}
			set
			{
				this.result_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong RecordRowId
		{
			get
			{
				return this.recordRowId_;
			}
			set
			{
				this.recordRowId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.UserId1 != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.UserId1);
			}
			if (this.UserId2 != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.UserId2);
			}
			if (this.Result != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Result);
			}
			if (this.RecordRowId != 0UL)
			{
				output.WriteRawTag(40);
				output.WriteUInt64(this.RecordRowId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.UserId1 != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId1);
			}
			if (this.UserId2 != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId2);
			}
			if (this.Result != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Result);
			}
			if (this.RecordRowId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RecordRowId);
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
						this.UserId1 = input.ReadInt64();
						continue;
					}
					if (num == 16U)
					{
						this.UserId2 = input.ReadInt64();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Result = input.ReadUInt32();
						continue;
					}
					if (num == 40U)
					{
						this.RecordRowId = input.ReadUInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UserPVPRecordDto> _parser = new MessageParser<UserPVPRecordDto>(() => new UserPVPRecordDto());

		public const int UserId1FieldNumber = 1;

		private long userId1_;

		public const int UserId2FieldNumber = 2;

		private long userId2_;

		public const int ResultFieldNumber = 3;

		private uint result_;

		public const int RecordRowIdFieldNumber = 5;

		private ulong recordRowId_;
	}
}
