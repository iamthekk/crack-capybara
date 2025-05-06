using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class RateViewModule_HeartItem : MonoBehaviour
	{
		public void Init(int index, Action<int> onclickCall)
		{
			this._index = index;
			this._onclickCall = onclickCall;
			if (this.Button_Heart)
			{
				this.Button_Heart.onClick.AddListener(new UnityAction(this.OnClick));
			}
		}

		public void Clear()
		{
			if (this.Button_Heart)
			{
				this.Button_Heart.onClick.RemoveAllListeners();
			}
			this._onclickCall = null;
		}

		public void SetShow(bool isShow)
		{
			this.Image_Heart.gameObject.SetActive(isShow);
		}

		private void OnClick()
		{
			Action<int> onclickCall = this._onclickCall;
			if (onclickCall == null)
			{
				return;
			}
			onclickCall(this._index);
		}

		public Image Image_Heart;

		public Button Button_Heart;

		private int _index;

		private Action<int> _onclickCall;
	}
}
