using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public class PushGiftData
	{
		public int ConfigId { get; private set; }

		public List<ItemData> Items { get; private set; }

		public long EndTime { get; private set; }

		public long StartTime { get; private set; }

		public IAP_pushIap PushConfig { get; private set; }

		public IAP_Purchase PurchaseConfig { get; private set; }

		public PushGiftPushType PushType { get; private set; }

		public PushGiftPosType PosType { get; private set; }

		public string PrefabType { get; private set; }

		public bool Init(IAP_pushIap config)
		{
			this.PushConfig = config;
			this.PurchaseConfig = GameApp.Table.GetManager().GetIAP_Purchase(config.id);
			if (this.PushConfig == null || this.PurchaseConfig == null)
			{
				return false;
			}
			this.ConfigId = config.id;
			this.Items = new List<ItemData>();
			foreach (string text in this.PushConfig.products)
			{
				ItemData itemData = new ItemData();
				string[] array = text.Split(',', StringSplitOptions.None);
				itemData.SetCount(Convert.ToInt64(array[1]));
				itemData.SetID(Convert.ToUInt32(array[0]));
				this.Items.Add(itemData);
			}
			this.PushType = (PushGiftPushType)this.PushConfig.type;
			this.PosType = (PushGiftPosType)this.PushConfig.PosType;
			this.PrefabType = this.PushConfig.PrefabType;
			return true;
		}

		public bool Init(PushIapItemDto pushIapDto)
		{
			this.PushConfig = GameApp.Table.GetManager().GetIAP_pushIap(pushIapDto.ConfigId);
			this.PurchaseConfig = GameApp.Table.GetManager().GetIAP_Purchase(pushIapDto.ConfigId);
			if (this.PushConfig == null || this.PurchaseConfig == null)
			{
				return false;
			}
			this.ConfigId = pushIapDto.ConfigId;
			this.Items = new List<ItemData>();
			foreach (string text in this.PushConfig.products)
			{
				ItemData itemData = new ItemData();
				string[] array = text.Split(',', StringSplitOptions.None);
				itemData.SetCount(Convert.ToInt64(array[1]));
				itemData.SetID(Convert.ToUInt32(array[0]));
				this.Items.Add(itemData);
			}
			if (pushIapDto.ItemId > 0)
			{
				ItemData itemData2 = new ItemData();
				itemData2.SetCount(1L);
				itemData2.SetID(pushIapDto.ItemId);
				this.Items.Add(itemData2);
			}
			this.StartTime = pushIapDto.ETime - (long)this.PushConfig.lastSeconds;
			this.EndTime = pushIapDto.ETime;
			this.PushType = (PushGiftPushType)this.PushConfig.type;
			this.PosType = (PushGiftPosType)this.PushConfig.PosType;
			this.PrefabType = this.PushConfig.PrefabType;
			return true;
		}

		public bool IsPop;
	}
}
