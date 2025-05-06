using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UIGameEventItemItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public void Refresh(GameEventItemData data)
		{
			if (data == null)
			{
				return;
			}
			string atlasPath = GameApp.Table.GetAtlasPath(data.atlas);
			this.imageIcon.SetImage(atlasPath, data.icon);
		}

		public CustomImage imageIcon;
	}
}
