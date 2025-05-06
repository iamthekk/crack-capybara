using System;
using DG.Tweening;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public abstract class UIDailyActivitiesNodeBase : CustomBehaviour
	{
		public float SizeY
		{
			get
			{
				return base.rectTransform.sizeDelta.y;
			}
		}

		public abstract FunctionID FunctionOpenID { get; }

		public abstract UIDailyActivitiesType DailyType { get; }

		public virtual bool CanShow()
		{
			return true;
		}

		public void Show()
		{
			this.OnShow();
			this.RefreshFuncionOpen();
		}

		public void Hide()
		{
			this.OnHide();
		}

		protected abstract void OnShow();

		protected abstract void OnHide();

		public void SetAnchorPosY(float y)
		{
			this.mPosY = y;
			base.rectTransform.anchoredPosition = new Vector2(base.rectTransform.anchoredPosition.x, this.mPosY);
		}

		public void PlayShow(SequencePool m_seqPool, int index)
		{
			base.SetActive(true);
			Sequence sequence = m_seqPool.Get();
			CanvasGroup component = base.gameObject.GetComponent<CanvasGroup>();
			component.alpha = 0f;
			TweenSettingsExtensions.AppendInterval(sequence, (float)index * 0.03f);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(component, 1f, 0.2f));
			base.rectTransform.anchoredPosition = new Vector2(base.rectTransform.anchoredPosition.x, this.mPosY - 300f);
			Sequence sequence2 = m_seqPool.Get();
			TweenSettingsExtensions.AppendInterval(sequence2, (float)index * 0.03f);
			TweenSettingsExtensions.Append(sequence2, ShortcutExtensions46.DOAnchorPosY(base.rectTransform, this.mPosY, 0.2f, false));
		}

		public void RefreshFuncionOpen()
		{
			this.mIsLock = this.FunctionOpenID != FunctionID.None && !Singleton<GameFunctionController>.Instance.IsFunctionOpened(this.FunctionOpenID, false);
			if (this.mIsLock)
			{
				if (this.ObjLock != null)
				{
					GameObject objLock = this.ObjLock;
					if (objLock != null)
					{
						objLock.SetActive(true);
					}
				}
				if (this.TextLock != null)
				{
					this.TextLock.text = Singleton<GameFunctionController>.Instance.GetLockTips((int)this.FunctionOpenID);
				}
				if (this.lockBgRT != null)
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate(this.lockBgRT);
					return;
				}
			}
			else if (this.ObjLock != null)
			{
				this.ObjLock.SetActive(false);
			}
		}

		protected void OnRedPointChange(RedNodeListenData redData)
		{
			if (this.redNode == null)
			{
				return;
			}
			this.redNode.gameObject.SetActive(redData.m_count > 0);
		}

		public bool IsFunctionOpen()
		{
			return Singleton<GameFunctionController>.Instance.IsFunctionOpened(this.FunctionOpenID, false);
		}

		public GameObject ObjLock;

		public RectTransform lockBgRT;

		public CustomText TextLock;

		public RedNodeOneCtrl redNode;

		protected bool mIsLock;

		private float mPosY;
	}
}
