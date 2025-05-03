using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.GuildRace
{
	public sealed class GuildRaceOwnerUserApplyListRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildRaceOwnerUserApplyListRequest> Parser
		{
			get
			{
				return GuildRaceOwnerUserApplyListRequest._parser;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.CommonParams);
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
					input.SkipLastField();
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

		private static readonly MessageParser<GuildRaceOwnerUserApplyListRequest> _parser = new MessageParser<GuildRaceOwnerUserApplyListRequest>(() => new GuildRaceOwnerUserApplyListRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;
	}
}
