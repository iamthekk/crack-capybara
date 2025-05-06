using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserUpdateSystemMaskRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserUpdateSystemMaskRequest> Parser
		{
			get
			{
				return UserUpdateSystemMaskRequest._parser;
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
		public uint Position
		{
			get
			{
				return this.position_;
			}
			set
			{
				this.position_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint Value
		{
			get
			{
				return this.value_;
			}
			set
			{
				this.value_ = value;
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
			if (this.Position != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.Position);
			}
			if (this.Value != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.Value);
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
			if (this.Position != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Position);
			}
			if (this.Value != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.Value);
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
						if (num != 24U)
						{
							input.SkipLastField();
						}
						else
						{
							this.Value = input.ReadUInt32();
						}
					}
					else
					{
						this.Position = input.ReadUInt32();
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

		private static readonly MessageParser<UserUpdateSystemMaskRequest> _parser = new MessageParser<UserUpdateSystemMaskRequest>(() => new UserUpdateSystemMaskRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int PositionFieldNumber = 2;

		private uint position_;

		public const int ValueFieldNumber = 3;

		private uint value_;
	}
}
