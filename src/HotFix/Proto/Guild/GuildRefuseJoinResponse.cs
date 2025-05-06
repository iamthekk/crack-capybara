using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Guild
{
	public sealed class GuildRefuseJoinResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildRefuseJoinResponse> Parser
		{
			get
			{
				return GuildRefuseJoinResponse._parser;
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
		public uint ApplyCount
		{
			get
			{
				return this.applyCount_;
			}
			set
			{
				this.applyCount_ = value;
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
			if (this.ApplyCount != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.ApplyCount);
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
			if (this.ApplyCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ApplyCount);
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
						this.ApplyCount = input.ReadUInt32();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<GuildRefuseJoinResponse> _parser = new MessageParser<GuildRefuseJoinResponse>(() => new GuildRefuseJoinResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int ApplyCountFieldNumber = 2;

		private uint applyCount_;
	}
}
