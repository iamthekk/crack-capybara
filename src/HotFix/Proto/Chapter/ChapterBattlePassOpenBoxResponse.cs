using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class ChapterBattlePassOpenBoxResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChapterBattlePassOpenBoxResponse> Parser
		{
			get
			{
				return ChapterBattlePassOpenBoxResponse._parser;
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
		public int InitGrade
		{
			get
			{
				return this.initGrade_;
			}
			set
			{
				this.initGrade_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> UpgradeInfo
		{
			get
			{
				return this.upgradeInfo_;
			}
		}

		[DebuggerNonUserCode]
		public int FinalRewardCount
		{
			get
			{
				return this.finalRewardCount_;
			}
			set
			{
				this.finalRewardCount_ = value;
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
			if (this.InitGrade != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.InitGrade);
			}
			this.upgradeInfo_.WriteTo(output, ChapterBattlePassOpenBoxResponse._repeated_upgradeInfo_codec);
			if (this.FinalRewardCount != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.FinalRewardCount);
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
			if (this.InitGrade != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.InitGrade);
			}
			num += this.upgradeInfo_.CalculateSize(ChapterBattlePassOpenBoxResponse._repeated_upgradeInfo_codec);
			if (this.FinalRewardCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.FinalRewardCount);
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
					if (num == 24U)
					{
						this.InitGrade = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 32U || num == 34U)
					{
						this.upgradeInfo_.AddEntriesFrom(input, ChapterBattlePassOpenBoxResponse._repeated_upgradeInfo_codec);
						continue;
					}
					if (num == 40U)
					{
						this.FinalRewardCount = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<ChapterBattlePassOpenBoxResponse> _parser = new MessageParser<ChapterBattlePassOpenBoxResponse>(() => new ChapterBattlePassOpenBoxResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int InitGradeFieldNumber = 3;

		private int initGrade_;

		public const int UpgradeInfoFieldNumber = 4;

		private static readonly FieldCodec<int> _repeated_upgradeInfo_codec = FieldCodec.ForInt32(34U);

		private readonly RepeatedField<int> upgradeInfo_ = new RepeatedField<int>();

		public const int FinalRewardCountFieldNumber = 5;

		private int finalRewardCount_;
	}
}
