using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class TaskTaskNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_receiveBt.onClick.AddListener(new UnityAction(this.OnClickReciveBt));
			this.m_gotoBt.onClick.AddListener(new UnityAction(this.OnClickGotoBt));
		}

		protected override void OnDeInit()
		{
			this.m_receiveBt.onClick.RemoveListener(new UnityAction(this.OnClickReciveBt));
			this.m_gotoBt.onClick.RemoveListener(new UnityAction(this.OnClickGotoBt));
			this.m_dailyData = null;
		}

		private void OnClickReciveBt()
		{
			if (!this.m_dailyData.IsCompleteAndNoAward())
			{
				return;
			}
			NetworkUtils.Task.DoTaskRewardDailyRequest(this.m_dailyData.ID, null);
		}

		private async void OnClickGotoBt()
		{
			if (this.m_dailyData.JumpViewID > 0)
			{
				ViewJumpType viewJumpType = (ViewJumpType)this.m_dailyData.JumpViewID;
				if (Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(viewJumpType, null, true))
				{
					if (viewJumpType == ViewJumpType.Tower)
					{
						if (!GameApp.Data.GetDataModule(DataName.TowerDataModule).IsAllFinish)
						{
							await Singleton<ViewJumpCtrl>.Instance.JumpTo(viewJumpType, null);
						}
						else
						{
							viewJumpType = ViewJumpType.RogueDungeon;
							await Singleton<ViewJumpCtrl>.Instance.JumpTo(viewJumpType, null);
						}
					}
					else
					{
						await Singleton<ViewJumpCtrl>.Instance.JumpTo(viewJumpType, null);
					}
					GameApp.View.CloseView(ViewName.TaskViewModule, null);
				}
			}
		}

		public void OnRefresh(TaskDataModule.TaskDailyData data)
		{
			this.m_dailyData = data;
			if (this.m_dailyData == null)
			{
				return;
			}
			this.m_activeTxt.text = string.Format("+{0}", this.m_dailyData.DailyActiveReward);
			this.m_titleTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.m_dailyData.DailyDescribe, new object[] { this.m_dailyData.DailyNeedCount });
			this.m_progressTxt.text = string.Format("{0}/{1}", this.m_dailyData.DailyCompleteCount, this.m_dailyData.DailyNeedCount);
			this.m_slider.value = (float)this.m_dailyData.DailyCompleteCount * 1f / (float)this.m_dailyData.DailyNeedCount;
			if (!this.m_dailyData.IsComplete)
			{
				this.m_receiveBt.gameObject.SetActive(false);
				this.m_receivedObj.SetActive(false);
				if (this.m_dailyData.JumpViewID == 0)
				{
					this.m_gotoBt.gameObject.SetActive(false);
					this.m_unCompleteObj.SetActive(true);
				}
				else
				{
					this.m_gotoBt.gameObject.SetActive(true);
					this.m_unCompleteObj.SetActive(false);
				}
			}
			else
			{
				this.m_gotoBt.gameObject.SetActive(false);
				this.m_unCompleteObj.SetActive(false);
				if (!this.m_dailyData.IsAward)
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
			if (data.IsAward)
			{
				this.m_goMask.SetActive(true);
				return;
			}
			this.m_goMask.SetActive(false);
		}

		public CustomText m_activeTxt;

		public CustomText m_titleTxt;

		public Slider m_slider;

		public CustomText m_progressTxt;

		public CustomButton m_receiveBt;

		public CustomButton m_gotoBt;

		public GameObject m_receivedObj;

		public GameObject m_unCompleteObj;

		public GameObject m_goMask;

		public TaskDataModule.TaskDailyData m_dailyData;
	}
}
