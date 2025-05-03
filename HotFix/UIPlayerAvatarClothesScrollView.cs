using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class UIPlayerAvatarClothesScrollView : CustomBehaviour
	{
		public int CurrentSelectedId
		{
			get
			{
				return this.m_CurrentSelectedId;
			}
		}

		protected override void OnInit()
		{
			this.m_gridView.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
			this.loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
		}

		protected override void OnDeInit()
		{
			this.loginDataModule = null;
		}

		public void Close()
		{
			this.RemoveAllNodes();
			this.m_datalist.Clear();
		}

		private void RemoveAllNodes()
		{
			foreach (KeyValuePair<int, UIPlayerAvatarClothesCtrl> keyValuePair in this.m_UIItemDic)
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.DeInit();
				}
			}
			this.m_UIItemDic.Clear();
			this.m_sequencePool.Clear(false);
		}

		private LoopGridViewItem OnGetItemByIndex(LoopGridView view, int index, int row, int column)
		{
			if (index < 0 || index >= this.m_datalist.Count)
			{
				return null;
			}
			int num = this.m_datalist[index];
			LoopGridViewItem loopGridViewItem = view.NewListViewItem("UIPlayerAvatarClothesCtrl");
			int instanceID = loopGridViewItem.gameObject.GetInstanceID();
			UIPlayerAvatarClothesCtrl uiplayerAvatarClothesCtrl = this.TryGetUI(instanceID);
			if (uiplayerAvatarClothesCtrl == null)
			{
				uiplayerAvatarClothesCtrl = this.TryAddUI(instanceID, loopGridViewItem, loopGridViewItem.GetComponent<UIPlayerAvatarClothesCtrl>());
			}
			uiplayerAvatarClothesCtrl.viewType = this.m_viewType;
			uiplayerAvatarClothesCtrl.tabType = this.m_TabType;
			uiplayerAvatarClothesCtrl.RefreshData(num, this.Register);
			uiplayerAvatarClothesCtrl.SetSelected(this.IsCur(uiplayerAvatarClothesCtrl));
			return loopGridViewItem;
		}

		private UIPlayerAvatarClothesCtrl TryGetUI(int key)
		{
			UIPlayerAvatarClothesCtrl uiplayerAvatarClothesCtrl;
			if (this.m_UIItemDic.TryGetValue(key, out uiplayerAvatarClothesCtrl))
			{
				return uiplayerAvatarClothesCtrl;
			}
			return null;
		}

		private UIPlayerAvatarClothesCtrl TryAddUI(int key, LoopGridViewItem loopitem, UIPlayerAvatarClothesCtrl ui)
		{
			ui.Init();
			ui.OnClick = new Action<UIPlayerAvatarClothesCtrl>(this.OnClickNode);
			UIPlayerAvatarClothesCtrl uiplayerAvatarClothesCtrl;
			if (this.m_UIItemDic.TryGetValue(key, out uiplayerAvatarClothesCtrl))
			{
				if (uiplayerAvatarClothesCtrl == null)
				{
					this.m_UIItemDic[key] = ui;
				}
				return ui;
			}
			this.m_UIItemDic.Add(key, ui);
			return ui;
		}

		private void OnClickNode(UIPlayerAvatarClothesCtrl node)
		{
			this.SelectNode((node != null) ? node.Id : 0);
		}

		public void SelectNode(int id)
		{
			this.m_CurrentSelectedId = id;
			foreach (KeyValuePair<int, UIPlayerAvatarClothesCtrl> keyValuePair in this.m_UIItemDic)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.SetSelected(this.IsCur(keyValuePair.Value));
				}
			}
			Action<int> onSelectIcon = this.OnSelectIcon;
			if (onSelectIcon == null)
			{
				return;
			}
			onSelectIcon(this.CurrentSelectedId);
		}

		private bool IsCur(UIPlayerAvatarClothesCtrl node)
		{
			return !(node == null) && node.Id == this.m_CurrentSelectedId;
		}

		public void SwitchUIType(int viewType, int tabType)
		{
			this.m_viewType = viewType;
			this.m_TabType = tabType;
			this.m_datalist.Clear();
			switch (this.m_viewType)
			{
			case 1:
				this.CollectAvatarList();
				break;
			case 2:
				this.CollectClothesList();
				break;
			case 3:
				this.CollectSceneList();
				break;
			}
			this.m_datalist.Sort();
			this.m_gridView.SetListItemCount(this.m_datalist.Count, true);
			this.RefreshAllShownItem();
			this.PlayScale();
		}

		public void RefreshAllShownItem()
		{
			this.m_gridView.RefreshAllShownItem();
		}

		private void CollectSceneList()
		{
			List<Avatar_SceneSkin> list = GameApp.Table.GetManager().GetAvatar_SceneSkinElements().ToList<Avatar_SceneSkin>();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].isHide != 1)
				{
					this.m_datalist.Add(list[i].id);
				}
			}
		}

		private void CollectAvatarList()
		{
			List<Avatar_Avatar> list = GameApp.Table.GetManager().GetAvatar_AvatarElements().ToList<Avatar_Avatar>();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].isHide != 1 && list[i].type == this.m_TabType)
				{
					if (this.m_TabType == 1)
					{
						if (this.loginDataModule.IsAvatarIconEndTime(list[i].id))
						{
							goto IL_00C6;
						}
					}
					else if (this.m_TabType == 2)
					{
						if (this.loginDataModule.IsAvatarFrameEndTime(list[i].id))
						{
							goto IL_00C6;
						}
					}
					else if (this.m_TabType == 7 && this.loginDataModule.IsAvatarTitleEndTime(list[i].id))
					{
						goto IL_00C6;
					}
					this.m_datalist.Add(list[i].id);
				}
				IL_00C6:;
			}
		}

		private void CollectClothesList()
		{
			List<Avatar_Skin> list = GameApp.Table.GetManager().GetAvatar_SkinElements().ToList<Avatar_Skin>();
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].isHide != 1 && list[i].part == this.m_TabType)
				{
					if (this.m_TabType == 2)
					{
						if (this.loginDataModule.IsClothesHeadEndTime(list[i].id))
						{
							goto IL_00C6;
						}
					}
					else if (this.m_TabType == 1)
					{
						if (this.loginDataModule.IsClothesBodyEndTime(list[i].id))
						{
							goto IL_00C6;
						}
					}
					else if (this.m_TabType == 3 && this.loginDataModule.IsClothesAccessoryEndTime(list[i].id))
					{
						goto IL_00C6;
					}
					this.m_datalist.Add(list[i].id);
				}
				IL_00C6:;
			}
		}

		private void PlayScale()
		{
			this.m_sequencePool.Clear(false);
			Sequence sequence = this.m_sequencePool.Get();
			for (int i = 0; i < this.m_gridView.ItemGroupList.Count; i++)
			{
				GridItemGroup gridItemGroup = this.m_gridView.ItemGroupList[i];
				if (gridItemGroup != null)
				{
					LoopGridViewItem loopGridViewItem = gridItemGroup.First;
					TweenSettingsExtensions.AppendInterval(sequence, 0.08f);
					while (loopGridViewItem != null)
					{
						UIPlayerAvatarClothesCtrl uiplayerAvatarClothesCtrl;
						this.m_UIItemDic.TryGetValue(loopGridViewItem.gameObject.GetInstanceID(), out uiplayerAvatarClothesCtrl);
						if (uiplayerAvatarClothesCtrl == null)
						{
							break;
						}
						if (uiplayerAvatarClothesCtrl.m_button != null)
						{
							uiplayerAvatarClothesCtrl.m_button.gameObject.transform.localScale = Vector3.zero;
							TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOScale(uiplayerAvatarClothesCtrl.m_button.gameObject.transform, Vector3.one, 0.08f));
						}
						loopGridViewItem = loopGridViewItem.NextItem;
					}
				}
			}
		}

		public LoopGridView m_gridView;

		private List<int> m_datalist = new List<int>();

		private Dictionary<int, UIPlayerAvatarClothesCtrl> m_UIItemDic = new Dictionary<int, UIPlayerAvatarClothesCtrl>();

		private SequencePool m_sequencePool = new SequencePool();

		private int m_viewType = 1;

		private int m_TabType = 1;

		private int m_CurrentSelectedId;

		private LoginDataModule loginDataModule;

		public Action<int> OnSelectIcon;

		public SpriteRegister Register;
	}
}
