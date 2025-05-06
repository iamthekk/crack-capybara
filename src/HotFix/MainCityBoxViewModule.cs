using System;
using System.Collections.Generic;
using System.Linq;
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
	public class MainCityBoxViewModule : BaseViewModule
	{
		public int GetName()
		{
			return 311;
		}

		public override void OnCreate(object data)
		{
			this.m_mainCityDataModule = GameApp.Data.GetDataModule(DataName.MainCityDataModule);
			this.m_integral.Init();
			this.m_integral.m_onClickInegralBtn = new Action(this.OnClickInegralBtn);
			this.m_contentPrefab.SetActive(false);
			int boxMaxCount = this.m_mainCityDataModule.GetBoxMaxCount();
			for (int i = 0; i < boxMaxCount; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.m_contentPrefab);
				gameObject.transform.SetParentNormal(this.m_contentLayout, false);
				MainCityBoxContentNode component = gameObject.GetComponent<MainCityBoxContentNode>();
				component.SetIndex(i);
				component.SetOnClick(new Action<MainCityBoxContentNode>(this.OnClickContentNodeBt));
				component.Init();
				component.SetActive(true);
				this.m_nodes[component.GetObjectInstanceID()] = component;
			}
		}

		public override void OnOpen(object data)
		{
			this.m_seqPool.Clear(false);
			this.m_isPlaying = false;
			this.m_time = 0f;
			this.m_integral.OnViewOpen();
			this.m_popCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnPopClick);
			if (this.m_infoBtn != null)
			{
				this.m_infoBtn.onClick.AddListener(new UnityAction(this.OnClickInfoBtn));
			}
			if (this.m_oneClickReceiveBtn != null)
			{
				this.m_oneClickReceiveBtn.onClick.AddListener(new UnityAction(this.OnClickOneClickReceiveBtn));
			}
			this.OnRefresh(true);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_contentLayout);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_isPlaying)
			{
				this.m_time -= unscaledDeltaTime;
				if (this.m_time < 0f)
				{
					this.m_isPlaying = false;
					this.m_time = 0f;
					NetworkUtils.MainCity.DoCityGetChestInfoRequest(null);
					this.OnRefresh(false);
				}
				if (this.m_unFullTxt != null && this.m_unFullTxt.isActiveAndEnabled)
				{
					this.m_unFullTxt.text = DxxTools.FormatFullTime((long)this.m_time);
				}
			}
			if (this.m_integral != null)
			{
				this.m_integral.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void OnClose()
		{
			this.m_seqPool.Clear(false);
			if (this.m_infoBtn != null)
			{
				this.m_infoBtn.onClick.RemoveListener(new UnityAction(this.OnClickInfoBtn));
			}
			if (this.m_oneClickReceiveBtn != null)
			{
				this.m_oneClickReceiveBtn.onClick.RemoveListener(new UnityAction(this.OnClickOneClickReceiveBtn));
			}
		}

		public override void OnDelete()
		{
			this.m_integral.DeInit();
			foreach (KeyValuePair<int, MainCityBoxContentNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_nodes.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_MainCityData_RefreshMainCityBoxData, new HandlerEvent(this.OnEventRefreshMainCityBoxData));
			manager.RegisterEvent(LocalMessageName.CC_MainCityBoxView_RefreshIntegral, new HandlerEvent(this.OnEventRefreshIntegral));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_MainCityData_RefreshMainCityBoxData, new HandlerEvent(this.OnEventRefreshMainCityBoxData));
			manager.RegisterEvent(LocalMessageName.CC_MainCityBoxView_RefreshIntegral, new HandlerEvent(this.OnEventRefreshIntegral));
		}

		private void OnClickInegralBtn()
		{
			if (!this.m_mainCityDataModule.IsCanReceiveBoxIntegral())
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6515));
				return;
			}
			NetworkUtils.MainCity.DoCityTakeScoreRewardRequest(delegate(bool isOk, CityTakeScoreRewardResponse rep)
			{
				if (!isOk)
				{
					return;
				}
				DxxTools.UI.OpenRewardCommon(rep.CommonData.Reward, null, false);
			});
		}

		private void OnClickInfoBtn()
		{
			InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData();
			openData.m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6513);
			openData.m_contextInfo = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6514);
			GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
		}

		private void OnPopClick(UIPopCommon.UIPopCommonClickType type)
		{
			this.OnClickBackBtn();
		}

		private void OnClickBackBtn()
		{
			GameApp.View.CloseView(ViewName.MainCityBoxViewModule, null);
		}

		private void OnClickOneClickReceiveBtn()
		{
			Dictionary<ulong, MainCityBoxData> chestDatas = this.m_mainCityDataModule.m_chestDatas;
			if (chestDatas.Count <= 0)
			{
				return;
			}
			MainCityBoxRewardViewModule.OpenData openData = new MainCityBoxRewardViewModule.OpenData();
			openData.m_datas = new List<MainCityBoxData>();
			openData.m_datas.AddRange(chestDatas.Values.ToList<MainCityBoxData>());
			GameApp.View.OpenView(ViewName.MainCityBoxRewardViewModule, openData, 1, null, null);
		}

		private void OnClickContentNodeBt(MainCityBoxContentNode obj)
		{
			if (!this.m_mainCityDataModule.IsHaveBox())
			{
				return;
			}
			MainCityBoxRewardViewModule.OpenData openData = new MainCityBoxRewardViewModule.OpenData();
			openData.m_datas = new List<MainCityBoxData>();
			openData.m_datas.Add(obj.m_data);
			GameApp.View.OpenView(ViewName.MainCityBoxRewardViewModule, openData, 1, null, null);
		}

		private void OnEventRefreshMainCityBoxData(object sender, int type, BaseEventArgs eventargs)
		{
			if (!(eventargs is EventArgsRefreshMainCityBoxData))
			{
				return;
			}
			this.OnRefresh(false);
		}

		private void OnEventRefreshIntegral(object sender, int type, BaseEventArgs eventargs)
		{
			if (this.m_integral != null)
			{
				this.m_integral.OnEventRefreshIntergral();
			}
		}

		public void OnRefresh(bool isInit = true)
		{
			this.m_seqPool.Clear(false);
			bool flag = this.m_mainCityDataModule.IsBoxFull();
			this.m_unFullTxt.gameObject.SetActive(!flag);
			this.m_fullTxt.SetActive(flag);
			List<int> list;
			int boxLoopIndex;
			long num;
			if (this.m_mainCityDataModule.IsBoxLoop(out list, out boxLoopIndex, out num))
			{
				boxLoopIndex = this.m_mainCityDataModule.GetBoxLoopIndex(out list, out num);
				this.m_duration = (float)list[boxLoopIndex];
			}
			else
			{
				this.m_duration = (float)list[boxLoopIndex];
			}
			this.m_time = (float)num;
			this.m_isPlaying = true;
			if (!flag)
			{
				this.m_unFullTxt.text = DxxTools.FormatTime((long)this.m_time);
			}
			if (this.m_mainCityDataModule.IsHaveBox())
			{
				this.m_oneClickReceiveBtnGrays.Recovery();
			}
			else
			{
				this.m_oneClickReceiveBtnGrays.SetUIGray();
			}
			int num2 = 0;
			List<ulong> list2 = this.m_mainCityDataModule.m_chestDatas.Keys.ToList<ulong>();
			List<ulong> refreshChestAddRowIDs = this.m_mainCityDataModule.GetRefreshChestAddRowIDs();
			foreach (KeyValuePair<int, MainCityBoxContentNode> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					if (num2 >= this.m_mainCityDataModule.m_chestDatas.Count)
					{
						keyValuePair.Value.SetAsHasBox(false);
					}
					else
					{
						keyValuePair.Value.PlayBoxScaleToShow(this.m_seqPool);
						ulong num3 = list2[num2];
						keyValuePair.Value.RefreshUI(this.m_mainCityDataModule.m_chestDatas[num3]);
						if (refreshChestAddRowIDs.Contains(num3))
						{
							keyValuePair.Value.PlayScale();
						}
					}
					num2++;
				}
			}
			this.m_integral.RefreshUI(isInit);
		}

		[Header("当前时间")]
		public CustomText m_unFullTxt;

		public GameObject m_fullTxt;

		[Header("宝箱组")]
		public RectTransform m_contentLayout;

		public GameObject m_contentPrefab;

		[Header("积分和进度")]
		public MainCityBoxIntegral m_integral;

		[Header("其他")]
		public UIPopCommon m_popCommon;

		public CustomButton m_infoBtn;

		public CustomButton m_oneClickReceiveBtn;

		public UIGrays m_oneClickReceiveBtnGrays;

		public Dictionary<int, MainCityBoxContentNode> m_nodes = new Dictionary<int, MainCityBoxContentNode>();

		public MainCityDataModule m_mainCityDataModule;

		[Header("控制参数")]
		public float m_time;

		public float m_duration;

		public bool m_isPlaying;

		private SequencePool m_seqPool = new SequencePool();
	}
}
