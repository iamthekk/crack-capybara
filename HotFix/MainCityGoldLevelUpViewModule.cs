using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Proto.Actor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class MainCityGoldLevelUpViewModule : BaseViewModule
	{
		public int GetName()
		{
			return 310;
		}

		public override void OnCreate(object data)
		{
			this.m_mainCityDataModule = GameApp.Data.GetDataModule(DataName.MainCityDataModule);
			if (this.m_defaultItemLayoutHeight == 0f)
			{
				this.m_defaultItemLayoutHeight = this.m_itemLayout.rect.height;
			}
		}

		public override void OnOpen(object data)
		{
			this.m_popCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnPopClick);
			this.m_itemPrefab.gameObject.SetActive(false);
			this.RefreshUI();
			this.m_infoBtn.onClick.AddListener(new UnityAction(this.OnClickInfoBtn));
			this.m_levelUpBtn.onClick.AddListener(new UnityAction(this.OnClickLevelUpBtn));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_isPlaying)
			{
				this.m_time += unscaledDeltaTime;
				if (this.m_time > this.m_duration)
				{
					this.m_time = 0f;
				}
				long num;
				if (this.m_mainCityDataModule.IsFullForGold(out num))
				{
					this.m_unfullTxt.gameObject.SetActive(false);
					this.m_fullTxt.SetActive(true);
					this.m_isPlaying = false;
					return;
				}
				this.m_unfullTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6516, new object[] { DxxTools.FormatFullTime(num) });
			}
		}

		public override void OnClose()
		{
			this.m_infoBtn.onClick.RemoveListener(new UnityAction(this.OnClickInfoBtn));
			this.m_levelUpBtn.onClick.RemoveListener(new UnityAction(this.OnClickLevelUpBtn));
			foreach (KeyValuePair<int, MainCityGoldLevelUpItem> keyValuePair in this.m_items)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_items.Clear();
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void RefreshUI()
		{
			foreach (KeyValuePair<int, MainCityGoldLevelUpItem> keyValuePair in this.m_items)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					keyValuePair.Value.gameObject.SetActive(false);
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_itemLayout);
			this.m_items.Clear();
			long num;
			bool flag = this.m_mainCityDataModule.IsFullForGold(out num);
			bool flag2 = this.m_mainCityDataModule.IsFullLevelForGold();
			bool flag3 = this.m_mainCityDataModule.IsCanLevelUpForGold();
			if (flag)
			{
				this.m_unfullTxt.gameObject.SetActive(false);
				this.m_fullTxt.SetActive(true);
				this.m_isPlaying = false;
			}
			else
			{
				this.m_unfullTxt.gameObject.SetActive(true);
				this.m_fullTxt.SetActive(false);
				this.m_unfullTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6516, new object[] { DxxTools.FormatFullTime(num) });
				this.m_isPlaying = true;
				this.m_time = 0f;
			}
			this.m_titleTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6517, new object[] { this.m_mainCityDataModule.m_goldLevel });
			this.m_maxTimeTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6518, new object[] { DxxTools.FormatFullTime(this.m_mainCityDataModule.GetGoldMaxDuration()) });
			this.m_groupTitleTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6503);
			this.m_levelItem.RefreshUI(this.m_mainCityDataModule.m_goldLevel, this.m_mainCityDataModule.m_goldLevel + 1, flag2);
			if (flag2)
			{
				this.m_levelUpBtn.gameObject.SetActive(false);
				this.m_unlevelUpTxt.gameObject.SetActive(false);
				this.m_fullLevelTxt.SetActive(true);
			}
			else if (!flag3)
			{
				this.m_levelUpBtn.gameObject.SetActive(true);
				this.m_unlevelUpTxt.gameObject.SetActive(true);
				this.m_fullLevelTxt.SetActive(false);
				this.m_levelUpBtnGray.SetUIGray();
				this.m_unlevelUpTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6520, new object[] { this.m_mainCityDataModule.GetRequiredLevel() });
			}
			else
			{
				this.m_levelUpBtn.gameObject.SetActive(true);
				this.m_unlevelUpTxt.gameObject.SetActive(false);
				this.m_fullLevelTxt.SetActive(false);
			}
			Dictionary<int, ItemData> spanItemDatasForGold = this.m_mainCityDataModule.GetSpanItemDatasForGold(this.m_mainCityDataModule.m_goldLevel, 3600);
			Dictionary<int, ItemData> spanItemDatasForGold2 = this.m_mainCityDataModule.GetSpanItemDatasForGold(this.m_mainCityDataModule.m_goldLevel + 1, 3600);
			float num2 = 0f;
			if (!flag2)
			{
				this.m_levelItem.gameObject.SetActive(true);
				num2 += this.m_levelItem.rectTransform.rect.height;
				using (Dictionary<int, ItemData>.Enumerator enumerator2 = spanItemDatasForGold2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<int, ItemData> keyValuePair2 = enumerator2.Current;
						if (keyValuePair2.Value != null)
						{
							ItemData itemData;
							spanItemDatasForGold.TryGetValue(keyValuePair2.Key, out itemData);
							if (itemData != null && keyValuePair2.Value.TotalCount != itemData.TotalCount)
							{
								GameObject gameObject = Object.Instantiate<GameObject>(this.m_itemPrefab);
								gameObject.transform.SetParentNormal(this.m_itemLayout, false);
								gameObject.SetActive(true);
								MainCityGoldLevelUpItem component = gameObject.GetComponent<MainCityGoldLevelUpItem>();
								component.Init();
								component.RefreshUI(keyValuePair2.Key, itemData.TotalCount, keyValuePair2.Value.TotalCount, flag2);
								this.m_items[keyValuePair2.Key] = component;
								num2 += component.rectTransform.rect.height;
							}
						}
					}
					goto IL_04DE;
				}
			}
			this.m_levelItem.gameObject.SetActive(false);
			foreach (KeyValuePair<int, ItemData> keyValuePair3 in spanItemDatasForGold)
			{
				if (keyValuePair3.Value != null)
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.m_itemPrefab);
					gameObject2.transform.SetParentNormal(this.m_itemLayout, false);
					gameObject2.SetActive(true);
					MainCityGoldLevelUpItem component2 = gameObject2.GetComponent<MainCityGoldLevelUpItem>();
					component2.Init();
					component2.RefreshUI(keyValuePair3.Key, keyValuePair3.Value.TotalCount, keyValuePair3.Value.TotalCount, flag2);
					this.m_items[keyValuePair3.Key] = component2;
					num2 += component2.rectTransform.rect.height;
				}
			}
			IL_04DE:
			if (num2 > this.m_defaultItemLayoutHeight)
			{
				this.m_popCommon.SetRtfHeightByOffset(num2 - this.m_defaultItemLayoutHeight);
			}
			else
			{
				this.m_popCommon.SetRtfHeightByOffset(0f);
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_itemLayout);
		}

		private void OnPopClick(UIPopCommon.UIPopCommonClickType type)
		{
			this.OnClickBackBtn();
		}

		private void OnClickInfoBtn()
		{
			InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData();
			openData.m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6521);
			openData.m_contextInfo = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6522);
			GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
		}

		private void OnClickBackBtn()
		{
			GameApp.View.CloseView(ViewName.MainCityGoldLevelUpViewModule, null);
		}

		private void OnClickLevelUpBtn()
		{
			bool flag = this.m_mainCityDataModule.IsFullLevelForGold();
			bool flag2 = this.m_mainCityDataModule.IsCanLevelUpForGold();
			if (flag || !flag2)
			{
				return;
			}
			NetworkUtils.MainCity.DoCityGoldmineLevelUpRequest(delegate(bool isOk, CityGoldmineLevelUpResponse req)
			{
				if (!isOk)
				{
					return;
				}
				this.RefreshUI();
				if (this.m_levelItem != null)
				{
					this.m_levelItem.PlayParticleSystem();
				}
				foreach (KeyValuePair<int, MainCityGoldLevelUpItem> keyValuePair in this.m_items)
				{
					if (!(keyValuePair.Value == null))
					{
						keyValuePair.Value.PlayParticleSystem();
					}
				}
			});
		}

		[Header("Common")]
		public UIPopCommon m_popCommon;

		public CustomText m_titleTxt;

		public CustomButton m_infoBtn;

		[Header("挂机时间")]
		public CustomText m_unfullTxt;

		public GameObject m_fullTxt;

		public CustomText m_maxTimeTxt;

		[Header("升级属性变化")]
		public CustomText m_groupTitleTxt;

		public MainCityGoldLevelUpLevelItem m_levelItem;

		public RectTransform m_itemLayout;

		public GameObject m_itemPrefab;

		private float m_defaultItemLayoutHeight;

		[Header("Bottom")]
		public CustomButton m_levelUpBtn;

		public UIGrays m_levelUpBtnGray;

		public CustomText m_unlevelUpTxt;

		public GameObject m_fullLevelTxt;

		private float m_time;

		private bool m_isPlaying;

		private float m_duration = 1f;

		public const int HourTime = 3600;

		public Dictionary<int, MainCityGoldLevelUpItem> m_items = new Dictionary<int, MainCityGoldLevelUpItem>();

		private MainCityDataModule m_mainCityDataModule;
	}
}
