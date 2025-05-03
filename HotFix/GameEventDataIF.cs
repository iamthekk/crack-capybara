using System;
using Server;

namespace HotFix
{
	public class GameEventDataIF : GameEventData
	{
		public GameEventNodeIFType ifType { get; private set; }

		public GameEventNodeOPType opType { get; private set; }

		public GameEventDataIF(GameEventPoolData poolData, GameEventNodeIFType ifType, GameEventNodeOPType opType, float num)
		{
			this.poolData = poolData;
			this.ifType = ifType;
			this.opType = opType;
			this.num = num;
			this.sysRandom = Singleton<GameEventController>.Instance.GetGroupRandom();
			if (this.sysRandom == null)
			{
				this.sysRandom = new XRandom(poolData.randomSeed);
			}
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.IF;
		}

		public override GameEventNodeOptionType GetNodeOptionType()
		{
			return GameEventNodeOptionType.Option;
		}

		public override GameEventData GetNext(int index)
		{
			if (this.children.Count != 2)
			{
				HLog.LogError(string.Format("选择节点必须有两个子节点，eventResId={0}", this.poolData.tableId));
				return null;
			}
			GameEventData gameEventData;
			if (this.CheckResult())
			{
				gameEventData = this.children[0];
			}
			else
			{
				gameEventData = this.children[1];
			}
			while (gameEventData != null && gameEventData.GetNodeOptionType() == GameEventNodeOptionType.Option)
			{
				gameEventData = gameEventData.GetNext(0);
			}
			return gameEventData;
		}

		private bool CheckResult()
		{
			bool flag;
			if (this.ifType == GameEventNodeIFType.WEIGHT)
			{
				flag = this.CheckRandomWeight();
			}
			else if (this.ifType == GameEventNodeIFType.HAS_SKILL)
			{
				flag = Singleton<GameEventController>.Instance.IsHaveSkill((int)this.num);
			}
			else
			{
				flag = this.CheckAttribute();
			}
			return flag;
		}

		private bool CheckRandomWeight()
		{
			bool flag = false;
			int num = this.sysRandom.Range(0, 100);
			long luck = this.GetLuck((long)this.num);
			GameEventNodeOPType opType = this.opType;
			if (opType != GameEventNodeOPType.Greater)
			{
				if (opType == GameEventNodeOPType.Less)
				{
					if ((long)num < luck)
					{
						flag = true;
					}
				}
			}
			else if ((long)num > luck)
			{
				flag = true;
			}
			return flag;
		}

		private bool CheckAttribute()
		{
			bool flag = false;
			long currentAttribute = this.GetCurrentAttribute();
			GameEventNodeOPType opType = this.opType;
			if (opType != GameEventNodeOPType.Greater)
			{
				if (opType == GameEventNodeOPType.Less)
				{
					if ((float)currentAttribute <= this.num)
					{
						flag = true;
					}
				}
			}
			else if ((float)currentAttribute >= this.num)
			{
				flag = true;
			}
			return flag;
		}

		private long GetCurrentAttribute()
		{
			BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
			if (playerData != null)
			{
				switch (this.ifType)
				{
				case GameEventNodeIFType.ATK:
					return playerData.Attack.AsLong();
				case GameEventNodeIFType.DEF:
					return playerData.Defence.AsLong();
				case GameEventNodeIFType.RHP:
					return playerData.CurrentHp.AsLong();
				case GameEventNodeIFType.HP:
					return playerData.HpMax.AsLong();
				case GameEventNodeIFType.EXP_LV:
					return (long)playerData.ExpLevel.mVariable;
				case GameEventNodeIFType.SHELL:
					return (long)playerData.Food;
				}
			}
			return 0L;
		}

		private long GetLuck(long value)
		{
			return value;
		}

		public override string GetInfo()
		{
			return string.Format("IF_{0}_{1}_{2}", this.ifType, this.opType, this.num);
		}

		public float num;

		private readonly XRandom sysRandom;
	}
}
