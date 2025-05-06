using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Guild
{
	public sealed class GuildSimpleDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildSimpleDto> Parser
		{
			get
			{
				return GuildSimpleDto._parser;
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
		public uint Avatar
		{
			get
			{
				return this.avatar_;
			}
			set
			{
				this.avatar_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint AvatarFrame
		{
			get
			{
				return this.avatarFrame_;
			}
			set
			{
				this.avatarFrame_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong Power
		{
			get
			{
				return this.power_;
			}
			set
			{
				this.power_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong Damage
		{
			get
			{
				return this.damage_;
			}
			set
			{
				this.damage_ = value;
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
		public ulong UserId
		{
			get
			{
				return this.userId_;
			}
			set
			{
				this.userId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string NickName
		{
			get
			{
				return this.nickName_;
			}
			set
			{
				this.nickName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint GuildIcon
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
		public uint GuildIconBg
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
		public uint TitleId
		{
			get
			{
				return this.titleId_;
			}
			set
			{
				this.titleId_ = value;
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
			if (this.Avatar != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Avatar);
			}
			if (this.AvatarFrame != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.AvatarFrame);
			}
			if (this.Power != 0UL)
			{
				output.WriteRawTag(40);
				output.WriteUInt64(this.Power);
			}
			if (this.Damage != 0UL)
			{
				output.WriteRawTag(48);
				output.WriteUInt64(this.Damage);
			}
			if (this.Level != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.Level);
			}
			if (this.UserId != 0UL)
			{
				output.WriteRawTag(64);
				output.WriteUInt64(this.UserId);
			}
			if (this.NickName.Length != 0)
			{
				output.WriteRawTag(74);
				output.WriteString(this.NickName);
			}
			if (this.GuildIcon != 0U)
			{
				output.WriteRawTag(80);
				output.WriteUInt32(this.GuildIcon);
			}
			if (this.GuildIconBg != 0U)
			{
				output.WriteRawTag(88);
				output.WriteUInt32(this.GuildIconBg);
			}
			if (this.TitleId != 0U)
			{
				output.WriteRawTag(96);
				output.WriteUInt32(this.TitleId);
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
			if (this.Avatar != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Avatar);
			}
			if (this.AvatarFrame != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.AvatarFrame);
			}
			if (this.Power != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Power);
			}
			if (this.Damage != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Damage);
			}
			if (this.Level != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Level);
			}
			if (this.UserId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.UserId);
			}
			if (this.NickName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.NickName);
			}
			if (this.GuildIcon != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GuildIcon);
			}
			if (this.GuildIconBg != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GuildIconBg);
			}
			if (this.TitleId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TitleId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 48U)
				{
					if (num <= 24U)
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
						if (num == 24U)
						{
							this.Avatar = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 32U)
						{
							this.AvatarFrame = input.ReadUInt32();
							continue;
						}
						if (num == 40U)
						{
							this.Power = input.ReadUInt64();
							continue;
						}
						if (num == 48U)
						{
							this.Damage = input.ReadUInt64();
							continue;
						}
					}
				}
				else if (num <= 74U)
				{
					if (num == 56U)
					{
						this.Level = input.ReadUInt32();
						continue;
					}
					if (num == 64U)
					{
						this.UserId = input.ReadUInt64();
						continue;
					}
					if (num == 74U)
					{
						this.NickName = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 80U)
					{
						this.GuildIcon = input.ReadUInt32();
						continue;
					}
					if (num == 88U)
					{
						this.GuildIconBg = input.ReadUInt32();
						continue;
					}
					if (num == 96U)
					{
						this.TitleId = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildSimpleDto> _parser = new MessageParser<GuildSimpleDto>(() => new GuildSimpleDto());

		public const int GuildIdFieldNumber = 1;

		private ulong guildId_;

		public const int GuildNameFieldNumber = 2;

		private string guildName_ = "";

		public const int AvatarFieldNumber = 3;

		private uint avatar_;

		public const int AvatarFrameFieldNumber = 4;

		private uint avatarFrame_;

		public const int PowerFieldNumber = 5;

		private ulong power_;

		public const int DamageFieldNumber = 6;

		private ulong damage_;

		public const int LevelFieldNumber = 7;

		private uint level_;

		public const int UserIdFieldNumber = 8;

		private ulong userId_;

		public const int NickNameFieldNumber = 9;

		private string nickName_ = "";

		public const int GuildIconFieldNumber = 10;

		private uint guildIcon_;

		public const int GuildIconBgFieldNumber = 11;

		private uint guildIconBg_;

		public const int TitleIdFieldNumber = 12;

		private uint titleId_;
	}
}
