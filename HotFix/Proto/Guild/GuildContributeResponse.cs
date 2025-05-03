using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildContributeResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildContributeResponse> Parser
		{
			get
			{
				return GuildContributeResponse._parser;
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
		public int DayContributeTimes
		{
			get
			{
				return this.dayContributeTimes_;
			}
			set
			{
				this.dayContributeTimes_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int DayALLContributeTimes
		{
			get
			{
				return this.dayALLContributeTimes_;
			}
			set
			{
				this.dayALLContributeTimes_ = value;
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
			if (this.DayContributeTimes != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.DayContributeTimes);
			}
			if (this.DayALLContributeTimes != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.DayALLContributeTimes);
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
			if (this.DayContributeTimes != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.DayContributeTimes);
			}
			if (this.DayALLContributeTimes != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.DayALLContributeTimes);
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
					if (num == 24U)
					{
						this.DayContributeTimes = input.ReadInt32();
						continue;
					}
					if (num == 32U)
					{
						this.DayALLContributeTimes = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildContributeResponse> _parser = new MessageParser<GuildContributeResponse>(() => new GuildContributeResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int DayContributeTimesFieldNumber = 3;

		private int dayContributeTimes_;

		public const int DayALLContributeTimesFieldNumber = 4;

		private int dayALLContributeTimes_;
	}
}
