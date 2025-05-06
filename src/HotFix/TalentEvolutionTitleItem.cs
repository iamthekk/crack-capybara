using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class TalentEvolutionTitleItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Button_Self.m_onClick = new Action(this.OnClickSelf);
		}

		protected override void OnDeInit()
		{
			this.Button_Self.m_onClick = null;
		}

		private void OnClickSelf()
		{
			if (this.m_cfg == null)
			{
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_TanlentLegacyClickPreview, null);
				return;
			}
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_TalentLegacyClickTitle, null);
		}

		public void SetData(TalentNew_talentMegaStage cfg)
		{
			this.m_cfg = cfg;
			if (cfg != null)
			{
				TalentDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(cfg.languageId);
				string text;
				if (dataModule.TalentStage > cfg.maxStep)
				{
					text = "<color=#D5F44E>" + infoByID + "</color>";
				}
				else if (dataModule.TalentStage >= cfg.minStep && dataModule.TalentStage <= cfg.maxStep)
				{
					text = "<color=#FFF380>" + infoByID + "</color>";
				}
				else
				{
					text = "<color=#6B657F>" + infoByID + "</color>";
				}
				this.txtTitle.text = text;
				return;
			}
			this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("legacy_preview_title");
		}

		public CustomText txtTitle;

		public CustomButton Button_Self;

		private TalentNew_talentMegaStage m_cfg;
	}
}
