using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;
using Proto.Mission;

namespace Proto.Guild
{
	public sealed class GuildBossEndRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildBossEndRequest> Parser
		{
			get
			{
				return GuildBossEndRequest._parser;
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
		public uint Result
		{
			get
			{
				return this.result_;
			}
			set
			{
				this.result_ = value;
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
			if (this.ConfigId != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.ConfigId);
			}
			if (this.Result != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.Result);
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
			if (this.ConfigId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ConfigId);
			}
			if (this.Result != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Result);
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
					if (num == 10U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 18U)
					{
						if (this.missionCompleteDto_ == null)
						{
							this.missionCompleteDto_ = new MissionCompleteDto();
						}
						input.ReadMessage(this.missionCompleteDto_);
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.ConfigId = input.ReadUInt32();
						continue;
					}
					if (num == 32U)
					{
						this.Result = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<GuildBossEndRequest> _parser = new MessageParser<GuildBossEndRequest>(() => new GuildBossEndRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int MissionCompleteDtoFieldNumber = 2;

		private MissionCompleteDto missionCompleteDto_;

		public const int ConfigIdFieldNumber = 3;

		private uint configId_;

		public const int ResultFieldNumber = 4;

		private uint result_;
	}
}
