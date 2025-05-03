using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class GetChapterBattlePassRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GetChapterBattlePassRewardResponse> Parser
		{
			get
			{
				return GetChapterBattlePassRewardResponse._parser;
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
		public ChapterBattlePassDto ChapterBattlePassDto
		{
			get
			{
				return this.chapterBattlePassDto_;
			}
			set
			{
				this.chapterBattlePassDto_ = value;
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
			if (this.chapterBattlePassDto_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.ChapterBattlePassDto);
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
			if (this.chapterBattlePassDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.ChapterBattlePassDto);
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
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							if (this.chapterBattlePassDto_ == null)
							{
								this.chapterBattlePassDto_ = new ChapterBattlePassDto();
							}
							input.ReadMessage(this.chapterBattlePassDto_);
						}
					}
					else
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<GetChapterBattlePassRewardResponse> _parser = new MessageParser<GetChapterBattlePassRewardResponse>(() => new GetChapterBattlePassRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ChapterBattlePassDtoFieldNumber = 3;

		private ChapterBattlePassDto chapterBattlePassDto_;
	}
}
