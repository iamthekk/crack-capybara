using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class ShopBuyConfirmViewModule : BaseViewModule
	{
		public ViewName GetName()
		{
			return ViewName.ShopBuyConfirmViewModule;
		}

		public override void OnCreate(object data)
		{
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.Button_Free.onClick.AddListener(new UnityAction(this.OnClickSure));
			this.Button_Buy.Init();
			this.Button_Buy.m_onClick = new Action(this.OnClickSure);
			this.Prefab_Item.SetActive(false);
		}

		public override void OnOpen(object data)
		{
			ShopBuyConfirmData shopBuyConfirmData = data as ShopBuyConfirmData;
			if (shopBuyConfirmData != null)
			{
				this.mData = shopBuyConfirmData;
				this.m_seqPool.Clear(false);
				this.mData.CheckDefault();
				this.Text_Title.text = this.mData.m_title;
				this.Button_Free.gameObject.SetActive(this.mData.IsFree);
				this.Button_Buy.gameObject.SetActive(!this.mData.IsFree);
				if (!this.mData.IsFree)
				{
					this.Button_Buy.SetItem(this.mData.m_costItem);
				}
				this.RefreshRewardItems();
				return;
			}
			HLog.LogError("PopCommonViewModule OnOpen data == null");
		}

		private void closeWindow()
		{
			GameApp.View.CloseView(this.GetName(), null);
		}

		public override void OnClose()
		{
			this.m_seqPool.Clear(false);
		}

		public override void OnDelete()
		{
			this.uiPopCommon.OnClick = null;
			CustomButton button_Free = this.Button_Free;
			if (button_Free != null)
			{
				button_Free.onClick.RemoveListener(new UnityAction(this.OnClickSure));
			}
			CustomButtonIconAndText button_Buy = this.Button_Buy;
			if (button_Buy != null)
			{
				button_Buy.DeInit();
			}
			this.RemoveAllRewards();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void RemoveAllRewards()
		{
			for (int i = 0; i < this.mItemUIList.Count; i++)
			{
				this.mItemUIList[i].DeInit();
				Object.Destroy(this.mItemUIList[i].gameObject);
			}
			this.mItemUIList.Clear();
		}

		private void RefreshRewardItems()
		{
			this.RemoveAllRewards();
			List<ItemData> rewards = this.mData.m_rewards;
			int num = rewards.Count / 4;
			if (rewards.Count % 4 != 0)
			{
				num++;
			}
			float num2 = (float)(num * 160 + (num - 1) * 20);
			this.uiPopCommon.SetRtfHeightByOffset(num2);
			for (int i = 0; i < rewards.Count; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.Prefab_Item, this.RTF_ItemRoot);
				UIItem component = gameObject.GetComponent<UIItem>();
				component.Init();
				gameObject.SetActive(true);
				component.SetData(rewards[i].ToPropData());
				component.OnRefresh();
				this.mItemUIList.Add(component);
				RectTransform rectTransform = component.gameObject.transform as RectTransform;
				rectTransform.localScale = Vector3.zero;
				Sequence sequence = this.m_seqPool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, (float)i * 0.05f);
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(rectTransform, 1f, 0.3f));
			}
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickClose();
			}
		}

		private void OnClickClose()
		{
			this.closeWindow();
			Action onCancel = this.mData.m_onCancel;
			if (onCancel == null)
			{
				return;
			}
			onCancel();
		}

		private void OnClickSure()
		{
			this.closeWindow();
			Action onSure = this.mData.m_onSure;
			if (onSure == null)
			{
				return;
			}
			onSure();
		}

		[GameTestMethod]
		private static void OnTest()
		{
			ShopBuyConfirmData shopBuyConfirmData = new ShopBuyConfirmData();
			shopBuyConfirmData.SetRewards(new List<ItemData>
			{
				new ItemData(1, 9999L),
				new ItemData(4, 666L),
				new ItemData(2030, 3L)
			});
			shopBuyConfirmData.SetCost(2, 648);
			GameApp.View.OpenView(ViewName.ShopBuyConfirmViewModule, shopBuyConfirmData, 2, null, null);
		}

		public const int ItemSize_Y = 160;

		public const int ItemSize_Spacing_Y = 20;

		public CustomText Text_Title;

		public UIPopCommon uiPopCommon;

		public CustomButton Button_Free;

		public CustomButtonIconAndText Button_Buy;

		public RectTransform RTF_ItemRoot;

		public GameObject Prefab_Item;

		private List<UIItem> mItemUIList = new List<UIItem>();

		private SequencePool m_seqPool = new SequencePool();

		private ShopBuyConfirmData mData;
	}
}
