using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class TowerProgressCtr : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.towerDataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
			this.progressListView.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetProgressItem), null);
			this.rewardButton.onClick.AddListener(new UnityAction(this.OnRewardButtonClick));
		}

		protected override void OnDeInit()
		{
			this.rewardButton.onClick.RemoveListener(new UnityAction(this.OnRewardButtonClick));
		}

		private void OnRewardButtonClick()
		{
			UIBoxInfoViewModule.Transfer transfer = new UIBoxInfoViewModule.Transfer
			{
				nodeType = UIBoxInfoViewModule.UIBoxInfoNodeType.Right,
				rewards = this.towerDataModule.GetTowerReward(this.towerDataModule.CurTowerConfig),
				position = this.rewardButton.transform.position,
				anchoredPositionOffset = new Vector3(50f, 0f, 0f),
				secondLayer = true
			};
			GameApp.View.OpenView(ViewName.RewardBoxInfoViewModule, transfer, 1, null, null);
		}

		private LoopListViewItem2 OnGetProgressItem(LoopListView2 listView, int pageIndex)
		{
			if (pageIndex < 0)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("ProgressItem");
			loopListViewItem.GetComponent<TowerProgressNode>().SetData(pageIndex, this.towerDataModule.CurTowerLevelNum);
			return loopListViewItem;
		}

		public void UpdateProgressList()
		{
			this.progressListView.SetListItemCount(this.towerDataModule.MaxLevelCount, true);
			this.progressListView.RefreshAllShownItem();
			if (this.progressListView.ShownItemCount <= 0)
			{
				this.rewardTipObj.gameObject.SetActive(false);
				return;
			}
			this.rewardTipObj.gameObject.SetActive(true);
			Vector3 position = this.rewardTipObj.position;
			if (this.towerDataModule.IsShowTowerTopReward(this.towerDataModule.CurTowerLevelId))
			{
				position.y = this.rewardButton.transform.position.y;
				this.rewardTipText.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitower_box");
			}
			else
			{
				TowerProgressNode component = this.progressListView.GetShownItemByItemIndex(this.towerDataModule.CurTowerLevelIndex).GetComponent<TowerProgressNode>();
				position.y = component.TipItemPoint.position.y;
				this.rewardTipText.text = string.Format("{0}/{1}", this.towerDataModule.CurTowerLevelNum, this.towerDataModule.MaxLevelCount);
			}
			this.rewardTipObj.position = position;
		}

		[SerializeField]
		private LoopListView2 progressListView;

		[SerializeField]
		private Transform rewardTipObj;

		[SerializeField]
		private CustomText rewardTipText;

		[SerializeField]
		private CustomButton rewardButton;

		private TowerDataModule towerDataModule;
	}
}
