using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.EventSystem;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIGuildHallCtrl : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.mGuildHallList.Add(this.baseInfo);
			this.mGuildHallList.Add(this.notice);
			this.mGuildHallList.Add(this.build);
			this.mGuildHallList.Add(this.mission);
			for (int i = 0; i < this.mGuildHallList.Count; i++)
			{
				this.mGuildHallList[i].OnRefreshParent = new Action(this.Refresh);
				this.mGuildHallList[i].Init();
			}
			this.canvasList.Add(this.titleAni.GetComponent<CanvasGroup>());
			this.canvasList.Add(this.noticAni.GetComponent<CanvasGroup>());
			this.canvasList.Add(this.buildAni.GetComponent<CanvasGroup>());
			this.canvasList.Add(this.missionAni.GetComponent<CanvasGroup>());
			this.animatorList.AddRange(new List<Animator> { this.titleAni, this.noticAni, this.buildAni, this.missionAni });
			this.ResetAni();
			this.buttonRefresh.gameObject.SetActiveSafe(false);
		}

		protected override void GuildUI_OnUnInit()
		{
			for (int i = 0; i < this.mGuildHallList.Count; i++)
			{
				this.mGuildHallList[i].DeInit();
			}
			this.mGuildHallList.Clear();
			this.canvasList.Clear();
		}

		protected override void GuildUI_OnShow()
		{
			base.gameObject.SetActiveSafe(true);
			GuildProxy.GameEvent.AddReceiver(LocalMessageName.CC_Guild_RefreshHall, new HandlerEvent(this.OnRefresh));
			base.SDK.Event.RegisterEvent(23, new GuildHandlerEvent(this.OnGuildRefresh));
			base.SDK.Event.RegisterEvent(20, new GuildHandlerEvent(this.OnGuildRefresh));
			this.Refresh();
		}

		protected override void GuildUI_OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			for (int i = 0; i < this.mGuildHallList.Count; i++)
			{
				this.mGuildHallList[i].OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		protected override void GuildUI_OnClose()
		{
			base.gameObject.SetActiveSafe(false);
			GuildProxy.GameEvent.RemoveReceiver(LocalMessageName.CC_Guild_RefreshHall, new HandlerEvent(this.OnRefresh));
			base.SDK.Event.UnRegisterEvent(23, new GuildHandlerEvent(this.OnGuildRefresh));
			base.SDK.Event.UnRegisterEvent(20, new GuildHandlerEvent(this.OnGuildRefresh));
			this.ResetAni();
		}

		private void Refresh()
		{
			for (int i = 0; i < this.mGuildHallList.Count; i++)
			{
				this.mGuildHallList[i].OnRefreshUI();
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.hallLayoutRT);
		}

		public void ShowAni()
		{
			this.scroll.content.anchoredPosition = Vector2.zero;
			for (int i = 0; i < this.canvasList.Count; i++)
			{
				if (this.canvasList[i] != null)
				{
					this.canvasList[i].alpha = 0f;
				}
			}
			for (int j = 0; j < this.animatorList.Count; j++)
			{
				if (this.animatorList[j] != null)
				{
					DelayCall.Instance.CallOnce<Animator>(j * 50, new DelayCall.CallAction<Animator>(this.OnShowAni), new object[] { this.animatorList[j] });
				}
			}
		}

		public void ResetAni()
		{
			for (int i = 0; i < this.animatorList.Count; i++)
			{
				if (this.animatorList[i] != null)
				{
					this.animatorList[i].enabled = false;
				}
			}
			for (int j = 0; j < this.canvasList.Count; j++)
			{
				this.canvasList[j].alpha = 1f;
				this.canvasList[j].GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
			}
		}

		private void OnShowAni(Animator ani)
		{
			if (ani != null)
			{
				ani.enabled = true;
			}
		}

		private void OnRefresh(object sender, int type, BaseEventArgs eventArgs)
		{
			this.Refresh();
		}

		private void OnGuildRefresh(int type, GuildBaseEvent eventArgs)
		{
			if (base.SDK.GuildInfo.IsShowLevelUp)
			{
				GuildProxy.UI.OpenUIGuildLevelUp(null);
			}
			this.Refresh();
		}

		private List<UIGuildHall_Base> mGuildHallList = new List<UIGuildHall_Base>();

		public UIGuildHall_BaseInfo baseInfo;

		public UIGuildHall_Notice notice;

		public UIGuildHall_Build build;

		public UIGuildHall_Mission mission;

		public Animator titleAni;

		public Animator noticAni;

		public Animator buildAni;

		public Animator missionAni;

		public RectTransform hallLayoutRT;

		public CustomButton buttonRefresh;

		public ScrollRect scroll;

		private List<CanvasGroup> canvasList = new List<CanvasGroup>();

		private List<Animator> animatorList = new List<Animator>();
	}
}
