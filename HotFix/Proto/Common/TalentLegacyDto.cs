using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class TalentLegacyDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TalentLegacyDto> Parser
		{
			get
			{
				return TalentLegacyDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public int TalentLegacyId
		{
			get
			{
				return this.talentLegacyId_;
			}
			set
			{
				this.talentLegacyId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Level
		{
			get
			{
				return this.level_;
			}
			set
			{
				this.level_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long LevelUpTime
		{
			get
			{
				return this.levelUpTime_;
			}
			set
			{
				this.levelUpTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.TalentLegacyId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.TalentLegacyId);
			}
			if (this.Level != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Level);
			}
			if (this.LevelUpTime != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.LevelUpTime);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.TalentLegacyId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TalentLegacyId);
			}
			if (this.Level != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Level);
			}
			if (this.LevelUpTime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.LevelUpTime);
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
					if (num != 16U)
					{
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.LevelUpTime = input.ReadInt64();
						}
					}
					else
					{
						this.Level = input.ReadInt32();
					}
				}
				else
				{
					this.TalentLegacyId = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<TalentLegacyDto> _parser = new MessageParser<TalentLegacyDto>(() => new TalentLegacyDto());

		public const int TalentLegacyIdFieldNumber = 1;

		private int talentLegacyId_;

		public const int LevelFieldNumber = 2;

		private int level_;

		public const int LevelUpTimeFieldNumber = 3;

		private long levelUpTime_;
	}
}
