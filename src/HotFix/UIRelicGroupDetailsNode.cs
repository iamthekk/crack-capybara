using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIRelicGroupDetailsNode : CustomBehaviour
	{
		public void SetData(int quality, bool isGray, List<MergeAttributeData> mergeAttributeDatas)
		{
			this.m_quality = quality;
			this.m_mergeAttributeDatas = mergeAttributeDatas;
			this.m_isGray = isGray;
		}

		protected override void OnInit()
		{
			if (this.m_isGray)
			{
				this.m_grays.SetUIGray();
				this.m_prefabgrays.SetUIGray();
			}
			else
			{
				this.m_grays.Recovery();
				this.m_prefabgrays.Recovery();
			}
			this.m_prefab.gameObject.SetActive(false);
			if (this.m_name != null)
			{
				string infoByID_LogError = Singleton<LanguageManager>.Instance.GetInfoByID_LogError((this.m_quality == 0) ? 6111 : 6110, new object[] { this.m_quality });
				this.m_name.text = infoByID_LogError;
			}
			LayoutRebuilder.MarkLayoutForRebuild(base.rectTransform);
			for (int i = 0; i < this.m_mergeAttributeDatas.Count; i++)
			{
				MergeAttributeData mergeAttributeData = this.m_mergeAttributeDatas[i];
				if (mergeAttributeData != null)
				{
					Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(mergeAttributeData.Header);
					if (elementById != null)
					{
						string text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId) + " +";
						text += this.GetAttributeString(mergeAttributeData);
						GameObject gameObject = Object.Instantiate<GameObject>(this.m_prefab);
						gameObject.SetParentNormal(this.m_parent, false);
						gameObject.SetActive(true);
						CustomText component = gameObject.GetComponent<CustomText>();
						if (component != null)
						{
							component.text = text;
						}
						this.m_objects[gameObject.GetInstanceID()] = gameObject;
					}
				}
			}
			LayoutRebuilder.MarkLayoutForRebuild(this.m_parent);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_parent);
			LayoutRebuilder.MarkLayoutForRebuild(base.rectTransform);
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.rectTransform);
		}

		protected override void OnDeInit()
		{
			this.m_mergeAttributeDatas = null;
			foreach (KeyValuePair<int, GameObject> keyValuePair in this.m_objects)
			{
				if (!(keyValuePair.Value == null))
				{
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_objects.Clear();
		}

		private string GetAttributeString(MergeAttributeData data)
		{
			if (data.Header.Contains("%"))
			{
				return data.Value.ToString() + "%";
			}
			return DxxTools.FormatNumber(data.Value.AsLong());
		}

		private int m_quality;

		private List<MergeAttributeData> m_mergeAttributeDatas = new List<MergeAttributeData>();

		private bool m_isGray;

		public CustomText m_name;

		public UIGrays m_grays;

		public RectTransform m_parent;

		public GameObject m_prefab;

		public UIGrays m_prefabgrays;

		public Dictionary<int, GameObject> m_objects = new Dictionary<int, GameObject>();
	}
}
