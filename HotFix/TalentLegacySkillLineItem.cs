using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class TalentLegacySkillLineItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_talentLegacyDataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			this.m_image = base.GetComponent<CustomImage>();
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(463, new HandlerEvent(this.OnTalentLegacyNodeTimeEnd));
			GameApp.Event.UnRegisterEvent(464, new HandlerEvent(this.OnTalentLegacyNodeTimeEnd));
		}

		public void SetData(int careerId, int nodeId)
		{
			GameApp.Event.RegisterEvent(463, new HandlerEvent(this.OnTalentLegacyNodeTimeEnd));
			GameApp.Event.RegisterEvent(464, new HandlerEvent(this.OnTalentLegacyNodeTimeEnd));
			this.NodeId = nodeId;
			this.CareerId = careerId;
			this.OnRefreshImage();
		}

		private void OnRefreshImage()
		{
			bool flag = this.m_talentLegacyDataModule.IsUnlockTalentLegacyNode(this.NodeId);
			TalentLegacyDataModule.TalentLegacySkillInfo talentLegacySkillInfo = this.m_talentLegacyDataModule.OnGetTalentLegacySkillInfo(this.CareerId, this.NodeId);
			string text = "img_talentLegacyLine01";
			if (flag && talentLegacySkillInfo != null && talentLegacySkillInfo.Level >= 1)
			{
				text = "img_talentLegacyLine02";
			}
			if (this.m_image.sprite.name != text)
			{
				this.m_image.SetImage(160, text);
			}
		}

		private void OnTalentLegacyNodeTimeEnd(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefreshImage();
		}

		public int NodeId;

		public int CareerId;

		private CustomImage m_image;

		private TalentLegacyDataModule m_talentLegacyDataModule;
	}
}
