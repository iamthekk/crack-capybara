using System;
using DG.Tweening;
using Dxx.Guild;
using Framework;
using Framework.Logic.Modules;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIMainGuildCreateGuildCtrl : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.iconCtrl.Init();
			Text text = this.inputName.placeholder.GetComponent<Text>();
			if (text != null)
			{
				text.text = GuildProxy.Language.GetInfoByID("400095");
			}
			text = this.inputSlogan.placeholder.GetComponent<Text>();
			if (text != null)
			{
				text.text = GuildProxy.Language.GetInfoByID("400096");
			}
			text = this.inputCondition.placeholder.GetComponent<Text>();
			if (text != null)
			{
				text.text = GuildProxy.Language.GetInfoByID("400097");
			}
			Guild_guildConst guildConstTable = GuildProxy.Table.GetGuildConstTable(108);
			Guild_guildConst guildConstTable2 = GuildProxy.Table.GetGuildConstTable(109);
			this.logoBgIndex = guildConstTable.TypeInt;
			this.logoIndex = guildConstTable2.TypeInt;
			this.buttonFreeJoin.OnClickButton = delegate(CustomChooseButton btn)
			{
				this.joinKind = GuildJoinKind.Free;
				this.SetButtonChoose();
			};
			this.buttonApplyJoin.OnClickButton = delegate(CustomChooseButton btn)
			{
				this.joinKind = GuildJoinKind.Conditional;
				this.SetButtonChoose();
			};
			this.buttonLanguage.onClick.AddListener(new UnityAction(this.OnClickOpenLanguage));
			Guild_guildConst guildConstTable3 = GuildProxy.Table.GetGuildConstTable(101);
			this.buttonCreateGuild.Init();
			this.buttonCreateGuild.SetData(2, guildConstTable3.TypeInt);
			this.buttonCreateGuild.onClick = delegate
			{
				if (this.CheckCreateEnabled())
				{
					GuildCreateData guildCreateData = new GuildCreateData();
					guildCreateData.GuildShowName = this.guildname;
					guildCreateData.GuildSlogan = this.slogan;
					guildCreateData.GuildLogo = this.logoIndex;
					guildCreateData.GuildLogoBG = this.logoBgIndex;
					guildCreateData.JoinKind = this.joinKind;
					guildCreateData.JoinCondition_Level = this.conditionNum;
					guildCreateData.Language = this.language;
					GuildNetUtil.Guild.DoRequest_CreateGuild(guildCreateData, delegate(bool result, GuildCreateResponse response)
					{
						if (result)
						{
							GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_Guild_ShowHall);
						}
					});
				}
			};
			this.buttonChangeIcon.onClick.AddListener(new UnityAction(this.OnClickOpenIcon));
		}

		protected override void GuildUI_OnShow()
		{
			base.gameObject.SetActiveSafe(true);
			this.inputName.characterLimit = GuildProxy.Table.NAME_LENGTH_MAX;
			this.inputName.onValidateInput = new InputField.OnValidateInput(this.OnValidateInputForInputName);
			this.inputName.onValueChanged.AddListener(new UnityAction<string>(this.OnNameChanged));
			this.inputSlogan.characterLimit = GuildProxy.Table.SLOGAN_LENGTH;
			this.inputSlogan.onValidateInput = new InputField.OnValidateInput(this.OnValidateInputForInputSlogan);
			this.inputSlogan.onValueChanged.AddListener(new UnityAction<string>(this.OnSloganChanged));
			this.inputCondition.onValueChanged.AddListener(new UnityAction<string>(this.OnConditionChanged));
			this.SetButtonChoose();
			this.language = GuildProxy.Language.GetCurrentLanguage();
			this.RefreshLanguage();
			this.OnNameChanged("");
			this.OnSloganChanged("");
		}

		protected override void GuildUI_OnClose()
		{
			this.inputName.onValidateInput = null;
			this.inputName.onValueChanged.RemoveListener(new UnityAction<string>(this.OnNameChanged));
			this.inputSlogan.onValidateInput = null;
			this.inputSlogan.onValueChanged.RemoveListener(new UnityAction<string>(this.OnSloganChanged));
			this.inputCondition.onValueChanged.RemoveListener(new UnityAction<string>(this.OnConditionChanged));
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.buttonLanguage != null)
			{
				this.buttonLanguage.onClick.RemoveListener(new UnityAction(this.OnClickOpenLanguage));
			}
			if (this.buttonChangeIcon != null)
			{
				this.buttonChangeIcon.onClick.RemoveListener(new UnityAction(this.OnClickOpenIcon));
			}
			if (this.iconCtrl != null)
			{
				this.iconCtrl.DeInit();
			}
		}

		private bool CheckCreateEnabled()
		{
			string infoByID = GuildProxy.Language.GetInfoByID("400119");
			if (string.IsNullOrWhiteSpace(this.guildname))
			{
				string infoByID2 = GuildProxy.Language.GetInfoByID1("400220", GuildProxy.Table.NAME_LENGTH_MAX);
				GuildProxy.UI.OpenUIPopCommonSimple(infoByID, infoByID2);
				return false;
			}
			if (GuildProxy.Language.CheckTextLength(this.guildname) > GuildProxy.Table.NAME_LENGTH_MAX)
			{
				string infoByID3 = GuildProxy.Language.GetInfoByID1("400064", GuildProxy.Table.NAME_LENGTH_MAX);
				GuildProxy.UI.OpenUIPopCommonSimple(infoByID, infoByID3);
				return false;
			}
			if (GuildProxy.Language.CheckTextLength(this.slogan) > GuildProxy.Table.SLOGAN_LENGTH)
			{
				string infoByID4 = GuildProxy.Language.GetInfoByID1("400065", GuildProxy.Table.SLOGAN_LENGTH);
				GuildProxy.UI.OpenUIPopCommonSimple(infoByID, infoByID4);
				return false;
			}
			Guild_guildConst guildConstTable = GuildProxy.Table.GetGuildConstTable(101);
			if (GuildProxy.GameUser.MyDiamond() < guildConstTable.TypeInt)
			{
				GameApp.View.ShowItemNotEnoughTip(2, true);
				return false;
			}
			return true;
		}

		private char OnValidateInputForInputName(string text, int charindex, char addedchar)
		{
			return GuildProxy.Language.CheckValidateInput(text, charindex, addedchar, GuildProxy.Table.NAME_LENGTH_MAX);
		}

		private char OnValidateInputForInputSlogan(string text, int charindex, char addedchar)
		{
			return GuildProxy.Language.CheckValidateInput(text, charindex, addedchar, GuildProxy.Table.SLOGAN_LENGTH);
		}

		public void OnNameChanged(string text)
		{
			this.guildname = text;
			this.inputName.text = text;
			int num = GuildProxy.Language.CheckTextLength(text);
			string infoByID = GuildProxy.Language.GetInfoByID1("400033", GuildProxy.Table.NAME_LENGTH_MAX - num);
			this.textNameLength.text = infoByID;
			if (GuildProxy.Table.NAME_LENGTH_MAX - num <= 0)
			{
				this.textNameLength.color = Color.red;
				TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.textNameLength.transform, Vector3.one * 1.5f, 0.1f), 1), delegate
				{
					this.textNameLength.transform.localScale = Vector3.one;
				});
				return;
			}
			this.textNameLength.color = this.defaultTipColor;
		}

		public void OnSloganChanged(string text)
		{
			this.slogan = text;
			this.inputSlogan.text = text;
			int num = GuildProxy.Language.CheckTextLength(text);
			string infoByID = GuildProxy.Language.GetInfoByID1("400033", GuildProxy.Table.SLOGAN_LENGTH - num);
			this.textSloganLength.text = infoByID;
			if (GuildProxy.Table.SLOGAN_LENGTH - num <= 0)
			{
				this.textSloganLength.color = Color.red;
				TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.textSloganLength.transform, Vector3.one * 1.5f, 0.1f), 1), delegate
				{
					this.textSloganLength.transform.localScale = Vector3.one;
				});
				return;
			}
			this.textSloganLength.color = this.defaultTipColor;
		}

		private void OnConditionChanged(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				this.conditionNum = 0;
				this.inputCondition.SetTextWithoutNotify(string.Empty);
				return;
			}
			int num;
			if (int.TryParse(text, out num))
			{
				this.conditionNum = Mathf.Clamp(num, 0, int.MaxValue);
				this.inputCondition.SetTextWithoutNotify(this.conditionNum.ToString());
				return;
			}
			string text2 = ((this.conditionNum > 0) ? this.conditionNum.ToString() : string.Empty);
			this.inputCondition.SetTextWithoutNotify(text2);
		}

		private void OnClickOpenLanguage()
		{
			GuildProxy.UI.OpenUILanguage(this.language, new Action<LanguageType>(this.OnSwitchLanugage));
		}

		private void OnClickOpenIcon()
		{
			GuildProxy.UI.OpenUIGuildIconSet(new GuildIconSetData
			{
				defaultIconId = this.logoIndex,
				callback = delegate(int iconId)
				{
					this.logoIndex = iconId;
					this.iconCtrl.SetIcon(iconId);
				}
			});
		}

		private void SetButtonChoose()
		{
			this.buttonFreeJoin.SetSelect(this.joinKind == GuildJoinKind.Free);
			this.buttonApplyJoin.SetSelect(this.joinKind == GuildJoinKind.Conditional);
		}

		private void OnSwitchLanugage(LanguageType ltype)
		{
			this.language = ltype;
			this.RefreshLanguage();
			GuildProxy.UI.CloseUILanguage();
		}

		private void RefreshLanguage()
		{
			this.textLanguage.text = GuildProxy.Language.GetLanguageNameString(this.language);
		}

		public void Hide()
		{
			base.gameObject.SetActiveSafe(false);
		}

		[Header("公会图标")]
		public UIGuildIcon iconCtrl;

		public CustomButton buttonChangeIcon;

		[Header("公会名称")]
		public InputField inputName;

		public CustomText textNameLength;

		[Header("公会宣言")]
		public InputField inputSlogan;

		public CustomText textSloganLength;

		[Header("加入类型")]
		public CustomChooseButton buttonFreeJoin;

		public CustomChooseButton buttonApplyJoin;

		[Header("等级要求")]
		public InputField inputCondition;

		[Header("公会语言")]
		public CustomButton buttonLanguage;

		public CustomText textLanguage;

		[Header("其他")]
		public UIGuildCurrencyButton buttonCreateGuild;

		private string guildname = "";

		private string slogan = "";

		private int logoIndex;

		private int logoBgIndex;

		private GuildJoinKind joinKind;

		private int conditionNum;

		private LanguageType language = 2;

		private Color defaultTipColor = new Color(0.4823529f, 0.4823529f, 0.4823529f);
	}
}
