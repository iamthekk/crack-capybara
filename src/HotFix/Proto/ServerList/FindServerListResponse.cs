using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.ServerList
{
	public sealed class FindServerListResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<FindServerListResponse> Parser
		{
			get
			{
				return FindServerListResponse._parser;
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
		public ZoneInfoDto ServerInfoDto
		{
			get
			{
				return this.serverInfoDto_;
			}
			set
			{
				this.serverInfoDto_ = value;
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
			if (this.serverInfoDto_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.ServerInfoDto);
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
			if (this.serverInfoDto_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.ServerInfoDto);
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
						if (this.serverInfoDto_ == null)
						{
							this.serverInfoDto_ = new ZoneInfoDto();
						}
						input.ReadMessage(this.serverInfoDto_);
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<FindServerListResponse> _parser = new MessageParser<FindServerListResponse>(() => new FindServerListResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int ServerInfoDtoFieldNumber = 2;

		private ZoneInfoDto serverInfoDto_;
	}
}
