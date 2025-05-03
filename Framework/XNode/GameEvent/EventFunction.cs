using System;

namespace XNode.GameEvent
{
	public enum EventFunction
	{
		None,
		AddEventPoint,
		DoEventPoint,
		PlayerAnimation,
		ChangeMap,
		AddMonsterGroup,
		MonsterGroupLeave,
		AddFollowNpc,
		Emoticons,
		PassingNpc = 10,
		LostRandomSkill,
		LostTagSkills,
		GetSkill,
		RandomSkill,
		ChangeAttr,
		GetItem,
		LevelUp,
		LostIDSkill = 19,
		LostAllFood,
		RandomBox,
		FixedBox,
		RandomSurprise,
		FixedSurprise,
		GetItemServer,
		FishingAllEnd = 101
	}
}
