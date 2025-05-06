using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Talents
{
	public sealed class TalentsLvUpResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<TalentsLvUpResponse> Parser
		{
			get
			{
				return TalentsLvUpResponse._parser;
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
		public CommonData CommonData
		{
			get
			{
				return this.commonData_;
			}
			set
			{
				this.commonData_ = value;
			}
		}

		[DebuggerNonUserCode]
		public TalentsInfo TalentsInfo
		{
			get
			{
				return this.talentsInfo_;
			}
			set
			{
				this.talentsInfo_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int RewardConfigId
		{
			get
			{
				return this.rewardConfigId_;
			}
			set
			{
				this.rewardConfigId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint CritType
		{
			get
			{
				return this.critType_;
			}
			set
			{
				this.critType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint CritRate
		{
			get
			{
				return this.critRate_;
			}
			set
			{
				this.critRate_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint AddLvAndExp
		{
			get
			{
				return this.addLvAndExp_;
			}
			set
			{
				this.addLvAndExp_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int TalentId
		{
			get
			{
				return this.talentId_;
			}
			set
			{
				this.talentId_ = value;
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
			if (this.commonData_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.CommonData);
			}
			if (this.talentsInfo_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.TalentsInfo);
			}
			if (this.RewardConfigId != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.RewardConfigId);
			}
			if (this.CritType != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.CritType);
			}
			if (this.CritRate != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.CritRate);
			}
			if (this.AddLvAndExp != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.AddLvAndExp);
			}
			if (this.TalentId != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.TalentId);
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
			if (this.commonData_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonData);
			}
			if (this.talentsInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.TalentsInfo);
			}
			if (this.RewardConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.RewardConfigId);
			}
			if (this.CritType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.CritType);
			}
			if (this.CritRate != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.CritRate);
			}
			if (this.AddLvAndExp != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.AddLvAndExp);
			}
			if (this.TalentId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.TalentId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 32U)
				{
					if (num <= 18U)
					{
						if (num == 8U)
						{
							this.Code = input.ReadInt32();
							continue;
						}
						if (num == 18U)
						{
							if (this.commonData_ == null)
							{
								this.commonData_ = new CommonData();
							}
							input.ReadMessage(this.commonData_);
							continue;
						}
					}
					else
					{
						if (num == 26U)
						{
							if (this.talentsInfo_ == null)
							{
								this.talentsInfo_ = new TalentsInfo();
							}
							input.ReadMessage(this.talentsInfo_);
							continue;
						}
						if (num == 32U)
						{
							this.RewardConfigId = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 40U)
					{
						this.CritType = input.ReadUInt32();
						continue;
					}
					if (num == 48U)
					{
						this.CritRate = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 56U)
					{
						this.AddLvAndExp = input.ReadUInt32();
						continue;
					}
					if (num == 64U)
					{
						this.TalentId = input.ReadInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<TalentsLvUpResponse> _parser = new MessageParser<TalentsLvUpResponse>(() => new TalentsLvUpResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int TalentsInfoFieldNumber = 3;

		private TalentsInfo talentsInfo_;

		public const int RewardConfigIdFieldNumber = 4;

		private int rewardConfigId_;

		public const int CritTypeFieldNumber = 5;

		private uint critType_;

		public const int CritRateFieldNumber = 6;

		private uint critRate_;

		public const int AddLvAndExpFieldNumber = 7;

		private uint addLvAndExp_;

		public const int TalentIdFieldNumber = 8;

		private int talentId_;
	}
}
