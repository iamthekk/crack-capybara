using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Develop
{
	public sealed class DevelopLoginResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<DevelopLoginResponse> Parser
		{
			get
			{
				return DevelopLoginResponse._parser;
			}
		}

		[DebuggerNonUserCode]
		public int Code
		{
			get
			{
				return this.code_;
			}
			set
			{
				this.code_ = value;
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
		public long UserId
		{
			get
			{
				return this.userId_;
			}
			set
			{
				this.userId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long DbIdx
		{
			get
			{
				return this.dbIdx_;
			}
			set
			{
				this.dbIdx_ = value;
			}
		}

		[DebuggerNonUserCode]
		public long TableIdx
		{
			get
			{
				return this.tableIdx_;
			}
			set
			{
				this.tableIdx_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Coins
		{
			get
			{
				return this.coins_;
			}
			set
			{
				this.coins_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Diamonds
		{
			get
			{
				return this.diamonds_;
			}
			set
			{
				this.diamonds_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int ChapterId
		{
			get
			{
				return this.chapterId_;
			}
			set
			{
				this.chapterId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint LoginType
		{
			get
			{
				return this.loginType_;
			}
			set
			{
				this.loginType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Level
		{
			get
			{
				return this.level_;
			}
			set
			{
				this.level_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Exp
		{
			get
			{
				return this.exp_;
			}
			set
			{
				this.exp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string Extra
		{
			get
			{
				return this.extra_;
			}
			set
			{
				this.extra_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public uint MissionId
		{
			get
			{
				return this.missionId_;
			}
			set
			{
				this.missionId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Life
		{
			get
			{
				return this.life_;
			}
			set
			{
				this.life_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonParams);
			}
			if (this.UserId != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.UserId);
			}
			if (this.DbIdx != 0L)
			{
				output.WriteRawTag(32);
				output.WriteInt64(this.DbIdx);
			}
			if (this.TableIdx != 0L)
			{
				output.WriteRawTag(40);
				output.WriteInt64(this.TableIdx);
			}
			if (this.Coins != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.Coins);
			}
			if (this.Diamonds != 0)
			{
				output.WriteRawTag(56);
				output.WriteInt32(this.Diamonds);
			}
			if (this.ChapterId != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.ChapterId);
			}
			if (this.LoginType != 0U)
			{
				output.WriteRawTag(72);
				output.WriteUInt32(this.LoginType);
			}
			if (this.Level != 0U)
			{
				output.WriteRawTag(80);
				output.WriteUInt32(this.Level);
			}
			if (this.Exp != 0U)
			{
				output.WriteRawTag(88);
				output.WriteUInt32(this.Exp);
			}
			if (this.Extra.Length != 0)
			{
				output.WriteRawTag(98);
				output.WriteString(this.Extra);
			}
			if (this.MissionId != 0U)
			{
				output.WriteRawTag(104);
				output.WriteUInt32(this.MissionId);
			}
			if (this.Life != 0U)
			{
				output.WriteRawTag(112);
				output.WriteUInt32(this.Life);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.UserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.UserId);
			}
			if (this.DbIdx != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.DbIdx);
			}
			if (this.TableIdx != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.TableIdx);
			}
			if (this.Coins != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Coins);
			}
			if (this.Diamonds != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Diamonds);
			}
			if (this.ChapterId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ChapterId);
			}
			if (this.LoginType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.LoginType);
			}
			if (this.Level != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Level);
			}
			if (this.Exp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Exp);
			}
			if (this.Extra.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Extra);
			}
			if (this.MissionId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.MissionId);
			}
			if (this.Life != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Life);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 56U)
				{
					if (num <= 24U)
					{
						if (num == 8U)
						{
							this.Code = input.ReadInt32();
							continue;
						}
						if (num == 18U)
						{
							if (this.commonParams_ == null)
							{
								this.commonParams_ = new CommonParams();
							}
							input.ReadMessage(this.commonParams_);
							continue;
						}
						if (num == 24U)
						{
							this.UserId = input.ReadInt64();
							continue;
						}
					}
					else if (num <= 40U)
					{
						if (num == 32U)
						{
							this.DbIdx = input.ReadInt64();
							continue;
						}
						if (num == 40U)
						{
							this.TableIdx = input.ReadInt64();
							continue;
						}
					}
					else
					{
						if (num == 48U)
						{
							this.Coins = input.ReadInt32();
							continue;
						}
						if (num == 56U)
						{
							this.Diamonds = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 80U)
				{
					if (num == 64U)
					{
						this.ChapterId = input.ReadInt32();
						continue;
					}
					if (num == 72U)
					{
						this.LoginType = input.ReadUInt32();
						continue;
					}
					if (num == 80U)
					{
						this.Level = input.ReadUInt32();
						continue;
					}
				}
				else if (num <= 98U)
				{
					if (num == 88U)
					{
						this.Exp = input.ReadUInt32();
						continue;
					}
					if (num == 98U)
					{
						this.Extra = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 104U)
					{
						this.MissionId = input.ReadUInt32();
						continue;
					}
					if (num == 112U)
					{
						this.Life = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<DevelopLoginResponse> _parser = new MessageParser<DevelopLoginResponse>(() => new DevelopLoginResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonParamsFieldNumber = 2;

		private CommonParams commonParams_;

		public const int UserIdFieldNumber = 3;

		private long userId_;

		public const int DbIdxFieldNumber = 4;

		private long dbIdx_;

		public const int TableIdxFieldNumber = 5;

		private long tableIdx_;

		public const int CoinsFieldNumber = 6;

		private int coins_;

		public const int DiamondsFieldNumber = 7;

		private int diamonds_;

		public const int ChapterIdFieldNumber = 8;

		private int chapterId_;

		public const int LoginTypeFieldNumber = 9;

		private uint loginType_;

		public const int LevelFieldNumber = 10;

		private uint level_;

		public const int ExpFieldNumber = 11;

		private uint exp_;

		public const int ExtraFieldNumber = 12;

		private string extra_ = "";

		public const int MissionIdFieldNumber = 13;

		private uint missionId_;

		public const int LifeFieldNumber = 14;

		private uint life_;
	}
}
