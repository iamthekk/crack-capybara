using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Equip;
using UnityEngine.Events;

namespace HotFix
{
	public class UIEquipSelectorNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_equipItem.Init();
			this.m_wearBt.onClick.AddListener(new UnityAction(this.OnClickWearBt));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.m_data = null;
			if (this.m_wearBt != null)
			{
				this.m_wearBt.onClick.RemoveListener(new UnityAction(this.OnClickWearBt));
			}
			if (this.m_equipItem != null)
			{
				this.m_equipItem.DeInit();
			}
		}

		public void RefreshData(EquipData equipData)
		{
			this.m_data = equipData;
			if (this.m_data == null)
			{
				return;
			}
			if (this.m_equipItem != null)
			{
				this.m_equipItem.RefreshData(this.m_data);
			}
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.m_data.nameId);
			}
			double num = GameApp.Data.GetDataModule(DataName.EquipDataModule).MathCombatData(equipData);
			if (this.m_combatTxt != null)
			{
				this.m_combatTxt.text = DxxTools.FormatNumber((long)num);
			}
		}

		private void OnClickWearBt()
		{
			if (this.m_data == null)
			{
				return;
			}
			EquipDataModule dataModule = GameApp.Data.GetDataModule(DataName.EquipDataModule);
			NetworkUtils.Equip.DoEquipDress(dataModule.ReplaceEquipDressRowIds(dataModule.m_equipDressRowIds, this.m_data.equipType, this.m_data.rowID, 0UL), delegate(bool isOk, EquipDressResponse response)
			{
				if (!isOk)
				{
					return;
				}
				GameApp.View.CloseView(ViewName.EquipSelectorViewModule, null);
			});
		}

		public EquipData m_data;

		public UIHeroEquipItem m_equipItem;

		public CustomText m_nameTxt;

		public CustomText m_combatTxt;

		public CustomButton m_wearBt;
	}
}
