using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class UIItemLogicEquip : BaseUIItemLogic
	{
		public override void OnRefreshCustom()
		{
			if (this.m_item == null)
			{
				return;
			}
			this.m_item.CreateStar();
			this.m_item.m_star.SetStarCount(0);
			PropData propData = this.m_item.m_args as PropData;
			if (propData != null)
			{
				this.m_item.m_star.SetStarCount((int)propData.level);
			}
			if (this.m_item.m_starParent != null)
			{
				this.m_item.m_starParent.gameObject.SetActive(true);
			}
		}

		public override void OnRefreshSpecialTypeView()
		{
			this.m_item.SetQualityTxtImageActive(true);
			this.m_item.SetTypeBgImageActive(true);
			Equip_equip elementById = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById(base.m_tableData.id);
			if (elementById != null)
			{
				Equip_equipType elementById2 = GameApp.Table.GetManager().GetEquip_equipTypeModelInstance().GetElementById(elementById.Type);
				if (elementById2 != null)
				{
					Quality_equipQuality elementById3 = GameApp.Table.GetManager().GetQuality_equipQualityModelInstance().GetElementById(base.m_tableData.quality);
					if (elementById3 != null)
					{
						this.m_item.SetTypeBgImage(base.getAtlasPath(105), elementById3.equipTypeBgSpriteName ?? "");
						string atlasPath = GameApp.Table.GetAtlasPath(elementById2.atlasID);
						this.m_item.SetTypeImage(atlasPath, elementById2.iconName);
						this.m_item.SetQualityRank(elementById.rank);
					}
				}
			}
		}
	}
}
