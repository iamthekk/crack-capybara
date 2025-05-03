using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class PetQuickStarUpgradeResultViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.btnTapToClose.OnClose = new Action(this.OnBtnCloseClick);
			this.btnTapToClose.gameObject.SetActive(false);
			this.prefabRewardItem.gameObject.SetActive(false);
			RectTransform rectTransform = this.prefabRewardItem.rectTransform;
			this.ItemWidth = rectTransform.sizeDelta.x;
			this.ItemHeight = rectTransform.sizeDelta.y;
			this.remainMoveY = 0f;
			this.remainMoveFrameCount = 0;
			this.scrollRect.vertical = false;
			this.titleUIBgText.m_text.text = Singleton<LanguageManager>.Instance.GetInfoByID("Common_Reward");
		}

		public override void OnOpen(object data)
		{
			this.openData = data as PetQuickStarUpgradeResultViewModule.OpenData;
			this.ShowRewards();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
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

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.btnTapToClose.OnClose = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(this, null);
		}

		public void ShowRewards()
		{
			this.itemDatas = this.openData.itemList;
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
				if (row > 3)
				{
					this.remainMoveY += this.ItemHeight;
					this.remainMoveFrameCount += 5;
				}
				for (int j = 0; j < count; j = num + 1)
				{
					scale -= perSpace;
					scale = Mathf.Max(scale, perSpace);
					ItemData itemData = this.itemDatas[i + j];
					if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemData.ID) == null)
					{
						HLog.LogError(string.Format("itemId:{0} not found in item config", itemData.ID));
					}
					else
					{
						PetQuickStarUpgradeItem rewardItem = Object.Instantiate<PetQuickStarUpgradeItem>(this.prefabRewardItem, this.prefabRewardItem.transform.parent, false);
						rewardItem.gameObject.SetActive(true);
						rewardItem.Init();
						int num2;
						this.openData.oldStarData.TryGetValue(itemData.ID, out num2);
						int num3;
						this.openData.newStarData.TryGetValue(itemData.ID, out num3);
						rewardItem.SetData(itemData, num2, num3);
						RectTransform rectTransform = rewardItem.rectTransform;
						rectTransform.anchoredPosition = new Vector2(offsetX + (float)j * this.ItemWidth, -posY);
						rewardItem.SetImageWhiteAlpha(1f);
						rectTransform.localScale = Vector3.one * scale;
						TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DOScale(rectTransform, Vector3.one, 0.1f), delegate
						{
							ShortcutExtensions46.DOFade(rewardItem.imgWhite, 0f, 0.2f);
						});
						yield return 0;
					}
					num = j;
				}
			}
			this.scrollRect.vertical = true;
			this.btnTapToClose.gameObject.SetActive(true);
			yield break;
		}

		private void AutoSetViewSize()
		{
			int num = Mathf.CeilToInt((float)this.itemDatas.Count / 5f);
			float num2 = (float)num * this.ItemHeight + this.itemPaddingTop + this.itemPaddingBottom;
			float num3;
			if (num + 1 > 3)
			{
				this.scrollRect.vertical = true;
				num3 = 3f * this.ItemHeight + this.itemPaddingTop;
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

		public UIBgText titleUIBgText;

		public TapToCloseCtrl btnTapToClose;

		public RectTransform rectNode;

		public RectTransform rectScrollNode;

		public RectTransform rectContent;

		public CustomScrollRect scrollRect;

		public float rectTopSpace = 80f;

		public float rectBottomSpace = 70f;

		public float itemPaddingTop;

		public float itemPaddingBottom;

		public PetQuickStarUpgradeItem prefabRewardItem;

		private const int LineCount = 5;

		private const int MaxLine = 3;

		private float ItemWidth;

		private float ItemHeight;

		private List<ItemData> itemDatas;

		private int remainMoveFrameCount;

		private int curMaxLine;

		private float remainMoveY;

		private PetQuickStarUpgradeResultViewModule.OpenData openData;

		public class OpenData
		{
			public List<ItemData> itemList;

			public Dictionary<int, int> oldStarData;

			public Dictionary<int, int> newStarData;
		}
	}
}
