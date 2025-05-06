using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class CollectionItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.txtActive.gameObject.SetActive(false);
			Color color = this.txtActive.color;
			color.a = 0.65f;
			TweenSettingsExtensions.SetLoops<Tweener>(ShortcutExtensions46.DOColor(this.txtActive, color, 1.2f), -1, 1);
			this.redNode.Value = 0;
			this.sliderFragment.gameObject.SetActive(false);
			this.starNode.SetStar(0);
			this.txtPlus.text = "";
			this.btnItem.m_onClick = new Action(this.OnBtnItemClick);
		}

		protected override void OnDeInit()
		{
			this.btnItem.m_onClick = null;
		}

		public void SetData(CollectionData collectionData)
		{
			this.data = collectionData;
			if (this.data == null)
			{
				return;
			}
			this.UpdateView();
		}

		private void UpdateView()
		{
			this.imgIcon.SetImage(this.data.atlasId, this.data.iconName);
			Quality_collectionQuality elementById = GameApp.Table.GetManager().GetQuality_collectionQualityModelInstance().GetElementById(this.data.quality);
			this.imgQuality.SetImage(elementById.atlasId, elementById.itemBg);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(this.data.nameId);
			this.txtName.text = string.Concat(new string[] { "<color=", elementById.colorName, ">", infoByID, "</color>" });
			this.txtPlus.text = "";
			this.starNode.SetStar(this.data.collectionStar);
			this.sliderFragment.gameObject.SetActive(this.data.collectionType == 2U);
			this.sliderFragment.maxValue = (float)this.data.MergeNeedFragment;
			this.sliderFragment.value = (float)this.data.fragMentCount;
			this.txtSlider.text = string.Format("{0}/{1}", this.data.fragMentCount, this.data.MergeNeedFragment);
			if (this.data.collectionType == 1U)
			{
				this.uiGray.Recovery();
				this.imgIcon.SetAlpha(1f);
			}
			else
			{
				this.imgIcon.SetAlpha(0.9f);
				this.uiGray.SetUIGray();
			}
			this.txtActive.gameObject.SetActive(this.data.IsCanMerge);
			if (this.RedCalcAction != null)
			{
				this.redNode.gameObject.SetActive(this.RedCalcAction(this.data));
			}
		}

		private void OnBtnItemClick()
		{
			if (this.data != null)
			{
				if (this.data.IsCanMerge)
				{
					NetworkUtils.Collection.CollectionMergeRequest((uint)this.data.fragmentRowId);
					return;
				}
				GameApp.View.OpenView(ViewName.CollectionDetailViewModule, this.data, 1, null, null);
			}
		}

		public Text txtActive;

		public CustomButton btnItem;

		public CustomImage imgQuality;

		public CustomImage imgIcon;

		public UIGray uiGray;

		public CustomText txtPlus;

		public CustomText txtName;

		public CollectionStarNode starNode;

		public RedNodeOneCtrl redNode;

		public Slider sliderFragment;

		public CustomText txtSlider;

		public CollectionData data;

		public Func<CollectionData, bool> RedCalcAction;
	}
}
