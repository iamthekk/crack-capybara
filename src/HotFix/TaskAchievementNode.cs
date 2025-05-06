using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Task;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class TaskAchievementNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_item.onClick = new Action<UIItem, PropData, object>(DxxTools.UI.OnItemClick);
			this.m_item.Init();
			this.m_receiveBt.onClick.AddListener(new UnityAction(this.OnClickReceiveBt));
		}

		protected override void OnDeInit()
		{
			this.m_receiveBt.onClick.RemoveListener(new UnityAction(this.OnClickReceiveBt));
			this.m_item.DeInit();
			this.m_data = null;
		}

		private void OnClickReceiveBt()
		{
			if (this.m_data == null)
			{
				return;
			}
			if (!this.m_data.IsCompleteAndNoAward())
			{
				return;
			}
			int id = this.m_data.ID;
			NetworkUtils.Task.DoTaskRewardAchieveRequest(this.m_data.ID, delegate(bool isOK, TaskRewardAchieveResponse resp)
			{
				DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, delegate
				{
					EventArgTaskViewReceiveAchievementRewardTask instance = Singleton<EventArgTaskViewReceiveAchievementRewardTask>.Instance;
					instance.SetData(id, (int)((resp.UpdateTaskDto != null) ? resp.UpdateTaskDto.Id : 0U));
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_TaskViewModule_ReceiveAchievementRewardTask, instance);
				}, true);
			});
		}

		public void OnRefresh(TaskDataModule.AchievementData data)
		{
			this.m_data = data;
			if (data == null)
			{
				return;
			}
			List<ItemData> list = data.Reward.ToItemDataList();
			if (list.Count != 1)
			{
				HLog.LogError(string.Format("Task_Achievement Reward not one item ,id = {0}", data.ID));
				return;
			}
			this.m_root.localScale = Vector3.one;
			ItemData itemData = list[0];
			this.m_item.SetData(itemData.ToPropData());
			this.m_item.OnRefresh();
			this.m_titleTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(data.AchievementDescribe, new object[] { data.GetAchievementDescribeParameter() });
			this.m_progressTxt.text = data.GetProgressInfo();
			this.m_slider.value = (float)data.AchievementCompleteCount * 1f / (float)data.AchievementNeedCount;
			if (!data.IsComplete)
			{
				this.m_receiveBt.gameObject.SetActive(false);
				this.m_receivedObj.SetActive(false);
				this.m_unCompleteObj.SetActive(true);
			}
			else
			{
				this.m_unCompleteObj.SetActive(false);
				if (!data.IsReceive)
				{
					this.m_receiveBt.gameObject.SetActive(true);
					this.m_receivedObj.SetActive(false);
				}
				else
				{
					this.m_receiveBt.gameObject.SetActive(false);
					this.m_receivedObj.SetActive(true);
				}
			}
			if (data.IsReceive)
			{
				this.m_item.SetEnableButton(false);
				this.m_goMask.SetActive(true);
				return;
			}
			this.m_item.SetEnableButton(true);
			this.m_goMask.SetActive(false);
		}

		public void PlayScale(Vector3 from, Vector3 to, TweenCallback complete)
		{
			if (this.m_root == null)
			{
				return;
			}
			this.m_root.localScale = from;
			Tweener tweener = ShortcutExtensions.DOScale(this.m_root, to, 0.2f);
			if (complete != null)
			{
				TweenSettingsExtensions.OnComplete<Tweener>(tweener, complete);
			}
		}

		public RectTransform m_root;

		public UIItem m_item;

		public CustomText m_titleTxt;

		public Slider m_slider;

		public CustomText m_progressTxt;

		public CustomButton m_receiveBt;

		public GameObject m_receivedObj;

		public GameObject m_unCompleteObj;

		public GameObject m_goMask;

		public TaskDataModule.AchievementData m_data;
	}
}
