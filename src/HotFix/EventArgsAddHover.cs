using System;
using Framework.EventSystem;
using UnityEngine;

namespace HotFix
{
	public class EventArgsAddHover : BaseEventArgs
	{
		public void SetData(HoverType type, HoverData targetData, object data, object param)
		{
			if (type == HoverType.GetItem)
			{
				this.otherParam = param;
			}
			this.SetData(type, targetData, data);
		}

		public void SetData(HoverType type, HoverData targetData, object data)
		{
			this.Clear();
			this.targetData = targetData;
			this.type = type;
			switch (type)
			{
			case HoverType.FriendlyStateBar:
			case HoverType.EnemyStateBar:
				this.SetStateBarData(type, this.targetData, null);
				return;
			case HoverType.LessHp:
			case HoverType.LessHpCrit:
			case HoverType.LessHpBigSkill:
			case HoverType.PlusHp:
			case HoverType.LessHpIce:
			case HoverType.LessHpFire:
			case HoverType.LessHpThunder:
			case HoverType.LessHpPoison:
			case HoverType.LessHPTrueDamage:
			{
				HoverLongData hoverLongData = new HoverLongData();
				hoverLongData.SetData((long)data);
				this.hoverData = hoverLongData;
				return;
			}
			case HoverType.BattleText:
			case HoverType.WaitRoundCount:
				this.hoverData = data;
				return;
			case HoverType.BigSkillName:
			case HoverType.NormalSkillName:
			case HoverType.BuffName:
			case HoverType.BattleSign:
			case HoverType.Emoticons:
			{
				HoverStringData hoverStringData = new HoverStringData();
				hoverStringData.SetData((string)data, this.targetData.id, this.targetData.targetCamp);
				this.hoverData = hoverStringData;
				return;
			}
			case HoverType.GetSkill:
			{
				HoverSkillBuildData hoverSkillBuildData = new HoverSkillBuildData();
				hoverSkillBuildData.SetData((GameEventSkillBuildData)data);
				this.hoverData = hoverSkillBuildData;
				return;
			}
			case HoverType.GetItem:
				if (this.otherParam != null && this.otherParam is Vector3)
				{
					HoverEventItemData hoverEventItemData = new HoverEventItemData();
					hoverEventItemData.SetData((GameEventItemData)data, (Vector3)this.otherParam);
					this.hoverData = hoverEventItemData;
				}
				return;
			case HoverType.SkillName:
			{
				HoverLongData hoverLongData2 = new HoverLongData();
				long num = Convert.ToInt64(data);
				hoverLongData2.SetData(num, this.targetData.id, this.targetData.targetCamp);
				this.hoverData = hoverLongData2;
				return;
			}
			default:
				return;
			}
		}

		public void SetStateBarData(HoverType type, HoverData targetData)
		{
			this.SetStateBarData(type, targetData, null);
		}

		private void SetStateBarData(HoverType type, HoverData targetData, HoverStateBarData data)
		{
			if (type == HoverType.FriendlyStateBar || type == HoverType.EnemyStateBar)
			{
				if (data == null)
				{
					data = new HoverStateBarData();
					ClampData clampData = new ClampData(targetData.currentHp, targetData.maxHp, 0L);
					ClampData clampData2 = new ClampData(targetData.currentRecharge, targetData.maxRecharge, 0L);
					bool flag = true;
					if (type == HoverType.EnemyStateBar)
					{
						flag = targetData.isShowRecharge;
					}
					data.SetData(clampData, clampData2, flag);
				}
				this.targetData = targetData;
				this.type = type;
				this.hoverData = data;
				return;
			}
			HLog.LogError("EventArgsAddHover.SetStateBarData: type must be FriendlyStateBar or EnemyStateBar");
		}

		public override void Clear()
		{
			this.hoverData = null;
			this.targetData = null;
		}

		public HoverType type;

		public HoverData targetData;

		public object hoverData;

		public object otherParam;
	}
}
