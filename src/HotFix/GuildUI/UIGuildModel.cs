using System;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildModel
	{
		public void SetGameObject(GameObject obj)
		{
			this.gameObject = obj;
		}

		public void OnInitUI()
		{
		}

		public void OnUnInitUI()
		{
		}

		public void SetCurrencyType(int id)
		{
		}

		public void SetValue(int count)
		{
		}

		public void Refresh(int modelid, GuildProxy.AnimationType show)
		{
		}

		public GameObject gameObject;
	}
}
