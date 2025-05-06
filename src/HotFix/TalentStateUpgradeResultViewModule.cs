using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public class TalentStateUpgradeResultViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.btnClose.enabled = false;
			this.roleBackEffectGo.SetActive(false);
			this.roleFrontEffectGo.SetActive(false);
			this.roleFrontEffectGoMin.SetActive(false);
			this.talentDataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
			this.heroDataModule = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			this.addAttributeDataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
		}

		public override void OnDelete()
		{
		}

		public override void OnOpen(object data)
		{
			try
			{
				this.openData = data as TalentStateUpgradeResultViewModule.OpenData;
				if (this.openData == null)
				{
					this.OnBtnCloseClick();
				}
				else
				{
					this.RefreshCardData(this.heroDataModule.MainCardData);
					this.Skin_ModelItem.Init();
					GameApp.Data.GetDataModule(DataName.ClothesDataModule).PushUIModelItem(this.Skin_ModelItem, new Action(this.FreshSkin));
					if (this.openData.isBig)
					{
						this.Skin_ModelItem_Next.Init();
						this.FreshSkin_Next();
					}
					this.Animator_Model.Play(this.openData.isBig ? "LevelUpBig" : "Empty");
					this.UpdateView();
					base.StartCoroutine(this.PlayOpenAnimation());
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				this.btnClose.enabled = true;
			}
		}

		public override void OnClose()
		{
			if (this.openData != null && this.openData.closeCallback != null)
			{
				this.openData.closeCallback();
			}
			GameApp.Data.GetDataModule(DataName.ClothesDataModule).PopUIModelItem(this.Skin_ModelItem);
			this.Skin_ModelItem.OnHide(false);
			this.Skin_ModelItem.DeInit();
			if (this.openData != null && this.openData.isBig)
			{
				this.Skin_ModelItem_Next.OnHide(false);
				this.Skin_ModelItem_Next.DeInit();
			}
		}

		private IEnumerator PlayOpenAnimation()
		{
			GameApp.Sound.PlayClip(645, 1f);
			this.nodeRole.localPosition = new Vector3(0f, -185f, 0f);
			this.roleBackEffectGo.SetActive(true);
			if (this.openData.isBig)
			{
				this.roleFrontEffectGo.SetActive(true);
			}
			else
			{
				this.roleFrontEffectGoMin.SetActive(true);
			}
			if (this.openData.isBig)
			{
				yield return new WaitForSeconds(this.effectDuration);
				this.roleFrontEffectGo.SetActive(false);
			}
			else
			{
				yield return new WaitForSeconds(this.effectDurationMin);
				this.roleFrontEffectGoMin.SetActive(false);
			}
			TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(this.nodeRole, 0f, 0.5f, false), 27);
			yield return new WaitForSeconds(0.5f);
			GameApp.Sound.PlayClip(646, 1f);
			this.uiAnimator.Play("Show");
			yield return new WaitForSeconds(0.3f);
			this.btnClose.enabled = true;
			yield break;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.btnClose.m_onClick = new Action(this.OnBtnCloseClick);
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.btnClose.m_onClick = null;
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.TalentStateUpgradeResultViewModule, null);
		}

		private void UpdateView()
		{
			TalentNew_talent elementById = GameApp.Table.GetManager().GetTalentNew_talentModelInstance().GetElementById(this.openData.talentId);
			TalentNew_talentEvolution elementById2 = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(elementById.evolution - 1);
			if (elementById == null || elementById2 == null)
			{
				return;
			}
			List<MergeAttributeData> mergeAttributeData = elementById2.evolutionAttributes.GetMergeAttributeData();
			AddAttributeDataModule dataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			long num = 0L;
			TalentAttributeChangeItem talentAttributeChangeItem = null;
			for (int i = 0; i < mergeAttributeData.Count; i++)
			{
				long num2 = mergeAttributeData[i].Value.AsLong();
				string header = mergeAttributeData[i].Header;
				if (!(header == "Attack"))
				{
					if (!(header == "Defence"))
					{
						if (header == "HPMax")
						{
							talentAttributeChangeItem = this.attributeChangeItemHp;
							num = dataModule.MemberAttributeData.GetHpMax4UI();
						}
					}
					else
					{
						talentAttributeChangeItem = this.attributeChangeItemDefense;
						num = dataModule.MemberAttributeData.GetDefence4UI();
					}
				}
				else
				{
					talentAttributeChangeItem = this.attributeChangeItemAtk;
					num = dataModule.MemberAttributeData.GetAttack4UI();
				}
				if (talentAttributeChangeItem != null)
				{
					long num3 = num - num2;
					num3 = Utility.Math.Max(0L, num3);
					talentAttributeChangeItem.SetData(num3, num, num2);
				}
			}
			int num4 = elementById.evolution - 1;
			int evolution = elementById.evolution;
			TalentNew_talentEvolution elementById3 = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(num4);
			TalentNew_talentEvolution elementById4 = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(evolution);
			this.txtStageValueOld.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById3.stepLanguageId);
			this.txtStageValueNew.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById4.stepLanguageId);
		}

		private void RefreshCardData(CardData cardData)
		{
			this.cardData = new CardData();
			this.cardData.CloneFrom(cardData);
			this.cardData.UpdateAttribute(this.addAttributeDataModule.AttributeDatas);
		}

		[GameTestMethod("天赋", "天赋升阶结算界面", "", 0)]
		private static void OnTest()
		{
			TalentDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
			if (dataModule == null)
			{
				HLog.LogError("当前没有天赋数据");
				return;
			}
			TalentStateUpgradeResultViewModule.OpenData openData = new TalentStateUpgradeResultViewModule.OpenData();
			openData.talentId = dataModule.TalentStage;
			int num = dataModule.talentProgressData.curId;
			for (;;)
			{
				TalentNew_talent elementById = GameApp.Table.GetManager().GetTalentNew_talentModelInstance().GetElementById(num);
				if (elementById == null)
				{
					goto IL_0076;
				}
				if (elementById.rewardType == 2 || elementById.rewardType == 3)
				{
					break;
				}
				num++;
			}
			openData.talentId = num;
			IL_0076:
			GameApp.View.OpenView(ViewName.TalentStateUpgradeResultViewModule, openData, 1, null, null);
		}

		private void FreshSkin()
		{
			if (!this.Skin_ModelItem.IsCameraShow)
			{
				return;
			}
			this.Skin_ModelItem.OnShow();
			this.Skin_ModelItem.ShowSelfPlayerModel(DataName.TalentDataModule.ToString() + "Upgrade_ModelNodeCtrl", false);
		}

		public void FreshSkin_End()
		{
			if (!this.Skin_ModelItem.IsCameraShow)
			{
				return;
			}
			this.Skin_ModelItem.OnShow();
			this.Skin_ModelItem.RefreshPlayerSkins(this.GetNextSkinDatas());
		}

		private void FreshSkin_Next()
		{
			if (!this.Skin_ModelItem_Next.IsCameraShow)
			{
				return;
			}
			int memberID = GameApp.Data.GetDataModule(DataName.HeroDataModule).MainCardData.m_memberID;
			int weaponId = GameApp.Data.GetDataModule(DataName.EquipDataModule).GetWeaponId();
			this.Skin_ModelItem_Next.OnShow();
			this.Skin_ModelItem_Next.ShowPlayerModel(DataName.TalentDataModule.ToString() + "UpgradeNext_ModelNodeCtrlBig", memberID, weaponId, this.GetNextSkinDatas(), 0);
		}

		private Dictionary<SkinType, SkinData> GetNextSkinDatas()
		{
			ClothesData clothesData = GameApp.Data.GetDataModule(DataName.ClothesDataModule).CopySelfClothesData();
			string[] array = GameApp.Table.GetManager().GetTalentNew_talent(this.openData.talentId).reward.Split('|', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				int num = int.Parse(array[i].Split(',', StringSplitOptions.None)[0]);
				if (num > 0)
				{
					int num2 = int.Parse(GameApp.Table.GetManager().GetItem_Item(num).itemTypeParam[0]);
					Avatar_Skin avatar_Skin = GameApp.Table.GetManager().GetAvatar_Skin(num2);
					clothesData.DressPart((SkinType)avatar_Skin.part, num2);
				}
			}
			return clothesData.GetSkinDatas();
		}

		public Animator uiAnimator;

		public CustomButton btnClose;

		public UIModelItem Skin_ModelItem;

		public UIModelItem Skin_ModelItem_Next;

		public Animator Animator_Model;

		public Transform nodeRole;

		public GameObject roleBackEffectGo;

		public GameObject roleFrontEffectGo;

		public GameObject roleFrontEffectGoMin;

		public float effectDuration = 2f;

		public float effectDurationMin = 0.5f;

		public CustomText txtStageValueOld;

		public CustomText txtStageValueNew;

		public Transform nodeStage;

		public TalentAttributeChangeItem attributeChangeItemAtk;

		public TalentAttributeChangeItem attributeChangeItemDefense;

		public TalentAttributeChangeItem attributeChangeItemHp;

		private TalentStateUpgradeResultViewModule.OpenData openData;

		private CardData cardData;

		private TalentDataModule talentDataModule;

		private HeroDataModule heroDataModule;

		private AddAttributeDataModule addAttributeDataModule;

		public class OpenData
		{
			public int talentId;

			public int rewardType;

			public string reward;

			public bool isBig;

			public Action closeCallback;
		}
	}
}
