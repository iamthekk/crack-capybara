using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class PetOpenEggViewModule_PagePetDrawRewardShow : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.uiShake.Init();
			this.btnTapToClose.OnClose = new Action(this.OnBtnCloseClick);
			this.btnDrawAgain.m_onClick = new Action(this.OnBtnAgainClick);
			this.Button_Skip.m_onClick = new Action(this.OnClickSkip);
			this.btnTapToClose.gameObject.SetActive(false);
			this.btnDrawAgain.gameObject.SetActive(false);
			this.Button_Skip.gameObject.SetActiveSafe(false);
			this.prefabRewardItem.gameObject.SetActive(false);
			PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this.SkipSelectObj.SetActiveSafe(dataModule.IsSkipAni);
			RectTransform rectTransform = this.prefabRewardItem.rectTransform;
			this.ItemWidth = rectTransform.sizeDelta.x;
			this.ItemHeight = rectTransform.sizeDelta.y;
			this.remainMoveY = 0f;
			this.remainMoveFrameCount = 0;
			this.scrollRect.vertical = false;
			this.uiBgText.m_text.text = Singleton<LanguageManager>.Instance.GetInfoByID("Common_Reward");
		}

		protected override void OnDeInit()
		{
			this.uiShake.DeInit();
			this.btnTapToClose.OnClose = null;
			this.btnDrawAgain.m_onClick = null;
			this.Button_Skip.m_onClick = null;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.uiShake.OnUpdate(deltaTime, unscaledDeltaTime);
			if (this.remainMoveFrameCount > 1)
			{
				this.remainMoveFrameCount--;
				float num = this.remainMoveY / (float)this.remainMoveFrameCount;
				this.remainMoveY -= num;
				Vector2 anchoredPosition = this.rectContent.anchoredPosition;
				anchoredPosition.y += num;
				this.rectContent.anchoredPosition = anchoredPosition;
			}
		}

		public void ShowRewards(EPetBoxType petBoxType, List<ItemData> itemDatas, Action onShowRewardsCallback)
		{
			this.petBoxType = petBoxType;
			this.itemDatas = itemDatas;
			this.onShowRewardsCallback = onShowRewardsCallback;
			this.AutoSetViewSize();
			base.StartCoroutine(this.PlayRewardsAnimation());
		}

		private IEnumerator PlayRewardsAnimation()
		{
			this.scrollRect.vertical = false;
			int row = 0;
			this.remainMoveY = 0f;
			this.remainMoveFrameCount = 0;
			int perLineDelayFrame = 2;
			for (int i = 0; i < this.itemDatas.Count; i += 5)
			{
				int num = row;
				row = num + 1;
				int count = Mathf.Min(5, this.itemDatas.Count - i);
				if (count <= 0)
				{
					break;
				}
				if (row > 1)
				{
					for (int j = 0; j < perLineDelayFrame; j = num + 1)
					{
						yield return 0;
						num = j;
					}
				}
				float offsetX = -2f * this.ItemWidth;
				float posY = this.itemPaddingTop + ((float)row - 0.5f) * this.ItemHeight;
				float scale = 1f;
				float perSpace = 0.2f;
				if (row > 5)
				{
					this.remainMoveY += this.ItemHeight;
					this.remainMoveFrameCount += 5;
				}
				for (int j = 0; j < count; j = num + 1)
				{
					PetOpenEggViewModule_PagePetDrawRewardShow.<>c__DisplayClass33_0 CS$<>8__locals1 = new PetOpenEggViewModule_PagePetDrawRewardShow.<>c__DisplayClass33_0();
					scale -= perSpace;
					scale = Mathf.Max(scale, perSpace);
					ItemData itemData = this.itemDatas[i + j];
					Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID);
					if (elementById == null)
					{
						HLog.LogError(string.Format("itemId:{0} not found in item config", itemData.ID));
					}
					else
					{
						bool needShake = elementById.itemType == 21;
						if (this.openShake && needShake)
						{
							yield return new WaitForSeconds(0.4f);
							this.uiShake.Shake(UIShake.ShakeType.Random, 0f, 0.3f, 15f, 5);
						}
						CS$<>8__locals1.rewardItem = Object.Instantiate<OpenCheckShowView_ChestRewardItem>(this.prefabRewardItem, this.prefabRewardItem.transform.parent, false);
						CS$<>8__locals1.rewardItem.gameObject.SetActive(true);
						CS$<>8__locals1.rewardItem.Init();
						CS$<>8__locals1.rewardItem.SetData(itemData);
						RectTransform rectTransform = CS$<>8__locals1.rewardItem.rectTransform;
						rectTransform.anchoredPosition = new Vector2(offsetX + (float)j * this.ItemWidth, -posY);
						CS$<>8__locals1.rewardItem.SetImageWhiteAlpha(1f);
						rectTransform.localScale = Vector3.one * scale;
						TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DOScale(rectTransform, Vector3.one, 0.1f), delegate
						{
							ShortcutExtensions46.DOFade(CS$<>8__locals1.rewardItem.imgWhite, 0f, 0.2f);
						});
						if (this.openShake && needShake)
						{
							yield return new WaitForSeconds(0.6f);
						}
						yield return 0;
						CS$<>8__locals1 = null;
						itemData = null;
					}
					num = j;
				}
			}
			this.scrollRect.vertical = true;
			this.btnTapToClose.gameObject.SetActive(true);
			this.btnDrawAgain.gameObject.SetActive(true);
			this.Button_Skip.gameObject.SetActiveSafe(true);
			this.RefreshDrawAgainCost();
			yield break;
		}

		public void AutoSetViewSize()
		{
			int num = Mathf.CeilToInt((float)this.itemDatas.Count / 5f);
			float num2 = (float)num * this.ItemHeight + this.itemPaddingTop + this.itemPaddingBottom;
			float num3;
			if (num + 1 > 5)
			{
				this.scrollRect.vertical = true;
				num3 = 5f * this.ItemHeight + this.itemPaddingTop;
			}
			else
			{
				this.scrollRect.vertical = false;
				num3 = (float)num * this.ItemHeight + this.itemPaddingTop + this.itemPaddingBottom;
			}
			this.rectContent.sizeDelta = new Vector2(this.rectContent.sizeDelta.x, num2);
			float num4 = num3 + this.rectTopSpace + this.rectBottomSpace;
			this.rectScrollNode.sizeDelta = new Vector2(this.rectScrollNode.sizeDelta.x, num3);
			this.rectNode.sizeDelta = new Vector2(this.rectNode.sizeDelta.x, num4);
		}

		private void OnBtnCloseClick()
		{
			Action action = this.onShowRewardsCallback;
			if (action == null)
			{
				return;
			}
			action();
		}

		private void OnClickSkip()
		{
			PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			dataModule.IsSkipAni = !dataModule.IsSkipAni;
			this.SkipSelectObj.SetActiveSafe(dataModule.IsSkipAni);
		}

		private void OnBtnAgainClick()
		{
			this.btnDrawAgain.gameObject.SetActive(false);
			this.Button_Skip.gameObject.SetActiveSafe(false);
			if (this.petBoxType == EPetBoxType.AdDraw)
			{
				PetOpenEggHelper.OnPetBoxFreeClick(this.petBoxType);
				return;
			}
			PetOpenEggHelper.OnPetBoxAdvanceClickImpl(this.petBoxType);
		}

		private void RefreshDrawAgainCost()
		{
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			if (this.petBoxType == EPetBoxType.AdDraw)
			{
				this.btnDrawAgainText.text = Singleton<LanguageManager>.Instance.GetInfoByID("common_free");
				AdData adData = GameApp.Data.GetDataModule(DataName.AdDataModule).GetAdData(8);
				int remainTimes = adData.GetRemainTimes();
				this.txtAdCost.text = remainTimes.ToString() + "/" + adData.watchCountMax.ToString();
				this.drawAgainCost.gameObject.SetActive(false);
				this.txtAdCost.gameObject.SetActive(true);
				this.btnDrawAgain.gameObject.SetActive(remainTimes > 0);
				return;
			}
			this.btnDrawAgainText.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_draw_again");
			this.drawAgainCost.gameObject.SetActive(true);
			this.txtAdCost.gameObject.SetActive(false);
			this.btnDrawAgain.gameObject.SetActive(true);
			long itemDataCountByid = dataModule.GetItemDataCountByid(11UL);
			long itemDataCountByid2 = dataModule.GetItemDataCountByid(2UL);
			int num;
			int num2;
			if (this.petBoxType == EPetBoxType.Draw35)
			{
				num = Singleton<GameConfig>.Instance.Pet35DrawTicketCost;
				num2 = Singleton<GameConfig>.Instance.Pet35DrawDiamondCost;
			}
			else
			{
				num = Singleton<GameConfig>.Instance.Pet15DrawTicketCost;
				num2 = Singleton<GameConfig>.Instance.Pet15DrawDiamondCost;
			}
			ItemData itemData = new ItemData();
			if (itemDataCountByid >= (long)num)
			{
				itemData.SetID(11);
				itemData.SetCount((long)num);
				this.drawAgainCost.SetData(itemData, itemDataCountByid, itemData.TotalCount);
				return;
			}
			itemData.SetID(2);
			itemData.SetCount((long)num2);
			this.drawAgainCost.SetData(itemData, itemDataCountByid2, itemData.TotalCount);
		}

		public UIBgText uiBgText;

		public CustomButton btnDrawAgain;

		public CustomText btnDrawAgainText;

		public CommonCostItem drawAgainCost;

		public CustomText txtAdCost;

		public bool openShake = true;

		public UIShake uiShake;

		public TapToCloseCtrl btnTapToClose;

		public RectTransform rectNode;

		public RectTransform rectScrollNode;

		public RectTransform rectContent;

		public CustomScrollRect scrollRect;

		public float rectTopSpace = 80f;

		public float rectBottomSpace = 70f;

		public float itemPaddingTop;

		public float itemPaddingBottom;

		public OpenCheckShowView_ChestRewardItem prefabRewardItem;

		public CustomButton Button_Skip;

		public GameObject SkipSelectObj;

		private const int LineCount = 5;

		private const int MaxLine = 5;

		private float ItemWidth;

		private float ItemHeight;

		private List<ItemData> itemDatas;

		private Action onShowRewardsCallback;

		private EPetBoxType petBoxType;

		private int remainMoveFrameCount;

		private int curMaxLine;

		private float remainMoveY;
	}
}
