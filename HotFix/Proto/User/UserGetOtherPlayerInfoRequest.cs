using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserGetOtherPlayerInfoRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserGetOtherPlayerInfoRequest> Parser
		{
			get
			{
				return UserGetOtherPlayerInfoRequest._parser;
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
		public RepeatedField<long> OtherUserIds
		{
			get
			{
				return this.otherUserIds_;
			}
		}

		[DebuggerNonUserCode]
		public bool NeedAttrDetail
		{
			get
			{
				return this.needAttrDetail_;
			}
			set
			{
				this.needAttrDetail_ = value;
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
			this.otherUserIds_.WriteTo(output, UserGetOtherPlayerInfoRequest._repeated_otherUserIds_codec);
			if (this.NeedAttrDetail)
			{
				output.WriteRawTag(24);
				output.WriteBool(this.NeedAttrDetail);
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
			num += this.otherUserIds_.CalculateSize(UserGetOtherPlayerInfoRequest._repeated_otherUserIds_codec);
			if (this.NeedAttrDetail)
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
						goto IL_0046;
					}
				}
				else
				{
					if (num == 18U)
					{
						goto IL_0046;
					}
					if (num == 24U)
					{
						this.NeedAttrDetail = input.ReadBool();
						continue;
					}
				}
				input.SkipLastField();
				continue;
				IL_0046:
				this.otherUserIds_.AddEntriesFrom(input, UserGetOtherPlayerInfoRequest._repeated_otherUserIds_codec);
			}
		}

		private static readonly MessageParser<UserGetOtherPlayerInfoRequest> _parser = new MessageParser<UserGetOtherPlayerInfoRequest>(() => new UserGetOtherPlayerInfoRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int OtherUserIdsFieldNumber = 2;

		private static readonly FieldCodec<long> _repeated_otherUserIds_codec = FieldCodec.ForInt64(18U);

		private readonly RepeatedField<long> otherUserIds_ = new RepeatedField<long>();

		public const int NeedAttrDetailFieldNumber = 3;

		private bool needAttrDetail_;
	}
}
