using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.GuildRace
{
	public sealed class GuildRaceGuildApplyRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildRaceGuildApplyRequest> Parser
		{
			get
			{
				return GuildRaceGuildApplyRequest._parser;
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
		public ulong GuildId
		{
			get
			{
				return this.guildId_;
			}
			set
			{
				this.guildId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Type
		{
			get
			{
				return this.type_;
			}
			set
			{
				this.type_ = value;
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
			if (this.GuildId != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.GuildId);
			}
			if (this.Type != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Type);
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
			if (this.GuildId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.GuildId);
			}
			if (this.Type != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Type);
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
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.Type = input.ReadUInt32();
						}
					}
					else
					{
						this.GuildId = input.ReadUInt64();
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

		private static readonly MessageParser<GuildRaceGuildApplyRequest> _parser = new MessageParser<GuildRaceGuildApplyRequest>(() => new GuildRaceGuildApplyRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int GuildIdFieldNumber = 2;

		private ulong guildId_;

		public const int TypeFieldNumber = 3;

		private uint type_;
	}
}
