using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public abstract class UIBaseMainCityNode : CustomBehaviour
	{
		public abstract int FunctionOpenID { get; }

		public abstract MainCityName Name { get; }

		public abstract string RedName { get; }

		public abstract int NameLanguageID { get; }

		public abstract string NameLanguageIDStr { get; }

		protected override void OnInit()
		{
			this.m_onclickForUnLockButton = null;
			if (this.m_lockBt != null)
			{
				this.m_lockBt.onClick.AddListener(new UnityAction(this.InternalOnClickBt));
			}
			if (this.m_unlockBt != null)
			{
				this.m_unlockBt.onClick.AddListener(new UnityAction(this.InternalOnClickBt));
			}
			if (this.m_redPoint != null)
			{
				this.m_redPoint.Value = 0;
			}
		}

		protected override void OnDeInit()
		{
			this.m_onclickForUnLockButton = null;
			if (this.m_lockBt != null)
			{
				this.m_lockBt.onClick.RemoveListener(new UnityAction(this.InternalOnClickBt));
			}
			if (this.m_unlockBt != null)
			{
				this.m_unlockBt.onClick.RemoveListener(new UnityAction(this.InternalOnClickBt));
			}
		}

		public virtual void OnShow()
		{
			if (!string.IsNullOrEmpty(this.RedName))
			{
				RedPointController.Instance.RegRecordChange(this.RedName, new Action<RedNodeListenData>(this.OnRedPointChange));
			}
			this.OnRefreshFunctionOpenState();
			if (this.m_unlockObj != null && !this.m_isLock)
			{
				this.m_unlockObj.localScale = Vector3.zero;
				Sequence sequence = this.m_pool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, (float)this.Name * 0.1f);
				TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.m_unlockObj, Vector3.one, 0.3f), 27));
			}
			this.OnLanguageChange();
		}

		public virtual void OnHide()
		{
			this.m_pool.Clear(false);
			if (!string.IsNullOrEmpty(this.RedName))
			{
				RedPointController.Instance.UnRegRecordChange(this.RedName, new Action<RedNodeListenData>(this.OnRedPointChange));
			}
		}

		private void InternalOnClickBt()
		{
			Action onclickForUnLockButton = this.m_onclickForUnLockButton;
			if (onclickForUnLockButton == null)
			{
				return;
			}
			onclickForUnLockButton();
		}

		protected virtual void OnClickUnlockBt()
		{
			if (!string.IsNullOrEmpty(this.RedName))
			{
				RedPointController.Instance.ClickRecord(this.RedName);
			}
		}

		protected virtual void OnClickLockBt()
		{
			this.ShowLockTips();
		}

		public void OnRefreshFunctionOpenState()
		{
			this.m_isLock = this.IsLock();
			this.SetLock(this.m_isLock);
			if (!this.m_isLock && !string.IsNullOrEmpty(this.RedName))
			{
				RedPointController.Instance.ReCalc(this.RedName, true);
			}
		}

		public void SetLock(bool isLock)
		{
			if (isLock)
			{
				this.m_onclickForUnLockButton = new Action(this.OnClickLockBt);
			}
			else
			{
				this.m_onclickForUnLockButton = new Action(this.OnClickUnlockBt);
			}
			if (this.m_lockObj != null)
			{
				this.m_lockObj.gameObject.SetActive(false);
			}
			if (this.m_unlockObj != null)
			{
				this.m_unlockObj.gameObject.SetActive(true);
			}
			if (this.m_lockImage != null)
			{
				this.m_lockImage.SetActive(isLock);
			}
			if (!isLock)
			{
				this.OnLanguageChange();
			}
		}

		public bool IsLock()
		{
			return this.FunctionOpenID != 0 && !Singleton<GameFunctionController>.Instance.IsFunctionOpened(this.FunctionOpenID, false);
		}

		protected void ShowLockTips()
		{
			if (this.FunctionOpenID == 0)
			{
				return;
			}
			string lockTips = Singleton<GameFunctionController>.Instance.GetLockTips(this.FunctionOpenID);
			GameApp.View.ShowStringTip(lockTips);
		}

		protected virtual void OnRedPointChange(RedNodeListenData obj)
		{
			if (this.m_redPoint == null)
			{
				return;
			}
			this.m_redPoint.Value = obj.m_count;
		}

		public virtual void OnLanguageChange()
		{
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.SetText(Singleton<LanguageManager>.Instance.GetInfoByID(this.NameLanguageIDStr));
			}
		}

		public void GotoClick()
		{
			this.m_isLock = this.IsLock();
			if (this.m_isLock)
			{
				this.OnClickLockBt();
				return;
			}
			this.OnClickUnlockBt();
		}

		public RectTransform m_lockObj;

		public CustomButton m_lockBt;

		public RectTransform m_unlockObj;

		public CustomButton m_unlockBt;

		public UIBgText m_nameTxt;

		public RedNodeOneCtrl m_redPoint;

		public GameObject m_lockImage;

		protected bool m_isLock;

		private SequencePool m_pool = new SequencePool();

		private Action m_onclickForUnLockButton;
	}
}
