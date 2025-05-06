using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.GuildRace
{
	public sealed class RaceUserDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RaceUserDto> Parser
		{
			get
			{
				return RaceUserDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public long UserId
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
		public ulong Avatar
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
		public ulong AvatarFrame
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
		public uint ServerId
		{
			get
			{
				return this.serverId_;
			}
			set
			{
				this.serverId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Score
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
		public uint Seq
		{
			get
			{
				return this.seq_;
			}
			set
			{
				this.seq_ = value;
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
		public uint Ap
		{
			get
			{
				return this.ap_;
			}
			set
			{
				this.ap_ = value;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.UserId != 0L)
			{
				output.WriteRawTag(8);
				output.WriteInt64(this.UserId);
			}
			if (this.NickName.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.NickName);
			}
			if (this.Avatar != 0UL)
			{
				output.WriteRawTag(24);
				output.WriteUInt64(this.Avatar);
			}
			if (this.AvatarFrame != 0UL)
			{
				output.WriteRawTag(32);
				output.WriteUInt64(this.AvatarFrame);
			}
			if (this.ServerId != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.ServerId);
			}
			if (this.Score != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.Score);
			}
			if (this.Seq != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.Seq);
			}
			if (this.Power != 0UL)
			{
				output.WriteRawTag(64);
				output.WriteUInt64(this.Power);
			}
			if (this.Ap != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.Ap);
			}
			if (this.GuildId != 0UL)
			{
				output.WriteRawTag(80);
				output.WriteUInt64(this.GuildId);
			}
			if (this.GuildName.Length != 0)
			{
				output.WriteRawTag(90);
				output.WriteString(this.GuildName);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.UserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId);
			}
			if (this.NickName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.NickName);
			}
			if (this.Avatar != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Avatar);
			}
			if (this.AvatarFrame != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.AvatarFrame);
			}
			if (this.ServerId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ServerId);
			}
			if (this.Score != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Score);
			}
			if (this.Seq != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Seq);
			}
			if (this.Power != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Power);
			}
			if (this.Ap != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Ap);
			}
			if (this.GuildId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.GuildId);
			}
			if (this.GuildName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GuildName);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 40U)
				{
					if (num <= 18U)
					{
						if (num == 8U)
						{
							this.UserId = input.ReadInt64();
							continue;
						}
						if (num == 18U)
						{
							this.NickName = input.ReadString();
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.Avatar = input.ReadUInt64();
							continue;
						}
						if (num == 32U)
						{
							this.AvatarFrame = input.ReadUInt64();
							continue;
						}
						if (num == 40U)
						{
							this.ServerId = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 64U)
				{
					if (num == 48U)
					{
						this.Score = input.ReadUInt32();
						continue;
					}
					if (num == 56U)
					{
						this.Seq = input.ReadUInt32();
						continue;
					}
					if (num == 64U)
					{
						this.Power = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 72U)
					{
						this.Ap = input.ReadUInt32();
						continue;
					}
					if (num == 80U)
					{
						this.GuildId = input.ReadUInt64();
						continue;
					}
					if (num == 90U)
					{
						this.GuildName = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<RaceUserDto> _parser = new MessageParser<RaceUserDto>(() => new RaceUserDto());

		public const int UserIdFieldNumber = 1;

		private long userId_;

		public const int NickNameFieldNumber = 2;

		private string nickName_ = "";

		public const int AvatarFieldNumber = 3;

		private ulong avatar_;

		public const int AvatarFrameFieldNumber = 4;

		private ulong avatarFrame_;

		public const int ServerIdFieldNumber = 5;

		private uint serverId_;

		public const int ScoreFieldNumber = 6;

		private uint score_;

		public const int SeqFieldNumber = 7;

		private uint seq_;

		public const int PowerFieldNumber = 8;

		private ulong power_;

		public const int ApFieldNumber = 9;

		private uint ap_;

		public const int GuildIdFieldNumber = 10;

		private ulong guildId_;

		public const int GuildNameFieldNumber = 11;

		private string guildName_ = "";
	}
}
