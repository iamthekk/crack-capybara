using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class PetSkillTipViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.loopListView2.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		public override void OnOpen(object data)
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.btnFullBg.onClick.AddListener(new UnityAction(this.OnBtnCloseClick));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.uiPopCommon.OnClick = null;
			this.btnFullBg.onClick.RemoveListener(new UnityAction(this.OnBtnCloseClick));
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			this.OnBtnCloseClick();
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.PetSkillTipViewModule, null);
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 view, int index)
		{
			if (index < 0 || index >= this.skillIds.Count)
			{
				return null;
			}
			int num = this.skillIds[index];
			int num2 = this.unlockLevels[index];
			LoopListViewItem2 loopListViewItem = view.NewListViewItem("SkillDescItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			CustomBehaviour customBehaviour;
			this.itemsDict.TryGetValue(instanceID, out customBehaviour);
			if (customBehaviour == null)
			{
				PetSkillDescItem component = loopListViewItem.gameObject.GetComponent<PetSkillDescItem>();
				component.Init();
				this.itemsDict[instanceID] = component;
				customBehaviour = component;
			}
			PetSkillDescItem petSkillDescItem = customBehaviour as PetSkillDescItem;
			if (petSkillDescItem != null)
			{
				petSkillDescItem.SetData(index, num, this.openData.petData.level >= num2);
			}
			return loopListViewItem;
		}

		public UIPopCommon uiPopCommon;

		public CustomText txtTitle;

		public CustomImage imgSkillIcon;

		public GameObject goAssist;

		public GameObject goFight;

		public CustomText txtDesc;

		public PetSkillDescItem petSkillDescItem;

		public LoopListView2 loopListView2;

		public CustomButton btnFullBg;

		private PetSkillTipViewModule.OpenData openData;

		private List<int> skillIds = new List<int>();

		private List<int> unlockLevels = new List<int>();

		private Dictionary<int, CustomBehaviour> itemsDict = new Dictionary<int, CustomBehaviour>();

		public class OpenData
		{
			public PetData petData;

			public PetSkillData petSkillData;
		}
	}
}
