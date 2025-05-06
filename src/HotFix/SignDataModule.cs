using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using LocalModels.Bean;
using Proto.Common;
using Proto.SignIn;

namespace HotFix
{
	public class SignDataModule : IDataModule
	{
		public int GetName()
		{
			return 129;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_SignIn_DataList, new HandlerEvent(this.RefreshSignInListData));
			manager.RegisterEvent(LocalMessageName.CC_DayChange_Sign_DataPull, new HandlerEvent(this.OnEventDayChange));
			manager.RegisterEvent(LocalMessageName.CC_CommonActivity, new HandlerEvent(this.RefreshCommonActivity));
			manager.RegisterEvent(LocalMessageName.CC_SignIn_Day_Update, new HandlerEvent(this.OnRefreshSignInByDay));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_SignIn_DataList, new HandlerEvent(this.RefreshSignInListData));
			manager.UnRegisterEvent(LocalMessageName.CC_DayChange_Sign_DataPull, new HandlerEvent(this.OnEventDayChange));
			manager.UnRegisterEvent(LocalMessageName.CC_CommonActivity, new HandlerEvent(this.RefreshCommonActivity));
			manager.UnRegisterEvent(LocalMessageName.CC_SignIn_Day_Update, new HandlerEvent(this.OnRefreshSignInByDay));
		}

		public void Reset()
		{
		}

		private void UpdateSignInData(SignInData signInData, bool isLogin = false)
		{
			this.InitSignData(signInData, isLogin);
			RedPointController.Instance.ReCalc("Main.Sign", true);
			GameApp.Event.DispatchNow(this, 221, null);
		}

		private void RefreshSignInListData(object sender, int type, BaseEventArgs eventArgs)
		{
			EventSignDataList eventSignDataList = eventArgs as EventSignDataList;
			if (eventSignDataList == null)
			{
				return;
			}
			this.UpdateSignInData(eventSignDataList.signInData, false);
		}

		private void OnEventDayChange(object sender, int type, BaseEventArgs eventArgs)
		{
			this.TryRefreshData();
		}

		private void RefreshCommonActivity(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsActivityCommonData eventArgsActivityCommonData = eventArgs as EventArgsActivityCommonData;
			if (eventArgsActivityCommonData == null)
			{
				return;
			}
			this.UpdateSignInData(eventArgsActivityCommonData.Response.SignInData, true);
		}

		private void OnRefreshSignInByDay(object sender, int type, BaseEventArgs eventArgs)
		{
			this.isCanPopSignIn = true;
		}

		private void InitSignData(SignInData signInData, bool isLogin)
		{
			if (signInData == null)
			{
				return;
			}
			this.IsCanSignIn = signInData.IsCanSignIn;
			this.SignInTime = signInData.Timestamp;
			this.SignInIndex = signInData.Log;
			this.id = (int)signInData.ConfigId;
			this.rewardDtoList.Clear();
			this.rewardDtoList.AddRange(signInData.RewardDtoList);
			this.signInItemInfos.Clear();
			SignIn_SignIn elementById = GameApp.Table.GetManager().GetSignIn_SignInModelInstance().GetElementById(this.id);
			List<int> list = new List<int>();
			list = elementById.colour.GetListInt('|');
			for (int i = 0; i < this.rewardDtoList.Count; i++)
			{
				SignInItemInfo signInItemInfo = new SignInItemInfo();
				signInItemInfo.SignInIndex = i + 1;
				signInItemInfo.rewardDtoListDto = this.rewardDtoList[i];
				if ((long)i < (long)((ulong)this.SignInIndex))
				{
					signInItemInfo.isSigin = true;
				}
				if ((long)i == (long)((ulong)this.SignInIndex) && this.IsCanSignIn)
				{
					signInItemInfo.isCanSignIn = true;
				}
				if (list.Contains(i + 1))
				{
					signInItemInfo.isSpecial = true;
				}
				this.signInItemInfos.Add(signInItemInfo);
			}
		}

		public void OnCheckPopSignIn(bool isLogin)
		{
			MainViewModule viewModule = GameApp.View.GetViewModule(ViewName.MainViewModule);
			if (isLogin && this.isCanPopSignIn && this.IsCanSignIn && Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Activity_Sign, false) && viewModule != null && viewModule.GetCurrentPageEnum() == UIMainPageName.Battle)
			{
				this.isCanPopSignIn = false;
				GameApp.View.OpenView(ViewName.SignViewModule, null, 1, null, null);
			}
		}

		public int GetAfterDay(int day)
		{
			int num = 0;
			for (int i = 0; i < this.signInItemInfos.Count; i++)
			{
				SignInItemInfo signInItemInfo = this.signInItemInfos[i];
				if (!signInItemInfo.isCanSignIn && (long)signInItemInfo.SignInIndex == (long)((ulong)this.SignInIndex))
				{
					num = i + 1;
				}
				if (this.SignInIndex == 0U && signInItemInfo.SignInIndex == 1)
				{
					num = i + 1;
				}
			}
			return day - num;
		}

		public bool GetSignInItemInfo(uint index, out SignInItemInfo itemInfo)
		{
			itemInfo = this.signInItemInfos.Find((SignInItemInfo item) => (long)item.SignInIndex == (long)((ulong)index));
			return itemInfo != null;
		}

		public uint GetToday()
		{
			if (!this.IsCanSignIn)
			{
				return this.SignInIndex;
			}
			return this.SignInIndex + 1U;
		}

		public void TryRefreshData()
		{
			this.IsShouldRefresh();
		}

		public bool IsShouldRefresh()
		{
			return DxxTools.Time.ServerTimestamp > (long)this.SignInTime && !this.IsCanSignIn;
		}

		public int id = 1;

		public bool IsCanSignIn;

		public ulong SignInTime;

		public uint SignInIndex;

		public List<RewardDtoListDto> rewardDtoList = new List<RewardDtoListDto>();

		public List<SignInItemInfo> signInItemInfos = new List<SignInItemInfo>();

		private bool isCanPopSignIn = true;
	}
}
