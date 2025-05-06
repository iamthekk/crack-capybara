using System;
using Framework;
using Framework.Logic.UI;

namespace HotFix
{
	public class ActivityScoreTypeData : AttributeTypeDataBase
	{
		public override void SetImage(CustomImage image)
		{
			string atlasPath = GameApp.Table.GetAtlasPath(this.atlas);
			image.SetImage(atlasPath, this.icon);
		}

		public int atlas;

		public string icon;
	}
}
