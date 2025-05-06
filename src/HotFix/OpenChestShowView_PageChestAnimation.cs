using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace HotFix
{
	public class OpenChestShowView_PageChestAnimation : CustomBehaviour
	{
		protected override void OnInit()
		{
			try
			{
				this.uiItem.Init();
				this.spineMainChest.Initialize(true);
				this.rewardItem.SetActive(false);
				this.btnSkip.gameObject.SetActive(false);
				this.btnTapContinue.gameObject.SetActive(false);
				this.btnTapContinue.m_onClick = new Action(this.OnBtnContinueClick);
				this.btnSkip.m_onClick = new Action(this.OnBtnSkipClick);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
		}

		protected override void OnDeInit()
		{
			this.btnTapContinue.m_onClick = null;
			this.btnSkip.m_onClick = null;
		}

		public void ShowRewards(int chestType, List<ItemData> itemDatas, Action callback)
		{
			this.onPageEndCallback = callback;
			this.itemDatas = itemDatas;
			this.totalRewardCount = itemDatas.Count;
			this.curRewardCount = 0;
			if (GameApp.Data.GetDataModule(DataName.MainCityDataModule).IsSkipAni)
			{
				this.OnBtnSkipClick();
				return;
			}
			string boxSkinName = MainChestType.GetBoxSkinName(chestType);
			this.spineMainChest.Skeleton.SetSkin(boxSkinName);
			this.spineMainChest.Skeleton.SetSlotsToSetupPose();
			this.spineMainChest.AnimationState.SetAnimation(0, "Idle", true);
			GameApp.Sound.PlayClip(659, 1f);
			base.StartCoroutine(this.PlayBoxOpen());
		}

		private IEnumerator PlayBoxOpen()
		{
			this.btnTapContinue.gameObject.SetActive(false);
			int num = this.curRewardCount;
			this.curRewardCount++;
			this.uiItem.SetData(this.itemDatas[num].ToPropData());
			this.uiItem.OnRefresh();
			this.rewardItem.SetActive(false);
			int id = this.itemDatas[num].ID;
			if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(id) == null)
			{
				HLog.LogError(string.Format("id:{0} in item table is null", id));
				yield break;
			}
			Animation animation = this.spineMainChest.AnimationState.Data.SkeletonData.FindAnimation("Open");
			this.spineMainChest.AnimationState.SetAnimation(0, "Open", false);
			float num2 = Mathf.Max(animation.Duration, 0f);
			yield return new WaitForSeconds(num2);
			this.OpenBoxAniEnd();
			yield break;
		}

		private void OpenBoxAniEnd()
		{
			GameApp.Sound.PlayClip(661, 1f);
			Action action = this.onPageEndCallback;
			if (action == null)
			{
				return;
			}
			action();
		}

		private void OnBtnContinueClick()
		{
			if (this.curRewardCount < this.totalRewardCount)
			{
				base.StartCoroutine(this.PlayBoxOpen());
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

		public SkeletonGraphic spineMainChest;

		public UIItem uiItem;

		public GameObject rewardItem;

		public CustomText txtName;

		public CustomText txtQuality;

		public CustomButton btnTapContinue;

		public CustomButton btnSkip;

		public GameObject effectNormal;

		private List<ItemData> itemDatas;

		private int curRewardCount;

		private int totalRewardCount;

		private Action onPageEndCallback;
	}
}
