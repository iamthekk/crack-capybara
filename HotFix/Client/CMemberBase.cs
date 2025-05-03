using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.Tools;
using Framework.Platfrom;
using LocalModels.Bean;
using Server;
using Spine;
using UnityEngine;

namespace HotFix.Client
{
	public class CMemberBase
	{
		public int InstanceID
		{
			get
			{
				return this.m_memberData.InstanceID;
			}
		}

		public GameObject m_gameObject { get; private set; }

		public CMemberData m_memberData { get; private set; }

		public CMemberFactory memberFactory
		{
			get
			{
				return this.m_memberFactory;
			}
		}

		public CSkillFactory skillFactory
		{
			get
			{
				return this.m_skillFactory;
			}
		}

		public CBulletFactory bulletFactory
		{
			get
			{
				return this.m_bulletFactory;
			}
		}

		public CBuffFactory buffFactory
		{
			get
			{
				return this.m_buffFactory;
			}
		}

		public RoleSpinePlayerBase roleSpinePlayer
		{
			get
			{
				return this.m_roleSpinePlayer;
			}
		}

		public MountSpinePlayer mountSpinePlayer
		{
			get
			{
				return this.m_mountSpinePlayer;
			}
		}

		public bool IsDeath
		{
			get
			{
				return this.m_memberData.m_curHp <= 0;
			}
		}

		public GameObject SpineRoot
		{
			get
			{
				return this.m_spineRoot;
			}
		}

		public GameObject ShakeRoot
		{
			get
			{
				return this.m_shakeRoot;
			}
		}

		public void SetMemberData(CMemberData memberData)
		{
			CMemberData memberData2 = this.m_memberData;
			if (memberData2 != null)
			{
				memberData2.Dispose();
			}
			this.m_memberData = memberData;
			CMemberData memberData3 = this.m_memberData;
			memberData3.m_curShieldChanged = (Action<FP, FP, bool, bool>)Delegate.Combine(memberData3.m_curShieldChanged, new Action<FP, FP, bool, bool>(this.OnShieldChangeHandler));
		}

		public void SetGameObject(GameObject gameObject)
		{
			this.m_gameObject = gameObject;
		}

		public void SetMemberFactory(CMemberFactory memberFactory)
		{
			this.m_memberFactory = memberFactory;
		}

		public async Task OnInit()
		{
			this.m_componentRegister = this.m_gameObject.GetComponent<ComponentRegister>();
			if (this.m_componentRegister == null)
			{
				HLog.LogError(HLog.ToColor("BaseMember.OnInit  m_componentRegister is null", 3));
			}
			this.m_spineRoot = this.m_componentRegister.GetGameObject("SpineRoot");
			if (this.m_spineRoot == null)
			{
				HLog.LogError(HLog.ToColor("模型 SpineRoot is null. PrefabName = " + this.m_componentRegister.gameObject.name, 3));
			}
			if (!this.m_memberData.IsMainMember)
			{
				if (this.m_memberData.RoleType == MemberRoleType.Mount)
				{
					this.m_roleSpinePlayer = this.m_spineRoot.GetComponent<MountSpinePlayer>();
				}
				else
				{
					this.m_roleSpinePlayer = this.m_spineRoot.GetComponent<NormalSpinePlayerPlayer>();
					if (this.m_roleSpinePlayer == null)
					{
						this.m_roleSpinePlayer = this.m_spineRoot.AddComponent<NormalSpinePlayerPlayer>();
					}
				}
			}
			else
			{
				this.m_roleSpinePlayer = this.m_spineRoot.GetComponent<MainMemberSpinePlayer>();
				MainMemberSpinePlayer mainMemberSpinePlayer = this.m_roleSpinePlayer as MainMemberSpinePlayer;
				if (mainMemberSpinePlayer != null)
				{
					mainMemberSpinePlayer.IsEnemyPlayer = this.m_memberData.IsEnemyPlayer;
				}
			}
			this.m_roleSpinePlayer.transform.localScale = this.m_memberData.m_RootScale;
			this.m_roleSpinePlayer.Init(this.m_componentRegister);
			this.ResetMorph();
			this.m_shakeRoot = this.m_componentRegister.GetGameObject("ShakeRoot");
			if (this.m_shakeRoot == null)
			{
				HLog.LogError(HLog.ToColor("模型 ShakeRoot is null. PrefabName = " + this.m_componentRegister.gameObject.name, 3));
			}
			this.m_shakeAnimator = this.m_shakeRoot.GetComponent<Animator>();
			this.m_body = this.m_gameObject.GetComponent<MemberBody>();
			if (this.m_body == null)
			{
				HLog.LogError(HLog.ToColor("BaseMember.OnInit body is null", 3));
			}
			await this.Init_Spine();
			this.CreateHpHUD();
			this.Init_AI();
			List<Task> list = new List<Task>();
			list.Add(this.Init_Skill());
			list.Add(this.Init_Bullet());
			list.Add(this.Init_Buff());
			list.Add(this.Init_MemberSubmodule());
			list.Add(this.Init_Hit());
			list.AddRange(this.Init_Preload());
			list.Add(this.Init_Shield());
			await Task.WhenAll(list);
			this.Init_ColorRender();
			this.Init_Volume();
			this.m_moveTaskManager.OnInit();
			this.m_moveTaskManager.OnPlay();
		}

		public async Task OnDeInit()
		{
			if (this.mountSpinePlayer != null)
			{
				this.mountSpinePlayer.DeInit();
				this.SpineRoot.SetParentNormal(this.ShakeRoot, false);
				Object.DestroyImmediate(this.m_mountObj);
			}
			MainMemberSpinePlayer mainMemberSpinePlayer = this.m_roleSpinePlayer as MainMemberSpinePlayer;
			if (mainMemberSpinePlayer != null)
			{
				mainMemberSpinePlayer.DeInitSkin();
			}
			RoleSpinePlayerBase roleSpinePlayer = this.m_roleSpinePlayer;
			if (roleSpinePlayer != null)
			{
				roleSpinePlayer.DeInit();
			}
			this.m_moveTaskManager.OnDeInit();
			this.RemoveHpHUD();
			this.DeInit_ColorRender();
			await Task.WhenAll(new List<Task>
			{
				this.DeInit_Hit(),
				this.DeInit_Preload(),
				this.DeInit_MemberSubmodule(),
				this.Deinit_Shield()
			});
			this.DeInit_Volume();
			this.DeInit_Spine();
			this.DeInit_Skill();
			this.DeInit_Buff();
			this.DeInit_Bullet();
			this.DeInit_AI();
			this.ResetShakeAnimatorDefault();
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			float num = deltaTime * 1f;
			this.Update_Skill(deltaTime, unscaledDeltaTime);
			this.Update_Hit(deltaTime, unscaledDeltaTime);
			this.Update_ColorRender(num, unscaledDeltaTime, deltaTime);
			this.Update_Buff(deltaTime, unscaledDeltaTime);
			CBulletFactory bulletFactory = this.m_bulletFactory;
			if (bulletFactory != null)
			{
				bulletFactory.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			this.m_moveTaskManager.OnUpdate(deltaTime);
			this.Update_Volume(deltaTime);
			MountSpinePlayer mountSpinePlayer = this.m_mountSpinePlayer;
			if (mountSpinePlayer == null)
			{
				return;
			}
			mountSpinePlayer.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public void OnGameStart(GameStartMemberData _)
		{
			this.InitStartPosition();
		}

		public void OnGameOver(bool _)
		{
			this.ShowHpHUD(false);
		}

		public void Hurt(BattleReportData_HurtOneData data)
		{
			this.m_memberData.SetMaxHp(data.m_maxHp);
			this.m_memberData.SetCurHp(data.m_curHp);
			this.m_memberData.SetAttack(data.m_attack);
			this.m_memberData.SetDefense(data.m_defense);
			this.ShowHurtHUD(data.m_changeHPData);
			this.RefreshHpHUD();
			this.RefreshPlayerAttrUI();
			if (data.m_changeHPData.m_type == ChangeHPType.Miss)
			{
				this.m_hitFactory.ShowMiss();
				return;
			}
			if (data.m_curHp <= 0)
			{
				this.AI.SwitchState(MemberState.Death);
			}
			if (data.m_changeHPData.m_type == ChangeHPType.Crit)
			{
				this.bulletFactory.OnShake(4);
			}
			else
			{
				this.bulletFactory.OnShake(data.m_bulletId);
			}
			if (data.m_attackerInstanceID > 0)
			{
				CMemberBase member = this.m_memberFactory.GetMember(data.m_attackerInstanceID);
				if (member == null)
				{
					HLog.LogError(string.Format("attacker is error .MemberFactory is not member InstanceID = {0}", data.m_attackerInstanceID));
					return;
				}
				if (data.m_fireBulletID > 0)
				{
					GameSkill_fireBullet elementById = GameApp.Table.GetManager().GetGameSkill_fireBulletModelInstance().GetElementById(data.m_fireBulletID);
					int hitEffectID = elementById.hitEffectID;
					if (hitEffectID > 0)
					{
						this.m_hitFactory.ShowBulletHitEffect(hitEffectID);
					}
					int hitPrefabIDByMeat = this.GetHitPrefabIDByMeat(MemberMeatType.Jelly, elementById.hitPrefabIDs);
					if (hitPrefabIDByMeat > 0)
					{
						List<int> listInt = elementById.hitPosID.GetListInt(',');
						if (listInt != null && listInt.Count == 3)
						{
							TaskOutValue<HitBase> taskOutValue = new TaskOutValue<HitBase>();
							this.m_hitFactory.AddHit(taskOutValue, hitPrefabIDByMeat, listInt[0], (PointRotationDirection)listInt[1], (HitTargetType)listInt[2], member, null);
						}
					}
					int hitEffectID2 = this.m_memberData.m_hitEffectID;
					if (hitEffectID2 > 0)
					{
						TaskOutValue<HitBase> taskOutValue2 = new TaskOutValue<HitBase>();
						this.m_hitFactory.AddHit(taskOutValue2, hitEffectID2, 2, PointRotationDirection.Target, HitTargetType.Owner, member, null);
					}
					if (data.IsShowCombo)
					{
						CMemberBase member2 = this.memberFactory.GetMember(data.m_attackerInstanceID);
						if (member2 != null)
						{
							member2.ShowComboUI(data.CurComboCount);
						}
					}
					this.PlayRimPower();
				}
				else if (data.m_buffId > 0 && data.m_changeHPData.m_type != ChangeHPType.NormalHurt && data.m_changeHPData.m_type != ChangeHPType.PhysicalHurt && data.m_changeHPData.m_type != ChangeHPType.Recover && data.m_changeHPData.m_type != ChangeHPType.Vampire)
				{
					this.m_hitFactory.ShowBulletHitEffect(1);
				}
				GameApp.Sound.PlayClip(this.m_memberData.m_hitSoundID, 1f);
			}
		}

		public void Destroy()
		{
			CMemberFactory memberFactory = this.m_memberFactory;
			if (memberFactory == null)
			{
				return;
			}
			memberFactory.RemoveMember(this);
		}

		private void OnShieldChangeHandler(FP lastShield, FP curShield, bool isThunder, bool isDurian)
		{
			this.RefreshShieldHUD();
			if (lastShield <= 0 && curShield > 0)
			{
				this.SetShieldEffectArtBuffID(isThunder, isDurian);
				this.ShowShieldEffect();
				return;
			}
			if (lastShield > 0 && curShield <= 0)
			{
				this.HideShieldEffect();
			}
		}

		public void BattleJump()
		{
			this.ShowBigSkill(false);
		}

		public void RefreshCardData(List<int> addSkillIDs, MemberAttributeData attributes, FP curHp, FP curRecharge, Dictionary<int, FP> curLegacyPower, bool isUsedRevive)
		{
			if (this.m_memberData == null)
			{
				return;
			}
			this.m_memberData.RefreshCardData(addSkillIDs, attributes, curHp, curRecharge, curLegacyPower, isUsedRevive);
			this.skillFactory.AddSkills(addSkillIDs);
		}

		public void OnPause(bool pause)
		{
			this.PauseAnimation(pause);
			this.m_ignoreRimRowerPause = pause;
		}

		public void SetMountSpinePlayer(GameObject goMount, MountSpinePlayer mountSpine, ComponentRegister cr)
		{
			this.m_mountComponentRegister = cr;
			if (goMount == null || mountSpine == null || cr == null)
			{
				return;
			}
			this.m_mountObj = goMount;
			this.m_mountSpinePlayer = mountSpine;
			this.m_mountSpinePlayer.Init(cr);
			this.m_mountSpinePlayer.PlayAni("Idle", true);
			this.m_mountSpinePlayer.SetBackOrderLayer(this.MountBackLayer);
			this.m_mountSpinePlayer.SetFrontOrderLayer(this.MountFrontLayer);
		}

		public MemberAIBase AI
		{
			get
			{
				return this.m_ai;
			}
		}

		public MemberState AiState
		{
			get
			{
				return this.m_state;
			}
		}

		private void Init_AI()
		{
			this.m_ai = new MemberAIDefault();
			this.m_ai.SetOwner(this);
			this.m_ai.OnInit();
			this.m_ai.SwitchState(MemberState.Idle);
		}

		private void DeInit_AI()
		{
			MemberAIBase ai = this.m_ai;
			if (ai != null)
			{
				ai.OnDeInit();
			}
			this.m_ai = null;
		}

		private void Update_AI(float deltaTime)
		{
			MemberAIBase ai = this.m_ai;
			if (ai == null)
			{
				return;
			}
			ai.OnUpdate(deltaTime, 0f);
		}

		public void SetMemberState(MemberState state)
		{
			this.m_state = state;
		}

		public void PlayShakeAnimatorAnim(string animationName)
		{
			if (this.m_shakeAnimator == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(animationName))
			{
				return;
			}
			if (this.m_shakeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Normal"))
			{
				this.m_shakeAnimator.SetTrigger(animationName);
			}
		}

		public bool HasShakeAnimatorAnim(string animationName)
		{
			for (int i = 0; i < this.m_shakeAnimator.parameters.Length; i++)
			{
				if (this.m_shakeAnimator.parameters[i].name.Equals(animationName))
				{
					return true;
				}
			}
			return false;
		}

		private void ResetShakeAnimatorDefault()
		{
			this.m_shakeAnimator.transform.localPosition = Vector3.zero;
			this.m_shakeAnimator.transform.localScale = Vector3.one;
		}

		private async Task Init_BigSkill()
		{
			this.m_bigSkillCtrl = new BigSkillControl();
			this.m_bigSkillCtrl.SetOwner(this);
			await this.m_bigSkillCtrl.OnInit();
		}

		private void DeInit_BigSkill()
		{
			BigSkillControl bigSkillCtrl = this.m_bigSkillCtrl;
			if (bigSkillCtrl != null)
			{
				bigSkillCtrl.OnDeInit();
			}
			this.m_bigSkillCtrl = null;
		}

		private void Update_BigSkill(float deltaTime, float unscaledDeltaTime)
		{
			BigSkillControl bigSkillCtrl = this.m_bigSkillCtrl;
			if (bigSkillCtrl == null)
			{
				return;
			}
			bigSkillCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public void ShowBigSkill(bool isShow)
		{
			if (isShow)
			{
				BigSkillControl bigSkillCtrl = this.m_bigSkillCtrl;
				if (bigSkillCtrl != null)
				{
					bigSkillCtrl.Play();
				}
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_ShowHighlight, null);
				return;
			}
			BigSkillControl bigSkillCtrl2 = this.m_bigSkillCtrl;
			if (bigSkillCtrl2 != null)
			{
				bigSkillCtrl2.Stop();
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_HideHighlight, null);
		}

		private async Task Init_Buff()
		{
			this.m_buffFactory = new CBuffFactory(this);
			await this.m_buffFactory.OnInit();
		}

		private void Update_Buff(float deltaTime, float unscaledDeltaTime)
		{
			CBuffFactory buffFactory = this.m_buffFactory;
			if (buffFactory == null)
			{
				return;
			}
			buffFactory.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		private void DeInit_Buff()
		{
			if (this.m_buffFactory != null)
			{
				this.m_buffFactory.OnDeInit();
				this.m_buffFactory = null;
			}
		}

		public async Task AddBuff(string guid, int buffId, CMemberBase attacker)
		{
			await this.m_buffFactory.AddBuff(guid, buffId, attacker);
			this.ActiveMorphCheck(guid, buffId);
		}

		public async Task RemoveBuff(string guid, int buffId, CMemberBase attacker)
		{
			await this.m_buffFactory.RemoveBuff(guid, buffId, attacker);
			this.InactiveMorphCheck(guid, buffId);
		}

		public void RemoveAllBuff()
		{
			CBuffFactory buffFactory = this.m_buffFactory;
			if (buffFactory != null)
			{
				buffFactory.RemoveAllBuff(true);
			}
			this.ResetMorph();
		}

		private async Task Init_Bullet()
		{
			this.m_bulletFactory = new CBulletFactory(this);
			await this.m_bulletFactory.OnInit();
		}

		private void DeInit_Bullet()
		{
			CBulletFactory bulletFactory = this.m_bulletFactory;
			if (bulletFactory != null)
			{
				bulletFactory.OnDeInit();
			}
			this.m_bulletFactory = null;
		}

		public void FireBullet(CFireBulletData data)
		{
			this.m_bulletFactory.AddBullet(data);
		}

		public int weaponID
		{
			get
			{
				return this.m_weaponID;
			}
		}

		private void Init_Equip()
		{
		}

		private void DeInit_Equip()
		{
		}

		public void SetEquip(int weaponID)
		{
			this.m_weaponID = weaponID;
			this.m_roleSpinePlayer.SetWeapon(weaponID);
		}

		public void PlayAnimationIdle()
		{
			string idleAnimationName = MemberAnimationName.GetIdleAnimationName(this.m_weaponID);
			this.PlayAnimation(idleAnimationName);
		}

		public void AddAnimationIdle()
		{
			string idleAnimationName = MemberAnimationName.GetIdleAnimationName(this.m_weaponID);
			this.AddAnimation(idleAnimationName);
		}

		private async Task Init_Hit()
		{
			this.m_hitFactory = new HitFactory(this);
			await this.m_hitFactory.OnInit();
		}

		private async Task DeInit_Hit()
		{
			await this.m_hitFactory.OnDeInit();
			this.m_hitFactory = null;
		}

		private void Update_Hit(float deltaTime, float unscaledDeltaTime)
		{
			HitFactory hitFactory = this.m_hitFactory;
			if (hitFactory == null)
			{
				return;
			}
			hitFactory.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		private int GetHitPrefabIDByMeat(MemberMeatType type, int[] hitPrefabIDs)
		{
			if (hitPrefabIDs == null || hitPrefabIDs.Length == 0)
			{
				return 0;
			}
			int num = type - MemberMeatType.Stone;
			if (num < hitPrefabIDs.Length)
			{
				return hitPrefabIDs[num];
			}
			return hitPrefabIDs[0];
		}

		public void InsertPlayHitEffect(TaskOutValue<HitBase> outValue, int memberHitEffectID, MemberBodyPosType posType, PointRotationDirection direction, HitTargetType targetType, CMemberBase attacker, CBulletBase bullet)
		{
			if (this.m_hitFactory == null)
			{
				return;
			}
			this.m_hitFactory.AddHit(outValue, memberHitEffectID, posType, direction, targetType, attacker, bullet);
		}

		public void CreateHpHUD()
		{
			if (this.m_memberData.cardData.IsPet)
			{
				return;
			}
			HoverData hoverData = new HoverData(this.InstanceID, this.m_body.m_health, this.m_memberData.Camp);
			EventArgsAddHover instance = Singleton<EventArgsAddHover>.Instance;
			if (this.m_memberData.Camp == MemberCamp.Friendly)
			{
				hoverData.SetStateData(this.m_memberData.m_curHp.AsLong(), this.m_memberData.m_maxHp.AsLong(), this.m_memberData.m_curRecharge.AsLong(), this.m_memberData.m_maxRecharge.AsLong(), true, this.m_memberData.m_curLegacyPowerDict, this.m_memberData.m_maxLegacyPowerDict);
				instance.SetStateBarData(HoverType.FriendlyStateBar, hoverData);
			}
			else
			{
				hoverData.SetStateData(this.m_memberData.m_curHp.AsLong(), this.m_memberData.m_maxHp.AsLong(), 0L, 1L, this.m_memberData.m_isShowRecharge, this.m_memberData.m_curLegacyPowerDict, this.m_memberData.m_maxLegacyPowerDict);
				instance.SetStateBarData(HoverType.EnemyStateBar, hoverData);
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_AddHover, instance);
			EventArgsAddHover instance2 = Singleton<EventArgsAddHover>.Instance;
			instance2.SetData(HoverType.BattleSign, hoverData, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_AddHover, instance2);
			EventArgsAddHover instance3 = Singleton<EventArgsAddHover>.Instance;
			HoverData hoverData2 = new HoverData(this.InstanceID, this.m_body.m_headTop, this.m_memberData.Camp);
			instance3.SetData(HoverType.WaitRoundCount, hoverData2, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_AddHover, instance3);
		}

		public void ShowHpHUD(bool isShow)
		{
			EventArgsShowHpHUD instance = Singleton<EventArgsShowHpHUD>.Instance;
			instance.instanceId = this.InstanceID;
			instance.isShow = isShow;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_ShowHpHUD, instance);
		}

		public void RemoveHpHUD()
		{
			EventArgsRemoveHoverByID instance = Singleton<EventArgsRemoveHoverByID>.Instance;
			instance.instanceId = this.InstanceID;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_RemoveHoverByID, instance);
		}

		public void RefreshHpHUD()
		{
			if (this.m_memberData.cardData.IsPet)
			{
				return;
			}
			EventArgsRefreshHP instance = Singleton<EventArgsRefreshHP>.Instance;
			instance.SetData(this.InstanceID, this.m_memberData.m_curHp.AsLong(), this.m_memberData.m_maxHp.AsLong());
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_RefreshHP, instance);
		}

		public void RefreshRechargeHUD()
		{
			if (this.m_memberData.cardData.IsPet)
			{
				return;
			}
			EventArgsRefreshRecharge instance = Singleton<EventArgsRefreshRecharge>.Instance;
			instance.SetData(this.InstanceID, this.m_memberData.m_curRecharge.AsLong(), this.m_memberData.m_maxRecharge.AsLong());
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_RefresRecharge, instance);
		}

		public void RefreshLegacyPowerHUD(int refreshSkillId = 0)
		{
			if (this.m_memberData.cardData.IsPet)
			{
				return;
			}
			foreach (KeyValuePair<int, FP> keyValuePair in this.m_memberData.m_curLegacyPowerDict)
			{
				int key = keyValuePair.Key;
				if (refreshSkillId <= 0 || key == refreshSkillId)
				{
					long num = keyValuePair.Value.AsLong();
					FP fp;
					this.m_memberData.m_maxLegacyPowerDict.TryGetValue(keyValuePair.Key, out fp);
					EventArgsRefreshLegacyPower instance = Singleton<EventArgsRefreshLegacyPower>.Instance;
					instance.SetData(this.InstanceID, key, num, fp.AsLong());
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_RefreshLegacyPower, instance);
				}
			}
		}

		public void RefreshShieldHUD()
		{
			EventArgsRefreshShield instance = Singleton<EventArgsRefreshShield>.Instance;
			instance.SetData(this.InstanceID, this.m_memberData.m_curShield.AsLong());
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_RefreshShild, instance);
		}

		public void ShowHurtHUD(ChangeHPData changeHpData)
		{
			long num = changeHpData.m_hpUpdate.AsLong();
			switch (changeHpData.m_type)
			{
			case ChangeHPType.NormalHurt:
			{
				HoverType hoverType = HoverType.LessHp;
				this.ShowHurtHUD(num, hoverType);
				return;
			}
			case ChangeHPType.IceHurt:
			{
				HoverType hoverType = HoverType.LessHpIce;
				this.ShowHurtHUD(num, hoverType);
				return;
			}
			case ChangeHPType.FireHurt:
			{
				HoverType hoverType = HoverType.LessHpFire;
				this.ShowHurtHUD(num, hoverType);
				return;
			}
			case ChangeHPType.Crit:
			{
				HoverType hoverType = HoverType.LessHpCrit;
				this.ShowHurtHUD(num, hoverType);
				return;
			}
			case ChangeHPType.Miss:
				this.ShowTextHUD(EHoverTextType.Miss);
				return;
			case ChangeHPType.Recover:
			case ChangeHPType.Vampire:
			{
				HoverType hoverType = HoverType.PlusHp;
				this.ShowHurtHUD(num, hoverType);
				return;
			}
			case ChangeHPType.PoisonHurt:
			{
				HoverType hoverType = HoverType.LessHpPoison;
				this.ShowHurtHUD(num, hoverType);
				return;
			}
			case ChangeHPType.ThunderHurt:
			{
				HoverType hoverType = HoverType.LessHpThunder;
				this.ShowHurtHUD(num, hoverType);
				return;
			}
			case ChangeHPType.Seckill:
			{
				HoverType hoverType = HoverType.LessHp;
				this.ShowHurtHUD(num, hoverType);
				this.ShowTextHUD(EHoverTextType.Seckill);
				return;
			}
			case ChangeHPType.TrueDamage:
			{
				HoverType hoverType = HoverType.LessHPTrueDamage;
				this.ShowHurtHUD(num, hoverType);
				break;
			}
			case ChangeHPType.FallingSwordHurt:
				break;
			case ChangeHPType.PhysicalHurt:
			{
				HoverType hoverType = HoverType.LessHp;
				this.ShowHurtHUD(num, hoverType);
				return;
			}
			default:
				return;
			}
		}

		private void ShowHurtHUD(long value, HoverType hoverType)
		{
			EventArgsAddHover instance = Singleton<EventArgsAddHover>.Instance;
			HoverData hoverData = new HoverData(this.InstanceID, this.m_body.m_headTop, this.m_memberData.Camp);
			instance.SetData(hoverType, hoverData, value);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_AddHover, instance);
		}

		public void ShowTextHUD(EHoverTextType hoverTextType)
		{
			EventArgsAddHover instance = Singleton<EventArgsAddHover>.Instance;
			HoverData hoverData = new HoverData(this.InstanceID, this.m_body.m_headTop, this.m_memberData.Camp);
			instance.SetData(HoverType.BattleText, hoverData, new HoverTextData(hoverTextType, string.Empty));
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_AddHover, instance);
		}

		public void ShowTextHUD(string languageID)
		{
			EventArgsAddHover instance = Singleton<EventArgsAddHover>.Instance;
			HoverData hoverData = new HoverData(this.InstanceID, this.m_body.m_headTop, this.m_memberData.Camp);
			instance.SetData(HoverType.BattleText, hoverData, new HoverTextData(EHoverTextType.Default, languageID));
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_AddHover, instance);
		}

		public void ShowSkillInfoHUD(int skillID)
		{
			if (skillID <= 0)
			{
				return;
			}
			GameSkill_skill elementById = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(skillID);
			if (elementById == null)
			{
				HLog.LogError(string.Format("skillId:{0} is not exist in Table", skillID));
				return;
			}
			if (elementById.isShowInfoHUD == 0)
			{
				return;
			}
			EventArgsAddHover instance = Singleton<EventArgsAddHover>.Instance;
			HoverData hoverData = new HoverData(this.InstanceID, this.m_body.m_headTop, this.m_memberData.Camp);
			instance.SetData(HoverType.SkillName, hoverData, skillID);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameHover_AddHover, instance);
		}

		public void RefreshPlayerAttrUI()
		{
			if (!this.m_memberData.IsMainMember)
			{
				return;
			}
			EventArgsGameChangeAttribute instance = Singleton<EventArgsGameChangeAttribute>.Instance;
			instance.SetData(new BattleChangeAttributeData
			{
				hpMax = this.m_memberData.m_maxHp,
				currentHp = this.m_memberData.m_curHp,
				attack = this.m_memberData.m_attack,
				defence = this.m_memberData.m_defense,
				isUsedRevive = this.m_memberData.m_isUsedRevive
			});
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleChangeAttribute, instance);
		}

		public void ShowComboUI(int count)
		{
			EventArgsInt instance = Singleton<EventArgsInt>.Instance;
			instance.SetData(count);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshCombo, instance);
		}

		public void ShowWaitRoundCount(int roundCount)
		{
			this.UpdateWaitRoundCount((long)roundCount);
		}

		public void UpdateWaitRoundCount(long value)
		{
			HoverType hoverType = HoverType.WaitRoundCount;
			EventArgsAddHover instance = Singleton<EventArgsAddHover>.Instance;
			HoverData hoverData = new HoverData(this.InstanceID, this.m_body.m_headTop, this.m_memberData.Camp);
			instance.SetData(hoverType, hoverData, value);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameView_Refresh_WaitRoundCount, instance);
		}

		public async Task Preload_LegacySkill(int skillId)
		{
			if (this.m_legacySkillCtrl != null)
			{
				await this.m_legacySkillCtrl.TryLoadRes(skillId);
			}
		}

		private async Task Init_LegacySkill()
		{
			if (this.m_legacySkillCtrl == null)
			{
				this.m_legacySkillCtrl = new LegacySkillControl();
				this.m_legacySkillCtrl.SetOwner(this);
				await this.m_legacySkillCtrl.OnInit();
			}
		}

		private void DeInit_LegacySkill()
		{
			LegacySkillControl legacySkillCtrl = this.m_legacySkillCtrl;
			if (legacySkillCtrl != null)
			{
				legacySkillCtrl.OnDeInit();
			}
			this.m_legacySkillCtrl = null;
		}

		private void Update_LegacySkill(float deltaTime, float unscaledDeltaTime)
		{
			LegacySkillControl legacySkillCtrl = this.m_legacySkillCtrl;
			if (legacySkillCtrl == null)
			{
				return;
			}
			legacySkillCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public void LegacySkillSummonDisplay(BattleReportData_LegacySkillSummonDisplay reportData)
		{
			LegacySkillControl legacySkillCtrl = this.m_legacySkillCtrl;
			if (legacySkillCtrl == null)
			{
				return;
			}
			legacySkillCtrl.LegacySkillSummonDisplay(reportData);
		}

		public void PlayLegacySkillAnimation(int skillId, string animationName)
		{
			animationName = "Skill";
			NormalSpinePlayerPlayer curLegacySummonSpinePlayer = this.m_legacySkillCtrl.curLegacySummonSpinePlayer;
			if (curLegacySummonSpinePlayer == null)
			{
				return;
			}
			this.PlayAnimation(curLegacySummonSpinePlayer, animationName, null, null);
			this.AddAnimation(curLegacySummonSpinePlayer, "Idle");
		}

		public void PlayLegacySkillCompleteAnimation(int skillId)
		{
			this.m_legacySkillCtrl.LegacySkillRevertDisplay();
		}

		public bool IsInControl()
		{
			return this.m_frozen != null && this.m_frozen.IsControlled;
		}

		public void ControlledStateChange()
		{
			if (this.m_frozen == null || this.m_frozen.IsControlled)
			{
				if (this.m_state != MemberState.Controlled)
				{
					this.m_ai.SwitchState(MemberState.Controlled);
					return;
				}
			}
			else if (this.m_state == MemberState.Controlled)
			{
				this.m_ai.SwitchState(MemberState.Idle);
			}
		}

		private async Task Init_MemberSubmodule()
		{
			this.m_stun = new CMemberSubmodule_Stun(this, 202, 5);
			await this.m_stun.Init();
			this.m_frozen = new CMemberSubmodule_Frozen(this, 204, 2);
			await this.m_frozen.Init();
			this.m_silence = new CMemberSubmodule_Silence(this, 209, 5);
			await this.m_silence.Init();
		}

		private async Task DeInit_MemberSubmodule()
		{
			CMemberSubmodule_Stun stun = this.m_stun;
			await ((stun != null) ? stun.DeInit() : null);
			CMemberSubmodule_Frozen frozen = this.m_frozen;
			await ((frozen != null) ? frozen.DeInit() : null);
			CMemberSubmodule_Silence silence = this.m_silence;
			await ((silence != null) ? silence.DeInit() : null);
		}

		private void Update_MemberSubmodule(float deltaTime, float unscaledDeltaTime, float unDeltatimePause)
		{
			CMemberSubmodule_Stun stun = this.m_stun;
			if (stun != null)
			{
				stun.Update(deltaTime, unscaledDeltaTime, unDeltatimePause);
			}
			CMemberSubmodule_Frozen frozen = this.m_frozen;
			if (frozen != null)
			{
				frozen.Update(deltaTime, unscaledDeltaTime, unDeltatimePause);
			}
			CMemberSubmodule_Silence silence = this.m_silence;
			if (silence == null)
			{
				return;
			}
			silence.Update(deltaTime, unscaledDeltaTime, unDeltatimePause);
		}

		public async Task Reset_MemberSubmodule()
		{
			CMemberSubmodule_Stun stun = this.m_stun;
			await ((stun != null) ? stun.OnReset() : null);
			CMemberSubmodule_Frozen frozen = this.m_frozen;
			await ((frozen != null) ? frozen.OnReset() : null);
			CMemberSubmodule_Silence silence = this.m_silence;
			await ((silence != null) ? silence.OnReset() : null);
		}

		public void OnRoleRoundEnd_MemberSubmodule()
		{
			CMemberSubmodule_Stun stun = this.m_stun;
			if (stun != null)
			{
				stun.OnRoleRoundEnd(1);
			}
			CMemberSubmodule_Frozen frozen = this.m_frozen;
			if (frozen != null)
			{
				frozen.OnRoleRoundEnd(1);
			}
			CMemberSubmodule_Silence silence = this.m_silence;
			if (silence == null)
			{
				return;
			}
			silence.OnRoleRoundEnd(1);
		}

		private void ActiveMorphCheck(string guid, int buffId)
		{
			if (this.inMorphing)
			{
				return;
			}
			CBuffBase cbuff = this.buffFactory.GetCBuff(guid, buffId);
			if (cbuff != null && cbuff is CBuff_Morph)
			{
				CBuff_Morph cbuff_Morph = cbuff as CBuff_Morph;
				this.curBuffGuid = guid;
				this.inMorphing = true;
				this.ActiveMorph(cbuff_Morph.m_data.MorphId);
			}
		}

		private void InactiveMorphCheck(string guid, int buffId)
		{
			if (!this.inMorphing)
			{
				return;
			}
			if (this.curBuffGuid.Equals(guid))
			{
				if (this.buffFactory != null && this.buffFactory.m_buffContainer != null)
				{
					for (int i = this.buffFactory.m_buffContainer.Count - 1; i >= 0; i--)
					{
						List<CBuffBase> value = this.buffFactory.m_buffContainer[i].Value;
						for (int j = value.Count - 1; j >= 0; j--)
						{
							CBuffBase cbuffBase = value[j];
							if (cbuffBase is CBuff_Morph)
							{
								this.inMorphing = false;
								CBuff_Morph cbuff_Morph = cbuffBase as CBuff_Morph;
								this.ActiveMorphCheck(cbuff_Morph.m_guid, cbuff_Morph.m_buffData.m_id);
								return;
							}
						}
					}
				}
				this.RevertMorph();
			}
		}

		private void RevertMorph()
		{
			if (!this.inMorphing)
			{
				return;
			}
			this.inMorphing = false;
			this.curBuffGuid = "";
			this.ActiveMorph(0);
		}

		private void ResetMorph()
		{
			this.inMorphing = false;
			this.curBuffGuid = "";
			this.curMorphId = 0;
			if (this.m_roleSpinePlayer != null)
			{
				this.m_roleSpinePlayer.ActiveMorph(this.curMorphId);
			}
		}

		public async Task PreloadMorph(int morphId)
		{
			if (morphId > 0)
			{
				if (this.m_roleSpinePlayer != null)
				{
					await this.m_roleSpinePlayer.PreInitMorph(morphId);
					this.LoadCompleteMorph(morphId);
				}
			}
		}

		private void LoadCompleteMorph(int morphId)
		{
			if (this.inMorphing && this.waitActiveMorphId > 0 && this.waitActiveMorphId == morphId)
			{
				this.ActiveMorph(this.waitActiveMorphId);
			}
		}

		private async Task ActiveMorph(int morphId)
		{
			if (morphId > 0 && this.m_roleSpinePlayer != null && this.m_roleSpinePlayer.GetMorph(morphId) == null)
			{
				this.waitActiveMorphId = morphId;
			}
			else
			{
				this.waitActiveMorphId = 0;
				if (!this.curMorphId.Equals(morphId))
				{
					if (morphId == 0)
					{
						CMemberBase.<>c__DisplayClass142_0 CS$<>8__locals1 = new CMemberBase.<>c__DisplayClass142_0();
						ArtBuff_Buff artTable = GameApp.Table.GetManager().GetArtBuff_BuffModelInstance().GetElementById(1001);
						if (!artTable.path.Equals(string.Empty))
						{
							await PoolManager.Instance.CheckPrefab(artTable.path);
						}
						Transform transform = this.m_body.GetTransform(2);
						CS$<>8__locals1.obj = PoolManager.Instance.Out(artTable.path, transform.position, transform.rotation, transform);
						DelayCall.Instance.CallOnce(3000, delegate
						{
							if (CS$<>8__locals1.obj != null)
							{
								PoolManager.Instance.Put(CS$<>8__locals1.obj);
							}
						});
						CS$<>8__locals1 = null;
						artTable = null;
					}
					this.curMorphId = morphId;
					if (this.m_roleSpinePlayer != null)
					{
						this.m_roleSpinePlayer.ActiveMorph(morphId);
						this.PlayAnimationIdle();
					}
				}
			}
		}

		public void InitStartPosition()
		{
			this.m_startPosition = this.m_gameObject.transform.position;
		}

		public Vector3 GetStartPosition()
		{
			return this.m_startPosition;
		}

		public void Move(BattleReportData_Move reportData)
		{
			if (reportData.m_isMoveBack)
			{
				this.MoveBack(reportData);
				return;
			}
			this.MoveTo(reportData);
		}

		private void MoveTo(BattleReportData_Move reportData)
		{
			GameSkill_skill elementById = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(reportData.m_skillId);
			if (elementById == null && elementById.moveType == 0)
			{
				return;
			}
			int targetInstanceID = reportData.m_targetInstanceID;
			CMemberBase member = this.memberFactory.GetMember(targetInstanceID);
			CSkillMoveTaskBase cskillMoveTaskBase = null;
			if (elementById.moveType == 1)
			{
				CSkillMove_MeleeFront cskillMove_MeleeFront = new CSkillMove_MeleeFront();
				cskillMove_MeleeFront.m_owner = this;
				cskillMove_MeleeFront.m_target = member;
				cskillMove_MeleeFront.SetSetParameters(elementById.moveParam);
				cskillMove_MeleeFront.m_time = Config.GetTimeByFrame(reportData.m_moveFrame);
				cskillMoveTaskBase = cskillMove_MeleeFront;
			}
			if (cskillMoveTaskBase != null)
			{
				this.m_moveTaskManager.AddTask(cskillMoveTaskBase);
			}
		}

		public void MoveBack(BattleReportData_Move reportData)
		{
			CSkillMoveBack cskillMoveBack = new CSkillMoveBack();
			cskillMoveBack.m_owner = this;
			cskillMoveBack.m_time = Config.GetTimeByFrame(reportData.m_moveFrame);
			this.m_moveTaskManager.AddTask(cskillMoveBack);
		}

		public void MoveEnter(int remainFrame)
		{
			CMemberEnterMove cmemberEnterMove = new CMemberEnterMove();
			cmemberEnterMove.m_owner = this;
			this.m_gameObject.transform.position = this.m_startPosition + Vector3.right * 5f;
			cmemberEnterMove.m_time = Config.GetTimeByFrame(remainFrame);
			this.m_moveTaskManager.AddTask(cmemberEnterMove);
		}

		private List<Task> Init_Preload()
		{
			List<Task> list = new List<Task>();
			int hitEffectID = this.m_memberData.m_hitEffectID;
			if (hitEffectID != 0)
			{
				ArtHit_Hit elementById = GameApp.Table.GetManager().GetArtHit_HitModelInstance().GetElementById(hitEffectID);
				list.Add(PoolManager.Instance.Cache(elementById.path));
			}
			List<CSkillBase> skills = this.m_skillFactory.GetSkills();
			for (int i = 0; i < skills.Count; i++)
			{
				int[] fireBullets = skills[i].skillData.m_fireBullets;
				for (int j = 0; j < fireBullets.Length; j++)
				{
					GameSkill_fireBullet elementById2 = GameApp.Table.GetManager().GetGameSkill_fireBulletModelInstance().GetElementById(fireBullets[j]);
					if (elementById2 != null)
					{
						ArtBullet_Bullet elementById3 = GameApp.Table.GetManager().GetArtBullet_BulletModelInstance().GetElementById(elementById2.bulletID);
						if (elementById3 != null)
						{
							list.Add(PoolManager.Instance.Cache(elementById3.path));
						}
					}
				}
			}
			Task task = this.m_bigSkillCtrl.Preload();
			list.Add(task);
			return list;
		}

		private async Task DeInit_Preload()
		{
			await Task.CompletedTask;
		}

		private void Update_Preload(float deltaTime, float unscaledDeltaTime)
		{
		}

		private void Init_ColorRender()
		{
			this.m_roleSpinePlayer.Init_ColorRender();
		}

		private void Update_ColorRender(float deltaTime, float unscaledDeltaTime, float unDeltatimePause)
		{
			this.m_roleSpinePlayer.m_colorRender.OnUpdate(this.m_ignoreRimRowerPause ? unDeltatimePause : deltaTime, unscaledDeltaTime);
		}

		private void DeInit_ColorRender()
		{
			this.m_roleSpinePlayer.DeInit_ColorRender();
		}

		public void PlayRimPower()
		{
			this.m_roleSpinePlayer.m_colorRender.Play(0f);
		}

		public void StopRimPower()
		{
			this.m_roleSpinePlayer.m_colorRender.Stop();
		}

		public void SetAlpha(float alpha)
		{
			ColorRender colorRender = this.m_roleSpinePlayer.m_colorRender;
			if (colorRender != null)
			{
				colorRender.SetFillAlgha(alpha);
			}
		}

		public void PlayAlpha(Action complete = null)
		{
			ColorRender colorRender = this.m_roleSpinePlayer.m_colorRender;
			if (colorRender != null)
			{
				colorRender.PlayAlgha(complete);
				return;
			}
			if (complete != null)
			{
				complete();
			}
		}

		public void PlayAlphaShow(Action complete = null)
		{
			ColorRender colorRender = this.m_roleSpinePlayer.m_colorRender;
			if (colorRender != null)
			{
				colorRender.PlayAlphaShow(complete);
				return;
			}
			if (complete != null)
			{
				complete();
			}
		}

		public void StopAlpha()
		{
			ColorRender colorRender = this.m_roleSpinePlayer.m_colorRender;
			if (colorRender == null)
			{
				return;
			}
			colorRender.Stop();
		}

		public void PlayVColor(float to, float duration, Action complete = null)
		{
			ColorRender colorRender = this.m_roleSpinePlayer.m_colorRender;
			if (colorRender != null)
			{
				colorRender.PlayVColor(to, duration, complete);
				return;
			}
			if (complete != null)
			{
				complete();
			}
		}

		public void SetVColor(float to)
		{
			ColorRender colorRender = this.m_roleSpinePlayer.m_colorRender;
			if (colorRender != null)
			{
				colorRender.SetFillVColor(to);
			}
		}

		public async Task Init_Shield()
		{
			this.m_isShowShield = false;
			await Task.CompletedTask;
		}

		public async Task Deinit_Shield()
		{
			this.m_isShowShield = false;
			if (this.m_shieldEffectGo != null)
			{
				PoolManager.Instance.Put(this.m_shieldEffectGo);
				this.m_shieldEffectGo = null;
			}
			await Task.CompletedTask;
		}

		public void SetShieldEffectArtBuffID(bool isThunder, bool isDurian)
		{
			if (isDurian)
			{
				this.curArtBuffID = 212;
				return;
			}
			if (isThunder)
			{
				this.curArtBuffID = 211;
				return;
			}
			this.curArtBuffID = 205;
		}

		public void ShowShieldEffect()
		{
			this.m_isShowShield = true;
			this.PlayShield();
		}

		public void HideShieldEffect()
		{
			this.m_isShowShield = false;
			this.PlayShield();
		}

		private async Task PlayShield()
		{
			if (this.m_shieldEffectGo == null)
			{
				if (this.isInShieldLoading || !this.m_isShowShield)
				{
					return;
				}
				this.isInShieldLoading = true;
				ArtBuff_Buff artBuffTable = GameApp.Table.GetManager().GetArtBuff_BuffModelInstance().GetElementById(this.curArtBuffID);
				if (artBuffTable == null)
				{
					HLog.LogError(HLog.ToColor(string.Format("artBuffTable is Error. ArtBuffID = {0}", this.curArtBuffID), 3));
					return;
				}
				await PoolManager.Instance.CheckPrefab(artBuffTable.path);
				Transform transform = this.m_body.GetTransform(2);
				this.m_shieldEffectGo = PoolManager.Instance.Out(artBuffTable.path, transform.position, transform.rotation, transform);
				this.m_shieldEffectColor = this.m_shieldEffectGo.GetComponent<EffectColor>();
				this.isInShieldLoading = false;
				artBuffTable = null;
			}
			this.PlayShieldGradient(this.m_isShowShield);
		}

		private void PlayShieldGradient(bool isShow)
		{
			if (isShow)
			{
				if (!this.m_isShieldDisplayed)
				{
					this.m_shieldEffectColor.OnShowBefore();
					this.m_isShieldDisplayed = true;
					this.m_isShieldHidden = false;
					return;
				}
			}
			else if (!this.m_isShieldHidden)
			{
				this.m_shieldEffectColor.OnShowAfter();
				this.m_isShieldHidden = true;
				this.m_isShieldDisplayed = false;
			}
		}

		private async Task Init_Skill()
		{
			this.m_skillFactory = new CSkillFactory();
			this.m_skillFactory.OnInit(this);
			await this.Init_BigSkill();
			await this.Init_LegacySkill();
			await this.m_skillFactory.AddSkills(this.m_memberData.m_skillIDs);
		}

		private void DeInit_Skill()
		{
			this.DeInit_BigSkill();
			this.DeInit_LegacySkill();
			CSkillFactory skillFactory = this.m_skillFactory;
			if (skillFactory != null)
			{
				skillFactory.OnDeInit();
			}
			this.m_skillFactory = null;
		}

		private void Update_Skill(float deltaTime, float unscaledDeltaTime)
		{
			CSkillFactory skillFactory = this.m_skillFactory;
			if (skillFactory != null)
			{
				skillFactory.OnUpdate(deltaTime);
			}
			this.Update_BigSkill(deltaTime, unscaledDeltaTime);
			this.Update_LegacySkill(deltaTime, unscaledDeltaTime);
		}

		public void PlaySkill(BattleReportData_PlaySkill reportData)
		{
			if (this.m_skillFactory != null)
			{
				this.m_skillFactory.PlaySkill(reportData);
			}
		}

		protected int MountBackLayer
		{
			get
			{
				return this.m_curSpineLayer + 50;
			}
		}

		protected int MountFrontLayer
		{
			get
			{
				return this.m_curSpineLayer + 90;
			}
		}

		private async Task Init_Spine()
		{
			MainMemberSpinePlayer mainMemberSpinePlayer = this.m_roleSpinePlayer as MainMemberSpinePlayer;
			if (mainMemberSpinePlayer != null)
			{
				await mainMemberSpinePlayer.InitSkin();
			}
			this.InitSpineLayer();
			this.m_roleSpinePlayer.onEventCallback += this.OnEvent;
		}

		private void DeInit_Spine()
		{
			this.m_roleSpinePlayer.onEventCallback -= this.OnEvent;
		}

		public void InitSpineLayer()
		{
			if (this.m_memberData.cardData.m_memberRace == MemberRace.Hero)
			{
				if (this.m_memberData.Camp == MemberCamp.Friendly)
				{
					this.m_curSpineLayer = 200;
				}
				else
				{
					this.m_curSpineLayer = 100;
				}
			}
			else if (this.m_memberData.cardData.m_posIndex == MemberPos.Two)
			{
				this.m_curSpineLayer = 400;
			}
			else if (this.m_memberData.cardData.m_posIndex == MemberPos.Three)
			{
				this.m_curSpineLayer = 0;
			}
			else if (this.m_memberData.cardData.m_posIndex == MemberPos.Four)
			{
				this.m_curSpineLayer = 600;
			}
			this.SetNormalLayer();
		}

		private void OnEvent(TrackEntry trackEntry, Event e)
		{
			string name = e.Data.Name;
			if (name == "FootStep")
			{
				this.PlayFootStepSound();
				return;
			}
			if (!(name == "Tackle"))
			{
				if (!(name == "Shake"))
				{
					if (!(name == "HitScale"))
					{
						if (!(name == "HitScaleGlobal"))
						{
							return;
						}
						float num;
						float.TryParse(e.Data.String, out num);
						if (num != 0f)
						{
							this.HitScale(num);
						}
					}
					else
					{
						float num2;
						float.TryParse(e.Data.String, out num2);
						if (num2 != 0f)
						{
							this.HitStop(num2);
							return;
						}
					}
				}
				else
				{
					int num3;
					int.TryParse(e.Data.String, out num3);
					if (num3 != 0)
					{
						EventArgsGameCameraShake instance = Singleton<EventArgsGameCameraShake>.Instance;
						instance.SetData(num3);
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_Shake, instance);
						return;
					}
				}
				return;
			}
			GameApp.Sound.PlayClip(411, 1f);
		}

		public void PlayAnimation(string animationName)
		{
			this.PlayAnimation(animationName, null, null);
		}

		public void PlayAnimation(string animationName, AnimationState.TrackEntryEventDelegate spineEvent)
		{
			this.PlayAnimation(animationName, spineEvent, null);
		}

		public void PlayAnimation(string animationName, AnimationState.TrackEntryDelegate complete)
		{
			this.PlayAnimation(animationName, null, complete);
		}

		public void PlayAnimation(string animationName, AnimationState.TrackEntryEventDelegate spineEvent, AnimationState.TrackEntryDelegate complete)
		{
			this.PlayAnimation(this.m_roleSpinePlayer, animationName, spineEvent, complete);
		}

		public void PlayAnimation(RoleSpinePlayerBase spinePlayer, string animationName, AnimationState.TrackEntryEventDelegate spineEvent, AnimationState.TrackEntryDelegate complete)
		{
			if (this.IsInControl() && !this.IsDeath)
			{
				return;
			}
			if (string.IsNullOrEmpty(animationName))
			{
				return;
			}
			bool flag = MemberAnimationName.IsLoop(animationName);
			if (spinePlayer.IsHaveAni(animationName))
			{
				spinePlayer.PlayAni(animationName, flag, spineEvent, complete);
			}
		}

		public void AddAnimation(string animationName)
		{
			this.AddAnimation(this.m_roleSpinePlayer, animationName);
		}

		public void AddAnimation(RoleSpinePlayerBase roleSpinePlayer, string animationName)
		{
			bool flag = MemberAnimationName.IsLoop(animationName);
			if (roleSpinePlayer.IsHaveAni(animationName))
			{
				roleSpinePlayer.AddAni(animationName, flag);
			}
		}

		public float GetAnimationDuration(string animationName)
		{
			float num = 0f;
			if (this.m_roleSpinePlayer.IsHaveAni(animationName))
			{
				num = this.roleSpinePlayer.GetAnimationDuration(animationName);
			}
			return num;
		}

		public void SetAttackLayer()
		{
			int num = ((this.m_memberData.Camp == MemberCamp.Friendly) ? (this.m_curSpineLayer + 1000) : (this.m_curSpineLayer + 300));
			if (this.m_memberData.Camp == MemberCamp.Friendly)
			{
				this.m_roleSpinePlayer.SetOrderLayer(this.m_curSpineLayer + 1000);
				if (this.m_memberData.cardData.m_isMainMember)
				{
					MountSpinePlayer mountSpinePlayer = this.m_mountSpinePlayer;
					if (mountSpinePlayer != null)
					{
						mountSpinePlayer.SetBackOrderLayer(this.MountBackLayer + 1000);
					}
					MountSpinePlayer mountSpinePlayer2 = this.m_mountSpinePlayer;
					if (mountSpinePlayer2 != null)
					{
						mountSpinePlayer2.SetFrontOrderLayer(this.MountFrontLayer + 1000);
					}
				}
			}
			else
			{
				this.m_roleSpinePlayer.SetOrderLayer(this.m_curSpineLayer + 300);
			}
			LegacySkillControl legacySkillCtrl = this.m_legacySkillCtrl;
			if (legacySkillCtrl == null)
			{
				return;
			}
			legacySkillCtrl.SetOrderLayer(num);
		}

		public void SetNormalLayer()
		{
			this.m_roleSpinePlayer.SetOrderLayer(this.m_curSpineLayer);
			if (this.m_memberData.cardData.m_isMainMember)
			{
				MountSpinePlayer mountSpinePlayer = this.m_mountSpinePlayer;
				if (mountSpinePlayer != null)
				{
					mountSpinePlayer.SetBackOrderLayer(this.MountBackLayer);
				}
				MountSpinePlayer mountSpinePlayer2 = this.m_mountSpinePlayer;
				if (mountSpinePlayer2 != null)
				{
					mountSpinePlayer2.SetFrontOrderLayer(this.MountFrontLayer);
				}
			}
			LegacySkillControl legacySkillCtrl = this.m_legacySkillCtrl;
			if (legacySkillCtrl == null)
			{
				return;
			}
			legacySkillCtrl.SetOrderLayer(this.m_curSpineLayer);
		}

		public void SetDeathLayer()
		{
			this.m_roleSpinePlayer.SetOrderLayer(this.m_curSpineLayer);
			if (this.m_memberData.cardData.m_isMainMember)
			{
				MountSpinePlayer mountSpinePlayer = this.m_mountSpinePlayer;
				if (mountSpinePlayer != null)
				{
					mountSpinePlayer.SetBackOrderLayer(this.MountBackLayer);
				}
				MountSpinePlayer mountSpinePlayer2 = this.m_mountSpinePlayer;
				if (mountSpinePlayer2 == null)
				{
					return;
				}
				mountSpinePlayer2.SetFrontOrderLayer(this.MountFrontLayer);
			}
		}

		public void SetAnimatorSpeed(float speed)
		{
			this.m_roleSpinePlayer.SetSpeed(speed);
		}

		public void PauseAnimation(bool pause)
		{
			this.SetAnimatorSpeed((float)(pause ? 0 : 1));
		}

		public void SetSkin(string skinName)
		{
			this.m_roleSpinePlayer.SetSkin(skinName);
		}

		private void SetSkin()
		{
			int initSkinID = this.m_memberData.m_initSkinID;
			if (initSkinID != 0)
			{
				GameMember_skin elementById = GameApp.Table.GetManager().GetGameMember_skinModelInstance().GetElementById(initSkinID);
				this.m_roleSpinePlayer.SetSkin(elementById.skin);
			}
		}

		public async void HitStop(float duration)
		{
			int delay = (int)(duration * 1000f);
			float tempScale = 1f;
			this.SetAnimatorSpeed(0f);
			await TaskExpand.Delay(delay);
			this.SetAnimatorSpeed(tempScale * 2f);
			await TaskExpand.Delay(delay);
			this.SetAnimatorSpeed(tempScale);
		}

		private async void HitScale(float scaleDuration)
		{
			int delay = (int)(scaleDuration * 1000f);
			float timeScale = 1f;
			if (Time.timeScale > 0f)
			{
				timeScale = Time.timeScale;
			}
			GameApp.SetTimeScale(0f);
			await TaskExpand.Delay(delay);
			GameApp.SetTimeScale(timeScale * 2f);
			await TaskExpand.Delay(delay);
			GameApp.SetTimeScale(timeScale);
		}

		private void PlayFootStepSound()
		{
			this.curSoundID = this.GetRangeSoundID(this.curSoundID);
			GameApp.Sound.PlayClip(this.curSoundID, 1f);
		}

		private int GetRangeSoundID(int index = -1)
		{
			this.curSoundIDs = new List<int>(this.Sounds);
			if (index >= 0)
			{
				this.curSoundIDs.Remove(index);
			}
			int num = Random.Range(0, this.curSoundIDs.Count);
			return this.curSoundIDs[num];
		}

		private void Init_Volume()
		{
			this.OnResetVolume();
		}

		private void DeInit_Volume()
		{
			this.OnResetVolume();
		}

		private void Update_Volume(float deltaTime)
		{
			if (!this.m_volmeIsPlaying)
			{
				return;
			}
			this.m_volmeTime += deltaTime * this.m_volmeSpeed;
			if (this.m_volmeTime >= this.m_volmeDuration)
			{
				this.m_volmeTime = this.m_volmeDuration;
				this.m_volmeIsPlaying = false;
			}
			float num = this.m_volmeTime / this.m_volmeDuration;
			this.m_volumeCurrent = Mathf.Lerp(this.m_volumFrom, this.m_volmeTo, num);
			float num2 = ((this.m_volumeCurrent >= 0.1f) ? this.m_volumeCurrent : 0.1f);
			this.m_gameObject.transform.localScale = Vector3.one * num2;
		}

		private void OnResetVolume()
		{
			this.m_volume = 1f;
			this.m_volumFrom = 1f;
			this.m_volmeTo = 1f;
			this.m_volumeCurrent = 1f;
			this.m_volmeTime = 0f;
			this.m_gameObject.transform.localScale = Vector3.one * this.m_volume;
			this.m_volmeIsPlaying = false;
		}

		public void AddVolume(float addScale, float lerpSpeed = 1f)
		{
			this.m_volume += addScale;
			if (this.m_volume != this.m_volumeCurrent)
			{
				this.m_volmeSpeed = lerpSpeed;
				this.m_volmeTime = 0f;
				this.m_volumFrom = this.m_volumeCurrent;
				this.m_volmeTo = Mathf.Clamp(this.m_volume, 0.6f, 1.6f);
				this.m_volmeIsPlaying = true;
			}
		}

		protected CMemberFactory m_memberFactory;

		protected CSkillFactory m_skillFactory;

		protected CBulletFactory m_bulletFactory;

		protected CBuffFactory m_buffFactory;

		public RoleSpinePlayerBase m_roleSpinePlayer;

		private MountSpinePlayer m_mountSpinePlayer;

		public Animator m_shakeAnimator;

		public MemberBody m_body;

		public HitFactory m_hitFactory;

		public BigSkillControl m_bigSkillCtrl;

		private TaskManager m_moveTaskManager = new TaskManager();

		public ComponentRegister m_componentRegister;

		public GameObject m_mountObj;

		public ComponentRegister m_mountComponentRegister;

		private GameObject m_spineRoot;

		private GameObject m_shakeRoot;

		private MemberAIBase m_ai;

		private MemberState m_state;

		private int m_weaponID;

		private LegacySkillControl m_legacySkillCtrl;

		public CMemberSubmodule_Stun m_stun;

		public CMemberSubmodule_Frozen m_frozen;

		public CMemberSubmodule_Silence m_silence;

		private string curBuffGuid;

		private int curMorphId;

		private bool inMorphing;

		private int waitActiveMorphId;

		private Vector3 m_startPosition = Vector3.zero;

		private bool m_ignoreRimRowerPause;

		private const int defaultArtBuffID = 205;

		private const int thunderArtBuffID = 211;

		private const int durianArtBuffID = 212;

		private int curArtBuffID;

		private GameObject m_shieldEffectGo;

		private EffectColor m_shieldEffectColor;

		private bool m_isShowShield;

		private bool m_isShieldDisplayed;

		private bool m_isShieldHidden;

		private bool isInShieldLoading;

		private const int m_mainRoleLayer = 200;

		private const int m_MonsterLayer = 100;

		private const int m_petLayerOffset = 200;

		private const int m_heroAttackLayer = 1000;

		private const int m_monsterAttackLayer = 300;

		private const int m_mountBackLayerOffset = 50;

		private const int m_mountFrontLayerOffset = 90;

		private int m_curSpineLayer = 100;

		private int[] Sounds = new int[] { 401, 402, 403, 404, 405, 406, 407 };

		private int curSoundID = -1;

		private List<int> curSoundIDs;

		private const float m_volumeMax = 1.6f;

		private const float m_volumeMin = 0.6f;

		private float m_volume = 1f;

		private float m_volumeCurrent = 1f;

		private float m_volumFrom;

		private float m_volmeTo;

		private float m_volmeSpeed = 1f;

		private float m_volmeDuration = 0.5f;

		private float m_volmeTime;

		private bool m_volmeIsPlaying;
	}
}
