using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class CommonUseTipViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.addButton.onClick.AddListener(new UnityAction(this.AddCountOne));
			this.subButton.onClick.AddListener(new UnityAction(this.SubCountOne));
			this.maxButtonTen.onClick.AddListener(new UnityAction(this.OnClickMaxBtn));
			this.Button_Buy.m_onClick = new Action(this.OnClickUse);
			this.ItemDefault.Init();
		}

		public override void OnOpen(object data)
		{
			this.isClickBuy = false;
			this.openData = (CommonUseTipViewModule.OpenData)data;
			this.dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.m_talentLegacyDataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			this.itemTable = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(this.openData.ItemId);
			if (this.itemTable == null)
			{
				this.CloseSelf();
			}
			this.SetMaxUseCount();
			this.SetBuyCount((int)this.m_maxCount, false);
			this.RefreshView();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.Button_Buy.m_onClick = null;
			this.ItemDefault.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickUse()
		{
			if (this.dataModule.GetItemDataCountByid((ulong)((long)this.openData.ItemId)) <= 0L)
			{
				GameApp.View.ShowItemNotEnoughTip(this.openData.ItemId, true);
				return;
			}
			Action<int> callBack = this.openData.CallBack;
			if (callBack == null)
			{
				return;
			}
			callBack(this.curBuyCount);
		}

		private void SetMaxUseCount()
		{
			long itemDataCountByid = this.dataModule.GetItemDataCountByid((ulong)((long)this.openData.ItemId));
			long num;
			if (this.openData.MaxCount <= 0)
			{
				num = itemDataCountByid;
			}
			else if ((long)this.openData.MaxCount > itemDataCountByid)
			{
				num = itemDataCountByid;
			}
			else
			{
				num = (long)this.openData.MaxCount;
			}
			if (num <= 0L)
			{
				num = 1L;
			}
			this.m_maxCount = num;
		}

		private void RefreshView()
		{
			if (string.IsNullOrEmpty(this.openData.TitleStr))
			{
				this.Text_Title.text = Singleton<LanguageManager>.Instance.GetInfoByID("Global_Buy");
			}
			else
			{
				this.Text_Title.text = this.openData.TitleStr;
			}
			this.Text_BuyButtonTitle.text = this.openData.BuyButtonTitle;
			PropData propData = new PropData();
			propData.id = (uint)this.openData.ItemId;
			propData.count = (ulong)this.dataModule.GetItemDataCountByid((ulong)((long)this.openData.ItemId));
			this.ItemDefault.SetData(propData);
			this.ItemDefault.OnRefresh();
			this.RefreshBuyCount();
		}

		private void RefreshBuyCount()
		{
			this.dataModule.GetItemDataCountByid((ulong)((long)this.openData.ItemId));
			this.buyCountText.text = this.curBuyCount.ToString();
			int num = Singleton<GameConfig>.Instance.TalentLegacySpeedItemTime * this.curBuyCount;
			this.buyItemInfo.text = HLog.StringBuilderFormat(Singleton<LanguageManager>.Instance.GetInfoByID("legacy_study_can_time"), DxxTools.FormatFullTime((long)num));
			this.OnRefreshMaxBtn();
		}

		private void OnRefreshMaxBtn()
		{
			this.maxButtonTen.gameObject.SetActiveSafe((long)this.curBuyCount < this.m_maxCount);
		}

		private int GetName()
		{
			return 1028;
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.CloseSelf();
			}
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(this.GetName(), null);
		}

		private void AddCountOne()
		{
			this.AddCount(1, true);
		}

		private void SubCountOne()
		{
			this.AddCount(-1, true);
		}

		private void OnClickMaxBtn()
		{
			this.AddCount((int)this.m_maxCount - this.curBuyCount, true);
		}

		private void AddCount(int count, bool isShowTip = true)
		{
			this.SetBuyCount(this.curBuyCount + count, isShowTip);
			this.RefreshBuyCount();
		}

		private void SetBuyCount(int buyCount, bool showUpperLimitTip)
		{
			long itemDataCountByid = this.dataModule.GetItemDataCountByid((ulong)((long)this.openData.ItemId));
			if ((long)buyCount > this.m_maxCount)
			{
				this.curBuyCount = (int)this.m_maxCount;
				if (showUpperLimitTip)
				{
					if (itemDataCountByid <= 0L)
					{
						string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(this.itemTable.nameID);
						GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("tip_item_unenough", new object[] { infoByID }));
						return;
					}
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("legacy_study_use_limit_tip"));
					return;
				}
			}
			else
			{
				if (buyCount < 1)
				{
					this.curBuyCount = 1;
					return;
				}
				this.curBuyCount = buyCount;
			}
		}

		private const int MinBuyCount = 1;

		[SerializeField]
		private UIItem ItemDefault;

		[SerializeField]
		private UIPopCommon uiPopCommon;

		[SerializeField]
		private CustomButton addButton;

		[SerializeField]
		private CustomButton subButton;

		[SerializeField]
		private CustomButton maxButtonTen;

		[SerializeField]
		private CustomText buyCountText;

		[SerializeField]
		private CustomText Text_BuyButtonTitle;

		[SerializeField]
		private CustomText buyItemInfo;

		[SerializeField]
		private CustomButton Button_Buy;

		public CustomText Text_Title;

		private CommonUseTipViewModule.OpenData openData;

		private PropDataModule dataModule;

		private TalentLegacyDataModule m_talentLegacyDataModule;

		private Item_Item itemTable;

		private int curBuyCount;

		private bool isClickBuy;

		private long m_maxCount;

		public class OpenData
		{
			public string TitleStr;

			public int ItemId;

			public string BuyButtonTitle;

			public int MaxCount;

			public Action<int> CallBack;
		}
	}
}
