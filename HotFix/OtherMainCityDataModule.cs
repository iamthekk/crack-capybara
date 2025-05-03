using System;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Proto.Common;
using Proto.User;
using UnityEngine;

namespace HotFix
{
	public class OtherMainCityDataModule : IDataModule
	{
		public long TargetUserID
		{
			get
			{
				return this.m_targetUserID;
			}
		}

		public UserGetCityInfoResponse Data
		{
			get
			{
				return this.m_response;
			}
		}

		public int GetName()
		{
			return 124;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_OtherMainCityData_SetData, new HandlerEvent(this.OnEventSetData));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Battle, new HandlerEvent(this.OnEventBattle));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Revolt, new HandlerEvent(this.OnEventRevolt));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Loot, new HandlerEvent(this.OnEventLoot));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Pardon, new HandlerEvent(this.OnEventPardon));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_OtherMainCityData_SetData, new HandlerEvent(this.OnEventSetData));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ConquerData_Battle, new HandlerEvent(this.OnEventBattle));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ConquerData_Revolt, new HandlerEvent(this.OnEventRevolt));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ConquerData_Loot, new HandlerEvent(this.OnEventLoot));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ConquerData_Pardon, new HandlerEvent(this.OnEventPardon));
		}

		public void Reset()
		{
		}

		private void OnEventSetData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsSetOtherMainCityData eventArgsSetOtherMainCityData = eventargs as EventArgsSetOtherMainCityData;
			if (eventArgsSetOtherMainCityData == null)
			{
				return;
			}
			this.m_targetUserID = eventArgsSetOtherMainCityData.m_targetUserID;
			this.m_response = eventArgsSetOtherMainCityData.m_response;
		}

		private void OnEventBattle(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsConquerTypeData eventArgsConquerTypeData = eventargs as EventArgsConquerTypeData;
			if (eventArgsConquerTypeData == null)
			{
				return;
			}
			if (this.Data == null)
			{
				return;
			}
			if (this.Data.UserId != eventArgsConquerTypeData.m_targetUserID)
			{
				return;
			}
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.Data.Extra = new LordDto();
			this.Data.Extra.LordUid = dataModule.userId;
			this.Data.Extra.LordAvatar = dataModule.Avatar;
			this.Data.Extra.LordAvatarFrame = dataModule.AvatarFrame;
			this.Data.Extra.LordNickName = dataModule.NickName;
		}

		private void OnEventRevolt(object sender, int type, BaseEventArgs eventargs)
		{
			if (!(eventargs is EventArgsConquerTypeData))
			{
				return;
			}
			if (this.Data == null)
			{
				return;
			}
			if (GameApp.Data.GetDataModule(DataName.LoginDataModule).userId == this.Data.UserId)
			{
				this.Data.Extra = null;
			}
		}

		private void OnEventLoot(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsConquerTypeData eventArgsConquerTypeData = eventargs as EventArgsConquerTypeData;
			if (eventArgsConquerTypeData == null)
			{
				return;
			}
			if (this.Data == null)
			{
				return;
			}
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			if (dataModule.userId == this.Data.UserId)
			{
				return;
			}
			if (this.Data.UserId == eventArgsConquerTypeData.m_targetUserID)
			{
				this.Data.Extra = new LordDto();
				this.Data.Extra.LordUid = dataModule.userId;
				this.Data.Extra.LordAvatar = dataModule.Avatar;
				this.Data.Extra.LordAvatarFrame = dataModule.AvatarFrame;
				this.Data.Extra.LordNickName = dataModule.NickName;
			}
		}

		private void OnEventPardon(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsConquerTypeData eventArgsConquerTypeData = eventargs as EventArgsConquerTypeData;
			if (eventArgsConquerTypeData == null)
			{
				return;
			}
			if (this.Data == null)
			{
				return;
			}
			if (GameApp.Data.GetDataModule(DataName.LoginDataModule).userId == this.Data.UserId)
			{
				return;
			}
			if (this.Data.UserId == eventArgsConquerTypeData.m_targetUserID)
			{
				this.Data.Extra = null;
			}
		}

		[SerializeField]
		private long m_targetUserID;

		[SerializeField]
		private UserGetCityInfoResponse m_response;
	}
}
