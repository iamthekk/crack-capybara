using System;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Proto.Conquer;
using UnityEngine;

namespace HotFix
{
	public class ConquerDataModule : IDataModule
	{
		public long TargetUserID
		{
			get
			{
				return this.m_targetUserID;
			}
		}

		public string TargetNick
		{
			get
			{
				return this.m_targetNick;
			}
		}

		public ConquerListResponse Data
		{
			get
			{
				return this.m_response;
			}
		}

		public bool IsUser
		{
			get
			{
				return this.m_isUser;
			}
		}

		public int GetName()
		{
			return 126;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_ConquerData_SetData, new HandlerEvent(this.OnEventSetData));
			manager.RegisterEvent(LocalMessageName.CC_ConquerData_SetResponseData, new HandlerEvent(this.OnEventSetResponseData));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Battle, new HandlerEvent(this.OnEventBattle));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Revolt, new HandlerEvent(this.OnEventRevolt));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Loot, new HandlerEvent(this.OnEventLoot));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ConquerData_Pardon, new HandlerEvent(this.OnEventPardon));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_ConquerData_SetData, new HandlerEvent(this.OnEventSetData));
			manager.UnRegisterEvent(LocalMessageName.CC_ConquerData_SetResponseData, new HandlerEvent(this.OnEventSetResponseData));
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
			EventArgsSetConquerData eventArgsSetConquerData = eventargs as EventArgsSetConquerData;
			if (eventArgsSetConquerData == null)
			{
				return;
			}
			this.m_targetUserID = eventArgsSetConquerData.m_targetUserID;
			this.m_targetNick = eventArgsSetConquerData.m_targetNick;
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.m_isUser = dataModule.userId == this.m_targetUserID;
		}

		private void OnEventSetResponseData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsSetConquerResponseData eventArgsSetConquerResponseData = eventargs as EventArgsSetConquerResponseData;
			if (eventArgsSetConquerResponseData == null)
			{
				return;
			}
			if (eventArgsSetConquerResponseData.m_response != null)
			{
				this.m_response = eventArgsSetConquerResponseData.m_response;
			}
		}

		private void OnEventBattle(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsConquerTypeData eventArgsConquerTypeData = eventargs as EventArgsConquerTypeData;
			if (eventArgsConquerTypeData == null)
			{
				return;
			}
			if (this.Data.UserId != eventArgsConquerTypeData.m_targetUserID)
			{
				return;
			}
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			AddAttributeDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			this.Data.Lord = new ConquerUserDto();
			this.Data.Lord.UserId = dataModule.userId;
			this.Data.Lord.Avatar = dataModule.Avatar;
			this.Data.Lord.AvatarFrame = dataModule.AvatarFrame;
			this.Data.Lord.NickName = dataModule.NickName;
			this.Data.Lord.Coin = 0U;
			this.Data.Lord.Power = (ulong)((uint)dataModule2.Combat);
		}

		private void OnEventRevolt(object sender, int type, BaseEventArgs eventargs)
		{
			if (!(eventargs is EventArgsConquerTypeData))
			{
				return;
			}
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			if (dataModule.userId == this.Data.UserId)
			{
				this.Data.Lord = null;
				return;
			}
			ConquerUserDto conquerUserDto = null;
			for (int i = 0; i < this.Data.Slaves.Count; i++)
			{
				ConquerUserDto conquerUserDto2 = this.Data.Slaves[i];
				if (conquerUserDto2 != null && conquerUserDto2.UserId == dataModule.userId)
				{
					conquerUserDto = conquerUserDto2;
					break;
				}
			}
			if (conquerUserDto != null)
			{
				this.Data.Slaves.Remove(conquerUserDto);
			}
		}

		private void OnEventLoot(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsConquerTypeData eventArgsConquerTypeData = eventargs as EventArgsConquerTypeData;
			if (eventArgsConquerTypeData == null)
			{
				return;
			}
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			AddAttributeDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			if (dataModule.userId == this.Data.UserId)
			{
				return;
			}
			if (this.Data.UserId == eventArgsConquerTypeData.m_targetUserID)
			{
				this.Data.Lord = new ConquerUserDto();
				this.Data.Lord.UserId = dataModule.userId;
				this.Data.Lord.Avatar = dataModule.Avatar;
				this.Data.Lord.AvatarFrame = dataModule.AvatarFrame;
				this.Data.Lord.NickName = dataModule.NickName;
				this.Data.Lord.Coin = 0U;
				this.Data.Lord.Power = (ulong)((uint)dataModule2.Combat);
				return;
			}
			ConquerUserDto conquerUserDto = null;
			for (int i = 0; i < this.Data.Slaves.Count; i++)
			{
				ConquerUserDto conquerUserDto2 = this.Data.Slaves[i];
				if (conquerUserDto2 != null && conquerUserDto2.UserId == eventArgsConquerTypeData.m_targetUserID)
				{
					conquerUserDto = conquerUserDto2;
					break;
				}
			}
			if (conquerUserDto != null)
			{
				this.Data.Slaves.Remove(conquerUserDto);
			}
		}

		private void OnEventPardon(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsConquerTypeData eventArgsConquerTypeData = eventargs as EventArgsConquerTypeData;
			if (eventArgsConquerTypeData == null)
			{
				return;
			}
			if (GameApp.Data.GetDataModule(DataName.LoginDataModule).userId == this.Data.UserId)
			{
				ConquerUserDto conquerUserDto = null;
				for (int i = 0; i < this.Data.Slaves.Count; i++)
				{
					ConquerUserDto conquerUserDto2 = this.Data.Slaves[i];
					if (conquerUserDto2 != null && conquerUserDto2.UserId == eventArgsConquerTypeData.m_targetUserID)
					{
						conquerUserDto = conquerUserDto2;
						break;
					}
				}
				if (conquerUserDto != null)
				{
					this.Data.Slaves.Remove(conquerUserDto);
					return;
				}
			}
			else if (this.Data.UserId == eventArgsConquerTypeData.m_targetUserID)
			{
				this.Data.Lord = null;
			}
		}

		[SerializeField]
		private long m_targetUserID;

		[SerializeField]
		private string m_targetNick;

		[SerializeField]
		private ConquerListResponse m_response;

		[SerializeField]
		private bool m_isUser;
	}
}
