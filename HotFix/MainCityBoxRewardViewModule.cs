using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.Actor;
using Proto.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class MainCityBoxRewardViewModule : BaseViewModule
	{
		public ViewName GetName()
		{
			return ViewName.MainCityBoxRewardViewModule;
		}

		public override void OnCreate(object data)
		{
			this.m_mainCityDataModule = GameApp.Data.GetDataModule(DataName.MainCityDataModule);
			this.m_prefab.SetActive(false);
			this.m_animDict = new Dictionary<string, Animator>();
			this.m_animDict["Box_1"] = this.m_boxAnimatorList[0];
			this.m_animDict["Box_2"] = this.m_boxAnimatorList[1];
			this.m_animDict["Box_3"] = this.m_boxAnimatorList[2];
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as MainCityBoxRewardViewModule.OpenData;
			this.m_isNetFinished = false;
			this.m_isBoxOpenFinished = false;
			if (this.m_title != null)
			{
				this.m_title.SetText(Singleton<LanguageManager>.Instance.GetInfoByID("1102"));
			}
			this.m_panel.gameObject.SetActive(false);
			int num = 0;
			for (int i = 0; i < this.m_openData.m_datas.Count; i++)
			{
				MainCityBoxData mainCityBoxData = this.m_openData.m_datas[i];
				if (mainCityBoxData != null && mainCityBoxData.m_quality > num)
				{
					num = mainCityBoxData.m_quality;
				}
			}
			Box_boxBase elementById = GameApp.Table.GetManager().GetBox_boxBaseModelInstance().GetElementById(num);
			this.m_boxAnimator = this.m_animDict[elementById.uiObjName];
			this.m_boxListen = this.m_boxAnimator.GetComponent<AnimatorListen>();
			this.m_boxAnimator.gameObject.SetActive(true);
			this.m_boxAnimator.SetTrigger("TopIn");
			this.m_lastCount = this.m_mainCityDataModule.m_chestIntegral;
			NetworkUtils.MainCity.DoCityOpenChestRequest(this.m_openData.m_datas.Select((MainCityBoxData c) => c.m_rowID).ToList<ulong>(), delegate(bool isOk, CityOpenChestResponse rep)
			{
				if (!isOk)
				{
					return;
				}
				this.m_response = rep;
				this.m_isNetFinished = true;
				this.CheckShowItems();
			}, false);
			this.m_boxListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnListenBox));
			this.m_panelListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnListenPanel));
			this.m_closeBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_content);
			LayoutRebuilder.MarkLayoutForRebuild(this.m_content);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.m_sequencePool.Clear(false);
			this.m_closeBt.onClick.RemoveAllListeners();
			if (this.m_boxListen != null)
			{
				this.m_boxListen.onListen.RemoveAllListeners();
			}
			if (this.m_panelListen != null)
			{
				this.m_panelListen.onListen.RemoveAllListeners();
			}
			foreach (KeyValuePair<int, UIItem> keyValuePair in this.m_dic)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_dic.Clear();
			this.m_openData = null;
			this.m_mainCityDataModule = null;
			this.m_response = null;
			this.m_isNetFinished = false;
			this.m_isBoxOpenFinished = false;
			this.m_animDict.Clear();
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

		private void OnClickCloseBt()
		{
			if (this.m_response != null && this.m_openData != null)
			{
				ItemData itemData = new ItemData(23, (long)(this.m_response.Score - this.m_lastCount));
				GameApp.View.FlyItemDatas(FlyItemModel.Default, new List<ItemData> { itemData }, delegate(FlyNodeItemDatas data, int index, int endNodeIndex, int itemIndex, float progress)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_MainCityBoxView_RefreshIntegral, null);
				}, null);
			}
			GameApp.View.CloseView(this.GetName(), null);
		}

		private void OnListenBox(GameObject obj, string args)
		{
			if (string.IsNullOrEmpty(args))
			{
				return;
			}
			if (!string.Equals(args, "open"))
			{
				return;
			}
			this.m_isBoxOpenFinished = true;
			this.CheckShowItems();
		}

		private void OnListenPanel(GameObject obj, string args)
		{
			if (string.IsNullOrEmpty(args))
			{
				return;
			}
			if (!string.Equals(args, "show"))
			{
				return;
			}
			this.m_sequencePool.Clear(false);
			Sequence sequence = this.m_sequencePool.Get();
			int num = 0;
			foreach (KeyValuePair<int, UIItem> keyValuePair in this.m_dic)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.gameObject.transform.localScale = Vector3.zero;
					TweenSettingsExtensions.Join(sequence, TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOScale(keyValuePair.Value.gameObject.transform, Vector3.one, 0.1f), (float)num * 0.01f));
					num++;
				}
			}
		}

		private void CheckShowItems()
		{
			if (!this.m_isNetFinished || !this.m_isBoxOpenFinished)
			{
				return;
			}
			for (int i = 0; i < this.m_response.CommonData.Reward.Count; i++)
			{
				RewardDto rewardDto = this.m_response.CommonData.Reward[i];
				if (rewardDto != null)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.m_prefab);
					gameObject.SetParentNormal(this.m_content, false);
					gameObject.SetActive(true);
					UIItem component = gameObject.GetComponent<UIItem>();
					component.Init();
					component.SetData(rewardDto.ToPropData());
					component.OnRefresh();
					this.m_dic[component.GetInstanceID()] = component;
				}
			}
			this.m_panel.gameObject.SetActive(true);
		}

		public CustomButton m_closeBt;

		public UIBgText m_title;

		public RectTransform m_boxs;

		public RectTransform m_panel;

		public AnimatorListen m_panelListen;

		public ScrollRect m_scrollRect;

		public RectTransform m_content;

		public GameObject m_prefab;

		public Dictionary<int, UIItem> m_dic = new Dictionary<int, UIItem>();

		public SequencePool m_sequencePool = new SequencePool();

		public Animator[] m_boxAnimatorList;

		public Animator m_boxAnimator;

		public AnimatorListen m_boxListen;

		public MainCityBoxRewardViewModule.OpenData m_openData;

		public MainCityDataModule m_mainCityDataModule;

		private CityOpenChestResponse m_response;

		private bool m_isNetFinished;

		private bool m_isBoxOpenFinished;

		private int m_lastCount;

		private Dictionary<string, Animator> m_animDict;

		public class OpenData
		{
			public List<MainCityBoxData> m_datas = new List<MainCityBoxData>();
		}
	}
}
