using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class CarnivalActiveProgressCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this._carnivalDataModule = GameApp.Data.GetDataModule(DataName.SevenDayCarnivalDataModule);
			this.TotalActive.text = this._carnivalDataModule.ActivePower.ToString();
			this.RefreshActiveProgress(this._carnivalDataModule.ActivePower);
			this.RefreshActiveItems();
			this.BoxItem.SetActive(false);
			this.EquipOne.SetActive(false);
		}

		protected override void OnDeInit()
		{
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
				int num = (int)Mathf.Lerp((float)this.m_fromActive, (float)this.m_toActive, this.m_timeProgress / this.m_durationProgress);
				this.RefreshActiveProgress(num);
			}
		}

		public void PlayProgress(int lastActive, int toActive)
		{
			this.m_fromActive = lastActive;
			this.m_toActive = toActive;
			this.RefreshActiveProgress(lastActive);
			this.m_isPlayingProgress = false;
			this.m_isPlayingDelay = true;
			this.m_delayTime = 0f;
			this.m_timeProgress = 0f;
		}

		public void RefreshActiveProgress(int activePower)
		{
			int num = Mathf.Max(this._carnivalDataModule.ActiveOneItems.Count, 1);
			int num2;
			int num3;
			int curRewardIndex = this.GetCurRewardIndex(activePower, out num2, out num3);
			float num4 = (float)curRewardIndex * 1f / (float)num;
			float num5;
			if (curRewardIndex >= this._carnivalDataModule.ActiveOneItems.Count)
			{
				num5 = 1f;
			}
			else
			{
				num5 = num4 + (float)num2 * 1f / (float)num3 / (float)num;
			}
			this.ActiveProgrsss.value = Mathf.Min(1f, num5);
			this.TotalActive.text = activePower.ToString();
		}

		private int GetCurRewardIndex(int activePower, out int curStagePower, out int curStageMaxPower)
		{
			List<ActiveOneItem> activeOneItems = this._carnivalDataModule.ActiveOneItems;
			int num = 0;
			for (int i = 0; i < activeOneItems.Count; i++)
			{
				if (activeOneItems[i].ActiveNeed > activePower)
				{
					curStagePower = ((i == 0) ? activePower : (activePower - activeOneItems[i - 1].ActiveNeed));
					curStageMaxPower = activeOneItems[i].ActiveNeed - num;
					return i;
				}
				num = activeOneItems[i].ActiveNeed;
			}
			curStageMaxPower = 1;
			curStagePower = 0;
			return activeOneItems.Count;
		}

		public void RefreshActiveItems()
		{
			List<ActiveOneItem> activeOneItems = this._carnivalDataModule.ActiveOneItems;
			if (this._activeItems.Count > 0)
			{
				return;
			}
			foreach (Transform transform in this.RewardBoxRoots)
			{
				transform.gameObject.SetActive(false);
			}
			this.progressTotalW = (this.ActiveProgrsss.transform as RectTransform).rect.width;
			this.progressPerRewardW = this.progressTotalW / (float)activeOneItems.Count;
			for (int i = 0; i < activeOneItems.Count; i++)
			{
				this.RewardBoxRoots[i].gameObject.SetActive(true);
				RectTransform component = this.RewardBoxRoots[i].GetComponent<RectTransform>();
				Vector2 anchoredPosition = component.anchoredPosition;
				anchoredPosition.x = this.progressPerRewardW * (float)(i + 1);
				component.anchoredPosition = anchoredPosition;
				bool ifEquipOne = activeOneItems[i].IfEquipOne;
				GameObject item = Object.Instantiate<GameObject>(ifEquipOne ? this.EquipOne : this.BoxItem, this.RewardBoxRoots[i]);
				item.SetActive(true);
				this._activeItems.Add(item);
				CarnivalItemBaseCtrl component2 = item.GetComponent<CarnivalItemBaseCtrl>();
				component2.Init();
				item.transform.Find("activeText").GetComponent<CustomText>().text = activeOneItems[i].ActiveNeed.ToString();
				component2.SetOpen(false);
				if (this._carnivalDataModule.IfActiveStageGot(i + 1))
				{
					if (component2 != null)
					{
						component2.PlayAnimation(false);
					}
					component2.SetOpen(true);
					component2.SetOpen(CarnivalItemBaseCtrl.BoxState.State_Opened);
				}
				else if (activeOneItems[i].ActiveNeed <= this._carnivalDataModule.ActivePower)
				{
					if (component2 != null)
					{
						component2.PlayAnimation(true);
					}
					if (component2 != null)
					{
						component2.SetOpen(CarnivalItemBaseCtrl.BoxState.State_Normal);
					}
				}
				else if (component2 != null)
				{
					component2.PlayAnimation(false);
				}
				int tempIndex = i;
				if (ifEquipOne)
				{
					component2.gameObject.SetActive(true);
					component2.SetEquipInfo(new PropData
					{
						rowId = (ulong)((long)activeOneItems[i].DropItems[0].ID),
						id = (uint)activeOneItems[i].DropItems[0].ID,
						count = (ulong)((uint)activeOneItems[i].DropItems[0].TotalCount)
					});
					component2.SetEquipClickCallBack(delegate(UIItem item, PropData propData, object obj)
					{
						this.ClickActiveRewardNormal(tempIndex, item.transform.position);
					});
				}
				else
				{
					item.GetComponent<CustomButton>().onClick.AddListener(delegate
					{
						this.ClickActiveRewardNormal(tempIndex, item.transform.position);
					});
				}
			}
		}

		public void RefreshActiveItemsAnim()
		{
			List<ActiveOneItem> activeOneItems = this._carnivalDataModule.ActiveOneItems;
			if (this._activeItems.Count > activeOneItems.Count)
			{
				return;
			}
			for (int i = 0; i < this._activeItems.Count; i++)
			{
				CarnivalItemBaseCtrl component = this._activeItems[i].GetComponent<CarnivalItemBaseCtrl>();
				component.SetOpen(CarnivalItemBaseCtrl.BoxState.State_Gray);
				component.SetOpen(false);
				if (this._carnivalDataModule.IfActiveStageGot(i + 1))
				{
					if (component != null)
					{
						component.PlayAnimation(false);
					}
					component.SetOpen(true);
					if (component != null)
					{
						component.SetOpen(CarnivalItemBaseCtrl.BoxState.State_Opened);
					}
				}
				else if (activeOneItems[i].ActiveNeed <= this._carnivalDataModule.ActivePower)
				{
					if (component != null)
					{
						component.SetOpen(CarnivalItemBaseCtrl.BoxState.State_Normal);
					}
					if (component != null)
					{
						component.PlayAnimation(true);
					}
				}
				else
				{
					if (component != null)
					{
						component.SetOpen(CarnivalItemBaseCtrl.BoxState.State_Gray);
					}
					if (component != null)
					{
						component.PlayAnimation(false);
					}
				}
			}
		}

		public void RefreshActiveItemsState()
		{
			List<ActiveOneItem> activeOneItems = this._carnivalDataModule.ActiveOneItems;
			for (int i = 0; i < activeOneItems.Count; i++)
			{
				if (i < this.RewardBoxRoots.Count)
				{
					Transform child = this.RewardBoxRoots[i].GetChild(0);
					if (this._carnivalDataModule.IfActiveStageGot(i + 1))
					{
						CarnivalItemBaseCtrl component = child.GetComponent<CarnivalItemBaseCtrl>();
						component.PlayAnimation(false);
						component.SetOpen(CarnivalItemBaseCtrl.BoxState.State_Opened);
						component.SetOpen(true);
					}
					else
					{
						int activeNeed = activeOneItems[i].ActiveNeed;
						int activePower = this._carnivalDataModule.ActivePower;
					}
				}
			}
		}

		private void ClickActiveRewardNormal(int index, Vector3 tipPos)
		{
			int num = index + 1;
			List<ActiveOneItem> activeOneItems = this._carnivalDataModule.ActiveOneItems;
			if (this._carnivalDataModule.IfActiveStageGot(num))
			{
				return;
			}
			if (activeOneItems[index].ActiveNeed > this._carnivalDataModule.ActivePower)
			{
				if (activeOneItems[index].IfEquipOne)
				{
					PropData propData = new PropData
					{
						rowId = (ulong)((long)activeOneItems[index].DropItems[0].ID),
						id = (uint)activeOneItems[index].DropItems[0].ID,
						count = (ulong)((uint)activeOneItems[index].DropItems[0].TotalCount)
					};
					DxxTools.UI.ShowItemInfo(new ItemInfoOpenData
					{
						m_propData = propData,
						m_openDataType = ItemInfoOpenDataType.eShow,
						m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume)
					}, default(Vector3), 0f);
					return;
				}
				UIBoxInfoViewModule.Transfer transfer = new UIBoxInfoViewModule.Transfer
				{
					nodeType = UIBoxInfoViewModule.UIBoxInfoNodeType.Up,
					rewards = activeOneItems[index].DropItems,
					position = tipPos,
					anchoredPositionOffset = new Vector3(0f, 40f, 0f),
					secondLayer = true
				};
				GameApp.View.OpenView(ViewName.RewardBoxInfoViewModule, transfer, 1, null, null);
				return;
			}
			else
			{
				Action<int, int> onSendGetActive = this.OnSendGetActive;
				if (onSendGetActive == null)
				{
					return;
				}
				onSendGetActive(num, 0);
				return;
			}
		}

		private int GetCurActiveItemNeedPower(int curActiveIndex)
		{
			if (curActiveIndex == 0)
			{
				return this._carnivalDataModule.ActiveOneItems[curActiveIndex].ActiveNeed;
			}
			return this._carnivalDataModule.ActiveOneItems[curActiveIndex].ActiveNeed - this._carnivalDataModule.ActiveOneItems[curActiveIndex - 1].ActiveNeed;
		}

		[SerializeField]
		private Transform ActivePos;

		[SerializeField]
		private CustomText TotalActive;

		[SerializeField]
		private Slider ActiveProgrsss;

		[SerializeField]
		private List<Transform> RewardBoxRoots;

		[SerializeField]
		private GameObject BoxItem;

		[SerializeField]
		private GameObject EquipOne;

		[Header("延迟播放进度动画配置")]
		[SerializeField]
		private bool m_isPlayingProgress;

		[SerializeField]
		private bool m_isPlayingDelay;

		[SerializeField]
		private int m_fromActive;

		[SerializeField]
		private int m_toActive;

		[SerializeField]
		private float m_timeProgress;

		[SerializeField]
		private float m_durationProgress = 0.8f;

		[SerializeField]
		private float m_delayTime;

		[SerializeField]
		private float m_delayMaxTime = 0.7f;

		private List<GameObject> _activeItems = new List<GameObject>();

		private SevenDayCarnivalDataModule _carnivalDataModule;

		public Action<int, int> OnSendGetActive;

		private float progressTotalW;

		private float progressPerRewardW;
	}
}
