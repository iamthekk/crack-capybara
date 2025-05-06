using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class PetItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_goDeploy.SetActive(false);
			this.m_goAssist.SetActive(false);
			this.m_lock.SetActive(false);
			this.m_mask.SetActive(false);
			this.m_tick.SetActive(false);
			this.m_selectGo.SetActive(false);
			this.m_redNode.gameObject.SetActive(false);
			this.fragmentSlider.gameObject.SetActive(false);
			this.m_btnItem.onClick.AddListener(new UnityAction(this.OnBtnItemClick));
		}

		protected override void OnDeInit()
		{
			this.m_btnItem.onClick.RemoveListener(new UnityAction(this.OnBtnItemClick));
		}

		public void SetIndex(int index)
		{
			this.index = index;
		}

		public void RefreshData(PetData petData)
		{
			this.data = petData;
			try
			{
				this.m_txtLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("text_level_n", new object[] { petData.level });
				Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(petData.petId);
				GameMember_member elementById2 = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(elementById.memberId);
				Atlas_atlas elementById3 = GameApp.Table.GetManager().GetAtlas_atlasModelInstance().GetElementById(elementById2.iconAtlasID);
				this.m_imgIcon.SetImage(elementById3.path, elementById2.iconSpriteName);
				this.mPetStarNode.SetActive(false);
				this.SetQualityFrame();
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		public void SetFormationTypeActive(bool active)
		{
			if (active)
			{
				this.m_goDeploy.SetActive(this.data.formationType == EPetFormationType.Fight1 || this.data.formationType == EPetFormationType.Fight2 || this.data.formationType == EPetFormationType.Fight3);
				this.m_goAssist.SetActive(false);
				return;
			}
			this.m_goDeploy.SetActive(false);
			this.m_goAssist.SetActive(false);
		}

		public void SetSelectFrameActive(bool active)
		{
			this.m_selectGo.SetActive(active);
		}

		public void SetTickActive(bool active)
		{
			if (this.m_tick == null)
			{
				return;
			}
			this.m_tick.SetActive(active);
		}

		public void SetMaskActive(bool active)
		{
			if (this.m_mask == null)
			{
				return;
			}
			this.m_mask.SetActive(active);
		}

		public void SetRedNodeActive(bool active)
		{
			if (this.m_redNode == null)
			{
				return;
			}
			this.m_redNode.gameObject.SetActive(active);
		}

		public void SetFragmentProgressActive(bool active, int cur, int max)
		{
			if (this.fragmentSlider == null || this.fragmentSliderText == null)
			{
				return;
			}
			this.fragmentSlider.gameObject.SetActive(active);
			if (active)
			{
				this.fragmentSlider.value = (float)cur / (float)max;
				this.fragmentSliderText.text = string.Format("{0}/{1}", cur, max);
			}
		}

		private void SetQualityFrame()
		{
			Quality_petQuality elementById = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(this.data.quality);
			if (elementById == null)
			{
				HLog.LogError(string.Format("quality: {0}, Pet quality is invalid!!", this.data.quality));
				return;
			}
			Atlas_atlas elementById2 = GameApp.Table.GetManager().GetAtlas_atlasModelInstance().GetElementById(elementById.atlasId);
			string text = ((elementById2 != null) ? elementById2.path : "");
			this.m_imgQualityBg.SetImage(text, elementById.bgSpriteName);
		}

		private void OnBtnItemClick()
		{
			Action<PetItem, bool> action = this.onItemClickCallback;
			if (action == null)
			{
				return;
			}
			action(this, true);
		}

		public CustomButton m_btnItem;

		public CustomImage m_imgQualityBg;

		public CustomImage m_imgIcon;

		public CustomText m_txtLevel;

		public PetStarNode mPetStarNode;

		public GameObject m_goDeploy;

		public GameObject m_goAssist;

		public GameObject m_lock;

		public GameObject m_mask;

		public GameObject m_tick;

		public GameObject m_selectGo;

		public Slider fragmentSlider;

		public CustomText fragmentSliderText;

		public RedNodeOneCtrl m_redNode;

		public Action<PetItem, bool> onItemClickCallback;

		public int index;

		public PetData data;
	}
}
