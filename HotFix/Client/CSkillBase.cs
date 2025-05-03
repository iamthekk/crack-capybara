using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.Platfrom;
using LocalModels.Bean;
using Server;

namespace HotFix.Client
{
	public abstract class CSkillBase
	{
		public CSkillData skillData
		{
			get
			{
				return this.m_skillData;
			}
		}

		public virtual async Task OnInit(CMemberBase owner, CSkillData skillData)
		{
			this.m_owner = owner;
			this.m_skillData = skillData;
			this.Init_StartEffect();
			await this.Init_SetSagecraft();
			await this.PreloadSkillRes();
		}

		public virtual void OnDeInit()
		{
			this.DeInit_StartEffect();
			this.DeInit_SetSagecraft();
		}

		public virtual void OnUpdate(float deltaTime)
		{
			this.Update_StartEffect(deltaTime, deltaTime);
		}

		protected abstract void OnReadParameters(string parameters);

		protected abstract void OnPlay();

		protected abstract void OnPlayComplete();

		public void Play(BattleReportData_PlaySkill reportData)
		{
			this.m_owner.SetAttackLayer();
			this.PlayAnimation();
			this.OnPlay();
			if (this.m_startEffectFactory != null)
			{
				TaskOutValue<StartEffectBase> taskOutValue = new TaskOutValue<StartEffectBase>();
				this.m_startEffectFactory.AddNode(taskOutValue);
				this.curStartEffectBase = taskOutValue.Value;
			}
			if (reportData.m_castType == SkillCastType.Counter)
			{
				this.m_owner.ShowTextHUD(EHoverTextType.Counter);
			}
			else if (reportData.m_castType == SkillCastType.Combo)
			{
				this.m_owner.ShowTextHUD(EHoverTextType.Combo);
			}
			this.m_owner.ShowSkillInfoHUD(reportData.m_skillId);
			GameApp.Sound.PlayClip(this.skillData.m_soundID, 1f);
		}

		public void PlayComplete(BattleReportData_PlaySkillComplete reportData)
		{
			this.OnPlayComplete();
			this.m_owner.SetNormalLayer();
			if (this.m_skillData.m_freedType == SkillFreedType.Legacy)
			{
				this.m_owner.PlayLegacySkillCompleteAnimation(this.m_skillData.m_id);
			}
		}

		protected void PlayAnimation()
		{
			string animationAttackName = this.m_skillData.m_AnimationAttackName;
			if (animationAttackName.Equals(string.Empty))
			{
				return;
			}
			this.ShowBigSkill();
			if (this.m_skillData.m_freedType == SkillFreedType.Legacy)
			{
				this.m_owner.PlayLegacySkillAnimation(this.m_skillData.m_id, animationAttackName);
				return;
			}
			this.m_owner.PlayAnimation(animationAttackName);
			this.m_owner.AddAnimationIdle();
		}

		private void ShowBigSkill()
		{
			if (this.m_skillData.isBlack && this.m_owner.m_memberData.Camp == MemberCamp.Friendly && this.m_skillData.m_freedType == SkillFreedType.Big)
			{
				this.m_owner.ShowBigSkill(true);
				this.DelayHideBigSkill(850);
			}
		}

		private async Task DelayHideBigSkill(int time)
		{
			await TaskExpand.Delay(time);
			this.m_owner.ShowBigSkill(false);
		}

		private async Task PreloadSkillRes()
		{
			List<Task> list = new List<Task>();
			if (this.skillData.m_freedType == SkillFreedType.Legacy)
			{
				list.Add(this.m_owner.Preload_LegacySkill(this.skillData.m_id));
			}
			List<int> skillAddBuffs = SkillHelper.GetSkillAddBuffs(this.skillData.skillStartOwnerAddBuffs);
			List<int> skillAddBuffs2 = SkillHelper.GetSkillAddBuffs(this.skillData.skillEndOwnerAddBuffs);
			List<int> skillAddBuffs3 = SkillHelper.GetSkillAddBuffs(this.skillData.skillStartTargetAddBuffs);
			List<int> skillAddBuffs4 = SkillHelper.GetSkillAddBuffs(this.skillData.skillEndTargetAddBuffs);
			List<int> skillAddBuffs5 = SkillHelper.GetSkillAddBuffs(this.skillData.skillStartFriendAddBuffs);
			List<int> skillAddBuffs6 = SkillHelper.GetSkillAddBuffs(this.skillData.skillEndFriendAddBuffs);
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.UnionWith(skillAddBuffs);
			hashSet.UnionWith(skillAddBuffs2);
			hashSet.UnionWith(skillAddBuffs3);
			hashSet.UnionWith(skillAddBuffs4);
			hashSet.UnionWith(skillAddBuffs5);
			hashSet.UnionWith(skillAddBuffs6);
			foreach (int num in hashSet)
			{
				GameBuff_buff elementById = GameApp.Table.GetManager().GetGameBuff_buffModelInstance().GetElementById(num);
				if (elementById != null && elementById.buffType == 9)
				{
					CBuff_Morph.Data data = JsonManager.ToObject<CBuff_Morph.Data>(elementById.parameters);
					if (data != null && data.MorphId > 0)
					{
						list.Add(this.m_owner.PreloadMorph(data.MorphId));
					}
				}
			}
			await Task.WhenAll(list);
		}

		protected async Task Init_SetSagecraft()
		{
			int sagecraftType = this.m_skillData.m_sagecraftType;
			if (this.m_owner.m_memberData.IsMainMember && !sagecraftType.Equals(0))
			{
				await this.m_owner.roleSpinePlayer.ShowSagecraft((SagecraftType)sagecraftType);
			}
		}

		protected async Task DeInit_SetSagecraft()
		{
			int sagecraftType = this.m_skillData.m_sagecraftType;
			if (this.m_owner.m_memberData.IsMainMember && !sagecraftType.Equals(0))
			{
				this.m_owner.roleSpinePlayer.DestroySagecraft((SagecraftType)sagecraftType);
			}
			await Task.CompletedTask;
		}

		protected void Init_StartEffect()
		{
			this.m_startEffectFactory = new StartEffectFactory(this.m_owner, this);
			this.m_startEffectFactory.OnInit();
		}

		protected void Update_StartEffect(float deltaTime, float unscaledDeltaTime)
		{
			this.m_startEffectFactory.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected async Task DeInit_StartEffect()
		{
			await this.m_startEffectFactory.OnDeInit();
			this.m_startEffectFactory = null;
		}

		public async Task Reset_StartEffect()
		{
			await this.m_startEffectFactory.OnReset();
		}

		public void GameStart_StartEffect()
		{
			this.m_startEffectFactory.OnGameStart();
		}

		public void Pause_StartEffect(bool pause)
		{
			this.m_startEffectFactory.OnPause(pause);
		}

		public void GameOver_StartEffect(GameOverType gameOverType)
		{
			this.m_startEffectFactory.OnGameOver(gameOverType);
		}

		protected CMemberBase m_owner;

		protected CSkillData m_skillData;

		protected StartEffectBase curStartEffectBase;

		protected StartEffectFactory m_startEffectFactory;
	}
}
