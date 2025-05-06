using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Socket.Guild
{
	public sealed class SocketQuitGroupRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<SocketQuitGroupRequest> Parser
		{
			get
			{
				return SocketQuitGroupRequest._parser;
			}
		}

		[DebuggerNonUserCode]
		public uint GroupType
		{
			get
			{
				return this.groupType_;
			}
			set
			{
				this.groupType_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string GroupId
		{
			get
			{
				return this.groupId_;
			}
			set
			{
				this.groupId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.GroupType != 0U)
			{
				output.WriteRawTag(8);
				output.WriteUInt32(this.GroupType);
			}
			if (this.GroupId.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.GroupId);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.GroupType != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.GroupType);
			}
			if (this.GroupId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.GroupId);
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
						this.GroupId = input.ReadString();
					}
				}
				else
				{
					this.GroupType = input.ReadUInt32();
				}
			}
		}

		private static readonly MessageParser<SocketQuitGroupRequest> _parser = new MessageParser<SocketQuitGroupRequest>(() => new SocketQuitGroupRequest());

		public const int GroupTypeFieldNumber = 1;

		private uint groupType_;

		public const int GroupIdFieldNumber = 2;

		private string groupId_ = "";
	}
}
