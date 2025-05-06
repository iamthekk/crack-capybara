using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Guild
{
	public sealed class GuildBossConfigDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildBossConfigDto> Parser
		{
			get
			{
				return GuildBossConfigDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint BossStep
		{
			get
			{
				return this.bossStep_;
			}
			set
			{
				this.bossStep_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long NowHp
		{
			get
			{
				return this.nowHp_;
			}
			set
			{
				this.nowHp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.BossStep != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.BossStep);
			}
			if (this.NowHp != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.NowHp);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.BossStep != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BossStep);
			}
			if (this.NowHp != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.NowHp);
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
						input.SkipLastField();
					}
					else
					{
						this.NowHp = input.ReadInt64();
					}
				}
				else
				{
					this.BossStep = input.ReadUInt32();
				}
			}
		}

		private static readonly MessageParser<GuildBossConfigDto> _parser = new MessageParser<GuildBossConfigDto>(() => new GuildBossConfigDto());

		public const int BossStepFieldNumber = 1;

		private uint bossStep_;

		public const int NowHpFieldNumber = 2;

		private long nowHp_;
	}
}
