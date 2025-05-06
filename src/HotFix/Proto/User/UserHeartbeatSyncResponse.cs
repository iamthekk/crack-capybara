using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Proto.User
{
	public sealed class UserHeartbeatSyncResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserHeartbeatSyncResponse> Parser
		{
			get
			{
				return UserHeartbeatSyncResponse._parser;
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
		public ulong Time
		{
			get
			{
				return this.time_;
			}
			set
			{
				this.time_ = value;
			}
		}

		[DebuggerNonUserCode]
		public bool SetCloseFunction
		{
			get
			{
				return this.setCloseFunction_;
			}
			set
			{
				this.setCloseFunction_ = value;
			}
		}

		[DebuggerNonUserCode]
		public RepeatedField<int> CloseFunctionId
		{
			get
			{
				return this.closeFunctionId_;
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
			if (this.Time != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.Time);
			}
			if (this.SetCloseFunction)
			{
				output.WriteRawTag(24);
				output.WriteBool(this.SetCloseFunction);
			}
			this.closeFunctionId_.WriteTo(output, UserHeartbeatSyncResponse._repeated_closeFunctionId_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Code != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Code);
			}
			if (this.Time != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.Time);
			}
			if (this.SetCloseFunction)
			{
				num += 2;
			}
			return num + this.closeFunctionId_.CalculateSize(UserHeartbeatSyncResponse._repeated_closeFunctionId_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 16U)
				{
					if (num == 8U)
					{
						this.Code = input.ReadInt32();
						continue;
					}
					if (num == 16U)
					{
						this.Time = input.ReadUInt64();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.SetCloseFunction = input.ReadBool();
						continue;
					}
					if (num == 32U || num == 34U)
					{
						this.closeFunctionId_.AddEntriesFrom(input, UserHeartbeatSyncResponse._repeated_closeFunctionId_codec);
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UserHeartbeatSyncResponse> _parser = new MessageParser<UserHeartbeatSyncResponse>(() => new UserHeartbeatSyncResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int TimeFieldNumber = 2;

		private ulong time_;

		public const int SetCloseFunctionFieldNumber = 3;

		private bool setCloseFunction_;

		public const int CloseFunctionIdFieldNumber = 4;

		private static readonly FieldCodec<int> _repeated_closeFunctionId_codec = FieldCodec.ForInt32(34U);

		private readonly RepeatedField<int> closeFunctionId_ = new RepeatedField<int>();
	}
}
