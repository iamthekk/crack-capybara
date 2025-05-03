using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class SkinData
	{
		public int SkinID { get; private set; }

		public SkinType SkinType { get; private set; }

		public int SkinPrefabID { get; private set; }

		public string Path { get; private set; } = string.Empty;

		public SkinData()
		{
		}

		public SkinData(SkinType skinType, int skinID)
		{
			this.Refresh(skinType, skinID);
		}

		public void Refresh(SkinType skinType, int skinID)
		{
			if (this.SkinType == skinType && this.SkinID == skinID)
			{
				return;
			}
			DxxTools.Game.TryVersionMatchSkin(ref skinID, skinType);
			this.SkinType = skinType;
			this.SkinID = skinID;
			Avatar_Skin elementById = GameApp.Table.GetManager().GetAvatar_SkinModelInstance().GetElementById(skinID);
			if (elementById == null)
			{
				HLog.LogError(string.Format("Avatar_SkinModelInstance is error. skinID = {0}", skinID));
				return;
			}
			this.SkinPrefabID = elementById.skinPrefab;
			ArtMember_clothes elementById2 = GameApp.Table.GetManager().GetArtMember_clothesModelInstance().GetElementById(this.SkinPrefabID);
			if (elementById2 == null)
			{
				HLog.LogError(string.Format("ArtMember_clothesModelInstance is error. skinID = {0}, skinPrefabID = {1}", skinID, this.SkinPrefabID));
				return;
			}
			this.Path = elementById2.path;
		}
	}
}
