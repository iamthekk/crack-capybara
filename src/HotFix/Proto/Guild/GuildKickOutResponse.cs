using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Guild
{
	public sealed class GuildKickOutResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildKickOutResponse> Parser
		{
			get
			{
				return GuildKickOutResponse._parser;
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
		public uint Members
		{
			get
			{
				return this.members_;
			}
			set
			{
				this.members_ = value;
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
			if (this.Members != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Members);
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
			if (this.Members != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Members);
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
					if (num != 16U)
					{
						input.SkipLastField();
					}
					else
					{
						this.Members = input.ReadUInt32();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<GuildKickOutResponse> _parser = new MessageParser<GuildKickOutResponse>(() => new GuildKickOutResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int MembersFieldNumber = 2;

		private uint members_;
	}
}
