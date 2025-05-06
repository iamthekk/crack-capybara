using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Pay
{
	public sealed class VivoOrderDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<VivoOrderDto> Parser
		{
			get
			{
				return VivoOrderDto._parser;
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
		public string CpOrderNumber
		{
			get
			{
				return this.cpOrderNumber_;
			}
			set
			{
				this.cpOrderNumber_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
		public string OrderAmount
		{
			get
			{
				return this.orderAmount_;
			}
			set
			{
				this.orderAmount_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string VivoSignature
		{
			get
			{
				return this.vivoSignature_;
			}
			set
			{
				this.vivoSignature_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Extuid
		{
			get
			{
				return this.extuid_;
			}
			set
			{
				this.extuid_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string NotifyUrl
		{
			get
			{
				return this.notifyUrl_;
			}
			set
			{
				this.notifyUrl_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string ExpireTime
		{
			get
			{
				return this.expireTime_;
			}
			set
			{
				this.expireTime_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Level
		{
			get
			{
				return this.level_;
			}
			set
			{
				this.level_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Vip
		{
			get
			{
				return this.vip_;
			}
			set
			{
				this.vip_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Balance
		{
			get
			{
				return this.balance_;
			}
			set
			{
				this.balance_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Party
		{
			get
			{
				return this.party_;
			}
			set
			{
				this.party_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
		public string ExtInfo
		{
			get
			{
				return this.extInfo_;
			}
			set
			{
				this.extInfo_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.AppId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.AppId);
			}
			if (this.CpOrderNumber.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.CpOrderNumber);
			}
			if (this.ProductName.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.ProductName);
			}
			if (this.ProductDesc.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.ProductDesc);
			}
			if (this.OrderAmount.Length != 0)
			{
				output.WriteRawTag(42);
				output.WriteString(this.OrderAmount);
			}
			if (this.VivoSignature.Length != 0)
			{
				output.WriteRawTag(50);
				output.WriteString(this.VivoSignature);
			}
			if (this.Extuid.Length != 0)
			{
				output.WriteRawTag(58);
				output.WriteString(this.Extuid);
			}
			if (this.NotifyUrl.Length != 0)
			{
				output.WriteRawTag(66);
				output.WriteString(this.NotifyUrl);
			}
			if (this.ExpireTime.Length != 0)
			{
				output.WriteRawTag(74);
				output.WriteString(this.ExpireTime);
			}
			if (this.Level.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.Level);
			}
			if (this.Vip.Length != 0)
			{
				output.WriteRawTag(90);
				output.WriteString(this.Vip);
			}
			if (this.Balance.Length != 0)
			{
				output.WriteRawTag(98);
				output.WriteString(this.Balance);
			}
			if (this.Party.Length != 0)
			{
				output.WriteRawTag(106);
				output.WriteString(this.Party);
			}
			if (this.RoleId.Length != 0)
			{
				output.WriteRawTag(114);
				output.WriteString(this.RoleId);
			}
			if (this.RoleName.Length != 0)
			{
				output.WriteRawTag(122);
				output.WriteString(this.RoleName);
			}
			if (this.ServerName.Length != 0)
			{
				output.WriteRawTag(130, 1);
				output.WriteString(this.ServerName);
			}
			if (this.ExtInfo.Length != 0)
			{
				output.WriteRawTag(138, 1);
				output.WriteString(this.ExtInfo);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.AppId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AppId);
			}
			if (this.CpOrderNumber.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.CpOrderNumber);
			}
			if (this.ProductName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ProductName);
			}
			if (this.ProductDesc.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ProductDesc);
			}
			if (this.OrderAmount.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.OrderAmount);
			}
			if (this.VivoSignature.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.VivoSignature);
			}
			if (this.Extuid.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Extuid);
			}
			if (this.NotifyUrl.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.NotifyUrl);
			}
			if (this.ExpireTime.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ExpireTime);
			}
			if (this.Level.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Level);
			}
			if (this.Vip.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Vip);
			}
			if (this.Balance.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Balance);
			}
			if (this.Party.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Party);
			}
			if (this.RoleId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoleId);
			}
			if (this.RoleName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoleName);
			}
			if (this.ServerName.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.ServerName);
			}
			if (this.ExtInfo.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.ExtInfo);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 66U)
				{
					if (num <= 34U)
					{
						if (num <= 18U)
						{
							if (num == 10U)
							{
								this.AppId = input.ReadString();
								continue;
							}
							if (num == 18U)
							{
								this.CpOrderNumber = input.ReadString();
								continue;
							}
						}
						else
						{
							if (num == 26U)
							{
								this.ProductName = input.ReadString();
								continue;
							}
							if (num == 34U)
							{
								this.ProductDesc = input.ReadString();
								continue;
							}
						}
					}
					else if (num <= 50U)
					{
						if (num == 42U)
						{
							this.OrderAmount = input.ReadString();
							continue;
						}
						if (num == 50U)
						{
							this.VivoSignature = input.ReadString();
							continue;
						}
					}
					else
					{
						if (num == 58U)
						{
							this.Extuid = input.ReadString();
							continue;
						}
						if (num == 66U)
						{
							this.NotifyUrl = input.ReadString();
							continue;
						}
					}
				}
				else if (num <= 98U)
				{
					if (num <= 82U)
					{
						if (num == 74U)
						{
							this.ExpireTime = input.ReadString();
							continue;
						}
						if (num == 82U)
						{
							this.Level = input.ReadString();
							continue;
						}
					}
					else
					{
						if (num == 90U)
						{
							this.Vip = input.ReadString();
							continue;
						}
						if (num == 98U)
						{
							this.Balance = input.ReadString();
							continue;
						}
					}
				}
				else if (num <= 114U)
				{
					if (num == 106U)
					{
						this.Party = input.ReadString();
						continue;
					}
					if (num == 114U)
					{
						this.RoleId = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 122U)
					{
						this.RoleName = input.ReadString();
						continue;
					}
					if (num == 130U)
					{
						this.ServerName = input.ReadString();
						continue;
					}
					if (num == 138U)
					{
						this.ExtInfo = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<VivoOrderDto> _parser = new MessageParser<VivoOrderDto>(() => new VivoOrderDto());

		public const int AppIdFieldNumber = 1;

		private string appId_ = "";

		public const int CpOrderNumberFieldNumber = 2;

		private string cpOrderNumber_ = "";

		public const int ProductNameFieldNumber = 3;

		private string productName_ = "";

		public const int ProductDescFieldNumber = 4;

		private string productDesc_ = "";

		public const int OrderAmountFieldNumber = 5;

		private string orderAmount_ = "";

		public const int VivoSignatureFieldNumber = 6;

		private string vivoSignature_ = "";

		public const int ExtuidFieldNumber = 7;

		private string extuid_ = "";

		public const int NotifyUrlFieldNumber = 8;

		private string notifyUrl_ = "";

		public const int ExpireTimeFieldNumber = 9;

		private string expireTime_ = "";

		public const int LevelFieldNumber = 10;

		private string level_ = "";

		public const int VipFieldNumber = 11;

		private string vip_ = "";

		public const int BalanceFieldNumber = 12;

		private string balance_ = "";

		public const int PartyFieldNumber = 13;

		private string party_ = "";

		public const int RoleIdFieldNumber = 14;

		private string roleId_ = "";

		public const int RoleNameFieldNumber = 15;

		private string roleName_ = "";

		public const int ServerNameFieldNumber = 16;

		private string serverName_ = "";

		public const int ExtInfoFieldNumber = 17;

		private string extInfo_ = "";
	}
}
