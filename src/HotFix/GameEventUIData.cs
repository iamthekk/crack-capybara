using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class GameEventUIData
	{
		public int eventResId { get; private set; }

		public int stage { get; private set; }

		public string languageId { get; private set; }

		public object[] languageParams { get; private set; }

		public string titleId { get; private set; }

		public string title
		{
			get
			{
				if (string.IsNullOrEmpty(this.titleId))
				{
					return "";
				}
				return Singleton<LanguageManager>.Instance.GetInfoByID(this.titleId);
			}
		}

		public string summaryId { get; private set; }

		public string summary
		{
			get
			{
				if (string.IsNullOrEmpty(this.summaryId))
				{
					return "";
				}
				return Singleton<LanguageManager>.Instance.GetInfoByID(this.summaryId);
			}
		}

		public string info
		{
			get
			{
				if (string.IsNullOrEmpty(this.languageId))
				{
					return "";
				}
				if (this.languageParams == null || this.languageParams.Length == 0)
				{
					return Singleton<LanguageManager>.Instance.GetInfoByID(this.languageId);
				}
				return Singleton<LanguageManager>.Instance.GetInfoByID(this.languageId, this.languageParams);
			}
		}

		public string tipInfoId { get; private set; }

		public string TipInfo
		{
			get
			{
				if (string.IsNullOrEmpty(this.tipInfoId))
				{
					this.tipInfoId = "UIGameEvent_115";
				}
				return Singleton<LanguageManager>.Instance.GetInfoByID(this.tipInfoId);
			}
		}

		public string randomId { get; private set; }

		public EventSizeType sizeType { get; private set; }

		public bool IsTextSlot
		{
			get
			{
				return this.isRoot && this.sizeType == EventSizeType.BigWin;
			}
		}

		public float CacheTotalHeight { get; set; }

		public float CacheBgHeight { get; set; }

		public GameEventUIData.UserActionState actionState { get; private set; }

		public int selectedBtnIndex { get; private set; }

		public GameEventUIData(int eventResId, int stage, bool isRoot, string languageId, object[] languageParams)
		{
			this.eventResId = eventResId;
			this.stage = stage;
			this.isRoot = isRoot;
			this.languageId = languageId;
			this.languageParams = languageParams;
			this.actionState = GameEventUIData.UserActionState.Show;
			if (eventResId > 0)
			{
				Chapter_eventRes elementById = GameApp.Table.GetManager().GetChapter_eventResModelInstance().GetElementById(eventResId);
				if (elementById != null)
				{
					this.sizeType = (EventSizeType)elementById.type;
					return;
				}
			}
			else
			{
				this.sizeType = EventSizeType.Normal;
			}
		}

		public void SetAttParam(List<NodeAttParam> list)
		{
			this.paramList = list ?? new List<NodeAttParam>();
		}

		public void SetItemParam(List<NodeItemParam> items)
		{
			this.itemList = items ?? new List<NodeItemParam>();
		}

		public void SetSkillParam(List<NodeSkillParam> skills)
		{
			this.skillList = skills ?? new List<NodeSkillParam>();
		}

		public void SetInfParam(List<string> infos)
		{
			this.infoList = infos ?? new List<string>();
		}

		public void SetScoreParam(List<NodeScoreParam> scores)
		{
			this.scoreList = scores ?? new List<NodeScoreParam>();
		}

		public void SetInfo(string titleId, string summaryId)
		{
			this.titleId = titleId;
			this.summaryId = summaryId;
		}

		public void AddButton(int index, GameEventDataSelect btnData, bool isUndo, string undoTip)
		{
			GameEventUIButtonData gameEventUIButtonData = new GameEventUIButtonData(index, btnData, isUndo, undoTip);
			this.buttons.Add(gameEventUIButtonData);
		}

		public void AddButton(GameEventUIButtonData data)
		{
			if (data == null)
			{
				return;
			}
			this.buttons.Add(data);
		}

		public GameEventUIButtonData GetButton(int index)
		{
			if (index < this.buttons.Count)
			{
				return this.buttons[index];
			}
			return null;
		}

		public List<GameEventUIButtonData> GetButtons()
		{
			return this.buttons;
		}

		public bool IsShowButton()
		{
			return this.buttons.Count > 0;
		}

		public List<AttributeTypeData> GetAttParamList()
		{
			return NodeAttParam.GetAttParamList(this.paramList);
		}

		public bool IsHaveParams()
		{
			return this.paramList.Count > 0 || this.itemList.Count > 0 || this.skillList.Count > 0 || this.scoreList.Count > 0;
		}

		public List<ItemTypeData> GetItemParamList()
		{
			return NodeItemParam.GetItemParamList(this.itemList);
		}

		public List<SkillTypeData> GetSkillParamList()
		{
			return NodeSkillParam.GetSkillParamList(this.skillList);
		}

		public List<InfoTypeData> GetInfoParamList()
		{
			return GameEventUIData.GetInfoParamList(this.infoList);
		}

		public List<ActivityScoreTypeData> GetScoreParamList()
		{
			return NodeScoreParam.GetScoreParamList(this.scoreList);
		}

		public static List<InfoTypeData> GetInfoParamList(List<string> infos)
		{
			List<InfoTypeData> list = new List<InfoTypeData>();
			for (int i = 0; i < infos.Count; i++)
			{
				list.Add(new InfoTypeData
				{
					m_value = Singleton<LanguageManager>.Instance.GetInfoByID(infos[i]),
					m_tgaValue = Singleton<LanguageManager>.Instance.GetInfoByID(2, infos[i])
				});
			}
			return list;
		}

		public void SetTipInfoId(string id)
		{
			this.tipInfoId = id;
		}

		public void SetSelected(int btnIndex)
		{
			this.selectedBtnIndex = btnIndex;
			this.actionState = GameEventUIData.UserActionState.Selected;
		}

		public List<NodeAttParam> GetNodeAttList()
		{
			return this.paramList;
		}

		public List<NodeItemParam> GetNodeItemList()
		{
			return this.itemList;
		}

		public List<NodeSkillParam> GetNodeSkillList()
		{
			return this.skillList;
		}

		public List<string> GetNodeInfoList()
		{
			return this.infoList;
		}

		public List<NodeScoreParam> GetNodeScoreList()
		{
			return this.scoreList;
		}

		public bool isRoot;

		private List<NodeAttParam> paramList;

		private List<NodeItemParam> itemList;

		private List<NodeSkillParam> skillList;

		private List<string> infoList;

		private List<NodeScoreParam> scoreList;

		private List<GameEventUIButtonData> buttons = new List<GameEventUIButtonData>();

		public const int ONE_LINE_MAX_ITEM = 3;

		public enum UserActionState
		{
			Show,
			Selected
		}
	}
}
