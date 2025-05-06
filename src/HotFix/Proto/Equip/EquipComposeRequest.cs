using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Proto.Common;

namespace Proto.Equip
{
	public sealed class EquipComposeRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<EquipComposeRequest> Parser
		{
			get
			{
				return EquipComposeRequest._parser;
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
		public RepeatedField<EquipComposeData> ComposeData
		{
			get
			{
				return this.composeData_;
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
			this.composeData_.WriteTo(output, EquipComposeRequest._repeated_composeData_codec);
		}

		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.commonParams_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CommonParams);
			}
			return num + this.composeData_.CalculateSize(EquipComposeRequest._repeated_composeData_codec);
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 18U)
					{
						input.SkipLastField();
					}
					else
					{
						this.composeData_.AddEntriesFrom(input, EquipComposeRequest._repeated_composeData_codec);
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

		private static readonly MessageParser<EquipComposeRequest> _parser = new MessageParser<EquipComposeRequest>(() => new EquipComposeRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int ComposeDataFieldNumber = 2;

		private static readonly FieldCodec<EquipComposeData> _repeated_composeData_codec = FieldCodec.ForMessage<EquipComposeData>(18U, EquipComposeData.Parser);

		private readonly RepeatedField<EquipComposeData> composeData_ = new RepeatedField<EquipComposeData>();
	}
}
