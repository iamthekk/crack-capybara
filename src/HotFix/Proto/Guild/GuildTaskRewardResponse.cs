using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildTaskRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildTaskRewardResponse> Parser
		{
			get
			{
				return GuildTaskRewardResponse._parser;
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
		public GuildTaskDto UpdateTaskDto
		{
			get
			{
				return this.updateTaskDto_;
			}
			set
			{
				this.updateTaskDto_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint DeleteTaskDtoId
		{
			get
			{
				return this.deleteTaskDtoId_;
			}
			set
			{
				this.deleteTaskDtoId_ = value;
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
			if (this.updateTaskDto_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.UpdateTaskDto);
			}
			if (this.DeleteTaskDtoId != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.DeleteTaskDtoId);
			}
			this.tasks_.WriteTo(output, GuildTaskRewardResponse._repeated_tasks_codec);
			if (this.UserDailyActive != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.UserDailyActive);
			}
			if (this.UserWeeklyActive != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.UserWeeklyActive);
			}
			if (this.guildUpdateInfo_ != null)
			{
				output.WriteRawTag(66);
				output.WriteMessage(this.GuildUpdateInfo);
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
			if (this.updateTaskDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UpdateTaskDto);
			}
			if (this.DeleteTaskDtoId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.DeleteTaskDtoId);
			}
			num += this.tasks_.CalculateSize(GuildTaskRewardResponse._repeated_tasks_codec);
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
							if (this.updateTaskDto_ == null)
							{
								this.updateTaskDto_ = new GuildTaskDto();
							}
							input.ReadMessage(this.updateTaskDto_);
							continue;
						}
						if (num == 32U)
						{
							this.DeleteTaskDtoId = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 42U)
					{
						this.tasks_.AddEntriesFrom(input, GuildTaskRewardResponse._repeated_tasks_codec);
						continue;
					}
					if (num == 48U)
					{
						this.UserDailyActive = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 56U)
					{
						this.UserWeeklyActive = input.ReadUInt32();
						continue;
					}
					if (num == 66U)
					{
						if (this.guildUpdateInfo_ == null)
						{
							this.guildUpdateInfo_ = new GuildUpdateInfoDto();
						}
						input.ReadMessage(this.guildUpdateInfo_);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildTaskRewardResponse> _parser = new MessageParser<GuildTaskRewardResponse>(() => new GuildTaskRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int UpdateTaskDtoFieldNumber = 3;

		private GuildTaskDto updateTaskDto_;

		public const int DeleteTaskDtoIdFieldNumber = 4;

		private uint deleteTaskDtoId_;

		public const int TasksFieldNumber = 5;

		private static readonly FieldCodec<GuildTaskDto> _repeated_tasks_codec = FieldCodec.ForMessage<GuildTaskDto>(42U, GuildTaskDto.Parser);

		private readonly RepeatedField<GuildTaskDto> tasks_ = new RepeatedField<GuildTaskDto>();

		public const int UserDailyActiveFieldNumber = 6;

		private uint userDailyActive_;

		public const int UserWeeklyActiveFieldNumber = 7;

		private uint userWeeklyActive_;

		public const int GuildUpdateInfoFieldNumber = 8;

		private GuildUpdateInfoDto guildUpdateInfo_;
	}
}
