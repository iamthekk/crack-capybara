using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.User
{
	public sealed class UserGetOtherPlayerInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserGetOtherPlayerInfoResponse> Parser
		{
			get
			{
				return UserGetOtherPlayerInfoResponse._parser;
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
		public RepeatedField<PlayerInfoDto> PlayerInfos
		{
			get
			{
				return this.playerInfos_;
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
			this.playerInfos_.WriteTo(output, UserGetOtherPlayerInfoResponse._repeated_playerInfos_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			return num + this.playerInfos_.CalculateSize(UserGetOtherPlayerInfoResponse._repeated_playerInfos_codec);
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
						input.SkipLastField();
					}
					else
					{
						this.playerInfos_.AddEntriesFrom(input, UserGetOtherPlayerInfoResponse._repeated_playerInfos_codec);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<UserGetOtherPlayerInfoResponse> _parser = new MessageParser<UserGetOtherPlayerInfoResponse>(() => new UserGetOtherPlayerInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int PlayerInfosFieldNumber = 2;

		private static readonly FieldCodec<PlayerInfoDto> _repeated_playerInfos_codec = FieldCodec.ForMessage<PlayerInfoDto>(18U, PlayerInfoDto.Parser);

		private readonly RepeatedField<PlayerInfoDto> playerInfos_ = new RepeatedField<PlayerInfoDto>();
	}
}
