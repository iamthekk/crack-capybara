using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.Modules;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class LanguageChooseViewItem : CustomBehaviour
	{
		public void SetType(LanguageRaft_languageTab type)
		{
			this.data = type;
		}

		protected override void OnInit()
		{
			this.Button_Language.m_onClick = new Action(this.OnClickSelectLanguage);
		}

		protected override void OnDeInit()
		{
		}

		public void UpdateButton(LanguageType curSelectType)
		{
			this.Obj_Select.SetActive(curSelectType == this.data.enumId);
		}

		public void RefreshUI()
		{
			this.Text_Content.text = this.data.name;
			this.Text_ContentChoose.text = this.data.name;
		}

		private void OnClickSelectLanguage()
		{
			EventArgLanguageType instance = Singleton<EventArgLanguageType>.Instance;
			instance.SetData(this.data.enumId);
			GameApp.Event.DispatchNow(this, 141, instance);
		}

		public CustomText Text_Content;

		public CustomText Text_ContentChoose;

		public CustomButton Button_Language;

		public GameObject Obj_Select;

		public LanguageRaft_languageTab data;
	}
}
