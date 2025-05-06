using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.Mining;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class MiningTreasureUpgradeViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.miningDataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
			this.buttonUpgrade.onClick.AddListener(new UnityAction(this.OnClickUpgrade));
			this.copyItem.gameObject.SetActiveSafe(false);
			for (int i = 0; i < this.nodeDatas.Length; i++)
			{
				if (this.nodeDatas[i] != null)
				{
					GameObject node = this.nodeDatas[i].node;
					if (node)
					{
						node.SetActiveSafe(false);
					}
				}
			}
		}

		public override void OnOpen(object data)
		{
			this.treasureRes = GameApp.Table.GetManager().GetMining_oreRes(this.miningDataModule.MiningInfo.TreasureResId);
			if (this.treasureRes == null)
			{
				return;
			}
			GridDto treasureFirstGridDto = this.miningDataModule.GetTreasureFirstGridDto();
			if (treasureFirstGridDto == null)
			{
				return;
			}
			this.currentGrade = treasureFirstGridDto.Grade;
			this.currentProgress = 0;
			this.SendMsg();
			for (int i = 0; i < this.treasureRes.uptimes; i++)
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
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.buttonUpgrade.onClick.RemoveListener(new UnityAction(this.OnClickUpgrade));
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
			if (this.treasureRes == null)
			{
				return;
			}
			Mining_oreQuality mining_oreQuality = GameApp.Table.GetManager().GetMining_oreQuality(this.currentGrade);
			string text = ((mining_oreQuality != null) ? Singleton<LanguageManager>.Instance.GetInfoByID(mining_oreQuality.languageId) : "");
			this.textGrade.text = text;
			this.textTips.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_treasure_upgrade");
			RepeatedField<int> treasureUpGradeInfo = this.miningDataModule.MiningInfo.TreasureUpGradeInfo;
			for (int i = 0; i < this.progressItems.Count; i++)
			{
				UITreasureProgressItem uitreasureProgressItem = this.progressItems[i];
				int num = -1;
				if (treasureUpGradeInfo != null && i < treasureUpGradeInfo.Count && i < this.currentProgress)
				{
					num = treasureUpGradeInfo[i];
				}
				uitreasureProgressItem.SetData(num, i == this.currentProgress);
			}
			MiningTreasureUpgradeViewModule.NodeData node = this.GetNode(this.currentGrade);
			if (node != null && node.animator != null)
			{
				this.ShowNode(node);
				string normalAnim = MiningAnimation.GetNormalAnim(this.currentGrade);
				node.animator.Play(normalAnim, 0, 0f);
			}
		}

		private void ShowOpen()
		{
			this.textTips.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_treasure_continue");
		}

		private MiningTreasureUpgradeViewModule.NodeData GetNode(int quality)
		{
			for (int i = 0; i < this.nodeDatas.Length; i++)
			{
				MiningTreasureUpgradeViewModule.NodeData nodeData = this.nodeDatas[i];
				if (nodeData != null && nodeData.quality == quality)
				{
					return nodeData;
				}
			}
			return null;
		}

		private void ShowNode(MiningTreasureUpgradeViewModule.NodeData showNode)
		{
			for (int i = 0; i < this.nodeDatas.Length; i++)
			{
				MiningTreasureUpgradeViewModule.NodeData nodeData = this.nodeDatas[i];
				if (nodeData != null && !(nodeData.node == null))
				{
					if (nodeData.quality == showNode.quality)
					{
						nodeData.node.SetActiveSafe(true);
					}
					else
					{
						nodeData.node.SetActiveSafe(false);
					}
				}
			}
		}

		private void OnCloseSelf()
		{
			GameApp.View.CloseView(ViewName.MiningTreasureUpgradeViewModule, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Mining_GetAllReward, null);
		}

		private void OnClickUpgrade()
		{
			if (this.isMsg)
			{
				return;
			}
			if (this.isPlayAni)
			{
				return;
			}
			this.isPlayAni = true;
			this.currentProgress++;
			if (this.currentProgress > this.miningDataModule.MiningInfo.TreasureUpGradeInfo.Count)
			{
				if (this.showRewards != null)
				{
					GameApp.Sound.PlayClip(637, 1f);
					MiningTreasureUpgradeViewModule.NodeData node = this.GetNode(this.currentGrade);
					if (node == null)
					{
						return;
					}
					this.ShowNode(node);
					string openAnim = MiningAnimation.GetOpenAnim(this.currentGrade);
					float animationLength = DxxTools.Animator.GetAnimationLength(node.animator, openAnim);
					node.animator.Play(openAnim, 0, 0f);
					DelayCall.Instance.CallOnce((int)(animationLength * 1000f), delegate
					{
						DxxTools.UI.OpenRewardCommon(this.showRewards, delegate
						{
							this.isPlayAni = false;
							this.showRewards.Clear();
							this.textTips.text = "";
							this.OnCloseSelf();
						}, true);
					});
				}
				return;
			}
			RepeatedField<int> treasureUpGradeInfo = this.miningDataModule.MiningInfo.TreasureUpGradeInfo;
			int num = 0;
			int num2 = this.currentProgress - 1;
			if (num2 < treasureUpGradeInfo.Count)
			{
				num = treasureUpGradeInfo[num2];
			}
			if (num > 0)
			{
				this.currentGrade++;
			}
			MiningTreasureUpgradeViewModule.NodeData node2 = this.GetNode(this.currentGrade);
			if (node2 == null)
			{
				return;
			}
			this.ShowNode(node2);
			string clickAnim = MiningAnimation.GetClickAnim(this.currentGrade);
			float animationLength2 = DxxTools.Animator.GetAnimationLength(node2.animator, clickAnim);
			node2.animator.Play(clickAnim, 0, 0f);
			int num3 = ((num == 0) ? 636 : 635);
			GameApp.Sound.PlayClip(num3, 1f);
			DelayCall.Instance.CallOnce((int)(animationLength2 * 1000f), delegate
			{
				this.isPlayAni = false;
				this.Refresh();
				if (this.currentProgress == this.miningDataModule.MiningInfo.TreasureUpGradeInfo.Count)
				{
					this.ShowOpen();
				}
			});
		}

		private void SendMsg()
		{
			this.isMsg = true;
			NetworkUtils.Mining.DoMiningBoxUpgradeRewardRequest(delegate(bool result, MiningBoxUpgradeRewardResponse response)
			{
				if (result)
				{
					this.isMsg = false;
					if (response.CommonData.Reward != null && response.CommonData.Reward.Count > 0)
					{
						this.showRewards = response.CommonData.Reward;
					}
				}
			});
		}

		public CustomButton buttonUpgrade;

		public CustomText textGrade;

		public CustomText textTips;

		public GameObject layout;

		public GameObject copyItem;

		[SerializeField]
		private MiningTreasureUpgradeViewModule.NodeData[] nodeDatas;

		private MiningDataModule miningDataModule;

		private List<UITreasureProgressItem> progressItems = new List<UITreasureProgressItem>();

		private Mining_oreRes treasureRes;

		private RepeatedField<RewardDto> showRewards;

		private bool isPlayAni;

		private bool isMsg;

		private int currentProgress;

		private int currentGrade;

		[Serializable]
		public class NodeData
		{
			public int quality;

			public GameObject node;

			public Animator animator;
		}
	}
}
