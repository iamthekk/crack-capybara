using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class TowerMonsterItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.spineModelItem.Init();
			this.towerDataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
			this.addAttributeDataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
		}

		protected override void OnDeInit()
		{
			this.spineModelItem.DeInit();
		}

		public void Refresh(TowerEnemyData enemyData)
		{
			if (enemyData == null)
			{
				return;
			}
			this.heroLevelText.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitower_monster_lv", new object[] { enemyData.Level });
			this.powerCountText.text = DxxTools.FormatNumber(enemyData.Power);
			this.spineModelItem.ShowMemberModel(enemyData.MemberId, "Idle", true);
		}

		public void PlayEffect()
		{
		}

		public UISpineModelItem spineModelItem;

		public CustomText powerCountText;

		public CustomText heroLevelText;

		public RectTransform spineTrans;

		public RectTransform levelTrans;

		public RectTransform powerTrans;

		private TowerDataModule towerDataModule;

		private AddAttributeDataModule addAttributeDataModule;
	}
}
