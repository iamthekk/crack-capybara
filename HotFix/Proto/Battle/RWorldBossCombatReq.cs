using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Battle
{
	public sealed class RWorldBossCombatReq : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RWorldBossCombatReq> Parser
		{
			get
			{
				return RWorldBossCombatReq._parser;
			}
		}

		[DebuggerNonUserCode]
		public int MonsterCfgId
		{
			get
			{
				return this.monsterCfgId_;
			}
			set
			{
				this.monsterCfgId_ = value;
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
		public int ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.MonsterCfgId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.MonsterCfgId);
			}
			if (this.userInfo_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.UserInfo);
			}
			if (this.Seed != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Seed);
			}
			if (this.ClientVersion.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.ClientVersion);
			}
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.ConfigId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.MonsterCfgId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.MonsterCfgId);
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
			if (this.ConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ConfigId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
				{
					if (num == 8U)
					{
						this.MonsterCfgId = input.ReadInt32();
						continue;
					}
					if (num == 18U)
					{
						if (this.userInfo_ == null)
						{
							this.userInfo_ = new BattleUserDto();
						}
						input.ReadMessage(this.userInfo_);
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Seed = input.ReadInt32();
						continue;
					}
					if (num == 34U)
					{
						this.ClientVersion = input.ReadString();
						continue;
					}
					if (num == 40U)
					{
						this.ConfigId = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<RWorldBossCombatReq> _parser = new MessageParser<RWorldBossCombatReq>(() => new RWorldBossCombatReq());

		public const int MonsterCfgIdFieldNumber = 1;

		private int monsterCfgId_;

		public const int UserInfoFieldNumber = 2;

		private BattleUserDto userInfo_;

		public const int SeedFieldNumber = 3;

		private int seed_;

		public const int ClientVersionFieldNumber = 4;

		private string clientVersion_ = "";

		public const int ConfigIdFieldNumber = 5;

		private int configId_;
	}
}
