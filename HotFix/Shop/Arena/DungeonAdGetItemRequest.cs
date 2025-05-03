using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Shop.Arena
{
	public sealed class DungeonAdGetItemRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<DungeonAdGetItemRequest> Parser
		{
			get
			{
				return DungeonAdGetItemRequest._parser;
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
		public uint DungeonId
		{
			get
			{
				return this.dungeonId_;
			}
			set
			{
				this.dungeonId_ = value;
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
			if (this.DungeonId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.DungeonId);
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
			if (this.DungeonId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.DungeonId);
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
						this.DungeonId = input.ReadUInt32();
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

		private static readonly MessageParser<DungeonAdGetItemRequest> _parser = new MessageParser<DungeonAdGetItemRequest>(() => new DungeonAdGetItemRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int DungeonIdFieldNumber = 2;

		private uint dungeonId_;
	}
}
