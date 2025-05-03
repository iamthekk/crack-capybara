using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.Mining;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIMiningGridCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.miningDataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
			this.copyItem.gameObject.SetActiveSafe(false);
			this.treasureItem.gameObject.SetActiveSafe(false);
			this.treasureItem.Init();
			this.buttonDoor.gameObject.SetActiveSafe(false);
			this.buttonDoor.onClick.AddListener(new UnityAction(this.OnClickDoor));
		}

		protected override void OnDeInit()
		{
			this.buttonDoor.onClick.RemoveListener(new UnityAction(this.OnClickDoor));
			this.treasureItem.DeInit();
			for (int i = 0; i < this.gridItems.Count; i++)
			{
				UIMiningGridItem uiminingGridItem = this.gridItems[i];
				if (uiminingGridItem)
				{
					uiminingGridItem.DeInit();
				}
			}
			this.gridItems.Clear();
		}

		public void SetData()
		{
			List<GridDto> sortedGridDtoList = this.miningDataModule.GetSortedGridDtoList();
			GridDto treasureFirstGridDto = this.miningDataModule.GetTreasureFirstGridDto();
			for (int i = 0; i < sortedGridDtoList.Count; i++)
			{
				UIMiningGridItem uiminingGridItem;
				if (i < this.gridItems.Count)
				{
					uiminingGridItem = this.gridItems[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
					gameObject.SetParentNormal(this.layoutObj, false);
					uiminingGridItem = gameObject.GetComponent<UIMiningGridItem>();
					uiminingGridItem.Init();
					this.gridItems.Add(uiminingGridItem);
				}
				uiminingGridItem.gameObject.SetActiveSafe(true);
				uiminingGridItem.SetData(sortedGridDtoList[i]);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.layoutObj.GetComponent<RectTransform>());
			bool flag = false;
			if (treasureFirstGridDto != null)
			{
				Mining_oreRes mining_oreRes = GameApp.Table.GetManager().GetMining_oreRes(this.miningDataModule.MiningInfo.TreasureResId);
				if (mining_oreRes == null)
				{
					return;
				}
				for (int j = 0; j < this.gridItems.Count; j++)
				{
					if (this.gridItems[j].ServerPos == treasureFirstGridDto.Pos && mining_oreRes.iconOffset.Length >= 2)
					{
						RectTransform component = this.gridItems[j].GetComponent<RectTransform>();
						float num = component.sizeDelta.x * mining_oreRes.iconOffset[0];
						float num2 = component.sizeDelta.y * mining_oreRes.iconOffset[1];
						this.buttonDoor.transform.position = this.gridItems[j].transform.position;
						this.treasureItem.transform.position = this.gridItems[j].transform.position;
						this.treasureItem.GetComponent<RectTransform>().anchoredPosition += new Vector2(num, num2);
						this.treasureItem.SetData(mining_oreRes);
						flag = true;
						break;
					}
				}
			}
			this.treasureItem.gameObject.SetActiveSafe(flag);
			this.buttonDoor.gameObject.SetActiveSafe(this.miningDataModule.IsHaveTreasure && this.miningDataModule.IsTreasureGet);
		}

		public void OpenTreasure()
		{
			this.treasureItem.OpenTreasure();
		}

		public void OpenBomb(RepeatedField<GridDto> bombGrids)
		{
			for (int i = 0; i < bombGrids.Count; i++)
			{
				GridDto gridDto = bombGrids[i];
				for (int j = 0; j < this.gridItems.Count; j++)
				{
					UIMiningGridItem uiminingGridItem = this.gridItems[j];
					if (uiminingGridItem.ServerPos == gridDto.Pos)
					{
						uiminingGridItem.ShowBoomEffect();
						break;
					}
				}
			}
		}

		private void OnClickDoor()
		{
			if (this.miningDataModule.IsNotGetReward())
			{
				NetworkUtils.Mining.DoGetMiningRewardRequest(delegate(bool result, GetMiningRewardResponse response)
				{
					this._nextDoorCacheRewards.Clear();
					if (result && response.CommonData.Reward != null && response.CommonData.Reward.Count > 0)
					{
						this._nextDoorCacheRewards.AddRange(response.CommonData.Reward);
					}
				}, new Action(this.GoNextFloor));
				return;
			}
			this._nextDoorCacheRewards.Clear();
			this.GoNextFloor();
		}

		private void GoNextFloor()
		{
			int slotLeft = 0;
			foreach (GridDto gridDto in this.miningDataModule.MiningInfo.Grids)
			{
				if (gridDto.Status != 0 || gridDto.Floors != 0)
				{
					int slotLeft2 = slotLeft;
					slotLeft = slotLeft2 + 1;
				}
			}
			NetworkUtils.Mining.DoOpenNextDoorRequest(delegate(bool result, OpenNextDoorResponse response)
			{
				if (result)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Mining_NextFloor, null);
					GameApp.SDK.Analyze.Track_MiningMine_Next(this._nextDoorCacheRewards, slotLeft);
				}
			});
		}

		public async Task AutoMiningAni(List<GridDto> list)
		{
			List<Task> list2 = new List<Task>();
			for (int i = 0; i < this.gridItems.Count; i++)
			{
				for (int j = 0; j < list.Count; j++)
				{
					GridDto gridDto = list[j];
					if (gridDto.Pos == this.gridItems[i].GridData.Pos)
					{
						list2.Add(this.gridItems[i].AutoMining(gridDto));
					}
				}
			}
			await Task.WhenAll(list2);
		}

		public async Task AutoBombAni(int bombPos, List<GridDto> gridList)
		{
			List<Task> list = new List<Task>();
			for (int i = 0; i < this.gridItems.Count; i++)
			{
				if (this.gridItems[i].GridData.Pos == bombPos)
				{
					this.gridItems[i].AutoBombAni();
				}
				for (int j = 0; j < gridList.Count; j++)
				{
					GridDto gridDto = gridList[j];
					if (gridDto.Pos == this.gridItems[i].GridData.Pos)
					{
						list.Add(this.gridItems[i].AutoBombEffect(gridDto));
					}
				}
			}
			await Task.WhenAll(list);
		}

		public UIMiningGridItem GetGridItem(GridDto gridDto)
		{
			for (int i = 0; i < this.gridItems.Count; i++)
			{
				if (this.gridItems[i].GridData.Pos == gridDto.Pos)
				{
					return this.gridItems[i];
				}
			}
			return null;
		}

		public MiningGrid gridType;

		public GameObject layoutObj;

		public GameObject copyItem;

		public UIMiningTreasureItem treasureItem;

		public CustomButton buttonDoor;

		private List<UIMiningGridItem> gridItems = new List<UIMiningGridItem>();

		private MiningDataModule miningDataModule;

		private List<RewardDto> _nextDoorCacheRewards = new List<RewardDto>();
	}
}
