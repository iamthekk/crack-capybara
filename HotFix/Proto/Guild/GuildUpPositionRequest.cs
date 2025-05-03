using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Guild
{
	public sealed class GuildUpPositionRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<GuildUpPositionRequest> Parser
		{
			get
			{
				return GuildUpPositionRequest._parser;
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
		public long UserId
		{
			get
			{
				return this.userId_;
			}
			set
			{
				this.userId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Position
		{
			get
			{
				return this.position_;
			}
			set
			{
				this.position_ = value;
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
			if (this.UserId != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.UserId);
			}
			if (this.Position != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Position);
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
			if (this.UserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId);
			}
			if (this.Position != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Position);
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
							this.Position = input.ReadUInt32();
						}
					}
					else
					{
						this.UserId = input.ReadInt64();
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

		private static readonly MessageParser<GuildUpPositionRequest> _parser = new MessageParser<GuildUpPositionRequest>(() => new GuildUpPositionRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int UserIdFieldNumber = 2;

		private long userId_;

		public const int PositionFieldNumber = 3;

		private uint position_;
	}
}
