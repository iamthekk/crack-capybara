using System;
using System.Collections;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class TalentAttributeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.levelUpEffect1.SetActive(false);
			this.levelUpEffect2.SetActive(false);
			this.levelUpEffect3.SetActive(false);
			this.sweepLightEffect.SetActive(false);
			this.imgBgMax.SetActive(false);
			this.btnItem.m_onClick = new Action(this.OnBtnItemClick);
		}

		protected override void OnDeInit()
		{
			this.btnItem.m_onClick = null;
		}

		public IEnumerator PlayOpenAnimation(int delayFrame)
		{
			this.levelUpEffect1.SetActive(false);
			this.levelUpEffect2.SetActive(false);
			this.levelUpEffect3.SetActive(false);
			this.sweepLightEffect.SetActive(false);
			while (delayFrame > 0)
			{
				int num = delayFrame;
				delayFrame = num - 1;
				yield return 0;
			}
			this.itemAnimator.Play("Open");
			yield break;
		}

		public IEnumerator PlaySweepLightAnimation()
		{
			this.sweepLightEffect.SetActive(false);
			this.sweepLightEffect.SetActive(true);
			yield return new WaitForSeconds(1f);
			this.sweepLightEffect.SetActive(false);
			yield break;
		}

		public void PlayLevelUpAnimation(int talentLevelUpCritType)
		{
			GameApp.Sound.PlayClip(623, 1f);
			this.levelUpEffect1.SetActive(false);
			this.levelUpEffect2.SetActive(false);
			this.levelUpEffect3.SetActive(false);
			GameObject gameObject = null;
			if (talentLevelUpCritType == 0)
			{
				gameObject = this.levelUpEffect1;
			}
			else if (talentLevelUpCritType == 1)
			{
				gameObject = this.levelUpEffect2;
			}
			else if (talentLevelUpCritType == 2)
			{
				gameObject = this.levelUpEffect3;
			}
			if (gameObject != null)
			{
				gameObject.SetActive(true);
				ParticleSystem[] componentsInChildren = gameObject.GetComponentsInChildren<ParticleSystem>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].Play();
				}
			}
			if (talentLevelUpCritType != 0)
			{
				DelayCall.Instance.CallOnce(100, delegate
				{
					if (this != null && this.critEffectItem != null)
					{
						Object.Instantiate<TalentCritEffectItem>(this.critEffectItem, this.critEffectItem.transform.parent, false).PlayCritAnimation(talentLevelUpCritType);
					}
				});
			}
		}

		public void SetEffectScale(float scaleX)
		{
			this.levelUpEffect1.transform.localScale = new Vector3(scaleX, 1f, 1f);
			this.levelUpEffect2.transform.localScale = new Vector3(scaleX, 1f, 1f);
			this.levelUpEffect3.transform.localScale = new Vector3(scaleX, 1f, 1f);
			this.sweepLightEffect.transform.localScale = new Vector3(scaleX, 1f, 1f);
		}

		public void SetData(TalentAttributeLevelUpData data)
		{
			this.data = data;
			this.itemLevel = data.curLevel;
			this.txtLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("UITalent_Level", new object[] { this.itemLevel });
			Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(data.talentAttributeKey);
			this.imgIcon.SetImage(elementById.iconAtlasID, elementById.iconName);
			this.txtDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId) ?? "";
			string text = DxxTools.FormatNumber((long)data.talentAttributeValue * (long)this.itemLevel);
			this.txtValue.text = "+" + text;
			this.txtFullLevel.gameObject.SetActive(this.itemLevel >= data.maxLevel);
			this.imgBgNormal.SetActive(this.itemLevel < data.maxLevel);
			this.imgBgMax.SetActive(this.itemLevel >= data.maxLevel);
			this.UpdateCost();
		}

		public void UpdateLevel()
		{
			this.itemLevel = Mathf.Clamp(this.data.curLevel, 0, this.data.maxLevel);
			Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(this.data.talentAttributeKey);
			if (this.itemLevel >= this.data.maxLevel)
			{
				this.txtLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("talent_attribute_level_max");
				this.txtDesc.text = "<color=#4A2875>" + Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId) + "</color>";
				string text = DxxTools.FormatNumber((long)this.data.talentAttributeValue * (long)this.itemLevel);
				this.txtValue.text = "<color=#FFFFFF>+" + text + "</color>";
			}
			else
			{
				this.txtLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("UITalent_Level", new object[] { this.itemLevel });
				this.txtDesc.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId) ?? "";
				string text2 = DxxTools.FormatNumber((long)this.data.talentAttributeValue * (long)this.itemLevel);
				this.txtValue.text = "+" + text2;
			}
			this.imgBgNormal.SetActive(this.itemLevel < this.data.maxLevel);
			this.imgBgMax.SetActive(this.itemLevel >= this.data.maxLevel);
			if (this.itemLevel >= this.data.maxLevel)
			{
				this.txtFullLevel.gameObject.SetActive(this.itemLevel >= this.data.maxLevel);
				this.costNode.SetActive(false);
			}
			this.UpdateCost();
		}

		private void UpdateCost()
		{
			if (this.itemLevel < this.data.maxLevel && this.data.levelUpCost.Length == 2)
			{
				this.costData = new ItemData((int)this.data.levelUpCost[0], this.data.levelUpCost[1]);
				this.costNode.SetActive(true);
				this.costNode.SetData(this.costData);
				return;
			}
			Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(this.data.talentAttributeKey);
			this.txtLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("talent_attribute_level_max");
			this.costNode.SetActive(false);
			this.txtDesc.text = "<color=#4A2875>" + Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId) + "</color>";
			string text = DxxTools.FormatNumber((long)this.data.talentAttributeValue * (long)this.itemLevel);
			this.txtValue.text = "<color=#FFFFFF>+" + text + "</color>";
		}

		private void OnBtnItemClick()
		{
			Action<TalentAttributeItem> action = this.clickCallback;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		public void TriggerNewbieGuide()
		{
			GuideController.Instance.DelTarget("TalentAttributeItem");
			GuideController.Instance.AddTarget("TalentAttributeItem", this.btnItem.transform);
		}

		public GameObject imgBgNormal;

		public GameObject imgBgMax;

		public GameObject levelUpEffect1;

		public GameObject levelUpEffect2;

		public GameObject levelUpEffect3;

		public GameObject sweepLightEffect;

		public TalentCritEffectItem critEffectItem;

		public Animator itemAnimator;

		public CustomButton btnItem;

		public CustomText txtLevel;

		public CustomImage imgIcon;

		public CustomText txtValue;

		public CustomText txtDesc;

		public UICostNode costNode;

		public CustomText txtFullLevel;

		public Action<TalentAttributeItem> clickCallback;

		public TalentAttributeLevelUpData data;

		private int itemLevel;

		public ItemData costData;
	}
}
