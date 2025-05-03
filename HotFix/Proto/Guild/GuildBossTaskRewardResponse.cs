﻿using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildBossTaskRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildBossTaskRewardResponse> Parser
		{
			get
			{
				return GuildBossTaskRewardResponse._parser;
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
		public GuildBossTaskDto GuildBossTaskDto
		{
			get
			{
				return this.guildBossTaskDto_;
			}
			set
			{
				this.guildBossTaskDto_ = value;
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
			if (this.guildBossTaskDto_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.GuildBossTaskDto);
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
			if (this.guildBossTaskDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GuildBossTaskDto);
			}
			return num;
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
							if (this.guildBossTaskDto_ == null)
							{
								this.guildBossTaskDto_ = new GuildBossTaskDto();
							}
							input.ReadMessage(this.guildBossTaskDto_);
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

		private static readonly MessageParser<GuildBossTaskRewardResponse> _parser = new MessageParser<GuildBossTaskRewardResponse>(() => new GuildBossTaskRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int GuildBossTaskDtoFieldNumber = 3;

		private GuildBossTaskDto guildBossTaskDto_;
	}
}
