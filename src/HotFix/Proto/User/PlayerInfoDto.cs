using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.User
{
	public sealed class PlayerInfoDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PlayerInfoDto> Parser
		{
			get
			{
				return PlayerInfoDto._parser;
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
		public ulong LastLoginTimestamp
		{
			get
			{
				return this.lastLoginTimestamp_;
			}
			set
			{
				this.lastLoginTimestamp_ = value;
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
		public string GuildId
		{
			get
			{
				return this.guildId_;
			}
			set
			{
				this.guildId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public int SlaveCount
		{
			get
			{
				return this.slaveCount_;
			}
			set
			{
				this.slaveCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ChapterId
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
		public int WaveIndex
		{
			get
			{
				return this.waveIndex_;
			}
			set
			{
				this.waveIndex_ = value;
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
		public RepeatedField<EquipmentDto> Equipments
		{
			get
			{
				return this.equipments_;
			}
		}

		[DebuggerNonUserCode]
		public HeroDto Hero
		{
			get
			{
				return this.hero_;
			}
			set
			{
				this.hero_ = value;
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
		public uint SkinHeaddressId
		{
			get
			{
				return this.skinHeaddressId_;
			}
			set
			{
				this.skinHeaddressId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint SkinBodyId
		{
			get
			{
				return this.skinBodyId_;
			}
			set
			{
				this.skinBodyId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint SkinAccessoryId
		{
			get
			{
				return this.skinAccessoryId_;
			}
			set
			{
				this.skinAccessoryId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int TalentId
		{
			get
			{
				return this.talentId_;
			}
			set
			{
				this.talentId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<PetDto> Pets
		{
			get
			{
				return this.pets_;
			}
		}

		[DebuggerNonUserCode]
		public MountInfo MountInfo
		{
			get
			{
				return this.mountInfo_;
			}
			set
			{
				this.mountInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public ArtifactInfo ArtifactInfo
		{
			get
			{
				return this.artifactInfo_;
			}
			set
			{
				this.artifactInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint GuildPosition
		{
			get
			{
				return this.guildPosition_;
			}
			set
			{
				this.guildPosition_ = value;
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
		public ulong Timestamp
		{
			get
			{
				return this.timestamp_;
			}
			set
			{
				this.timestamp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public MapField<int, double> Attributes
		{
			get
			{
				return this.attributes_;
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
			if (this.NickName.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.NickName);
			}
			if (this.UserId != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.UserId);
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
			if (this.LastLoginTimestamp != 0UL)
			{
				output.WriteRawTag(40);
				output.WriteUInt64(this.LastLoginTimestamp);
			}
			if (this.GuildName.Length != 0)
			{
				output.WriteRawTag(50);
				output.WriteString(this.GuildName);
			}
			if (this.GuildId.Length != 0)
			{
				output.WriteRawTag(58);
				output.WriteString(this.GuildId);
			}
			if (this.SlaveCount != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.SlaveCount);
			}
			if (this.ChapterId != 0)
			{
				output.WriteRawTag(72);
				output.WriteInt32(this.ChapterId);
			}
			if (this.WaveIndex != 0)
			{
				output.WriteRawTag(80);
				output.WriteInt32(this.WaveIndex);
			}
			if (this.Power != 0L)
			{
				output.WriteRawTag(88);
				output.WriteInt64(this.Power);
			}
			this.equipments_.WriteTo(output, PlayerInfoDto._repeated_equipments_codec);
			if (this.hero_ != null)
			{
				output.WriteRawTag(106);
				output.WriteMessage(this.Hero);
			}
			if (this.extra_ != null)
			{
				output.WriteRawTag(114);
				output.WriteMessage(this.Extra);
			}
			if (this.SkinHeaddressId != 0U)
			{
				output.WriteRawTag(120);
				output.WriteUInt32(this.SkinHeaddressId);
			}
			if (this.SkinBodyId != 0U)
			{
				output.WriteRawTag(128, 1);
				output.WriteUInt32(this.SkinBodyId);
			}
			if (this.SkinAccessoryId != 0U)
			{
				output.WriteRawTag(136, 1);
				output.WriteUInt32(this.SkinAccessoryId);
			}
			if (this.TalentId != 0)
			{
				output.WriteRawTag(144, 1);
				output.WriteInt32(this.TalentId);
			}
			this.pets_.WriteTo(output, PlayerInfoDto._repeated_pets_codec);
			if (this.mountInfo_ != null)
			{
				output.WriteRawTag(162, 1);
				output.WriteMessage(this.MountInfo);
			}
			if (this.artifactInfo_ != null)
			{
				output.WriteRawTag(170, 1);
				output.WriteMessage(this.ArtifactInfo);
			}
			if (this.GuildPosition != 0U)
			{
				output.WriteRawTag(176, 1);
				output.WriteUInt32(this.GuildPosition);
			}
			if (this.ServerId != 0U)
			{
				output.WriteRawTag(184, 1);
				output.WriteUInt32(this.ServerId);
			}
			if (this.Timestamp != 0UL)
			{
				output.WriteRawTag(192, 1);
				output.WriteUInt64(this.Timestamp);
			}
			this.attributes_.WriteTo(output, PlayerInfoDto._map_attributes_codec);
			if (this.TitleId != 0U)
			{
				output.WriteRawTag(208, 1);
				output.WriteUInt32(this.TitleId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.NickName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.NickName);
			}
			if (this.UserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId);
			}
			if (this.Avatar != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Avatar);
			}
			if (this.AvatarFrame != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.AvatarFrame);
			}
			if (this.LastLoginTimestamp != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.LastLoginTimestamp);
			}
			if (this.GuildName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GuildName);
			}
			if (this.GuildId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GuildId);
			}
			if (this.SlaveCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.SlaveCount);
			}
			if (this.ChapterId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ChapterId);
			}
			if (this.WaveIndex != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.WaveIndex);
			}
			if (this.Power != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.Power);
			}
			num += this.equipments_.CalculateSize(PlayerInfoDto._repeated_equipments_codec);
			if (this.hero_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Hero);
			}
			if (this.extra_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Extra);
			}
			if (this.SkinHeaddressId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SkinHeaddressId);
			}
			if (this.SkinBodyId != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.SkinBodyId);
			}
			if (this.SkinAccessoryId != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.SkinAccessoryId);
			}
			if (this.TalentId != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.TalentId);
			}
			num += this.pets_.CalculateSize(PlayerInfoDto._repeated_pets_codec);
			if (this.mountInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.MountInfo);
			}
			if (this.artifactInfo_ != null)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.ArtifactInfo);
			}
			if (this.GuildPosition != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.GuildPosition);
			}
			if (this.ServerId != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.ServerId);
			}
			if (this.Timestamp != 0UL)
			{
				num += 2 + CodedOutputStream.ComputeUInt64Size(this.Timestamp);
			}
			num += this.attributes_.CalculateSize(PlayerInfoDto._map_attributes_codec);
			if (this.TitleId != 0U)
			{
				num += 2 + CodedOutputStream.ComputeUInt32Size(this.TitleId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 106U)
				{
					if (num <= 50U)
					{
						if (num <= 24U)
						{
							if (num == 10U)
							{
								this.NickName = input.ReadString();
								continue;
							}
							if (num == 16U)
							{
								this.UserId = input.ReadInt64();
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
								this.LastLoginTimestamp = input.ReadUInt64();
								continue;
							}
							if (num == 50U)
							{
								this.GuildName = input.ReadString();
								continue;
							}
						}
					}
					else if (num <= 72U)
					{
						if (num == 58U)
						{
							this.GuildId = input.ReadString();
							continue;
						}
						if (num == 64U)
						{
							this.SlaveCount = input.ReadInt32();
							continue;
						}
						if (num == 72U)
						{
							this.ChapterId = input.ReadInt32();
							continue;
						}
					}
					else if (num <= 88U)
					{
						if (num == 80U)
						{
							this.WaveIndex = input.ReadInt32();
							continue;
						}
						if (num == 88U)
						{
							this.Power = input.ReadInt64();
							continue;
						}
					}
					else
					{
						if (num == 98U)
						{
							this.equipments_.AddEntriesFrom(input, PlayerInfoDto._repeated_equipments_codec);
							continue;
						}
						if (num == 106U)
						{
							if (this.hero_ == null)
							{
								this.hero_ = new HeroDto();
							}
							input.ReadMessage(this.hero_);
							continue;
						}
					}
				}
				else if (num <= 154U)
				{
					if (num <= 128U)
					{
						if (num == 114U)
						{
							if (this.extra_ == null)
							{
								this.extra_ = new LordDto();
							}
							input.ReadMessage(this.extra_);
							continue;
						}
						if (num == 120U)
						{
							this.SkinHeaddressId = input.ReadUInt32();
							continue;
						}
						if (num == 128U)
						{
							this.SkinBodyId = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 136U)
						{
							this.SkinAccessoryId = input.ReadUInt32();
							continue;
						}
						if (num == 144U)
						{
							this.TalentId = input.ReadInt32();
							continue;
						}
						if (num == 154U)
						{
							this.pets_.AddEntriesFrom(input, PlayerInfoDto._repeated_pets_codec);
							continue;
						}
					}
				}
				else if (num <= 176U)
				{
					if (num == 162U)
					{
						if (this.mountInfo_ == null)
						{
							this.mountInfo_ = new MountInfo();
						}
						input.ReadMessage(this.mountInfo_);
						continue;
					}
					if (num == 170U)
					{
						if (this.artifactInfo_ == null)
						{
							this.artifactInfo_ = new ArtifactInfo();
						}
						input.ReadMessage(this.artifactInfo_);
						continue;
					}
					if (num == 176U)
					{
						this.GuildPosition = input.ReadUInt32();
						continue;
					}
				}
				else if (num <= 192U)
				{
					if (num == 184U)
					{
						this.ServerId = input.ReadUInt32();
						continue;
					}
					if (num == 192U)
					{
						this.Timestamp = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 202U)
					{
						this.attributes_.AddEntriesFrom(input, PlayerInfoDto._map_attributes_codec);
						continue;
					}
					if (num == 208U)
					{
						this.TitleId = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<PlayerInfoDto> _parser = new MessageParser<PlayerInfoDto>(() => new PlayerInfoDto());

		public const int NickNameFieldNumber = 1;

		private string nickName_ = "";

		public const int UserIdFieldNumber = 2;

		private long userId_;

		public const int AvatarFieldNumber = 3;

		private uint avatar_;

		public const int AvatarFrameFieldNumber = 4;

		private uint avatarFrame_;

		public const int LastLoginTimestampFieldNumber = 5;

		private ulong lastLoginTimestamp_;

		public const int GuildNameFieldNumber = 6;

		private string guildName_ = "";

		public const int GuildIdFieldNumber = 7;

		private string guildId_ = "";

		public const int SlaveCountFieldNumber = 8;

		private int slaveCount_;

		public const int ChapterIdFieldNumber = 9;

		private int chapterId_;

		public const int WaveIndexFieldNumber = 10;

		private int waveIndex_;

		public const int PowerFieldNumber = 11;

		private long power_;

		public const int EquipmentsFieldNumber = 12;

		private static readonly FieldCodec<EquipmentDto> _repeated_equipments_codec = FieldCodec.ForMessage<EquipmentDto>(98U, EquipmentDto.Parser);

		private readonly RepeatedField<EquipmentDto> equipments_ = new RepeatedField<EquipmentDto>();

		public const int HeroFieldNumber = 13;

		private HeroDto hero_;

		public const int ExtraFieldNumber = 14;

		private LordDto extra_;

		public const int SkinHeaddressIdFieldNumber = 15;

		private uint skinHeaddressId_;

		public const int SkinBodyIdFieldNumber = 16;

		private uint skinBodyId_;

		public const int SkinAccessoryIdFieldNumber = 17;

		private uint skinAccessoryId_;

		public const int TalentIdFieldNumber = 18;

		private int talentId_;

		public const int PetsFieldNumber = 19;

		private static readonly FieldCodec<PetDto> _repeated_pets_codec = FieldCodec.ForMessage<PetDto>(154U, PetDto.Parser);

		private readonly RepeatedField<PetDto> pets_ = new RepeatedField<PetDto>();

		public const int MountInfoFieldNumber = 20;

		private MountInfo mountInfo_;

		public const int ArtifactInfoFieldNumber = 21;

		private ArtifactInfo artifactInfo_;

		public const int GuildPositionFieldNumber = 22;

		private uint guildPosition_;

		public const int ServerIdFieldNumber = 23;

		private uint serverId_;

		public const int TimestampFieldNumber = 24;

		private ulong timestamp_;

		public const int AttributesFieldNumber = 25;

		private static readonly MapField<int, double>.Codec _map_attributes_codec = new MapField<int, double>.Codec(FieldCodec.ForInt32(8U), FieldCodec.ForDouble(17U), 202U);

		private readonly MapField<int, double> attributes_ = new MapField<int, double>();

		public const int TitleIdFieldNumber = 26;

		private uint titleId_;
	}
}
