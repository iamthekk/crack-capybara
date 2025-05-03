using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class OpenEquipBox_DrawAnimation : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.uiItem.Init();
			this.rewardItem.SetActive(false);
			this.btnSkip.gameObject.SetActive(false);
			this.btnTapContinue.gameObject.SetActive(false);
			this.btnTapContinue.m_onClick = new Action(this.OnBtnContinueClick);
			this.btnSkip.m_onClick = new Action(this.OnBtnSkipClick);
		}

		protected override void OnDeInit()
		{
			this.btnTapContinue.m_onClick = null;
			this.btnSkip.m_onClick = null;
		}

		public async void SetData(int boxId, List<ItemData> itemDatas, int iapMainActivityType, Action callback)
		{
			this.onPageEndCallback = callback;
			this.itemDatas = itemDatas;
			this.totalRewardCount = itemDatas.Count;
			this.curRewardCount = 0;
			string boxSkinName = EquipChestType.GetBoxSkinName(boxId);
			this.spineGraphic.initialSkinName = boxSkinName;
			this.spineGraphic.Initialize(true);
			this.PlayBoxOpen();
		}

		private void PlayBoxOpen()
		{
			this.btnTapContinue.gameObject.SetActive(false);
			int num = this.curRewardCount;
			this.curRewardCount++;
			this.uiItem.SetData(this.itemDatas[num].ToPropData());
			this.uiItem.OnRefresh();
			this.rewardItem.SetActive(false);
			int id = this.itemDatas[num].ID;
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(id);
			if (elementById == null)
			{
				HLog.LogError(string.Format("id:{0} in item table is null", id));
				return;
			}
			this.effectNormal.SetActive(elementById.quality == 1 || elementById.quality == 2);
			Quality_equipQuality elementById2 = GameApp.Table.GetManager().GetQuality_equipQualityModelInstance().GetElementById(elementById.quality);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			this.txtName.text = DxxTools.UI.GetColorString(infoByID, elementById2.colorNumDark);
			string text = "";
			this.txtQuality.text = text;
			this.spineGraphic.AnimationState.SetAnimation(0, "Open", false);
			this.spineGraphic.AnimationState.AddAnimation(0, "Open_Idle", true, 0f);
			int num2 = (int)((this.spineGraphic.Skeleton.Data.FindAnimation("Open").Duration - 0.3f) * 1000f);
			if (num2 < 0)
			{
				num2 = 0;
			}
			DelayCall.Instance.ClearCall(new DelayCall.CallAction(this.OpenBoxAniEnd));
			DelayCall.Instance.CallOnce(num2, new DelayCall.CallAction(this.OpenBoxAniEnd));
		}

		private void OpenBoxAniEnd()
		{
			if (this != null && base.gameObject != null)
			{
				GameApp.Sound.PlayClip(49, 1f);
				this.btnSkip.gameObject.SetActive(this.totalRewardCount > 1);
				this.btnTapContinue.gameObject.SetActive(true);
				this.rewardItem.gameObject.SetActive(true);
				this.rewardItem.transform.position = this.pointEnd.position;
			}
		}

		private void OnBtnContinueClick()
		{
			if (this.curRewardCount < this.totalRewardCount)
			{
				this.PlayBoxOpen();
				return;
			}
			Action action = this.onPageEndCallback;
			if (action == null)
			{
				return;
			}
			action();
		}

		private void OnBtnSkipClick()
		{
			Action action = this.onPageEndCallback;
			if (action == null)
			{
				return;
			}
			action();
		}

		public SkeletonGraphic spineGraphic;

		public UIItem uiItem;

		public GameObject rewardItem;

		public CustomText txtName;

		public CustomText txtQuality;

		public Transform pointStart;

		public Transform pointEnd;

		public CustomButton btnTapContinue;

		public CustomButton btnSkip;

		public GameObject effectNormal;

		private List<ItemData> itemDatas;

		private int curRewardCount;

		private int totalRewardCount;

		private Action onPageEndCallback;
	}
}
