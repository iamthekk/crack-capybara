using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI.GuildDetailInfoUI
{
	public class GuildDetailInfo_Info : GuildDetailInfo_Base
	{
		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.GuildIcon.Init();
			this.Button_ChangeIcon.onClick.AddListener(new UnityAction(this.OnChangeIcon));
			this.Input_Name.characterLimit = GuildProxy.Table.NAME_LENGTH_MAX;
			this.Input_Name.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
			this.Input_Name.onValidateInput = new InputField.OnValidateInput(this.OnValidateInput);
		}

		private void OnChangeIcon()
		{
			GuildProxy.UI.OpenUIGuildIconSet(new GuildIconSetData
			{
				defaultIconId = base.CreateData.GuildLogo,
				callback = delegate(int iconId)
				{
					base.CreateData.GuildLogo = iconId;
					this.GuildIcon.SetIcon(base.CreateData.GuildLogo);
				}
			});
		}

		protected override void GuildUI_OnUnInit()
		{
			base.GuildUI_OnUnInit();
			if (this.Button_ChangeIcon != null)
			{
				this.Button_ChangeIcon.onClick.RemoveListener(new UnityAction(this.OnChangeIcon));
			}
			this.GuildIcon.DeInit();
			this.Input_Name.onValueChanged.RemoveListener(new UnityAction<string>(this.OnValueChanged));
			this.Input_Name.onValidateInput = null;
		}

		public override void RefreshUI(GuildShareData sharedata, GuildShareDetailData detaildata)
		{
			this.mGuildData = sharedata;
			this.GuildIcon.SetIcon(sharedata.GuildIcon);
			this.Text_Name.text = GuildProxy.Language.GetInfoByID("400040");
			this.Input_Name.SetTextWithoutNotify(sharedata.GuildShowName);
			int typeInt = GuildProxy.Table.GetGuildConstTable(102).TypeInt;
			this.Text_Cost.text = GuildProxy.Language.GetInfoByID1("400081", typeInt);
			this.CalcNameLength();
		}

		private void CalcNameLength()
		{
			int num = GuildProxy.Language.CheckTextLength(this.Input_Name.text);
			string infoByID = GuildProxy.Language.GetInfoByID1("400033", GuildProxy.Table.NAME_LENGTH_MAX - num);
			this.Text_NameLength.text = infoByID;
			if (GuildProxy.Table.NAME_LENGTH_MAX - num <= 0)
			{
				this.Text_NameLength.color = Color.red;
				return;
			}
			this.Text_NameLength.color = Color.gray;
		}

		private char OnValidateInput(string text, int charIndex, char addedChar)
		{
			return GuildProxy.Language.CheckValidateInput(text, charIndex, addedChar, GuildProxy.Table.NAME_LENGTH_MAX);
		}

		private void OnValueChanged(string text)
		{
			base.CreateData.GuildShowName = this.Input_Name.text;
			this.CalcNameLength();
			Action onNameChanged = this.OnNameChanged;
			if (onNameChanged == null)
			{
				return;
			}
			onNameChanged();
		}

		[SerializeField]
		private UIGuildIcon GuildIcon;

		[SerializeField]
		private CustomButton Button_ChangeIcon;

		[SerializeField]
		private CustomText Text_Name;

		[SerializeField]
		private InputField Input_Name;

		[SerializeField]
		private CustomText Text_NameLength;

		[SerializeField]
		private CustomText Text_Cost;

		public Action OnNameChanged;

		private GuildShareData mGuildData;
	}
}
