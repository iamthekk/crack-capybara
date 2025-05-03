using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Battle
{
	public sealed class RPVPCombatReq : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RPVPCombatReq> Parser
		{
			get
			{
				return RPVPCombatReq._parser;
			}
		}

		[DebuggerNonUserCode]
		public BattleUserDto OwnerUser
		{
			get
			{
				return this.ownerUser_;
			}
			set
			{
				this.ownerUser_ = value;
			}
		}

		[DebuggerNonUserCode]
		public BattleUserDto OtherUser
		{
			get
			{
				return this.otherUser_;
			}
			set
			{
				this.otherUser_ = value;
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
			if (this.ownerUser_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.OwnerUser);
			}
			if (this.otherUser_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.OtherUser);
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
			if (this.ownerUser_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.OwnerUser);
			}
			if (this.otherUser_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.OtherUser);
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
					if (num == 10U)
					{
						if (this.ownerUser_ == null)
						{
							this.ownerUser_ = new BattleUserDto();
						}
						input.ReadMessage(this.ownerUser_);
						continue;
					}
					if (num == 18U)
					{
						if (this.otherUser_ == null)
						{
							this.otherUser_ = new BattleUserDto();
						}
						input.ReadMessage(this.otherUser_);
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

		private static readonly MessageParser<RPVPCombatReq> _parser = new MessageParser<RPVPCombatReq>(() => new RPVPCombatReq());

		public const int OwnerUserFieldNumber = 1;

		private BattleUserDto ownerUser_;

		public const int OtherUserFieldNumber = 2;

		private BattleUserDto otherUser_;

		public const int SeedFieldNumber = 3;

		private int seed_;

		public const int ClientVersionFieldNumber = 4;

		private string clientVersion_ = "";
	}
}
