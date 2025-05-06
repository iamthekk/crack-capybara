using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Battle
{
	public sealed class RTowerCombatReq : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RTowerCombatReq> Parser
		{
			get
			{
				return RTowerCombatReq._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Tower
		{
			get
			{
				return this.tower_;
			}
			set
			{
				this.tower_ = value;
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
			if (this.Tower != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Tower);
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
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Tower != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Tower);
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
				if (num <= 18U)
				{
					if (num == 8U)
					{
						this.Tower = input.ReadInt32();
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
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<RTowerCombatReq> _parser = new MessageParser<RTowerCombatReq>(() => new RTowerCombatReq());

		public const int TowerFieldNumber = 1;

		private int tower_;

		public const int UserInfoFieldNumber = 2;

		private BattleUserDto userInfo_;

		public const int SeedFieldNumber = 3;

		private int seed_;

		public const int ClientVersionFieldNumber = 4;

		private string clientVersion_ = "";
	}
}
