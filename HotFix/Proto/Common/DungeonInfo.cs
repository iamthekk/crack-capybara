using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class DungeonInfo : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<DungeonInfo> Parser
		{
			get
			{
				return DungeonInfo._parser;
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
		public uint LevelId
		{
			get
			{
				return this.levelId_;
			}
			set
			{
				this.levelId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.DungeonId != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.DungeonId);
			}
			if (this.LevelId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.LevelId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.DungeonId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.DungeonId);
			}
			if (this.LevelId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.LevelId);
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
						this.LevelId = input.ReadUInt32();
					}
				}
				else
				{
					this.DungeonId = input.ReadUInt32();
				}
			}
		}

		private static readonly MessageParser<DungeonInfo> _parser = new MessageParser<DungeonInfo>(() => new DungeonInfo());

		public const int DungeonIdFieldNumber = 1;

		private uint dungeonId_;

		public const int LevelIdFieldNumber = 2;

		private uint levelId_;
	}
}
