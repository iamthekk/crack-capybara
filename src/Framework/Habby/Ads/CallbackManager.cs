using System;

namespace Habby.Ads
{
	internal interface CallbackManager
	{
		void AddCallback(AdsCallback callback);

		void RemoveCallback(AdsCallback callback);
	}
}
