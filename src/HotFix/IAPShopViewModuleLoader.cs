using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.U2D;

namespace HotFix
{
	public class IAPShopViewModuleLoader : BaseViewModuleLoader
	{
		private LoadPool<GameObject> ShopPanel { get; } = new LoadPool<GameObject>();

		public override async Task OnLoad(object data)
		{
			List<Task> list = new List<Task>();
			this.m_atlasList = new LoadPool<SpriteAtlas>();
			list.Add(this.m_atlasList.Load(GameApp.Table.GetAtlasPath(119)));
			foreach (KeyValuePair<IAPShopType, string> keyValuePair in this.iapShopPanelPathDic)
			{
				list.Add(this.ShopPanel.Load(keyValuePair.Value));
			}
			foreach (KeyValuePair<IAPDiamondsType, string> keyValuePair2 in this.iapDiamondsSubPanelPathDic)
			{
				list.Add(this.ShopPanel.Load(keyValuePair2.Value));
			}
			await Task.WhenAll(list);
		}

		public override void OnUnLoad()
		{
			if (this.m_atlasList != null)
			{
				this.m_atlasList.UnLoadAll();
				this.m_atlasList = null;
			}
			this.ShopPanel.UnLoadAll();
		}

		public bool GetIAPShopPanel(IAPShopType shopType, out IAPShopPanelBase panelPrefab)
		{
			panelPrefab = null;
			string text;
			if (!this.iapShopPanelPathDic.TryGetValue(shopType, out text))
			{
				return false;
			}
			GameObject gameObject = this.ShopPanel.Get(text);
			if (gameObject != null)
			{
				panelPrefab = gameObject.GetComponent<IAPShopPanelBase>();
			}
			return panelPrefab != null;
		}

		public bool GetIAPDiamondsSubPanel(IAPDiamondsType shopType, out IAPDiamondsSubPanelBase panelPrefab)
		{
			panelPrefab = null;
			string text;
			if (!this.iapDiamondsSubPanelPathDic.TryGetValue(shopType, out text))
			{
				return false;
			}
			GameObject gameObject = this.ShopPanel.Get(text);
			if (gameObject != null)
			{
				panelPrefab = gameObject.GetComponent<IAPDiamondsSubPanelBase>();
			}
			return panelPrefab != null;
		}

		public LoadPool<SpriteAtlas> m_atlasList;

		private const string IAPShopPath = "Assets/_Resources/Prefab/UI/IAPShop";

		private readonly Dictionary<IAPShopType, string> iapShopPanelPathDic = new Dictionary<IAPShopType, string>
		{
			{
				IAPShopType.Main,
				"Assets/_Resources/Prefab/UI/IAPShop/IAPMain/IAPMainPanel.prefab"
			},
			{
				IAPShopType.Diamonds,
				"Assets/_Resources/Prefab/UI/IAPShop/IAPDiamonds/IAPDiamondsPanel.prefab"
			},
			{
				IAPShopType.MonthCard,
				"Assets/_Resources/Prefab/UI/IAPShop/IAPMonthCard/IAPMonthCardPanel.prefab"
			},
			{
				IAPShopType.BattlePass,
				"Assets/_Resources/Prefab/UI/IAPShop/IAPBattlePass/IAPBattlePassPanel.prefab"
			},
			{
				IAPShopType.GrowthFund,
				"Assets/_Resources/Prefab/UI/IAPShop/IAPLevelFund/IAPLevelFundPanel.prefab"
			}
		};

		private const string IAPDiamondsPath = "Assets/_Resources/Prefab/UI/IAPShop/IAPDiamonds";

		private readonly Dictionary<IAPDiamondsType, string> iapDiamondsSubPanelPathDic = new Dictionary<IAPDiamondsType, string>
		{
			{
				IAPDiamondsType.DiamondsPack,
				"Assets/_Resources/Prefab/UI/IAPShop/IAPDiamonds/DiamondsPack/IAPDiamondsPackPanel.prefab"
			},
			{
				IAPDiamondsType.DayPack,
				"Assets/_Resources/Prefab/UI/IAPShop/IAPDiamonds/TimePack/IAPDayPackPanel.prefab"
			},
			{
				IAPDiamondsType.WeeklyPack,
				"Assets/_Resources/Prefab/UI/IAPShop/IAPDiamonds/TimePack/IAPWeeklyPackPanel.prefab"
			},
			{
				IAPDiamondsType.MonthlyPack,
				"Assets/_Resources/Prefab/UI/IAPShop/IAPDiamonds/TimePack/IAPMonthlyPackPanel.prefab"
			}
		};
	}
}
