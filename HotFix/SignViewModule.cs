using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine.Events;

namespace HotFix
{
	public class SignViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.maskBtn.m_onClick = new Action(this.CloseThisView);
			this.m_closeBt.onClick.AddListener(new UnityAction(this.CloseThisView));
			for (int i = 0; i < this.signinList.Count; i++)
			{
				this.signinList[i].Init();
			}
			this.signDataModule = GameApp.Data.GetDataModule(DataName.SignDataModule);
		}

		public override void OnOpen(object data)
		{
			this.RefreshUI();
			this.signDataModule.TryRefreshData();
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.m_closeBt.onClick.RemoveAllListeners();
			for (int i = 0; i < this.signinList.Count; i++)
			{
				this.signinList[i].DeInit();
			}
			this.signinList.Clear();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_SignIn_DataList, new HandlerEvent(this.RefreshUIList));
			manager.RegisterEvent(LocalMessageName.CC_CommonActivity, new HandlerEvent(this.RefreshUIList));
			manager.RegisterEvent(LocalMessageName.CC_SignIn_DataUpdate, new HandlerEvent(this.RefreshUIList));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_SignIn_DataList, new HandlerEvent(this.RefreshUIList));
			manager.UnRegisterEvent(LocalMessageName.CC_CommonActivity, new HandlerEvent(this.RefreshUIList));
			manager.UnRegisterEvent(LocalMessageName.CC_SignIn_DataUpdate, new HandlerEvent(this.RefreshUIList));
		}

		private void RefreshUIList(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshUI();
		}

		public void RefreshUI()
		{
			List<SignInItemInfo> signInItemInfos = this.signDataModule.signInItemInfos;
			for (int i = 0; i < this.signinList.Count; i++)
			{
				if (i < signInItemInfos.Count)
				{
					this.signinList[i].SetSigninData(signInItemInfos[i], (i == this.signinList.Count - 1) ? SignInAwardType.Multi : SignInAwardType.One);
				}
			}
		}

		private void CloseThisView()
		{
			GameApp.View.CloseView(ViewName.SignViewModule, null);
		}

		public CustomButton maskBtn;

		public CustomButton m_closeBt;

		public List<SignItemCrtl> signinList = new List<SignItemCrtl>();

		private SignDataModule signDataModule;
	}
}
