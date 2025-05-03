using System;
using UnityEngine;

namespace XNode.GameEvent
{
	public enum AttEnum
	{
		[Header("攻击")]
		Attack = 1,
		[Header("当前血量")]
		HP,
		[Header("血上限")]
		HPMax,
		[Header("经验")]
		Exp,
		[Header("食物")]
		Food,
		[Header("防御")]
		Defense,
		[Header("营地回复血量百分百")]
		CampHpRate = 8,
		[Header("赌博机筹码")]
		Chips
	}
}
