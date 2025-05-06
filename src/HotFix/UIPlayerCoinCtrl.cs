using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIPlayerCoinCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			if (this.items != null)
			{
				for (int i = 0; i < this.items.Length; i++)
				{
					if (!(this.items[i] == null))
					{
						this.items[i].Init();
					}
				}
			}
		}

		protected override void OnDeInit()
		{
			if (this.items != null)
			{
				for (int i = 0; i < this.items.Length; i++)
				{
					if (!(this.items[i] == null))
					{
						this.items[i].DeInit();
					}
				}
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.items != null)
			{
				for (int i = 0; i < this.items.Length; i++)
				{
					if (!(this.items[i] == null))
					{
						this.items[i].OnUpdate(deltaTime, unscaledDeltaTime);
					}
				}
			}
		}

		public void SetCoin(CurrencyType type, long coin, float duration)
		{
			if (this.items != null)
			{
				for (int i = 0; i < this.items.Length; i++)
				{
					if (!(this.items[i] == null) && this.items[i].currencyType == type)
					{
						this.items[i].SetCoin(coin, duration);
					}
				}
			}
		}

		public void Show(CurrencyType type, Action callback = null)
		{
			if (this.items != null)
			{
				for (int i = 0; i < this.items.Length; i++)
				{
					if (!(this.items[i] == null) && this.items[i].currencyType == type)
					{
						this.items[i].Show(callback);
					}
				}
			}
		}

		public void Hide(CurrencyType type)
		{
			if (this.items != null)
			{
				for (int i = 0; i < this.items.Length; i++)
				{
					if (!(this.items[i] == null) && this.items[i].currencyType == type)
					{
						this.items[i].Hide();
					}
				}
			}
		}

		public Transform GetFlyNode(CurrencyType type)
		{
			if (this.items != null)
			{
				for (int i = 0; i < this.items.Length; i++)
				{
					if (!(this.items[i] == null) && this.items[i].currencyType == type)
					{
						return this.items[i].flyNode.transform;
					}
				}
			}
			return base.transform;
		}

		public UICoinNodeItem[] items;
	}
}
