using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class GuildDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildDto> Parser
		{
			get
			{
				return GuildDto._parser;
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
			if (this.GuildIcon != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.GuildIcon);
			}
			if (this.GuildIconBg != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.GuildIconBg);
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
			if (this.GuildIcon != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GuildIcon);
			}
			if (this.GuildIconBg != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GuildIconBg);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
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
						if (num == 24U)
						{
							this.Avatar = input.ReadUInt32();
							continue;
						}
						if (num == 32U)
						{
							this.AvatarFrame = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
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
				else
				{
					if (num == 56U)
					{
						this.Level = input.ReadUInt32();
						continue;
					}
					if (num == 64U)
					{
						this.GuildIcon = input.ReadUInt32();
						continue;
					}
					if (num == 72U)
					{
						this.GuildIconBg = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildDto> _parser = new MessageParser<GuildDto>(() => new GuildDto());

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

		public const int GuildIconFieldNumber = 8;

		private uint guildIcon_;

		public const int GuildIconBgFieldNumber = 9;

		private uint guildIconBg_;
	}
}
