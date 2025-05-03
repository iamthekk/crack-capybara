using System;
using System.Diagnostics;
using Google.Protobuf;
using Proto.Common;

namespace Proto.User
{
	public sealed class UpdateUserAvatarRequest : IMessage
	{
		[DebuggerNonUserCode]
		public static MessageParser<UpdateUserAvatarRequest> Parser
		{
			get
			{
				return UpdateUserAvatarRequest._parser;
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
		public uint AvatarId
		{
			get
			{
				return this.avatarId_;
			}
			set
			{
				this.avatarId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint AvatarFrameId
		{
			get
			{
				return this.avatarFrameId_;
			}
			set
			{
				this.avatarFrameId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint SkinHeaddressId
		{
			get
			{
				return this.skinHeaddressId_;
			}
			set
			{
				this.skinHeaddressId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint SkinBodyId
		{
			get
			{
				return this.skinBodyId_;
			}
			set
			{
				this.skinBodyId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint SkinAccessoryId
		{
			get
			{
				return this.skinAccessoryId_;
			}
			set
			{
				this.skinAccessoryId_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint BackGround
		{
			get
			{
				return this.backGround_;
			}
			set
			{
				this.backGround_ = value;
			}
		}

		[DebuggerNonUserCode]
		public uint TitleId
		{
			get
			{
				return this.titleId_;
			}
			set
			{
				this.titleId_ = value;
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
			if (this.AvatarId != 0U)
			{
				output.WriteRawTag(16);
				output.WriteUInt32(this.AvatarId);
			}
			if (this.AvatarFrameId != 0U)
			{
				output.WriteRawTag(24);
				output.WriteUInt32(this.AvatarFrameId);
			}
			if (this.SkinHeaddressId != 0U)
			{
				output.WriteRawTag(32);
				output.WriteUInt32(this.SkinHeaddressId);
			}
			if (this.SkinBodyId != 0U)
			{
				output.WriteRawTag(40);
				output.WriteUInt32(this.SkinBodyId);
			}
			if (this.SkinAccessoryId != 0U)
			{
				output.WriteRawTag(48);
				output.WriteUInt32(this.SkinAccessoryId);
			}
			if (this.BackGround != 0U)
			{
				output.WriteRawTag(56);
				output.WriteUInt32(this.BackGround);
			}
			if (this.TitleId != 0U)
			{
				output.WriteRawTag(64);
				output.WriteUInt32(this.TitleId);
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
			if (this.AvatarId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.AvatarId);
			}
			if (this.AvatarFrameId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.AvatarFrameId);
			}
			if (this.SkinHeaddressId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SkinHeaddressId);
			}
			if (this.SkinBodyId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SkinBodyId);
			}
			if (this.SkinAccessoryId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.SkinAccessoryId);
			}
			if (this.BackGround != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.BackGround);
			}
			if (this.TitleId != 0U)
			{
				num += 1 + CodedOutputStream.ComputeUInt32Size(this.TitleId);
			}
			return num;
		}

		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 32U)
				{
					if (num <= 16U)
					{
						if (num == 10U)
						{
							if (this.commonParams_ == null)
							{
								this.commonParams_ = new CommonParams();
							}
							input.ReadMessage(this.commonParams_);
							continue;
						}
						if (num == 16U)
						{
							this.AvatarId = input.ReadUInt32();
							continue;
						}
					}
					else
					{
						if (num == 24U)
						{
							this.AvatarFrameId = input.ReadUInt32();
							continue;
						}
						if (num == 32U)
						{
							this.SkinHeaddressId = input.ReadUInt32();
							continue;
						}
					}
				}
				else if (num <= 48U)
				{
					if (num == 40U)
					{
						this.SkinBodyId = input.ReadUInt32();
						continue;
					}
					if (num == 48U)
					{
						this.SkinAccessoryId = input.ReadUInt32();
						continue;
					}
				}
				else
				{
					if (num == 56U)
					{
						this.BackGround = input.ReadUInt32();
						continue;
					}
					if (num == 64U)
					{
						this.TitleId = input.ReadUInt32();
						continue;
					}
				}
				input.SkipLastField();
			}
		}

		private static readonly MessageParser<UpdateUserAvatarRequest> _parser = new MessageParser<UpdateUserAvatarRequest>(() => new UpdateUserAvatarRequest());

		public const int CommonParamsFieldNumber = 1;

		private CommonParams commonParams_;

		public const int AvatarIdFieldNumber = 2;

		private uint avatarId_;

		public const int AvatarFrameIdFieldNumber = 3;

		private uint avatarFrameId_;

		public const int SkinHeaddressIdFieldNumber = 4;

		private uint skinHeaddressId_;

		public const int SkinBodyIdFieldNumber = 5;

		private uint skinBodyId_;

		public const int SkinAccessoryIdFieldNumber = 6;

		private uint skinAccessoryId_;

		public const int BackGroundFieldNumber = 7;

		private uint backGround_;

		public const int TitleIdFieldNumber = 8;

		private uint titleId_;
	}
}
