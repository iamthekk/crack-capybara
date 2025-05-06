using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.ServerList
{
	public sealed class UserGetLastLoginResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserGetLastLoginResponse> Parser
		{
			get
			{
				return UserGetLastLoginResponse._parser;
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
		public RepeatedField<RoleDetailDto> RoleList
		{
			get
			{
				return this.roleList_;
			}
		}

		[DebuggerNonUserCode]
		public MapField<uint, ZoneInfoDto> ServerList
		{
			get
			{
				return this.serverList_;
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
			this.roleList_.WriteTo(output, UserGetLastLoginResponse._repeated_roleList_codec);
			this.serverList_.WriteTo(output, UserGetLastLoginResponse._map_serverList_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			num += this.roleList_.CalculateSize(UserGetLastLoginResponse._repeated_roleList_codec);
			return num + this.serverList_.CalculateSize(UserGetLastLoginResponse._map_serverList_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 18U)
					{
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							this.serverList_.AddEntriesFrom(input, UserGetLastLoginResponse._map_serverList_codec);
						}
					}
					else
					{
						this.roleList_.AddEntriesFrom(input, UserGetLastLoginResponse._repeated_roleList_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<UserGetLastLoginResponse> _parser = new MessageParser<UserGetLastLoginResponse>(() => new UserGetLastLoginResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int RoleListFieldNumber = 2;

		private static readonly FieldCodec<RoleDetailDto> _repeated_roleList_codec = FieldCodec.ForMessage<RoleDetailDto>(18U, RoleDetailDto.Parser);

		private readonly RepeatedField<RoleDetailDto> roleList_ = new RepeatedField<RoleDetailDto>();

		public const int ServerListFieldNumber = 3;

		private static readonly MapField<uint, ZoneInfoDto>.Codec _map_serverList_codec = new MapField<uint, ZoneInfoDto>.Codec(FieldCodec.ForUInt32(8U), FieldCodec.ForMessage<ZoneInfoDto>(18U, ZoneInfoDto.Parser), 26U);

		private readonly MapField<uint, ZoneInfoDto> serverList_ = new MapField<uint, ZoneInfoDto>();
	}
}
