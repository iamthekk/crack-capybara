using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Mission
{
	public sealed class WorldBossGetInfoResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<WorldBossGetInfoResponse> Parser
		{
			get
			{
				return WorldBossGetInfoResponse._parser;
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
		public WorldBossDto Info
		{
			get
			{
				return this.info_;
			}
			set
			{
				this.info_ = value;
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
			if (this.info_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.Info);
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
			if (this.info_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Info);
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
						input.SkipLastField();
					}
					else
					{
						if (this.info_ == null)
						{
							this.info_ = new WorldBossDto();
						}
						input.ReadMessage(this.info_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<WorldBossGetInfoResponse> _parser = new MessageParser<WorldBossGetInfoResponse>(() => new WorldBossGetInfoResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int InfoFieldNumber = 2;

		private WorldBossDto info_;
	}
}
