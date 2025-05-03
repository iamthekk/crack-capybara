using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.GuildRace
{
	public sealed class GuildRaceUserInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildRaceUserInfoResponse> Parser
		{
			get
			{
				return GuildRaceUserInfoResponse._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Code
		{
			get
			{
				return this.code_;
			}
			set
			{
				this.code_ = value;
			}
		}

		[DebuggerNonUserCode]
		public CommonData CommonData
		{
			get
			{
				return this.commonData_;
			}
			set
			{
				this.commonData_ = value;
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
		public UserInfoDto UserInfoDto
		{
			get
			{
				return this.userInfoDto_;
			}
			set
			{
				this.userInfoDto_ = value;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.commonData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonData);
			}
			if (this.UserId != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.UserId);
			}
			if (this.userInfoDto_ != null)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.UserInfoDto);
			}
			this.equipments_.WriteTo(output, GuildRaceUserInfoResponse._repeated_equipments_codec);
			if (this.Atk != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.Atk);
			}
			if (this.Hp != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.Hp);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			if (this.UserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId);
			}
			if (this.userInfoDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserInfoDto);
			}
			num += this.equipments_.CalculateSize(GuildRaceUserInfoResponse._repeated_equipments_codec);
			if (this.Atk != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Atk);
			}
			if (this.Hp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Hp);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 24U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 18U)
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
					if (num == 24U)
					{
						this.UserId = input.ReadInt64();
						continue;
					}
				}
				else if (num <= 42U)
				{
					if (num == 34U)
					{
						if (this.userInfoDto_ == null)
						{
							this.userInfoDto_ = new UserInfoDto();
						}
						input.ReadMessage(this.userInfoDto_);
						continue;
					}
					if (num == 42U)
					{
						this.equipments_.AddEntriesFrom(input, GuildRaceUserInfoResponse._repeated_equipments_codec);
						continue;
					}
				}
				else
				{
					if (num == 48U)
					{
						this.Atk = input.ReadUInt32();
						continue;
					}
					if (num == 56U)
					{
						this.Hp = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildRaceUserInfoResponse> _parser = new MessageParser<GuildRaceUserInfoResponse>(() => new GuildRaceUserInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int UserIdFieldNumber = 3;

		private long userId_;

		public const int UserInfoDtoFieldNumber = 4;

		private UserInfoDto userInfoDto_;

		public const int EquipmentsFieldNumber = 5;

		private static readonly FieldCodec<EquipmentDto> _repeated_equipments_codec = FieldCodec.ForMessage<EquipmentDto>(42U, EquipmentDto.Parser);

		private readonly RepeatedField<EquipmentDto> equipments_ = new RepeatedField<EquipmentDto>();

		public const int AtkFieldNumber = 6;

		private uint atk_;

		public const int HpFieldNumber = 7;

		private uint hp_;
	}
}
