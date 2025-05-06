using System;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix.CrossArenaRewardsUI
{
	public class CrossArenaRewardsBottomTabButtons : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.SwitchButtonGroup.OnSwitch = new Action<CustomChooseButton>(this.OnSwitchTab);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SwitchTab(CrossArenaRewardsKind kind)
		{
			if (kind == CrossArenaRewardsKind.Daily)
			{
				this.SwitchButtonGroup.ChooseButtonName("Button_Daily");
				return;
			}
			if (kind != CrossArenaRewardsKind.Season)
			{
				return;
			}
			this.SwitchButtonGroup.ChooseButtonName("Button_Season");
		}

		private void OnSwitchTab(CustomChooseButton button)
		{
			if (button == null)
			{
				return;
			}
			CrossArenaRewardsKind crossArenaRewardsKind = this.CurSelectKind;
			string name = button.name;
			if (!(name == "Button_Daily"))
			{
				if (name == "Button_Season")
				{
					crossArenaRewardsKind = CrossArenaRewardsKind.Season;
				}
			}
			else
			{
				crossArenaRewardsKind = CrossArenaRewardsKind.Daily;
			}
			if (crossArenaRewardsKind != this.CurSelectKind)
			{
				this.OnSwitchTabButton(crossArenaRewardsKind);
			}
		}

		private void OnSwitchTabButton(CrossArenaRewardsKind kind)
		{
			this.CurSelectKind = kind;
			Action<CrossArenaRewardsKind> onSwitch = this.OnSwitch;
			if (onSwitch == null)
			{
				return;
			}
			onSwitch(this.CurSelectKind);
		}

		public CustomChooseButtonGroup SwitchButtonGroup;

		public CrossArenaRewardsKind CurSelectKind;

		public Action<CrossArenaRewardsKind> OnSwitch;
	}
}
