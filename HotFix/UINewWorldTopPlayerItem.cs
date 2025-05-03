using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.LeaderBoard;
using Proto.NewWorld;
using UnityEngine.Events;

namespace HotFix
{
	public class UINewWorldTopPlayerItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.dataModule = GameApp.Data.GetDataModule(DataName.NewWorldDataModule);
			if (this.rankItem)
			{
				this.rankItem.Init();
			}
			this.buttonLike.onClick.AddListener(new UnityAction(this.OnClickLike));
		}

		protected override void OnDeInit()
		{
			if (this.rankItem)
			{
				this.rankItem.DeInit();
			}
			this.buttonLike.onClick.RemoveListener(new UnityAction(this.OnClickLike));
		}

		public void OnShow()
		{
			if (this.rankItem)
			{
				this.rankItem.OnShow();
			}
		}

		public void OnHide()
		{
			if (this.rankItem)
			{
				this.rankItem.OnHide();
			}
		}

		public void SetData(int rankId, RankUserDto dto)
		{
			this.rank = rankId;
			this.rankUser = dto;
			LoginDataModule loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.likeCount = this.dataModule.GetLikeCount(this.rank);
			this.isLike = this.dataModule.GetLikeState(this.rank) > 0;
			if (dto != null && dto.UserInfo.UserId == loginDataModule.userId)
			{
				this.isLike = true;
			}
			this.buttonLike.gameObject.SetActiveSafe(dto != null);
			if (this.rankItem)
			{
				this.rankItem.SetData(RankType.NewWorld, this.rankUser, string.Format("NewWorld_TopPlayer_{0}", rankId));
				this.rankItem.ShowCustomNewWorld();
			}
			this.RefreshLike();
		}

		private void RefreshLike()
		{
			this.textLikeNum.text = this.likeCount.ToString();
			this.buttonLike.SetSelect(this.isLike);
		}

		private void OnClickLike()
		{
			if (this.isLike)
			{
				return;
			}
			NetworkUtils.NewWorld.DoLikeRequest(this.rank, delegate(bool isOk, LikeResponse response)
			{
				if (isOk && response != null)
				{
					this.isLike = true;
					this.likeCount = this.dataModule.GetLikeCount(this.rank);
					this.RefreshLike();
					if (response.CommonData.Reward != null && response.CommonData.Reward.Count > 0)
					{
						DxxTools.UI.OpenRewardCommon(response.CommonData.Reward, null, true);
					}
				}
			});
		}

		public UIRankTopItem rankItem;

		public CustomChooseButton buttonLike;

		public CustomText textLikeNum;

		private int rank;

		private int likeCount;

		private bool isLike;

		private RankUserDto rankUser;

		private NewWorldDataModule dataModule;
	}
}
