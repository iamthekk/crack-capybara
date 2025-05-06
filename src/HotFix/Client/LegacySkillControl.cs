using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix.Client
{
	public class LegacySkillControl
	{
		public void SetOwner(CMemberBase owner)
		{
			this.m_owner = owner;
		}

		public async Task OnInit()
		{
			if (this.m_effect == null)
			{
				ArtSkill_Skill artTable = GameApp.Table.GetManager().GetArtSkill_SkillModelInstance().GetElementById(4);
				await PoolManager.Instance.CheckPrefab(artTable.path);
				this.m_effect = PoolManager.Instance.Out(artTable.path, this.m_owner.m_gameObject.transform.position, this.m_owner.m_gameObject.transform.rotation, this.m_owner.m_gameObject.transform);
				this.m_effect.SetActiveSafe(false);
				artTable = null;
			}
		}

		public async Task TryLoadRes(int skillId)
		{
			if (skillId > 0)
			{
				if (!this.legacySummonSpinePlayerDict.ContainsKey(skillId))
				{
					GameSkill_skill elementById = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(skillId);
					ArtMember_member elementById2 = GameApp.Table.GetManager().GetArtMember_memberModelInstance().GetElementById(elementById.legacyBindmodelId);
					string modelPath = elementById2.path;
					if (!string.IsNullOrEmpty(modelPath))
					{
						await PoolManager.Instance.CheckPrefab(modelPath);
						GameObject gameObject = PoolManager.Instance.Out(modelPath, this.m_owner.m_gameObject.transform.position, this.m_owner.m_gameObject.transform.rotation, this.m_owner.m_gameObject.transform);
						this.m_modelGoDict[skillId] = gameObject;
						ComponentRegister component = gameObject.GetComponent<ComponentRegister>();
						GameObject gameObject2 = component.GetGameObject("SpineRoot");
						NormalSpinePlayerPlayer normalSpinePlayerPlayer = gameObject2.GetComponent<NormalSpinePlayerPlayer>();
						if (normalSpinePlayerPlayer == null)
						{
							normalSpinePlayerPlayer = gameObject2.AddComponent<NormalSpinePlayerPlayer>();
						}
						normalSpinePlayerPlayer.Init(component);
						this.legacySummonSpinePlayerDict[skillId] = normalSpinePlayerPlayer;
						gameObject.gameObject.SetActiveSafe(false);
					}
				}
			}
		}

		public void OnDeInit()
		{
			this.curModelGo = null;
			this.curLegacySummonSpinePlayer = null;
			foreach (KeyValuePair<int, GameObject> keyValuePair in this.m_modelGoDict)
			{
				if (keyValuePair.Value != null)
				{
					Object.Destroy(keyValuePair.Value);
				}
			}
			this.m_modelGoDict.Clear();
			this.legacySummonSpinePlayerDict.Clear();
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (!this.m_isPlay && !this.m_isRevert)
			{
				return;
			}
			if (this.m_isPlay)
			{
				this.m_playDuration += deltaTime;
				this.MainRoleFadeOut();
				if (this.m_playDuration >= this.m_playTime)
				{
					this.LegacySkillSummonDisplay_End();
					return;
				}
				if (this.m_playDuration >= this.m_playphase1TimeCost && !this.m_isPlayPhase2)
				{
					this.LegacySkillSummonDisplay_Phase2();
					return;
				}
			}
			else if (this.m_isRevert)
			{
				this.m_revertDuration += deltaTime;
				if (this.m_revertDuration + 0.06666667f >= this.m_revertTime)
				{
					this.LegacySkillRevertDisplay_End();
				}
			}
		}

		public void SetOrderLayer(int layer)
		{
			if (this.curLegacySummonSpinePlayer != null)
			{
				this.curLegacySummonSpinePlayer.SetOrderLayer(layer);
			}
		}

		private void MainRoleFadeOut()
		{
			if (this.m_playDuration <= this.m_playphase1TimeCost)
			{
				float num = Mathf.Clamp01(1f - this.m_playDuration / this.m_playphase1TimeCost);
				this.m_owner.SetAlpha(num);
			}
		}

		public void LegacySkillSummonDisplay(BattleReportData_LegacySkillSummonDisplay reportData)
		{
			this.LegacySkillRevertDisplay_End();
			this.ResetParam();
			this.m_isPlay = true;
			this.m_curSkillId = reportData.m_skillId;
			this.m_modelGoDict.TryGetValue(this.m_curSkillId, out this.curModelGo);
			this.legacySummonSpinePlayerDict.TryGetValue(this.m_curSkillId, out this.curLegacySummonSpinePlayer);
			if (this.curModelGo == null)
			{
				this.TryLoadRes(this.m_curSkillId);
			}
			this.m_playTime = Config.GetTimeByFrame(reportData.m_displayFrame);
			this.m_playphase1TimeCost = Config.GetTimeByFrame(18);
			this.m_owner.StopAlpha();
			this.m_owner.SetAlpha(1f);
			this.LegacySkillSummonDisplay_Phase1();
		}

		private void LegacySkillSummonDisplay_Phase1()
		{
			this.m_isPlayPhase1 = true;
			if (this.m_effect != null)
			{
				this.m_effect.SetActiveSafe(false);
				this.m_effect.SetActiveSafe(true);
			}
			if (this.curModelGo == null)
			{
				this.m_modelGoDict.TryGetValue(this.m_curSkillId, out this.curModelGo);
			}
			if (this.curLegacySummonSpinePlayer == null)
			{
				this.legacySummonSpinePlayerDict.TryGetValue(this.m_curSkillId, out this.curLegacySummonSpinePlayer);
			}
			this.m_owner.PlayAnimation("Battle/Cast");
		}

		private void LegacySkillSummonDisplay_Phase2()
		{
			this.m_isPlayPhase2 = true;
			if (this.m_owner.roleSpinePlayer != null)
			{
				this.m_owner.roleSpinePlayer.SetRoleModelShow(false);
			}
			if (this.m_owner.mountSpinePlayer != null)
			{
				this.m_owner.mountSpinePlayer.SetRoleModelShow(false);
			}
			if (this.m_owner.m_body != null)
			{
				this.m_owner.m_body.m_center.gameObject.SetActiveSafe(false);
			}
			if (this.m_owner.m_mountComponentRegister != null)
			{
				GameObject gameObject = this.m_owner.m_mountComponentRegister.GetGameObject("PointEffect");
				if (gameObject != null)
				{
					gameObject.SetActiveSafe(false);
				}
			}
			if (this.curModelGo == null)
			{
				this.m_modelGoDict.TryGetValue(this.m_curSkillId, out this.curModelGo);
			}
			if (this.curLegacySummonSpinePlayer == null)
			{
				this.legacySummonSpinePlayerDict.TryGetValue(this.m_curSkillId, out this.curLegacySummonSpinePlayer);
			}
			if (this.m_effect != null)
			{
				this.m_effect.SetActiveSafe(false);
			}
			if (this.curModelGo != null)
			{
				this.curModelGo.SetActiveSafe(true);
			}
			if (this.curLegacySummonSpinePlayer != null)
			{
				if (this.curLegacySummonSpinePlayer.IsHaveAni("Appear"))
				{
					this.curLegacySummonSpinePlayer.PlayAni("Appear", false);
				}
				this.curLegacySummonSpinePlayer.AddAni("Idle", true);
			}
		}

		private void LegacySkillSummonDisplay_End()
		{
			this.m_isPlay = false;
			if (this.m_effect != null)
			{
				this.m_effect.SetActiveSafe(false);
			}
		}

		public void LegacySkillRevertDisplay()
		{
			this.ResetParam();
			this.m_isRevert = true;
			this.m_revertTime = Config.GetTimeByFrame(20);
			this.LegacySkillRevertDisplay_Phase1();
		}

		private void LegacySkillRevertDisplay_Phase1()
		{
			this.m_isRevert = true;
			if (this.m_effect != null)
			{
				this.m_effect.SetActiveSafe(true);
			}
			this.m_owner.PlayAnimation("Appear");
		}

		private void LegacySkillRevertDisplay_End()
		{
			if (!this.m_isRevert)
			{
				return;
			}
			this.m_isRevert = false;
			if (this.m_effect != null)
			{
				this.m_effect.SetActiveSafe(false);
			}
			if (this.curModelGo != null)
			{
				this.curModelGo.SetActiveSafe(false);
			}
			if (this.m_owner.roleSpinePlayer != null)
			{
				this.m_owner.roleSpinePlayer.SetRoleModelShow(true);
			}
			if (this.m_owner.mountSpinePlayer != null)
			{
				this.m_owner.mountSpinePlayer.SetRoleModelShow(true);
			}
			this.m_owner.m_body.m_center.gameObject.SetActiveSafe(true);
			if (this.m_owner.m_mountComponentRegister != null)
			{
				GameObject gameObject = this.m_owner.m_mountComponentRegister.GetGameObject("PointEffect");
				if (gameObject != null)
				{
					gameObject.SetActiveSafe(true);
				}
			}
			this.m_owner.StopAlpha();
			this.m_owner.SetAlpha(1f);
			this.m_owner.PlayAnimationIdle();
		}

		private void ResetParam()
		{
			this.m_playDuration = 0f;
			this.m_isPlay = false;
			this.m_playTime = 1f;
			this.m_playphase1TimeCost = 0f;
			this.m_isPlayPhase1 = false;
			this.m_isPlayPhase2 = false;
			this.m_revertDuration = 0f;
			this.m_isRevert = false;
			this.m_revertTime = 1f;
			this.m_isRevertPhase1 = false;
			this.m_isRevertPhase2 = false;
		}

		private CMemberBase m_owner;

		private GameObject m_effect;

		private Dictionary<int, GameObject> m_modelGoDict = new Dictionary<int, GameObject>();

		public Dictionary<int, NormalSpinePlayerPlayer> legacySummonSpinePlayerDict = new Dictionary<int, NormalSpinePlayerPlayer>();

		private float m_playDuration;

		private float m_playTime = 1f;

		private bool m_isPlay;

		private float m_playphase1TimeCost;

		private bool m_isPlayPhase1;

		private bool m_isPlayPhase2;

		private float m_revertDuration;

		private float m_revertTime = 1f;

		private bool m_isRevert;

		private bool m_isRevertPhase1;

		private bool m_isRevertPhase2;

		private int m_curSkillId;

		private GameObject curModelGo;

		public NormalSpinePlayerPlayer curLegacySummonSpinePlayer;
	}
}
