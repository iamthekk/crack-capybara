using System;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class BossDamageCtr : MonoBehaviour
	{
		public void SetDamage(long damageValue, int indexValue)
		{
			this.damage.text = DxxTools.FormatNumber(damageValue);
			this.index.text = indexValue.ToString();
		}

		public CustomText index;

		public CustomText damage;
	}
}
