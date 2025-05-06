using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Mission
{
	public sealed class MissionCompleteRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MissionCompleteRequest> Parser
		{
			get
			{
				return MissionCompleteRequest._parser;
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
		public MissionCompleteDto MissionCompleteDto
		{
			get
			{
				return this.missionCompleteDto_;
			}
			set
			{
				this.missionCompleteDto_ = value;
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
			if (this.missionCompleteDto_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.MissionCompleteDto);
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
			if (this.missionCompleteDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.MissionCompleteDto);
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
					if (num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						if (this.missionCompleteDto_ == null)
						{
							this.missionCompleteDto_ = new MissionCompleteDto();
						}
						input.ReadMessage(this.missionCompleteDto_);
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

		private static readonly MessageParser<MissionCompleteRequest> _parser = new MessageParser<MissionCompleteRequest>(() => new MissionCompleteRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int MissionCompleteDtoFieldNumber = 2;

		private MissionCompleteDto missionCompleteDto_;
	}
}
