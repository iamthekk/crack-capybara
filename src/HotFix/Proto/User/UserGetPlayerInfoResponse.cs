using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserGetPlayerInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserGetPlayerInfoResponse> Parser
		{
			get
			{
				return UserGetPlayerInfoResponse._parser;
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
		public long PlayerUserId
		{
			get
			{
				return this.playerUserId_;
			}
			set
			{
				this.playerUserId_ = value;
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
			if (this.PlayerUserId != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.PlayerUserId);
			}
			if (this.userInfoDto_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.UserInfoDto);
			}
			this.equipments_.WriteTo(output, UserGetPlayerInfoResponse._repeated_equipments_codec);
			if (this.Atk != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.Atk);
			}
			if (this.Hp != 0U)
			{
				output.WriteRawTag(48);
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
			if (this.PlayerUserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.PlayerUserId);
			}
			if (this.userInfoDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserInfoDto);
			}
			num += this.equipments_.CalculateSize(UserGetPlayerInfoResponse._repeated_equipments_codec);
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
				if (num <= 26U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 16U)
					{
						this.PlayerUserId = input.ReadInt64();
						continue;
					}
					if (num == 26U)
					{
						if (this.userInfoDto_ == null)
						{
							this.userInfoDto_ = new UserInfoDto();
						}
						input.ReadMessage(this.userInfoDto_);
						continue;
					}
				}
				else
				{
					if (num == 34U)
					{
						this.equipments_.AddEntriesFrom(input, UserGetPlayerInfoResponse._repeated_equipments_codec);
						continue;
					}
					if (num == 40U)
					{
						this.Atk = input.ReadUInt32();
						continue;
					}
					if (num == 48U)
					{
						this.Hp = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UserGetPlayerInfoResponse> _parser = new MessageParser<UserGetPlayerInfoResponse>(() => new UserGetPlayerInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int PlayerUserIdFieldNumber = 2;

		private long playerUserId_;

		public const int UserInfoDtoFieldNumber = 3;

		private UserInfoDto userInfoDto_;

		public const int EquipmentsFieldNumber = 4;

		private static readonly FieldCodec<EquipmentDto> _repeated_equipments_codec = FieldCodec.ForMessage<EquipmentDto>(34U, EquipmentDto.Parser);

		private readonly RepeatedField<EquipmentDto> equipments_ = new RepeatedField<EquipmentDto>();

		public const int AtkFieldNumber = 5;

		private uint atk_;

		public const int HpFieldNumber = 6;

		private uint hp_;
	}
}
