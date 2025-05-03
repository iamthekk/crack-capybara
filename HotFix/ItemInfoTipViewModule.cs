using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class ItemInfoTipViewModule : BaseViewModule
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
			this.m_data = data as ItemInfoTipViewModule.InfoTipData;
			this.m_offset = this.m_data.m_offsetY;
			this.Button.onClick.AddListener(new UnityAction(this.OnBtnCloseClick));
			PropData propData = new PropData();
			propData.rowId = this.m_data.openData.m_propData.rowId;
			propData.id = this.m_data.openData.m_propData.id;
			propData.count = 0UL;
			this.uiItem.SetData(propData);
			this.uiItem.OnRefresh();
			long num = DxxTools.UI.OnItemInfoMathVolume(propData);
			this.txtCount.text = Singleton<LanguageManager>.Instance.GetInfoByID("common_quantity_owned", new object[] { DxxTools.FormatNumber(num) });
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)propData.id);
			Quality_equipQuality elementById2 = GameApp.Table.GetManager().GetQuality_equipQualityModelInstance().GetElementById(elementById.quality);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			this.txtName.text = string.Concat(new string[] { "<color=", elementById2.colorNumDark, ">", infoByID, "</color>" });
			this.txtDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.describeID);
			this.SetPosition(this.m_data.m_position);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.Button.onClick.RemoveListener(new UnityAction(this.OnBtnCloseClick));
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
			position.z = base.transform.position.z;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_bg);
			Vector2 windowSize = Utility.UI.GetWindowSize();
			float num = 20f;
			float num2 = this.m_bg.rect.height * 0.5f + num + this.m_offset;
			this.m_offsetNode.anchoredPosition = new Vector2(0f, num2);
			this.m_bg.anchoredPosition = Vector2.zero;
			this.m_child.position = position;
			this.m_leftFollow.position = this.m_left.position;
			this.m_rightFollow.position = this.m_right.position;
			this.m_upFollow.position = this.m_up.position;
			if (this.m_leftFollow.anchoredPosition.x < -windowSize.x / 2f)
			{
				float num3 = -windowSize.x / 2f - this.m_leftFollow.anchoredPosition.x;
				this.m_bg.anchoredPosition = new Vector3(num3, this.m_bg.anchoredPosition.y);
			}
			else if (this.m_rightFollow.anchoredPosition.x > windowSize.x / 2f)
			{
				float num4 = windowSize.x / 2f - this.m_rightFollow.anchoredPosition.x;
				this.m_bg.anchoredPosition = new Vector3(num4, this.m_bg.anchoredPosition.y);
			}
			if (this.m_upFollow.anchoredPosition.y > windowSize.y / 2f)
			{
				this.m_arrowBottom.SetActive(false);
				this.m_arrowTop.SetActive(true);
				this.m_offsetNode.anchoredPosition = new Vector2(0f, -num2);
			}
			else
			{
				this.m_arrowBottom.SetActive(true);
				this.m_arrowTop.SetActive(false);
				this.m_offsetNode.anchoredPosition = new Vector2(0f, num2);
			}
			this.SetArrowPosition(position);
		}

		private void SetArrowPosition(Vector3 position)
		{
			position.z = base.transform.position.z;
			Vector3 vector = this.m_arrowBottom.transform.parent.InverseTransformPoint(position);
			if (Mathf.Abs(vector.x) >= this.m_bg.rect.width * 0.5f * 0.75f)
			{
				int num = ((vector.x >= 0f) ? 1 : (-1));
				vector.x = this.m_bg.rect.width * 0.5f * 0.75f * (float)num;
			}
			Vector3 localPosition = this.m_arrowTop.transform.localPosition;
			localPosition.x = vector.x;
			this.m_arrowTop.transform.localPosition = localPosition;
			Vector3 localPosition2 = this.m_arrowBottom.transform.localPosition;
			localPosition2.x = vector.x;
			this.m_arrowBottom.transform.localPosition = localPosition2;
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.ItemInfoTipViewModule, null);
			Action onClose = this.m_data.m_onClose;
			if (onClose == null)
			{
				return;
			}
			onClose();
		}

		public UIItem uiItem;

		public CustomText txtCount;

		public CustomButton Button;

		public CustomText txtName;

		public CustomText txtDesc;

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

		private ItemInfoTipViewModule.InfoTipData m_data;

		public class InfoTipData
		{
			public ItemInfoOpenData openData;

			public Vector3 m_position;

			public float m_offsetY;

			public Action m_onClose;
		}
	}
}
