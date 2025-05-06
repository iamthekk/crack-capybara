using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class GameEventUIButtonData
	{
		public int index { get; set; }

		public GameEventButtonType buttonType { get; private set; }

		public GameEventButtonColorEnum ButtonColorType { get; private set; }

		public string languageId { get; private set; }

		public string tipLanguageId { get; private set; }

		public int needId { get; private set; }

		public int param { get; private set; }

		public bool isUndoFunction { get; private set; }

		public string undoTip { get; private set; }

		public bool isBattleButton { get; private set; }

		public GameEventUIButtonData(int index, GameEventDataSelect btnData, bool isUndo, string undoTip)
		{
			this.index = index;
			this.buttonType = btnData.buttonType;
			this.ButtonColorType = btnData.buttonColorType;
			this.languageId = btnData.languageId;
			this.needId = btnData.needId;
			this.param = btnData.buttonParam;
			this.tipLanguageId = btnData.tipLanguageId;
			this.isBattleButton = btnData.IsBattleButton();
			this.isUndoFunction = isUndo;
			this.undoTip = undoTip;
			this.attList = btnData.GetMyFunctionAttr();
			this.itemList = btnData.GetMyFunctionItems();
			this.skillList = btnData.GetMyFunctionSkills();
			this.infoList = btnData.GetMyFunctionInfos();
			btnData.GetAttParam();
		}

		public string GetInfo()
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID(this.languageId);
		}

		public string GetInfoTip()
		{
			return Singleton<LanguageManager>.Instance.GetInfoByID(this.tipLanguageId);
		}

		public List<AttributeTypeData> GetAttributeList()
		{
			return NodeAttParam.GetAttParamList(this.attList);
		}

		public List<ItemTypeData> GetItemList()
		{
			return NodeItemParam.GetItemParamList(this.itemList);
		}

		public List<SkillTypeData> GetSkillList()
		{
			return NodeSkillParam.GetSkillParamList(this.skillList);
		}

		public List<InfoTypeData> GetInfoList()
		{
			return GameEventUIData.GetInfoParamList(this.infoList);
		}

		public string GetTGAString()
		{
			string text = Singleton<LanguageManager>.Instance.GetInfoByID(2, this.languageId);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(2, this.tipLanguageId);
			string text2 = "";
			switch (this.buttonType)
			{
			case GameEventButtonType.Normal:
			{
				List<AttributeTypeDataBase> list = new List<AttributeTypeDataBase>();
				List<AttributeTypeData> attributeList = this.GetAttributeList();
				if (attributeList != null)
				{
					list.AddRange(attributeList);
				}
				List<ItemTypeData> list2 = this.GetItemList();
				if (list2 != null)
				{
					list.AddRange(list2);
				}
				List<SkillTypeData> list3 = this.GetSkillList();
				if (list3 != null)
				{
					list.AddRange(list3);
				}
				List<InfoTypeData> list4 = this.GetInfoList();
				if (list4 != null)
				{
					list.AddRange(list4);
				}
				if (list.Count <= 0)
				{
					goto IL_01D9;
				}
				using (List<AttributeTypeDataBase>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AttributeTypeDataBase attributeTypeDataBase = enumerator.Current;
						if (!string.IsNullOrEmpty(attributeTypeDataBase.m_tgaValue))
						{
							text2 = text2 + "(" + attributeTypeDataBase.m_tgaValue + ")";
						}
					}
					goto IL_01D9;
				}
				break;
			}
			case GameEventButtonType.Buy:
				break;
			case GameEventButtonType.EventItemBuy:
			{
				Chapter_eventItem elementById = GameApp.Table.GetManager().GetChapter_eventItemModelInstance().GetElementById(this.needId);
				if (elementById != null)
				{
					string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID(2, elementById.languageId);
					text2 = Singleton<LanguageManager>.Instance.GetInfoByID(2, "UIGameEvent_162", new object[] { infoByID2 });
					goto IL_01D9;
				}
				goto IL_01D9;
			}
			case GameEventButtonType.NeedEventItem:
			{
				Chapter_eventItem elementById2 = GameApp.Table.GetManager().GetChapter_eventItemModelInstance().GetElementById(this.needId);
				if (elementById2 != null)
				{
					string infoByID3 = Singleton<LanguageManager>.Instance.GetInfoByID(2, elementById2.languageId);
					text2 = Singleton<LanguageManager>.Instance.GetInfoByID(2, "UIGameEvent_163", new object[] { infoByID3 });
					goto IL_01D9;
				}
				goto IL_01D9;
			}
			default:
				goto IL_01D9;
			}
			text2 = Singleton<LanguageManager>.Instance.GetInfoByID(2, "UIGameEvent_CostFood", new object[] { this.param });
			IL_01D9:
			if (!string.IsNullOrEmpty(infoByID))
			{
				text = text + "(" + infoByID + ")";
			}
			if (!string.IsNullOrEmpty(text2))
			{
				text += text2;
			}
			return text;
		}

		private List<NodeAttParam> attList;

		private List<NodeItemParam> itemList;

		private List<NodeSkillParam> skillList;

		private List<string> infoList;
	}
}
