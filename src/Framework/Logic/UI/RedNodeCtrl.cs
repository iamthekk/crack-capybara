using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Framework.Logic.UI
{
	public class RedNodeCtrl
	{
		public int Value
		{
			set
			{
				if (value > 0)
				{
					this.Show(true);
					this._value = value;
					this.loadRed(delegate
					{
						if (this._redctrl != null)
						{
							this._redctrl.Value = value;
						}
					});
					return;
				}
				this.Show(false);
			}
		}

		public void SetData(GameObject obj)
		{
			this.m_gameObject = obj;
		}

		public void Init()
		{
		}

		public void DeInit()
		{
		}

		public void Show(bool value)
		{
			if (this.m_gameObject)
			{
				this.m_gameObject.SetActive(value);
			}
		}

		public void DestroyChild()
		{
			if (this._redctrl != null)
			{
				Object.DestroyImmediate(this._redctrl.gameObject);
			}
		}

		private void SetText(string value)
		{
			this._text = value;
			this.loadRed(delegate
			{
				if (this._redctrl != null)
				{
					this._redctrl.SetText(value);
				}
			});
		}

		public void SetType(RedNodeType type)
		{
			this._type = type;
			this.loadRed(delegate
			{
				if (this._redctrl != null)
				{
					this._redctrl.SetType(type);
				}
			});
		}

		private void loadRed(Action onFinish)
		{
			this.onLoadFinish = onFinish;
			if (this._redctrl != null)
			{
				Action action = this.onLoadFinish;
				if (action == null)
				{
					return;
				}
				action();
				return;
			}
			else if (this.m_gameObject.transform.childCount > 0)
			{
				this._redctrl = this.m_gameObject.transform.GetChild(0).GetComponent<RedNodeOneCtrl>();
				Action action2 = this.onLoadFinish;
				if (action2 == null)
				{
					return;
				}
				action2();
				return;
			}
			else
			{
				if (this._isLoadingRed)
				{
					return;
				}
				this._isLoadingRed = true;
				GameApp.Resources.LoadAssetAsync<GameObject>(RedNodeCtrl.RedNodeOne).Completed += delegate(AsyncOperationHandle<GameObject> x)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(x.Result);
					this._redctrl = gameObject.GetComponent<RedNodeOneCtrl>();
					RedNodeCtrl.SetParentNormalInternal(this._redctrl.transform, this.m_gameObject.transform);
					if (!this.m_isInit && this._redctrl)
					{
						this.m_isInit = true;
						this._redctrl.Value = this._value;
						this._redctrl.SetText(this._text);
						this._redctrl.SetType(this._type);
					}
					Action action3 = this.onLoadFinish;
					if (action3 == null)
					{
						return;
					}
					action3();
				};
				return;
			}
		}

		private static void SetParentNormalInternal(Transform child, Transform parent)
		{
			if (child == null || parent == null)
			{
				return;
			}
			child.SetParent(parent, false);
			RectTransform rectTransform = child as RectTransform;
			child.localPosition = Vector3.zero;
			child.localScale = Vector3.one;
			child.localEulerAngles = Vector3.zero;
			if (rectTransform)
			{
				rectTransform.anchoredPosition = Vector2.zero;
			}
		}

		public static readonly string RedNodeOne = "Assets/_Resources/Prefab/UI/Common/RedNodeOne.prefab";

		private GameObject m_gameObject;

		private RedNodeType _type;

		public RedNodeOneCtrl _redctrl;

		private bool _isLoadingRed;

		private bool m_isInit;

		private Action onLoadFinish;

		private int _value;

		private string _text;
	}
}
