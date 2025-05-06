using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.NewWorld;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UINewWorldTaskItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.buttonGo.onClick.AddListener(new UnityAction(this.OnClickGo));
			this.buttonGet.onClick.AddListener(new UnityAction(this.OnClickGet));
			this.copyItem.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			this.buttonGo.onClick.RemoveListener(new UnityAction(this.OnClickGo));
			this.buttonGet.onClick.RemoveListener(new UnityAction(this.OnClickGet));
			for (int i = 0; i < this.itemList.Count; i++)
			{
				UIItem uiitem = this.itemList[i];
				if (uiitem)
				{
					uiitem.DeInit();
				}
			}
			this.itemList.Clear();
		}

		public void SetData(NewWorld_newWorldTask task, Action onClickGo)
		{
			if (task == null)
			{
				return;
			}
			this.mTaskData = task;
			this.OnGo = onClickGo;
			NewWorldDataModule dataModule = GameApp.Data.GetDataModule(DataName.NewWorldDataModule);
			int num = 0;
			int num2 = task.num;
			switch (task.group)
			{
			case 1:
				num = GameApp.Data.GetDataModule(DataName.ChapterDataModule).CurrentChapter.id - 1;
				this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(task.nameId, new object[] { num2 });
				break;
			case 2:
				num = GameApp.Data.GetDataModule(DataName.TalentDataModule).TalentExp;
				this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(task.nameId);
				break;
			case 3:
			{
				TowerDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.TowerDataModule);
				if (dataModule2.CompleteTowerLevelId > 0)
				{
					TowerChallenge_TowerLevel towerChallenge_TowerLevel = GameApp.Table.GetManager().GetTowerChallenge_TowerLevel(dataModule2.CompleteTowerLevelId);
					if (towerChallenge_TowerLevel != null)
					{
						num = towerChallenge_TowerLevel.layer;
					}
				}
				TowerChallenge_TowerLevel towerChallenge_TowerLevel2 = GameApp.Table.GetManager().GetTowerChallenge_TowerLevel(task.num);
				if (towerChallenge_TowerLevel2 != null)
				{
					num2 = towerChallenge_TowerLevel2.layer;
				}
				this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(task.nameId, new object[] { num2 });
				break;
			}
			}
			bool flag = dataModule.IsGetTaskReward(task.id);
			bool flag2 = num >= num2;
			string text = string.Format("{0}/{1}", num, num2);
			float num3 = (float)num / (float)num2;
			num3 = ((num3 >= 1f) ? 1f : num3);
			this.sliderProgress.value = num3;
			this.textSlider.text = text;
			this.buttonGo.gameObject.SetActiveSafe(!flag2);
			this.buttonGet.gameObject.SetActiveSafe(flag2 && !flag);
			this.finishObj.SetActiveSafe(flag2 && flag);
			List<ItemData> list = task.reward.ToItemDataList();
			if (list.Count != this.itemList.Count)
			{
				for (int i = 0; i < this.itemList.Count; i++)
				{
					this.itemList[i].gameObject.SetActiveSafe(false);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				UIItem uiitem;
				if (j < this.itemList.Count)
				{
					uiitem = this.itemList[j];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
					gameObject.SetParentNormal(this.layout, false);
					uiitem = gameObject.GetComponent<UIItem>();
					uiitem.Init();
					this.itemList.Add(uiitem);
				}
				uiitem.gameObject.SetActiveSafe(true);
				uiitem.SetData(list[j].ToPropData());
				uiitem.OnRefresh();
			}
		}

		private void OnClickGo()
		{
			Action onGo = this.OnGo;
			if (onGo == null)
			{
				return;
			}
			onGo();
		}

		private void OnClickGet()
		{
			if (this.mTaskData == null)
			{
				return;
			}
			NetworkUtils.NewWorld.DoNewWorldTaskRewardRequest(this.mTaskData.id, delegate(bool isOK, NewWorldTaskRewardResponse resp)
			{
				if (isOK)
				{
					if (resp != null && resp.CommonData.Reward != null && resp.CommonData.Reward.Count > 0)
					{
						DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
					}
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_NewWorld_Refresh_Task, null);
				}
			});
		}

		public CustomText textTitle;

		public Slider sliderProgress;

		public CustomText textSlider;

		public CustomButton buttonGo;

		public CustomButton buttonGet;

		public GameObject finishObj;

		public GameObject layout;

		public GameObject copyItem;

		private NewWorld_newWorldTask mTaskData;

		private Action OnGo;

		private List<UIItem> itemList = new List<UIItem>();
	}
}
