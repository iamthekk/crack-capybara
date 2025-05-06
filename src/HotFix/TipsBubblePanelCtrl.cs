using System;
using System.Collections;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class TipsBubblePanelCtrl : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.CloseButton.onClick.AddListener(new UnityAction(this.OnCloseButtonClicked));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.CloseButton.onClick.RemoveListener(new UnityAction(this.OnCloseButtonClicked));
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnCloseButtonClicked()
		{
			GameApp.View.CloseView(ViewName.TipsBubbleViewModule, null);
		}

		private void Show(Vector3 pos)
		{
			base.gameObject.SetActive(true);
			base.transform.position = pos;
			float num = 0f;
			if (this.Left.position.x < 0f)
			{
				num = -this.Left.position.x;
			}
			else if (this.Right.position.x > 1080f)
			{
				num = 1080f - this.Right.position.x;
			}
			this.Root.position = new Vector3(pos.x + num, pos.y, 0f);
			this.ImageArrow.position = new Vector3(pos.x, this.ImageArrow.position.y, 0f);
		}

		public void Show(RectTransform rect, string title, string content)
		{
			base.gameObject.SetActive(true);
			this._rect = rect;
			this.TextName.text = title;
			this.TextInfo.text = content;
			base.StartCoroutine(this.endFrameInit());
		}

		private IEnumerator endFrameInit()
		{
			this.Root.gameObject.SetActive(false);
			yield return new WaitForEndOfFrame();
			this.Root.gameObject.SetActive(true);
			Vector3 vector = Vector3.zero;
			if (this._rect != null)
			{
				Vector2 vector2 = new Vector2(0.5f, 1f) - this._rect.pivot;
				vector = this._rect.position;
				vector.x += this._rect.sizeDelta.x * vector2.x * this._rect.localScale.x;
				vector.y += this._rect.sizeDelta.y * vector2.y * this._rect.localScale.y;
			}
			this.Show(vector);
			yield break;
		}

		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		[SerializeField]
		private RectTransform Root;

		[SerializeField]
		private CustomText TextName;

		[SerializeField]
		private CustomText TextInfo;

		[SerializeField]
		private RectTransform ImageArrow;

		[SerializeField]
		private RectTransform Left;

		[SerializeField]
		private RectTransform Right;

		[SerializeField]
		private CustomButton CloseButton;

		private RectTransform _rect;
	}
}
