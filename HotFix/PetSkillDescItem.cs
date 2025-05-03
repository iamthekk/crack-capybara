using System;
using System.Text.RegularExpressions;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class PetSkillDescItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(int index, int skillId, bool unlock)
		{
			GameSkill_skill elementById = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(skillId);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.infoID);
			string text = "<color=#[0-9a-fA-F]{6}>|<color=[a-zA-Z]+>|</color>";
			string text2 = Regex.Replace(infoByID, text, string.Empty);
			string text3 = Singleton<LanguageManager>.Instance.GetInfoByID("skill_level_desc", new object[]
			{
				index + 2,
				text2
			});
			if (unlock)
			{
				text3 = "<color=#399639>" + text3 + "</color>";
			}
			else
			{
				text3 = "<color=#D1AC87>" + text3 + "</color>";
			}
			this.txtDesc.text = text3;
		}

		public CustomText txtDesc;
	}
}
