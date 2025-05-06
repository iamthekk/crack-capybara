using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Task;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class TaskTaskDailyActiveGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_taskDataModule = GameApp.Data.GetDataModule(DataName.TaskDataModule);
			for (int i = 0; i < this.m_items.Count; i++)
			{
				TaskTaskActiveItem taskTaskActiveItem = this.m_items[i];
				if (!(taskTaskActiveItem == null))
				{
					taskTaskActiveItem.Init();
					taskTaskActiveItem.OnClickCallback = new Action<TaskTaskActiveItem, UIItem, PropData, object>(this.OnTaskTaskActiveItemClick);
				}
			}
			this.m_receiveBt.onClick.AddListener(new UnityAction(this.OnClickReceiveBt));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			if (this.m_isPlayingDelay)
			{
				this.m_delayTime += deltaTime;
				if (this.m_delayTime >= this.m_delayMaxTime)
				{
					this.m_delayTime = this.m_delayMaxTime;
					this.m_isPlayingProgress = true;
					this.m_isPlayingDelay = false;
				}
			}
			if (this.m_isPlayingProgress)
			{
				this.m_timeProgress += deltaTime;
				if (this.m_timeProgress >= this.m_durationProgress)
				{
					this.m_timeProgress = this.m_durationProgress;
					this.m_isPlayingProgress = false;
				}
				this.OnRefreshProgress(this.m_timeProgress / this.m_durationProgress);
			}
		}

		protected override void OnDeInit()
		{
			this.m_receiveBt.onClick.RemoveListener(new UnityAction(this.OnClickReceiveBt));
			for (int i = 0; i < this.m_items.Count; i++)
			{
				TaskTaskActiveItem taskTaskActiveItem = this.m_items[i];
				if (!(taskTaskActiveItem == null))
				{
					taskTaskActiveItem.DeInit();
				}
			}
			this.m_taskDataModule = null;
		}

		public void OnRefreshUI()
		{
			for (int i = 0; i < this.m_taskDataModule.DailyTaskActiveDatas.Count; i++)
			{
				TaskDataModule.TaskActive taskActive = this.m_taskDataModule.DailyTaskActiveDatas[i];
				if (taskActive != null)
				{
					TaskTaskActiveItem taskTaskActiveItem = this.m_items[i];
					if (!(taskTaskActiveItem == null))
					{
						taskTaskActiveItem.OnRefreshUI(taskActive);
					}
				}
			}
			this.m_icon.localScale = Vector3.one;
			this.m_currentActive = (float)this.m_taskDataModule.DailyActive;
			this.m_sliderTxt.text = this.m_currentActive.ToString();
			this.m_slider.minValue = 0f;
			this.m_slider.maxValue = (float)Singleton<GameConfig>.Instance.MaxDailyScore;
			this.m_slider.value = (float)this.m_taskDataModule.DailyActive;
			this.m_receiveBt.gameObject.SetActive(!this.m_taskDataModule.IsDailyActiveFinished);
			this.m_receivedObj.SetActive(this.m_taskDataModule.IsDailyActiveFinished);
			if (!this.m_taskDataModule.IsDailyActiveFinished && this.m_taskDataModule.IsHaveDailyActiveReceive)
			{
				this.m_redPoint.Value = 1;
				this.m_receiveBt.GetComponent<UIGrays>().Recovery();
				return;
			}
			this.m_redPoint.Value = 0;
			this.m_receiveBt.GetComponent<UIGrays>().SetUIGray();
		}

		public void PlayEffect()
		{
			if (this.m_effect == null)
			{
				return;
			}
			this.m_effect.Play();
		}

		private void OnRefreshProgress(float value)
		{
			this.m_currentActive = Mathf.Lerp(this.m_fromActive, this.m_toActive, value);
			if (this.m_sliderTxt != null)
			{
				this.m_sliderTxt.text = ((int)this.m_currentActive).ToString();
			}
			if (this.m_slider != null)
			{
				this.m_slider.minValue = 0f;
				this.m_slider.maxValue = (float)Singleton<GameConfig>.Instance.MaxDailyScore;
				this.m_slider.value = this.m_currentActive;
			}
		}

		private void OnTaskTaskActiveItemClick(TaskTaskActiveItem item, UIItem uiItem, PropData propData, object openData)
		{
			if (this.m_taskDataModule.IsHaveDailyActiveReceive)
			{
				this.OnClickReceiveBt();
				return;
			}
			DxxTools.UI.OnItemClick(uiItem, propData, openData);
		}

		private void OnClickReceiveBt()
		{
			if (!this.m_taskDataModule.IsHaveDailyActiveReceive)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("1263"));
				return;
			}
			NetworkUtils.Task.DoTaskActiveRewardAllRequest(1, delegate(bool isOk, TaskActiveRewardAllResponse resp)
			{
				DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
			});
		}

		public float GetCurrentActive()
		{
			return this.m_currentActive;
		}

		public void PlayProgress(float lastActive, float toActive)
		{
			this.m_fromActive = lastActive;
			this.m_toActive = toActive;
			this.OnRefreshProgress(0f);
			this.m_isPlayingProgress = false;
			this.m_isPlayingDelay = true;
			this.m_delayTime = 0f;
			this.m_timeProgress = 0f;
		}

		public CustomButton m_receiveBt;

		public RedNodeOneCtrl m_redPoint;

		public GameObject m_receivedObj;

		public Transform m_icon;

		public ParticleSystem m_effect;

		public Slider m_slider;

		public CustomText m_sliderTxt;

		public List<TaskTaskActiveItem> m_items = new List<TaskTaskActiveItem>();

		private TaskDataModule m_taskDataModule;

		[SerializeField]
		[Label]
		private float m_currentActive;

		[SerializeField]
		[Label]
		private bool m_isPlayingProgress;

		[SerializeField]
		[Label]
		private bool m_isPlayingDelay;

		[SerializeField]
		[Label]
		private float m_fromActive;

		[SerializeField]
		[Label]
		private float m_toActive;

		[SerializeField]
		[Label]
		private float m_timeProgress;

		[SerializeField]
		private float m_durationProgress = 0.8f;

		[SerializeField]
		[Label]
		private float m_delayTime;

		[SerializeField]
		private float m_delayMaxTime = 0.7f;
	}
}
