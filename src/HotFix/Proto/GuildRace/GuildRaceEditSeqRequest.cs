using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.GuildRace
{
	public sealed class GuildRaceEditSeqRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildRaceEditSeqRequest> Parser
		{
			get
			{
				return GuildRaceEditSeqRequest._parser;
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
		public uint OpSeq
		{
			get
			{
				return this.opSeq_;
			}
			set
			{
				this.opSeq_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long TargetUserId
		{
			get
			{
				return this.targetUserId_;
			}
			set
			{
				this.targetUserId_ = value;
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
			if (this.OpSeq != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.OpSeq);
			}
			if (this.TargetUserId != 0L)
			{
				output.WriteRawTag(32);
				output.WriteInt64(this.TargetUserId);
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
			if (this.OpSeq != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.OpSeq);
			}
			if (this.TargetUserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.TargetUserId);
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
						if (num != 32U)
						{
							input.SkipLastField();
						}
						else
						{
							this.TargetUserId = input.ReadInt64();
						}
					}
					else
					{
						this.OpSeq = input.ReadUInt32();
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

		private static readonly MessageParser<GuildRaceEditSeqRequest> _parser = new MessageParser<GuildRaceEditSeqRequest>(() => new GuildRaceEditSeqRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int OpSeqFieldNumber = 3;

		private uint opSeq_;

		public const int TargetUserIdFieldNumber = 4;

		private long targetUserId_;
	}
}
