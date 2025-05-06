using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public class CollectionSuitAttributeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.strColorActive = ColorUtility.ToHtmlStringRGB(this.colorActive);
			this.strColorLock = ColorUtility.ToHtmlStringRGB(this.colorLock);
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(Collection_collectionSuit data, bool isActive)
		{
			List<MergeAttributeData> mergeAttributeData = data.attributes.GetMergeAttributeData();
			if (mergeAttributeData == null || mergeAttributeData.Count <= 0)
			{
				this.txtAttribute.text = "";
				return;
			}
			MergeAttributeData mergeAttributeData2 = mergeAttributeData[0];
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(data.conditonPrifixTextId);
			string text = "";
			Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(mergeAttributeData2.Header);
			string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId);
			string text2;
			if (mergeAttributeData2.Header.Contains("%"))
			{
				text = "%";
				text2 = string.Format("{0} +{1}", infoByID2, mergeAttributeData2.Value);
			}
			else
			{
				text2 = infoByID2 + " +" + DxxTools.FormatNumber(mergeAttributeData2.Value.AsLong());
			}
			string text3;
			if (isActive)
			{
				text3 = string.Concat(new string[] { "<color=#", this.strColorActive, ">", infoByID, text2, text, "</color>" });
			}
			else
			{
				text3 = string.Concat(new string[] { "<color=#", this.strColorLock, ">", infoByID, text2, text, "</color>" });
			}
			this.txtAttribute.text = text3;
		}

		public Color colorActive;

		public Color colorLock;

		public CustomText txtAttribute;

		private string strColorActive;

		private string strColorLock;
	}
}
