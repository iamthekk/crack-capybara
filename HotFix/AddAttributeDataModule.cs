using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public class AddAttributeDataModule : IDataModule
	{
		public List<MergeAttributeData> AttributeDatas
		{
			get
			{
				return this.m_attributeDatas;
			}
			set
			{
				this.m_attributeDatas = value;
			}
		}

		public List<int> SkillIDs
		{
			get
			{
				return this.m_skillIDs;
			}
		}

		public double Combat
		{
			get
			{
				return this.m_combat;
			}
		}

		public CardData MainCardData
		{
			get
			{
				return this.m_mainCardData;
			}
		}

		public MemberAttributeData MemberAttributeData
		{
			get
			{
				return this.m_memberAttributeData;
			}
		}

		public int GetName()
		{
			return 117;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_AddAttributeData_RefreshDatas, new HandlerEvent(this.OnEventRefreshDatas));
			manager.RegisterEvent(LocalMessageName.CC_HeroLevelUpDataModule_RefreshData, new HandlerEvent(this.OnEventRefreshDatas));
			manager.RegisterEvent(LocalMessageName.CC_GameLoginData_UpdateRelicsData, new HandlerEvent(this.OnEventRefreshDatas));
			manager.RegisterEvent(LocalMessageName.CC_UITalent_RefreshData, new HandlerEvent(this.OnEventRefreshDatas));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_AddAttributeData_RefreshDatas, new HandlerEvent(this.OnEventRefreshDatas));
			manager.UnRegisterEvent(LocalMessageName.CC_HeroLevelUpDataModule_RefreshData, new HandlerEvent(this.OnEventRefreshDatas));
			manager.UnRegisterEvent(LocalMessageName.CC_GameLoginData_UpdateRelicsData, new HandlerEvent(this.OnEventRefreshDatas));
			manager.UnRegisterEvent(LocalMessageName.CC_UITalent_RefreshData, new HandlerEvent(this.OnEventRefreshDatas));
		}

		public void Reset()
		{
			this.m_attributeDatas.Clear();
			this.m_skillIDs.Clear();
			this.m_combat = 0.0;
			this.m_mainCardData = null;
			this.m_memberAttributeData = new MemberAttributeData();
		}

		private void MathfMainPlayerAttributeDatas(bool isSyncPower)
		{
			this.m_memberAttributeData = new MemberAttributeData();
			this.m_attributeDatas.Clear();
			this.m_skillIDs.Clear();
			AddAttributeData addAttributeData = new AddAttributeData();
			HeroDataModule dataModule = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			if (dataModule != null && dataModule.MainCardData != null && dataModule.MainCardData.m_memberID > 0)
			{
				GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(dataModule.MainCardData.m_memberID);
				List<MergeAttributeData> mergeAttributeData = elementById.baseAttributes.GetMergeAttributeData();
				addAttributeData.m_attributeDatas = mergeAttributeData.Merge();
				this.m_skillIDs.AddRange(elementById.skillIDs.GetListInt('|'));
			}
			HeroLevelUpDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.HeroLevelUpDataModule);
			addAttributeData.Merge(dataModule2.m_addAttributeData);
			EquipDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.EquipDataModule);
			addAttributeData.Merge(dataModule3.m_addAttributeData);
			PetDataModule dataModule4 = GameApp.Data.GetDataModule(DataName.PetDataModule);
			addAttributeData.Merge(dataModule4.m_addAttributeData);
			RelicDataModule dataModule5 = GameApp.Data.GetDataModule(DataName.RelicDataModule);
			addAttributeData.Merge(dataModule5.m_addAttributeData);
			TalentDataModule dataModule6 = GameApp.Data.GetDataModule(DataName.TalentDataModule);
			addAttributeData.Merge(dataModule6.m_addAttributeData);
			TalentLegacyDataModule dataModule7 = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			addAttributeData.Merge(dataModule7.m_addAttributeData);
			MountDataModule dataModule8 = GameApp.Data.GetDataModule(DataName.MountDataModule);
			addAttributeData.Merge(dataModule8.AddAttributeData);
			ArtifactDataModule dataModule9 = GameApp.Data.GetDataModule(DataName.ArtifactDataModule);
			addAttributeData.Merge(dataModule9.AddAttributeData);
			CollectionDataModule dataModule10 = GameApp.Data.GetDataModule(DataName.CollectionDataModule);
			addAttributeData.Merge(dataModule10.m_addAttributeData);
			addAttributeData.m_attributeDatas = addAttributeData.m_attributeDatas.Merge();
			this.m_attributeDatas = addAttributeData.m_attributeDatas;
			this.m_skillIDs.AddRange(addAttributeData.m_skillIDs);
			this.m_memberAttributeData.MergeAttributes(this.AttributeDatas, false);
			this.m_mainCardData = new CardData();
			this.m_mainCardData.CloneFrom(dataModule.MainCardData);
			this.m_mainCardData.m_isMainMember = true;
			this.m_mainCardData.m_memberAttributeData = this.m_memberAttributeData;
			this.m_mainCardData.UpdateSkills(this.SkillIDs);
			this.m_mainCardData.SetMemberRace(MemberRace.Hero);
			this.m_combatData = new CombatData();
			this.m_combatData.MathCombat(GameApp.Table.GetManager(), this.m_memberAttributeData, this.m_skillIDs);
			this.m_combat = this.m_combatData.CurComba;
			if (isSyncPower)
			{
				NetworkUtils.MainCity.DoCitySyncPowerRequest((long)this.m_combat);
			}
			GameApp.Data.GetDataModule(DataName.TicketDataModule).CalcRealRecoverTime();
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Attribute);
		}

		private void OnEventRefreshDatas(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsBool eventArgsBool = eventargs as EventArgsBool;
			bool flag = eventArgsBool == null || eventArgsBool.Value;
			double combat = this.m_combat;
			this.MathfMainPlayerAttributeDatas(flag);
			if (sender is LoginDataModule || sender is TalentLegacyDataModule)
			{
				return;
			}
			if (GameApp.State.GetCurrentStateName() != 103)
			{
				return;
			}
			EventArgsAddCombatTipNode eventArgsAddCombatTipNode = new EventArgsAddCombatTipNode();
			eventArgsAddCombatTipNode.m_from = (long)combat;
			eventArgsAddCombatTipNode.m_to = (long)this.m_combat;
			if (eventArgsAddCombatTipNode.m_from == eventArgsAddCombatTipNode.m_to)
			{
				return;
			}
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_TipViewModule_AddAddCombatTipNode, eventArgsAddCombatTipNode);
		}

		[SerializeField]
		private List<MergeAttributeData> m_attributeDatas = new List<MergeAttributeData>();

		[SerializeField]
		private List<int> m_skillIDs = new List<int>();

		[SerializeField]
		private double m_combat;

		[SerializeField]
		private CardData m_mainCardData;

		private MemberAttributeData m_memberAttributeData = new MemberAttributeData();

		private CombatData m_combatData;
	}
}
