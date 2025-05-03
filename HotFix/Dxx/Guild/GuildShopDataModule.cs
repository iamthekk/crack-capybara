using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	[GuildInternalModule(16)]
	public class GuildShopDataModule : IGuildModule
	{
		public int ModuleName
		{
			get
			{
				return 16;
			}
		}

		public bool Init(GuildInitConfig config)
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.RegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
			@event.RegisterEvent(16, new GuildHandlerEvent(this.OnSetMyGuildFeaturesInfo));
			@event.RegisterEvent(19, new GuildHandlerEvent(this.OnGuildShopSetData));
			return true;
		}

		public void UnInit()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.UnRegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
			@event.UnRegisterEvent(16, new GuildHandlerEvent(this.OnSetMyGuildFeaturesInfo));
			@event.UnRegisterEvent(19, new GuildHandlerEvent(this.OnGuildShopSetData));
		}

		private void OnLoginSuccess(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_LoginSuccess guildEvent_LoginSuccess = eventArgs as GuildEvent_LoginSuccess;
			if (guildEvent_LoginSuccess != null)
			{
				if (guildEvent_LoginSuccess.IsJoin)
				{
					List<GuildShopGroup> shopList = guildEvent_LoginSuccess.ShopList;
					this.mShopDic.Clear();
					if (shopList != null)
					{
						for (int i = 0; i < shopList.Count; i++)
						{
							GuildShopGroup guildShopGroup = shopList[i];
							if (guildShopGroup != null)
							{
								this.mShopDic[guildShopGroup.GuildShopType] = guildShopGroup;
							}
						}
					}
					this.mShopList.Clear();
					this.mShopList.AddRange(this.mShopDic.Values);
					return;
				}
				this.mShopDic.Clear();
				this.mShopList.Clear();
			}
		}

		private void OnSetMyGuildFeaturesInfo(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_SetMyGuildFeaturesInfo guildEvent_SetMyGuildFeaturesInfo = eventArgs as GuildEvent_SetMyGuildFeaturesInfo;
			if (guildEvent_SetMyGuildFeaturesInfo != null)
			{
				List<GuildShopGroup> shopList = guildEvent_SetMyGuildFeaturesInfo.ShopList;
				this.mShopDic.Clear();
				if (shopList != null)
				{
					for (int i = 0; i < shopList.Count; i++)
					{
						GuildShopGroup guildShopGroup = shopList[i];
						if (guildShopGroup != null)
						{
							this.mShopDic[guildShopGroup.GuildShopType] = guildShopGroup;
						}
					}
				}
				this.mShopList.Clear();
				this.mShopList.AddRange(this.mShopDic.Values);
			}
		}

		private void OnGuildShopSetData(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GuildShopSetData guildEvent_GuildShopSetData = eventArgs as GuildEvent_GuildShopSetData;
			if (guildEvent_GuildShopSetData != null)
			{
				GuildShopGroup shopGroup = this.GetShopGroup(guildEvent_GuildShopSetData.ShopType);
				if (shopGroup != null)
				{
					shopGroup.UpdateGuildShopList(guildEvent_GuildShopSetData.ShopDataList);
				}
			}
		}

		public GuildShopGroup GetShopGroup(int shoptype)
		{
			GuildShopGroup guildShopGroup;
			if (this.mShopDic.TryGetValue(shoptype, out guildShopGroup))
			{
				return guildShopGroup;
			}
			return null;
		}

		public Dictionary<int, GuildShopGroup> GetShopGropDic()
		{
			return this.mShopDic;
		}

		public List<GuildShopData> GetShopDataList(int shoptype)
		{
			GuildShopGroup guildShopGroup;
			if (this.mShopDic.TryGetValue(shoptype, out guildShopGroup))
			{
				return guildShopGroup.ShopList;
			}
			return new List<GuildShopData>();
		}

		private Dictionary<int, GuildShopGroup> mShopDic = new Dictionary<int, GuildShopGroup>();

		private List<GuildShopGroup> mShopList = new List<GuildShopGroup>();
	}
}
