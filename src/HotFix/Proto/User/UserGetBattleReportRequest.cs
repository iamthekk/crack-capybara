using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserGetBattleReportRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserGetBattleReportRequest> Parser
		{
			get
			{
				return UserGetBattleReportRequest._parser;
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
		public ulong ReportId
		{
			get
			{
				return this.reportId_;
			}
			set
			{
				this.reportId_ = value;
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
			if (this.ReportId != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.ReportId);
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
			if (this.ReportId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.ReportId);
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
						input.SkipLastField();
					}
					else
					{
						this.ReportId = input.ReadUInt64();
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

		private static readonly MessageParser<UserGetBattleReportRequest> _parser = new MessageParser<UserGetBattleReportRequest>(() => new UserGetBattleReportRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ReportIdFieldNumber = 2;

		private ulong reportId_;
	}
}
