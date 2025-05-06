using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public abstract class UIBaseMainPageTabNode : CustomBehaviour
	{
		public int m_pageName { get; private set; }

		private protected string m_languageID { protected get; private set; }

		protected string m_redPointName { get; set; }

		private int m_functionID { get; set; }

		protected bool m_isPlaying { get; set; }

		private float m_time { get; set; }

		private float m_duration { get; set; } = 0.1f;

		public virtual float ElementMin
		{
			get
			{
				return 0f;
			}
		}

		public virtual float ElementMax
		{
			get
			{
				return 150f;
			}
		}

		public void SetPageName(int pageName)
		{
			this.m_pageName = pageName;
		}

		public void SetLanguageID(string languageID)
		{
			this.m_languageID = languageID;
		}

		public void SetRedPointName(string redPointName)
		{
			this.m_redPointName = redPointName;
		}

		public void SetFunctionID(int functionID)
		{
			this.m_functionID = functionID;
		}

		public int GetFunctionID()
		{
			return this.m_functionID;
		}

		protected override void OnInit()
		{
			if (this.m_redNode != null)
			{
				this.m_redNode.Value = 0;
			}
			if (this.m_btn != null)
			{
				this.m_btn.onClick.AddListener(new UnityAction(this.OnClickBt));
			}
			RedPointController.Instance.RegRecordChange(this.m_redPointName, new Action<RedNodeListenData>(this.OnRedPointChange));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			if (!this.m_isPlaying)
			{
				return;
			}
			this.m_time += Time.deltaTime;
			if (this.m_time >= this.m_duration)
			{
				this.m_time = this.m_duration;
				this.m_isPlaying = false;
			}
			float num = this.m_time / this.m_duration;
			this.m_layoutElement.minWidth = Mathf.Lerp(this.m_fromElement, this.m_toElement, num);
			this.PlayUpdate(!this.m_isPlaying, num);
		}

		protected override void OnDeInit()
		{
			RedPointController.Instance.UnRegRecordChange(this.m_redPointName, new Action<RedNodeListenData>(this.OnRedPointChange));
			if (this.m_btn != null)
			{
				this.m_btn.onClick.RemoveAllListeners();
			}
		}

		protected virtual void PlayUpdate(bool isFinished, float progress)
		{
		}

		protected virtual void PlayReClickAnimation(Vector3 toScale)
		{
			if (this.reClickAnimationTarget != null)
			{
				ShortcutExtensions.DOKill(this.reClickAnimationTarget, false);
				this.reClickAnimationTarget.localScale = toScale;
				ShortcutExtensions.DOPunchScale(this.reClickAnimationTarget, -0.1f * Vector3.one, 0.2f, 1, 0.5f);
			}
		}

		public abstract void OnLanguageChange();

		public virtual void OnSelect(bool isSelect, bool isLerp = false)
		{
			if (isSelect)
			{
				if (!isLerp)
				{
					this.m_layoutElement.minWidth = this.ElementMax;
				}
				else
				{
					this.m_fromElement = this.m_layoutElement.minWidth;
					this.m_toElement = this.ElementMax;
					this.m_time = 0f;
					this.m_isPlaying = true;
				}
			}
			else if (!isLerp)
			{
				this.m_layoutElement.minWidth = this.ElementMin;
			}
			else
			{
				this.m_fromElement = this.m_layoutElement.minWidth;
				this.m_toElement = this.ElementMin;
				this.m_time = 0f;
				this.m_isPlaying = true;
			}
			this.isSelected = isSelect;
		}

		public void SetLock(bool isLock)
		{
			if (this.m_lockNode == null)
			{
				return;
			}
			this.m_lockNode.gameObject.SetActive(isLock);
			this.m_unlockNode.gameObject.SetActiveSafe(!isLock);
		}

		protected void OnClickBt()
		{
			if (this.m_onClick == null)
			{
				return;
			}
			this.m_onClick(this, null);
			GameApp.Sound.PlayClip(622, 1f);
		}

		public bool IsLock()
		{
			return this.m_functionID != 0 && !Singleton<GameFunctionController>.Instance.IsFunctionOpened(this.m_functionID, false);
		}

		public abstract void OnRefreshFunctionOpenState();

		public virtual void OnSetTabIcon(string iconName)
		{
		}

		protected virtual void OnRedPointChange(RedNodeListenData obj)
		{
			if (this.m_pageName == 2)
			{
				this.SetRedValue(obj.m_count);
				return;
			}
			this.SetRedValue(obj.m_count);
		}

		protected void SetRedType(RedNodeType type)
		{
			if (this.m_redNode == null)
			{
				return;
			}
			this.m_redNode.SetType(type);
		}

		protected void SetRedValue(int value)
		{
			if (this.m_redNode == null)
			{
				return;
			}
			this.m_redNode.Value = value;
		}

		protected void SetRedShow(bool value)
		{
			if (this.m_redNode == null)
			{
				return;
			}
			this.m_redNode.Value = (value ? 1 : 0);
			this.m_redNode.gameObject.SetActive(value);
		}

		[SerializeField]
		private LayoutElement m_layoutElement;

		[Header("Default")]
		[SerializeField]
		protected RectTransform m_default;

		public Button m_btn;

		[SerializeField]
		private RectTransform m_unlockNode;

		[SerializeField]
		protected RectTransform m_iconParent;

		[SerializeField]
		protected CustomImage m_icon;

		[SerializeField]
		private RedNodeOneCtrl m_redNode;

		[SerializeField]
		private RectTransform m_lockNode;

		[SerializeField]
		protected CustomText m_pageNameTxt;

		[SerializeField]
		protected RectTransform m_bgUnselect;

		[SerializeField]
		protected RectTransform m_bgselect;

		[Header("Other")]
		[SerializeField]
		protected RectTransform m_other;

		public Action<UIBaseMainPageTabNode, object> m_onClick;

		private float m_fromElement;

		private float m_toElement;

		protected RectTransform reClickAnimationTarget;

		protected bool isSelected;
	}
}
