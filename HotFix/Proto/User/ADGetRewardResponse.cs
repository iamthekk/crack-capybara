using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.User
{
	public sealed class ADGetRewardResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<ADGetRewardResponse> Parser
		{
			get
			{
				return ADGetRewardResponse._parser;
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
		public long FreeAdLifeRefreshtime
		{
			get
			{
				return this.freeAdLifeRefreshtime_;
			}
			set
			{
				this.freeAdLifeRefreshtime_ = value;
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
			if (this.FreeAdLifeRefreshtime != 0L)
			{
				output.WriteRawTag(24);
				output.WriteInt64(this.FreeAdLifeRefreshtime);
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
			if (this.FreeAdLifeRefreshtime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.FreeAdLifeRefreshtime);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 18U)
					{
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.FreeAdLifeRefreshtime = input.ReadInt64();
						}
					}
					else
					{
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<ADGetRewardResponse> _parser = new MessageParser<ADGetRewardResponse>(() => new ADGetRewardResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int FreeAdLifeRefreshtimeFieldNumber = 3;

		private long freeAdLifeRefreshtime_;
	}
}
