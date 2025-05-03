using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Shop.Arena;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class TVRewardViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonClose.m_onClick = new Action(this.CloseSelf);
			this.buttonMask.m_onClick = new Action(this.CloseSelf);
			this.dataModule = GameApp.Data.GetDataModule(DataName.TVRewardDataModule);
		}

		public override void OnDelete()
		{
			this.buttonClose.m_onClick = null;
			this.buttonMask.m_onClick = null;
			this.nodeCtrls.Clear();
			foreach (TVRewardNodeCtrl tvrewardNodeCtrl in this.nodeCacheList)
			{
				tvrewardNodeCtrl.btn.m_onClick = null;
				tvrewardNodeCtrl.DeInit();
				Object.Destroy(tvrewardNodeCtrl.gameObject);
			}
			this.nodeCacheList.Clear();
			this.dataModule.ClearCacheTextures();
			this.dataModule = null;
		}

		public override void OnOpen(object data)
		{
			this.contentObj.SetActive(false);
			this.netObj.SetActive(true);
			this.dataModule.RequestTVInfos(false);
		}

		public override void OnClose()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_TVReward_GetInfos, new HandlerEvent(this.OnOpenedView));
			manager.RegisterEvent(LocalMessageName.CC_TVReward_PickedReward, new HandlerEvent(this.OnPickedReward));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_TVReward_GetInfos, new HandlerEvent(this.OnOpenedView));
			manager.UnRegisterEvent(LocalMessageName.CC_TVReward_PickedReward, new HandlerEvent(this.OnPickedReward));
		}

		private void OnOpenedView(object sender, int eventID, BaseEventArgs eventArgs)
		{
			if (!this.dataModule.IsLoadedAll)
			{
				this.CloseSelf();
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("tvreward_loadall_error"));
				return;
			}
			this.netObj.SetActive(false);
			this.contentObj.SetActive(true);
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("tvreward_title");
			this.RefreshEntryNodes();
		}

		private void OnPickedReward(object sender, int eventID, BaseEventArgs eventArgs)
		{
			EventPickedReward eventPickedReward = eventArgs as EventPickedReward;
			if (eventPickedReward != null)
			{
				this.FreshTVReward(eventPickedReward.UID, null);
			}
		}

		private void RefreshEntryNodes()
		{
			foreach (TVRewardNodeCtrl tvrewardNodeCtrl in this.nodeCacheList)
			{
				tvrewardNodeCtrl.gameObject.SetActive(false);
			}
			this.nodeCtrls.Clear();
			this.nodeCtrlParent.anchoredPosition = Vector2.zero;
			this.scrollRect.StopMovement();
			for (int i = 0; i < this.dataModule.tvInfos.Count; i++)
			{
				this.CreateNode(this.dataModule.tvInfos[i]);
			}
			this.CheckEmpty();
		}

		private void CreateNode(GmVideoDto tvInfo)
		{
			string tvtextureUrl = this.dataModule.GetTVTextureUrl(tvInfo, -1);
			if (string.IsNullOrEmpty(tvtextureUrl))
			{
				return;
			}
			int count = this.nodeCtrls.Count;
			TVRewardNodeCtrl nodeCtrl;
			if (count < this.nodeCacheList.Count)
			{
				nodeCtrl = this.nodeCacheList[count];
			}
			else
			{
				nodeCtrl = Object.Instantiate<TVRewardNodeCtrl>(this.nodeCtrlPrefab, this.nodeCtrlParent);
				this.nodeCacheList.Add(nodeCtrl);
				nodeCtrl.Init();
			}
			if (!nodeCtrl.gameObject.activeSelf)
			{
				nodeCtrl.gameObject.SetActive(true);
			}
			this.nodeCtrls.Add(nodeCtrl);
			nodeCtrl.uid = tvInfo.VideoId;
			nodeCtrl.btn.m_onClick = delegate
			{
				this.OnClickEntryNode(nodeCtrl);
			};
			nodeCtrl.banner.texture = this.defaultUrlTexture;
			this.dataModule.SetUrlImage(tvtextureUrl, nodeCtrl.banner);
			this.FreshTVReward(tvInfo.VideoId, nodeCtrl);
		}

		private void FreshTVReward(string uid, TVRewardNodeCtrl nodeCtrl = null)
		{
			if (nodeCtrl == null)
			{
				for (int i = 0; i < this.nodeCtrls.Count; i++)
				{
					if (this.nodeCtrls[i].uid == uid)
					{
						nodeCtrl = this.nodeCtrls[i];
						break;
					}
				}
			}
			if (nodeCtrl == null)
			{
				return;
			}
			if (this.dataModule.ShowRed(uid))
			{
				nodeCtrl.rewardObj.SetActiveSafe(true);
				string[] array = GameApp.Table.GetManager().GetGameConfig_Config(99).Value.Split(',', StringSplitOptions.None);
				ItemData itemData = new ItemData(int.Parse(array[0]), long.Parse(array[1]));
				nodeCtrl.item.SetData(itemData.ToPropData());
				nodeCtrl.item.OnRefresh();
				return;
			}
			nodeCtrl.rewardObj.SetActiveSafe(false);
		}

		private void CheckEmpty()
		{
			this.emptyTipObj.SetActiveSafe(this.nodeCtrls.Count == 0);
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(ViewName.TVRewardViewModule, null);
		}

		private void OnClickEntryNode(TVRewardNodeCtrl ctrl)
		{
			this.dataModule.RequestWatchTVStart(ctrl.uid);
		}

		public void OnApplicationPause(bool pauseStatus)
		{
			if (!pauseStatus && this.dataModule.WatchTVEndByReturnUnity)
			{
				this.dataModule.RequestWatchTVEnd();
			}
		}

		public CustomButton buttonMask;

		public CustomButton buttonClose;

		public CustomText textTitle;

		public GameObject contentObj;

		public GameObject netObj;

		public ScrollRect scrollRect;

		public RectTransform nodeCtrlParent;

		public TVRewardNodeCtrl nodeCtrlPrefab;

		public GameObject emptyTipObj;

		public Texture2D defaultUrlTexture;

		private List<TVRewardNodeCtrl> nodeCtrls = new List<TVRewardNodeCtrl>();

		private List<TVRewardNodeCtrl> nodeCacheList = new List<TVRewardNodeCtrl>();

		private TVRewardDataModule dataModule;
	}
}
