using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.NewWorld
{
	public sealed class LikeResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<LikeResponse> Parser
		{
			get
			{
				return LikeResponse._parser;
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
		public RepeatedField<int> LikeInfo
		{
			get
			{
				return this.likeInfo_;
			}
		}

		[DebuggerNonUserCode]
		public long LikeRefreshtime
		{
			get
			{
				return this.likeRefreshtime_;
			}
			set
			{
				this.likeRefreshtime_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> LikeCount
		{
			get
			{
				return this.likeCount_;
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
			this.likeInfo_.WriteTo(output, LikeResponse._repeated_likeInfo_codec);
			if (this.LikeRefreshtime != 0L)
			{
				output.WriteRawTag(32);
				output.WriteInt64(this.LikeRefreshtime);
			}
			this.likeCount_.WriteTo(output, LikeResponse._repeated_likeCount_codec);
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
			num += this.likeInfo_.CalculateSize(LikeResponse._repeated_likeInfo_codec);
			if (this.LikeRefreshtime != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.LikeRefreshtime);
			}
			return num + this.likeCount_.CalculateSize(LikeResponse._repeated_likeCount_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
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
						if (this.commonData_ == null)
						{
							this.commonData_ = new CommonData();
						}
						input.ReadMessage(this.commonData_);
						continue;
					}
					if (num == 24U)
					{
						goto IL_006C;
					}
				}
				else if (num <= 32U)
				{
					if (num == 26U)
					{
						goto IL_006C;
					}
					if (num == 32U)
					{
						this.LikeRefreshtime = input.ReadInt64();
						continue;
					}
				}
				else if (num == 40U || num == 42U)
				{
					this.likeCount_.AddEntriesFrom(input, LikeResponse._repeated_likeCount_codec);
					continue;
				}
				input.SkipLastField();
				continue;
				IL_006C:
				this.likeInfo_.AddEntriesFrom(input, LikeResponse._repeated_likeInfo_codec);
			}
		}

		private static readonly MessageParser<LikeResponse> _parser = new MessageParser<LikeResponse>(() => new LikeResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int CommonDataFieldNumber = 2;

		private CommonData commonData_;

		public const int LikeInfoFieldNumber = 3;

		private static readonly FieldCodec<int> _repeated_likeInfo_codec = FieldCodec.ForInt32(26U);

		private readonly RepeatedField<int> likeInfo_ = new RepeatedField<int>();

		public const int LikeRefreshtimeFieldNumber = 4;

		private long likeRefreshtime_;

		public const int LikeCountFieldNumber = 5;

		private static readonly FieldCodec<int> _repeated_likeCount_codec = FieldCodec.ForInt32(42U);

		private readonly RepeatedField<int> likeCount_ = new RepeatedField<int>();
	}
}
