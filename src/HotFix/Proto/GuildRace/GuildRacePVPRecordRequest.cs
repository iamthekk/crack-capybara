using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.GuildRace
{
	public sealed class GuildRacePVPRecordRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildRacePVPRecordRequest> Parser
		{
			get
			{
				return GuildRacePVPRecordRequest._parser;
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
		public ulong RecordId
		{
			get
			{
				return this.recordId_;
			}
			set
			{
				this.recordId_ = value;
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
			if (this.RecordId != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.RecordId);
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
			if (this.RecordId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.RecordId);
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
						this.RecordId = input.ReadUInt64();
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

		private static readonly MessageParser<GuildRacePVPRecordRequest> _parser = new MessageParser<GuildRacePVPRecordRequest>(() => new GuildRacePVPRecordRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int RecordIdFieldNumber = 2;

		private ulong recordId_;
	}
}
