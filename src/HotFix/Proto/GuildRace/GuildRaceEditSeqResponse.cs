using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.GuildRace
{
	public sealed class GuildRaceEditSeqResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildRaceEditSeqResponse> Parser
		{
			get
			{
				return GuildRaceEditSeqResponse._parser;
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
		public long OpUserId
		{
			get
			{
				return this.opUserId_;
			}
			set
			{
				this.opUserId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long TargetUserId
		{
			get
			{
				return this.targetUserId_;
			}
			set
			{
				this.targetUserId_ = value;
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
			if (this.commonData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonData);
			}
			if (this.OpUserId != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.OpUserId);
			}
			if (this.TargetUserId != 0L)
			{
				output.WriteRawTag(32);
				output.WriteInt64(this.TargetUserId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			if (this.OpUserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.OpUserId);
			}
			if (this.TargetUserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.TargetUserId);
			}
			return num;
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
					if (num == 18U)
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.OpUserId = input.ReadInt64();
						continue;
					}
					if (num == 32U)
					{
						this.TargetUserId = input.ReadInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildRaceEditSeqResponse> _parser = new MessageParser<GuildRaceEditSeqResponse>(() => new GuildRaceEditSeqResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int OpUserIdFieldNumber = 3;

		private long opUserId_;

		public const int TargetUserIdFieldNumber = 4;

		private long targetUserId_;
	}
}
