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
	public class UIGuildBossChallengeItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.aniRt = this.canvasGroup.GetComponent<RectTransform>();
			this.itemObj.SetActiveSafe(false);
			this.grayObj.SetActive(false);
			this.finishObj.SetActiveSafe(false);
			this.buttonGetReward.onClick.AddListener(new UnityAction(this.OnClickGetReward));
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.buttonGetReward != null)
			{
				this.buttonGetReward.onClick.AddListener(new UnityAction(this.OnClickGetReward));
			}
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].DeInit();
			}
			this.itemList.Clear();
		}

		public void Refresh(GuildBossTask bossTask)
		{
			throw new NotImplementedException("公会BOSS中的 任务功能已被移除，不再使用");
		}

		public void ClearAni()
		{
			this.canvasGroup.alpha = 1f;
			this.aniRt.anchoredPosition = Vector2.zero;
			if (this.animator != null)
			{
				this.animator.enabled = false;
			}
		}

		public void ShowAni(int index)
		{
			if (this.animator == null)
			{
				return;
			}
			this.animator.enabled = false;
			this.canvasGroup.alpha = 0f;
			this.OnShowAni();
		}

		private void OnShowAni()
		{
			if (this.animator == null)
			{
				return;
			}
			this.animator.enabled = true;
			this.animator.Play("UIGuildBossChallengeItem_Show");
		}

		private void OnClickItem(UIGuildItem item)
		{
			GuildProxy.UI.OpenUIItemPop(item.gameObject, item.GuildItemData);
		}

		private void OnClickGetReward()
		{
			GuildNetUtil.Guild.DoRequest_GetGuildBossTaskReward(this.guildBossTask.TaskID, delegate(bool result, GuildBossTaskRewardResponse resp)
			{
				if (result)
				{
					if (resp != null && resp.CommonData != null)
					{
						GuildProxy.UI.OpenUICommonReward(resp.CommonData.Reward, null);
					}
					GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_GuildBoss_Refresh);
				}
			});
		}

		public CustomText textName;

		public CustomText textProgress;

		public Slider sliderProgress;

		public HorizontalLayoutGroup rewardLayout;

		public GameObject itemObj;

		public RectTransform imageRewardBg;

		public CustomButton buttonGetReward;

		public GameObject finishObj;

		public GameObject grayObj;

		public Animator animator;

		public CanvasGroup canvasGroup;

		private List<UIGuildItem> itemList = new List<UIGuildItem>();

		private GuildBossTask guildBossTask;

		private RectTransform aniRt;
	}
}
