using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Common
{
	public sealed class MountItemDto : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MountItemDto> Parser
		{
			get
			{
				return MountItemDto._parser;
			}
		}

		[DebuggerNonUserCode]
		public int ConfigId
		{
			get
			{
				return this.configId_;
			}
			set
			{
				this.configId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public int Star
		{
			get
			{
				return this.star_;
			}
			set
			{
				this.star_ = value;
			}
		}

		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.ConfigId);
			}
			if (this.Star != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.Star);
			}
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.ConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ConfigId);
			}
			if (this.Star != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.Star);
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
					if (num != 16U)
					{
						input.SkipLastField();
					}
					else
					{
						this.Star = input.ReadInt32();
					}
				}
				else
				{
					this.ConfigId = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<MountItemDto> _parser = new MessageParser<MountItemDto>(() => new MountItemDto());

		public const int ConfigIdFieldNumber = 1;

		private int configId_;

		public const int StarFieldNumber = 2;

		private int star_;
	}
}
