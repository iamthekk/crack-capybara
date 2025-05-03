using System;
using System.Collections.Generic;
using Framework;
using Framework.SDKManager;

namespace Habby.Ads
{
	public class GameAdsConfigImpl : GameAdsConfigInterface
	{
		public void track(string eventName, Dictionary<string, object> properties)
		{
			SDKManager sdk = GameApp.SDK;
			if (sdk == null)
			{
				return;
			}
			sdk.Analyze.Track(eventName, properties, true);
		}

		public AdsRequestHelper.AdsConfiguration GetDebugConfig()
		{
			return new AdsRequestHelper.AdsConfiguration
			{
				admobAppId = "ca-app-pub-9185802584272702~2104968631",
				rewardedSlots = new AdsRequestHelper.AdsSlot[]
				{
					new AdsRequestHelper.AdsSlot
					{
						code = 'N',
						mediation = AdsRequestHelper.AdsMediationType.ADMOB,
						slotId = "ca-app-pub-3940256099942544/5224354917"
					}
				},
				rewarded = new AdsRequestHelper.AdsPlacement[]
				{
					new AdsRequestHelper.AdsPlacement
					{
						name = "Admob",
						slots = "N"
					}
				}
			};
		}

		public AdsRequestHelper.AdsConfiguration GetProductionConfig()
		{
			return new AdsRequestHelper.AdsConfiguration
			{
				rewardedSlots = new AdsRequestHelper.AdsSlot[]
				{
					new AdsRequestHelper.AdsSlot
					{
						code = 'H',
						mediation = AdsRequestHelper.AdsMediationType.ALMAX,
						slotId = "8cf90d6d5602890f"
					},
					new AdsRequestHelper.AdsSlot
					{
						code = 'J',
						mediation = AdsRequestHelper.AdsMediationType.ALMAX,
						slotId = "0c49c3ed6098927a"
					},
					new AdsRequestHelper.AdsSlot
					{
						code = 'K',
						mediation = AdsRequestHelper.AdsMediationType.ALMAX,
						slotId = "cd1a90321a5dbf4c"
					},
					new AdsRequestHelper.AdsSlot
					{
						code = 'M',
						mediation = AdsRequestHelper.AdsMediationType.ALMAX,
						slotId = "bc4652c76f95b847"
					},
					new AdsRequestHelper.AdsSlot
					{
						code = 'N',
						mediation = AdsRequestHelper.AdsMediationType.ALMAX,
						slotId = "dfc97650e8b6abfd"
					}
				},
				rewarded = new AdsRequestHelper.AdsPlacement[]
				{
					new AdsRequestHelper.AdsPlacement
					{
						name = "ALMAX",
						slots = "H,J,K,M,N"
					}
				}
			};
		}

		public List<string> GetTestDeviceId()
		{
			return new List<string> { "SIMULATOR", "A6B17CA8-00B3-4CF7-B5DE-0D71F2D5C338", "e7753875-8abc-4e73-9ae1-b88f63f8e7a9" };
		}

		public bool isTest()
		{
			return false;
		}
	}
}
