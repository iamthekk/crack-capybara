using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.Platfrom;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIGuildBossKillCtrl : UIGuildBossBaseCtrl
	{
		public override UIGuildBossTag BossTag
		{
			get
			{
				return UIGuildBossTag.Kill;
			}
		}

		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.itemObj.SetActiveSafe(false);
			this.buttonGetReward.onClick.AddListener(new UnityAction(this.OnClickGetReward));
			this.CreateBox();
		}

		protected override void GuildUI_OnUnInit()
		{
			base.GuildUI_OnUnInit();
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

		protected override void GuildUI_OnShow()
		{
			base.gameObject.SetActive(true);
			base.SDK.Event.RegisterEvent(202, new GuildHandlerEvent(this.OnRefresh));
			this.Refresh();
		}

		protected override void GuildUI_OnClose()
		{
			base.gameObject.SetActive(false);
			base.SDK.Event.UnRegisterEvent(202, new GuildHandlerEvent(this.OnRefresh));
		}

		private void Refresh()
		{
			GuildBossInfo guildBoss = base.SDK.GuildActivity.GuildBoss;
			this.textGuildDamage.text = GuildProxy.Language.FormatNumber(guildBoss.TotalGuildDamage);
			this.textMyDamage.text = GuildProxy.Language.FormatNumber(guildBoss.TotalPersonalDamage);
			GuildBossKillBox guildBossKillBox = guildBoss.GetCurrentBox();
			this.currentBox = guildBossKillBox;
			this.isLastBox = guildBoss.IsLastBox(guildBossKillBox);
			if (guildBossKillBox.boxState == GuildBossKillBox.GuildBossKillBoxState.CanGetReward)
			{
				this.buttonGetReward.gameObject.SetActiveSafe(true);
				this.textNoReward.text = "";
			}
			else
			{
				this.buttonGetReward.gameObject.SetActiveSafe(false);
				if (this.isLastBox)
				{
					this.textNoReward.text = GuildProxy.Language.GetInfoByID("400256");
				}
				else
				{
					this.textNoReward.text = GuildProxy.Language.GetInfoByID("400238");
				}
			}
			List<GuildBossKillBox> showBoxList = guildBoss.GetShowBoxList();
			for (int i = 0; i < this.itemList.Count; i++)
			{
				if (i < showBoxList.Count)
				{
					this.itemList[i].gameObject.SetActiveSafe(true);
					this.itemList[i].Refresh(showBoxList[i]);
					this.itemList[i].ResetAni();
					if (showBoxList[i].Equals(this.currentBox))
					{
						if (this.isShowAni)
						{
							this.isShowAni = false;
							this.itemList[i].ShowAni();
						}
						this.rewardMoveRT.anchoredPosition = new Vector2((float)i * this.MoveParam, 0f);
					}
				}
				else
				{
					this.itemList[i].gameObject.SetActiveSafe(false);
				}
			}
		}

		private async void CreateBox()
		{
			for (int i = 0; i < 7; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.itemObj);
				gameObject.transform.SetParentNormal(this.rewardLayout.gameObject, false);
				UIGuildBossKillRewardItem component = gameObject.GetComponent<UIGuildBossKillRewardItem>();
				component.Init();
				this.itemList.Add(component);
				await TaskExpand.Delay(20);
			}
		}

		private void OnRefresh(int type, GuildBaseEvent eventArgs)
		{
			this.Refresh();
		}

		private void OnCloseRewardUI(object sender, int type, BaseEventArgs eventArgs)
		{
			HLog.LogError("OnCloseRewardUI!!!!!");
		}

		private void OnClickGetReward()
		{
			if (this.currentBox != null)
			{
				GuildNetUtil.Guild.DoRequest_GetGuildBossBoxReward(this.currentBox.BoxID, delegate(bool result, GuildBossBoxRewardResponse resp)
				{
					if (resp != null && resp.CommonData != null)
					{
						GuildProxy.UI.OpenUICommonReward(resp.CommonData.Reward, null);
					}
				});
			}
		}

		public CustomText textGuildDamage;

		public CustomText textMyDamage;

		public RectTransform rewardMoveRT;

		public CustomButton buttonGetReward;

		public HorizontalLayoutGroup rewardLayout;

		public GameObject itemObj;

		public CustomText textNoReward;

		private List<UIGuildBossKillRewardItem> itemList = new List<UIGuildBossKillRewardItem>();

		private GuildBossKillBox currentBox;

		private bool isLastBox;

		private float MoveParam = -400f;
	}
}
