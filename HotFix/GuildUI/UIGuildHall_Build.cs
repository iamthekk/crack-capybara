using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.UI;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIGuildHall_Build : UIGuildHall_Base
	{
		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.itemObj.SetActiveSafe(false);
			this.buttonBuild.onClick.AddListener(new UnityAction(this.OnClickBuild));
			this.buttonBuildFree.onClick.AddListener(new UnityAction(this.OnClickBuild));
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.buttonBuild != null)
			{
				this.buttonBuild.onClick.RemoveListener(new UnityAction(this.OnClickBuild));
			}
			if (this.buttonBuildFree != null)
			{
				this.buttonBuildFree.onClick.RemoveListener(new UnityAction(this.OnClickBuild));
			}
			base.GuildUI_OnUnInit();
		}

		protected override void GuildUI_OnShow()
		{
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.RefreshBuildTime));
			DelayCall.Instance.CallLoop(1000, new DelayCall.CallAction(this.RefreshBuildTime));
		}

		protected override void GuildUI_OnClose()
		{
			DelayCall.Instance.CallLoop(1000, new DelayCall.CallAction(this.RefreshBuildTime));
		}

		public override void OnRefreshUI()
		{
			if (GuildSDKManager.Instance.GuildInfo.GuildData == null)
			{
				return;
			}
			this.RefreshBuildReward();
		}

		private void RefreshBuildReward()
		{
			GuildSignData guildSignData = base.SDK.GuildInfo.GuildSignData;
			this.textBuild.text = GuildProxy.Language.GetInfoByID2("400144", guildSignData.SignCount, guildSignData.MaxSignCount);
			bool flag = this.IsFreeBuild();
			this.buttonBuildFree.gameObject.SetActiveSafe(flag);
			this.buttonBuild.gameObject.SetActiveSafe(!flag);
			this.buildButtonObj.gameObject.SetActiveSafe(guildSignData.SignCount < guildSignData.MaxSignCount);
			this.buildFinishObj.SetActiveSafe(guildSignData.SignCount >= guildSignData.MaxSignCount);
			this.RefreshBuildTime();
			List<GuildItemData> rewards = guildSignData.Rewards;
			for (int i = 0; i < this.buildRewardItemList.Count; i++)
			{
				this.buildRewardItemList[i].gameObject.SetActiveSafe(false);
			}
			for (int j = 0; j < rewards.Count; j++)
			{
				GuildItemData guildItemData = rewards[j];
				UIGuildItem uiguildItem;
				if (j < this.buildRewardItemList.Count)
				{
					uiguildItem = this.buildRewardItemList[j];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.itemObj);
					gameObject.transform.SetParentNormal(this.buildRewardLayout.gameObject, false);
					uiguildItem = gameObject.GetComponent<UIGuildItem>();
					uiguildItem.Init();
					uiguildItem.OnClick = new Action<UIGuildItem>(this.OnClickItem);
					this.buildRewardItemList.Add(uiguildItem);
				}
				uiguildItem.SetItem(guildItemData);
				uiguildItem.RefreshUI();
				uiguildItem.SetActive(true);
			}
		}

		private void RefreshBuildTime()
		{
			long num = (long)(base.SDK.GetCustomRefreshTime() - (ulong)GuildProxy.Net.ServerTime());
			num = ((num > 0L) ? num : 0L);
			string time = Singleton<LanguageManager>.Instance.GetTime(num);
			this.textBuildRefreshTime.text = GuildProxy.Language.GetInfoByID1("400185", time);
		}

		private bool IsFreeBuild()
		{
			GuildSignData guildSignData = base.SDK.GuildInfo.GuildSignData;
			return guildSignData.SignCount < guildSignData.MaxSignCount && guildSignData.SignCost.count == 0;
		}

		private void OnClickBuild()
		{
			if (this.CheckBuildEnable())
			{
				if (this.IsFreeBuild())
				{
					this.OnSendBuildMsg();
					return;
				}
				GuildSignData guildSignData = base.SDK.GuildInfo.GuildSignData;
				string infoByID = GuildProxy.Language.GetInfoByID("400202");
				string infoByID_LogError = GuildProxy.Language.GetInfoByID_LogError(400204);
				GuildProxy.UI.ShowBuyPop(infoByID, infoByID_LogError, guildSignData.SignCost, new Action(this.OnSendBuildMsg));
			}
		}

		private bool CheckBuildEnable()
		{
			GuildSignData guildSignData = base.SDK.GuildInfo.GuildSignData;
			if (guildSignData.SignCount >= guildSignData.MaxSignCount)
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID_LogError(400189));
				return false;
			}
			return true;
		}

		private void OnSendBuildMsg()
		{
			GuildNetUtil.Guild.DoRequest_GuildSign(delegate(bool _result, GuildSignInResponse resp)
			{
				if (_result)
				{
					GuildProxy.UI.OpenUICommonReward(resp.CommonData.Reward, null);
					base.RefreshParent();
				}
			});
		}

		private void OnClickItem(UIGuildItem item)
		{
			GuildProxy.UI.OpenUIItemPop(item.gameObject, item.GuildItemData);
		}

		public CustomText textBuild;

		public GameObject itemObj;

		public HorizontalLayoutGroup buildRewardLayout;

		public CustomButton buttonBuild;

		public GameObject buildButtonObj;

		public CustomButton buttonBuildFree;

		public CustomText textBuildRefreshTime;

		public GameObject buildFinishObj;

		private List<UIGuildItem> buildRewardItemList = new List<UIGuildItem>();
	}
}
