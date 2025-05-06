using System;
using System.Collections.Generic;

namespace HotFix
{
	[Serializable]
	public class JsonShopSummon
	{
		public int adId { get; set; }

		public int boxId { get; set; }

		public List<string> first { get; set; } = new List<string>();

		public int freeTimes { get; set; }

		public int hardPityCount { get; set; }

		public int hardPityPool { get; set; }

		public List<List<int>> hardPityRate { get; set; } = new List<List<int>>();

		public int id { get; set; }

		public int miniPityCount { get; set; }

		public int miniPityPool { get; set; }

		public List<List<int>> miniPityRate { get; set; } = new List<List<int>>();

		public List<List<int>> normalRate { get; set; } = new List<List<int>>();

		public int groupId { get; set; }

		public int orderId { get; set; }

		public int priceId { get; set; }

		public int quickDraw { get; set; }

		public int reverseCount { get; set; }

		public int reversePool { get; set; }

		public List<List<int>> reverseRate { get; set; } = new List<List<int>>();

		public int singlePrice { get; set; }

		public int singlePriceOrigin { get; set; }

		public int tenPrice { get; set; }

		public int tenPriceOrigin { get; set; }

		public int upEquipID { get; set; }
	}
}
