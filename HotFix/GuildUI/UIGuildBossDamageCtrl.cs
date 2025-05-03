using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.UI;
using LocalModels.Bean;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildBossDamageCtrl : GuildProxy.GuildProxy_BaseBehaviour
	{
		private GuildBossInfo guildBoss
		{
			get
			{
				return base.SDK.GuildActivity.GuildBoss;
			}
		}

		protected override void GuildUI_OnInit()
		{
			this.ButtonLeft.onClick.AddListener(new UnityAction(this.OnClickLeft));
			this.ButtonRight.onClick.AddListener(new UnityAction(this.OnClickRight));
			this.ButtonScroll.onClick.AddListener(new UnityAction(this.OnClickScroll));
			LoopListViewInitParam loopListViewInitParam = LoopListViewInitParam.CopyDefaultInitParam();
			loopListViewInitParam.mSnapVecThreshold = 500f;
			loopListViewInitParam.mSnapFinishThreshold = 1f;
			loopListViewInitParam.mSmoothDumpRate = 0.1f;
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), loopListViewInitParam);
			this.Scroll.mOnSnapItemFinished = new Action<LoopListView2, LoopListViewItem2>(this.OnSnapItemFinished);
			this.Scroll.mOnSnapNearestChanged = new Action<LoopListView2, LoopListViewItem2>(this.OnSnapNearestChanged);
			this.Scroll.ScrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScrollValueChange));
			this.RewardsTips.Init();
		}

		protected override void GuildUI_OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.GuildUI_OnUpdate(deltaTime, unscaledDeltaTime);
			if (this.RewardsTips != null)
			{
				this.RewardsTips.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		protected override void GuildUI_OnUnInit()
		{
			this.ButtonLeft.onClick.RemoveListener(new UnityAction(this.OnClickLeft));
			this.ButtonRight.onClick.RemoveListener(new UnityAction(this.OnClickRight));
			this.ButtonScroll.onClick.RemoveListener(new UnityAction(this.OnClickScroll));
			this.RewardsTips.DeInit();
			this.DeInitAllScrollUI();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			int num = index / 2;
			if (index < 0 || num >= this.mDataList.Count)
			{
				return null;
			}
			if (index % 2 == 1)
			{
				return listView.NewListViewItem("LineNode");
			}
			UIGuildBossDamageBoxData uiguildBossDamageBoxData = this.mDataList[num];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("DamageNode");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UIGuildBossDamageBoxCell uiguildBossDamageBoxCell = this.TryGetUI(instanceID);
			UIGuildBossDamageBoxCell component = loopListViewItem.GetComponent<UIGuildBossDamageBoxCell>();
			if (uiguildBossDamageBoxCell == null)
			{
				uiguildBossDamageBoxCell = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			uiguildBossDamageBoxCell.SetData(uiguildBossDamageBoxData);
			uiguildBossDamageBoxCell.SetActive(true);
			uiguildBossDamageBoxCell.RefreshUI();
			return loopListViewItem;
		}

		private UIGuildBossDamageBoxCell TryGetUI(int key)
		{
			UIGuildBossDamageBoxCell uiguildBossDamageBoxCell;
			if (this.mUICtrlDic.TryGetValue(key, out uiguildBossDamageBoxCell))
			{
				return uiguildBossDamageBoxCell;
			}
			return null;
		}

		private UIGuildBossDamageBoxCell TryAddUI(int key, LoopListViewItem2 loopitem, UIGuildBossDamageBoxCell ui)
		{
			ui.Init();
			ui.OnClick = new Action<UIGuildBossDamageBoxCell>(this.OnClickBoxCell);
			UIGuildBossDamageBoxCell uiguildBossDamageBoxCell;
			if (this.mUICtrlDic.TryGetValue(key, out uiguildBossDamageBoxCell))
			{
				if (uiguildBossDamageBoxCell == null)
				{
					uiguildBossDamageBoxCell = ui;
					this.mUICtrlDic[key] = ui;
				}
				return ui;
			}
			this.mUICtrlDic.Add(key, ui);
			return ui;
		}

		private void DeInitAllScrollUI()
		{
			foreach (KeyValuePair<int, UIGuildBossDamageBoxCell> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUICtrlDic.Clear();
		}

		public void RefreshUI()
		{
			this.BuildDataList();
			this.Scroll.SetListItemCount(this.mDataList.Count * 2 - 1, true);
			this.Scroll.RefreshAllShowItems();
			this.Scroll.MovePanelToItemIndex(this.mIndexOfUnOpenData * 2, 0f);
			this.Scroll.FinishSnapImmediately();
		}

		private void BuildDataList()
		{
			GuildBossData currentBoss = this.guildBoss.GetCurrentBoss();
			this.mDataList.Clear();
			this.mIndexOfUnOpenData = 0;
			List<GuildBOSS_guildBossBox> allGuildBossDamageTables = GuildProxy.Table.GetAllGuildBossDamageTables(currentBoss.BossID);
			long totalPersonalDamage = this.guildBoss.TotalPersonalDamage;
			int num = -1;
			for (int i = 0; i < allGuildBossDamageTables.Count; i++)
			{
				UIGuildBossDamageBoxData uiguildBossDamageBoxData = UIGuildBossDamageBoxData.Get(allGuildBossDamageTables[i]);
				uiguildBossDamageBoxData.IsOpen = totalPersonalDamage >= uiguildBossDamageBoxData.Damage;
				if (uiguildBossDamageBoxData.IsOpen)
				{
					num = i;
				}
				this.mDataList.Add(uiguildBossDamageBoxData);
			}
			this.mIndexOfUnOpenData = num;
			if (this.mIndexOfUnOpenData >= this.mDataList.Count)
			{
				this.mIndexOfUnOpenData = this.mDataList.Count - 1;
			}
		}

		private void OnSnapItemFinished(LoopListView2 view, LoopListViewItem2 item)
		{
			int itemIndex = item.ItemIndex;
			if (itemIndex % 2 == 1)
			{
				int num = (itemIndex + 1) / 2;
				this.Scroll.SetSnapTargetItemIndex(num * 2, 0f);
			}
			else
			{
				this.mIndexOfCurShowData = (itemIndex + 1) / 2;
			}
			this.CheckLeftRightButtonShow(-1);
		}

		private void OnSnapNearestChanged(LoopListView2 view, LoopListViewItem2 item)
		{
			int itemIndex = item.ItemIndex;
			if (itemIndex % 2 == 0)
			{
				int num = (itemIndex + 1) / 2;
				this.CheckLeftRightButtonShow(num);
			}
		}

		private void OnScrollValueChange(Vector2 arg0)
		{
			this.HideTips(false);
		}

		private void OnClickLeft()
		{
			this.HideTips(false);
			if (this.mIndexOfCurShowData <= 0)
			{
				return;
			}
			this.mIndexOfCurShowData--;
			this.Scroll.SetSnapTargetItemIndex(this.mIndexOfCurShowData * 2, -1f);
			this.CheckLeftRightButtonShow(-1);
		}

		private void OnClickRight()
		{
			this.HideTips(false);
			if (this.mIndexOfCurShowData >= this.mDataList.Count)
			{
				return;
			}
			this.mIndexOfCurShowData++;
			this.Scroll.SetSnapTargetItemIndex(this.mIndexOfCurShowData * 2, -1f);
			this.CheckLeftRightButtonShow(-1);
		}

		private void CheckLeftRightButtonShow(int checkindex = -1)
		{
			if (checkindex < 0)
			{
				checkindex = this.mIndexOfCurShowData;
			}
			if (checkindex <= 1)
			{
				this.ButtonLeft.gameObject.SetActive(false);
			}
			else
			{
				this.ButtonLeft.gameObject.SetActive(true);
			}
			if (checkindex >= this.mDataList.Count - 2)
			{
				this.ButtonRight.gameObject.SetActive(false);
				return;
			}
			this.ButtonRight.gameObject.SetActive(true);
		}

		private void OnClickScroll()
		{
			this.HideTips(true);
		}

		private void OnClickBoxCell(UIGuildBossDamageBoxCell cell)
		{
			if (cell == null)
			{
				this.HideTips(true);
				return;
			}
			this.OnClickShowBoxTips(cell);
		}

		private void OnClickShowBoxTips(UIGuildBossDamageBoxCell cell)
		{
			if (this.mTipsTarget == cell)
			{
				this.HideTips(true);
				return;
			}
			this.mTipsTarget = cell;
			this.RewardsTips.SetData(cell.GetRewards());
			this.RewardsTips.AttachTo(cell.ObjBoxOpen.transform);
			this.RewardsTips.RefreshUI();
			this.RewardsTips.PlayShow(false);
		}

		public void HideTips(bool ani = false)
		{
			this.mTipsTarget = null;
			this.RewardsTips.PlayHide(ani);
		}

		[Header("左右按钮")]
		public CustomButton ButtonLeft;

		public CustomButton ButtonRight;

		[Header("伤害奖励Tips")]
		public CustomButton ButtonScroll;

		public UIGuildBossRewardsTips RewardsTips;

		private UIGuildBossDamageBoxCell mTipsTarget;

		[Header("滚动区域")]
		public LoopListView2 Scroll;

		private List<UIGuildBossDamageBoxData> mDataList = new List<UIGuildBossDamageBoxData>();

		private Dictionary<int, UIGuildBossDamageBoxCell> mUICtrlDic = new Dictionary<int, UIGuildBossDamageBoxCell>();

		private int mIndexOfUnOpenData;

		private int mIndexOfCurShowData;
	}
}
