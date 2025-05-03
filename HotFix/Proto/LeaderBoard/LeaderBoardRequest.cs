using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.LeaderBoard
{
	public sealed class LeaderBoardRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<LeaderBoardRequest> Parser
		{
			get
			{
				return LeaderBoardRequest._parser;
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
		public LeaderBoardType Type
		{
			get
			{
				return this.type_;
			}
			set
			{
				this.type_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Page
		{
			get
			{
				return this.page_;
			}
			set
			{
				this.page_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool IncludeSelf
		{
			get
			{
				return this.includeSelf_;
			}
			set
			{
				this.includeSelf_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.commonParams_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.CommonParams);
			}
			if (this.Type != LeaderBoardType.Default)
			{
				output.WriteRawTag(16);
				output.WriteEnum((int)this.Type);
			}
			if (this.Page != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.Page);
			}
			if (this.IncludeSelf)
			{
				output.WriteRawTag(32);
				output.WriteBool(this.IncludeSelf);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			if (this.Type != LeaderBoardType.Default)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.Type);
			}
			if (this.Page != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Page);
			}
			if (this.IncludeSelf)
			{
				num += 2;
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 10U)
					{
						if (this.commonParams_ == null)
						{
							this.commonParams_ = new CommonParams();
						}
						input.ReadMessage(this.commonParams_);
						continue;
					}
					if (num == 16U)
					{
						this.type_ = (LeaderBoardType)input.ReadEnum();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.Page = input.ReadInt32();
						continue;
					}
					if (num == 32U)
					{
						this.IncludeSelf = input.ReadBool();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<LeaderBoardRequest> _parser = new MessageParser<LeaderBoardRequest>(() => new LeaderBoardRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int TypeFieldNumber = 2;

		private LeaderBoardType type_;

		public const int PageFieldNumber = 3;

		private int page_;

		public const int IncludeSelfFieldNumber = 4;

		private bool includeSelf_;
	}
}
