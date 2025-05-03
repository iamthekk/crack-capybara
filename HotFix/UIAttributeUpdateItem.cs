using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIAttributeUpdateItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_typeImage.SetActive(false);
			this.m_typeText.SetActive(false);
		}

		public void SetData(List<AttributeTypeData> attributeList)
		{
			this.SetData(attributeList, null, null, null, null);
		}

		public void SetData(List<AttributeTypeData> attributeList, List<ItemTypeData> itemList, List<SkillTypeData> skillList, List<InfoTypeData> infoList, List<ActivityScoreTypeData> scoreList)
		{
			this.ClearItems();
			this.attrDic.Clear();
			List<AttributeTypeDataBase> list = new List<AttributeTypeDataBase>();
			if (attributeList != null)
			{
				list.AddRange(attributeList);
			}
			if (itemList != null)
			{
				list.AddRange(itemList);
			}
			if (skillList != null)
			{
				list.AddRange(skillList);
			}
			if (infoList != null)
			{
				list.AddRange(infoList);
			}
			if (scoreList != null)
			{
				list.AddRange(scoreList);
			}
			if (list.Count == 0)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (i != list.Count - 1)
				{
					bool flag = list.Count > 1;
				}
				AttributeTypeData attributeTypeData = null;
				AttributeTypeDataBase attributeTypeDataBase = list[i];
				AttributeTypeItemBase attributeTypeItemBase = null;
				GameObject gameObject = null;
				AttributeTypeData attributeTypeData2 = attributeTypeDataBase as AttributeTypeData;
				if (attributeTypeData2 != null)
				{
					string text;
					if (attributeTypeData2.IsImage(out text))
					{
						gameObject = Object.Instantiate<GameObject>(this.m_typeImage);
						attributeTypeItemBase = new AttributeTypeItem_Image();
						if (attributeTypeData2.num > 0f)
						{
							attributeTypeData = attributeTypeData2;
						}
					}
					else
					{
						gameObject = Object.Instantiate<GameObject>(this.m_typeText);
						attributeTypeItemBase = new AttributeTypeItem_Text();
					}
				}
				else if (attributeTypeDataBase is ItemTypeData || attributeTypeDataBase is SkillTypeData || attributeTypeDataBase is ActivityScoreTypeData)
				{
					gameObject = Object.Instantiate<GameObject>(this.m_typeImage);
					attributeTypeItemBase = new AttributeTypeItem_Image();
				}
				else if (attributeTypeDataBase is InfoTypeData)
				{
					gameObject = Object.Instantiate<GameObject>(this.m_typeText);
					attributeTypeItemBase = new AttributeTypeItem_Text();
				}
				if (attributeTypeItemBase != null && gameObject != null)
				{
					gameObject.SetActive(true);
					Transform transform = gameObject.transform;
					if (i < 3)
					{
						transform.SetParentNormal(this.m_child, false);
					}
					else
					{
						transform.SetParentNormal(this.m_child2, false);
					}
					attributeTypeItemBase.Init(gameObject);
					string empty = string.Empty;
					attributeTypeItemBase.SetData(attributeTypeDataBase, empty);
					this.m_list.Add(attributeTypeItemBase);
					if (attributeTypeData != null)
					{
						this.attrDic.Add(attributeTypeData.m_attrType, attributeTypeItemBase.GetFlyItem());
					}
				}
			}
			this.Rebuild();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.ClearItems();
		}

		public void ClearItems()
		{
			for (int i = 0; i < this.m_list.Count; i++)
			{
				AttributeTypeItemBase attributeTypeItemBase = this.m_list[i];
				attributeTypeItemBase.DeInit();
				Object.Destroy(attributeTypeItemBase.m_gameObject);
			}
			this.m_list.Clear();
			this.Rebuild();
		}

		public void Rebuild()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_child);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_child2);
		}

		public Dictionary<GameEventAttType, GameObject> GetFlyItems()
		{
			return this.attrDic;
		}

		public void ShowItems(Action onFinish)
		{
			Sequence sequence = DOTween.Sequence();
			this.Rebuild();
			for (int i = 0; i < this.m_list.Count; i++)
			{
				GameObject gameObject = this.m_list[i].m_gameObject;
				if (gameObject)
				{
					gameObject.transform.localScale = Vector3.zero;
				}
			}
			if (this.m_list.Count > 0)
			{
				TweenSettingsExtensions.AppendInterval(sequence, 0.2f);
			}
			for (int j = 0; j < this.m_list.Count; j++)
			{
				GameObject gameObject2 = this.m_list[j].m_gameObject;
				if (gameObject2)
				{
					TweenSettingsExtensions.AppendInterval(sequence, (float)j * 0.01f);
					TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(gameObject2.transform, Vector3.one * 1.1f, 0.15f)), ShortcutExtensions.DOScale(gameObject2.transform, Vector3.one, 0.05f));
				}
			}
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.Rebuild();
				Action onFinish2 = onFinish;
				if (onFinish2 == null)
				{
					return;
				}
				onFinish2();
			});
		}

		public int GetItemCount()
		{
			return this.m_list.Count;
		}

		public RectTransform m_child;

		public RectTransform m_child2;

		public GameObject m_typeImage;

		public GameObject m_typeText;

		private List<AttributeTypeItemBase> m_list = new List<AttributeTypeItemBase>();

		private Dictionary<GameEventAttType, GameObject> attrDic = new Dictionary<GameEventAttType, GameObject>();
	}
}
