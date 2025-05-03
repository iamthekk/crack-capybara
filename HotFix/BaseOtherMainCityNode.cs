using System;
using DG.Tweening;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.User;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public abstract class BaseOtherMainCityNode : CustomBehaviour
	{
		public void SetData(MainCityName name, int nameLanguageID)
		{
			this.m_name = name;
			this.m_nameLanguageID = nameLanguageID;
		}

		public void SetCityInfoResponse(UserGetCityInfoResponse data)
		{
			this.m_responseData = data;
		}

		protected override void OnInit()
		{
			if (this.m_lockBt != null)
			{
				this.m_lockBt.onClick.AddListener(new UnityAction(this.OnClickLockBt));
			}
			if (this.m_unlockBt != null)
			{
				this.m_unlockBt.onClick.AddListener(new UnityAction(this.OnClickUnlockBt));
			}
		}

		public virtual void OnShow()
		{
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.SetText(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(this.m_nameLanguageID));
			}
			if (this.m_unlockObj != null && this.m_isUnLock)
			{
				this.m_unlockObj.localScale = Vector3.zero;
				Sequence sequence = this.m_pool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, (float)this.m_name * 0.1f);
				TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.m_unlockObj, Vector3.one, 0.3f), 27));
			}
		}

		public virtual void OnHide()
		{
			this.m_responseData = null;
		}

		protected override void OnDeInit()
		{
			this.m_pool.Clear(false);
			if (this.m_lockBt != null)
			{
				this.m_lockBt.onClick.RemoveAllListeners();
			}
			if (this.m_unlockBt != null)
			{
				this.m_unlockBt.onClick.RemoveAllListeners();
			}
		}

		protected abstract void OnClickUnlockBt();

		protected abstract void OnClickLockBt();

		public void SetLock(bool isLock)
		{
			this.m_isUnLock = !isLock;
			if (this.m_lockObj != null)
			{
				this.m_lockObj.gameObject.SetActive(isLock);
			}
			if (this.m_unlockObj != null)
			{
				this.m_unlockObj.gameObject.SetActive(!isLock);
			}
		}

		public bool m_isUnLock;

		public MainCityName m_name = MainCityName.Box;

		public int m_nameLanguageID;

		public RectTransform m_lockObj;

		public CustomButton m_lockBt;

		public RectTransform m_unlockObj;

		public CustomButton m_unlockBt;

		public UIBgText m_nameTxt;

		private SequencePool m_pool = new SequencePool();

		protected UserGetCityInfoResponse m_responseData;
	}
}
