using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class InfoTipViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			this.m_data = data as InfoTipViewModule.InfoTipData;
			this.m_offset = this.m_data.m_offsetY;
			this.m_offsetX = this.m_data.m_offsetX;
			this.Button.onClick.AddListener(new UnityAction(this.OnClick));
			this.Text_Name.text = this.m_data.m_name;
			this.Text_Info.text = this.m_data.m_info;
			this.SetPosition(this.m_data.m_position);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.Button.onClick.RemoveListener(new UnityAction(this.OnClick));
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

		private void SetPosition(Vector3 position)
		{
			this.m_offsetNode.anchoredPosition = new Vector2(this.m_offsetX, this.m_offset);
			this.m_bg.anchoredPosition = Vector2.zero;
			this.m_child.position = position;
			this.m_leftFollow.position = this.m_left.position;
			this.m_rightFollow.position = this.m_right.position;
			this.m_upFollow.position = this.m_up.position;
			Vector2 windowSize = Utility.UI.GetWindowSize();
			if (this.m_leftFollow.anchoredPosition.x < -windowSize.x / 2f)
			{
				float num = -windowSize.x / 2f - this.m_leftFollow.anchoredPosition.x;
				this.m_bg.anchoredPosition = new Vector3(num, this.m_bg.anchoredPosition.y);
			}
			else if (this.m_rightFollow.anchoredPosition.x > windowSize.x / 2f)
			{
				float num2 = windowSize.x / 2f - this.m_rightFollow.anchoredPosition.x;
				this.m_bg.anchoredPosition = new Vector3(num2, this.m_bg.anchoredPosition.y);
			}
			if (this.m_upFollow.anchoredPosition.y > windowSize.y / 2f)
			{
				this.m_arrowBottom.SetActive(false);
				this.m_arrowTop.SetActive(true);
				this.m_offsetNode.anchoredPosition = new Vector2(this.m_offsetX, -this.m_offset);
				return;
			}
			this.m_arrowBottom.SetActive(true);
			this.m_arrowTop.SetActive(false);
			this.m_offsetNode.anchoredPosition = new Vector2(this.m_offsetX, this.m_offset);
		}

		private void OnClick()
		{
			GameApp.View.CloseView(ViewName.InfoTipViewModule, null);
			Action onClose = this.m_data.m_onClose;
			if (onClose == null)
			{
				return;
			}
			onClose();
		}

		public CustomButton Button;

		public CustomText Text_Name;

		public CustomText Text_Info;

		public RectTransform m_child;

		public RectTransform m_bg;

		public RectTransform m_offsetNode;

		public GameObject m_arrowBottom;

		public GameObject m_arrowTop;

		public RectTransform m_left;

		public RectTransform m_right;

		public RectTransform m_up;

		public RectTransform m_leftFollow;

		public RectTransform m_rightFollow;

		public RectTransform m_upFollow;

		private float m_offset;

		private float m_offsetX;

		private InfoTipViewModule.InfoTipData m_data;

		public class InfoTipData
		{
			public void Open()
			{
				GameApp.View.OpenView(ViewName.InfoTipViewModule, this, 2, null, null);
			}

			public string m_name;

			public string m_info;

			public Vector3 m_position;

			public float m_offsetY;

			public float m_offsetX;

			public Action m_onClose;
		}
	}
}
