using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.CrossArena
{
	public sealed class CrossArenaEnterResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<CrossArenaEnterResponse> Parser
		{
			get
			{
				return CrossArenaEnterResponse._parser;
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
		public uint Dan
		{
			get
			{
				return this.dan_;
			}
			set
			{
				this.dan_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Rank
		{
			get
			{
				return this.rank_;
			}
			set
			{
				this.rank_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Score
		{
			get
			{
				return this.score_;
			}
			set
			{
				this.score_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TeamCount
		{
			get
			{
				return this.teamCount_;
			}
			set
			{
				this.teamCount_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint CurSeason
		{
			get
			{
				return this.curSeason_;
			}
			set
			{
				this.curSeason_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint GroupId
		{
			get
			{
				return this.groupId_;
			}
			set
			{
				this.groupId_ = value;
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
			if (this.Dan != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Dan);
			}
			if (this.Rank != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.Rank);
			}
			if (this.Score != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.Score);
			}
			if (this.TeamCount != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.TeamCount);
			}
			if (this.CurSeason != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.CurSeason);
			}
			if (this.GroupId != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.GroupId);
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
			if (this.Dan != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Dan);
			}
			if (this.Rank != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Rank);
			}
			if (this.Score != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Score);
			}
			if (this.TeamCount != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TeamCount);
			}
			if (this.CurSeason != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.CurSeason);
			}
			if (this.GroupId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GroupId);
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
							this.Dan = input.ReadUInt32();
							continue;
						}
						if (num == 32U)
						{
							this.Rank = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 40U)
					{
						this.Score = input.ReadUInt32();
						continue;
					}
					if (num == 48U)
					{
						this.TeamCount = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 56U)
					{
						this.CurSeason = input.ReadUInt32();
						continue;
					}
					if (num == 64U)
					{
						this.GroupId = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<CrossArenaEnterResponse> _parser = new MessageParser<CrossArenaEnterResponse>(() => new CrossArenaEnterResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int DanFieldNumber = 3;

		private uint dan_;

		public const int RankFieldNumber = 4;

		private uint rank_;

		public const int ScoreFieldNumber = 5;

		private uint score_;

		public const int TeamCountFieldNumber = 6;

		private uint teamCount_;

		public const int CurSeasonFieldNumber = 7;

		private uint curSeason_;

		public const int GroupIdFieldNumber = 8;

		private uint groupId_;
	}
}
