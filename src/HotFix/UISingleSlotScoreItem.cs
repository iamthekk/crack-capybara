using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UISingleSlotScoreItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.copyItem.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.slotImageItemList.Count; i++)
			{
				this.slotImageItemList[i].DeInit();
			}
			this.slotImageItemList.Clear();
		}

		public void SetData(ChapterMiniGame_singleSlotReward slotReward)
		{
			if (slotReward == null)
			{
				return;
			}
			this.singleSlotReward = slotReward;
			Sprite sprite = this.spriteRegister.GetSprite(slotReward.id.ToString());
			for (int i = 0; i < this.singleSlotReward.score; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
				gameObject.SetParentNormal(this.itemParent, false);
				gameObject.SetActiveSafe(true);
				UISingleSlotImageItem component = gameObject.GetComponent<UISingleSlotImageItem>();
				component.SetData(sprite);
				this.slotImageItemList.Add(component);
			}
			this.currentScore = 0;
			this.Refresh();
		}

		public void AddScore()
		{
			this.currentScore++;
			int num = this.currentScore - 1;
			if (num < this.slotImageItemList.Count)
			{
				this.slotImageItemList[num].ShowEffect();
				this.slotImageItemList[num].PlayAni();
			}
			this.Refresh();
			if (this.currentScore == this.singleSlotReward.score)
			{
				GameEventSkillBuildData specifiedSkill = Singleton<GameEventController>.Instance.GetSpecifiedSkill(this.singleSlotReward.skillBuildId);
				if (specifiedSkill != null)
				{
					Singleton<GameEventController>.Instance.SelectSkill(specifiedSkill, false);
					GameTGATools.Ins.AddStageClickTempSkillSelect(new List<GameEventSkillBuildData> { specifiedSkill }, true);
					GameEventSlotSkillViewModule.OpenData openData = new GameEventSlotSkillViewModule.OpenData();
					openData.skillBuild = specifiedSkill;
					openData.noSlot = true;
					GameApp.View.OpenView(ViewName.GameEventSlotSkillViewModule, openData, 1, null, null);
					GameApp.Sound.PlayClip(121, 1f);
				}
			}
		}

		private void Refresh()
		{
			for (int i = 0; i < this.slotImageItemList.Count; i++)
			{
				this.slotImageItemList[i].SetGray(i >= this.currentScore);
			}
			if (this.singleSlotReward == null)
			{
				return;
			}
			GameSkillBuild_skillBuild gameSkillBuild_skillBuild = GameApp.Table.GetManager().GetGameSkillBuild_skillBuild(this.singleSlotReward.skillBuildId);
			if (gameSkillBuild_skillBuild == null)
			{
				HLog.LogError(string.Format("GameSkillBuild_skillBuild not found id={0}", this.singleSlotReward.skillBuildId));
				return;
			}
			GameSkill_skill gameSkill_skill = GameApp.Table.GetManager().GetGameSkill_skill(gameSkillBuild_skillBuild.skillId);
			if (gameSkill_skill == null)
			{
				HLog.LogError(string.Format("GameSkill_skill not found id={0}", gameSkillBuild_skillBuild.skillId));
				return;
			}
			this.textName.text = Singleton<LanguageManager>.Instance.GetInfoByID(gameSkill_skill.nameID);
			string atlasPath = GameApp.Table.GetAtlasPath(gameSkill_skill.iconAtlasID);
			this.imageIcon.SetImage(atlasPath, gameSkill_skill.icon);
		}

		public Vector3 GetFlyPosition()
		{
			for (int i = 0; i < this.slotImageItemList.Count; i++)
			{
				if (i >= this.currentScore)
				{
					return this.slotImageItemList[i].image.transform.position;
				}
			}
			if (this.slotImageItemList.Count > 0)
			{
				return this.slotImageItemList[0].image.transform.position;
			}
			return Vector3.zero;
		}

		public bool IsTargetReward(int scoreId)
		{
			return scoreId == this.singleSlotReward.id;
		}

		public CustomText textName;

		public CustomImage imageIcon;

		public GameObject itemParent;

		public GameObject copyItem;

		public SpriteRegister spriteRegister;

		private SingleSlotData slotData;

		private ChapterMiniGame_singleSlotReward singleSlotReward;

		private List<UISingleSlotImageItem> slotImageItemList = new List<UISingleSlotImageItem>();

		private int currentScore;
	}
}
