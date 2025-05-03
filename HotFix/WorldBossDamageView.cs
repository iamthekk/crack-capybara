using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Google.Protobuf.Collections;
using Proto.Mission;
using UnityEngine;

namespace HotFix
{
	public class WorldBossDamageView : BaseViewModule
	{
		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public override void OnCreate(object data)
		{
			this.closeButton.m_onClick = new Action(this.OnCloseButtonClick);
			this.markButton.m_onClick = new Action(this.OnCloseButtonClick);
			this.copyItem.gameObject.SetActive(false);
		}

		public override void OnDelete()
		{
			this.closeButton.m_onClick = null;
			this.markButton.m_onClick = null;
		}

		private void OnCloseButtonClick()
		{
			GameApp.View.CloseView(ViewName.WorldBossDamageViewModule, null);
		}

		public override void OnOpen(object data)
		{
			WorldBossDataModule dataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
			this.bossName.SetText(dataModule.bossCfg.nameLanguageID);
			RepeatedField<BattleRecordDto> historyList = dataModule.HistoryList;
			int count = historyList.Count;
			if (historyList.Count == 0)
			{
				this.emptyTip.SetActive(true);
				return;
			}
			this.emptyTip.SetActive(false);
			for (int i = 0; i < count; i++)
			{
				BossDamageCtr bossDamageCtr;
				if (i < this.itemCacheList.Count)
				{
					bossDamageCtr = this.itemCacheList[i];
				}
				else
				{
					bossDamageCtr = Object.Instantiate<BossDamageCtr>(this.copyItem, this.scrollContent);
					this.itemCacheList.Add(bossDamageCtr);
				}
				if (!bossDamageCtr.gameObject.activeSelf)
				{
					bossDamageCtr.gameObject.SetActive(true);
				}
				bossDamageCtr.transform.localScale = Vector3.one;
				bossDamageCtr.SetDamage(historyList[i].Damage, i + 1);
			}
		}

		public override void OnClose()
		{
			foreach (BossDamageCtr bossDamageCtr in this.itemCacheList)
			{
				bossDamageCtr.gameObject.SetActive(false);
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public CustomButton closeButton;

		public CustomButton markButton;

		public CustomText bossName;

		public GameObject emptyTip;

		public Transform scrollContent;

		public BossDamageCtr copyItem;

		private List<BossDamageCtr> itemCacheList = new List<BossDamageCtr>();
	}
}
