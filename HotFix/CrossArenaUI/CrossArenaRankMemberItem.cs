using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.CrossArenaUI
{
	public class CrossArenaRankMemberItem : CustomBehaviour
	{
		public int Rank
		{
			get
			{
				return this.mIndex + 1;
			}
		}

		public CrossArenaRankMember Data
		{
			get
			{
				return this.mData;
			}
		}

		protected override void OnInit()
		{
			this.ObjRank1.SetActive(false);
			this.ObjRank2.SetActive(false);
			this.ObjRank3.SetActive(false);
			this.HeadUI.Init();
			this.HeadUI.OnClick = new Action<UIAvatarCtrl>(this.OnClickHead);
		}

		private void OnClickHead(UIAvatarCtrl ctrl)
		{
			if (this.mData == null)
			{
				return;
			}
			Action<long> onShowPlayerInfo = this.OnShowPlayerInfo;
			if (onShowPlayerInfo == null)
			{
				return;
			}
			onShowPlayerInfo(this.mData.UserID);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			UIAvatarCtrl headUI = this.HeadUI;
			if (headUI == null)
			{
				return;
			}
			headUI.DeInit();
		}

		public void SetData(int index, CrossArenaRankMember data)
		{
			this.mIndex = index;
			this.mData = data;
		}

		public void RefreshUI()
		{
			if (base.gameObject == null || this.mData == null)
			{
				return;
			}
			int rank = this.Rank;
			this.ObjRank1.SetActive(rank == 1);
			this.ObjRank2.SetActive(rank == 2);
			this.ObjRank3.SetActive(rank == 3);
			if (this.Rank <= 3)
			{
				this.Text_Rank.text = "";
			}
			else
			{
				this.Text_Rank.text = rank.ToString();
			}
			this.Text_Score.text = this.mData.Score.ToString();
			this.Text_Name.text = this.mData.GetNick();
			this.Text_Power.text = DxxTools.FormatNumber(this.mData.Power);
			this.HeadUI.RefreshData(this.mData.Avatar, this.mData.AvatarFrame);
		}

		public void RefreshUIAsMine()
		{
			if (base.gameObject == null || this.mData == null)
			{
				return;
			}
			this.RefreshUI();
		}

		public GameObject ObjRank1;

		public GameObject ObjRank2;

		public GameObject ObjRank3;

		public CustomText Text_Rank;

		public CustomText Text_Name;

		public CustomText Text_Score;

		public CustomText Text_Power;

		public UIAvatarCtrl HeadUI;

		private int mIndex;

		private CrossArenaRankMember mData;

		public bool HasShowAnimation;

		public Action<long> OnShowPlayerInfo;
	}
}
