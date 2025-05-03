using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.Tower;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class TowerLayerTopViewCtr : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.levelRewardButton.onClick.AddListener(new UnityAction(this.ClaimLevelReward));
			this.towerDataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
			this.spineModelItem.Init();
		}

		protected override void OnDeInit()
		{
			Sequence sequence = this.rewardAnimDelay;
			if (sequence != null)
			{
				TweenExtensions.Kill(sequence, false);
			}
			this.rewardAnimDelay = null;
			this.towerDataModule = null;
			this.spineModelItem.DeInit();
		}

		public void RefreshData(TowerChallenge_Tower towerConfigVal, bool isCurShow)
		{
			this.towerConfig = towerConfigVal;
			bool flag = this.towerDataModule.CheckTowerRewardIsClaimed(towerConfigVal);
			this.levelRewardButton.enabled = !flag && isCurShow;
			if (flag)
			{
				this.spineModelItem.PlayAnimation("Open_Idle", true);
			}
			else
			{
				this.spineModelItem.PlayAnimation("Idle", true);
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(towerConfigVal.name);
			this.textTowerName.text = infoByID;
		}

		private void ClaimLevelReward()
		{
			if (this.isClaimReward)
			{
				return;
			}
			this.isClaimReward = true;
			NetworkUtils.Tower.TowerRewardRequest(this.towerConfig.id, delegate(bool res, TowerRewardResponse response)
			{
				if (!res)
				{
					this.isClaimReward = false;
					return;
				}
				float animationDuration = this.spineModelItem.GetAnimationDuration("Open");
				this.spineModelItem.PlayAnimation("Open", false);
				Sequence seq = DOTween.Sequence();
				TweenSettingsExtensions.AppendInterval(seq, animationDuration);
				Action <>9__2;
				TweenSettingsExtensions.AppendCallback(seq, delegate
				{
					EventArgsSetCurTowerLevelIdData instance = Singleton<EventArgsSetCurTowerLevelIdData>.Instance;
					instance.CompleteTowerLevelId = this.towerConfig.level[this.towerConfig.level.Length - 1];
					instance.ClaimedRewardTowerId = this.towerConfig.id;
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_TowerDataMoudule_SetCurTowerLevelIdData, instance);
					RepeatedField<RewardDto> reward = response.CommonData.Reward;
					Action action;
					if ((action = <>9__2) == null)
					{
						action = (<>9__2 = delegate
						{
							seq = DOTween.Sequence();
							TweenSettingsExtensions.AppendInterval(seq, 1.2f);
							TweenSettingsExtensions.AppendCallback(seq, delegate
							{
								GameApp.Event.DispatchNow(this, LocalMessageName.CC_TowerView_ToNextTower, null);
								this.isClaimReward = false;
							});
						});
					}
					DxxTools.UI.OpenRewardCommon(reward, action, true);
				});
			});
		}

		[SerializeField]
		private CustomButton levelRewardButton;

		[SerializeField]
		private UISpineModelItem spineModelItem;

		[SerializeField]
		private GameObject layerTitleObj;

		[SerializeField]
		private CustomText textTowerName;

		private TowerChallenge_Tower towerConfig;

		private int levelId;

		private Sequence rewardAnimDelay;

		private TowerDataModule towerDataModule;

		private bool isClaimReward;
	}
}
