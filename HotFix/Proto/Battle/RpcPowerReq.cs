using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Battle
{
	public sealed class RpcPowerReq : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<RpcPowerReq> Parser
		{
			get
			{
				return RpcPowerReq._parser;
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
			if (this.userInfo_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.UserInfo);
			}
			if (this.ClientVersion.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.ClientVersion);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.userInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserInfo);
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
				if (num != 10U)
				{
					if (num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						this.ClientVersion = input.ReadString();
					}
				}
				else
				{
					if (this.userInfo_ == null)
					{
						this.userInfo_ = new BattleUserDto();
					}
					input.ReadMessage(this.userInfo_);
				}
			}
		}

		private static readonly MessageParser<RpcPowerReq> _parser = new MessageParser<RpcPowerReq>(() => new RpcPowerReq());

		public const int UserInfoFieldNumber = 1;

		private BattleUserDto userInfo_;

		public const int ClientVersionFieldNumber = 2;

		private string clientVersion_ = "";
	}
}
