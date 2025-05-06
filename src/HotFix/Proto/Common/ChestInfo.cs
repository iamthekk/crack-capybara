using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class ChestInfo : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ChestInfo> Parser
		{
			get
			{
				return ChestInfo._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint LastRewardType
		{
			get
			{
				return this.lastRewardType_;
			}
			set
			{
				this.lastRewardType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint LastRewardConfigId
		{
			get
			{
				return this.lastRewardConfigId_;
			}
			set
			{
				this.lastRewardConfigId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Score
		{
			get
			{
				return this.score_;
			}
			set
			{
				this.score_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.LastRewardType != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.LastRewardType);
			}
			if (this.LastRewardConfigId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.LastRewardConfigId);
			}
			if (this.Score != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Score);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.LastRewardType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.LastRewardType);
			}
			if (this.LastRewardConfigId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.LastRewardConfigId);
			}
			if (this.Score != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Score);
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
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.Score = input.ReadUInt32();
						}
					}
					else
					{
						this.LastRewardConfigId = input.ReadUInt32();
					}
				}
				else
				{
					this.LastRewardType = input.ReadUInt32();
				}
			}
		}

		private static readonly MessageParser<ChestInfo> _parser = new MessageParser<ChestInfo>(() => new ChestInfo());

		public const int LastRewardTypeFieldNumber = 1;

		private uint lastRewardType_;

		public const int LastRewardConfigIdFieldNumber = 2;

		private uint lastRewardConfigId_;

		public const int ScoreFieldNumber = 3;

		private uint score_;
	}
}
