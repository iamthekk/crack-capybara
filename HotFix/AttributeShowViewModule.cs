using System;
using System.Collections.Generic;
using System.Reflection;
using Framework;
using Framework.EventSystem;
using Framework.ViewModule;
using LocalModels.Bean;
using Server;
using UnityEngine;

namespace HotFix
{
	public class AttributeShowViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.nodeItem.gameObject.SetActive(false);
		}

		public override void OnOpen(object data)
		{
			if (data is MemberAttributeData)
			{
				this.memberAttributeData = data as MemberAttributeData;
				this.UpdateView();
				return;
			}
			this.OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType.ButtonClose);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.uiPopCommon.OnClick = null;
		}

		private void UpdateView()
		{
			this.UpdateData();
			this.ClearNodeItemList();
			((RectTransform)this.nodeItem.transform.parent).anchoredPosition = Vector2.zero;
			if (this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.Base].attributeList.Count > 0)
			{
				AttributeShowNodeItem attributeShowNodeItem = Object.Instantiate<AttributeShowNodeItem>(this.nodeItem, this.nodeItem.transform.parent, false);
				this.nodeItemList.Add(attributeShowNodeItem);
				attributeShowNodeItem.gameObject.SetActive(true);
				attributeShowNodeItem.Init();
				attributeShowNodeItem.SetData(this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.Base]);
			}
			if (this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.Advance].attributeList.Count > 0)
			{
				AttributeShowNodeItem attributeShowNodeItem2 = Object.Instantiate<AttributeShowNodeItem>(this.nodeItem, this.nodeItem.transform.parent, false);
				this.nodeItemList.Add(attributeShowNodeItem2);
				attributeShowNodeItem2.gameObject.SetActive(true);
				attributeShowNodeItem2.Init();
				attributeShowNodeItem2.SetData(this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.Advance]);
			}
			if (this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.TotalBase].attributeList.Count > 0)
			{
				AttributeShowNodeItem attributeShowNodeItem3 = Object.Instantiate<AttributeShowNodeItem>(this.nodeItem, this.nodeItem.transform.parent, false);
				this.nodeItemList.Add(attributeShowNodeItem3);
				attributeShowNodeItem3.gameObject.SetActive(true);
				attributeShowNodeItem3.Init();
				attributeShowNodeItem3.SetData(this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.TotalBase]);
			}
		}

		private void ClearNodeItemList()
		{
			for (int i = 0; i < this.nodeItemList.Count; i++)
			{
				AttributeShowNodeItem attributeShowNodeItem = this.nodeItemList[i];
				if (!(attributeShowNodeItem == null))
				{
					Object.Destroy(attributeShowNodeItem.gameObject);
				}
			}
			this.nodeItemList.Clear();
		}

		private void UpdateData()
		{
			this.attributeNodeDataDic.Clear();
			Dictionary<string, FP> attributeInfo = AttributeShowViewModule.GetAttributeInfo(this.memberAttributeData);
			this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.Base] = new AttributeShowViewModule.ItemNodeData();
			this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.Base].titleId = "attribute_base";
			this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.Advance] = new AttributeShowViewModule.ItemNodeData();
			this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.Advance].titleId = "attribute_advance";
			IList<Attribute_AttrText> allElements = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Attribute_AttrText attribute_AttrText = allElements[i];
				if (attribute_AttrText != null && (attribute_AttrText.DisplayType == 1 || attribute_AttrText.DisplayType == 2))
				{
					AttributeShowViewModule.ItemNodeData itemNodeData = this.attributeNodeDataDic[(AttributeShowViewModule.DisplayAttributeType)attribute_AttrText.DisplayType];
					AttributeShowViewModule.AttributeShowItemData attributeShowItemData = default(AttributeShowViewModule.AttributeShowItemData);
					attributeShowItemData.attributeTable = attribute_AttrText;
					FP fp;
					attributeShowItemData.attributeValue = (attributeInfo.TryGetValue(attribute_AttrText.ID, out fp) ? fp : 0);
					itemNodeData.attributeList.Add(attributeShowItemData);
				}
			}
			this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.TotalBase] = new AttributeShowViewModule.ItemNodeData();
			this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.TotalBase].titleId = "attribute_total_base";
			this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.TotalBase].attributeList = new List<AttributeShowViewModule.AttributeShowItemData>();
			this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.TotalBase].attributeList.Add(new AttributeShowViewModule.AttributeShowItemData
			{
				attributeTable = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById("Attack"),
				attributeValue = this.memberAttributeData.GetAttack4UI()
			});
			this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.TotalBase].attributeList.Add(new AttributeShowViewModule.AttributeShowItemData
			{
				attributeTable = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById("Defence"),
				attributeValue = this.memberAttributeData.GetDefence4UI()
			});
			this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.TotalBase].attributeList.Add(new AttributeShowViewModule.AttributeShowItemData
			{
				attributeTable = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById("HPMax"),
				attributeValue = this.memberAttributeData.GetHpMax4UI()
			});
			this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.Base].attributeList.Sort(new Comparison<AttributeShowViewModule.AttributeShowItemData>(this.Sort));
			this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.Advance].attributeList.Sort(new Comparison<AttributeShowViewModule.AttributeShowItemData>(this.Sort));
			this.attributeNodeDataDic[AttributeShowViewModule.DisplayAttributeType.TotalBase].attributeList.Sort(new Comparison<AttributeShowViewModule.AttributeShowItemData>(this.Sort));
		}

		private int Sort(AttributeShowViewModule.AttributeShowItemData a, AttributeShowViewModule.AttributeShowItemData b)
		{
			return a.attributeTable.SortID.CompareTo(b.attributeTable.SortID);
		}

		public static Dictionary<string, FP> GetAttributeInfo(MemberAttributeData data)
		{
			Dictionary<string, FP> dictionary = new Dictionary<string, FP>();
			foreach (FieldInfo fieldInfo in typeof(MemberAttributeData).GetFields(BindingFlags.Instance | BindingFlags.Public))
			{
				MemberAttributeInfo customAttribute = fieldInfo.GetCustomAttribute<MemberAttributeInfo>();
				if (customAttribute != null)
				{
					FP fp = (FP)fieldInfo.GetValue(data);
					dictionary[customAttribute.AttributeKey] = fp;
				}
			}
			return dictionary;
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickCloseBt();
			}
		}

		private void OnClickCloseBt()
		{
			GameApp.View.CloseView(ViewName.AttributeShowViewModule, null);
		}

		public UIPopCommon uiPopCommon;

		public AttributeShowNodeItem nodeItem;

		private MemberAttributeData memberAttributeData;

		private Dictionary<AttributeShowViewModule.DisplayAttributeType, AttributeShowViewModule.ItemNodeData> attributeNodeDataDic = new Dictionary<AttributeShowViewModule.DisplayAttributeType, AttributeShowViewModule.ItemNodeData>();

		private List<AttributeShowNodeItem> nodeItemList = new List<AttributeShowNodeItem>();

		public enum DisplayAttributeType
		{
			Base = 1,
			Advance,
			TotalBase
		}

		public struct AttributeShowItemData
		{
			public Attribute_AttrText attributeTable;

			public FP attributeValue;
		}

		public class ItemNodeData
		{
			public string titleId;

			public List<AttributeShowViewModule.AttributeShowItemData> attributeList = new List<AttributeShowViewModule.AttributeShowItemData>();
		}
	}
}
