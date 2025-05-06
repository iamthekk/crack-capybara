using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.User
{
	public sealed class UserOpenModelRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UserOpenModelRequest> Parser
		{
			get
			{
				return UserOpenModelRequest._parser;
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
		public RepeatedField<uint> ModelIds
		{
			get
			{
				return this.modelIds_;
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
			this.modelIds_.WriteTo(output, UserOpenModelRequest._repeated_modelIds_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			return num + this.modelIds_.CalculateSize(UserOpenModelRequest._repeated_modelIds_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 16U && num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						this.modelIds_.AddEntriesFrom(input, UserOpenModelRequest._repeated_modelIds_codec);
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

		private static readonly MessageParser<UserOpenModelRequest> _parser = new MessageParser<UserOpenModelRequest>(() => new UserOpenModelRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ModelIdsFieldNumber = 2;

		private static readonly FieldCodec<uint> _repeated_modelIds_codec = FieldCodec.ForUInt32(18U);

		private readonly RepeatedField<uint> modelIds_ = new RepeatedField<uint>();
	}
}
