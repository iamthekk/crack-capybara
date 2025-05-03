using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class IAPFundItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.copyItem.SetActiveSafe(false);
			this.maskObj.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			this.DeleteItems();
		}

		private void DeleteItems()
		{
			for (int i = 0; i < this.freeRewardItems.Count; i++)
			{
				GameObject gameObject = this.freeRewardItems[i].gameObject;
				this.freeRewardItems[i].DeInit();
				if (gameObject)
				{
					Object.Destroy(gameObject);
				}
			}
			this.freeRewardItems.Clear();
			for (int j = 0; j < this.payRewardItems.Count; j++)
			{
				GameObject gameObject2 = this.payRewardItems[j].gameObject;
				this.payRewardItems[j].DeInit();
				if (gameObject2)
				{
					Object.Destroy(gameObject2);
				}
			}
			this.payRewardItems.Clear();
		}

		public void SetData(IAPLevelFundData data, IAPLevelFundGroup group, Action onClickReward)
		{
			this.fundData = data;
			this.fundGroup = group;
			this.OnClickReward = onClickReward;
		}

		public void Refresh(SequencePool m_seqPool)
		{
			if (this.fundData == null || this.fundGroup == null)
			{
				return;
			}
			bool flag = this.fundData.CheckArrive();
			this.maskObj.SetActive(!flag);
			if (this.fundGroup.GroupKind == IAPLevelFundGroupKind.TowerLevel)
			{
				TowerDataModule dataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
				int intParam = this.fundData.IntParam;
				TowerChallenge_Tower towerConfigByLevelId = dataModule.GetTowerConfigByLevelId(intParam);
				int towerConfigNum = dataModule.GetTowerConfigNum(towerConfigByLevelId);
				int levelNumByLevelId = dataModule.GetLevelNumByLevelId(intParam);
				string text = string.Format("{0}-{1}", towerConfigNum, levelNumByLevelId);
				this.textUnlock.text = Singleton<LanguageManager>.Instance.GetInfoByID("uifundtower_level", new object[] { text });
			}
			else if (this.fundGroup.GroupKind == IAPLevelFundGroupKind.RogueDungeonFloor)
			{
				this.textUnlock.text = Singleton<LanguageManager>.Instance.GetInfoByID("uifundtower_level", new object[] { this.fundData.IntParam });
			}
			else
			{
				this.textUnlock.text = Singleton<LanguageManager>.Instance.GetInfoByID("uifundtalent_level", new object[] { this.fundData.IntParam });
			}
			this.imageIcon.sprite = (flag ? this.spriteUnlock : this.spriteLock);
			this.RefreshProgressUI();
			if (this.freeRewardItems.Count != this.fundData.freeRewards.Count)
			{
				for (int i = 0; i < this.freeRewardItems.Count; i++)
				{
					GameObject gameObject = this.freeRewardItems[i].gameObject;
					this.freeRewardItems[i].DeInit();
					if (gameObject != null)
					{
						Object.Destroy(gameObject);
					}
				}
				this.freeRewardItems.Clear();
				for (int j = 0; j < this.fundData.freeRewards.Count; j++)
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.copyItem, this.freeRoot);
					IAPFundRewardItem component = gameObject2.GetComponent<IAPFundRewardItem>();
					component.item.SetCountShowType(UIItem.CountShowType.MissOne);
					gameObject2.SetActive(true);
					Transform transform = gameObject2.transform;
					if (m_seqPool != null)
					{
						transform.localScale = Vector3.zero;
						TweenSettingsExtensions.Append(m_seqPool.Get(), ShortcutExtensions.DOScale(transform, 1f, 0.3f));
					}
					else
					{
						transform.localScale = Vector3.one;
					}
					this.freeRewardItems.Add(component);
					gameObject2.SetActiveSafe(true);
				}
			}
			for (int k = 0; k < this.fundData.freeRewards.Count; k++)
			{
				IAPFundRewardItem iapfundRewardItem = this.freeRewardItems[k];
				iapfundRewardItem.SetData(this.fundData.freeRewards[k]);
				iapfundRewardItem.Init();
				iapfundRewardItem.Refresh();
				iapfundRewardItem.SetState(this.fundData.freeRewardState);
				iapfundRewardItem.item.onClick = new Action<UIItem, PropData, object>(this.OnClickItemFree);
			}
			if (this.payRewardItems.Count != this.fundData.payRewards.Count)
			{
				for (int l = 0; l < this.payRewardItems.Count; l++)
				{
					GameObject gameObject3 = this.payRewardItems[l].gameObject;
					this.payRewardItems[l].DeInit();
					if (gameObject3 != null)
					{
						Object.Destroy(gameObject3);
					}
				}
				this.payRewardItems.Clear();
				for (int m = 0; m < this.fundData.payRewards.Count; m++)
				{
					GameObject gameObject4 = Object.Instantiate<GameObject>(this.copyItem, this.payRoot);
					IAPFundRewardItem component2 = gameObject4.GetComponent<IAPFundRewardItem>();
					component2.item.SetCountShowType(UIItem.CountShowType.MissOne);
					Transform transform2 = gameObject4.transform;
					if (m_seqPool != null)
					{
						transform2.localScale = Vector3.zero;
						TweenSettingsExtensions.Append(m_seqPool.Get(), ShortcutExtensions.DOScale(transform2, 1f, 0.3f));
					}
					else
					{
						transform2.localScale = Vector3.one;
					}
					this.payRewardItems.Add(component2);
					gameObject4.SetActiveSafe(true);
				}
			}
			for (int n = 0; n < this.fundData.payRewards.Count; n++)
			{
				IAPFundRewardItem iapfundRewardItem2 = this.payRewardItems[n];
				iapfundRewardItem2.SetData(this.fundData.payRewards[n]);
				iapfundRewardItem2.Init();
				iapfundRewardItem2.Refresh();
				iapfundRewardItem2.SetState(this.fundData.payRewardState);
				iapfundRewardItem2.item.onClick = new Action<UIItem, PropData, object>(this.OnClickItemPay);
			}
		}

		private void RefreshProgressUI()
		{
			float height = this.progressBg.rect.height;
			float num = (this.fundData.CheckArrive() ? 0f : height);
			Utility.SetBottom(this.progressFg, num);
		}

		private void OnClickItemFree(UIItem item, PropData data, object obj)
		{
			if (this.fundData.freeRewardState != IAPLevelFund.RewardState.CanCollect)
			{
				item.OnBtnItemClick(item, data, obj);
				return;
			}
			this.CollectReward();
		}

		private void OnClickItemPay(UIItem item, PropData data, object obj)
		{
			if (this.fundData.payRewardState != IAPLevelFund.RewardState.CanCollect)
			{
				item.OnBtnItemClick(item, data, obj);
				return;
			}
			this.CollectReward();
		}

		private void CollectReward()
		{
			Action onClickReward = this.OnClickReward;
			if (onClickReward == null)
			{
				return;
			}
			onClickReward();
		}

		public GameObject copyItem;

		public RectTransform freeRoot;

		public RectTransform payRoot;

		public CustomText textUnlock;

		public Image imageIcon;

		public Sprite spriteLock;

		public Sprite spriteUnlock;

		public RectTransform progressFg;

		public RectTransform progressBg;

		public GameObject maskObj;

		private List<IAPFundRewardItem> freeRewardItems = new List<IAPFundRewardItem>();

		private List<IAPFundRewardItem> payRewardItems = new List<IAPFundRewardItem>();

		private IAPLevelFundData fundData;

		private IAPLevelFundGroup fundGroup;

		private Action OnClickReward;
	}
}
