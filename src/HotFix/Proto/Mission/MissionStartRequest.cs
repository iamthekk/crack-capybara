using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Mission
{
	public sealed class MissionStartRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MissionStartRequest> Parser
		{
			get
			{
				return MissionStartRequest._parser;
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
		public uint BattleType
		{
			get
			{
				return this.battleType_;
			}
			set
			{
				this.battleType_ = value;
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
			if (this.BattleType != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.BattleType);
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
			if (this.BattleType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BattleType);
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
					if (num != 24U)
					{
						input.SkipLastField();
					}
					else
					{
						this.BattleType = input.ReadUInt32();
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

		private static readonly MessageParser<MissionStartRequest> _parser = new MessageParser<MissionStartRequest>(() => new MissionStartRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int BattleTypeFieldNumber = 3;

		private uint battleType_;
	}
}
