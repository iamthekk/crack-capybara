using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildMemberInfoDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildMemberInfoDto> Parser
		{
			get
			{
				return GuildMemberInfoDto._parser;
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
		public ulong ActiveTime
		{
			get
			{
				return this.activeTime_;
			}
			set
			{
				this.activeTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Position
		{
			get
			{
				return this.position_;
			}
			set
			{
				this.position_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ChapterId
		{
			get
			{
				return this.chapterId_;
			}
			set
			{
				this.chapterId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Atk
		{
			get
			{
				return this.atk_;
			}
			set
			{
				this.atk_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Hp
		{
			get
			{
				return this.hp_;
			}
			set
			{
				this.hp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong BattlePower
		{
			get
			{
				return this.battlePower_;
			}
			set
			{
				this.battlePower_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong ApplyTime
		{
			get
			{
				return this.applyTime_;
			}
			set
			{
				this.applyTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ulong JoinTime
		{
			get
			{
				return this.joinTime_;
			}
			set
			{
				this.joinTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint DailyActive
		{
			get
			{
				return this.dailyActive_;
			}
			set
			{
				this.dailyActive_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint WeekActive
		{
			get
			{
				return this.weekActive_;
			}
			set
			{
				this.weekActive_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint HeroConfigId
		{
			get
			{
				return this.heroConfigId_;
			}
			set
			{
				this.heroConfigId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<uint> SkinItemConfigId
		{
			get
			{
				return this.skinItemConfigId_;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<uint> EquipIds
		{
			get
			{
				return this.equipIds_;
			}
		}

		[DebuggerNonUserCode]
		public LordDto Extra
		{
			get
			{
				return this.extra_;
			}
			set
			{
				this.extra_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool IsOnline
		{
			get
			{
				return this.isOnline_;
			}
			set
			{
				this.isOnline_ = value;
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
			if (this.Level != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.Level);
			}
			if (this.ActiveTime != 0UL)
			{
				output.WriteRawTag(48);
				output.WriteUInt64(this.ActiveTime);
			}
			if (this.Position != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.Position);
			}
			if (this.ChapterId != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.ChapterId);
			}
			if (this.Atk != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.Atk);
			}
			if (this.Hp != 0U)
			{
				output.WriteRawTag(80);
				output.WriteUInt32(this.Hp);
			}
			if (this.BattlePower != 0UL)
			{
				output.WriteRawTag(88);
				output.WriteUInt64(this.BattlePower);
			}
			if (this.ApplyTime != 0UL)
			{
				output.WriteRawTag(96);
				output.WriteUInt64(this.ApplyTime);
			}
			if (this.JoinTime != 0UL)
			{
				output.WriteRawTag(104);
				output.WriteUInt64(this.JoinTime);
			}
			if (this.DailyActive != 0U)
			{
				output.WriteRawTag(112);
				output.WriteUInt32(this.DailyActive);
			}
			if (this.WeekActive != 0U)
			{
				output.WriteRawTag(120);
				output.WriteUInt32(this.WeekActive);
			}
			if (this.HeroConfigId != 0U)
			{
				output.WriteRawTag(128, 1);
				output.WriteUInt32(this.HeroConfigId);
			}
			this.skinItemConfigId_.WriteTo(output, GuildMemberInfoDto._repeated_skinItemConfigId_codec);
			this.equipIds_.WriteTo(output, GuildMemberInfoDto._repeated_equipIds_codec);
			if (this.extra_ != null)
			{
				output.WriteRawTag(154, 1);
				output.WriteMessage(this.Extra);
			}
			if (this.IsOnline)
			{
				output.WriteRawTag(160, 1);
				output.WriteBool(this.IsOnline);
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
			if (this.Avatar != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Avatar);
			}
			if (this.AvatarFrame != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.AvatarFrame);
			}
			if (this.Level != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Level);
			}
			if (this.ActiveTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.ActiveTime);
			}
			if (this.Position != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Position);
			}
			if (this.ChapterId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ChapterId);
			}
			if (this.Atk != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Atk);
			}
			if (this.Hp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Hp);
			}
			if (this.BattlePower != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.BattlePower);
			}
			if (this.ApplyTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.ApplyTime);
			}
			if (this.JoinTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.JoinTime);
			}
			if (this.DailyActive != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.DailyActive);
			}
			if (this.WeekActive != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.WeekActive);
			}
			if (this.HeroConfigId != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.HeroConfigId);
			}
			num += this.skinItemConfigId_.CalculateSize(GuildMemberInfoDto._repeated_skinItemConfigId_codec);
			num += this.equipIds_.CalculateSize(GuildMemberInfoDto._repeated_equipIds_codec);
			if (this.extra_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.Extra);
			}
			if (this.IsOnline)
			{
				num += 3;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 88U)
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
								this.Avatar = input.ReadUInt32();
								continue;
							}
							if (num == 32U)
							{
								this.AvatarFrame = input.ReadUInt32();
								continue;
							}
							if (num == 40U)
							{
								this.Level = input.ReadUInt32();
								continue;
							}
						}
					}
					else if (num <= 64U)
					{
						if (num == 48U)
						{
							this.ActiveTime = input.ReadUInt64();
							continue;
						}
						if (num == 56U)
						{
							this.Position = input.ReadUInt32();
							continue;
						}
						if (num == 64U)
						{
							this.ChapterId = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 72U)
						{
							this.Atk = input.ReadUInt32();
							continue;
						}
						if (num == 80U)
						{
							this.Hp = input.ReadUInt32();
							continue;
						}
						if (num == 88U)
						{
							this.BattlePower = input.ReadUInt64();
							continue;
						}
					}
				}
				else
				{
					if (num > 128U)
					{
						if (num <= 144U)
						{
							if (num == 136U || num == 138U)
							{
								this.skinItemConfigId_.AddEntriesFrom(input, GuildMemberInfoDto._repeated_skinItemConfigId_codec);
								continue;
							}
							if (num != 144U)
							{
								goto IL_010C;
							}
						}
						else if (num != 146U)
						{
							if (num == 154U)
							{
								if (this.extra_ == null)
								{
									this.extra_ = new LordDto();
								}
								input.ReadMessage(this.extra_);
								continue;
							}
							if (num != 160U)
							{
								goto IL_010C;
							}
							this.IsOnline = input.ReadBool();
							continue;
						}
						this.equipIds_.AddEntriesFrom(input, GuildMemberInfoDto._repeated_equipIds_codec);
						continue;
					}
					if (num <= 104U)
					{
						if (num == 96U)
						{
							this.ApplyTime = input.ReadUInt64();
							continue;
						}
						if (num == 104U)
						{
							this.JoinTime = input.ReadUInt64();
							continue;
						}
					}
					else
					{
						if (num == 112U)
						{
							this.DailyActive = input.ReadUInt32();
							continue;
						}
						if (num == 120U)
						{
							this.WeekActive = input.ReadUInt32();
							continue;
						}
						if (num == 128U)
						{
							this.HeroConfigId = input.ReadUInt32();
							continue;
						}
					}
				}
				IL_010C:
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildMemberInfoDto> _parser = new MessageParser<GuildMemberInfoDto>(() => new GuildMemberInfoDto());

		public const int UserIdFieldNumber = 1;

		private long userId_;

		public const int NickNameFieldNumber = 2;

		private string nickName_ = "";

		public const int AvatarFieldNumber = 3;

		private uint avatar_;

		public const int AvatarFrameFieldNumber = 4;

		private uint avatarFrame_;

		public const int LevelFieldNumber = 5;

		private uint level_;

		public const int ActiveTimeFieldNumber = 6;

		private ulong activeTime_;

		public const int PositionFieldNumber = 7;

		private uint position_;

		public const int ChapterIdFieldNumber = 8;

		private uint chapterId_;

		public const int AtkFieldNumber = 9;

		private uint atk_;

		public const int HpFieldNumber = 10;

		private uint hp_;

		public const int BattlePowerFieldNumber = 11;

		private ulong battlePower_;

		public const int ApplyTimeFieldNumber = 12;

		private ulong applyTime_;

		public const int JoinTimeFieldNumber = 13;

		private ulong joinTime_;

		public const int DailyActiveFieldNumber = 14;

		private uint dailyActive_;

		public const int WeekActiveFieldNumber = 15;

		private uint weekActive_;

		public const int HeroConfigIdFieldNumber = 16;

		private uint heroConfigId_;

		public const int SkinItemConfigIdFieldNumber = 17;

		private static readonly FieldCodec<uint> _repeated_skinItemConfigId_codec = FieldCodec.ForUInt32(138U);

		private readonly RepeatedField<uint> skinItemConfigId_ = new RepeatedField<uint>();

		public const int EquipIdsFieldNumber = 18;

		private static readonly FieldCodec<uint> _repeated_equipIds_codec = FieldCodec.ForUInt32(146U);

		private readonly RepeatedField<uint> equipIds_ = new RepeatedField<uint>();

		public const int ExtraFieldNumber = 19;

		private LordDto extra_;

		public const int IsOnlineFieldNumber = 20;

		private bool isOnline_;
	}
}
