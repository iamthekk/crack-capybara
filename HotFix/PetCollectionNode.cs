using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Pet;
using Server;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class PetCollectionNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.petCollectionItem.gameObject.SetActive(false);
			this.btnActiveGroup.onClick.AddListener(new UnityAction(this.OnBtnActiveGroupClick));
			this.rectTrans = base.transform as RectTransform;
			this.initHeight = this.rectTrans.sizeDelta.y;
		}

		protected override void OnDeInit()
		{
			this.btnActiveGroup.onClick.RemoveListener(new UnityAction(this.OnBtnActiveGroupClick));
		}

		public void SetData(PetCollectionData collectionData, int index)
		{
			this.index = index;
			this.collectionData = collectionData;
			this.UpdateView();
		}

		public void UpdateView()
		{
			int curConfigId = this.collectionData.curConfigId;
			Pet_petCollection elementById = GameApp.Table.GetManager().GetPet_petCollectionModelInstance().GetElementById(curConfigId);
			if (elementById == null)
			{
				return;
			}
			Utility.Math.Max(this.collectionData.conditionStar, 1);
			int num = 0;
			this.txtStarRequire.text = string.Format("{0}", num);
			this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.groupNameId);
			this.goArrowUp.SetActive(this.collectionData.canUpgrade);
			if (this.items.Count < this.collectionData.requirePetIds.Count)
			{
				this.CreateItems(this.collectionData.requirePetIds.Count - this.items.Count);
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				int num2 = ((this.collectionData.requirePetIds.Count > i) ? this.collectionData.requirePetIds[i] : 0);
				if (num2 > 0)
				{
					this.items[i].gameObject.SetActive(true);
					this.items[i].SetData(num2, this.collectionData.conditionStar);
				}
				else
				{
					this.items[i].gameObject.SetActive(false);
				}
			}
			int num3 = Utility.Math.CeilToInt((float)this.collectionData.requirePetIds.Count * 1f / (float)this.gridLayout.constraintCount);
			float num4 = this.initHeight + (float)(num3 - 1) * this.gridLayout.cellSize.y + (float)(num3 - 1) * this.gridLayout.spacing.y;
			if (Mathf.Abs(num4 - this.rectTrans.sizeDelta.y) > 1f)
			{
				this.rectTrans.sizeDelta = new Vector2(this.rectTrans.sizeDelta.x, num4);
				base.gameObject.GetComponent<LoopListViewItem2>().ParentListView.OnItemSizeChanged(this.index);
			}
			this.UpdateAttribute();
		}

		private void UpdateAttribute()
		{
			this.btnActiveGroup.gameObject.SetActive(!this.collectionData.isFull);
			this.redNode.gameObject.SetActive(this.collectionData.canUpgrade);
			int curConfigId = this.collectionData.curConfigId;
			List<MergeAttributeData> mergeAttributeData = GameApp.Table.GetManager().GetPet_petCollectionModelInstance().GetElementById(curConfigId)
				.attributes.GetMergeAttributeData();
			if (mergeAttributeData.Count <= 0)
			{
				return;
			}
			MergeAttributeData mergeAttributeData2 = mergeAttributeData[0];
			Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(mergeAttributeData2.Header);
			string text = (mergeAttributeData2.Header.Contains("%") ? "%" : "");
			string text2 = (mergeAttributeData2.Header.Contains("%") ? string.Format("{0}", mergeAttributeData2.Value) : string.Format("{0}", mergeAttributeData2.Value.GetValue()));
			if (this.collectionData.isFull)
			{
				this.txtAttribute.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId) + " +" + text2 + text;
				return;
			}
			int num = ((this.collectionData.curIndex > 0) ? this.collectionData.GetConfigIdByIndex(this.collectionData.curIndex - 1) : 0);
			if (num > 0)
			{
				List<MergeAttributeData> mergeAttributeData3 = GameApp.Table.GetManager().GetPet_petCollectionModelInstance().GetElementById(num)
					.attributes.GetMergeAttributeData();
				if (mergeAttributeData3.Count > 0)
				{
					MergeAttributeData mergeAttributeData4 = mergeAttributeData3[0];
					FP fp = mergeAttributeData2.Value - mergeAttributeData4.Value;
					string text3 = (mergeAttributeData4.Header.Contains("%") ? string.Format("{0}", fp) : string.Format("{0}", fp.GetValue()));
					string text4 = (mergeAttributeData4.Header.Contains("%") ? string.Format("{0}", mergeAttributeData4.Value) : string.Format("{0}", mergeAttributeData4.Value.GetValue()));
					this.txtAttribute.text = string.Concat(new string[]
					{
						Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId),
						" +",
						text4,
						text,
						" <color=#319F34>+",
						text3,
						text,
						"</color>"
					});
				}
				else
				{
					this.txtAttribute.text = string.Concat(new string[]
					{
						Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId),
						" <color=#319F34>+",
						text2,
						text,
						"</color>"
					});
				}
			}
			else
			{
				this.txtAttribute.text = string.Concat(new string[]
				{
					Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId),
					" <color=#319F34>+",
					text2,
					text,
					"</color>"
				});
			}
			if (this.collectionData.canUpgrade)
			{
				this.btnActiveGroup.GetComponent<UIGrays>().Recovery();
				return;
			}
			this.btnActiveGroup.GetComponent<UIGrays>().SetUIGray();
		}

		private void CreateItems(int num)
		{
			for (int i = 0; i < num; i++)
			{
				PetCollectionItem petCollectionItem = Object.Instantiate<PetCollectionItem>(this.petCollectionItem);
				petCollectionItem.transform.SetParentNormal(this.petCollectionItem.transform.parent, false);
				petCollectionItem.Init();
				petCollectionItem.onItemClickCallback = new Action<PetCollectionItem>(this.OnPetCollectionItemClick);
				this.items.Add(petCollectionItem);
			}
		}

		private void OnPetCollectionItemClick(PetCollectionItem item)
		{
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(item.petConfigId);
			if (elementById == null)
			{
				return;
			}
			Vector3 position = item.petItem.transform.position;
			float num = 300f;
			new InfoTipViewModule.InfoTipData
			{
				m_name = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID),
				m_info = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.describeID),
				m_position = position,
				m_offsetY = num
			}.Open();
		}

		private void OnBtnActiveGroupClick()
		{
			if (this.collectionData.canUpgrade)
			{
				NetworkUtils.Pet.PetFetterActiveRequest(this.collectionData.curConfigId, delegate(bool isOk, PetFetterActiveResponse resp)
				{
					if (isOk && this != null && base.gameObject != null)
					{
						this.SetData(this.collectionData, this.index);
					}
				});
				return;
			}
			GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("pet_collection_condition_tip"));
		}

		public GridLayoutGroup gridLayout;

		public PetCollectionItem petCollectionItem;

		public CustomText txtTitle;

		public GameObject goArrowUp;

		public CustomText txtStarRequire;

		public CustomImage imgStar;

		public CustomText txtAttribute;

		public CustomButton btnActiveGroup;

		public RedNodeOneCtrl redNode;

		private PetCollectionData collectionData;

		private List<PetCollectionItem> items = new List<PetCollectionItem>();

		private RectTransform rectTrans;

		private float initHeight;

		private int index;
	}
}
