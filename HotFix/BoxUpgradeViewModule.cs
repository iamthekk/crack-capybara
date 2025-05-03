using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Google.Protobuf.Collections;
using Proto.Common;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class BoxUpgradeViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.modelItem.Init();
			this.copyItem.gameObject.SetActiveSafe(false);
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			this.openData = data as BoxUpgradeViewModule.OpenData;
			if (this.openData == null)
			{
				return;
			}
			this.isPlayAni = false;
			this.buttonUpgrade.onClick.AddListener(new UnityAction(this.OnClickUpgrade));
			this.currentProgress = 0;
			this.currentGrade = this.openData.initGrade;
			for (int i = 0; i < this.openData.upgradeLimit; i++)
			{
				UITreasureProgressItem uitreasureProgressItem;
				if (i < this.progressItems.Count)
				{
					uitreasureProgressItem = this.progressItems[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
					gameObject.SetParentNormal(this.layout, false);
					uitreasureProgressItem = gameObject.GetComponent<UITreasureProgressItem>();
					uitreasureProgressItem.Init();
					this.progressItems.Add(uitreasureProgressItem);
				}
				uitreasureProgressItem.gameObject.SetActiveSafe(true);
			}
			this.Refresh();
			this.modelItem.gameObject.SetActiveSafe(true);
			string birthAnim = BoxUpgradeAnimation.GetBirthAnim(this.openData.initGrade);
			string idleAnim = BoxUpgradeAnimation.GetIdleAnim(this.openData.initGrade);
			this.modelItem.PlayAnimation(birthAnim, false);
			this.modelItem.AddAnimation(idleAnim, true, 0f);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.buttonUpgrade.onClick.RemoveListener(new UnityAction(this.OnClickUpgrade));
			for (int i = 0; i < this.progressItems.Count; i++)
			{
				this.progressItems[i].gameObject.SetActiveSafe(false);
			}
		}

		public override void OnDelete()
		{
			this.modelItem.DeInit();
			for (int i = 0; i < this.progressItems.Count; i++)
			{
				this.progressItems[i].DeInit();
			}
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void Refresh()
		{
			if (this.openData == null)
			{
				return;
			}
			string boxQualityInfo = DxxTools.UI.GetBoxQualityInfo(this.currentGrade);
			this.textGrade.text = Singleton<LanguageManager>.Instance.GetInfoByID(boxQualityInfo);
			this.textTips.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_treasure_upgrade");
			for (int i = 0; i < this.progressItems.Count; i++)
			{
				UITreasureProgressItem uitreasureProgressItem = this.progressItems[i];
				int num = -1;
				if (this.openData.upgradeProgress != null && i < this.openData.upgradeProgress.Count && i < this.currentProgress)
				{
					num = this.openData.upgradeProgress[i];
				}
				uitreasureProgressItem.SetData(num, i == this.currentProgress);
			}
		}

		private void ShowOpen()
		{
			this.textTips.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_treasure_continue");
		}

		private void OnClickUpgrade()
		{
			if (this.isPlayAni)
			{
				return;
			}
			if (this.openData == null || this.openData.upgradeProgress == null)
			{
				return;
			}
			this.isPlayAni = true;
			this.currentProgress++;
			if (this.currentProgress > this.openData.upgradeLimit)
			{
				if (this.openData.showRewards != null)
				{
					GameApp.Sound.PlayClip(637, 1f);
					string openAnim = BoxUpgradeAnimation.GetOpenAnim(this.currentGrade);
					this.modelItem.PlayAnimation(openAnim, false);
					float animationDuration = this.modelItem.GetAnimationDuration(openAnim);
					DelayCall.Instance.CallOnce((int)(animationDuration * 1000f), delegate
					{
						this.modelItem.gameObject.SetActiveSafe(false);
						DxxTools.UI.OpenRewardCommon(this.openData.showRewards, delegate
						{
							this.isPlayAni = false;
							this.textTips.text = "";
							this.OnCloseSelf();
						}, true);
					});
				}
				return;
			}
			List<int> upgradeProgress = this.openData.upgradeProgress;
			int num = 0;
			int num2 = this.currentProgress - 1;
			if (num2 < upgradeProgress.Count)
			{
				num = upgradeProgress[num2];
			}
			if (num > 0)
			{
				this.currentGrade++;
			}
			int num3 = 636;
			float num4;
			if (num > 0)
			{
				string clickUpAnim = BoxUpgradeAnimation.GetClickUpAnim(this.currentGrade - 1);
				this.modelItem.PlayAnimation(clickUpAnim, false);
				num4 = this.modelItem.GetAnimationDuration(clickUpAnim);
				num3 = 635;
			}
			else
			{
				string clickAnim = BoxUpgradeAnimation.GetClickAnim(this.currentGrade);
				this.modelItem.PlayAnimation(clickAnim, false);
				num4 = this.modelItem.GetAnimationDuration(clickAnim);
			}
			string idleAnim = BoxUpgradeAnimation.GetIdleAnim(this.currentGrade);
			this.modelItem.AddAnimation(idleAnim, true, 0f);
			GameApp.Sound.PlayClip(num3, 1f);
			DelayCall.Instance.CallOnce((int)(num4 * 1000f), delegate
			{
				this.isPlayAni = false;
				this.Refresh();
				if (this.currentProgress == this.openData.upgradeLimit)
				{
					this.ShowOpen();
				}
			});
		}

		private void OnCloseSelf()
		{
			GameApp.View.CloseView(ViewName.BoxUpgradeViewModule, null);
		}

		public CustomButton buttonUpgrade;

		public UISpineModelItem modelItem;

		public CustomText textGrade;

		public CustomText textTips;

		public GameObject layout;

		public GameObject copyItem;

		private BoxUpgradeViewModule.OpenData openData;

		private List<UITreasureProgressItem> progressItems = new List<UITreasureProgressItem>();

		private int currentProgress;

		private int currentGrade;

		private bool isPlayAni;

		public class OpenData
		{
			public int initGrade;

			public int upgradeLimit;

			public List<int> upgradeProgress;

			public RepeatedField<RewardDto> showRewards;
		}
	}
}
