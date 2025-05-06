using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class ArtifactAdvanceData
	{
		public int ID { get; private set; }

		public int Star { get; private set; }

		public bool IsUnlock { get; private set; }

		public Artifact_advanceArtifact Config { get; private set; }

		public Item_Item ItemConfig { get; private set; }

		public MergeAttributeData InitAttributeData { get; private set; }

		public List<PropData> UpStarCostList { get; private set; }

		public bool IsMaxStar
		{
			get
			{
				return this.Star == this.Config.maxStar;
			}
		}

		public string ArtifactName
		{
			get
			{
				if (this.ItemConfig != null)
				{
					return Singleton<LanguageManager>.Instance.GetInfoByID(this.ItemConfig.nameID);
				}
				return "";
			}
		}

		public ArtifactAdvanceData(Artifact_advanceArtifact config)
		{
			this.Config = config;
			this.ID = config.id;
			this.IsUnlock = false;
			this.ItemConfig = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(config.itemId);
			List<MergeAttributeData> mergeAttributeData = config.attribute.GetMergeAttributeData();
			if (mergeAttributeData.Count > 0)
			{
				this.InitAttributeData = mergeAttributeData[0];
			}
			this.UpStarCostList = new List<PropData>();
			for (int i = 0; i < config.starCost.Length; i++)
			{
				List<int> listInt = config.starCost[i].GetListInt(',');
				if (listInt.Count >= 2)
				{
					PropData propData = new PropData();
					propData.id = (uint)listInt[0];
					propData.count = (ulong)listInt[1];
					this.UpStarCostList.Add(propData);
				}
			}
		}

		public void SetStar(int star)
		{
			this.Star = star;
		}

		public void SetUnLock()
		{
			this.IsUnlock = true;
		}

		public GameSkill_skill GetSkill()
		{
			GameSkill_skill gameSkill_skill;
			if (this.Star == this.Config.maxStar)
			{
				gameSkill_skill = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(this.Config.maxStarSkill);
			}
			else
			{
				gameSkill_skill = GameApp.Table.GetManager().GetGameSkill_skillModelInstance().GetElementById(this.Config.initSkill);
			}
			return gameSkill_skill;
		}

		public MergeAttributeData GetCurrentAttribute()
		{
			long currentAttributeValue = this.GetCurrentAttributeValue();
			return new MergeAttributeData(this.InitAttributeData.Header + "=" + currentAttributeValue.ToString(), null, null);
		}

		public long GetCurrentAttributeValue()
		{
			if (!this.IsUnlock)
			{
				return 0L;
			}
			long num = this.InitAttributeData.Value.GetValue();
			for (int i = 0; i < this.Config.levelAttribute.Length; i++)
			{
				if (i < this.Star)
				{
					num += (long)this.Config.levelAttribute[i];
				}
			}
			return num;
		}

		public long GetNextAttributeValue()
		{
			long num = this.InitAttributeData.Value.GetValue();
			if (!this.IsUnlock)
			{
				return num;
			}
			for (int i = 0; i < this.Config.levelAttribute.Length; i++)
			{
				if (i < this.Star + 1)
				{
					num += (long)this.Config.levelAttribute[i];
				}
			}
			return num;
		}

		public PropData GetNextStarCost()
		{
			if (this.Star < this.UpStarCostList.Count)
			{
				return this.UpStarCostList[this.Star];
			}
			return null;
		}

		public bool IsRedPoint()
		{
			if (this.IsMaxStar)
			{
				return false;
			}
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			if (this.IsUnlock)
			{
				PropData nextStarCost = this.GetNextStarCost();
				if (nextStarCost == null)
				{
					return false;
				}
				if (dataModule.GetItemDataCountByid((ulong)nextStarCost.id) >= (long)nextStarCost.count)
				{
					return true;
				}
			}
			else if (dataModule.GetItemDataCountByid((ulong)this.Config.unlockCostId) > 0L)
			{
				return true;
			}
			return false;
		}

		public bool IsUnlockEnabled()
		{
			return !this.IsMaxStar && !this.IsUnlock && GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)this.Config.unlockCostId) > 0L;
		}
	}
}
