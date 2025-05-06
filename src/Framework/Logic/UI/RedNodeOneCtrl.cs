using System;
using UnityEngine;

namespace Framework.Logic.UI
{
	public class RedNodeOneCtrl : MonoBehaviour
	{
		public bool IsShowing
		{
			get
			{
				return this.count > 0;
			}
		}

		public int Value
		{
			set
			{
				this.count = Mathf.Min(value, 99);
				base.gameObject.SetActive(this.count > 0);
				if (this.count > 0)
				{
					this.SetText(this.count.ToString());
					return;
				}
				this.SetText("");
			}
		}

		private void Awake()
		{
			if (this.imageRedNodeEquip != null)
			{
				this.imageRedNodeEquip.enabled = false;
			}
			this.SetType(this.mType);
			Singleton<RedNodeManager>.Instance.Add(this);
		}

		private void OnDestroy()
		{
			Singleton<RedNodeManager>.Instance.Remove(this);
		}

		public string Key
		{
			get
			{
				return this.m_key;
			}
		}

		public void SetText(string value)
		{
			if (this.CanSetText && this.text_count != null)
			{
				this.text_count.text = value;
			}
		}

		private bool CanSetText
		{
			get
			{
				return this.mType == RedNodeType.eRedCount;
			}
		}

		private void SetAniEnable(bool value)
		{
			this.ani.enabled = value;
			if (!value)
			{
				this.child.anchoredPosition = Vector2.zero;
			}
		}

		public void SetType(RedNodeType type)
		{
			this.mType = type;
			if (type <= RedNodeType.eRedWear)
			{
				if (type == RedNodeType.eRedCount)
				{
					this.imageRedNodeCommon.enabled = true;
					this.imageRedNodeEquip.enabled = false;
					this.imageRedNodeEmptyIcon.enabled = false;
					this.text_count.gameObject.SetActive(true);
					this.text_count.enabled = true;
					this.SetAniEnable(true);
					return;
				}
				if (type == RedNodeType.eRedEmpty)
				{
					this.imageRedNodeCommon.enabled = true;
					this.imageRedNodeEquip.enabled = false;
					this.imageRedNodeEmptyIcon.enabled = true;
					this.text_count.enabled = false;
					this.SetAniEnable(true);
					return;
				}
				if (type != RedNodeType.eRedWear)
				{
					return;
				}
				this.imageRedNodeCommon.enabled = false;
				this.imageRedNodeEquip.enabled = true;
				this.imageRedNodeEmptyIcon.enabled = true;
				this.text_count.enabled = false;
				this.SetAniEnable(true);
				return;
			}
			else
			{
				if (type == RedNodeType.eRedNew)
				{
					this.imageRedNodeCommon.enabled = true;
					this.imageRedNodeEquip.enabled = false;
					this.imageRedNodeEmptyIcon.enabled = false;
					this.text_count.enabled = true;
					this.SetAniEnable(true);
					this.text_count.text = "New";
					return;
				}
				if (type == RedNodeType.eRedUp)
				{
					this.imageRedNodeCommon.enabled = true;
					this.imageRedNodeEquip.enabled = false;
					this.imageRedNodeEmptyIcon.enabled = true;
					this.text_count.enabled = false;
					this.SetAniEnable(true);
					return;
				}
				if (type != RedNodeType.eWarning)
				{
					return;
				}
				this.imageRedNodeCommon.enabled = true;
				this.imageRedNodeEquip.enabled = false;
				this.imageRedNodeEmptyIcon.enabled = true;
				this.text_count.enabled = false;
				this.SetAniEnable(true);
				return;
			}
		}

		public void OnRefresh(RedNodeListenData data)
		{
			if (data == null)
			{
				return;
			}
			base.gameObject.SetActive(data.m_count > 0);
			RedNodeType redNodeType = this.mType;
			if (redNodeType <= RedNodeType.eRedEmpty)
			{
				if (redNodeType == RedNodeType.eInvalid)
				{
					return;
				}
				if (redNodeType == RedNodeType.eRedCount)
				{
					this.Value = data.m_count;
					return;
				}
				if (redNodeType == RedNodeType.eRedEmpty)
				{
					return;
				}
			}
			else if (redNodeType <= RedNodeType.eRedNew)
			{
				if (redNodeType == RedNodeType.eRedWear || redNodeType == RedNodeType.eRedNew)
				{
					return;
				}
			}
			else if (redNodeType == RedNodeType.eRedUp || redNodeType == RedNodeType.eWarning)
			{
				return;
			}
			throw new ArgumentOutOfRangeException();
		}

		[Header("Key")]
		[SerializeField]
		private string m_key = string.Empty;

		[Header("Setting")]
		public RectTransform child;

		public CustomText text_count;

		public CustomImage imageRedNodeCommon;

		public CustomImage imageRedNodeEquip;

		public CustomImage imageRedNodeEmptyIcon;

		public Animator ani;

		public int count;

		public RedNodeType mType;

		private static readonly string Atlas_Common = "Assets/_Resources/AtlasRaft/CommonHot/CommonHot.spriteatlas";
	}
}
