using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIGameEventSkillIconItem : CustomBehaviour
	{
		public int ID
		{
			get
			{
				return this.m_skillID;
			}
			set
			{
				this.m_skillID = value;
			}
		}

		protected override void OnInit()
		{
			this.SetActiveBg(false);
		}

		protected override void OnDeInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public void Refresh(GameEventSkillBuildData data)
		{
			if (data == null)
			{
				return;
			}
			this.m_skillID = data.id;
			this.Refresh(data.skillAtlas, data.skillIcon, data.skillIconBadge, data.config.quality);
		}

		public void Refresh(int skillBuildId)
		{
			this.m_skillID = skillBuildId;
			GameSkillBuild_skillBuild elementById = GameApp.Table.GetManager().GetGameSkillBuild_skillBuildModelInstance().GetElementById(skillBuildId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("Not found [GameSkillBuild] table, id={0}", skillBuildId));
				return;
			}
			GameSkill_skill elementById2 = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(elementById.skillId);
			if (elementById2 == null)
			{
				HLog.LogError(string.Format("Not found [GameSkill] table, id={0}", elementById.skillId));
				return;
			}
			string atlasPath = GameApp.Table.GetAtlasPath(elementById2.iconAtlasID);
			this.Refresh(atlasPath, elementById2.icon, elementById2.iconBadge, elementById.quality);
		}

		private void Refresh(string atlas, string icon, string badge, int quality)
		{
			if (string.IsNullOrEmpty(atlas) || string.IsNullOrEmpty(icon))
			{
				HLog.LogError(string.Format("atlas or icon is null : GameSkillBuild={0}", this.m_skillID));
				return;
			}
			this.imageIcon.SetImage(atlas, icon);
			if (string.IsNullOrEmpty(badge))
			{
				this.imageIconBadge.gameObject.SetActive(false);
			}
			else
			{
				this.imageIconBadge.gameObject.SetActive(true);
				this.imageIconBadge.SetImage(atlas, badge);
			}
			string atlasPath = GameApp.Table.GetAtlasPath(105);
			this.imageQuality.SetImage(atlasPath, GameEventSkillBuildData.GetQuality((SkillBuildQuality)quality));
		}

		public void PlayAnimation()
		{
			float t = 0.2f;
			TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(base.gameObject.transform, this.iconScale, t), 6), delegate
			{
				TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.gameObject.transform, Vector3.one, t), 7);
			});
		}

		public void SetActiveBg(bool isActive)
		{
			this.iconBg.SetActiveSafe(isActive);
		}

		public CustomImage imageIcon;

		public CustomImage imageIconBadge;

		public GameObject iconBg;

		public CustomImage imageQuality;

		private const float iconAnimationTime = 0.4f;

		private Vector3 iconScale = new Vector3(1.2f, 1.2f, 1f);

		private int m_skillID;
	}
}
