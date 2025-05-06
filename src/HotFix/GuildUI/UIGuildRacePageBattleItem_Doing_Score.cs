using System;
using System.Collections.Generic;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildRacePageBattleItem_Doing_Score : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.Scores.Clear();
			Transform transform = base.gameObject.transform;
			for (int i = 1; i <= 3; i++)
			{
				CustomChooseButton component = transform.Find(string.Format("Round{0}", i)).GetComponent<CustomChooseButton>();
				this.Scores.Add(component);
			}
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public void SetScore(int index, bool sel)
		{
			if (index >= 0 && index < this.Scores.Count)
			{
				this.Scores[index].SetSelect(sel);
			}
		}

		public void RevertScore(int startindex)
		{
			for (int i = startindex; i < this.Scores.Count; i++)
			{
				this.Scores[i].SetSelect(false);
			}
		}

		public List<CustomChooseButton> Scores = new List<CustomChooseButton>();
	}
}
