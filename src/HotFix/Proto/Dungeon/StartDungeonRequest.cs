using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Dungeon
{
	public sealed class StartDungeonRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<StartDungeonRequest> Parser
		{
			get
			{
				return StartDungeonRequest._parser;
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
		public int DungeonId
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
		public int LevelId
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
		public int OptionType
		{
			get
			{
				return this.optionType_;
			}
			set
			{
				this.optionType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string ClientVersion
		{
			get
			{
				return this.clientVersion_;
			}
			set
			{
				this.clientVersion_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.DungeonId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.DungeonId);
			}
			if (this.LevelId != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.LevelId);
			}
			if (this.OptionType != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.OptionType);
			}
			if (this.ClientVersion.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(this.ClientVersion);
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
			if (this.DungeonId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.DungeonId);
			}
			if (this.LevelId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.LevelId);
			}
			if (this.OptionType != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.OptionType);
			}
			if (this.ClientVersion.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ClientVersion);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 10U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 16U)
					{
						this.DungeonId = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.LevelId = input.ReadInt32();
						continue;
					}
					if (num == 32U)
					{
						this.OptionType = input.ReadInt32();
						continue;
					}
					if (num == 42U)
					{
						this.ClientVersion = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<StartDungeonRequest> _parser = new MessageParser<StartDungeonRequest>(() => new StartDungeonRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int DungeonIdFieldNumber = 2;

		private int dungeonId_;

		public const int LevelIdFieldNumber = 3;

		private int levelId_;

		public const int OptionTypeFieldNumber = 4;

		private int optionType_;

		public const int ClientVersionFieldNumber = 5;

		private string clientVersion_ = "";
	}
}
