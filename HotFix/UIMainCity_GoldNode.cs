using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Proto.Actor;
using UnityEngine;

namespace HotFix
{
	public class UIMainCity_GoldNode : UIBaseMainCityNode
	{
		public override int FunctionOpenID
		{
			get
			{
				return 23;
			}
		}

		public override MainCityName Name
		{
			get
			{
				return MainCityName.Gold;
			}
		}

		public override string RedName
		{
			get
			{
				return "Main.Gold";
			}
		}

		public override int NameLanguageID
		{
			get
			{
				return 6402;
			}
		}

		public override string NameLanguageIDStr
		{
			get
			{
				return "";
			}
		}

		protected override void OnInit()
		{
			this.m_mainCityDataModule = GameApp.Data.GetDataModule(DataName.MainCityDataModule);
			base.OnInit();
			this.m_itemDatas = this.m_mainCityDataModule.GetItemDatasForGold(out this.m_isFull, out this.m_isUpdate, out this.m_isHaveReward, out this.m_span);
			this.m_iconNode.SetActive(false);
			foreach (KeyValuePair<int, ItemData> keyValuePair in this.m_itemDatas)
			{
				if (keyValuePair.Value != null)
				{
					UIMainCityGoldNodeItem uimainCityGoldNodeItem = Object.Instantiate<UIMainCityGoldNodeItem>(this.m_iconNode);
					uimainCityGoldNodeItem.transform.SetParentNormal(this.m_parent, false);
					uimainCityGoldNodeItem.SetActive(true);
					uimainCityGoldNodeItem.SetItemID(keyValuePair.Key);
					uimainCityGoldNodeItem.Init();
					this.m_items[keyValuePair.Key] = uimainCityGoldNodeItem;
				}
			}
			this.m_textItem.Init();
			this.m_time = 0f;
			this.isPlaying = false;
		}

		public override void OnShow()
		{
			base.OnShow();
			this.OnRefreshUI();
			this.isPlaying = true;
			this.m_time = 0f;
			GameApp.Event.RegisterEvent(LocalMessageName.CC_MainCityData_RefreshMainCityGoldData, new HandlerEvent(this.OnEventRefreshMainCityGoldData));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			if (!this.isPlaying)
			{
				return;
			}
			this.m_time += deltaTime;
			if (this.m_time >= 5f)
			{
				this.m_time = 0f;
				this.OnRefreshUI();
			}
		}

		public override void OnHide()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_MainCityData_RefreshMainCityGoldData, new HandlerEvent(this.OnEventRefreshMainCityGoldData));
			base.OnHide();
			this.isPlaying = false;
		}

		protected override void OnDeInit()
		{
			base.OnDeInit();
			foreach (KeyValuePair<int, UIMainCityGoldNodeItem> keyValuePair in this.m_items)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_items.Clear();
			this.m_textItem.DeInit();
		}

		protected override void OnClickUnlockBt()
		{
			base.OnClickUnlockBt();
			if (this.m_isUpdate || !this.m_isHaveReward || !this.m_isShowReward)
			{
				GameApp.View.OpenView(ViewName.MainCityGoldLevelUpViewModule, null, 1, null, null);
				return;
			}
			NetworkUtils.MainCity.DoCityGoldmineHangRewardRequest(delegate(bool isOk, CityGoldmineHangRewardResponse req)
			{
				if (!isOk)
				{
					return;
				}
				GameApp.View.FlyItemDatas(FlyItemModel.Default, req.CommonData.Reward.ToItemDatas(), null, null);
			});
		}

		private void OnRefreshUI()
		{
			this.m_itemDatas = this.m_mainCityDataModule.GetItemDatasForGold(out this.m_isFull, out this.m_isUpdate, out this.m_isHaveReward, out this.m_span);
			this.m_isShowReward = this.m_mainCityDataModule.IsInShowTime(this.m_span) && !this.m_isLock;
			if (!this.m_isUpdate && !this.m_isFull && (!this.m_isHaveReward || !this.m_isShowReward))
			{
				this.m_parent.gameObject.SetActive(false);
				return;
			}
			RedPointController.Instance.ReCalc("Main.Gold", true);
			if (this.m_isUpdate)
			{
				this.m_parent.gameObject.SetActive(true);
				foreach (KeyValuePair<int, UIMainCityGoldNodeItem> keyValuePair in this.m_items)
				{
					if (!(keyValuePair.Value == null))
					{
						keyValuePair.Value.SetActive(false);
					}
				}
				this.m_parentAnimator.speed = 2f;
				this.m_textItem.SetText(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6501));
				this.m_textItem.SetActive(true);
				return;
			}
			if (this.m_isFull)
			{
				this.m_parent.gameObject.SetActive(true);
				foreach (KeyValuePair<int, UIMainCityGoldNodeItem> keyValuePair2 in this.m_items)
				{
					if (!(keyValuePair2.Value == null))
					{
						keyValuePair2.Value.SetActive(false);
					}
				}
				this.m_parentAnimator.speed = 2f;
				this.m_textItem.SetText(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6502));
				this.m_textItem.SetActive(true);
				return;
			}
			this.m_parent.gameObject.SetActive(this.m_isShowReward);
			this.m_textItem.SetActive(false);
			foreach (KeyValuePair<int, UIMainCityGoldNodeItem> keyValuePair3 in this.m_items)
			{
				if (!(keyValuePair3.Value == null))
				{
					ItemData itemData;
					this.m_itemDatas.TryGetValue(keyValuePair3.Key, out itemData);
					if (itemData == null)
					{
						keyValuePair3.Value.SetActive(false);
					}
					else if (itemData.TotalCount <= 0L)
					{
						keyValuePair3.Value.SetActive(false);
					}
					else
					{
						keyValuePair3.Value.SetItemID(itemData.ID);
						keyValuePair3.Value.SetText(itemData.TotalCount.ToString());
						keyValuePair3.Value.SetActive(true);
					}
				}
			}
			this.m_parentAnimator.speed = Mathf.Lerp(1f, 2f, this.m_mainCityDataModule.GetProgressForGold());
		}

		private void OnEventRefreshMainCityGoldData(object sender, int type, BaseEventArgs eventargs)
		{
			if (!(eventargs is EventArgsRefreshMainCityGoldData))
			{
				return;
			}
			this.OnRefreshUI();
		}

		public RectTransform m_parent;

		public Animator m_parentAnimator;

		public UIMainCityGoldNodeItem m_iconNode;

		public UIMainCityGoldNodeItem m_textItem;

		private const float m_duration = 5f;

		private float m_time;

		private bool isPlaying;

		public Dictionary<int, UIMainCityGoldNodeItem> m_items = new Dictionary<int, UIMainCityGoldNodeItem>();

		private MainCityDataModule m_mainCityDataModule;

		private bool m_isFull;

		private bool m_isUpdate;

		private bool m_isHaveReward;

		private long m_span;

		private bool m_isShowReward;

		private Dictionary<int, ItemData> m_itemDatas = new Dictionary<int, ItemData>();
	}
}
