using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine.Events;

namespace HotFix
{
	public class PetBoxAdvance : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.m_btnItem.onClick.AddListener(new UnityAction(this.OnBtnItemClick));
			this.redNode.Value = 0;
		}

		protected override void OnDeInit()
		{
			this.btnItemClickCallback = null;
			this.m_btnItem.onClick.RemoveListener(new UnityAction(this.OnBtnItemClick));
		}

		public void Refresh(PetDataModule petDataModule, EPetBoxType petBoxType)
		{
			this.petBoxType = petBoxType;
			long itemDataCountByid = this.propDataModule.GetItemDataCountByid(11UL);
			long itemDataCountByid2 = this.propDataModule.GetItemDataCountByid(2UL);
			int num;
			int num2;
			int num3;
			if (petBoxType == EPetBoxType.Draw35)
			{
				num = Singleton<GameConfig>.Instance.Pet35DrawTicketCost;
				num2 = Singleton<GameConfig>.Instance.Pet35DrawDiamondCost;
				num3 = Singleton<GameConfig>.Instance.Pet35DrawResultCount;
			}
			else
			{
				num = Singleton<GameConfig>.Instance.Pet15DrawTicketCost;
				num2 = Singleton<GameConfig>.Instance.Pet15DrawDiamondCost;
				num3 = Singleton<GameConfig>.Instance.Pet15DrawResultCount;
			}
			this.m_txtDrawTimes.text = string.Format("x{0}", num3);
			ItemData itemData = new ItemData();
			if (itemDataCountByid >= (long)num)
			{
				itemData.SetID(11);
				itemData.SetCount((long)num);
				this.m_costItem.SetCustomStyle1("x{0}", itemData, itemDataCountByid, true);
				this.redNode.Value = 1;
				return;
			}
			itemData.SetID(2);
			itemData.SetCount((long)num2);
			this.m_costItem.SetCustomStyle1("x{0}", itemData, itemDataCountByid2, true);
			this.redNode.Value = 0;
		}

		public void SetCallback(Action<PetBoxAdvance> cb)
		{
			this.btnItemClickCallback = cb;
		}

		private void OnBtnItemClick()
		{
			Action<PetBoxAdvance> action = this.btnItemClickCallback;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		public CustomButton m_btnItem;

		public CustomText m_txtDrawTimes;

		public CommonCostItem m_costItem;

		public EPetBoxType petBoxType;

		public RedNodeOneCtrl redNode;

		private Action<PetBoxAdvance> btnItemClickCallback;

		protected PropDataModule propDataModule;
	}
}
