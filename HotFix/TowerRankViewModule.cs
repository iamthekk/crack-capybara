using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.Logic.UI.SuperScrollUI;
using Framework.ViewModule;
using Proto.Common;
using Proto.Tower;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class TowerRankViewModule : BaseViewModule
	{
		public ViewName GetName()
		{
			return ViewName.TowerRankViewModule;
		}

		private void CloseSelf()
		{
			if (this.loadingStatus == TowerRankViewModule.LoadingStatus.Loading)
			{
				return;
			}
			GameApp.View.CloseView(this.GetName(), null);
		}

		private bool IsOpen()
		{
			return this.isOpen;
		}

		public override void OnCreate(object data)
		{
			this.closeButton.onClick.AddListener(new UnityAction(this.CloseSelf));
			this.maskButton.onClick.AddListener(new UnityAction(this.CloseSelf));
			this.towerDataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
			this.addAttributeDataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			this.loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.rankItemSelf.Init();
			foreach (TowerRankItemNode towerRankItemNode in this.rankItemTopList)
			{
				towerRankItemNode.Init();
			}
			this.rankItemScroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnRankListGetItemByIndex), null);
			this.rankItemScroll.mOnBeginDragAction = new Action(this.OnRankListBeginDrag);
			this.rankItemScroll.mOnDragingAction = new Action(this.OnRankListDraging);
			this.rankItemScroll.mOnEndDragAction = new Action(this.OnRankListEndDrag);
		}

		public override void OnOpen(object data)
		{
			this.isOpen = true;
			this.RefreshAll(false, false);
			this.obj_Empty.SetActiveSafe(false);
			this.towerDataModule.CheckLoadRankDataList(delegate
			{
				if (!this.IsOpen())
				{
					return;
				}
				this.OnBeginLoadRank(true);
			}, delegate(bool res)
			{
				if (!this.IsOpen())
				{
					this.netLoading.SetActive(false);
					this.RefreshAll(true, false);
					return;
				}
				this.OnEndLoadRank(res, true, true);
			});
			NetworkUtils.Tower.TowerRankIndexRequest(false, delegate(bool res, TowerRankIndexResponse response)
			{
				if (!this.IsOpen())
				{
					return;
				}
				if (res)
				{
					this.RefreshSelfRank();
				}
			});
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.isOpen = false;
			this.seqPool.Clear(false);
		}

		public override void OnDelete()
		{
			this.closeButton.onClick.RemoveListener(new UnityAction(this.CloseSelf));
			this.maskButton.onClick.RemoveListener(new UnityAction(this.CloseSelf));
			this.rankItemScroll.mOnBeginDragAction = null;
			this.rankItemScroll.mOnDragingAction = null;
			this.rankItemScroll.mOnEndDragAction = null;
			foreach (TowerRankItemNode towerRankItemNode in this.rankItemTopList)
			{
				towerRankItemNode.DeInit();
			}
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void RefreshAll(bool isShowRankList, bool isPlayAnim)
		{
			if (this.towerDataModule.RankDtoList.Count > 0)
			{
				this.obj_Empty.SetActive(false);
				if (isShowRankList)
				{
					this.rankItemScroll.SetListItemCount(this.towerDataModule.RankDtoList.Count + 1, false);
					this.rankItemScroll.RefreshAllShowItems();
				}
				else
				{
					this.rankItemScroll.SetListItemCount(0, true);
					this.rankItemScroll.RefreshAllShowItems();
				}
				for (int i = 0; i < this.rankItemTopList.Count; i++)
				{
					TowerRankItemNode towerRankItemNode = this.rankItemTopList[i];
					bool flag = i >= this.towerDataModule.RankDtoList.Count;
					towerRankItemNode.SetActive(!flag);
					if (!flag)
					{
						int num = i + 1;
						towerRankItemNode.RefreshData(this.towerDataModule.RankDtoList[i], num.ToString(), num);
					}
				}
				this.RefreshSelfRank();
				if (isPlayAnim)
				{
					this.PlayRankItemScale();
					return;
				}
			}
			else
			{
				this.obj_Empty.SetActive(true);
				this.rankItemScroll.SetListItemCount(0, true);
				this.rankItemScroll.RefreshAllShowItems();
				for (int j = 0; j < this.rankItemTopList.Count; j++)
				{
					TowerRankItemNode towerRankItemNode2 = this.rankItemTopList[j];
					bool flag2 = j >= this.towerDataModule.RankDtoList.Count;
					towerRankItemNode2.SetActive(!flag2);
					if (!flag2)
					{
						int num2 = j + 1;
						towerRankItemNode2.RefreshData(this.towerDataModule.RankDtoList[j], num2.ToString(), num2);
					}
				}
				this.RefreshSelfRank();
			}
		}

		private void PlayRankItemScale()
		{
			this.seqPool.Clear(false);
			for (int i = 0; i < this.rankItemScroll.ShownItemCount; i++)
			{
				LoopListViewItem2 shownItemByIndex = this.rankItemScroll.GetShownItemByIndex(i);
				if (!(shownItemByIndex == null))
				{
					RectTransform cachedRectTransform = shownItemByIndex.CachedRectTransform;
					cachedRectTransform.localScale = Vector3.zero;
					TweenSettingsExtensions.Append(this.seqPool.Get(), TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOScale(cachedRectTransform, 1f, 0.2f), (float)i * 0.05f));
				}
			}
		}

		private void RefreshSelfRank()
		{
			TowerRankDto towerRankDto = new TowerRankDto
			{
				UserId = this.loginDataModule.userId,
				NickName = this.loginDataModule.NickName,
				Avatar = this.loginDataModule.Avatar,
				AvatarFrame = this.loginDataModule.AvatarFrame,
				TitleId = this.loginDataModule.AvatarTitle,
				Power = (long)((int)this.addAttributeDataModule.Combat),
				Tower = this.towerDataModule.CompleteTowerLevelId,
				GuildName = this.loginDataModule.GetGuildName(false)
			};
			int curTowerRank = this.towerDataModule.CurTowerRank;
			string text = ((curTowerRank < 1) ? Singleton<LanguageManager>.Instance.GetInfoByID("uitower_norank") : this.towerDataModule.CurTowerRank.ToString());
			this.rankItemSelf.RefreshData(towerRankDto, text, curTowerRank);
		}

		private void OnBeginLoadRank(bool isShowNetLoading)
		{
			this.loadingStatus = TowerRankViewModule.LoadingStatus.Loading;
			this.netLoading.SetActive(isShowNetLoading);
		}

		private void OnEndLoadRank(bool res, bool isUpdateData, bool isPlayAnim)
		{
			this.loadingStatus = (isUpdateData ? TowerRankViewModule.LoadingStatus.None : TowerRankViewModule.LoadingStatus.Loaded);
			this.netLoading.SetActive(false);
			if (res)
			{
				this.RefreshAll(true, isPlayAnim);
			}
		}

		private void OnRankListBeginDrag()
		{
		}

		private void OnRankListDraging()
		{
			if (this.loadingStatus != TowerRankViewModule.LoadingStatus.ReadyLoad && this.loadingStatus != TowerRankViewModule.LoadingStatus.None)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex = this.rankItemScroll.GetShownItemByItemIndex(this.towerDataModule.RankDtoList.Count);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex2 = this.rankItemScroll.GetShownItemByItemIndex(this.towerDataModule.RankDtoList.Count - 1);
			if (shownItemByItemIndex2 == null)
			{
				return;
			}
			if (this.rankItemScroll.GetItemCornerPosInViewPort(shownItemByItemIndex2, 0).y + this.rankItemScroll.ViewPortSize >= this.loadingTipItemHeight)
			{
				if (this.loadingStatus != TowerRankViewModule.LoadingStatus.None)
				{
					return;
				}
				this.loadingStatus = TowerRankViewModule.LoadingStatus.ReadyLoad;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
			else
			{
				if (this.loadingStatus != TowerRankViewModule.LoadingStatus.ReadyLoad)
				{
					return;
				}
				this.loadingStatus = TowerRankViewModule.LoadingStatus.None;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
		}

		private void OnRankListEndDrag()
		{
			if (this.loadingStatus != TowerRankViewModule.LoadingStatus.ReadyLoad)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex = this.rankItemScroll.GetShownItemByItemIndex(this.towerDataModule.RankDtoList.Count);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			this.rankItemScroll.OnItemSizeChanged(shownItemByItemIndex.ItemIndex);
			this.UpdateLoadingTip(shownItemByItemIndex);
			this.towerDataModule.LoadRankDataList(true, delegate
			{
				if (!this.IsOpen())
				{
					return;
				}
				this.OnBeginLoadRank(false);
			}, delegate(bool res, bool isUpdateData)
			{
				if (!this.IsOpen())
				{
					return;
				}
				this.OnEndLoadRank(res, isUpdateData, false);
			});
		}

		private LoopListViewItem2 OnRankListGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem;
			if (index == this.towerDataModule.RankDtoList.Count)
			{
				loopListViewItem = listView.NewListViewItem("LoadingNode");
				this.UpdateLoadingTip(loopListViewItem);
				return loopListViewItem;
			}
			TowerRankDto towerRankDto = this.towerDataModule.RankDtoList[index];
			if (towerRankDto == null)
			{
				return null;
			}
			loopListViewItem = listView.NewListViewItem("TowerRankItem");
			CustomBehaviour component;
			this.rankNodeList.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out component);
			if (component == null)
			{
				component = loopListViewItem.gameObject.GetComponent<TowerRankItemNode>();
				component.Init();
				this.rankNodeList[loopListViewItem.gameObject.GetInstanceID()] = component;
			}
			if (index == this.towerDataModule.RankDtoList.Count - 1)
			{
				loopListViewItem.Padding = 0f;
			}
			TowerRankItemNode towerRankItemNode = component as TowerRankItemNode;
			if (towerRankItemNode != null)
			{
				int num = index + 1;
				towerRankItemNode.RefreshData(towerRankDto, num.ToString(), num);
			}
			return loopListViewItem;
		}

		private void UpdateLoadingTip(LoopListViewItem2 item)
		{
			if (item == null)
			{
				return;
			}
			SuperScrollBottomLoadingItem component = item.gameObject.GetComponent<SuperScrollBottomLoadingItem>();
			if (component == null)
			{
				return;
			}
			if (this.loadingStatus == TowerRankViewModule.LoadingStatus.None)
			{
				component.SetActive(false);
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, 0f);
				return;
			}
			component.SetActive(true);
			component.SetScroll(this.rankItemScroll);
			if (this.loadingStatus == TowerRankViewModule.LoadingStatus.Loaded)
			{
				component.SetAsNoMoreData();
			}
			else
			{
				component.SetAsLoading();
			}
			item.CachedRectTransform.SetSizeWithCurrentAnchors(1, this.loadingTipItemHeight);
		}

		[SerializeField]
		private LoopListView2 rankItemScroll;

		[SerializeField]
		private GameObject netLoading;

		[SerializeField]
		private CustomButton maskButton;

		[SerializeField]
		private CustomButton closeButton;

		[SerializeField]
		private List<TowerRankItemNode> rankItemTopList;

		[SerializeField]
		private TowerRankItemNode rankItemSelf;

		[SerializeField]
		private GameObject obj_Empty;

		private TowerRankViewModule.LoadingStatus loadingStatus;

		private readonly float loadingTipItemHeight = 100f;

		private Dictionary<int, CustomBehaviour> rankNodeList = new Dictionary<int, CustomBehaviour>();

		private TowerDataModule towerDataModule;

		private AddAttributeDataModule addAttributeDataModule;

		private LoginDataModule loginDataModule;

		private readonly SequencePool seqPool = new SequencePool();

		private bool isOpen;

		private enum LoadingStatus
		{
			None,
			ReadyLoad,
			Loading,
			Loaded
		}
	}
}
