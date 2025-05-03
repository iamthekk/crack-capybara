using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Battle
{
	public sealed class RDungeonCombatReq : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RDungeonCombatReq> Parser
		{
			get
			{
				return RDungeonCombatReq._parser;
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
		public BattleUserDto UserInfo
		{
			get
			{
				return this.userInfo_;
			}
			set
			{
				this.userInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Seed
		{
			get
			{
				return this.seed_;
			}
			set
			{
				this.seed_ = value;
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
			if (this.DungeonId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.DungeonId);
			}
			if (this.LevelId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.LevelId);
			}
			if (this.userInfo_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.UserInfo);
			}
			if (this.Seed != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Seed);
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
			if (this.DungeonId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.DungeonId);
			}
			if (this.LevelId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.LevelId);
			}
			if (this.userInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserInfo);
			}
			if (this.Seed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seed);
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
					if (num == 8U)
					{
						this.DungeonId = input.ReadInt32();
						continue;
					}
					if (num == 16U)
					{
						this.LevelId = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						if (this.userInfo_ == null)
						{
							this.userInfo_ = new BattleUserDto();
						}
						input.ReadMessage(this.userInfo_);
						continue;
					}
					if (num == 32U)
					{
						this.Seed = input.ReadInt32();
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

		private static readonly MessageParser<RDungeonCombatReq> _parser = new MessageParser<RDungeonCombatReq>(() => new RDungeonCombatReq());

		public const int DungeonIdFieldNumber = 1;

		private int dungeonId_;

		public const int LevelIdFieldNumber = 2;

		private int levelId_;

		public const int UserInfoFieldNumber = 3;

		private BattleUserDto userInfo_;

		public const int SeedFieldNumber = 4;

		private int seed_;

		public const int ClientVersionFieldNumber = 5;

		private string clientVersion_ = "";
	}
}
