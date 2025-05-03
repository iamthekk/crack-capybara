using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Mission
{
	public sealed class MissionGetInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MissionGetInfoResponse> Parser
		{
			get
			{
				return MissionGetInfoResponse._parser;
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
		public UserMission UserMission
		{
			get
			{
				return this.userMission_;
			}
			set
			{
				this.userMission_ = value;
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
			if (this.userMission_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.UserMission);
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
			if (this.userMission_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserMission);
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
						input.SkipLastField();
					}
					else
					{
						if (this.userMission_ == null)
						{
							this.userMission_ = new UserMission();
						}
						input.ReadMessage(this.userMission_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<MissionGetInfoResponse> _parser = new MessageParser<MissionGetInfoResponse>(() => new MissionGetInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int UserMissionFieldNumber = 2;

		private UserMission userMission_;
	}
}
