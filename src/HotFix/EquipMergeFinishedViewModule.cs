using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class EquipMergeFinishedViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_equipItem.Init();
			this.m_attributeNodePrefab.SetActive(false);
			this.m_skillNodePrefab.SetActive(false);
		}

		public override void OnOpen(object data)
		{
			if (this.m_closeBtn != null)
			{
				this.m_closeBtn.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			}
			if (this.m_closeMaskBt != null)
			{
				this.m_closeMaskBt.onClick.AddListener(new UnityAction(this.OnClickCloseBt));
			}
			this.m_seqPool.Clear(false);
			this.m_openData = data as EquipMergeFinishedViewModule.OpenData;
			if (this.m_openData == null)
			{
				return;
			}
			if (this.m_equipItem != null)
			{
				this.m_equipItem.RefreshData(this.m_openData.m_equipData);
				this.m_equipItem.SetButtonEnable(false);
			}
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.m_openData.m_equipData.nameId);
			}
			this.SetInfo(this.m_openData.m_lastEquipData, this.m_openData.m_equipData);
			GameApp.Sound.PlayClip(603, 1f);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			if (this.m_closeBtn != null)
			{
				this.m_closeBtn.onClick.RemoveListener(new UnityAction(this.OnClickCloseBt));
			}
			if (this.m_closeMaskBt != null)
			{
				this.m_closeMaskBt.onClick.RemoveListener(new UnityAction(this.OnClickCloseBt));
			}
		}

		public override void OnDelete()
		{
			this.m_seqPool.Clear(false);
			foreach (KeyValuePair<int, UIEquipMergeAttributeNode> keyValuePair in this.m_attributeDic)
			{
				keyValuePair.Value.DeInit();
				Object.Destroy(keyValuePair.Value.gameObject);
			}
			this.m_attributeDic.Clear();
			foreach (KeyValuePair<int, UIEquipMergeSkillNode> keyValuePair2 in this.m_skillDic)
			{
				keyValuePair2.Value.DeInit();
				Object.Destroy(keyValuePair2.Value.gameObject);
			}
			this.m_skillDic.Clear();
			this.m_closeBtn = null;
			this.m_closeMaskBt = null;
			this.m_equipItem = null;
			this.m_nameTxt = null;
			this.m_attributesGroup = null;
			this.m_attributeNodePrefab = null;
			this.m_skillNodePrefab = null;
			this.m_openData = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void SetInfo(EquipData lastEquipData, EquipData equipData)
		{
			if (lastEquipData == null)
			{
				return;
			}
			GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(lastEquipData.composeId);
			Equip_equipCompose elementById = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(equipData.composeId);
			Color color;
			ColorUtility.TryParseHtmlString(GameApp.Table.GetManager().GetQuality_equipQualityModelInstance().GetElementById(elementById.qualityColor)
				.colorImgLightBlend, ref color);
			this.m_imgCircleLight.color = color;
			foreach (KeyValuePair<int, UIEquipMergeAttributeNode> keyValuePair in this.m_attributeDic)
			{
				keyValuePair.Value.DeInit();
				Object.Destroy(keyValuePair.Value.gameObject);
			}
			this.m_attributeDic.Clear();
			foreach (KeyValuePair<int, UIEquipMergeSkillNode> keyValuePair2 in this.m_skillDic)
			{
				keyValuePair2.Value.DeInit();
				Object.Destroy(keyValuePair2.Value.gameObject);
			}
			this.m_skillDic.Clear();
			float num = 0.5f;
			float num2 = 0f;
			float num3 = 10f;
			int qualityColor = lastEquipData.qualityColor;
			int qualityColor2 = equipData.qualityColor;
			Quality_equipQuality elementById2 = GameApp.Table.GetManager().GetQuality_equipQualityModelInstance().GetElementById(qualityColor);
			Quality_equipQuality elementById3 = GameApp.Table.GetManager().GetQuality_equipQualityModelInstance().GetElementById(qualityColor2);
			if (elementById2 != null && elementById3 != null && elementById2.colorNumDark != elementById3.colorNumDark)
			{
				Color color2;
				ColorUtility.TryParseHtmlString(elementById2.colorNumDark, ref color2);
				Color color3;
				ColorUtility.TryParseHtmlString(elementById3.colorNumDark, ref color3);
				this.PlayTextColorChange(this.m_nameTxt, color2, color3, num, 0.5f);
				this.PlayTextColorChange(this.m_imgNameLine, color2, color3, num, 0.5f);
				num += 1f;
				num += 0.5f;
			}
			else
			{
				Color color4;
				ColorUtility.TryParseHtmlString(elementById3.colorNumDark, ref color4);
				this.m_nameTxt.color = color4;
				this.m_imgNameLine.color = color4;
			}
			List<MergeAttributeData> list = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)lastEquipData.id)
				.GetMergeAttributeData((int)lastEquipData.level, lastEquipData.evolution)
				.Merge();
			List<MergeAttributeData> list2 = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)equipData.id)
				.GetMergeAttributeData((int)equipData.level, equipData.evolution)
				.Merge();
			List<MergeAttributeData> list3 = MergeAttributeDataHelper.MinusMergeList(list2, list);
			for (int i = 0; i < list3.Count; i++)
			{
				string header = list3[i].Header;
				GameObject gameObject = Object.Instantiate<GameObject>(this.m_attributeNodePrefab);
				gameObject.SetParentNormal(this.m_attributesGroup, false);
				UIEquipMergeAttributeNode component = gameObject.GetComponent<UIEquipMergeAttributeNode>();
				component.Init();
				component.SetActive(true);
				Attribute_AttrText elementById4 = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(header);
				component.SetNameTxt(Singleton<LanguageManager>.Instance.GetInfoByID(elementById4.LanguageId));
				FP attributeValue = list.GetAttributeValue(header);
				FP attributeValue2 = list2.GetAttributeValue(header);
				component.SetFromTxt(header.Contains("%") ? (Utility.Math.Round(attributeValue.AsDouble(), 2).ToString() + "%") : attributeValue.AsLong().ToString());
				component.SetToTxt(header.Contains("%") ? (Utility.Math.Round(attributeValue2.AsDouble(), 2).ToString() + "%") : attributeValue2.AsLong().ToString());
				this.m_attributeDic[component.GetObjectInstanceID()] = component;
				RectTransform rectTransform = gameObject.transform as RectTransform;
				rectTransform.anchoredPosition = new Vector2(0f, -num2);
				num2 += rectTransform.sizeDelta.y + num3;
				this.PlayItemScale(rectTransform, num, 1f);
				num += 0.3f;
			}
			num2 += 10f;
			List<EquipSkillInfo> skillInfoList = GameApp.Data.GetDataModule(DataName.EquipDataModule).GetSkillInfoList((int)lastEquipData.id, equipData.composeId);
			for (int j = 0; j < skillInfoList.Count; j++)
			{
				EquipSkillInfo equipSkillInfo = skillInfoList[j];
				if (equipSkillInfo != null)
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(this.m_skillNodePrefab);
					gameObject2.SetParentNormal(this.m_attributesGroup, false);
					UIEquipMergeSkillNode component2 = gameObject2.GetComponent<UIEquipMergeSkillNode>();
					component2.Init();
					component2.SetActive(true);
					Equip_skill elementById5 = GameApp.Table.GetManager().GetEquip_skillModelInstance().GetElementById(equipSkillInfo.m_skillID);
					component2.SetTxt(Singleton<LanguageManager>.Instance.GetInfoByID(elementById5.descId));
					component2.CalcTxtLineAndHeight();
					this.m_skillDic[component2.GetObjectInstanceID()] = component2;
					RectTransform rectTransform2 = gameObject2.transform as RectTransform;
					rectTransform2.anchoredPosition = new Vector2(0f, -num2);
					num2 += rectTransform2.sizeDelta.y + num3;
					this.PlayItemScale(rectTransform2, num, 1f);
					num += 0.3f;
				}
			}
			this.m_attributesGroup.sizeDelta = new Vector2(this.m_attributesGroup.sizeDelta.x, num2);
		}

		private void PlayTextColorChange(Graphic g, Color color1, Color color2, float interval, float time)
		{
			RectTransform rectTransform = g.transform as RectTransform;
			rectTransform.localScale = Vector3.one;
			g.color = color1;
			Sequence sequence = this.m_seqPool.Get();
			Sequence sequence2 = this.m_seqPool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, interval);
			TweenSettingsExtensions.AppendInterval(sequence2, interval);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOColor(g, Color.white, time * 0.4f));
			TweenSettingsExtensions.Append(sequence2, ShortcutExtensions.DOScale(rectTransform, 1.5f, time * 0.4f));
			TweenSettingsExtensions.AppendInterval(sequence, time * 0.1f);
			TweenSettingsExtensions.AppendInterval(sequence2, time * 0.1f);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOColor(g, color2, time * 0.5f));
			TweenSettingsExtensions.Append(sequence2, ShortcutExtensions.DOScale(rectTransform, 1f, time * 0.5f));
		}

		private void PlayItemScale(RectTransform tf, float interval, float time = 1f)
		{
			tf.anchoredPosition = new Vector2(-1500f, tf.anchoredPosition.y);
			Sequence sequence = this.m_seqPool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, interval);
			TweenSettingsExtensions.SetEase<Sequence>(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(tf, 0f, time, false)), 27);
		}

		private void OnClickCloseBt()
		{
			GameApp.View.CloseView(ViewName.EquipMergeFinishedViewModule, null);
		}

		public CustomButton m_closeBtn;

		public CustomButton m_closeMaskBt;

		public UIHeroEquipItem m_equipItem;

		public CustomImage m_imgCircleLight;

		public CustomText m_nameTxt;

		public CustomImage m_imgNameLine;

		public RectTransform m_attributesGroup;

		public GameObject m_attributeNodePrefab;

		public GameObject m_skillNodePrefab;

		private Dictionary<int, UIEquipMergeAttributeNode> m_attributeDic = new Dictionary<int, UIEquipMergeAttributeNode>();

		private Dictionary<int, UIEquipMergeSkillNode> m_skillDic = new Dictionary<int, UIEquipMergeSkillNode>();

		private SequencePool m_seqPool = new SequencePool();

		public EquipMergeFinishedViewModule.OpenData m_openData;

		public class OpenData
		{
			public EquipData m_lastEquipData;

			public EquipData m_equipData;
		}
	}
}
