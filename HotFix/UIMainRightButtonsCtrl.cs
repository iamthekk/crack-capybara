using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIMainRightButtonsCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ActivitySlotTrain_StageChanged, new HandlerEvent(this.OnActivitySlotTrain_StageChanged));
			this.m_isUnfold = true;
			this.m_allButtons.Clear();
			this.m_allButtons.Add(this.m_chapterRankButton);
			this.m_allButtons.Add(this.m_chapterWheelButton);
			this.m_allButtons.Add(this.m_activityButton);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("Activity", this.m_activityButton.transform);
			this.m_redNodePaths.Add("Main.Activity_Week");
			this.m_allButtons.Add(this.m_slotButton);
			Singleton<GameFunctionController>.Instance.SetFunctionTarget("Activity_Slot", this.m_slotButton.transform);
			this.m_redNodePaths.Add("Main.Activity_Slot");
			foreach (BaseUIMainButton baseUIMainButton in this.m_allButtons)
			{
				if (!(baseUIMainButton == null))
				{
					baseUIMainButton.Init();
				}
			}
			this.m_allButtons.Sort(delegate(BaseUIMainButton a, BaseUIMainButton b)
			{
				int num;
				int num2;
				a.GetPriority(out num, out num2);
				int num3;
				int num4;
				b.GetPriority(out num3, out num4);
				if (num == num3)
				{
					return num4 - num2;
				}
				return num3 - num;
			});
			foreach (BaseUIMainButton baseUIMainButton2 in this.m_allButtons)
			{
				if (!(baseUIMainButton2 == null) && !(baseUIMainButton2.rectTransform == null))
				{
					baseUIMainButton2.rectTransform.SetAsFirstSibling();
					baseUIMainButton2.SetActive(baseUIMainButton2.IsShow());
				}
			}
			this.OnRefreshScroll();
			foreach (string text in this.m_redNodePaths)
			{
				if (!string.IsNullOrEmpty(text))
				{
					RedPointController.Instance.RegRecordChange(text, new Action<RedNodeListenData>(this.OnRefreshRedPointChange));
				}
			}
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ActivitySlotTrain_StageChanged, new HandlerEvent(this.OnActivitySlotTrain_StageChanged));
			for (int i = 0; i < this.m_allButtons.Count; i++)
			{
				BaseUIMainButton baseUIMainButton = this.m_allButtons[i];
				if (!(baseUIMainButton == null))
				{
					baseUIMainButton.DeInit();
				}
			}
			for (int j = 0; j < this.m_redNodePaths.Count; j++)
			{
				string text = this.m_redNodePaths[j];
				if (!string.IsNullOrEmpty(text))
				{
					RedPointController.Instance.UnRegRecordChange(text, new Action<RedNodeListenData>(this.OnRefreshRedPointChange));
				}
			}
			this.m_allButtons.Clear();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_chapterRankButton)
			{
				this.m_chapterRankButton.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (this.m_chapterWheelButton)
			{
				this.m_chapterWheelButton.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		private void OnClickChooseButton(CustomChooseButton obj)
		{
			this.m_isUnfold = !this.m_isUnfold;
			this.OnRefreshScroll();
		}

		public void SwitchToFold()
		{
			this.m_isUnfold = false;
			this.OnRefreshScroll();
		}

		public void SwitchToUnfold()
		{
			this.m_isUnfold = true;
			this.OnRefreshScroll();
		}

		private void OnRefreshRedPointChange(RedNodeListenData obj)
		{
			if (!this.m_isUnfold)
			{
				for (int i = 0; i < this.m_redNodePaths.Count; i++)
				{
					RedPointDataRecord record = RedPointController.Instance.GetRecord(this.m_redNodePaths[i]);
					if (record != null && record.RedPointCount > 0)
					{
						break;
					}
				}
			}
			for (int j = 0; j < this.m_allButtons.Count; j++)
			{
				BaseUIMainButton baseUIMainButton = this.m_allButtons[j];
				if (!(baseUIMainButton == null))
				{
					baseUIMainButton.OnRefreshAnimation();
				}
			}
		}

		private int GetCount()
		{
			int num = 0;
			for (int i = 0; i < this.m_allButtons.Count; i++)
			{
				BaseUIMainButton baseUIMainButton = this.m_allButtons[i];
				if (!(baseUIMainButton == null) && baseUIMainButton.IsShow())
				{
					num++;
				}
			}
			return num;
		}

		private void OnRefreshScroll()
		{
			int count = this.GetCount();
			if (count == 0)
			{
				if (this.m_scroll != null)
				{
					this.m_scroll.gameObject.SetActive(false);
				}
				return;
			}
			if (this.m_scroll != null)
			{
				this.m_scroll.gameObject.SetActive(true);
			}
			if (this.m_parent != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_parent);
			}
			if (this.m_gridLayoutGroup == null)
			{
				return;
			}
			bool isUnfold = this.m_isUnfold;
			int num = Mathf.CeilToInt((float)count * 1f / (float)this.m_gridLayoutGroup.constraintCount);
			int num2 = ((count <= this.m_gridLayoutGroup.constraintCount) ? count : this.m_gridLayoutGroup.constraintCount);
			Vector2 zero = Vector2.zero;
			float num3 = 0f;
			if (!isUnfold)
			{
				num = 1;
				num2 = 1;
			}
			if (this.m_chooseCustomButton != null)
			{
				this.m_chooseCustomButton.SetActive(false);
			}
			if (this.m_scroll != null)
			{
				RectTransform rectTransform = this.m_scroll.transform as RectTransform;
				float num4 = (float)num * (this.m_gridLayoutGroup.cellSize.x + this.m_gridLayoutGroup.spacing.x) + (float)this.m_gridLayoutGroup.padding.horizontal - this.m_gridLayoutGroup.spacing.x;
				float num5 = (float)num2 * (this.m_gridLayoutGroup.cellSize.y + this.m_gridLayoutGroup.spacing.y) + (float)this.m_gridLayoutGroup.padding.vertical - this.m_gridLayoutGroup.spacing.y;
				rectTransform.sizeDelta = new Vector2(num4, num5) + zero;
				if (this.m_scroll.viewport != null)
				{
					Utility.SetBottom(this.m_scroll.viewport, num3);
				}
			}
		}

		public void OnRefresh()
		{
			for (int i = 0; i < this.m_allButtons.Count; i++)
			{
				BaseUIMainButton baseUIMainButton = this.m_allButtons[i];
				if (!(baseUIMainButton == null))
				{
					baseUIMainButton.SetActive(baseUIMainButton.IsShow());
				}
			}
			this.OnRefreshScroll();
			for (int j = 0; j < this.m_allButtons.Count; j++)
			{
				BaseUIMainButton baseUIMainButton2 = this.m_allButtons[j];
				if (!(baseUIMainButton2 == null))
				{
					baseUIMainButton2.OnRefresh();
				}
			}
		}

		public void OnLanguageChange()
		{
			for (int i = 0; i < this.m_allButtons.Count; i++)
			{
				BaseUIMainButton baseUIMainButton = this.m_allButtons[i];
				if (!(baseUIMainButton == null))
				{
					baseUIMainButton.OnLanguageChange();
				}
			}
		}

		public void SwitchFoldState(int state)
		{
			if (state == 1)
			{
				this.SwitchToFold();
				return;
			}
			if (state != 2)
			{
				this.OnClickChooseButton(null);
				return;
			}
			this.SwitchToUnfold();
		}

		private void OnActivitySlotTrain_StageChanged(object sender, int eventid, BaseEventArgs eventArgs)
		{
			this.m_slotButton.SetActive(this.m_slotButton.IsShow());
			this.OnRefreshScroll();
		}

		public ScrollRect m_scroll;

		public UIMainButton_ChapterRank m_chapterRankButton;

		public UIMainButton_ChapterWheel m_chapterWheelButton;

		public UIMainButton_Activity m_activityButton;

		public UIMainButton_Slot m_slotButton;

		public UIMainChooseButtonCtrl m_chooseCustomButton;

		public RectTransform m_parent;

		public GridLayoutGroup m_gridLayoutGroup;

		private List<BaseUIMainButton> m_allButtons = new List<BaseUIMainButton>();

		private List<string> m_redNodePaths = new List<string>();

		private bool m_isUnfold;
	}
}
