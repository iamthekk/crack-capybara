using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine.Events;

namespace HotFix
{
	public class UIHelpButton : CustomBehaviour
	{
		protected override void OnInit()
		{
			Module_moduleInfo elementById = GameApp.Table.GetManager().GetModule_moduleInfoModelInstance().GetElementById((int)this.ModuleId);
			if (elementById != null)
			{
				this.nameId = elementById.nameId;
				this.desId = elementById.infoId;
			}
			this.button.onClick.AddListener(new UnityAction(this.OnClickHelp));
		}

		protected override void OnDeInit()
		{
			this.button.onClick.RemoveListener(new UnityAction(this.OnClickHelp));
		}

		private void OnClickHelp()
		{
			if (!string.IsNullOrEmpty(this.nameId) && !string.IsNullOrEmpty(this.desId))
			{
				InfoCommonViewModule.OpenData openData = new InfoCommonViewModule.OpenData
				{
					m_tileInfo = Singleton<LanguageManager>.Instance.GetInfoByID(this.nameId),
					m_contextInfo = Singleton<LanguageManager>.Instance.GetInfoByID(this.desId)
				};
				GameApp.View.OpenView(ViewName.InfoCommonViewModule, openData, 1, null, null);
			}
		}

		public EModuleId ModuleId;

		public CustomButton button;

		private string nameId;

		private string desId;
	}
}
