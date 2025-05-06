using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.Common
{
	public sealed class UserRankInfoSimpleDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserRankInfoSimpleDto> Parser
		{
			get
			{
				return UserRankInfoSimpleDto._parser;
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
		public RepeatedField<EquipmentDto> Equips
		{
			get
			{
				return this.equips_;
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
			if (this.SkinHeaddressId != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.SkinHeaddressId);
			}
			if (this.SkinBodyId != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.SkinBodyId);
			}
			if (this.SkinAccessoryId != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.SkinAccessoryId);
			}
			this.equips_.WriteTo(output, UserRankInfoSimpleDto._repeated_equips_codec);
			if (this.mountInfo_ != null)
			{
				output.WriteRawTag(58);
				output.WriteMessage(this.MountInfo);
			}
			if (this.GuildId != 0UL)
			{
				output.WriteRawTag(64);
				output.WriteUInt64(this.GuildId);
			}
			if (this.GuildName.Length != 0)
			{
				output.WriteRawTag(74);
				output.WriteString(this.GuildName);
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
			if (this.SkinHeaddressId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SkinHeaddressId);
			}
			if (this.SkinBodyId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SkinBodyId);
			}
			if (this.SkinAccessoryId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SkinAccessoryId);
			}
			num += this.equips_.CalculateSize(UserRankInfoSimpleDto._repeated_equips_codec);
			if (this.mountInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.MountInfo);
			}
			if (this.GuildId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.GuildId);
			}
			if (this.GuildName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GuildName);
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
							this.SkinHeaddressId = input.ReadUInt32();
							continue;
						}
						if (num == 32U)
						{
							this.SkinBodyId = input.ReadUInt32();
							continue;
						}
						if (num == 40U)
						{
							this.SkinAccessoryId = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 64U)
				{
					if (num == 50U)
					{
						this.equips_.AddEntriesFrom(input, UserRankInfoSimpleDto._repeated_equips_codec);
						continue;
					}
					if (num == 58U)
					{
						if (this.mountInfo_ == null)
						{
							this.mountInfo_ = new MountInfo();
						}
						input.ReadMessage(this.mountInfo_);
						continue;
					}
					if (num == 64U)
					{
						this.GuildId = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 74U)
					{
						this.GuildName = input.ReadString();
						continue;
					}
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
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UserRankInfoSimpleDto> _parser = new MessageParser<UserRankInfoSimpleDto>(() => new UserRankInfoSimpleDto());

		public const int UserIdFieldNumber = 1;

		private long userId_;

		public const int NickNameFieldNumber = 2;

		private string nickName_ = "";

		public const int SkinHeaddressIdFieldNumber = 3;

		private uint skinHeaddressId_;

		public const int SkinBodyIdFieldNumber = 4;

		private uint skinBodyId_;

		public const int SkinAccessoryIdFieldNumber = 5;

		private uint skinAccessoryId_;

		public const int EquipsFieldNumber = 6;

		private static readonly FieldCodec<EquipmentDto> _repeated_equips_codec = FieldCodec.ForMessage<EquipmentDto>(50U, EquipmentDto.Parser);

		private readonly RepeatedField<EquipmentDto> equips_ = new RepeatedField<EquipmentDto>();

		public const int MountInfoFieldNumber = 7;

		private MountInfo mountInfo_;

		public const int GuildIdFieldNumber = 8;

		private ulong guildId_;

		public const int GuildNameFieldNumber = 9;

		private string guildName_ = "";

		public const int GuildIconFieldNumber = 10;

		private uint guildIcon_;

		public const int GuildIconBgFieldNumber = 11;

		private uint guildIconBg_;
	}
}
