using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Guild
{
	public sealed class GuildInfoDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildInfoDto> Parser
		{
			get
			{
				return GuildInfoDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public ulong GuildId
		{
			get
			{
				return this.guildId_;
			}
			set
			{
				this.guildId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string GuildName
		{
			get
			{
				return this.guildName_;
			}
			set
			{
				this.guildName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string GuildIntro
		{
			get
			{
				return this.guildIntro_;
			}
			set
			{
				this.guildIntro_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public ulong GuildIcon
		{
			get
			{
				return this.guildIcon_;
			}
			set
			{
				this.guildIcon_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong GuildIconBg
		{
			get
			{
				return this.guildIconBg_;
			}
			set
			{
				this.guildIconBg_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Members
		{
			get
			{
				return this.members_;
			}
			set
			{
				this.members_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint MaxMembers
		{
			get
			{
				return this.maxMembers_;
			}
			set
			{
				this.maxMembers_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Active
		{
			get
			{
				return this.active_;
			}
			set
			{
				this.active_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Level
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
		public uint ApplyType
		{
			get
			{
				return this.applyType_;
			}
			set
			{
				this.applyType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ApplyCondition
		{
			get
			{
				return this.applyCondition_;
			}
			set
			{
				this.applyCondition_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool IsApply
		{
			get
			{
				return this.isApply_;
			}
			set
			{
				this.isApply_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Language
		{
			get
			{
				return this.language_;
			}
			set
			{
				this.language_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string GuildNotice
		{
			get
			{
				return this.guildNotice_;
			}
			set
			{
				this.guildNotice_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public long GuildPresidentUserId
		{
			get
			{
				return this.guildPresidentUserId_;
			}
			set
			{
				this.guildPresidentUserId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string GuildPresidentNickName
		{
			get
			{
				return this.guildPresidentNickName_;
			}
			set
			{
				this.guildPresidentNickName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint Exp
		{
			get
			{
				return this.exp_;
			}
			set
			{
				this.exp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong TotalPower
		{
			get
			{
				return this.totalPower_;
			}
			set
			{
				this.totalPower_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string ImGroupId
		{
			get
			{
				return this.imGroupId_;
			}
			set
			{
				this.imGroupId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.GuildId != 0UL)
			{
				output.WriteRawTag(8);
				output.WriteUInt64(this.GuildId);
			}
			if (this.GuildName.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.GuildName);
			}
			if (this.GuildIntro.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.GuildIntro);
			}
			if (this.GuildIcon != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.GuildIcon);
			}
			if (this.GuildIconBg != 0UL)
			{
				output.WriteRawTag(40);
				output.WriteUInt64(this.GuildIconBg);
			}
			if (this.Members != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.Members);
			}
			if (this.MaxMembers != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.MaxMembers);
			}
			if (this.Active != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.Active);
			}
			if (this.Level != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.Level);
			}
			if (this.ApplyType != 0U)
			{
				output.WriteRawTag(80);
				output.WriteUInt32(this.ApplyType);
			}
			if (this.ApplyCondition != 0U)
			{
				output.WriteRawTag(88);
				output.WriteUInt32(this.ApplyCondition);
			}
			if (this.IsApply)
			{
				output.WriteRawTag(96);
				output.WriteBool(this.IsApply);
			}
			if (this.Language != 0U)
			{
				output.WriteRawTag(104);
				output.WriteUInt32(this.Language);
			}
			if (this.GuildNotice.Length != 0)
			{
				output.WriteRawTag(114);
				output.WriteString(this.GuildNotice);
			}
			if (this.GuildPresidentUserId != 0L)
			{
				output.WriteRawTag(120);
				output.WriteInt64(this.GuildPresidentUserId);
			}
			if (this.GuildPresidentNickName.Length != 0)
			{
				output.WriteRawTag(130, 1);
				output.WriteString(this.GuildPresidentNickName);
			}
			if (this.Exp != 0U)
			{
				output.WriteRawTag(136, 1);
				output.WriteUInt32(this.Exp);
			}
			if (this.TotalPower != 0UL)
			{
				output.WriteRawTag(144, 1);
				output.WriteUInt64(this.TotalPower);
			}
			if (this.ImGroupId.Length != 0)
			{
				output.WriteRawTag(154, 1);
				output.WriteString(this.ImGroupId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.GuildId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.GuildId);
			}
			if (this.GuildName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GuildName);
			}
			if (this.GuildIntro.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GuildIntro);
			}
			if (this.GuildIcon != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.GuildIcon);
			}
			if (this.GuildIconBg != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.GuildIconBg);
			}
			if (this.Members != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Members);
			}
			if (this.MaxMembers != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.MaxMembers);
			}
			if (this.Active != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Active);
			}
			if (this.Level != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Level);
			}
			if (this.ApplyType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ApplyType);
			}
			if (this.ApplyCondition != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ApplyCondition);
			}
			if (this.IsApply)
			{
				num += 2;
			}
			if (this.Language != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Language);
			}
			if (this.GuildNotice.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GuildNotice);
			}
			if (this.GuildPresidentUserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.GuildPresidentUserId);
			}
			if (this.GuildPresidentNickName.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.GuildPresidentNickName);
			}
			if (this.Exp != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.Exp);
			}
			if (this.TotalPower != 0UL)
			{
				num += 2 + CodedOutputStream.ComputeUInt64Size(this.TotalPower);
			}
			if (this.ImGroupId.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.ImGroupId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 72U)
				{
					if (num <= 32U)
					{
						if (num <= 18U)
						{
							if (num == 8U)
							{
								this.GuildId = input.ReadUInt64();
								continue;
							}
							if (num == 18U)
							{
								this.GuildName = input.ReadString();
								continue;
							}
						}
						else
						{
							if (num == 26U)
							{
								this.GuildIntro = input.ReadString();
								continue;
							}
							if (num == 32U)
							{
								this.GuildIcon = input.ReadUInt64();
								continue;
							}
						}
					}
					else if (num <= 48U)
					{
						if (num == 40U)
						{
							this.GuildIconBg = input.ReadUInt64();
							continue;
						}
						if (num == 48U)
						{
							this.Members = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 56U)
						{
							this.MaxMembers = input.ReadUInt32();
							continue;
						}
						if (num == 64U)
						{
							this.Active = input.ReadUInt32();
							continue;
						}
						if (num == 72U)
						{
							this.Level = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 114U)
				{
					if (num <= 88U)
					{
						if (num == 80U)
						{
							this.ApplyType = input.ReadUInt32();
							continue;
						}
						if (num == 88U)
						{
							this.ApplyCondition = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 96U)
						{
							this.IsApply = input.ReadBool();
							continue;
						}
						if (num == 104U)
						{
							this.Language = input.ReadUInt32();
							continue;
						}
						if (num == 114U)
						{
							this.GuildNotice = input.ReadString();
							continue;
						}
					}
				}
				else if (num <= 130U)
				{
					if (num == 120U)
					{
						this.GuildPresidentUserId = input.ReadInt64();
						continue;
					}
					if (num == 130U)
					{
						this.GuildPresidentNickName = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 136U)
					{
						this.Exp = input.ReadUInt32();
						continue;
					}
					if (num == 144U)
					{
						this.TotalPower = input.ReadUInt64();
						continue;
					}
					if (num == 154U)
					{
						this.ImGroupId = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildInfoDto> _parser = new MessageParser<GuildInfoDto>(() => new GuildInfoDto());

		public const int GuildIdFieldNumber = 1;

		private ulong guildId_;

		public const int GuildNameFieldNumber = 2;

		private string guildName_ = "";

		public const int GuildIntroFieldNumber = 3;

		private string guildIntro_ = "";

		public const int GuildIconFieldNumber = 4;

		private ulong guildIcon_;

		public const int GuildIconBgFieldNumber = 5;

		private ulong guildIconBg_;

		public const int MembersFieldNumber = 6;

		private uint members_;

		public const int MaxMembersFieldNumber = 7;

		private uint maxMembers_;

		public const int ActiveFieldNumber = 8;

		private uint active_;

		public const int LevelFieldNumber = 9;

		private uint level_;

		public const int ApplyTypeFieldNumber = 10;

		private uint applyType_;

		public const int ApplyConditionFieldNumber = 11;

		private uint applyCondition_;

		public const int IsApplyFieldNumber = 12;

		private bool isApply_;

		public const int LanguageFieldNumber = 13;

		private uint language_;

		public const int GuildNoticeFieldNumber = 14;

		private string guildNotice_ = "";

		public const int GuildPresidentUserIdFieldNumber = 15;

		private long guildPresidentUserId_;

		public const int GuildPresidentNickNameFieldNumber = 16;

		private string guildPresidentNickName_ = "";

		public const int ExpFieldNumber = 17;

		private uint exp_;

		public const int TotalPowerFieldNumber = 18;

		private ulong totalPower_;

		public const int ImGroupIdFieldNumber = 19;

		private string imGroupId_ = "";
	}
}
