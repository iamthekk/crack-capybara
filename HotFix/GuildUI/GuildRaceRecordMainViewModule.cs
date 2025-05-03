using System;
using System.Collections.Generic;
using DG.Tweening;
using Dxx.Guild;
using Framework.Logic.UI;
using Proto.GuildRace;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class GuildRaceRecordMainViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.PopCommon.OnClick = new Action<int>(this.OnPopClick);
			this.Prefab_GuildVS.gameObject.SetActive(false);
			this.Prefab_VSContent.gameObject.SetActive(false);
			this.UserVSContent = Object.Instantiate<GuildRaceRecordMainUserVSContent>(this.Prefab_VSContent, this.RTF_Content);
			this.UserVSContent.Init();
			this.UserVSContent.SetActive(false);
			this.Text_Score.text = "";
			this.SwitchButtons.OnSwitch = new Action<CustomChooseButton>(this.OnSwitchDay);
			this.Text_Empty.SetActive(false);
			this.LoadingUI.SetActive(false);
		}

		protected override void OnViewOpen(object data)
		{
			this.SelectDefaultDay();
		}

		protected override void OnViewClose()
		{
			this.SaveViewCacheData();
			this.m_seqPool.Clear(false);
			this.CurrentUnFoldUI = null;
			this.CurShowDay = 0;
			this.AllData.Clear();
		}

		protected override void OnViewDelete()
		{
			base.OnViewDelete();
			this.RemoveAllGuildVS(true);
			this.UserVSContent.DeInit();
			Object.Destroy(this.UserVSContent);
			this.UserVSContent = null;
			this.CurrentUnFoldUI = null;
		}

		private void SelectDefaultDay()
		{
			this.CheckRevertViewOnOpen();
			this.CurShowDay = 0;
			this.SwitchButtons.ChooseButtonName(this.mDayButtonNames[this.CurShowDay]);
		}

		private void GetDataFromServer()
		{
			List<GuildRaceGuildVSRecord> list;
			if (this.AllData.TryGetValue(this.CurShowDay, out list))
			{
				this.CurDataList = list;
				this.RefreshUI();
				return;
			}
			this.Text_Empty.SetActive(false);
			int day = this.CurShowDay;
			this.LoadingUI.SetActive(true);
			GuildRaceBattleController instance = GuildRaceBattleController.Instance;
			if (instance != null && instance.CurrentRaceStage != null)
			{
				GuildRaceStagePart currentRaceStage = instance.CurrentRaceStage;
				if (day > currentRaceStage.BattleDay || (day == currentRaceStage.BattleDay && currentRaceStage.StageKind < GuildRaceStageKind.BattleOver))
				{
					this.LoadingUI.SetActive(false);
					List<GuildRaceGuildVSRecord> list2 = new List<GuildRaceGuildVSRecord>();
					this.AllData[day] = list2;
					this.CurDataList = list2;
					this.RefreshUI();
					return;
				}
			}
			GuildNetUtil.Guild.DoRequest_GuildRaceGuildRecordRequest(day, delegate(bool result, GuildRaceGuildRecordResponse resp)
			{
				if (!this.CheckIsViewOpen())
				{
					return;
				}
				this.LoadingUI.SetActive(false);
				if (result)
				{
					List<GuildRaceGuildVSRecord> list3 = resp.ToGuildVSRecordList();
					this.AllData[day] = list3;
					if (day == this.CurShowDay)
					{
						this.CurDataList = list3;
						this.RefreshUI();
						return;
					}
				}
				else
				{
					HLog.LogError(string.Format("获取战斗记录失败！{0} code:{1}", day, (resp != null) ? resp.Code : 0));
				}
			});
		}

		public void RemoveAllGuildVS(bool destroy)
		{
			if (destroy)
			{
				for (int i = 0; i < this.UIList.Count; i++)
				{
					this.UIList[i].DeInit();
				}
				this.UIList.Clear();
				this.RTF_Content.DestroyChildren();
				return;
			}
			for (int j = 0; j < this.UIList.Count; j++)
			{
				this.UIList[j].SetActive(false);
			}
		}

		private void OnSwitchDay(CustomChooseButton button)
		{
			int num = 0;
			if (button != null)
			{
				for (int i = 0; i < this.mDayButtonNames.Length; i++)
				{
					if (button.name == this.mDayButtonNames[i])
					{
						num = i + 1;
						break;
					}
				}
			}
			if (num == this.CurShowDay)
			{
				return;
			}
			this.ClearUIBySwitchDay();
			this.CurShowDay = num;
			this.GetDataFromServer();
		}

		private void ClearUIBySwitchDay()
		{
			this.Text_Empty.SetActive(false);
			if (this.CurrentUnFoldUI != null)
			{
				this.CurrentUnFoldUI.PlayFold(true, null);
			}
			this.CurrentUnFoldUI = null;
			this.UserVSContent.SetActive(false);
			for (int i = 0; i < this.UIList.Count; i++)
			{
				this.UIList[i].SetActive(false);
			}
		}

		public void RefreshUI()
		{
			if (GuildRaceBattleController.Instance != null)
			{
				this.Text_Score.text = GuildRaceBattleController.Instance.MyGuildCurScore.ToString();
			}
			if (this.CurDataList == null)
			{
				this.Text_Empty.SetActive(true);
				return;
			}
			this.m_seqPool.Clear(false);
			List<GuildRaceGuildVSRecord> curDataList = this.CurDataList;
			for (int i = 0; i < curDataList.Count; i++)
			{
				GuildRaceRecordMainGuildVS guildRaceRecordMainGuildVS = null;
				if (i < this.UIList.Count)
				{
					guildRaceRecordMainGuildVS = this.UIList[i];
				}
				if (guildRaceRecordMainGuildVS == null)
				{
					guildRaceRecordMainGuildVS = Object.Instantiate<GuildRaceRecordMainGuildVS>(this.Prefab_GuildVS, this.RTF_Content);
					guildRaceRecordMainGuildVS.SetActive(true);
					guildRaceRecordMainGuildVS.Init();
					guildRaceRecordMainGuildVS.PlayFold(true, null);
					guildRaceRecordMainGuildVS.OnClick = new Action<GuildRaceRecordMainGuildVS>(this.FoldGuildVS);
					if (i < this.UIList.Count)
					{
						this.UIList[i] = guildRaceRecordMainGuildVS;
					}
					else
					{
						this.UIList.Add(guildRaceRecordMainGuildVS);
					}
				}
				guildRaceRecordMainGuildVS.SetActive(true);
				guildRaceRecordMainGuildVS.SetData(curDataList[i]);
				guildRaceRecordMainGuildVS.RefreshUI();
				RectTransform rectTransform = guildRaceRecordMainGuildVS.gameObject.transform as RectTransform;
				rectTransform.localScale = Vector3.zero;
				Sequence sequence = this.m_seqPool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, (float)i * 0.05f);
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(rectTransform, 1f, 0.15f));
			}
			for (int j = curDataList.Count; j < this.UIList.Count; j++)
			{
				this.UIList[j].SetActive(false);
			}
			this.Text_Empty.SetActive(curDataList.Count <= 0);
		}

		public void FoldGuildVS(GuildRaceRecordMainGuildVS ui)
		{
			this.m_seqPool.Clear(false);
			this.SwitchGuildVSState(ui);
			this.RefreshUserVS();
			this.MoveScroll();
		}

		public void SwitchGuildVSState(GuildRaceRecordMainGuildVS ui)
		{
			if (ui == null)
			{
				if (this.CurrentUnFoldUI != null)
				{
					this.CurrentUnFoldUI.PlayFold(true, this.m_seqPool);
					return;
				}
			}
			else
			{
				if (this.CurrentUnFoldUI != null)
				{
					if (this.CurrentUnFoldUI == ui)
					{
						this.CurrentUnFoldUI.PlayFold(true, this.m_seqPool);
						this.CurrentUnFoldUI = null;
						return;
					}
					this.CurrentUnFoldUI.PlayFold(true, this.m_seqPool);
				}
				this.CurrentUnFoldUI = ui;
				this.CurrentUnFoldUI.PlayFold(false, this.m_seqPool);
			}
		}

		public void RefreshUserVS()
		{
			if (this.CurrentUnFoldUI == null)
			{
				this.UserVSContent.SetActive(false);
				return;
			}
			GuildRaceGuildVSRecord data = this.CurrentUnFoldUI.Data;
			int childCount = this.RTF_Content.childCount;
			for (int i = 0; i < childCount; i++)
			{
				GameObject gameObject = this.RTF_Content.GetChild(i).gameObject;
				if (gameObject.activeSelf && gameObject == this.CurrentUnFoldUI.gameObject)
				{
					this.UserVSContent.gameObject.transform.SetSiblingIndex(i);
					this.UserVSContent.SetActive(true);
				}
			}
			this.UserVSContent.SetDataList(data.ResultList);
			this.UserVSContent.RefreshUI(this.m_seqPool);
		}

		private void MoveScroll()
		{
			if (this.CurrentUnFoldUI != null)
			{
				Sequence sequence = this.m_seqPool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, 0.05f);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					if (this.CurrentUnFoldUI != null)
					{
						Sequence sequence2 = this.m_seqPool.Get();
						RectTransform rectTransform = this.CurrentUnFoldUI.gameObject.transform as RectTransform;
						Vector2 vector;
						vector..ctor(0f, Mathf.Abs(rectTransform.anchoredPosition.y));
						TweenSettingsExtensions.AppendInterval(sequence2, 0.05f);
						TweenSettingsExtensions.Append(sequence2, ShortcutExtensions46.DOAnchorPos(this.Scroll.content, vector, 0.3f, false));
					}
				});
			}
		}

		private void OnPopClick(int obj)
		{
			this.ClickCloseThis();
		}

		private void ClickCloseThis()
		{
			GuildProxy.UI.CloseGuildRaceRecord();
		}

		private void SaveViewCacheData()
		{
		}

		private int FindCurrentUnFoldUI()
		{
			if (this.CurrentUnFoldUI == null)
			{
				return -1;
			}
			for (int i = 0; i < this.UIList.Count; i++)
			{
				if (this.UIList[i] == this.CurrentUnFoldUI)
				{
					return i;
				}
			}
			return -1;
		}

		private void CheckRevertViewOnOpen()
		{
		}

		public UIGuildPopCommon PopCommon;

		public ScrollRect Scroll;

		public RectTransform RTF_Content;

		public GuildRaceRecordMainGuildVS Prefab_GuildVS;

		public GuildRaceRecordMainUserVSContent Prefab_VSContent;

		public GameObject Text_Empty;

		public CustomText Text_Score;

		public CustomChooseButtonGroup SwitchButtons;

		public GameObject LoadingUI;

		private Dictionary<int, List<GuildRaceGuildVSRecord>> AllData = new Dictionary<int, List<GuildRaceGuildVSRecord>>();

		private List<GuildRaceRecordMainGuildVS> UIList = new List<GuildRaceRecordMainGuildVS>();

		private GuildRaceRecordMainGuildVS CurrentUnFoldUI;

		private GuildRaceRecordMainUserVSContent UserVSContent;

		private int CurShowDay;

		public List<GuildRaceGuildVSRecord> CurDataList = new List<GuildRaceGuildVSRecord>();

		private string[] mDayButtonNames = new string[] { "Button_Day1", "Button_Day2", "Button_Day3" };

		private SequencePool m_seqPool = new SequencePool();

		private class ReOpenCacheData
		{
			public Dictionary<int, List<GuildRaceGuildVSRecord>> AllData = new Dictionary<int, List<GuildRaceGuildVSRecord>>();

			public int CurShowDay;

			public int CurrentUnFoldUIIndex = -1;
		}
	}
}
