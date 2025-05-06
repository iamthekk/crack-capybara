using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.CrossArenaRecordUI
{
	public class CrossArenaRecordItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Avatar.Init();
			this.Avatar.OnClick = new Action<UIAvatarCtrl>(this.OnClickShowUser);
			this.Button_Detail.onClick.AddListener(new UnityAction(this.OnGetBattleRecord));
			this.Obj_ATK.SetActive(false);
			this.Obj_DEF.SetActive(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			UIAvatarCtrl avatar = this.Avatar;
			if (avatar != null)
			{
				avatar.DeInit();
			}
			CustomButton button_Detail = this.Button_Detail;
			if (button_Detail == null)
			{
				return;
			}
			button_Detail.onClick.RemoveListener(new UnityAction(this.OnGetBattleRecord));
		}

		public void SetData(CrossArenaChallengeRecord data)
		{
			this.mData = data;
		}

		public void RefreshUI()
		{
			if (this.mData == null || this.mData.UserInfo == null)
			{
				this.RefreshAsNull();
				return;
			}
			this.Avatar.RefreshData((int)this.mData.UserInfo.Avatar, (int)this.mData.UserInfo.AvatarFrame);
			this.Text_Name.text = this.mData.GetNick();
			this.Text_Power.text = DxxTools.FormatNumber((long)this.mData.UserInfo.Power);
			if (this.mData.ScoreChg > 0)
			{
				string text = "+" + this.mData.ScoreChg.ToString();
				this.Text_Score.text = "<color=#D6F54C>" + text + "</color>";
			}
			else
			{
				string text2 = this.mData.ScoreChg.ToString();
				this.Text_Score.text = "<color=#FF604C>" + text2 + "</color>";
			}
			this.Obj_ATK.SetActive(this.mData.IsAttack);
			this.Obj_DEF.SetActive(!this.mData.IsAttack);
		}

		private void RefreshAsNull()
		{
		}

		private void OnGetBattleRecord()
		{
			if (this.mData == null || base.gameObject == null || !base.gameObject.activeSelf)
			{
				return;
			}
			Action<CrossArenaChallengeRecord> onJumpToBattle = this.OnJumpToBattle;
			if (onJumpToBattle == null)
			{
				return;
			}
			onJumpToBattle(this.mData);
		}

		private void OnClickShowUser(UIAvatarCtrl ctrl)
		{
			if (this.mData == null || this.mData.UserInfo == null)
			{
				return;
			}
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(this.mData.UserInfo.UserId);
			if (GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.PlayerInformationViewModule, openData, 1, null, null);
		}

		public UIAvatarCtrl Avatar;

		public CustomText Text_Name;

		public CustomText Text_Power;

		public CustomText Text_Score;

		public CustomButton Button_Detail;

		public GameObject Obj_ATK;

		public GameObject Obj_DEF;

		public Action<CrossArenaChallengeRecord> OnJumpToBattle;

		private CrossArenaChallengeRecord mData;
	}
}
