using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildSignInResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildSignInResponse> Parser
		{
			get
			{
				return GuildSignInResponse._parser;
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
		public GuilSignInDto SignInDto
		{
			get
			{
				return this.signInDto_;
			}
			set
			{
				this.signInDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint UserDailyActive
		{
			get
			{
				return this.userDailyActive_;
			}
			set
			{
				this.userDailyActive_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint UserWeeklyActive
		{
			get
			{
				return this.userWeeklyActive_;
			}
			set
			{
				this.userWeeklyActive_ = value;
			}
		}

		[DebuggerNonUserCode]
		public GuildUpdateInfoDto GuildUpdateInfo
		{
			get
			{
				return this.guildUpdateInfo_;
			}
			set
			{
				this.guildUpdateInfo_ = value;
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
		public string SignInRecord
		{
			get
			{
				return this.signInRecord_;
			}
			set
			{
				this.signInRecord_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.signInDto_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.SignInDto);
			}
			if (this.UserDailyActive != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.UserDailyActive);
			}
			if (this.UserWeeklyActive != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.UserWeeklyActive);
			}
			if (this.guildUpdateInfo_ != null)
			{
				output.WriteRawTag(50);
				output.WriteMessage(this.GuildUpdateInfo);
			}
			this.tasks_.WriteTo(output, GuildSignInResponse._repeated_tasks_codec);
			if (this.SignInRecord.Length != 0)
			{
				output.WriteRawTag(66);
				output.WriteString(this.SignInRecord);
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
			if (this.signInDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.SignInDto);
			}
			if (this.UserDailyActive != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.UserDailyActive);
			}
			if (this.UserWeeklyActive != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.UserWeeklyActive);
			}
			if (this.guildUpdateInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GuildUpdateInfo);
			}
			num += this.tasks_.CalculateSize(GuildSignInResponse._repeated_tasks_codec);
			if (this.SignInRecord.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.SignInRecord);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 32U)
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
							if (this.signInDto_ == null)
							{
								this.signInDto_ = new GuilSignInDto();
							}
							input.ReadMessage(this.signInDto_);
							continue;
						}
						if (num == 32U)
						{
							this.UserDailyActive = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 50U)
				{
					if (num == 40U)
					{
						this.UserWeeklyActive = input.ReadUInt32();
						continue;
					}
					if (num == 50U)
					{
						if (this.guildUpdateInfo_ == null)
						{
							this.guildUpdateInfo_ = new GuildUpdateInfoDto();
						}
						input.ReadMessage(this.guildUpdateInfo_);
						continue;
					}
				}
				else
				{
					if (num == 58U)
					{
						this.tasks_.AddEntriesFrom(input, GuildSignInResponse._repeated_tasks_codec);
						continue;
					}
					if (num == 66U)
					{
						this.SignInRecord = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildSignInResponse> _parser = new MessageParser<GuildSignInResponse>(() => new GuildSignInResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int SignInDtoFieldNumber = 3;

		private GuilSignInDto signInDto_;

		public const int UserDailyActiveFieldNumber = 4;

		private uint userDailyActive_;

		public const int UserWeeklyActiveFieldNumber = 5;

		private uint userWeeklyActive_;

		public const int GuildUpdateInfoFieldNumber = 6;

		private GuildUpdateInfoDto guildUpdateInfo_;

		public const int TasksFieldNumber = 7;

		private static readonly FieldCodec<GuildTaskDto> _repeated_tasks_codec = FieldCodec.ForMessage<GuildTaskDto>(58U, GuildTaskDto.Parser);

		private readonly RepeatedField<GuildTaskDto> tasks_ = new RepeatedField<GuildTaskDto>();

		public const int SignInRecordFieldNumber = 8;

		private string signInRecord_ = "";
	}
}
