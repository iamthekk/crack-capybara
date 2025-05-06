﻿using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserGetPlayerInfoRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserGetPlayerInfoRequest> Parser
		{
			get
			{
				return UserGetPlayerInfoRequest._parser;
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
		public long PlayerUserId
		{
			get
			{
				return this.playerUserId_;
			}
			set
			{
				this.playerUserId_ = value;
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
			if (this.PlayerUserId != 0L)
			{
				output.WriteRawTag(16);
				output.WriteInt64(this.PlayerUserId);
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
			if (this.PlayerUserId != 0L)
			{
				num += 1 + CodedOutputStream.ComputeInt64Size(this.PlayerUserId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U)
					{
						input.SkipLastField();
					}
					else
					{
						this.PlayerUserId = input.ReadInt64();
					}
				}
				else
				{
					if (this.commonParams_ == null)
					{
						this.commonParams_ = new CommonParams();
					}
					input.ReadMessage(this.commonParams_);
				}
			}
		}

		private static readonly MessageParser<UserGetPlayerInfoRequest> _parser = new MessageParser<UserGetPlayerInfoRequest>(() => new UserGetPlayerInfoRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int PlayerUserIdFieldNumber = 2;

		private long playerUserId_;
	}
}
