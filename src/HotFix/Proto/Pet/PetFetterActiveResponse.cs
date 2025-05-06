using System;
using System.Diagnostics;
using Google.Protobuf;

namespace Proto.Pet
{
	public sealed class PetFetterActiveResponse : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<PetFetterActiveResponse> Parser
		{
			get
			{
				return PetFetterActiveResponse._parser;
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
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Code != 0)
			{
				output.WriteRawTag(8);
				output.WriteInt32(this.Code);
			}
			if (this.ConfigId != 0)
			{
				output.WriteRawTag(16);
				output.WriteInt32(this.ConfigId);
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
			if (this.ConfigId != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ConfigId);
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
						this.ConfigId = input.ReadInt32();
					}
				}
				else
				{
					this.Code = input.ReadInt32();
				}
			}
		}

		private static readonly MessageParser<PetFetterActiveResponse> _parser = new MessageParser<PetFetterActiveResponse>(() => new PetFetterActiveResponse());

		public const int CodeFieldNumber = 1;

		private int code_;

		public const int ConfigIdFieldNumber = 2;

		private int configId_;
	}
}
