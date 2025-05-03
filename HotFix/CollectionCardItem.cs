using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine.UI;

namespace HotFix
{
	public class CollectionCardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.btnItem.m_onClick = new Action(this.OnBtnItemClick);
		}

		protected override void OnDeInit()
		{
			this.btnItem.m_onClick = null;
		}

		public void SetData(CollectionData data)
		{
			this.data = data;
			this.imgIcon.SetImage(data.atlasId, data.iconName);
			Quality_collectionQuality elementById = GameApp.Table.GetManager().GetQuality_collectionQualityModelInstance().GetElementById(data.quality);
			this.imgCardBg.SetImage(elementById.atlasId, elementById.cardBg);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(data.nameId);
			this.txtName.text = string.Concat(new string[] { "<color=", elementById.colorName, ">", infoByID, "</color>" });
			this.starNode.SetStar(data.collectionStar);
			this.sliderFragment.maxValue = (float)data.MergeNeedFragment;
			this.sliderFragment.value = (float)data.fragMentCount;
			this.txtSlider.text = string.Format("{0}/{1}", data.fragMentCount, data.MergeNeedFragment);
			this.sliderFragment.gameObject.SetActive(data.collectionType == 2U);
			this.imgMask.gameObject.SetActive(data.collectionType == 0U);
		}

		private void OnBtnItemClick()
		{
			if (this.data == null)
			{
				return;
			}
			GameApp.View.OpenView(ViewName.CollectionDetailViewModule, this.data, 1, null, null);
		}

		public CustomButton btnItem;

		public CustomImage imgCardBg;

		public CustomImage imgIcon;

		public CollectionStarNode starNode;

		public CustomText txtName;

		public Slider sliderFragment;

		public CustomText txtSlider;

		public CustomImage imgMask;

		private CollectionData data;
	}
}
