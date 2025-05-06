using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class ChainPacksViewModule : BaseViewModule
	{
		private int ActId
		{
			get
			{
				return this.actInfo.ActId;
			}
		}

		private ChainPacksDataModule.EChainPacksOriginType ActOriginType
		{
			get
			{
				return this.actInfo.ActOriginType;
			}
		}

		public ChainPacksDataModule.ActInfo actInfo { get; private set; }

		public override void OnCreate(object data)
		{
			this.buttonClose.m_onClick = new Action(this.OnClickClose);
			this.buttonMask.m_onClick = new Action(this.OnClickClose);
			this.buttonHelper.m_onClick = new Action(this.OnHelperClick);
			this.packContentDefaultPos = this.packContent.localPosition;
			this.pageCtrl.Init();
			this.pageCtrl.onPageChanged = new Action<UIPageCtrl>(this.OnPageChanged);
		}

		public override void OnDelete()
		{
			this.buttonClose.m_onClick = null;
			this.buttonMask.m_onClick = null;
			this.buttonHelper.m_onClick = null;
			this.pageCtrl.DeInit();
			this.pageCtrl.onPageChanged = null;
		}

		public override void OnOpen(object data)
		{
			this.dataModule = GameApp.Data.GetDataModule(DataName.ChainPacksDataModule);
			if (this.requestServerDataOnOpen)
			{
				this.viewContent.SetActive(false);
				this.isNetRequesting = true;
				NetworkUtils.ChainPacks.DoChainPacksTimeRequest(false, true, delegate(bool success, int code)
				{
					this.isNetRequesting = false;
					this.OnOpened();
				}, true);
				return;
			}
			this.OnOpened();
		}

		public override void OnClose()
		{
			this.scrollRect.StopAllCoroutines();
			this.ReleasePoolNodeList();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_ChainPacks_PickedReward, new HandlerEvent(this.OnNetEvent_PickedReward));
			manager.RegisterEvent(LocalMessageName.CC_ChainPacks_ActFreshed, new HandlerEvent(this.OnNetEvent_ActFreshed));
			manager.RegisterEvent(LocalMessageName.CC_ChainPacksPush_RerfreshView, new HandlerEvent(this.OnNetEvent_ActPushPacksFreshed));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_ChainPacks_PickedReward, new HandlerEvent(this.OnNetEvent_PickedReward));
			manager.UnRegisterEvent(LocalMessageName.CC_ChainPacks_ActFreshed, new HandlerEvent(this.OnNetEvent_ActFreshed));
			manager.UnRegisterEvent(LocalMessageName.CC_ChainPacksPush_RerfreshView, new HandlerEvent(this.OnNetEvent_ActPushPacksFreshed));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.isNetRequesting)
			{
				return;
			}
			this.FreshLeftTime();
		}

		private void OnOpened()
		{
			if (!this.dataModule.CanShow(ChainPacksDataModule.ChainPacksType.All))
			{
				this.OnClickClose();
				return;
			}
			this.viewContent.SetActive(true);
			this.activityIndex = 0;
			this.pageCtrl.SetPage(this.dataModule.actInfoDic.Count, this.activityIndex);
			this.actInfo = this.dataModule.GetActInfoIndex(this.activityIndex);
			this.FreshViewAll();
		}

		private void FreshViewAll()
		{
			if (this.actInfo == null)
			{
				this.OnClickClose();
				return;
			}
			this.FreshTheme();
			this.FreshViewTitle();
			this.FreshLeftTime();
			this.FreshRate();
			this.FreshActivityPacks();
			this.FreshPageRedNodes();
			this.MoveToCurrent(0f);
		}

		[ContextMenu("MoveToCurrent")]
		private void MoveToCurrent(float duration = 0f)
		{
			int num = this.actInfo.pickedIndex + 1;
			if (num > 0)
			{
				if (num > this.packCfgList.Count - 1)
				{
					num = this.packCfgList.Count - 1;
				}
				this.scrollRect.MoveCenterToItem(this.packView, this.packContent, this.packContent.GetChild(num) as RectTransform, duration);
			}
		}

		private void FreshTheme()
		{
			this.imageBanner.SetSprite(this.spriteRegister.GetSprite(this.actInfo.actTypeCfg.sprite_Banner));
			this.imageTitle.SetImage(this.actInfo.actTypeCfg.atlasId_Title, this.actInfo.actTypeCfg.sprite_Title);
			this.imageBg.SetSprite(this.spriteRegister.GetSprite(this.actInfo.actTypeCfg.sprite_Bg));
			this.imageBgMask.SetSprite(this.spriteRegister.GetSprite(this.actInfo.actTypeCfg.sprite_Bg_Mask));
		}

		private void FreshViewTitle()
		{
			if (!string.IsNullOrEmpty(this.actInfo.ActName))
			{
				this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.actInfo.ActName);
				return;
			}
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("chainpacks_title");
		}

		private void FreshLeftTime()
		{
			long num = (long)((this.actInfo != null) ? (this.actInfo.actDto.EndTime - (ulong)DxxTools.Time.ServerTimestamp) : 0UL);
			this.textLeftTime.text = DxxTools.FormatFullTimeWithDay(num);
			if (num <= 0L)
			{
				this.TimeEnd();
			}
		}

		private void FreshRate()
		{
			this.textRate.text = (this.actInfo.ActRateValue * 100).ToString() + "%";
		}

		private void FreshActivityPacks()
		{
			this.scrollRect.StopMovement();
			this.packContent.localPosition = this.packContentDefaultPos;
			this.ReleasePoolNodeList();
			this.packCfgList.Clear();
			IList<ChainPacks_ChainPacks> chainPacks_ChainPacksElements = GameApp.Table.GetManager().GetChainPacks_ChainPacksElements();
			for (int i = 0; i < chainPacks_ChainPacksElements.Count; i++)
			{
				if (chainPacks_ChainPacksElements[i].group == this.actInfo.ActGroupId)
				{
					this.packCfgList.Add(chainPacks_ChainPacksElements[i]);
				}
			}
			this.packCfgList.Sort((ChainPacks_ChainPacks a, ChainPacks_ChainPacks b) => a.id.CompareTo(b.id));
			for (int j = 0; j < this.packCfgList.Count; j++)
			{
				ChainPacksNodeCtrl poolNode = this.GetPoolNode();
				this.nodeList.Add(poolNode);
				Transform transform = poolNode.transform;
				transform.SetParent(this.packContent);
				transform.SetSiblingIndex(j);
				transform.localPosition = Vector3.zero;
				transform.localScale = Vector3.one;
				poolNode.Init();
				poolNode.SetFresh(this, this.ActId, this.ActOriginType, this.packCfgList[j], j);
			}
		}

		private void FreshPageRedNodes()
		{
			bool flag = this.dataModule.ShowRedPreAct(this.activityIndex);
			this.pageCtrl.SetRedNodePre(flag);
			bool flag2 = this.dataModule.ShowRedNextAct(this.activityIndex);
			this.pageCtrl.SetRedNodeNext(flag2);
		}

		private ChainPacksNodeCtrl GetPoolNode()
		{
			if (this.nodePools.Count > 0)
			{
				ChainPacksNodeCtrl chainPacksNodeCtrl = this.nodePools[0];
				this.nodePools.RemoveAt(0);
				chainPacksNodeCtrl.gameObject.SetActive(true);
				return chainPacksNodeCtrl;
			}
			return Object.Instantiate<ChainPacksNodeCtrl>(this.packsNodeTemplate);
		}

		private void ReleasePoolNodeList()
		{
			for (int i = 0; i < this.nodeList.Count; i++)
			{
				this.ReleasePoolNode(this.nodeList[i]);
			}
			this.nodeList.Clear();
		}

		private void ReleasePoolNode(ChainPacksNodeCtrl node)
		{
			node.DeInit();
			node.gameObject.SetActive(false);
			this.nodePools.Add(node);
		}

		public UIItem GetPoolItem()
		{
			if (this.itemPools.Count > 0)
			{
				UIItem uiitem = this.itemPools[0];
				this.itemPools.RemoveAt(0);
				uiitem.gameObject.SetActive(true);
				return uiitem;
			}
			return Object.Instantiate<UIItem>(this.itemPrefab);
		}

		public void ReleasePoolItem(UIItem item)
		{
			item.DeInit();
			item.gameObject.SetActive(false);
			this.itemPools.Add(item);
		}

		private void TimeEnd()
		{
			GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("chainpacks_activityend"));
			this.OnClickClose();
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.ChainPacksViewModule, null);
		}

		private void OnHelperClick()
		{
			Module_moduleInfo elementById = GameApp.Table.GetManager().GetModule_moduleInfoModelInstance().GetElementById(303);
			if (elementById != null)
			{
				InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData
				{
					m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameId),
					m_contextInfo = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.infoId)
				};
				GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
			}
		}

		private void OnPageChanged(UIPageCtrl pageCtrl)
		{
			this.activityIndex = pageCtrl.CurrentPage;
			this.actInfo = this.dataModule.GetActInfoIndex(this.activityIndex);
			this.FreshViewAll();
		}

		private void OnNetEvent_ActPushPacksFreshed(object sender, int type, BaseEventArgs eventArgs)
		{
		}

		private void OnNetEvent_PickedReward(object sender, int type, BaseEventArgs eventArgs)
		{
			if (!this.dataModule.CanShow(ChainPacksDataModule.ChainPacksType.All))
			{
				this.OnClickClose();
				return;
			}
			EventArgsChainPacksPickedReward eventArgsChainPacksPickedReward = eventArgs as EventArgsChainPacksPickedReward;
			if (eventArgsChainPacksPickedReward != null)
			{
				for (int i = 0; i < this.nodeList.Count; i++)
				{
					if (this.nodeList[i].packCfg.id == eventArgsChainPacksPickedReward.CfgID)
					{
						this.nodeList[i].FreshState(true);
						if (i + 1 < this.nodeList.Count)
						{
							this.nodeList[i + 1].FreshState(true);
						}
					}
				}
			}
			this.actInfo = this.dataModule.GetActInfoIndex(this.activityIndex);
		}

		private void OnNetEvent_ActFreshed(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnOpened();
		}

		[Header("Base")]
		public CustomButton buttonMask;

		public CustomButton buttonClose;

		public CustomText textTitle;

		public CustomButton buttonHelper;

		public GameObject viewContent;

		public SpriteRegister spriteRegister;

		[Header("主题")]
		public CustomImage imageBanner;

		public CustomImage imageTitle;

		public CustomImage imageBg;

		public CustomImage imageBgMask;

		[Header("页码")]
		public UIPageCtrl pageCtrl;

		[Header("Rate")]
		public CustomText textRate;

		[Header("Time")]
		public CustomText textLeftTime;

		[Header("礼包")]
		public ScrollRect scrollRect;

		public RectTransform packView;

		public RectTransform packContent;

		public ChainPacksNodeCtrl packsNodeTemplate;

		public UIItem itemPrefab;

		private ChainPacksDataModule dataModule;

		private int activityIndex;

		private List<ChainPacks_ChainPacks> packCfgList = new List<ChainPacks_ChainPacks>();

		private List<ChainPacksNodeCtrl> nodeList = new List<ChainPacksNodeCtrl>();

		private List<ChainPacksNodeCtrl> nodePools = new List<ChainPacksNodeCtrl>();

		private List<UIItem> itemPools = new List<UIItem>();

		private Vector3 packContentDefaultPos;

		private bool requestServerDataOnOpen = true;

		private bool isNetRequesting;
	}
}
