using System;
using Dxx.Guild;
using Framework.Logic.UI;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class GuildInfoPopUI_Rename : GuildInfoPopUI_Base
	{
		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("4063");
			this.Input_Name.placeholder.gameObject.GetComponent<Text>().text = infoByID;
			this.Input_Name.characterLimit = GuildProxy.Table.NAME_LENGTH_MAX;
			this.Input_Name.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
			this.Input_Name.onValidateInput = new InputField.OnValidateInput(this.OnValidateInput);
			this.mCreateData = new GuildCreateData();
			this.mCreateData.CloneFromShareData(base.SDK.GuildInfo.GuildData);
			this.Button_Confirm.onClick.AddListener(new UnityAction(this.OnConfirm));
			this.Button_Cancel.onClick.AddListener(new UnityAction(this.OnCancel));
			this.RefreshUI();
		}

		private void OnConfirm()
		{
			GuildNetUtil.Guild.DoRequest_ModifyGuildInfo(this.mCreateData, delegate(bool result, GuildModifyResponse resp)
			{
				if (result)
				{
					this.OnCancel();
				}
			});
		}

		private void OnCancel()
		{
			this.GuildUI_OnUnInit();
		}

		public override void RefreshUI()
		{
			this.mGuildData = GuildSDKManager.Instance.GuildInfo.GuildData;
			this.Input_Name.SetTextWithoutNotify(this.mGuildData.GuildShowName);
			int typeInt = GuildProxy.Table.GetGuildConstTable(102).TypeInt;
			this.Text_Cost.text = typeInt.ToString();
			this.CalcNameLength();
		}

		protected override void GuildUI_OnUnInit()
		{
			base.GuildUI_OnUnInit();
			this.Input_Name.onValueChanged.RemoveListener(new UnityAction<string>(this.OnValueChanged));
			this.Input_Name.onValidateInput = null;
			this.Button_Confirm.onClick.RemoveListener(new UnityAction(this.OnConfirm));
			base.gameObject.SetActiveSafe(false);
			this.Button_Cancel.onClick.RemoveListener(new UnityAction(this.OnCancel));
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

		private void OnValueChanged(string text)
		{
			this.mCreateData.GuildShowName = this.Input_Name.text;
			this.CalcNameLength();
		}

		private char OnValidateInput(string text, int charIndex, char addedChar)
		{
			return GuildProxy.Language.CheckValidateInput(text, charIndex, addedChar, GuildProxy.Table.NAME_LENGTH_MAX);
		}

		[SerializeField]
		private InputField Input_Name;

		[SerializeField]
		private CustomText Text_NameLength;

		[SerializeField]
		private CustomText Text_Cost;

		[SerializeField]
		private CustomButton Button_Confirm;

		[SerializeField]
		private CustomButton Button_Cancel;

		private GuildShareData mGuildData;

		private GuildCreateData mCreateData;
	}
}
