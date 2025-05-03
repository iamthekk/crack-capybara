using System;
using DG.Tweening;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class GuildAnnouncementModifyViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.popCommon.OnClick = new Action<int>(this.OnPopClick);
			this.Button_Cancle.onClick.AddListener(new UnityAction(this.CloseSelfView));
			this.Button_OK.onClick.AddListener(new UnityAction(this.OnClickOK));
			this.Input.characterLimit = GuildProxy.Table.SLOGAN_LENGTH;
			this.Input.onValueChanged.AddListener(new UnityAction<string>(this.OnInputChange));
			this.Input.onValidateInput = new InputField.OnValidateInput(this.OnValidateInput);
			this.Text_InputPlaceHolder.text = GuildProxy.Language.GetInfoByID("400100");
		}

		protected override void OnViewOpen(object data)
		{
			GuildShareData guildData = base.SDK.GuildInfo.GuildData;
			this.Input.text = ((guildData != null) ? guildData.GuildNotice : "");
			int num = GuildProxy.Table.NOTIC_LENGTH - GuildProxy.Language.CheckTextLength(guildData.GuildNotice);
			this.Text_Count.text = GuildProxy.Language.GetInfoByID1("400033", num);
		}

		protected override void OnViewDelete()
		{
			if (this.Button_Cancle != null)
			{
				this.Button_Cancle.onClick.RemoveListener(new UnityAction(this.CloseSelfView));
			}
			if (this.Button_OK != null)
			{
				this.Button_OK.onClick.RemoveListener(new UnityAction(this.OnClickOK));
			}
			if (this.Input != null)
			{
				this.Input.onValueChanged.RemoveListener(new UnityAction<string>(this.OnInputChange));
				this.Input.onValidateInput = null;
			}
		}

		private char OnValidateInput(string text, int charindex, char addedchar)
		{
			return GuildProxy.Language.CheckValidateInput(text, charindex, addedchar, GuildProxy.Table.SLOGAN_LENGTH);
		}

		private void OnInputChange(string text)
		{
			int num = GuildProxy.Language.CheckTextLength(text);
			int num2 = GuildProxy.Table.NOTIC_LENGTH - num;
			string infoByID = GuildProxy.Language.GetInfoByID1("400033", num2);
			this.Text_Count.text = infoByID;
			if (num2 <= 0)
			{
				this.Text_Count.color = Color.red;
				TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.Text_Count.transform, Vector3.one * 1.5f, 0.1f), 1), delegate
				{
					this.Text_Count.transform.localScale = Vector3.one;
				});
				return;
			}
			this.Text_Count.color = Color.black;
		}

		private void OnClickOK()
		{
			GuildShareData guildData = base.SDK.GuildInfo.GuildData;
			if (this.Input.text == guildData.GuildNotice)
			{
				this.CloseSelfView();
				return;
			}
			int num = GuildProxy.Language.CheckTextLength(this.Input.text);
			if (GuildProxy.Table.NOTIC_LENGTH - num < 0)
			{
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID1("400064", GuildProxy.Table.NOTIC_LENGTH));
				return;
			}
			GuildCreateData guildCreateData = new GuildCreateData();
			guildCreateData.CloneFromShareData(base.SDK.GuildInfo.GuildData);
			guildCreateData.GuildNotice = this.Input.text;
			GuildNetUtil.Guild.DoRequest_ModifyGuildInfo(guildCreateData, delegate(bool result, GuildModifyResponse resp)
			{
				if (result)
				{
					this.CloseSelfView();
				}
			});
		}

		private void CloseSelfView()
		{
			GameApp.View.CloseView(ViewName.GuildAnnouncementModifyViewModule, null);
		}

		private void OnPopClick(int obj)
		{
			this.CloseSelfView();
		}

		public UIGuildPopCommon popCommon;

		public InputField Input;

		public CustomButton Button_OK;

		public CustomButton Button_Cancle;

		public CustomText Text_Count;

		public Text Text_InputPlaceHolder;
	}
}
