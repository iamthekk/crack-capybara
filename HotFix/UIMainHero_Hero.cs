using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Actor;
using Server;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMainHero_Hero : BaseMainHeroPanel
	{
		protected override void OnInit()
		{
			this.m_dataModule = GameApp.Data.GetDataModule(DataName.HeroLevelUpDataModule);
			this.m_bottom.OnReqLevelUp = new Action(this.OnClickLevelUpBtn);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			if (this.m_center != null)
			{
				this.m_center.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		protected override void OnDeInit()
		{
		}

		public override void OnShow()
		{
			if (this.m_top != null)
			{
				this.m_top.Init();
			}
			if (this.m_center != null)
			{
				this.m_center.Init();
			}
			if (this.m_bottom != null)
			{
				this.m_bottom.Init();
			}
			if (this.m_attributeBt != null)
			{
				this.m_attributeBt.onClick.AddListener(new UnityAction(this.OnClickAttributeBt));
			}
			this.RefreshTop();
			this.RefreshCenter(false);
			this.RefreshBottom();
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCurrency, Singleton<EventArgsBool>.Instance.SetData(false));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_HeroLevelUpDataModule_RefreshData, new HandlerEvent(this.OnEventRefreshData));
		}

		public override void OnHide()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_HeroLevelUpDataModule_RefreshData, new HandlerEvent(this.OnEventRefreshData));
			if (this.m_attributeBt != null)
			{
				this.m_attributeBt.onClick.RemoveListener(new UnityAction(this.OnClickAttributeBt));
			}
			if (this.m_top != null)
			{
				this.m_top.DeInit();
			}
			if (this.m_center != null)
			{
				this.m_center.DeInit();
			}
			if (this.m_bottom != null)
			{
				this.m_bottom.DeInit();
			}
		}

		public override void PlayAnimation()
		{
			if (this.m_aniamtor == null)
			{
				return;
			}
			this.m_aniamtor.SetTrigger("Run");
		}

		public void RefreshTop()
		{
			if (this.m_top == null)
			{
				return;
			}
			this.m_top.SetTitle(this.m_dataModule.GetTrainingTitleName(Color.yellow));
		}

		public void RefreshCenter(bool isUplevel = false)
		{
			if (this.m_center == null)
			{
				return;
			}
			if (!isUplevel)
			{
				this.m_center.ShowPlayerModel(this.m_dataModule.MainPlayCardData.m_memberID);
			}
			if (isUplevel)
			{
				this.m_center.SetOpenProgress(this.m_dataModule.GetShowFgCount());
				return;
			}
			this.m_center.SetProgress(this.m_dataModule.GetShowFgCount());
		}

		public void RefreshBottom()
		{
			if (this.m_bottom == null)
			{
				return;
			}
			if (this.m_dataModule.IsLevelFull())
			{
				this.m_bottom.SetActiveForLevelUpBtn(false);
				this.m_bottom.SetActiveForEvolutionUpBtn(false);
				this.m_bottom.RefreshCostGroup(null);
				return;
			}
			if (this.m_dataModule.IsGradeUp())
			{
				List<ItemData> gradeUpCost = this.m_dataModule.GetGradeUpCost();
				bool flag = this.m_dataModule.IsHaveGradeUpCost(gradeUpCost);
				this.m_bottom.SetActiveForLevelUpBtn(false);
				this.m_bottom.SetActiveForEvolutionUpBtn(true);
				this.m_bottom.SetEvolutionUpGray(!flag);
				this.m_bottom.RefreshCostGroup(gradeUpCost);
				return;
			}
			List<ItemData> levelUpCost = this.m_dataModule.GetLevelUpCost();
			bool flag2 = this.m_dataModule.IsHaveGradeUpCost(levelUpCost);
			this.m_bottom.SetActiveForLevelUpBtn(true);
			this.m_bottom.SetActiveForEvolutionUpBtn(false);
			this.m_bottom.SetLevelUpGray(!flag2);
			this.m_bottom.RefreshCostGroup(levelUpCost);
		}

		private void OnEventRefreshData(object sender, int type, BaseEventArgs eventargs)
		{
			this.RefreshTop();
			this.RefreshCenter(true);
			this.RefreshBottom();
		}

		private void OnClickAttributeBt()
		{
			AttributeDetailedViewModule.OpenData openData = new AttributeDetailedViewModule.OpenData();
			openData.m_titleLanguageID = "6302";
			if (this.m_dataModule.m_addAttributeData == null)
			{
				return;
			}
			openData.m_datas = new List<AttributeDetailedViewModule.Data>();
			List<AttributeDetailedViewModule.Data> list = new List<AttributeDetailedViewModule.Data>();
			List<MergeAttributeData> attributeDatas = this.m_dataModule.m_addAttributeData.m_attributeDatas;
			for (int i = 0; i < attributeDatas.Count; i++)
			{
				MergeAttributeData mergeAttributeData = attributeDatas[i];
				if (mergeAttributeData != null)
				{
					AttributeDetailedViewModule.Data data = new AttributeDetailedViewModule.Data();
					Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(mergeAttributeData.Header);
					data.m_name = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId);
					data.m_to = this.GetAttributeString(mergeAttributeData);
					data.m_type = AttributeDetailedViewModule.Type.Attrubute;
					list.Add(data);
				}
			}
			if (list.Count != 0)
			{
				MemberAttributeData memberAttributeData = new MemberAttributeData();
				memberAttributeData.MergeAttributes(attributeDatas, false);
				CombatData combatData = new CombatData();
				combatData.MathCombat(GameApp.Table.GetManager(), memberAttributeData, null);
				AttributeDetailedViewModule.Data data2 = new AttributeDetailedViewModule.Data();
				data2.m_name = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6301);
				data2.m_to = DxxTools.FormatNumber((long)combatData.CurComba);
				data2.m_type = AttributeDetailedViewModule.Type.Combat;
				openData.m_datas.Add(data2);
			}
			openData.m_datas.AddRange(list);
			GameApp.View.OpenView(ViewName.AttributeDetailedViewModule, openData, 1, null, null);
		}

		private void OnClickLevelUpBtn()
		{
			if (this.m_bottom == null)
			{
				return;
			}
			if (this.m_bottom.m_isLevelUpGray)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("100001807"));
				return;
			}
			List<MergeAttributeData> rewards = this.m_dataModule.GetLevelUpRewards();
			NetworkUtils.Actor.DoActorLevelUpRequest(delegate(bool isOk, ActorLevelUpResponse request)
			{
				if (!isOk)
				{
					return;
				}
				if (this.m_center != null)
				{
					string text = string.Empty;
					for (int i = 0; i < rewards.Count; i++)
					{
						MergeAttributeData mergeAttributeData = rewards[i];
						if (mergeAttributeData != null)
						{
							text = text + Singleton<LanguageManager>.Instance.GetInfoByID(GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(mergeAttributeData.Header)
								.LanguageId) + "+" + mergeAttributeData.Value.AsLong().ToString();
							if (i != rewards.Count - 1)
							{
								text += "\n";
							}
						}
					}
					this.m_center.GotoHeroUpState();
					this.m_center.m_tipController.AddNode(text);
				}
				if (this.m_top != null)
				{
					this.m_top.PlayTitle();
				}
			});
		}

		private string GetAttributeString(MergeAttributeData data)
		{
			if (data.Header.Contains("%"))
			{
				return data.Value.AsLong().ToString() + "%";
			}
			return DxxTools.FormatNumber(data.Value.AsLong());
		}

		[SerializeField]
		private Animator m_aniamtor;

		[SerializeField]
		private UIMainHero_HeroTopGroup m_top;

		[SerializeField]
		private UIMainHero_HeroCenterGroup m_center;

		[SerializeField]
		private UIMainHero_HeroBottomGroup m_bottom;

		[SerializeField]
		private CustomButton m_attributeBt;

		private HeroLevelUpDataModule m_dataModule;
	}
}
