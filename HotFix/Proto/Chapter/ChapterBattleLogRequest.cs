using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class ChapterBattleLogRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChapterBattleLogRequest> Parser
		{
			get
			{
				return ChapterBattleLogRequest._parser;
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
		public int ChapterId
		{
			get
			{
				return this.chapterId_;
			}
			set
			{
				this.chapterId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Day
		{
			get
			{
				return this.day_;
			}
			set
			{
				this.day_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int BossId
		{
			get
			{
				return this.bossId_;
			}
			set
			{
				this.bossId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string LogData
		{
			get
			{
				return this.logData_;
			}
			set
			{
				this.logData_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.ChapterId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ChapterId);
			}
			if (this.Day != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Day);
			}
			if (this.BossId != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.BossId);
			}
			if (this.LogData.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(this.LogData);
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
			if (this.ChapterId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ChapterId);
			}
			if (this.Day != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Day);
			}
			if (this.BossId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.BossId);
			}
			if (this.LogData.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.LogData);
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
					if (num == 10U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 16U)
					{
						this.ChapterId = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Day = input.ReadInt32();
						continue;
					}
					if (num == 32U)
					{
						this.BossId = input.ReadInt32();
						continue;
					}
					if (num == 42U)
					{
						this.LogData = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChapterBattleLogRequest> _parser = new MessageParser<ChapterBattleLogRequest>(() => new ChapterBattleLogRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ChapterIdFieldNumber = 2;

		private int chapterId_;

		public const int DayFieldNumber = 3;

		private int day_;

		public const int BossIdFieldNumber = 4;

		private int bossId_;

		public const int LogDataFieldNumber = 5;

		private string logData_ = "";
	}
}
