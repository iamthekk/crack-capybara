using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIAttributeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(Attribute_AttrText attrText, long current, long next, bool isMax)
		{
			string atlasPath = GameApp.Table.GetAtlasPath(attrText.iconAtlasID);
			this.imageAttr.SetImage(atlasPath, attrText.iconName);
			this.textAttrName.text = Singleton<LanguageManager>.Instance.GetInfoByID(attrText.LanguageId);
			this.textCurrentAttr.text = (attrText.ID.Contains("%") ? string.Format("{0}%", current) : current.ToString());
			if (isMax)
			{
				this.textNextAttr.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimount_stage_max");
				return;
			}
			this.textNextAttr.text = (attrText.ID.Contains("%") ? string.Format("{0}%", next) : next.ToString());
		}

		public void ShowAni()
		{
			this.levelUpAni.Play("Show");
		}

		public CustomImage imageAttr;

		public CustomText textAttrName;

		public CustomText textCurrentAttr;

		public CustomText textNextAttr;

		public Animator levelUpAni;
	}
}
