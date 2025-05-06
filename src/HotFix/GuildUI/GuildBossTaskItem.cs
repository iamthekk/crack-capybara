using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Guild;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class GuildBossTaskItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.ScrollView_Item.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.Button_Collect.m_onClick = new Action(this.OnClickGet);
			this.ScollRect.enabled = false;
		}

		protected override void OnDeInit()
		{
		}

		private void OnClickGet()
		{
			if (this.m_guildBossTask == null)
			{
				return;
			}
			if (this.m_guildBossTask.taskState != GuildBossTask.GuildBossTaskState.CanGetReward)
			{
				return;
			}
			GuildNetUtil.Guild.DoRequest_GetGuildBossTaskReward(this.m_guildBossTask.TaskID, delegate(bool result, GuildBossTaskRewardResponse resp)
			{
				if (resp != null && resp.CommonData != null)
				{
					GuildProxy.UI.OpenUICommonReward(resp.CommonData.Reward, null);
				}
			});
		}

		public void SetData(GuildBossTask taskData)
		{
			if (taskData == null)
			{
				return;
			}
			this.m_guildBossTask = taskData;
			GuildBOSS_guildBossTask guildBossTaskTable = GuildProxy.Table.GetGuildBossTaskTable(taskData.TaskID);
			if (guildBossTaskTable == null)
			{
				return;
			}
			this.Text_Title.text = GuildProxy.Language.GetInfoByID1(guildBossTaskTable.languageId, DxxTools.FormatNumber(taskData.Need));
			this.Text_Progress.text = string.Format("{0}/{1}", DxxTools.FormatNumber(taskData.Progress), DxxTools.FormatNumber(taskData.Need));
			this.Slider_Progress.maxValue = (float)taskData.Need;
			this.Slider_Progress.value = (float)taskData.Progress;
			if (taskData.taskState == GuildBossTask.GuildBossTaskState.AllFinish)
			{
				this.Obj_Finish.SetActiveSafe(true);
				this.Button_Collect.gameObject.SetActiveSafe(false);
			}
			else
			{
				this.Obj_Finish.SetActiveSafe(false);
				this.Button_Collect.gameObject.SetActiveSafe(true);
				if (taskData.taskState == GuildBossTask.GuildBossTaskState.CanGetReward)
				{
					this.Button_Collect.enabled = true;
					this.Button_Collect.GetComponent<UIGrays>().Recovery();
					this.Text_Collect.text = GuildProxy.Language.GetInfoByID("uimining_get_reward");
				}
				else
				{
					this.Button_Collect.enabled = false;
					this.Button_Collect.GetComponent<UIGrays>().SetUIGray();
					this.Text_Collect.text = GuildProxy.Language.GetInfoByID("1256");
				}
			}
			this.RedCollect.gameObject.SetActiveSafe(taskData.taskState == GuildBossTask.GuildBossTaskState.CanGetReward);
			List<ItemData> list = guildBossTaskTable.Reward.ToItemDataList();
			this.m_itemDataList = list.ToPropList();
			this.ScrollView_Item.SetListItemCount(this.m_itemDataList.Count, true);
			this.ScrollView_Item.RefreshAllShowItems();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0)
			{
				return null;
			}
			PropData propData = this.m_itemDataList[index];
			if (propData == null)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("TaskItem");
			GuildBossTaskRewardItem component;
			this.dic.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out component);
			if (component == null)
			{
				component = loopListViewItem.gameObject.GetComponent<GuildBossTaskRewardItem>();
				component.Init();
				this.dic[loopListViewItem.gameObject.GetInstanceID()] = component;
			}
			component.SetData(propData);
			return loopListViewItem;
		}

		public CustomText Text_Title;

		public Slider Slider_Progress;

		public CustomText Text_Progress;

		public CustomButton Button_Collect;

		public CustomText Text_Collect;

		public LoopListView2 ScrollView_Item;

		public ScrollRect ScollRect;

		public GameObject Obj_Finish;

		public RedNodeOneCtrl RedCollect;

		private GuildBossTask m_guildBossTask;

		private Dictionary<int, GuildBossTaskRewardItem> dic = new Dictionary<int, GuildBossTaskRewardItem>();

		private List<PropData> m_itemDataList = new List<PropData>();
	}
}
