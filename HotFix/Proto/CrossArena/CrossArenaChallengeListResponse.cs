using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;
using Proto.User;

namespace Proto.CrossArena
{
	public sealed class CrossArenaChallengeListResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CrossArenaChallengeListResponse> Parser
		{
			get
			{
				return CrossArenaChallengeListResponse._parser;
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
		public RepeatedField<CrossArenaRankDto> OppList
		{
			get
			{
				return this.oppList_;
			}
		}

		[DebuggerNonUserCode]
		public uint RefreshCount
		{
			get
			{
				return this.refreshCount_;
			}
			set
			{
				this.refreshCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<PlayerInfoDto> OppInfos
		{
			get
			{
				return this.oppInfos_;
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
			this.oppList_.WriteTo(output, CrossArenaChallengeListResponse._repeated_oppList_codec);
			if (this.RefreshCount != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.RefreshCount);
			}
			this.oppInfos_.WriteTo(output, CrossArenaChallengeListResponse._repeated_oppInfos_codec);
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
			num += this.oppList_.CalculateSize(CrossArenaChallengeListResponse._repeated_oppList_codec);
			if (this.RefreshCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.RefreshCount);
			}
			return num + this.oppInfos_.CalculateSize(CrossArenaChallengeListResponse._repeated_oppInfos_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
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
				}
				else
				{
					if (num == 26U)
					{
						this.oppList_.AddEntriesFrom(input, CrossArenaChallengeListResponse._repeated_oppList_codec);
						continue;
					}
					if (num == 32U)
					{
						this.RefreshCount = input.ReadUInt32();
						continue;
					}
					if (num == 42U)
					{
						this.oppInfos_.AddEntriesFrom(input, CrossArenaChallengeListResponse._repeated_oppInfos_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<CrossArenaChallengeListResponse> _parser = new MessageParser<CrossArenaChallengeListResponse>(() => new CrossArenaChallengeListResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int OppListFieldNumber = 3;

		private static readonly FieldCodec<CrossArenaRankDto> _repeated_oppList_codec = FieldCodec.ForMessage<CrossArenaRankDto>(26U, CrossArenaRankDto.Parser);

		private readonly RepeatedField<CrossArenaRankDto> oppList_ = new RepeatedField<CrossArenaRankDto>();

		public const int RefreshCountFieldNumber = 4;

		private uint refreshCount_;

		public const int OppInfosFieldNumber = 5;

		private static readonly FieldCodec<PlayerInfoDto> _repeated_oppInfos_codec = FieldCodec.ForMessage<PlayerInfoDto>(42U, PlayerInfoDto.Parser);

		private readonly RepeatedField<PlayerInfoDto> oppInfos_ = new RepeatedField<PlayerInfoDto>();
	}
}
