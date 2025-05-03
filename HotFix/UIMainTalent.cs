using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using HotFix.EventArgs;
using LocalModels.Bean;
using Proto.Talents;
using Server;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace HotFix
{
	public class UIMainTalent : UIBaseMainPageNode
	{
		protected override void OnInit()
		{
			try
			{
				this.btnPreviewOriginPos = (this.btnEvolutionPreview.transform as RectTransform).anchoredPosition;
				this.roleEffectGo.SetActive(false);
				this.moduleCurrencyCtrl.Init();
				if (Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.TalentLegacy, false))
				{
					this.moduleCurrencyCtrl.SetStyle(EModuleId.Talent, new List<int> { 1, 2, 47 });
				}
				else
				{
					this.moduleCurrencyCtrl.SetStyle(EModuleId.Talent, new List<int> { 1, 2 });
				}
				this.heroDataModule = GameApp.Data.GetDataModule(DataName.HeroDataModule);
				this.addAttributeDataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
				this.btnEvolutionPreview.m_onClick = new Action(this.OnBtnEvolutionPreviewClick);
				this.prefabProgressNode.SetActive(false);
				this.prefabAttributeItem.gameObject.SetActive(false);
				this.helpButton.Init();
				if (this.Ctrl_TalentLegacyTree != null)
				{
					this.Ctrl_TalentLegacyTree.Init();
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		protected override void OnDeInit()
		{
			this.moduleCurrencyCtrl.DeInit();
			this.btnEvolutionPreview.m_onClick = null;
			for (int i = 0; i < this.attributeLevelUpItems.Count; i++)
			{
				this.attributeLevelUpItems[i].DeInit();
			}
			this.attributeLevelUpItems.Clear();
			this.attributeLevelUpItems = null;
			this.helpButton.DeInit();
			if (this.Ctrl_TalentLegacyRank != null)
			{
				this.Ctrl_TalentLegacyRank.DeInit();
			}
			if (this.Ctrl_TalentLegacyMain != null)
			{
				this.Ctrl_TalentLegacyMain.DeInit();
			}
			if (this.Ctrl_TalentLegacyTree != null)
			{
				this.Ctrl_TalentLegacyTree.DeInit();
			}
		}

		protected override async void OnShow(UIBaseMainPageNode.OpenData openData)
		{
			this.talentDataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
			this.curTalentData = this.talentDataModule.talentData;
			this.m_isMaxLevel = false;
			this.m_mainOpenData = null;
			if (openData != null)
			{
				this.m_mainOpenData = openData;
			}
			this.MaskCommon.SetActiveSafe(false);
			this.IsCheckTalentMaxLevel(false);
			this.OnRefreshView();
		}

		private void OnRefreshView()
		{
			this.inAnimationPlaying = false;
			this.needBlockClick = false;
			this.roleEffectGo.SetActive(false);
			this.talentDataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
			this.curTalentData = this.talentDataModule.talentData;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCurrency, Singleton<EventArgsBool>.Instance.SetData(false));
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCorner, Singleton<EventArgsBool>.Instance.SetData(false));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIOpenTalentLegacyMain, new HandlerEvent(this.OnOpenTalentLegacyMain));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIOpenTalentLegacyRank, new HandlerEvent(this.OnOpenTalentLegacyRank));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIOpenTalentLegacyTree, new HandlerEvent(this.OnOpenTalentLegacyTree));
			this.RefreshCardData(this.heroDataModule.MainCardData);
			this.Skin_ModelItem.Init();
			GameApp.Data.GetDataModule(DataName.ClothesDataModule).PushUIModelItem(this.Skin_ModelItem, new Action(this.FreshSkin));
			this.UpdateTitle();
			GameApp.Event.RegisterEvent(LocalMessageName.SC_IAPBuySupplyGiftSuccess, new HandlerEvent(this.OnCurrencyChange));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Currency_Update, new HandlerEvent(this.OnCurrencyChange));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
			this.curProgressNode = this.CreateProgress();
			this.UpdateAttributeItems();
			if (this.playUIOpenAnimationCoroutine != null)
			{
				base.StopCoroutine(this.playUIOpenAnimationCoroutine);
			}
			if (!this.m_isMaxLevel)
			{
				this.playUIOpenAnimationCoroutine = base.StartCoroutine(this.PlayUIOpenAnimation());
			}
			this.moduleCurrencyCtrl.SetFlyPosition();
		}

		private void OnCurrencyChange(object obj, int type, BaseEventArgs args)
		{
			this.UpdateAttributeItems();
		}

		protected override void OnHide()
		{
			this.DestroyProgress(this.curProgressNode);
			GameApp.Data.GetDataModule(DataName.ClothesDataModule).PopUIModelItem(this.Skin_ModelItem);
			this.Skin_ModelItem.OnHide(false);
			this.Skin_ModelItem.DeInit();
			GameApp.Event.UnRegisterEvent(LocalMessageName.SC_IAPBuySupplyGiftSuccess, new HandlerEvent(this.OnCurrencyChange));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Currency_Update, new HandlerEvent(this.OnCurrencyChange));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIOpenTalentLegacyMain, new HandlerEvent(this.OnOpenTalentLegacyMain));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIOpenTalentLegacyRank, new HandlerEvent(this.OnOpenTalentLegacyRank));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIOpenTalentLegacyTree, new HandlerEvent(this.OnOpenTalentLegacyTree));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
			if (this.Ctrl_TalentLegacyRank != null && this.Ctrl_TalentLegacyRank.gameObject.activeSelf)
			{
				this.Ctrl_TalentLegacyRank.gameObject.SetActiveSafe(false);
				this.Ctrl_TalentLegacyRank.OnClose();
			}
			if (this.Ctrl_TalentLegacyMain != null)
			{
				this.Ctrl_TalentLegacyMain.gameObject.SetActiveSafe(false);
				this.Ctrl_TalentLegacyMain.OnClose();
			}
			if (this.Ctrl_TalentLegacyTree != null)
			{
				this.Ctrl_TalentLegacyTree.gameObject.SetActiveSafe(false);
				this.Ctrl_TalentLegacyTree.OnClose();
			}
		}

		public override void OnLanguageChange()
		{
			this.UpdateTitle();
		}

		private void UpdateAttributeItems()
		{
			List<TalentAttributeLevelUpData> list = this.talentDataModule.talentData.AttributeMap.Values.ToList<TalentAttributeLevelUpData>();
			for (int i = 0; i < list.Count; i++)
			{
				if (this.attributeLevelUpItems.Count <= i)
				{
					TalentAttributeItem talentAttributeItem = Object.Instantiate<TalentAttributeItem>(this.prefabAttributeItem, this.prefabAttributeItem.transform.parent, false);
					talentAttributeItem.Init();
					talentAttributeItem.clickCallback = new Action<TalentAttributeItem>(this.OnAttributeItemClick);
					this.attributeLevelUpItems.Add(talentAttributeItem);
				}
				this.attributeLevelUpItems[i].SetEffectScale(this.attributeEffectScaleList[list.Count - 1]);
			}
			Vector2 zero = Vector2.zero;
			Vector2 cellSize = this.attributeGridLayoutGroup.cellSize;
			if (list.Count > this.attributeItemWidthList.Count)
			{
				cellSize.x = this.attributeItemWidthList[this.attributeItemWidthList.Count - 1];
				zero.x = this.attributeItemSpaceList[this.attributeItemWidthList.Count - 1];
			}
			else if (list.Count > 0)
			{
				cellSize.x = this.attributeItemWidthList[list.Count - 1];
				zero.x = this.attributeItemSpaceList[list.Count - 1];
			}
			else
			{
				cellSize.x = this.attributeItemWidthList[0];
				zero.x = this.attributeItemSpaceList[0];
			}
			this.attributeGridLayoutGroup.enabled = true;
			this.attributeGridLayoutGroup.spacing = zero;
			this.attributeGridLayoutGroup.cellSize = cellSize;
			for (int j = 0; j < this.attributeLevelUpItems.Count; j++)
			{
				TalentAttributeLevelUpData talentAttributeLevelUpData = ((list.Count > j) ? list[j] : null);
				if (talentAttributeLevelUpData != null)
				{
					this.attributeLevelUpItems[j].gameObject.SetActive(true);
					this.attributeLevelUpItems[j].SetData(talentAttributeLevelUpData);
					if (talentAttributeLevelUpData.talentAttributeKey.Equals("Attack"))
					{
						this.attributeLevelUpItems[j].TriggerNewbieGuide();
					}
				}
				else
				{
					this.attributeLevelUpItems[j].gameObject.SetActive(false);
				}
			}
			DelayCall.Instance.CallOnce(20, delegate
			{
				if (this.attributeGridLayoutGroup != null)
				{
					this.attributeGridLayoutGroup.enabled = false;
				}
			});
		}

		private IEnumerator PlayUIOpenAnimation()
		{
			this.inOpenAnimationPlaying = true;
			int delayFrame = 0;
			CanvasGroup canvasGroup = this.curProgressNode.GetComponent<CanvasGroup>();
			canvasGroup.alpha = 0f;
			RectTransform rectTransform = this.btnEvolutionPreview.transform as RectTransform;
			rectTransform.anchoredPosition = new Vector2(this.btnPreviewOriginPos.x + 300f, this.btnPreviewOriginPos.y);
			ShortcutExtensions46.DOAnchorPosX(rectTransform, this.btnPreviewOriginPos.x, 0.2f, false);
			yield return 0;
			if (this == null || base.gameObject == null || !base.gameObject.activeSelf)
			{
				yield break;
			}
			if (this.attributeGridLayoutGroup != null)
			{
				this.attributeGridLayoutGroup.enabled = false;
			}
			canvasGroup.alpha = 0.3f;
			ShortcutExtensions46.DOFade(canvasGroup, 1f, 0.2f);
			delayFrame = this.curProgressNode.PlayOpenAnimation();
			if (this == null || base.gameObject == null || !base.gameObject.activeSelf)
			{
				yield break;
			}
			for (int i = 0; i < this.attributeLevelUpItems.Count; i++)
			{
				base.StartCoroutine(this.attributeLevelUpItems[i].PlayOpenAnimation(i * 3));
			}
			delayFrame = this.attributeLevelUpItems.Count * 10;
			while (delayFrame > 0)
			{
				int num = delayFrame;
				delayFrame = num - 1;
				yield return 0;
			}
			this.inOpenAnimationPlaying = false;
			if (this == null || base.gameObject == null || !base.gameObject.activeSelf)
			{
				yield break;
			}
			base.StopCoroutine("RandPlaySweepLightEffect");
			base.StartCoroutine("RandPlaySweepLightEffect");
			yield break;
		}

		private IEnumerator RandPlaySweepLightEffect()
		{
			for (;;)
			{
				float num = Random.Range(2f, 7f);
				yield return new WaitForSeconds(num);
				List<TalentAttributeItem> list = new List<TalentAttributeItem>();
				for (int i = 0; i < this.attributeLevelUpItems.Count; i++)
				{
					if (this.attributeLevelUpItems[i].isActiveAndEnabled && this.attributeLevelUpItems[i].data.curLevel < this.attributeLevelUpItems[i].data.maxLevel)
					{
						list.Add(this.attributeLevelUpItems[i]);
					}
				}
				if (list.Count <= 0)
				{
					break;
				}
				int num2 = Random.Range(0, list.Count);
				list[num2].StartCoroutine(list[num2].PlaySweepLightAnimation());
			}
			yield break;
		}

		private void UpdateTitle()
		{
			if (this.talentDataModule == null)
			{
				return;
			}
			TalentNew_talentEvolution elementById = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(this.talentDataModule.talentData.TalentStage);
			this.txtTalentTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.stepLanguageId);
		}

		private TalentProgressNode CreateProgress()
		{
			TalentProgressData talentProgressData = this.talentDataModule.talentProgressData;
			TalentProgressNode talentProgressNode = Object.Instantiate<TalentProgressNode>(this.prefabProgressNode, this.prefabProgressNode.transform.parent, false);
			talentProgressNode.Init();
			talentProgressNode.InitProgress(talentProgressData);
			talentProgressNode.gameObject.SetActive(true);
			return talentProgressNode;
		}

		private void DestroyProgress(TalentProgressNode talentProgressNode)
		{
			if (talentProgressNode != null)
			{
				talentProgressNode.DeInit();
				Object.Destroy(talentProgressNode.gameObject);
			}
		}

		private void RefreshCardData(CardData cardData)
		{
			this.cardData = new CardData();
			this.cardData.CloneFrom(cardData);
			this.cardData.UpdateAttribute(this.addAttributeDataModule.AttributeDatas);
		}

		private void OnBtnEvolutionPreviewClick()
		{
			if (this.inOpenAnimationPlaying)
			{
				return;
			}
			GameApp.View.OpenView(ViewName.TalentEvolutionPreviewViewModule, null, 1, null, null);
		}

		private void OnAttributeItemClick(TalentAttributeItem item)
		{
			this.lastClickTime = Time.realtimeSinceStartup;
			if (this.inOpenAnimationPlaying || this.needBlockClick)
			{
				return;
			}
			TalentAttributeLevelUpData data = item.data;
			if (item.data.curLevel >= data.maxLevel)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("talent_attribute_full_level"));
				return;
			}
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			if (item.costData != null && dataModule.GetItemDataCountByid((ulong)((long)item.costData.ID)) < item.costData.TotalCount)
			{
				GameApp.View.ShowItemNotEnoughTip(item.costData.ID, true);
				return;
			}
			if (this.curTotalExp <= 0)
			{
				this.curTotalExp = this.curTalentData.TalentExp;
			}
			NetworkUtils.Talent.DoAttributeUpgradeRequest(data.talentStep, data.talentAttributeKey, delegate(bool isOk, TalentsLvUpResponse resData)
			{
				if (isOk)
				{
					if (data.talentAttributeKey.Equals("Attack"))
					{
						GuideController.Instance.CustomizeStringOver("TalentAttributeItem", true);
					}
					this.DoAttributeUpgradeCallback(item, resData, delegate
					{
						GameApp.Event.Dispatch(null, LocalMessageName.CC_UITalent_UpGradeBack, null);
					});
				}
			});
		}

		private void IsCheckTalentMaxLevel(bool isAni = false)
		{
			bool flag = false;
			if (this.talentDataModule != null)
			{
				flag = this.talentDataModule.IsCheckMaxLevel();
			}
			this.m_isMaxLevel = flag;
			if (!flag)
			{
				this.Obj_Talent.SetActiveSafe(true);
				this.Obj_TalentBg.SetActiveSafe(true);
				this.btnEvolutionPreview.gameObject.SetActiveSafe(true);
				if (this.Ctrl_TalentLegacyRank != null && this.Ctrl_TalentLegacyRank.gameObject.activeSelf)
				{
					this.Ctrl_TalentLegacyRank.OnClose();
					this.Ctrl_TalentLegacyRank.gameObject.SetActiveSafe(false);
				}
				if (this.Ctrl_TalentLegacyMain != null && this.Ctrl_TalentLegacyMain.gameObject.activeSelf)
				{
					this.Ctrl_TalentLegacyMain.OnClose();
					this.Ctrl_TalentLegacyMain.gameObject.SetActiveSafe(false);
				}
				if (this.Ctrl_TalentLegacyTree != null && this.Ctrl_TalentLegacyTree.gameObject.activeSelf)
				{
					this.Ctrl_TalentLegacyTree.OnClose();
					this.Ctrl_TalentLegacyTree.gameObject.SetActiveSafe(false);
				}
				return;
			}
			NetworkUtils.TalentLegacy.DoTalentLegacyInfoRequest(null, false);
			if (this.m_mainOpenData != null && this.m_mainOpenData.OriginType == UIBaseMainPageNode.EOriginType.Equip)
			{
				this.OnOpenTalentLegacyMain(null, 1, null);
				return;
			}
			TalentLegacyDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			if (isAni && !dataModule.IsAni)
			{
				dataModule.IsAni = true;
				CloudLoadingViewModule.OpenData openData = new CloudLoadingViewModule.OpenData();
				openData.onAnimFinish = delegate
				{
					GuideController.Instance.CustomizeStringTrigger("TalentLegacyFunctionOpen");
				};
				GameApp.View.OpenView(ViewName.CloudLoadingViewModule, openData, 1, null, null);
			}
			ValueTuple<int, int> valueTuple = this.IsHaveStudyNode();
			if (valueTuple.Item1 == -1 && valueTuple.Item2 == -1)
			{
				this.OnOpenTalentLegacyRank(null, 1, null);
				return;
			}
			if (Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.TalentLegacy, false))
			{
				EventArgsOpenTree eventArgsOpenTree = new EventArgsOpenTree(valueTuple.Item1, valueTuple.Item2);
				this.OnOpenTalentLegacyTree(null, 1, eventArgsOpenTree);
				return;
			}
			this.OnOpenTalentLegacyRank(null, 1, null);
		}

		private ValueTuple<int, int> IsHaveStudyNode()
		{
			string talentLegacyNode = PlayerPrefsKeys.GetTalentLegacyNode();
			if (string.IsNullOrEmpty(talentLegacyNode))
			{
				return new ValueTuple<int, int>(-1, -1);
			}
			int num = int.Parse(talentLegacyNode.Split('_', StringSplitOptions.None)[0]);
			int num2 = int.Parse(talentLegacyNode.Split('_', StringSplitOptions.None)[1]);
			return new ValueTuple<int, int>(num, num2);
		}

		private void OnOpenTalentLegacyRank(object obj, int type, BaseEventArgs args)
		{
			this.Obj_Talent.SetActiveSafe(false);
			this.Obj_TalentBg.SetActiveSafe(false);
			this.btnEvolutionPreview.gameObject.SetActiveSafe(false);
			this.MaskCommon.SetActiveSafe(true);
			this.OnOpenTalentLegacyRank();
		}

		private async void OnOpenTalentLegacyRank()
		{
			if (this.Ctrl_TalentLegacyRank != null)
			{
				this.Ctrl_TalentLegacyRank.OnClose();
				this.Ctrl_TalentLegacyRank.gameObject.SetActiveSafe(false);
			}
			if (this.Ctrl_TalentLegacyRank == null)
			{
				AsyncOperationHandle<GameObject> handle = GameApp.Resources.LoadAssetAsync<GameObject>("Assets/_Resources/Prefab/UI/TalentLegacy/TalentLegacyRank.prefab");
				await handle.Task;
				if (handle.Status != 1)
				{
					return;
				}
				GameObject gameObject = Object.Instantiate<GameObject>(handle.Result, this.Obj_TalentLegacy.transform);
				this.Ctrl_TalentLegacyRank = gameObject.GetComponent<TalentLegacyRank>();
				this.Ctrl_TalentLegacyRank.Init();
				handle = default(AsyncOperationHandle<GameObject>);
			}
			if (this.Ctrl_TalentLegacyMain != null && this.Ctrl_TalentLegacyMain.gameObject.activeSelf)
			{
				this.Ctrl_TalentLegacyMain.OnClose();
				this.Ctrl_TalentLegacyMain.gameObject.SetActiveSafe(false);
			}
			if (this.Ctrl_TalentLegacyTree != null && this.Ctrl_TalentLegacyTree.gameObject.activeSelf)
			{
				this.Ctrl_TalentLegacyTree.OnClose();
				this.Ctrl_TalentLegacyTree.gameObject.SetActiveSafe(false);
			}
			this.MaskCommon.SetActiveSafe(false);
			this.Ctrl_TalentLegacyRank.gameObject.SetActiveSafe(true);
			this.Ctrl_TalentLegacyRank.OnShow();
		}

		private async void OnOpenTalentLegacyMain(object obj, int type, BaseEventArgs args)
		{
			this.Obj_Talent.SetActiveSafe(false);
			this.Obj_TalentBg.SetActiveSafe(false);
			this.btnEvolutionPreview.gameObject.SetActiveSafe(false);
			if (this.Ctrl_TalentLegacyMain == null)
			{
				AsyncOperationHandle<GameObject> handle = GameApp.Resources.LoadAssetAsync<GameObject>("Assets/_Resources/Prefab/UI/TalentLegacy/TalentLegacyMain.prefab");
				await handle.Task;
				if (handle.Status != 1)
				{
					return;
				}
				GameObject gameObject = Object.Instantiate<GameObject>(handle.Result, this.Obj_TalentLegacy.transform);
				this.Ctrl_TalentLegacyMain = gameObject.GetComponent<TalentLegacyMain>();
				this.Ctrl_TalentLegacyMain.Init();
				handle = default(AsyncOperationHandle<GameObject>);
			}
			if (this.Ctrl_TalentLegacyRank != null && this.Ctrl_TalentLegacyRank.gameObject.activeSelf)
			{
				this.Ctrl_TalentLegacyRank.OnClose();
				this.Ctrl_TalentLegacyRank.gameObject.SetActiveSafe(false);
			}
			if (this.Ctrl_TalentLegacyTree != null && this.Ctrl_TalentLegacyTree.gameObject.activeSelf)
			{
				this.Ctrl_TalentLegacyTree.OnClose();
				this.Ctrl_TalentLegacyTree.gameObject.SetActiveSafe(false);
			}
			GuideController.Instance.CustomizeStringTrigger("TalentLegacyRank");
			this.Ctrl_TalentLegacyMain.gameObject.SetActiveSafe(true);
			this.Ctrl_TalentLegacyMain.OnShow();
		}

		private async void OnOpenTalentLegacyTree(object obj, int type, BaseEventArgs args)
		{
			EventArgsOpenTree openTree = (EventArgsOpenTree)args;
			if (openTree != null)
			{
				this.Obj_Talent.SetActiveSafe(false);
				this.Obj_TalentBg.SetActiveSafe(false);
				this.btnEvolutionPreview.gameObject.SetActiveSafe(false);
				this.MaskCommon.SetActiveSafe(true);
				if (this.Ctrl_TalentLegacyTree == null)
				{
					AsyncOperationHandle<GameObject> handle = GameApp.Resources.LoadAssetAsync<GameObject>("Assets/_Resources/Prefab/UI/TalentLegacy/TalentLegacyTree.prefab");
					await handle.Task;
					if (handle.Status != 1)
					{
						return;
					}
					GameObject gameObject = Object.Instantiate<GameObject>(handle.Result, this.Obj_TalentLegacy.transform);
					this.Ctrl_TalentLegacyTree = gameObject.GetComponent<TalentLegacyTree>();
					this.Ctrl_TalentLegacyTree.Init();
					handle = default(AsyncOperationHandle<GameObject>);
				}
				if (this.Ctrl_TalentLegacyRank != null && this.Ctrl_TalentLegacyRank.gameObject.activeSelf)
				{
					this.Ctrl_TalentLegacyRank.OnClose();
					this.Ctrl_TalentLegacyRank.gameObject.SetActiveSafe(false);
				}
				if (this.Ctrl_TalentLegacyMain != null && this.Ctrl_TalentLegacyMain.gameObject.activeSelf)
				{
					this.Ctrl_TalentLegacyMain.OnClose();
					this.Ctrl_TalentLegacyMain.gameObject.SetActiveSafe(false);
				}
				GuideController.Instance.CustomizeStringTrigger("ClickTalentLegacyNode");
				this.MaskCommon.SetActiveSafe(false);
				this.Ctrl_TalentLegacyTree.gameObject.SetActiveSafe(true);
				this.Ctrl_TalentLegacyTree.OnShow(openTree.CareerId, openTree.TalentLegacyNodeId);
			}
		}

		private void OnFunctionOpen(object obj, int type, BaseEventArgs args)
		{
			this.IsCheckTalentMaxLevel(true);
		}

		private void FreshSkin()
		{
			if (!this.Skin_ModelItem.IsCameraShow)
			{
				return;
			}
			this.Skin_ModelItem.OnShow();
			if (!this.Skin_ModelItem.RefreshPlayerSkins(null))
			{
				this.Skin_ModelItem.ShowSelfPlayerModel(DataName.TalentDataModule.ToString() + "_ModelNodeCtrl", false);
			}
		}

		[ContextMenu("Test")]
		private void Test()
		{
			int talentExp = this.talentDataModule.TalentExp;
			this.curProgressNode.PlayProgressAnimation(true, talentExp, new Action<TalentProgressNode, int>(this.ShowTalentReward));
		}

		private IEnumerator PlayProgressNodeAnimation(int exp, float delayTime)
		{
			this.inAnimationPlaying = true;
			yield return new WaitForSeconds(delayTime);
			this.curProgressNode.PlayProgressAnimation(true, exp, new Action<TalentProgressNode, int>(this.ShowTalentReward));
			yield break;
		}

		private IEnumerator ShowTalentRewardCallback(TalentProgressNode progressNode, int triggerTalentProgressId)
		{
			TalentNew_talent talentProgressTable = GameApp.Table.GetManager().GetTalentNew_talentModelInstance().GetElementById(triggerTalentProgressId);
			if (talentProgressTable == null)
			{
				this.TryPlayCacheData();
				yield break;
			}
			this.UpdateTitle();
			if (progressNode.curExp >= progressNode.data.maxLevel && !progressNode.data.isTheEndProgress)
			{
				TalentProgressNode oldProgressNode = this.curProgressNode;
				RectTransform rectTransform = oldProgressNode.transform as RectTransform;
				float x = rectTransform.anchoredPosition.x;
				this.curProgressNode.imgFillR.gameObject.SetActive(true);
				this.curProgressNode = this.CreateProgress();
				RectTransform rectTransform2 = this.curProgressNode.transform as RectTransform;
				ShortcutExtensions46.DOAnchorPosX(rectTransform2, x + 1080f, 0f, false);
				ShortcutExtensions46.DOAnchorPosX(rectTransform, x - 1080f, 0.6f, false);
				ShortcutExtensions46.DOAnchorPosX(rectTransform2, x, 0.6f, false);
				DelayCall.Instance.CallOnce(1200, delegate
				{
					this.DestroyProgress(oldProgressNode);
				});
				yield return new WaitForSeconds(0.6f);
				this.curProgressNode.PlayOpenAnimation();
			}
			if (!progressNode.data.isTheEndProgress)
			{
				this.UpdateAttributeItems();
				if (talentProgressTable.evolution > 0)
				{
					for (int i = 0; i < this.attributeLevelUpItems.Count; i++)
					{
						if (this != null && base.gameObject != null && base.gameObject.activeSelf)
						{
							base.StartCoroutine(this.attributeLevelUpItems[i].PlayOpenAnimation(i * 7));
						}
					}
					yield return new WaitForSeconds(0.5f);
				}
				if (progressNode.curExp >= progressNode.data.maxLevel)
				{
					this.TryPlayCacheData();
				}
				else
				{
					if (this == null || base.gameObject == null || !base.gameObject.activeSelf)
					{
						yield break;
					}
					base.StartCoroutine(this.PlayProgressNodeAnimation(progressNode.targetExp, 0f));
				}
			}
			else
			{
				progressNode.gameObject.SetActive(false);
				this.attributeGridLayoutGroup.gameObject.SetActive(false);
				if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.TalentLegacy, false) && !Singleton<GameFunctionController>.Instance.CheckNewFunctionOpen())
				{
					this.IsCheckTalentMaxLevel(true);
				}
			}
			yield break;
		}

		private void ShowTalentReward(TalentProgressNode progressNode, int triggerTalentProgressId)
		{
			if (triggerTalentProgressId <= 0)
			{
				this.TryPlayCacheData();
				return;
			}
			Action action = delegate
			{
				if (this == null || this.gameObject == null || !this.gameObject.activeSelf)
				{
					return;
				}
				this.StartCoroutine(this.ShowTalentRewardCallback(progressNode, triggerTalentProgressId));
			};
			TalentNew_talent elementById = GameApp.Table.GetManager().GetTalentNew_talentModelInstance().GetElementById(triggerTalentProgressId);
			if (elementById == null)
			{
				if (action != null)
				{
					action();
				}
				return;
			}
			if (elementById.evolution > 0)
			{
				TalentStateUpgradeResultViewModule.OpenData openData = new TalentStateUpgradeResultViewModule.OpenData();
				openData.talentId = elementById.id;
				openData.closeCallback = action;
				openData.rewardType = elementById.rewardType;
				openData.reward = elementById.reward;
				openData.isBig = GameApp.Table.GetManager().GetTalentNew_talentEvolution(elementById.evolution).type == 3;
				GameApp.View.OpenView(ViewName.TalentStateUpgradeResultViewModule, openData, 1, null, null);
				return;
			}
			if (elementById.rewardType == 1)
			{
				List<ItemData> list = elementById.reward.ToItemDataList();
				if (this.rewardCache.TryGetValue(elementById.id, out list))
				{
					DxxTools.UI.OpenRewardCommon(list, action, true);
					return;
				}
				DxxTools.UI.OpenRewardCommon(list, action, true);
				return;
			}
			else
			{
				if (elementById.rewardType == 2 || elementById.rewardType == 3 || elementById.rewardType == 4)
				{
					TalentSkillRewardViewModule.OpenData openData2 = new TalentSkillRewardViewModule.OpenData();
					openData2.atlasId = elementById.iconAtlasID;
					openData2.iconName = elementById.iconID;
					openData2.title = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.talentName);
					openData2.desc = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.talentDesc);
					openData2.closeCallback = action;
					GameApp.View.OpenView(ViewName.TalentSkillRewardViewModule, openData2, 1, null, null);
					return;
				}
				if (action != null)
				{
					action();
				}
				return;
			}
		}

		private void TryPlayCacheData()
		{
			for (int i = 0; i < this.attributeLevelUpItems.Count; i++)
			{
				if (this.attributeLevelUpItems[i].gameObject.activeSelf)
				{
					this.attributeLevelUpItems[i].UpdateLevel();
				}
			}
			if (this.cacheResData != null)
			{
				TalentsLvUpResponse talentsLvUpResponse = this.cacheResData;
				this.cacheResData = null;
				base.StartCoroutine(this.PlayProgressNodeAnimation((int)talentsLvUpResponse.TalentsInfo.ExpProcess, 0f));
				return;
			}
			this.inAnimationPlaying = false;
			this.needBlockClick = false;
			this.rewardCache.Clear();
			Action aniUpgradeCallBack = this.m_aniUpgradeCallBack;
			if (aniUpgradeCallBack != null)
			{
				aniUpgradeCallBack();
			}
			this.m_aniUpgradeCallBack = null;
		}

		private void DoAttributeUpgradeCallback(TalentAttributeItem item, TalentsLvUpResponse data, Action callBack = null)
		{
			if (data == null || data.TalentsInfo == null)
			{
				return;
			}
			if (this == null || base.gameObject == null)
			{
				return;
			}
			this.m_aniUpgradeCallBack = callBack;
			if (data.CommonData != null && data.CommonData.Reward != null && data.CommonData.Reward.Count > 0)
			{
				List<ItemData> list = data.CommonData.Reward.ToItemDataList();
				this.rewardCache[data.RewardConfigId] = list;
			}
			if ((ulong)data.TalentsInfo.ExpProcess < (ulong)((long)this.talentDataModule.TalentExp))
			{
				return;
			}
			if (data.AddLvAndExp <= 0U)
			{
				return;
			}
			int num = this.curTotalExp;
			this.curTotalExp = data.TalentsInfo.GetTotalExp(GameApp.Table.GetManager());
			int progressId = this.talentDataModule.GetProgressId(num);
			if (this.talentDataModule.GetProgressId(this.curTotalExp) > progressId)
			{
				this.needBlockClick = true;
			}
			this.roleEffectGo.SetActive(false);
			this.roleEffectGo.SetActive(true);
			item.PlayLevelUpAnimation((int)data.CritType);
			for (int i = 0; i < this.attributeLevelUpItems.Count; i++)
			{
				if (this.attributeLevelUpItems[i].gameObject.activeSelf)
				{
					this.attributeLevelUpItems[i].UpdateLevel();
				}
			}
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			CustomImage imgExp = Object.Instantiate<CustomImage>(this.curProgressNode.imgExp, this.curProgressNode.expNode.transform.parent, false);
			imgExp.transform.position = item.txtLevel.transform.position;
			Tweener tweener = ShortcutExtensions.DOMove(imgExp.transform, this.curProgressNode.txtExp.transform.position, 0.5f, false);
			TweenSettingsExtensions.OnUpdate<Tweener>(tweener, delegate
			{
				if (this.curProgressNode != null)
				{
					tweener.ChangeEndValue(this.curProgressNode.txtExp.transform.position, true);
				}
			});
			imgExp.transform.GetComponentInChildren<CustomText>().text = string.Format("+{0}", (int)data.AddLvAndExp);
			DelayCall.Instance.CallOnce(450, delegate
			{
				if (imgExp != null)
				{
					ShortcutExtensions.DOScale(imgExp.transform, 0f, 0.3f);
				}
			});
			DelayCall.Instance.CallOnce(500, delegate
			{
				if (imgExp != null)
				{
					Object.Destroy(imgExp.gameObject);
				}
			});
			if (this.inAnimationPlaying)
			{
				this.cacheResData = data;
				return;
			}
			if (this == null || base.gameObject == null || !base.gameObject.activeSelf)
			{
				return;
			}
			base.StartCoroutine(this.PlayProgressNodeAnimation((int)data.TalentsInfo.ExpProcess, 0.5f));
		}

		public ModuleCurrencyCtrl moduleCurrencyCtrl;

		public CustomText txtTalentTitle;

		public CustomButton btnEvolutionPreview;

		public TalentProgressNode prefabProgressNode;

		public UIModelItem Skin_ModelItem;

		public GameObject roleEffectGo;

		public GridLayoutGroup attributeGridLayoutGroup;

		public TalentAttributeItem prefabAttributeItem;

		public UIHelpButton helpButton;

		public GameObject MaskCommon;

		public GameObject Obj_TalentBg;

		public GameObject Obj_Talent;

		public GameObject Obj_TalentLegacy;

		private TalentLegacyRank Ctrl_TalentLegacyRank;

		private TalentLegacyMain Ctrl_TalentLegacyMain;

		public TalentLegacyTree Ctrl_TalentLegacyTree;

		private CardData cardData;

		private HeroDataModule heroDataModule;

		private AddAttributeDataModule addAttributeDataModule;

		private List<float> attributeItemWidthList = new List<float> { 286f, 286f, 286f, 244f, 204f };

		private List<float> attributeItemSpaceList = new List<float> { 0f, 50f, 50f, 18f, 10f };

		private List<float> attributeEffectScaleList = new List<float> { 1f, 1f, 1f, 0.9f, 0.75f };

		private List<TalentAttributeItem> attributeLevelUpItems = new List<TalentAttributeItem>();

		private TalentDataModule talentDataModule;

		private TalentData curTalentData;

		private bool inAnimationPlaying;

		private bool inNetWaiting;

		private bool inOpenAnimationPlaying;

		private Vector2 btnPreviewOriginPos;

		private TalentProgressNode curProgressNode;

		private Coroutine playUIOpenAnimationCoroutine;

		private bool m_isMaxLevel;

		private UIBaseMainPageNode.OpenData m_mainOpenData;

		private bool inProgressAnimationPlaying;

		private float lastClickTime;

		private int curTotalExp;

		private bool needBlockClick;

		private TalentsLvUpResponse cacheResData;

		private Dictionary<int, List<ItemData>> rewardCache = new Dictionary<int, List<ItemData>>();

		private Action m_aniUpgradeCallBack;
	}
}
