using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using LocalModels.Bean;
using Server;

namespace HotFix.Client
{
	public class CBuffData : IDisposable
	{
		public int m_id { get; private set; }

		public int m_prefabID { get; private set; }

		public string m_prefabPath { get; private set; } = string.Empty;

		public float m_effectDuration { get; private set; }

		public BuffType m_buffType { get; private set; } = BuffType.Default;

		public string m_parameters { get; private set; } = string.Empty;

		public MemberBodyPosType m_bodyType { get; private set; } = 2;

		public PointRotationDirection m_bodyDirection { get; private set; }

		public int m_removeEffectID { get; private set; }

		public string m_removeEffectPath { get; private set; } = string.Empty;

		public MemberBodyPosType m_removeEffectPos { get; private set; } = 2;

		public int m_soundID { get; private set; }

		public void SetTableData(GameBuff_buff data)
		{
			this.m_id = data.id;
			this.m_prefabID = data.prefabID;
			this.m_buffType = (BuffType)data.buffType;
			this.m_parameters = data.parameters;
			ArtBuff_Buff elementById = GameApp.Table.GetManager().GetArtBuff_BuffModelInstance().GetElementById(this.m_prefabID);
			if (elementById != null)
			{
				this.m_prefabPath = elementById.path;
				this.m_effectDuration = elementById.effectDuration;
			}
			else
			{
				HLog.LogError(HLog.ToColor(string.Format("ArtBuff_BuffModel is Error. m_prefabID = {0}", this.m_prefabID), 3));
			}
			List<int> listInt = data.transType.GetListInt(',');
			this.m_bodyType = listInt[0];
			this.m_bodyDirection = (PointRotationDirection)listInt[1];
			this.m_removeEffectID = data.removeEffectID;
			if (!this.m_removeEffectID.Equals(0))
			{
				ArtBuff_Buff elementById2 = GameApp.Table.GetManager().GetArtBuff_BuffModelInstance().GetElementById(this.m_removeEffectID);
				if (elementById2 != null)
				{
					this.m_removeEffectPath = elementById2.path;
				}
				this.m_removeEffectPos = data.removeEffectPos;
			}
			this.m_soundID = data.soundId;
		}

		public void Dispose()
		{
		}
	}
}
