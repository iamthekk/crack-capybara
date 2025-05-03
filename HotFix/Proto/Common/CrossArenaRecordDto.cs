using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class CrossArenaRecordDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CrossArenaRecordDto> Parser
		{
			get
			{
				return CrossArenaRecordDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public long ReportId
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
		public long Time
		{
			get
			{
				return this.time_;
			}
			set
			{
				this.time_ = value;
			}
		}

		[DebuggerNonUserCode]
		public UserInfoDto UserInfo
		{
			get
			{
				return this.userInfo_;
			}
			set
			{
				this.userInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Score
		{
			get
			{
				return this.score_;
			}
			set
			{
				this.score_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool IsAtt
		{
			get
			{
				return this.isAtt_;
			}
			set
			{
				this.isAtt_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ReportId != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.ReportId);
			}
			if (this.Time != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.Time);
			}
			if (this.userInfo_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.UserInfo);
			}
			if (this.Score != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Score);
			}
			if (this.IsAtt)
			{
				output.WriteRawTag(40);
				output.WriteBool(this.IsAtt);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ReportId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.ReportId);
			}
			if (this.Time != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Time);
			}
			if (this.userInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserInfo);
			}
			if (this.Score != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Score);
			}
			if (this.IsAtt)
			{
				num += 2;
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
						this.ReportId = input.ReadInt64();
						continue;
					}
					if (num == 16U)
					{
						this.Time = input.ReadInt64();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						if (this.userInfo_ == null)
						{
							this.userInfo_ = new UserInfoDto();
						}
						input.ReadMessage(this.userInfo_);
						continue;
					}
					if (num == 32U)
					{
						this.Score = input.ReadInt32();
						continue;
					}
					if (num == 40U)
					{
						this.IsAtt = input.ReadBool();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<CrossArenaRecordDto> _parser = new MessageParser<CrossArenaRecordDto>(() => new CrossArenaRecordDto());

		public const int ReportIdFieldNumber = 1;

		private long reportId_;

		public const int TimeFieldNumber = 2;

		private long time_;

		public const int UserInfoFieldNumber = 3;

		private UserInfoDto userInfo_;

		public const int ScoreFieldNumber = 4;

		private int score_;

		public const int IsAttFieldNumber = 5;

		private bool isAtt_;
	}
}
