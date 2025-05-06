using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class UIItemLogicPet : BaseUIItemLogic
	{
		public override void OnRefreshCustom()
		{
			Quality_petQuality elementById = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(base.m_tableData.quality);
			if (elementById != null)
			{
				this.m_item.SetBg(GameApp.Table.GetAtlasPath(elementById.atlasId), elementById.bgSpriteName ?? "");
			}
		}
	}
}
