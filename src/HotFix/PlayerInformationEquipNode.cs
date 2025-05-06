using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class PlayerInformationEquipNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_equipmentItem.Init();
			this.m_equipmentItem.m_onClick = new Action<UIHeroEquipItem>(this.OnClickEquipItem);
		}

		protected override void OnDeInit()
		{
			if (this.m_equipmentItem != null)
			{
				this.m_equipmentItem.DeInit();
			}
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

		private void OnClickEquipItem(UIHeroEquipItem obj)
		{
			if (obj == null || obj.m_equipData == null)
			{
				return;
			}
			EquipDetailsViewModule.OpenData openData = new EquipDetailsViewModule.OpenData();
			openData.m_equipData = obj.m_equipData;
			openData.m_isShowButtons = false;
			GameApp.View.OpenView(ViewName.EquipDetailsViewModule, openData, 1, null, null);
		}

		public CustomImage m_icon;

		public UIHeroEquipItem m_equipmentItem;

		public EquipType m_equipType = EquipType.Weapon;

		public int m_equipTypeIndex;
	}
}
