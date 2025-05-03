using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class CommonCombatItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_AddAttributeData_RefreshDatas, new HandlerEvent(this.Event_CombatUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_TipViewModule_AddAddCombatTipNode, new HandlerEvent(this.Event_CombatUpdate));
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_AddAttributeData_RefreshDatas, new HandlerEvent(this.Event_CombatUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_TipViewModule_AddAddCombatTipNode, new HandlerEvent(this.Event_CombatUpdate));
		}

		public void OnShow()
		{
			this.SetCombat();
		}

		public void OnHide()
		{
		}

		private void Event_CombatUpdate(object sender, int type, BaseEventArgs args)
		{
			this.SetCombat();
		}

		public void SetCombat()
		{
			AddAttributeDataModule dataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			this.Text_Combat.text = DxxTools.FormatNumber((long)dataModule.Combat);
		}

		public CustomText Text_Combat;
	}
}
