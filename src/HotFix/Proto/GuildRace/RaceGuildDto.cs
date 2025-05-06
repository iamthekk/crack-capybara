using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.GuildRace
{
	public sealed class RaceGuildDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RaceGuildDto> Parser
		{
			get
			{
				return RaceGuildDto._parser;
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
		public uint Dan
		{
			get
			{
				return this.dan_;
			}
			set
			{
				this.dan_ = value;
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
		public long Power
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
		public ulong OppGuildId
		{
			get
			{
				return this.oppGuildId_;
			}
			set
			{
				this.oppGuildId_ = value;
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
			if (this.Dan != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.Dan);
			}
			if (this.Score != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.Score);
			}
			if (this.Power != 0L)
			{
				output.WriteRawTag(64);
				output.WriteInt64(this.Power);
			}
			if (this.OppGuildId != 0UL)
			{
				output.WriteRawTag(72);
				output.WriteUInt64(this.OppGuildId);
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
			if (this.Dan != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Dan);
			}
			if (this.Score != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Score);
			}
			if (this.Power != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Power);
			}
			if (this.OppGuildId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.OppGuildId);
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
				else if (num <= 56U)
				{
					if (num == 40U)
					{
						this.Dan = input.ReadUInt32();
						continue;
					}
					if (num == 56U)
					{
						this.Score = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 64U)
					{
						this.Power = input.ReadInt64();
						continue;
					}
					if (num == 72U)
					{
						this.OppGuildId = input.ReadUInt64();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<RaceGuildDto> _parser = new MessageParser<RaceGuildDto>(() => new RaceGuildDto());

		public const int GuildIdFieldNumber = 1;

		private ulong guildId_;

		public const int GuildNameFieldNumber = 2;

		private string guildName_ = "";

		public const int AvatarFieldNumber = 3;

		private uint avatar_;

		public const int AvatarFrameFieldNumber = 4;

		private uint avatarFrame_;

		public const int DanFieldNumber = 5;

		private uint dan_;

		public const int ScoreFieldNumber = 7;

		private int score_;

		public const int PowerFieldNumber = 8;

		private long power_;

		public const int OppGuildIdFieldNumber = 9;

		private ulong oppGuildId_;
	}
}
