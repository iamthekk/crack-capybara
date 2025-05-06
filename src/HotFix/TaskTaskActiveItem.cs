using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class TaskTaskActiveItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_item.Init();
		}

		protected override void OnDeInit()
		{
			this.m_item.DeInit();
		}

		public void OnRefreshUI(TaskDataModule.TaskActive data)
		{
			ItemData itemData = data.Reward.ToItemDataList()[0];
			this.m_item.SetData(itemData.ToPropData());
			this.m_item.OnRefresh();
			this.m_progressTxt.text = data.NeedActive.ToString();
			this.m_receivedObj.SetActive(data.IsReward);
			if (data.IsReward)
			{
				this.m_item.SetEnableButton(false);
				this.m_grays.SetUIGray();
				return;
			}
			this.m_item.SetEnableButton(true);
			this.m_grays.Recovery();
		}

		public UIItem m_item;

		public UIGrays m_grays;

		public CustomText m_progressTxt;

		public GameObject m_receivedObj;

		public Action<TaskTaskActiveItem, UIItem, PropData, object> OnClickCallback;
	}
}
