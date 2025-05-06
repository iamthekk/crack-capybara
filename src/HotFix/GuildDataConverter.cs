using System;
using Framework;
using Google.Protobuf;
using LocalModels.Model;
using Proto.Common;

namespace HotFix
{
	public class GuildDataConverter : Singleton<GuildDataConverter>
	{
		public CommonData GetDefaultCommonData()
		{
			CommonData commonData = new CommonData();
			commonData.UpdateUserCurrency = new UpdateUserCurrency();
			commonData.UpdateUserCurrency.IsChange = false;
			commonData.UpdateUserCurrency.UserCurrency = new UserCurrency();
			if (this.m_loginDataModule == null)
			{
				this.m_loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			}
			commonData.UpdateUserCurrency.UserCurrency = UserCurrency.Parser.ParseFrom(MessageExtensions.ToByteString(this.m_loginDataModule.userCurrency));
			commonData.UpdateUserLevel = new UpdateUserLevel();
			commonData.UpdateUserLevel.IsChange = false;
			commonData.UpdateUserLevel.UserLevel = UserLevel.Parser.ParseFrom(MessageExtensions.ToByteString(this.m_loginDataModule.userLevel));
			return commonData;
		}

		public CommonData GetCommonDataForJson(string rewardInfo)
		{
			if (string.IsNullOrEmpty(rewardInfo))
			{
				return null;
			}
			ByteString byteString = ByteString.FromBase64(rewardInfo);
			CommonData commonData = CommonData.Parser.ParseFrom(byteString);
			LoginDataModule.LogUserInfoUnlock(commonData.UserInfoDto);
			return commonData;
		}

		private LoginDataModule m_loginDataModule;

		private PropDataModule m_propDataModule;

		private Item_ItemModel m_itemModel;
	}
}
