using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class GuildLevelUpViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			base.OnViewCreate();
			this.popCommon.OnClick = new Action<int>(this.OnPopClick);
			this.aniListen = AnimatorListen.Get(this.ani.gameObject);
			this.aniListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimationListen));
		}

		protected override void OnViewOpen(object data)
		{
			base.OnViewOpen(data);
			this.levelInfoIObj.SetActive(false);
			this.ResetAni();
			base.SDK.Event.DispatchNow(21, null);
			int guildLevel = base.SDK.GuildInfo.GuildData.GuildLevel;
			this.textLastLv.text = string.Format("Lv{0}", guildLevel - 1);
			this.textCurrentLv.text = string.Format("Lv{0}", guildLevel);
			this.TitleLayout.RefreshUI(GuildProxy.Language.GetInfoByID1("400155", guildLevel));
			this.ShowLevelInfo();
			DelayCall.Instance.CallOnce(200, new DelayCall.CallAction(this.OnShowAni));
		}

		protected override void OnViewClose()
		{
			base.OnViewClose();
		}

		protected override void OnViewDelete()
		{
			base.OnViewDelete();
			if (this.aniListen != null)
			{
				this.aniListen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimationListen));
			}
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].DeInit();
				Object.Destroy(this.itemList[i].gameObject);
			}
			this.itemList.Clear();
		}

		private void ShowLevelInfo()
		{
			List<GuildShareDataEx.LevelChangeData> levelDetailList = GuildUITool.GetLevelDetailList(base.SDK.GuildInfo.GuildData.GuildLevel);
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].gameObject.SetActiveSafe(false);
			}
			for (int j = 0; j < levelDetailList.Count; j++)
			{
				UIGuildLevelDetailItem uiguildLevelDetailItem;
				if (j < this.itemList.Count)
				{
					uiguildLevelDetailItem = this.itemList[j];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.levelInfoIObj);
					gameObject.transform.SetParentNormal(this.infoLayout.gameObject, false);
					uiguildLevelDetailItem = gameObject.GetComponent<UIGuildLevelDetailItem>();
					uiguildLevelDetailItem.Init();
					this.itemList.Add(uiguildLevelDetailItem);
				}
				uiguildLevelDetailItem.gameObject.SetActiveSafe(true);
				uiguildLevelDetailItem.Refresh(levelDetailList[j]);
				uiguildLevelDetailItem.ResetAni();
			}
		}

		private void ResetAni()
		{
			this.ani.Play("UIGuildLevelUp_Show");
			this.ani.Update(0f);
			this.ani.enabled = false;
		}

		private void OnShowAni()
		{
			this.ResetAni();
			this.ani.enabled = true;
		}

		private void ChildPlayAni()
		{
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].ShowAni(i);
			}
		}

		private void OnAnimationListen(GameObject obj, string eventParameter)
		{
			if (!string.IsNullOrEmpty(eventParameter) && eventParameter.Equals("End"))
			{
				this.ChildPlayAni();
			}
		}

		private void ClickCloseThis()
		{
			GuildProxy.UI.CloseUIGuildLevelUp();
		}

		private void OnPopClick(int obj)
		{
			this.ClickCloseThis();
		}

		public UIGuildPopCommon popCommon;

		public GuildLevelUpTitleLayoutUI TitleLayout;

		public CustomText textLastLv;

		public CustomText textCurrentLv;

		public VerticalLayoutGroup infoLayout;

		public GameObject levelInfoIObj;

		public Animator ani;

		public AnimatorListen aniListen;

		private List<UIGuildLevelDetailItem> itemList = new List<UIGuildLevelDetailItem>();
	}
}
