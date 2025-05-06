using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.Platform;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class UIRogueDungeonSkillCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_ClickSkill, new HandlerEvent(this.OnShowDetailInfo));
			this.Scroll.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
			RectTransform component = this.Scroll.GetComponent<RectTransform>();
			float y = component.sizeDelta.y;
			float y2 = this.bgRT.sizeDelta.y;
			float num = 0f;
			if (Singleton<PlatformHelper>.Instance.IsFringe())
			{
				num = Singleton<PlatformHelper>.Instance.GetBottomHeight();
			}
			component.sizeDelta = new Vector2(component.sizeDelta.x, y - num);
			this.bgRT.sizeDelta = new Vector2(this.bgRT.sizeDelta.x, y2 - num);
			if (Utility.UI.ScreenRatio > Utility.UI.DesignRatio)
			{
				float num2 = (Utility.UI.ScreenRatio - Utility.UI.DesignRatio + 1f) * y - num;
				component.sizeDelta = new Vector2(component.sizeDelta.x, num2);
				float num3 = (Utility.UI.ScreenRatio - Utility.UI.DesignRatio + 1f) * y2 - num;
				this.bgRT.sizeDelta = new Vector2(this.bgRT.sizeDelta.x, num3);
			}
			this.Scroll.ScrollRect.horizontal = false;
			this.Scroll.ScrollRect.vertical = true;
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_ClickSkill, new HandlerEvent(this.OnShowDetailInfo));
			this.DeInitAllScrollUI();
		}

		public void SetData(List<GameEventSkillBuildData> list)
		{
			this.mDataList = list;
			this.Scroll.SetListItemCount(this.mDataList.Count, false);
			this.Scroll.RefreshAllShownItem();
		}

		private void OnShowDetailInfo(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgClickSkill eventArgClickSkill = eventArgs as EventArgClickSkill;
			if (eventArgClickSkill == null)
			{
				return;
			}
			GameEventSkillBuildData skillBuildData = eventArgClickSkill.skillItem.GetSkillBuildData();
			new InfoTipViewModule.InfoTipData
			{
				m_name = skillBuildData.skillName,
				m_info = skillBuildData.skillFullDetail,
				m_position = eventArgClickSkill.skillItem.GetPosition(),
				m_offsetY = 280f
			}.Open();
		}

		private LoopGridViewItem OnGetItemByIndex(LoopGridView view, int index, int row, int column)
		{
			if (index < 0 || index >= this.mDataList.Count)
			{
				return null;
			}
			GameEventSkillBuildData gameEventSkillBuildData = this.mDataList[index];
			LoopGridViewItem loopGridViewItem = view.NewListViewItem("UIGameEventLearnedSkillItem");
			int instanceID = loopGridViewItem.gameObject.GetInstanceID();
			UIGameEventLearnedSkillItem uigameEventLearnedSkillItem = this.TryGetUI(instanceID);
			UIGameEventLearnedSkillItem component = loopGridViewItem.GetComponent<UIGameEventLearnedSkillItem>();
			if (uigameEventLearnedSkillItem == null)
			{
				uigameEventLearnedSkillItem = this.TryAddUI(instanceID, loopGridViewItem, component);
			}
			uigameEventLearnedSkillItem.Refresh(gameEventSkillBuildData);
			uigameEventLearnedSkillItem.SetActive(true);
			return loopGridViewItem;
		}

		private UIGameEventLearnedSkillItem TryGetUI(int key)
		{
			UIGameEventLearnedSkillItem uigameEventLearnedSkillItem;
			if (this.mUICtrlDic.TryGetValue(key, out uigameEventLearnedSkillItem))
			{
				return uigameEventLearnedSkillItem;
			}
			return null;
		}

		private UIGameEventLearnedSkillItem TryAddUI(int key, LoopGridViewItem loopItem, UIGameEventLearnedSkillItem ui)
		{
			ui.Init();
			UIGameEventLearnedSkillItem uigameEventLearnedSkillItem;
			if (this.mUICtrlDic.TryGetValue(key, out uigameEventLearnedSkillItem))
			{
				if (uigameEventLearnedSkillItem == null)
				{
					uigameEventLearnedSkillItem = ui;
					this.mUICtrlDic[key] = ui;
				}
				return ui;
			}
			this.mUICtrlDic.Add(key, ui);
			return ui;
		}

		private void DeInitAllScrollUI()
		{
			foreach (KeyValuePair<int, UIGameEventLearnedSkillItem> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUICtrlDic.Clear();
		}

		public RectTransform bgRT;

		public LoopGridView Scroll;

		private const string ITEM_PREFAB_NAME = "UIGameEventLearnedSkillItem";

		private List<GameEventSkillBuildData> mDataList = new List<GameEventSkillBuildData>();

		private Dictionary<int, UIGameEventLearnedSkillItem> mUICtrlDic = new Dictionary<int, UIGameEventLearnedSkillItem>();
	}
}
