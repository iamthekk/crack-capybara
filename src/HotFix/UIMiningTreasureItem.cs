using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMiningTreasureItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.miningDataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
			this.buttonClick.onClick.AddListener(new UnityAction(this.OnClickSelf));
		}

		protected override void OnDeInit()
		{
			this.buttonClick.onClick.RemoveListener(new UnityAction(this.OnClickSelf));
		}

		public void SetData(Mining_oreRes oreRes)
		{
			if (oreRes == null)
			{
				this.iconObj.SetActiveSafe(false);
				return;
			}
			this.iconObj.transform.localScale = Vector3.one * oreRes.iconScale;
			this.iconObj.SetActiveSafe(!this.miningDataModule.IsTreasureGet);
		}

		public void OpenTreasure()
		{
			this.iconObj.SetActiveSafe(!this.miningDataModule.IsTreasureGet);
		}

		private void OnClickSelf()
		{
			if (this.miningDataModule.IsTreasureCanGet)
			{
				GameApp.View.OpenView(ViewName.MiningTreasureUpgradeViewModule, null, 1, null, null);
			}
		}

		public CustomButton buttonClick;

		public GameObject iconObj;

		private bool isGet;

		private MiningDataModule miningDataModule;
	}
}
