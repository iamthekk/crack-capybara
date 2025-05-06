using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Talents
{
	public sealed class TalentLegacyLeaderBoardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TalentLegacyLeaderBoardResponse> Parser
		{
			get
			{
				return TalentLegacyLeaderBoardResponse._parser;
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
		public RepeatedField<UserRankInfoSimpleDto> UserList
		{
			get
			{
				return this.userList_;
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
			this.userList_.WriteTo(output, TalentLegacyLeaderBoardResponse._repeated_userList_codec);
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
			return num + this.userList_.CalculateSize(TalentLegacyLeaderBoardResponse._repeated_userList_codec);
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
							this.userList_.AddEntriesFrom(input, TalentLegacyLeaderBoardResponse._repeated_userList_codec);
						}
					}
					else
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<TalentLegacyLeaderBoardResponse> _parser = new MessageParser<TalentLegacyLeaderBoardResponse>(() => new TalentLegacyLeaderBoardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int UserListFieldNumber = 3;

		private static readonly FieldCodec<UserRankInfoSimpleDto> _repeated_userList_codec = FieldCodec.ForMessage<UserRankInfoSimpleDto>(26U, UserRankInfoSimpleDto.Parser);

		private readonly RepeatedField<UserRankInfoSimpleDto> userList_ = new RepeatedField<UserRankInfoSimpleDto>();
	}
}
