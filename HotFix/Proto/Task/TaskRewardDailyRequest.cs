﻿using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Task
{
	public sealed class TaskRewardDailyRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TaskRewardDailyRequest> Parser
		{
			get
			{
				return TaskRewardDailyRequest._parser;
			}
		}

		[DebuggerNonUserCode]
		public CommonParams CommonParams
		{
			get
			{
				return this.commonParams_;
			}
			set
			{
				this.commonParams_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Id
		{
			get
			{
				return this.id_;
			}
			set
			{
				this.id_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.CommonParams);
			}
			if (this.Id != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Id);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.Id != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Id);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U)
					{
						input.SkipLastField();
					}
					else
					{
						this.Id = input.ReadUInt32();
					}
				}
				else
				{
					if (this.commonParams_ == null)
					{
						this.commonParams_ = new CommonParams();
					}
					input.ReadMessage(this.commonParams_);
				}
			}
		}

		private static readonly MessageParser<TaskRewardDailyRequest> _parser = new MessageParser<TaskRewardDailyRequest>(() => new TaskRewardDailyRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int IdFieldNumber = 2;

		private uint id_;
	}
}
