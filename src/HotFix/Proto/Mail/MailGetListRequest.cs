using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.Mail
{
	public sealed class MailGetListRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<MailGetListRequest> Parser
		{
			get
			{
				return MailGetListRequest._parser;
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
		public ulong MaxRowId
		{
			get
			{
				return this.maxRowId_;
			}
			set
			{
				this.maxRowId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public string AppLanguage
		{
			get
			{
				return this.appLanguage_;
			}
			set
			{
				this.appLanguage_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
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
			if (this.MaxRowId != 0UL)
			{
				output.WriteRawTag(16);
				output.WriteUInt64(this.MaxRowId);
			}
			if (this.AppLanguage.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.AppLanguage);
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
			if (this.MaxRowId != 0UL)
			{
				num += 1 + CodedOutputStream.ComputeUInt64Size(this.MaxRowId);
			}
			if (this.AppLanguage.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AppLanguage);
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
						if (num != 26U)
						{
							input.SkipLastField();
						}
						else
						{
							this.AppLanguage = input.ReadString();
						}
					}
					else
					{
						this.MaxRowId = input.ReadUInt64();
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

		private static readonly MessageParser<MailGetListRequest> _parser = new MessageParser<MailGetListRequest>(() => new MailGetListRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int MaxRowIdFieldNumber = 2;

		private ulong maxRowId_;

		public const int AppLanguageFieldNumber = 3;

		private string appLanguage_ = "";
	}
}
