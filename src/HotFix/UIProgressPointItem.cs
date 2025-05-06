using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIProgressPointItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(int index, int currentWaveIndex, int monsterCfgId)
		{
			MonsterCfg_monsterCfg monsterCfg_monsterCfg = GameApp.Table.GetManager().GetMonsterCfg_monsterCfg(monsterCfgId);
			GameEventBattleType gameEventBattleType = GameEventBattleType.Normal;
			if (monsterCfg_monsterCfg != null)
			{
				gameEventBattleType = (GameEventBattleType)monsterCfg_monsterCfg.battleType;
			}
			this.normalObj.SetActiveSafe(gameEventBattleType == GameEventBattleType.Normal && index <= currentWaveIndex);
			this.imageSpecial.gameObject.SetActiveSafe(gameEventBattleType != GameEventBattleType.Normal);
			if (gameEventBattleType == GameEventBattleType.Boss)
			{
				this.imageSpecial.sprite = this.spriteReg.GetSprite("boss");
			}
			else if (gameEventBattleType == GameEventBattleType.Elite)
			{
				this.imageSpecial.sprite = this.spriteReg.GetSprite("elite");
			}
			if (index == 0)
			{
				this.normalObj.transform.localScale = Vector3.one;
				return;
			}
			this.normalObj.transform.localScale = Vector3.one * 0.8f;
		}

		public GameObject normalObj;

		public CustomImage imageSpecial;

		public SpriteRegister spriteReg;
	}
}
