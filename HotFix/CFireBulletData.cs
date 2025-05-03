using System;
using Framework;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class CFireBulletData
	{
		public int m_bulletID { get; private set; }

		public string m_artPath { get; private set; }

		public Transform m_startPoint { get; private set; }

		public Transform m_endPoint { get; private set; }

		public int m_startSoundID { get; private set; }

		public int m_hitSoundID { get; private set; }

		public bool IsMainTarget { get; set; }

		public bool IsShowBullet { get; set; }

		public void SetBulletID(int bulletID)
		{
			this.m_bulletID = bulletID;
			GameBullet_bullet elementById = GameApp.Table.GetManager().GetGameBullet_bulletModelInstance().GetElementById(this.m_bulletID);
			ArtBullet_Bullet elementById2 = GameApp.Table.GetManager().GetArtBullet_BulletModelInstance().GetElementById(elementById.prefabID);
			if (elementById2 == null)
			{
				HLog.LogError(string.Format("Tabla[ArtBullet_Bullet] is Error  prefabID = {0}", elementById.prefabID));
			}
			this.m_artPath = elementById2.path;
		}

		public void SetPosPoint(Transform start, Transform end)
		{
			this.m_startPoint = start;
			this.m_endPoint = end;
		}

		public void SetSoundData(int startSoundID, int hitSoundID)
		{
			this.m_startSoundID = startSoundID;
			this.m_hitSoundID = hitSoundID;
		}
	}
}
