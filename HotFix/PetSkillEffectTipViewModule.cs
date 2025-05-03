using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.ViewModule;
using LocalModels.Bean;
using SuperScrollView;

namespace HotFix
{
	public class PetSkillEffectTipViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.InitLoopList();
		}

		public override void OnOpen(object data)
		{
			if (data is PetSkillEffectTipViewModule.OpenData)
			{
				this.openData = data as PetSkillEffectTipViewModule.OpenData;
				this.RefreshLoopListItem();
				Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(this.openData.petId);
				this.petSkillItem.SetData(this.openData.battleSkill, elementById.quality);
				GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(this.openData.battleSkill.curSkillId);
				this.petSkillItem.txtLevel.text = "";
				return;
			}
			this.OnPopCommonClick(UIPopCommon.UIPopCommonClickType.ButtonClose);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.DeInitLoopList();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.popCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnPopCommonClick);
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.popCommon.OnClick = null;
		}

		private void OnPopCommonClick(UIPopCommon.UIPopCommonClickType type)
		{
			GameApp.View.CloseView(ViewName.PetSkillEffectTipViewModule, null);
		}

		private void InitLoopList()
		{
			this.loopList.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetServerItemByIndex), null);
		}

		private void DeInitLoopList()
		{
			foreach (CustomBehaviour customBehaviour in this.loopItemDict.Values)
			{
				if (customBehaviour != null)
				{
					customBehaviour.DeInit();
				}
			}
			this.loopItemDict.Clear();
		}

		private void RefreshLoopListItem()
		{
			this.itemNodeDatas.Clear();
			int num = GameApp.Table.GetManager().GetPet_petSkillModelInstance().GetElementById(this.openData.battleSkill.skillGroupId)
				.unlockLevel.Length;
			for (int i = 0; i < num; i++)
			{
				this.itemNodeDatas.Add(new PetSkillEffectTipViewModule.ItemNodeData
				{
					battleSkill = this.openData.battleSkill,
					petId = this.openData.petId,
					petLevel = this.openData.petLevel,
					itemIndex = i
				});
			}
			this.loopList.SetListItemCount(this.itemNodeDatas.Count, true);
			this.loopList.RefreshAllShownItem();
			this.loopList.MovePanelToItemIndex(this.openData.battleSkill.curIndex, 0f);
		}

		private LoopListViewItem2 OnGetServerItemByIndex(LoopListView2 view, int index)
		{
			if (index < 0 || index >= this.itemNodeDatas.Count)
			{
				return null;
			}
			PetSkillEffectTipViewModule.ItemNodeData itemNodeData = this.itemNodeDatas[index];
			LoopListViewItem2 loopListViewItem;
			if (itemNodeData.isTopSpace)
			{
				loopListViewItem = view.NewListViewItem("TopSpace");
				int instanceID = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour;
				this.loopItemDict.TryGetValue(instanceID, out customBehaviour);
				if (customBehaviour == null)
				{
					UIItemSpace component = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component.Init();
					this.loopItemDict[instanceID] = component;
				}
			}
			else if (itemNodeData.isBottomSpace)
			{
				loopListViewItem = view.NewListViewItem("BottomSpace");
				int instanceID2 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour2;
				this.loopItemDict.TryGetValue(instanceID2, out customBehaviour2);
				if (customBehaviour2 == null)
				{
					UIItemSpace component2 = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component2.Init();
					this.loopItemDict[instanceID2] = component2;
				}
			}
			else
			{
				loopListViewItem = view.NewListViewItem("PetSkillPreviewDescItem");
				int instanceID3 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour3;
				this.loopItemDict.TryGetValue(instanceID3, out customBehaviour3);
				if (customBehaviour3 == null)
				{
					PetSkillPreviewDescItem component3 = loopListViewItem.gameObject.GetComponent<PetSkillPreviewDescItem>();
					component3.Init();
					this.loopItemDict[instanceID3] = component3;
					customBehaviour3 = component3;
				}
				PetSkillPreviewDescItem petSkillPreviewDescItem = customBehaviour3 as PetSkillPreviewDescItem;
				if (petSkillPreviewDescItem != null)
				{
					petSkillPreviewDescItem.SetData(index, itemNodeData, this.loopList);
				}
			}
			return loopListViewItem;
		}

		public UIPopCommon popCommon;

		public PetSkillItem petSkillItem;

		public LoopListView2 loopList;

		private List<PetSkillEffectTipViewModule.ItemNodeData> itemNodeDatas = new List<PetSkillEffectTipViewModule.ItemNodeData>();

		private PetSkillEffectTipViewModule.OpenData openData;

		private Dictionary<int, CustomBehaviour> loopItemDict = new Dictionary<int, CustomBehaviour>();

		public class OpenData
		{
			public int petId;

			public int petLevel;

			public PetSkillData battleSkill;
		}

		public class ItemNodeData
		{
			public bool isTopSpace;

			public bool isBottomSpace;

			public int petId;

			public int petLevel;

			public PetSkillData battleSkill;

			public int itemIndex;
		}
	}
}
