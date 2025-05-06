using System;

namespace HotFix
{
	public class SceneSkinData
	{
		public int CurSkinId { get; private set; } = 1;

		public SceneSkinData(int skinId)
		{
			this.OnUpdateSkinId(skinId);
		}

		public void OnUpdateSkinId(int skinId)
		{
			if (skinId <= 0)
			{
				skinId = 1;
			}
			this.CurSkinId = skinId;
		}
	}
}
