using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Pay
{
	public sealed class KuaiShouOrderDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<KuaiShouOrderDto> Parser
		{
			get
			{
				return KuaiShouOrderDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public string ChannelId
		{
			get
			{
				return this.channelId_;
			}
			set
			{
				this.channelId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string UserIp
		{
			get
			{
				return this.userIp_;
			}
			set
			{
				this.userIp_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string AppId
		{
			get
			{
				return this.appId_;
			}
			set
			{
				this.appId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string ProductId
		{
			get
			{
				return this.productId_;
			}
			set
			{
				this.productId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string ProductName
		{
			get
			{
				return this.productName_;
			}
			set
			{
				this.productName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string ProductDesc
		{
			get
			{
				return this.productDesc_;
			}
			set
			{
				this.productDesc_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint ProductNum
		{
			get
			{
				return this.productNum_;
			}
			set
			{
				this.productNum_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Price
		{
			get
			{
				return this.price_;
			}
			set
			{
				this.price_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string ServerId
		{
			get
			{
				return this.serverId_;
			}
			set
			{
				this.serverId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string ServerName
		{
			get
			{
				return this.serverName_;
			}
			set
			{
				this.serverName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string RoleId
		{
			get
			{
				return this.roleId_;
			}
			set
			{
				this.roleId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string RoleName
		{
			get
			{
				return this.roleName_;
			}
			set
			{
				this.roleName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string RoleLevel
		{
			get
			{
				return this.roleLevel_;
			}
			set
			{
				this.roleLevel_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string OrderId
		{
			get
			{
				return this.orderId_;
			}
			set
			{
				this.orderId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string PayNotifyUrl
		{
			get
			{
				return this.payNotifyUrl_;
			}
			set
			{
				this.payNotifyUrl_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Extension
		{
			get
			{
				return this.extension_;
			}
			set
			{
				this.extension_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string CoinName
		{
			get
			{
				return this.coinName_;
			}
			set
			{
				this.coinName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Sign
		{
			get
			{
				return this.sign_;
			}
			set
			{
				this.sign_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ChannelId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.ChannelId);
			}
			if (this.UserIp.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.UserIp);
			}
			if (this.AppId.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.AppId);
			}
			if (this.ProductId.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.ProductId);
			}
			if (this.ProductName.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(this.ProductName);
			}
			if (this.ProductDesc.Length != 0)
			{
				output.WriteRawTag(50);
				output.WriteString(this.ProductDesc);
			}
			if (this.ProductNum != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.ProductNum);
			}
			if (this.Price != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.Price);
			}
			if (this.ServerId.Length != 0)
			{
				output.WriteRawTag(74);
				output.WriteString(this.ServerId);
			}
			if (this.ServerName.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.ServerName);
			}
			if (this.RoleId.Length != 0)
			{
				output.WriteRawTag(90);
				output.WriteString(this.RoleId);
			}
			if (this.RoleName.Length != 0)
			{
				output.WriteRawTag(98);
				output.WriteString(this.RoleName);
			}
			if (this.RoleLevel.Length != 0)
			{
				output.WriteRawTag(106);
				output.WriteString(this.RoleLevel);
			}
			if (this.OrderId.Length != 0)
			{
				output.WriteRawTag(114);
				output.WriteString(this.OrderId);
			}
			if (this.PayNotifyUrl.Length != 0)
			{
				output.WriteRawTag(122);
				output.WriteString(this.PayNotifyUrl);
			}
			if (this.Extension.Length != 0)
			{
				output.WriteRawTag(130, 1);
				output.WriteString(this.Extension);
			}
			if (this.CoinName.Length != 0)
			{
				output.WriteRawTag(138, 1);
				output.WriteString(this.CoinName);
			}
			if (this.Sign.Length != 0)
			{
				output.WriteRawTag(146, 1);
				output.WriteString(this.Sign);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ChannelId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ChannelId);
			}
			if (this.UserIp.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.UserIp);
			}
			if (this.AppId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AppId);
			}
			if (this.ProductId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ProductId);
			}
			if (this.ProductName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ProductName);
			}
			if (this.ProductDesc.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ProductDesc);
			}
			if (this.ProductNum != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.ProductNum);
			}
			if (this.Price != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Price);
			}
			if (this.ServerId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ServerId);
			}
			if (this.ServerName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ServerName);
			}
			if (this.RoleId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoleId);
			}
			if (this.RoleName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoleName);
			}
			if (this.RoleLevel.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoleLevel);
			}
			if (this.OrderId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.OrderId);
			}
			if (this.PayNotifyUrl.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.PayNotifyUrl);
			}
			if (this.Extension.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.Extension);
			}
			if (this.CoinName.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.CoinName);
			}
			if (this.Sign.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.Sign);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 74U)
				{
					if (num <= 34U)
					{
						if (num <= 18U)
						{
							if (num == 10U)
							{
								this.ChannelId = input.ReadString();
								continue;
							}
							if (num == 18U)
							{
								this.UserIp = input.ReadString();
								continue;
							}
						}
						else
						{
							if (num == 26U)
							{
								this.AppId = input.ReadString();
								continue;
							}
							if (num == 34U)
							{
								this.ProductId = input.ReadString();
								continue;
							}
						}
					}
					else if (num <= 50U)
					{
						if (num == 42U)
						{
							this.ProductName = input.ReadString();
							continue;
						}
						if (num == 50U)
						{
							this.ProductDesc = input.ReadString();
							continue;
						}
					}
					else
					{
						if (num == 56U)
						{
							this.ProductNum = input.ReadUInt32();
							continue;
						}
						if (num == 64U)
						{
							this.Price = input.ReadUInt32();
							continue;
						}
						if (num == 74U)
						{
							this.ServerId = input.ReadString();
							continue;
						}
					}
				}
				else if (num <= 106U)
				{
					if (num <= 90U)
					{
						if (num == 82U)
						{
							this.ServerName = input.ReadString();
							continue;
						}
						if (num == 90U)
						{
							this.RoleId = input.ReadString();
							continue;
						}
					}
					else
					{
						if (num == 98U)
						{
							this.RoleName = input.ReadString();
							continue;
						}
						if (num == 106U)
						{
							this.RoleLevel = input.ReadString();
							continue;
						}
					}
				}
				else if (num <= 122U)
				{
					if (num == 114U)
					{
						this.OrderId = input.ReadString();
						continue;
					}
					if (num == 122U)
					{
						this.PayNotifyUrl = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 130U)
					{
						this.Extension = input.ReadString();
						continue;
					}
					if (num == 138U)
					{
						this.CoinName = input.ReadString();
						continue;
					}
					if (num == 146U)
					{
						this.Sign = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<KuaiShouOrderDto> _parser = new MessageParser<KuaiShouOrderDto>(() => new KuaiShouOrderDto());

		public const int ChannelIdFieldNumber = 1;

		private string channelId_ = "";

		public const int UserIpFieldNumber = 2;

		private string userIp_ = "";

		public const int AppIdFieldNumber = 3;

		private string appId_ = "";

		public const int ProductIdFieldNumber = 4;

		private string productId_ = "";

		public const int ProductNameFieldNumber = 5;

		private string productName_ = "";

		public const int ProductDescFieldNumber = 6;

		private string productDesc_ = "";

		public const int ProductNumFieldNumber = 7;

		private uint productNum_;

		public const int PriceFieldNumber = 8;

		private uint price_;

		public const int ServerIdFieldNumber = 9;

		private string serverId_ = "";

		public const int ServerNameFieldNumber = 10;

		private string serverName_ = "";

		public const int RoleIdFieldNumber = 11;

		private string roleId_ = "";

		public const int RoleNameFieldNumber = 12;

		private string roleName_ = "";

		public const int RoleLevelFieldNumber = 13;

		private string roleLevel_ = "";

		public const int OrderIdFieldNumber = 14;

		private string orderId_ = "";

		public const int PayNotifyUrlFieldNumber = 15;

		private string payNotifyUrl_ = "";

		public const int ExtensionFieldNumber = 16;

		private string extension_ = "";

		public const int CoinNameFieldNumber = 17;

		private string coinName_ = "";

		public const int SignFieldNumber = 18;

		private string sign_ = "";
	}
}
