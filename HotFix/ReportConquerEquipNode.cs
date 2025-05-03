using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public class ReportConquerEquipNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_equipmentItem.Init();
		}

		protected override void OnDeInit()
		{
			if (this.m_equipmentItem != null)
			{
				this.m_equipmentItem.DeInit();
			}
			this.m_icon = null;
			this.m_equipmentItem = null;
		}

		public void SetData(EquipType equipType)
		{
			this.m_equipType = equipType;
		}

		public void RefreshData(EquipData equipData)
		{
			if (this.m_icon != null)
			{
				Equip_equipType elementById = GameApp.Table.GetManager().GetEquip_equipTypeModelInstance().GetElementById((int)this.m_equipType);
				string atlasPath = GameApp.Table.GetAtlasPath(elementById.atlasID);
				this.m_icon.SetImage(atlasPath, elementById.iconName);
			}
			if (this.m_equipmentItem != null)
			{
				bool flag = equipData != null;
				this.m_equipmentItem.SetActive(flag);
				if (flag)
				{
					this.m_equipmentItem.RefreshData(equipData);
				}
			}
		}

		public CustomImage m_icon;

		public UIHeroEquipItem m_equipmentItem;

		[HideInInspector]
		public EquipType m_equipType = EquipType.Weapon;
	}
}
