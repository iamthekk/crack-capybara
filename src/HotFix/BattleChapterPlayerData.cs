using System;
using System.Collections.Generic;
using System.Text;
using Framework;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class BattleChapterPlayerData
	{
		public int PlayerMemberId { get; private set; }

		public MemberAttributeData AttributeData { get; private set; }

		public FP CurrentHp { get; private set; }

		public FP CurrentRecharge { get; private set; }

		public FP HpMax { get; private set; }

		public FP Attack { get; private set; }

		public FP Defence { get; private set; }

		public int Food { get; private set; }

		public int BeginFood { get; private set; }

		public int MaxExpLevel { get; private set; }

		public long PlayerCoin { get; private set; }

		public int Diamond { get; private set; }

		public bool IsUsedRevive { get; private set; }

		public int BattleEndEffectActiveNum { get; set; }

		public int ReviveCount { get; private set; }

		public int RefreshSkillCount { get; private set; }

		public int PlayBigBonusCount { get; private set; }

		public int PlayMinorBonusCount { get; private set; }

		public int NextExp
		{
			get
			{
				Chapter_skillExp elementById = GameApp.Table.GetManager().GetChapter_skillExpModelInstance().GetElementById(Singleton<GameEventController>.Instance.PlayerData.ExpLevel.mVariable);
				if (elementById != null)
				{
					return elementById.expToNext;
				}
				return 0;
			}
		}

		public List<CardData> PetCards { get; private set; }

		public BattleChapterPlayerData(int playerMemberId, MemberAttributeData playerAttribute, List<int> skillIds, List<CardData> petCards)
		{
			this.PlayerMemberId = playerMemberId;
			this.AttributeData = new MemberAttributeData();
			this.AttributeData.CopyFrom(playerAttribute);
			this.AttributeData.ConvertBaseData();
			for (int i = 0; i < skillIds.Count; i++)
			{
				this.playerSkills.Add(skillIds[i]);
			}
			this.SetEventAttributes();
			GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(this.PlayerMemberId);
			this.CurrentHp = this.AttributeData.GetHpMax();
			this.CurrentRecharge = elementById.recharge;
			MemberAttributeData attributeData = this.AttributeData;
			attributeData.m_onHpMaxUpdate = (Action<FP>)Delegate.Combine(attributeData.m_onHpMaxUpdate, new Action<FP>(this.OnMaxHpChanged));
			this.IsUsedRevive = false;
			this.PetCards = new List<CardData>();
			if (petCards != null)
			{
				for (int j = 0; j < petCards.Count; j++)
				{
					CardData cardData = new CardData();
					cardData.CloneFrom(petCards[j]);
					this.PetCards.Add(cardData);
				}
			}
			this.BeginFood = this.Food;
			this.Chips = new SecureVariable();
			this.Chips.UpdateVariable(0);
			IList<Chapter_skillExp> allElements = GameApp.Table.GetManager().GetChapter_skillExpModelInstance().GetAllElements();
			this.MaxExpLevel = allElements[allElements.Count - 1].id;
			this.ExpLevel = new SecureVariable();
			this.ExpLevel.UpdateVariable(1);
			this.Exp = new SecureVariable();
			this.Exp.UpdateVariable(0);
			this.BattleEndEffectActiveNum = 0;
			this.ReviveCount = 0;
			this.RefreshSkillCount = 0;
			this.PlayBigBonusCount = 0;
			this.PlayMinorBonusCount = 0;
			this.PlayerCoin = 0L;
			this.Diamond = 0;
			EventArgAddSkills eventArgAddSkills = new EventArgAddSkills();
			eventArgAddSkills.SetData(this.playerSkills);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleAddSkill, eventArgAddSkills);
		}

		public void ClearData()
		{
			this.IsUsedRevive = false;
			MemberAttributeData attributeData = this.AttributeData;
			attributeData.m_onHpMaxUpdate = (Action<FP>)Delegate.Remove(attributeData.m_onHpMaxUpdate, new Action<FP>(this.OnMaxHpChanged));
			this.playerSkillBuildList.Clear();
			this.playerSkills.Clear();
		}

		public List<GameEventSkillBuildData> GetPlayerSkillBuildList()
		{
			return this.playerSkillBuildList;
		}

		public MemberAttributeData GetBattleAttribute()
		{
			return this.AttributeData;
		}

		public List<int> GetPlayerSkills()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.playerSkills.Count; i++)
			{
				list.Add(this.playerSkills[i]);
			}
			for (int j = 0; j < this.playerSkillBuildList.Count; j++)
			{
				list.Add(this.playerSkillBuildList[j].skillId);
			}
			return list;
		}

		public List<int> GetEventSkillBuildIds()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.playerSkillBuildList.Count; i++)
			{
				list.Add(this.playerSkillBuildList[i].id);
			}
			return list;
		}

		public List<int> GetEventSkillIDs()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.playerSkillBuildList.Count; i++)
			{
				list.Add(this.playerSkillBuildList[i].skillId);
			}
			return list;
		}

		public void AddRecoverSkillBuild(GameEventSkillBuildData data)
		{
			if (data == null)
			{
				return;
			}
			this.playerSkillBuildList.Add(data);
			EventArgAddSkills eventArgAddSkills = new EventArgAddSkills();
			eventArgAddSkills.SetData(new List<int> { data.skillId });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleAddSkill, eventArgAddSkills);
		}

		public void AddSkillBuild(GameEventSkillBuildData data, bool isNew = true)
		{
			if (data == null)
			{
				return;
			}
			this.playerSkillBuildList.Add(data);
			this.UpdateAttribute(data.GetMergeAttributeData(), false);
			if (isNew && data.config.recoverHp > 0)
			{
				this.UpdateAttribute(GameEventAttType.RecoverHpRate, (double)data.config.recoverHp);
			}
			EventArgAddSkills eventArgAddSkills = new EventArgAddSkills();
			eventArgAddSkills.SetData(new List<int> { data.skillId });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleAddSkill, eventArgAddSkills);
		}

		public void ReplaceSkillBuild(GameEventSkillBuildData oldData, GameEventSkillBuildData newData)
		{
			int num = this.playerSkillBuildList.IndexOf(oldData);
			this.playerSkillBuildList[num] = newData;
			this.UpdateAttribute(oldData.GetMergeAttributeData(), true);
			this.UpdateAttribute(newData.GetMergeAttributeData(), false);
			EventArgRemoveSkills eventArgRemoveSkills = new EventArgRemoveSkills();
			eventArgRemoveSkills.SetData(new List<int> { oldData.skillId });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleRemoveSkill, eventArgRemoveSkills);
			EventArgAddSkills eventArgAddSkills = new EventArgAddSkills();
			eventArgAddSkills.SetData(new List<int> { newData.skillId });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleAddSkill, eventArgAddSkills);
		}

		public void RemoveSkillBuild(int buildId)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.playerSkillBuildList.Count; i++)
			{
				GameEventSkillBuildData gameEventSkillBuildData = this.playerSkillBuildList[i];
				if (gameEventSkillBuildData.id == buildId)
				{
					this.playerSkillBuildList.RemoveAt(i);
					list.Add(gameEventSkillBuildData.skillId);
					this.UpdateAttribute(gameEventSkillBuildData.GetMergeAttributeData(), true);
					break;
				}
			}
			EventArgRemoveSkills eventArgRemoveSkills = new EventArgRemoveSkills();
			eventArgRemoveSkills.SetData(list);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleRemoveSkill, eventArgRemoveSkills);
		}

		public List<int> RemoveSkillBuildByGroup(int groupId)
		{
			List<int> list = new List<int>();
			List<GameEventSkillBuildData> list2 = new List<GameEventSkillBuildData>();
			for (int i = 0; i < this.playerSkillBuildList.Count; i++)
			{
				if (this.playerSkillBuildList[i].groupId == groupId)
				{
					list2.Add(this.playerSkillBuildList[i]);
				}
			}
			for (int j = 0; j < list2.Count; j++)
			{
				GameEventSkillBuildData gameEventSkillBuildData = list2[j];
				if (!list.Contains(gameEventSkillBuildData.skillId))
				{
					list.Add(gameEventSkillBuildData.skillId);
				}
				this.playerSkillBuildList.Remove(gameEventSkillBuildData);
				this.UpdateAttribute(gameEventSkillBuildData.GetMergeAttributeData(), true);
			}
			EventArgRemoveSkills eventArgRemoveSkills = new EventArgRemoveSkills();
			eventArgRemoveSkills.SetData(list);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleRemoveSkill, eventArgRemoveSkills);
			return list;
		}

		public void RemoveAllSkill()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.playerSkillBuildList.Count; i++)
			{
				GameEventSkillBuildData gameEventSkillBuildData = this.playerSkillBuildList[i];
				list.Add(gameEventSkillBuildData.skillId);
				this.UpdateAttribute(gameEventSkillBuildData.GetMergeAttributeData(), true);
			}
			this.playerSkillBuildList.Clear();
			EventArgRemoveSkills eventArgRemoveSkills = new EventArgRemoveSkills();
			eventArgRemoveSkills.SetData(list);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleRemoveSkill, eventArgRemoveSkills);
		}

		private void AddFood(int value)
		{
			this.Food += value;
			if (this.Food < 0)
			{
				this.Food = 0;
			}
		}

		private void AddChips(int value)
		{
			if (!this.Chips.IsDataValid())
			{
				return;
			}
			this.Chips.UpdateVariable(this.Chips.mVariable + value);
			if (this.Chips.mVariable < 0)
			{
				this.Chips.UpdateVariable(0);
			}
		}

		private void AddPlayerCoin(long value)
		{
			this.PlayerCoin += value;
			if (this.PlayerCoin < 0L)
			{
				this.PlayerCoin = 0L;
			}
		}

		private void AddDiamond(int value)
		{
			this.Diamond += value;
			if (this.Diamond < 0)
			{
				this.Diamond = 0;
			}
		}

		private void AddExp(int value)
		{
			if (!this.Exp.IsDataValid() || !this.ExpLevel.IsDataValid())
			{
				return;
			}
			this.Exp.UpdateVariable(this.Exp.mVariable + value);
			if (this.Exp.mVariable < 0)
			{
				this.Exp.UpdateVariable(0);
				return;
			}
			int num = this.ExpLevel.mVariable;
			int num2 = this.Exp.mVariable;
			IList<Chapter_skillExp> allElements = GameApp.Table.GetManager().GetChapter_skillExpModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Chapter_skillExp chapter_skillExp = allElements[i];
				if (num <= chapter_skillExp.id && num2 >= chapter_skillExp.expToNext)
				{
					num2 -= chapter_skillExp.expToNext;
					num++;
				}
			}
			if (num >= this.MaxExpLevel)
			{
				num = this.MaxExpLevel;
			}
			this.ExpLevel.UpdateVariable(num);
			this.Exp.UpdateVariable(num2);
		}

		private void AddHpKeepOne(FP addValue)
		{
			FP fp = this.AttributeData.GetHpMax();
			fp = ((fp < FP._1) ? FP._1 : fp);
			this.CurrentHp = FP.Round(MathTools.Clamp(this.CurrentHp + addValue, FP._1, fp));
		}

		private void AddHpPercentKeepOne(FP percent)
		{
			FP fp = this.AttributeData.GetHpMax();
			fp = ((fp < FP._1) ? FP._1 : fp);
			this.CurrentHp += fp * percent;
			this.CurrentHp = FP.Round(MathTools.Clamp(this.CurrentHp, FP._1, fp));
		}

		private void OnMaxHpChanged(FP beforeHpMax)
		{
			FP fp = this.AttributeData.GetHpMax() - beforeHpMax;
			if (fp > 0)
			{
				this.AddHpKeepOne(fp);
				return;
			}
			this.AddHpKeepOne(0);
		}

		public void AddBattleEndEffectActiveNum()
		{
			int battleEndEffectActiveNum = this.BattleEndEffectActiveNum;
			this.BattleEndEffectActiveNum = battleEndEffectActiveNum + 1;
		}

		public void UpdateAttribute(GameEventAttType type, double value)
		{
			string text = string.Empty;
			switch (type)
			{
			case GameEventAttType.AttackPercent:
				text = "Attack%=" + value.ToString();
				break;
			case GameEventAttType.RecoverHpRate:
			case GameEventAttType.CampHpRate:
			{
				FP fp = value;
				fp /= 100;
				this.AddHpPercentKeepOne(fp);
				return;
			}
			case GameEventAttType.HPMaxPercent:
				text = "HPMax%=" + value.ToString();
				break;
			case GameEventAttType.Exp:
				this.AddExp((int)value);
				break;
			case GameEventAttType.Food:
				this.AddFood((int)value);
				break;
			case GameEventAttType.DefencePercent:
				text = "Defence%=" + value.ToString();
				break;
			case GameEventAttType.Chips:
				this.AddChips((int)value);
				return;
			}
			if (!text.Equals(string.Empty))
			{
				MergeAttributeData mergeAttributeData = new MergeAttributeData(text, null, null);
				this.AttributeData.MergeAttribute(mergeAttributeData, false);
				this.SetEventAttributes();
			}
		}

		private void UpdateAttribute(List<MergeAttributeData> list, bool isRevert = false)
		{
			this.AttributeData.MergeAttributes(list, isRevert);
			this.SetEventAttributes();
		}

		public void SetEventAttributes()
		{
			long hpMax4UI = this.AttributeData.GetHpMax4UI();
			this.HpMax = ((hpMax4UI < FP._1) ? FP._1 : hpMax4UI);
			this.Attack = this.AttributeData.GetAttack4UI();
			this.Defence = this.AttributeData.GetDefence4UI();
		}

		public void BattleChangeAttribute(BattleChangeAttributeData changeData)
		{
			this.HpMax = changeData.hpMax;
			this.Attack = changeData.attack;
			this.Defence = changeData.defence;
			this.CurrentHp = changeData.currentHp;
			this.IsUsedRevive = changeData.isUsedRevive;
		}

		public void SetReviveCount(int reviveCount)
		{
			this.ReviveCount = reviveCount;
		}

		public void UpdateRefreshSkillCount()
		{
			this.RefreshSkillCount++;
		}

		public int GetCanRefreshSkillCount()
		{
			int num = this.AttributeData.GetSelectSkillCountByUpgrade.FloorToInt() - this.RefreshSkillCount;
			if (num <= 0)
			{
				num = 0;
			}
			return num;
		}

		public void AddDrops(List<NodeItemParam> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				NodeItemParam nodeItemParam = list[i];
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(nodeItemParam.itemId);
				int num = 3;
				if (elementById != null)
				{
					num = elementById.itemType;
				}
				if (num == 0)
				{
					switch (nodeItemParam.itemId)
					{
					case 1:
					case 4:
						this.AddPlayerCoin((long)nodeItemParam.FinalCount);
						break;
					case 2:
						this.AddDiamond((int)nodeItemParam.FinalCount);
						break;
					}
				}
			}
		}

		public string GetSkillListInfo()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.playerSkillBuildList.Count; i++)
			{
				stringBuilder.Append(string.Format("{0},{1}", this.playerSkillBuildList[i].id, this.playerSkillBuildList[i].skillId));
				if (i < this.playerSkillBuildList.Count - 1)
				{
					stringBuilder.Append("|");
				}
			}
			return stringBuilder.ToString();
		}

		public void UserRecord(EventRecordPlayerData record)
		{
			this.ExpLevel.UpdateVariable(record.expLv);
			this.PlayerCoin = record.playerCoin;
			this.Diamond = record.playerDiamond;
			this.UpdateAttribute(GameEventAttType.Chips, (double)record.playerChips);
			this.UpdateAttribute(GameEventAttType.Exp, (double)record.currentExp);
			this.UpdateAttribute(GameEventAttType.HPMaxPercent, record.maxHpPercent);
			this.UpdateAttribute(GameEventAttType.AttackPercent, record.atkPercent);
			this.UpdateAttribute(GameEventAttType.DefencePercent, record.defPercent);
			this.CurrentHp = record.currentHp;
			this.BattleEndEffectActiveNum = record.BattleEndEffectActiveNum;
			this.ReviveCount = record.ReviveCount;
			this.RefreshSkillCount = record.RefreshSkillCount;
			this.PlayBigBonusCount = record.PlayBigBonusCount;
			this.PlayMinorBonusCount = record.PlayMinorBonusCount;
		}

		public void UserRecord(long currentHp, MapField<string, int> attrMap, int reviveCount)
		{
			foreach (string text in attrMap.Keys)
			{
				if (text.Equals("HPMax%"))
				{
					this.UpdateAttribute(GameEventAttType.HPMaxPercent, (double)attrMap[text]);
				}
				else if (text.Equals("Attack%"))
				{
					this.UpdateAttribute(GameEventAttType.AttackPercent, (double)attrMap[text]);
				}
				else if (text.Equals("Defence%"))
				{
					this.UpdateAttribute(GameEventAttType.DefencePercent, (double)attrMap[text]);
				}
			}
			this.SetReviveCount(reviveCount);
			if (currentHp <= 0L)
			{
				this.CurrentHp = this.HpMax;
				return;
			}
			this.CurrentHp = currentHp;
		}

		public void PlayBigBonusGame()
		{
			int playBigBonusCount = this.PlayBigBonusCount;
			this.PlayBigBonusCount = playBigBonusCount + 1;
		}

		public void PlayMinorBonusGame()
		{
			int playMinorBonusCount = this.PlayMinorBonusCount;
			this.PlayMinorBonusCount = playMinorBonusCount + 1;
		}

		public void SetCurrentHp(FP hp)
		{
			if (hp < this.HpMax)
			{
				this.CurrentHp = hp;
				return;
			}
			this.CurrentHp = this.HpMax;
		}

		public readonly SecureVariable ExpLevel;

		public readonly SecureVariable Exp;

		public readonly SecureVariable Chips;

		private List<GameEventSkillBuildData> playerSkillBuildList = new List<GameEventSkillBuildData>();

		private List<int> playerSkills = new List<int>();
	}
}
