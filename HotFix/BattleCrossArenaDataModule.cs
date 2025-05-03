using System;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Proto.Common;
using UnityEngine;

namespace HotFix
{
	public class BattleCrossArenaDataModule : IDataModule
	{
		public int targetChangeScore { get; private set; }

		public int ownerChangeScore { get; private set; }

		public PVPRecordDto Record
		{
			get
			{
				return this.m_record;
			}
		}

		public int Duration
		{
			get
			{
				return this.m_duration;
			}
		}

		public int GetName()
		{
			return 112;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_BattleCrossArena_BattleCrossArenaEnter, new HandlerEvent(this.OnBattleEnter));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_BattleCrossArena_BattleCrossArenaEnter, new HandlerEvent(this.OnBattleEnter));
		}

		public void Reset()
		{
			this.m_record = null;
			this.targetChangeScore = 0;
			this.ownerChangeScore = 0;
			this.m_isRecord = false;
		}

		private void OnBattleEnter(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsBattleCrossArenaEnter eventArgsBattleCrossArenaEnter = eventargs as EventArgsBattleCrossArenaEnter;
			if (eventArgsBattleCrossArenaEnter == null)
			{
				return;
			}
			this.m_record = eventArgsBattleCrossArenaEnter.m_record;
			this.targetChangeScore = this.m_record.TargetChangeScore;
			this.ownerChangeScore = this.m_record.OwnerChangeScore;
			this.m_isRecord = eventArgsBattleCrossArenaEnter.m_isRecord;
		}

		public bool IsWin()
		{
			if (this.Record == null)
			{
				return false;
			}
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			bool flag = this.Record.Result != 0;
			if (dataModule.userId != this.Record.OwnerUser.UserId)
			{
				return !flag;
			}
			return flag;
		}

		public bool CheckAttackerIsMe()
		{
			return this.Record != null && GameApp.Data.GetDataModule(DataName.LoginDataModule).userId == this.Record.OwnerUser.UserId;
		}

		public void BindClothesData()
		{
			if (this.m_record != null && this.m_record.OtherUser != null)
			{
				ClothesDataModule dataModule = GameApp.Data.GetDataModule(DataName.ClothesDataModule);
				dataModule.UpdateBattleClothesData(true, this.m_record.OwnerUser.SkinHeaddressId, this.m_record.OwnerUser.SkinBodyId, this.m_record.OwnerUser.SkinAccessoryId);
				dataModule.UpdateBattleClothesData(false, this.m_record.OtherUser.SkinHeaddressId, this.m_record.OtherUser.SkinBodyId, this.m_record.OtherUser.SkinAccessoryId);
			}
		}

		public void UnBindClothesData()
		{
			ClothesDataModule dataModule = GameApp.Data.GetDataModule(DataName.ClothesDataModule);
			dataModule.ClearBattleClothesData(true);
			dataModule.ClearBattleClothesData(false);
		}

		[SerializeField]
		private PVPRecordDto m_record;

		public bool m_isRecord;

		[SerializeField]
		private int m_duration = 15;
	}
}
