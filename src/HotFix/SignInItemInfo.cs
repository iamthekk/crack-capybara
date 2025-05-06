using System;
using Framework;
using Proto.Common;

namespace HotFix
{
	[RuntimeDefaultSerializedProperty]
	public class SignInItemInfo
	{
		public bool isCanSignIn;

		public int SignInIndex;

		public RewardDtoListDto rewardDtoListDto;

		public bool isSigin;

		public bool isSpecial;
	}
}
