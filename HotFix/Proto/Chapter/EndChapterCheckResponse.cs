using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Chapter
{
	public sealed class EndChapterCheckResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<EndChapterCheckResponse> Parser
		{
			get
			{
				return EndChapterCheckResponse._parser;
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
		public int Result
		{
			get
			{
				return this.result_;
			}
			set
			{
				this.result_ = value;
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
		public int ReviveCount
		{
			get
			{
				return this.reviveCount_;
			}
			set
			{
				this.reviveCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string BattleServerLogId
		{
			get
			{
				return this.battleServerLogId_;
			}
			set
			{
				this.battleServerLogId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public string BattleServerLogData
		{
			get
			{
				return this.battleServerLogData_;
			}
			set
			{
				this.battleServerLogData_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.Result != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Result);
			}
			if (this.Seed != 0)
			{
				output.WriteRawTag(32);
				output.WriteInt32(this.Seed);
			}
			if (this.userInfo_ != null)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.UserInfo);
			}
			if (this.ReviveCount != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.ReviveCount);
			}
			if (this.BattleServerLogId.Length != 0)
			{
				output.WriteRawTag(58);
				output.WriteString(this.BattleServerLogId);
			}
			if (this.BattleServerLogData.Length != 0)
			{
				output.WriteRawTag(66);
				output.WriteString(this.BattleServerLogData);
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
			if (this.Result != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Result);
			}
			if (this.Seed != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Seed);
			}
			if (this.userInfo_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.UserInfo);
			}
			if (this.ReviveCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ReviveCount);
			}
			if (this.BattleServerLogId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.BattleServerLogId);
			}
			if (this.BattleServerLogData.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.BattleServerLogData);
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
						if (num == 24U)
						{
							this.Result = input.ReadInt32();
							continue;
						}
						if (num == 32U)
						{
							this.Seed = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 42U)
					{
						if (this.userInfo_ == null)
						{
							this.userInfo_ = new BattleUserDto();
						}
						input.ReadMessage(this.userInfo_);
						continue;
					}
					if (num == 48U)
					{
						this.ReviveCount = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 58U)
					{
						this.BattleServerLogId = input.ReadString();
						continue;
					}
					if (num == 66U)
					{
						this.BattleServerLogData = input.ReadString();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<EndChapterCheckResponse> _parser = new MessageParser<EndChapterCheckResponse>(() => new EndChapterCheckResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int ResultFieldNumber = 3;

		private int result_;

		public const int SeedFieldNumber = 4;

		private int seed_;

		public const int UserInfoFieldNumber = 5;

		private BattleUserDto userInfo_;

		public const int ReviveCountFieldNumber = 6;

		private int reviveCount_;

		public const int BattleServerLogIdFieldNumber = 7;

		private string battleServerLogId_ = "";

		public const int BattleServerLogDataFieldNumber = 8;

		private string battleServerLogData_ = "";
	}
}
