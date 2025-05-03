using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserGetBattleReportResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserGetBattleReportResponse> Parser
		{
			get
			{
				return UserGetBattleReportResponse._parser;
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
		public PVPRecordDto Record
		{
			get
			{
				return this.record_;
			}
			set
			{
				this.record_ = value;
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
			if (this.record_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.Record);
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
			if (this.record_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Record);
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
					if (num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						if (this.record_ == null)
						{
							this.record_ = new PVPRecordDto();
						}
						input.ReadMessage(this.record_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<UserGetBattleReportResponse> _parser = new MessageParser<UserGetBattleReportResponse>(() => new UserGetBattleReportResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int RecordFieldNumber = 2;

		private PVPRecordDto record_;
	}
}
