using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildRacePositionSetItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.SetAsSelect(false);
			this.UserCtrl.Init();
			this.Button.onClick.AddListener(new UnityAction(this.OnClickSelectThis));
		}

		protected override void GuildUI_OnUnInit()
		{
			CustomButton button = this.Button;
			if (button != null)
			{
				button.onClick.RemoveListener(new UnityAction(this.OnClickSelectThis));
			}
			UIGuildHead userCtrl = this.UserCtrl;
			if (userCtrl == null)
			{
				return;
			}
			userCtrl.DeInit();
		}

		public void SetData(GuildRacePositionSetItemData data)
		{
			this.Data = data;
		}

		public void RefreshUI()
		{
			GuildRaceBattlePosition position = this.Data.User.Position;
			bool isInBattle = this.Data.IsInBattle;
			bool isAPEngough = this.Data.IsAPEngough;
			bool isSpecailPos = this.Data.IsSpecailPos;
			this.Image_BG1.SetActive(isAPEngough && isSpecailPos);
			this.Image_BG2.SetActive(isAPEngough && !isSpecailPos);
			this.Image_BG3.SetActive(!isAPEngough);
			if (isInBattle)
			{
				this.Image_Flag.SetActive(true);
				this.Text_Flag.gameObject.SetActive(true);
				this.Text_Flag.text = this.Data.GetSpecailPosName();
			}
			else
			{
				this.Image_Flag.SetActive(false);
				this.Text_Flag.gameObject.SetActive(false);
			}
			this.Image_InBattle.SetActive(this.Data.IsInBattle);
			this.Text_AP.text = GuildProxy.Language.GetInfoByID1("400417", this.Data.User.ActivityPoint);
			this.Text_Power.text = GuildProxy.Language.GetInfoByID1("400416", GuildProxy.Language.FormatNumber((long)this.Data.User.Power));
			this.Text_Name.text = this.Data.User.UserData.GetNick();
			this.UserCtrl.Refresh(this.Data.User.UserData.Avatar, this.Data.User.UserData.AvatarFrame);
			this.UserCtrl.SetDefaultClick(this.Data.User.UserData.UserID);
		}

		private void OnClickSelectThis()
		{
			if (!this.Data.IsAPEngough)
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID_LogError(400510));
				return;
			}
			Action<GuildRacePositionSetItem> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(this);
		}

		public void SetAsSelect(bool select)
		{
			this.Image_Select.SetActive(select);
		}

		public GameObject Image_BG1;

		public GameObject Image_BG2;

		public GameObject Image_BG3;

		public GameObject Image_Select;

		public GameObject Image_Flag;

		public GameObject Image_InBattle;

		public CustomText Text_Flag;

		public CustomText Text_AP;

		public CustomText Text_Name;

		public CustomText Text_Power;

		public UIGuildHead UserCtrl;

		public CustomButton Button;

		public GuildRacePositionSetItemData Data;

		public Action<GuildRacePositionSetItem> OnClick;
	}
}
