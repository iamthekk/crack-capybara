using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class ClothesData
	{
		public int HeadId
		{
			get
			{
				return this.m_HeadId;
			}
			set
			{
				this.CheckHeadSafeId(value);
			}
		}

		public int BodyId
		{
			get
			{
				return this.m_BodyId;
			}
			set
			{
				this.CheckBodySafeId(value);
			}
		}

		public int AccessoryId
		{
			get
			{
				return this.m_AccessoryId;
			}
			set
			{
				this.CheckAccessorySafeId(value);
			}
		}

		public ClothesData()
		{
			this.changed = true;
			this.HeadId = 0;
			this.BodyId = 0;
			this.AccessoryId = 0;
		}

		public ClothesData(int headId, int bodyId, int accessoryId)
		{
			this.changed = true;
			this.FreshData(headId, bodyId, accessoryId);
		}

		public void FreshData(int headId, int bodyId, int accessoryId)
		{
			this.HeadId = headId;
			this.BodyId = bodyId;
			this.AccessoryId = accessoryId;
		}

		private void CheckHeadSafeId(int headId)
		{
			if (headId == 0)
			{
				headId = Singleton<GameConfig>.Instance.ClothesDefaultHeadId;
			}
			if (this.m_HeadId != headId)
			{
				this.m_HeadId = headId;
				this.changed = true;
			}
		}

		private void CheckBodySafeId(int bodyId)
		{
			if (bodyId == 0)
			{
				bodyId = Singleton<GameConfig>.Instance.ClothesDefaultBodyId;
			}
			if (this.m_BodyId != bodyId)
			{
				this.m_BodyId = bodyId;
				this.changed = true;
			}
		}

		private void CheckAccessorySafeId(int accessoryId)
		{
			if (accessoryId == 0)
			{
				accessoryId = Singleton<GameConfig>.Instance.ClothesDefaultAccessoryId;
			}
			if (this.m_AccessoryId != accessoryId)
			{
				this.m_AccessoryId = accessoryId;
				this.changed = true;
			}
		}

		public Dictionary<SkinType, SkinData> GetSkinDatas()
		{
			bool flag = false;
			if (this.m_SkinDatas == null)
			{
				flag = true;
				this.m_SkinDatas = new Dictionary<SkinType, SkinData>();
			}
			else if (this.changed)
			{
				flag = true;
			}
			if (flag)
			{
				this.changed = false;
				SkinData skinData;
				if (this.m_SkinDatas.TryGetValue(SkinType.Body, out skinData))
				{
					skinData.Refresh(SkinType.Body, this.m_BodyId);
				}
				else
				{
					this.m_SkinDatas.Add(SkinType.Body, new SkinData(SkinType.Body, this.m_BodyId));
				}
				SkinData skinData2;
				if (this.m_SkinDatas.TryGetValue(SkinType.Head, out skinData2))
				{
					skinData2.Refresh(SkinType.Head, this.m_HeadId);
				}
				else
				{
					this.m_SkinDatas.Add(SkinType.Head, new SkinData(SkinType.Head, this.m_HeadId));
				}
				SkinData skinData3;
				if (this.m_SkinDatas.TryGetValue(SkinType.Back, out skinData3))
				{
					skinData3.Refresh(SkinType.Back, this.m_AccessoryId);
				}
				else
				{
					this.m_SkinDatas.Add(SkinType.Back, new SkinData(SkinType.Back, this.m_AccessoryId));
				}
			}
			return this.m_SkinDatas;
		}

		public void DressPart(SkinType skinType, int skinId)
		{
			Avatar_Skin elementById = GameApp.Table.GetManager().GetAvatar_SkinModelInstance().GetElementById(skinId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("Avatar_SkinModelInstance is error. skinID = {0}", skinId));
				return;
			}
			switch (skinType)
			{
			case SkinType.Body:
				this.BodyId = skinId;
				break;
			case SkinType.Head:
				this.HeadId = skinId;
				break;
			case SkinType.Back:
				this.AccessoryId = skinId;
				break;
			}
			this.MutualBySelf(skinType, elementById);
			this.MutualByOther(skinType);
		}

		private void MutualBySelf(SkinType skinType, Avatar_Skin table)
		{
			if (table.mutualParts != null)
			{
				for (int i = 0; i < table.mutualParts.Length; i++)
				{
					int num = table.mutualParts[i];
					if (num != (int)skinType)
					{
						if (num == 2)
						{
							this.HeadId = 0;
						}
						if (num == 1)
						{
							this.BodyId = 0;
						}
						else if (num == 3)
						{
							this.AccessoryId = 0;
						}
					}
				}
			}
		}

		private void MutualByOther(SkinType skinType)
		{
			if (skinType != SkinType.Body && this.m_BodyId > 0)
			{
				Avatar_Skin elementById = GameApp.Table.GetManager().GetAvatar_SkinModelInstance().GetElementById(this.m_BodyId);
				for (int i = 0; i < elementById.mutualParts.Length; i++)
				{
					if (elementById.mutualParts[i] == (int)skinType)
					{
						this.BodyId = 0;
						break;
					}
				}
			}
			if (skinType != SkinType.Head && this.m_HeadId > 0)
			{
				Avatar_Skin elementById2 = GameApp.Table.GetManager().GetAvatar_SkinModelInstance().GetElementById(this.m_HeadId);
				for (int j = 0; j < elementById2.mutualParts.Length; j++)
				{
					if (elementById2.mutualParts[j] == (int)skinType)
					{
						this.HeadId = 0;
						break;
					}
				}
			}
			if (skinType != SkinType.Back && this.m_AccessoryId > 0)
			{
				Avatar_Skin elementById3 = GameApp.Table.GetManager().GetAvatar_SkinModelInstance().GetElementById(this.m_AccessoryId);
				for (int k = 0; k < elementById3.mutualParts.Length; k++)
				{
					if (elementById3.mutualParts[k] == (int)skinType)
					{
						this.AccessoryId = 0;
						return;
					}
				}
			}
		}

		private int m_HeadId;

		private int m_BodyId;

		private int m_AccessoryId;

		private Dictionary<SkinType, SkinData> m_SkinDatas;

		private bool changed;
	}
}
