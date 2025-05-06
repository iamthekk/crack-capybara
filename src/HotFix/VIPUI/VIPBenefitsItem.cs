using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.VIPUI
{
	public class VIPBenefitsItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(VIPDataModule.VIPData data)
		{
			this.mData = data;
		}

		public void RefreshUI()
		{
			this.TextNew.gameObject.SetActive(this.mData.m_isNew);
			this.ObjNormal.SetActive(!this.mData.m_isNew);
			this.TextContent.text = this.GetContent();
			this.ReCalcSize();
		}

		private string GetContent()
		{
			string text = this.MergeColor(this.mData.GetValueString());
			return Singleton<LanguageManager>.Instance.GetInfoByID(this.mData.m_languageID, new object[] { text });
		}

		private string MergeColor(string info)
		{
			return "<color=#E97E55>" + info + "</color>";
		}

		private void ReCalcSize()
		{
			RectTransform rectTransform = this.TextContent.rectTransform;
			Vector2 vector = rectTransform.sizeDelta;
			float preferredHeight = this.TextContent.preferredHeight;
			vector.y = preferredHeight;
			rectTransform.sizeDelta = vector;
			vector = base.rectTransform.sizeDelta;
			vector.y = preferredHeight + 12f;
			base.rectTransform.sizeDelta = vector;
		}

		private const string COLOR = "#E97E55";

		public CustomLanguageText TextNew;

		public CustomText TextContent;

		public GameObject ObjNormal;

		public VIPDataModule.VIPData mData;
	}
}
