using System;
using System.Collections.Generic;
using Framework.DataModule;
using Framework.EventSystem;
using Proto.Common;
using Proto.Guild;
using UnityEngine;

namespace HotFix
{
	public class BattleGuildBossDataModule : IDataModule
	{
		public PVPRecordDto Record
		{
			get
			{
				return this.m_record;
			}
		}

		public ulong OldTotalDamage
		{
			get
			{
				return this.m_oldTotalDamage;
			}
		}

		public ulong CurDamage
		{
			get
			{
				return this.m_curDamage;
			}
		}

		public ulong TotalDamage
		{
			get
			{
				return this.m_totalDamage;
			}
		}

		public int BattleRandomSeed
		{
			get
			{
				if (this.m_record != null)
				{
					return this.m_record.Seed;
				}
				return (int)DateTime.Now.Ticks;
			}
		}

		public int BattleSkillSeed { get; private set; }

		public int MaxRound
		{
			get
			{
				return this.m_maxRound;
			}
		}

		public long BeforeBattleBossHP
		{
			get
			{
				if (this.guildBossBattleResponse != null)
				{
					return this.guildBossBattleResponse.BeforeHp;
				}
				return -1L;
			}
		}

		public int GetName()
		{
			return 115;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_GuildBoss_BattleGuildBossEnter, new HandlerEvent(this.OnBattleEnter));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_GuildBoss_BattleGuildBossEnter, new HandlerEvent(this.OnBattleEnter));
		}

		public void Reset()
		{
		}

		private void OnBattleEnter(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsBattleGuildBossEnter eventArgsBattleGuildBossEnter = eventargs as EventArgsBattleGuildBossEnter;
			if (eventArgsBattleGuildBossEnter != null && eventArgsBattleGuildBossEnter.Response != null)
			{
				this.guildBossBattleResponse = eventArgsBattleGuildBossEnter.Response;
				this.m_record = null;
				this.m_reawrds.Clear();
				this.BoxReawrds.Clear();
				this.m_battleRewards.Clear();
				if (eventArgsBattleGuildBossEnter.Response.CommonData != null)
				{
					this.m_reawrds.AddRange(eventArgsBattleGuildBossEnter.Response.CommonData.Reward.ToItemDatas());
				}
				if (eventArgsBattleGuildBossEnter.Response.BoxReward != null)
				{
					this.BoxReawrds.AddRange(eventArgsBattleGuildBossEnter.Response.BoxReward.ToItemDatas());
				}
				if (eventArgsBattleGuildBossEnter.Response.BattleReward != null)
				{
					this.m_battleRewards.AddRange(eventArgsBattleGuildBossEnter.Response.CommonData.Reward.ToItemDatas());
				}
				this.m_curDamage = eventArgsBattleGuildBossEnter.Response.Damage;
				this.m_totalDamage = eventArgsBattleGuildBossEnter.Response.TotalDamage;
				this.m_oldTotalDamage = eventArgsBattleGuildBossEnter.Response.TotalDamage - this.m_curDamage;
			}
		}

		public List<ItemData> GetReards()
		{
			return this.m_reawrds;
		}

		public List<ItemData> GetBattleRewards()
		{
			return this.m_battleRewards;
		}

		public void SetBattleSkillSeed(int skillSeed)
		{
			this.BattleSkillSeed = skillSeed;
		}

		private const int FORMATION_COUNT = 5;

		[SerializeField]
		private PVPRecordDto m_record;

		private List<ItemData> m_reawrds = new List<ItemData>();

		private List<ItemData> m_battleRewards = new List<ItemData>();

		public List<ItemData> BoxReawrds = new List<ItemData>();

		private ulong m_oldTotalDamage;

		private ulong m_curDamage;

		private ulong m_totalDamage;

		[SerializeField]
		private int m_maxRound = 30;

		public GuildBossEndBattleResponse guildBossBattleResponse;
	}
}
