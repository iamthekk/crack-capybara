using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIRoundSelectSkillItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.skillItem.Init();
		}

		protected override void OnDeInit()
		{
			this.skillItem.DeInit();
		}

		public void SetData(GameEventSkillBuildData data, Action<GameEventSkillBuildData> onSelect)
		{
			this.skillBuild = data;
			this.skillItem.Refresh(data, onSelect);
		}

		public void Refresh(List<GameEventSkillBuildData> selectSkills)
		{
			if (selectSkills == null)
			{
				this.selectObj.SetActiveSafe(false);
				return;
			}
			bool flag = selectSkills.Contains(this.skillBuild);
			this.selectObj.SetActiveSafe(flag);
		}

		public void PlayEffect()
		{
			this.skillItem.PlayEffect();
		}

		public GameObject selectObj;

		public UIGameEventSelectSkillItem skillItem;

		private GameEventSkillBuildData skillBuild;
	}
}
