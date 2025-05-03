using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Guild
{
	public sealed class GuildBossRankDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildBossRankDto> Parser
		{
			get
			{
				return GuildBossRankDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public GuildMemberInfoDto GuildMemberInfo
		{
			get
			{
				return this.guildMemberInfo_;
			}
			set
			{
				this.guildMemberInfo_ = value;
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
		public uint Rank
		{
			get
			{
				return this.rank_;
			}
			set
			{
				this.rank_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.guildMemberInfo_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.GuildMemberInfo);
			}
			if (this.Damage != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.Damage);
			}
			if (this.Rank != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Rank);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.guildMemberInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GuildMemberInfo);
			}
			if (this.Damage != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Damage);
			}
			if (this.Rank != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Rank);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U)
					{
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.Rank = input.ReadUInt32();
						}
					}
					else
					{
						this.Damage = input.ReadUInt64();
					}
				}
				else
				{
					if (this.guildMemberInfo_ == null)
					{
						this.guildMemberInfo_ = new GuildMemberInfoDto();
					}
					input.ReadMessage(this.guildMemberInfo_);
				}
			}
		}

		private static readonly MessageParser<GuildBossRankDto> _parser = new MessageParser<GuildBossRankDto>(() => new GuildBossRankDto());

		public const int GuildMemberInfoFieldNumber = 1;

		private GuildMemberInfoDto guildMemberInfo_;

		public const int DamageFieldNumber = 2;

		private ulong damage_;

		public const int RankFieldNumber = 3;

		private uint rank_;
	}
}
