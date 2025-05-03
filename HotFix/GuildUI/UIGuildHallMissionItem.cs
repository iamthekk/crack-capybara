using System;
using System.Collections.Generic;
using DG.Tweening;
using Dxx.Guild;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIGuildHallMissionItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		public RectTransform missionRTF
		{
			get
			{
				return base.rectTransform;
			}
		}

		protected override void GuildUI_OnInit()
		{
			this.buttonGo.onClick.AddListener(new UnityAction(this.OnJumpToFunction));
			this.buttonGetReward.onClick.AddListener(new UnityAction(this.OnGetRewards));
			this.buttonRefresh.onClick.AddListener(new UnityAction(this.OnRefreshMission));
			this.itemObj.SetActiveSafe(false);
			this.maskObj.SetActiveSafe(false);
			this.itemWeight = this.itemObj.GetComponent<RectTransform>().sizeDelta.x;
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.buttonGo != null)
			{
				this.buttonGo.onClick.RemoveListener(new UnityAction(this.OnJumpToFunction));
			}
			if (this.buttonGetReward != null)
			{
				this.buttonGetReward.onClick.RemoveListener(new UnityAction(this.OnGetRewards));
			}
			if (this.buttonRefresh != null)
			{
				this.buttonRefresh.onClick.RemoveListener(new UnityAction(this.OnRefreshMission));
			}
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].DeInit();
				Object.Destroy(this.itemList[i].gameObject);
			}
			this.itemList.Clear();
		}

		public void Refresh(GuildTaskData data)
		{
			if (data == null)
			{
				return;
			}
			this.taskData = data;
			if (GuildProxy.Table.GetGuildTaskTable(data.TaskId) == null)
			{
				return;
			}
			this.textMission.text = GuildProxy.Language.GetInfoByID(data.ContentLanguageID);
			this.textProgress.text = string.Format("{0}/{1}", data.CurrentRate, data.NeedRate);
			float num = (float)data.CurrentRate / (float)data.NeedRate;
			num = ((num > 1f) ? 1f : num);
			this.sliderProgress.value = num;
			this.buttonGoObj.SetActiveSafe(!data.IsFinish);
			this.buttonGetReward.gameObject.SetActiveSafe(data.taskState == GuildTaskData.GuildTaskState.CanGetReward);
			this.maskObj.SetActiveSafe(data.taskState == GuildTaskData.GuildTaskState.AllFinish);
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].gameObject.SetActiveSafe(false);
			}
			for (int j = 0; j < data.Rewards.Count; j++)
			{
				GuildItemData guildItemData = data.Rewards[j];
				UIGuildItem uiguildItem;
				if (j < this.itemList.Count)
				{
					uiguildItem = this.itemList[j];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.itemObj);
					gameObject.transform.SetParentNormal(this.rewardLayout.gameObject, false);
					uiguildItem = gameObject.GetComponent<UIGuildItem>();
					uiguildItem.Init();
					uiguildItem.OnClick = new Action<UIGuildItem>(this.OnClickItem);
					this.itemList.Add(uiguildItem);
				}
				uiguildItem.SetItem(guildItemData);
				uiguildItem.RefreshUI();
				uiguildItem.SetActive(true);
			}
			bool flag = !data.IsFinish;
			if (flag)
			{
				Guild_guildConst guildConstTable = GuildProxy.Table.GetGuildConstTable(118);
				if (guildConstTable != null)
				{
					flag = guildConstTable.TypeInt == 1;
				}
			}
			this.buttonRefresh.gameObject.SetActiveSafe(flag);
		}

		private void OnSendRefreshMsg()
		{
			GuildNetUtil.Guild.DoRequest_GuildTaskRefresh(this.taskData.TaskId, delegate(bool result, GuildTaskRefreshResponse resp)
			{
				if (result)
				{
					TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DOLocalMoveX(this.aniTrans, -1500f, 0.2f, false), delegate
					{
						this.Refresh(resp.GuildTask.ToGuidTaskData());
						TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DOLocalMoveX(this.aniTrans, 0f, 0.2f, false), delegate
						{
							this.aniTrans.anchoredPosition = Vector2.zero;
						});
					});
				}
			});
		}

		private void OnClickItem(UIGuildItem item)
		{
			GuildProxy.UI.OpenUIItemPop(item.gameObject, item.GuildItemData);
		}

		public void SetMissionPosition(Vector2 pos)
		{
			this.missionRTF.anchoredPosition = pos;
		}

		private void OnJumpToFunction()
		{
			GuildProxy.UI.ShowTips("点击前往按钮！");
		}

		private void OnGetRewards()
		{
			if (this.taskData.taskState == GuildTaskData.GuildTaskState.CanGetReward)
			{
				GuildNetUtil.Guild.DoRequest_GetGuildTaskReward(this.taskData.TaskId, delegate(bool result, GuildTaskRewardResponse resp)
				{
					if (result)
					{
						GuildProxy.UI.OpenUICommonReward(resp.CommonData.Reward, null);
						GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_Guild_RefreshHall);
					}
				});
			}
		}

		private void OnRefreshMission()
		{
			if (base.SDK.GuildTask.RefreshData.TaskRefreshCount > 0)
			{
				this.OnSendRefreshMsg();
				return;
			}
			GuildProxy.UI.ShowBuyPop(GuildProxy.Language.GetInfoByID("400260"), GuildProxy.Language.GetInfoByID("400261"), base.SDK.GuildTask.RefreshData.RefreshCost, new Action(this.OnSendRefreshMsg));
		}

		public CustomText textMission;

		public CustomText textProgress;

		public HorizontalLayoutGroup rewardLayout;

		public GameObject buttonGoObj;

		public CustomButton buttonGo;

		public GameObject maskObj;

		public GameObject itemObj;

		public CustomButton buttonGetReward;

		public CustomButton buttonRefresh;

		public Slider sliderProgress;

		public CanvasGroup canvasGroup;

		public RectTransform aniTrans;

		private List<UIGuildItem> itemList = new List<UIGuildItem>();

		private GuildTaskData taskData;

		private float itemWeight;
	}
}
