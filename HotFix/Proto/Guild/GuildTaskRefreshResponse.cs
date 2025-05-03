using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildTaskRefreshResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildTaskRefreshResponse> Parser
		{
			get
			{
				return GuildTaskRefreshResponse._parser;
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
		public GuildTaskDto GuildTask
		{
			get
			{
				return this.guildTask_;
			}
			set
			{
				this.guildTask_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TaskRefreshCount
		{
			get
			{
				return this.taskRefreshCount_;
			}
			set
			{
				this.taskRefreshCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TaskRefreshCost
		{
			get
			{
				return this.taskRefreshCost_;
			}
			set
			{
				this.taskRefreshCost_ = value;
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
			if (this.guildTask_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.GuildTask);
			}
			if (this.TaskRefreshCount != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.TaskRefreshCount);
			}
			if (this.TaskRefreshCost != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.TaskRefreshCost);
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
			if (this.guildTask_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GuildTask);
			}
			if (this.TaskRefreshCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TaskRefreshCount);
			}
			if (this.TaskRefreshCost != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TaskRefreshCost);
			}
			return num;
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
						if (this.guildTask_ == null)
						{
							this.guildTask_ = new GuildTaskDto();
						}
						input.ReadMessage(this.guildTask_);
						continue;
					}
					if (num == 32U)
					{
						this.TaskRefreshCount = input.ReadUInt32();
						continue;
					}
					if (num == 40U)
					{
						this.TaskRefreshCost = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildTaskRefreshResponse> _parser = new MessageParser<GuildTaskRefreshResponse>(() => new GuildTaskRefreshResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int GuildTaskFieldNumber = 3;

		private GuildTaskDto guildTask_;

		public const int TaskRefreshCountFieldNumber = 4;

		private uint taskRefreshCount_;

		public const int TaskRefreshCostFieldNumber = 5;

		private uint taskRefreshCost_;
	}
}
