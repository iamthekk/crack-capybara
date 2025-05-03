using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildBossStartResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildBossStartResponse> Parser
		{
			get
			{
				return GuildBossStartResponse._parser;
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
		public uint ChallengeCnt
		{
			get
			{
				return this.challengeCnt_;
			}
			set
			{
				this.challengeCnt_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<GuildTaskDto> Tasks
		{
			get
			{
				return this.tasks_;
			}
		}

		[DebuggerNonUserCode]
		public ulong NextChallengeCntRecoveryTime
		{
			get
			{
				return this.nextChallengeCntRecoveryTime_;
			}
			set
			{
				this.nextChallengeCntRecoveryTime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
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
			if (this.ChallengeCnt != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.ChallengeCnt);
			}
			this.tasks_.WriteTo(output, GuildBossStartResponse._repeated_tasks_codec);
			if (this.NextChallengeCntRecoveryTime != 0UL)
			{
				output.WriteRawTag(40);
				output.WriteUInt64(this.NextChallengeCntRecoveryTime);
			}
			if (this.ConfigId != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.ConfigId);
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
			if (this.ChallengeCnt != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ChallengeCnt);
			}
			num += this.tasks_.CalculateSize(GuildBossStartResponse._repeated_tasks_codec);
			if (this.NextChallengeCntRecoveryTime != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.NextChallengeCntRecoveryTime);
			}
			if (this.ConfigId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ConfigId);
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
						this.ChallengeCnt = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 34U)
					{
						this.tasks_.AddEntriesFrom(input, GuildBossStartResponse._repeated_tasks_codec);
						continue;
					}
					if (num == 40U)
					{
						this.NextChallengeCntRecoveryTime = input.ReadUInt64();
						continue;
					}
					if (num == 48U)
					{
						this.ConfigId = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildBossStartResponse> _parser = new MessageParser<GuildBossStartResponse>(() => new GuildBossStartResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ChallengeCntFieldNumber = 3;

		private uint challengeCnt_;

		public const int TasksFieldNumber = 4;

		private static readonly FieldCodec<GuildTaskDto> _repeated_tasks_codec = FieldCodec.ForMessage<GuildTaskDto>(34U, GuildTaskDto.Parser);

		private readonly RepeatedField<GuildTaskDto> tasks_ = new RepeatedField<GuildTaskDto>();

		public const int NextChallengeCntRecoveryTimeFieldNumber = 5;

		private ulong nextChallengeCntRecoveryTime_;

		public const int ConfigIdFieldNumber = 6;

		private uint configId_;
	}
}
