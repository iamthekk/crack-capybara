using System;

namespace Server
{
	public static class ETalentAttributeTypeUtils
	{
		public static ETalentAttributeType String2ETalentAttributeType(string attrinuteName)
		{
			if (attrinuteName == "HPMax")
			{
				return ETalentAttributeType.HPMax;
			}
			if (attrinuteName == "Attack")
			{
				return ETalentAttributeType.Attack;
			}
			if (!(attrinuteName == "Defence"))
			{
				return ETalentAttributeType.None;
			}
			return ETalentAttributeType.Defence;
		}
	}
}
