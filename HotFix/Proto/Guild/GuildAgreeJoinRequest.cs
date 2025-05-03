using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildAgreeJoinRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildAgreeJoinRequest> Parser
		{
			get
			{
				return GuildAgreeJoinRequest._parser;
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
		public RepeatedField<long> UserIds
		{
			get
			{
				return this.userIds_;
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
			this.userIds_.WriteTo(output, GuildAgreeJoinRequest._repeated_userIds_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			return num + this.userIds_.CalculateSize(GuildAgreeJoinRequest._repeated_userIds_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U && num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						this.userIds_.AddEntriesFrom(input, GuildAgreeJoinRequest._repeated_userIds_codec);
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

		private static readonly MessageParser<GuildAgreeJoinRequest> _parser = new MessageParser<GuildAgreeJoinRequest>(() => new GuildAgreeJoinRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int UserIdsFieldNumber = 2;

		private static readonly FieldCodec<long> _repeated_userIds_codec = FieldCodec.ForInt64(18U);

		private readonly RepeatedField<long> userIds_ = new RepeatedField<long>();
	}
}
