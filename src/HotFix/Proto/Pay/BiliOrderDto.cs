using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Pay
{
	public sealed class BiliOrderDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<BiliOrderDto> Parser
		{
			get
			{
				return BiliOrderDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public string Uid
		{
			get
			{
				return this.uid_;
			}
			set
			{
				this.uid_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Role
		{
			get
			{
				return this.role_;
			}
			set
			{
				this.role_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
		public string OutTradeNo
		{
			get
			{
				return this.outTradeNo_;
			}
			set
			{
				this.outTradeNo_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint TotalFee
		{
			get
			{
				return this.totalFee_;
			}
			set
			{
				this.totalFee_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string ExtensionInfo
		{
			get
			{
				return this.extensionInfo_;
			}
			set
			{
				this.extensionInfo_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string OrderSign
		{
			get
			{
				return this.orderSign_;
			}
			set
			{
				this.orderSign_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint GameMoney
		{
			get
			{
				return this.gameMoney_;
			}
			set
			{
				this.gameMoney_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string Subject
		{
			get
			{
				return this.subject_;
			}
			set
			{
				this.subject_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string Body
		{
			get
			{
				return this.body_;
			}
			set
			{
				this.body_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Uid.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.Uid);
			}
			if (this.Role.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Role);
			}
			if (this.NotifyUrl.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.NotifyUrl);
			}
			if (this.OutTradeNo.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.OutTradeNo);
			}
			if (this.TotalFee != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.TotalFee);
			}
			if (this.ExtensionInfo.Length != 0)
			{
				output.WriteRawTag(50);
				output.WriteString(this.ExtensionInfo);
			}
			if (this.OrderSign.Length != 0)
			{
				output.WriteRawTag(58);
				output.WriteString(this.OrderSign);
			}
			if (this.GameMoney != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.GameMoney);
			}
			if (this.Subject.Length != 0)
			{
				output.WriteRawTag(74);
				output.WriteString(this.Subject);
			}
			if (this.Body.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.Body);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Uid.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Uid);
			}
			if (this.Role.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Role);
			}
			if (this.NotifyUrl.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.NotifyUrl);
			}
			if (this.OutTradeNo.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.OutTradeNo);
			}
			if (this.TotalFee != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TotalFee);
			}
			if (this.ExtensionInfo.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.ExtensionInfo);
			}
			if (this.OrderSign.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.OrderSign);
			}
			if (this.GameMoney != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GameMoney);
			}
			if (this.Subject.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Subject);
			}
			if (this.Body.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Body);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 40U)
				{
					if (num <= 18U)
					{
						if (num == 10U)
						{
							this.Uid = input.ReadString();
							continue;
						}
						if (num == 18U)
						{
							this.Role = input.ReadString();
							continue;
						}
					}
					else
					{
						if (num == 26U)
						{
							this.NotifyUrl = input.ReadString();
							continue;
						}
						if (num == 34U)
						{
							this.OutTradeNo = input.ReadString();
							continue;
						}
						if (num == 40U)
						{
							this.TotalFee = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 58U)
				{
					if (num == 50U)
					{
						this.ExtensionInfo = input.ReadString();
						continue;
					}
					if (num == 58U)
					{
						this.OrderSign = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 64U)
					{
						this.GameMoney = input.ReadUInt32();
						continue;
					}
					if (num == 74U)
					{
						this.Subject = input.ReadString();
						continue;
					}
					if (num == 82U)
					{
						this.Body = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<BiliOrderDto> _parser = new MessageParser<BiliOrderDto>(() => new BiliOrderDto());

		public const int UidFieldNumber = 1;

		private string uid_ = "";

		public const int RoleFieldNumber = 2;

		private string role_ = "";

		public const int NotifyUrlFieldNumber = 3;

		private string notifyUrl_ = "";

		public const int OutTradeNoFieldNumber = 4;

		private string outTradeNo_ = "";

		public const int TotalFeeFieldNumber = 5;

		private uint totalFee_;

		public const int ExtensionInfoFieldNumber = 6;

		private string extensionInfo_ = "";

		public const int OrderSignFieldNumber = 7;

		private string orderSign_ = "";

		public const int GameMoneyFieldNumber = 8;

		private uint gameMoney_;

		public const int SubjectFieldNumber = 9;

		private string subject_ = "";

		public const int BodyFieldNumber = 10;

		private string body_ = "";
	}
}
