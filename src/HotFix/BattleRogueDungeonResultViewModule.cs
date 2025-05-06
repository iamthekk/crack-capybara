using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class BattleRogueDungeonResultViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.mDataModule = GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule);
			this.copyItem.SetActiveSafe(false);
			this.tapCloseCtrl.gameObject.SetActiveSafe(false);
		}

		public override void OnOpen(object data)
		{
			this.tapCloseCtrl.OnClose = new Action(this.OnClickClose);
			EventArgsBool instance = Singleton<EventArgsBool>.Instance;
			instance.SetData(true);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_Pause, instance);
			this.winObj.SetActiveSafe(this.mDataModule.PassFloorCount > 0U);
			this.failObj.SetActiveSafe(this.mDataModule.PassFloorCount <= 0U);
			this.textPass.text = Singleton<LanguageManager>.Instance.GetInfoByID("roguedungon_result_pass", new object[] { this.mDataModule.PassFloorCount });
			Sequence sequence = this.seqPool.Get();
			List<ItemData> list = this.mDataModule.TotalReward.ToItemDataList();
			for (int i = 0; i < list.Count; i++)
			{
				UIItem item = null;
				if (i < this.itemList.Count)
				{
					item = this.itemList[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
					gameObject.SetParentNormal(this.itemParent, false);
					item = gameObject.GetComponent<UIItem>();
					item.Init();
					this.itemList.Add(item);
				}
				item.gameObject.SetActiveSafe(true);
				item.SetData(list[i].ToPropData());
				item.OnRefresh();
				int index = i;
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.PlayRewardScale(item, index);
				});
			}
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.tapCloseCtrl.gameObject.SetActiveSafe(true);
			});
			this.noRewardObj.SetActiveSafe(list.Count == 0);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			EventArgsBool instance = Singleton<EventArgsBool>.Instance;
			instance.SetData(false);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Battle_Pause, instance);
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].gameObject.SetActiveSafe(false);
			}
		}

		public override void OnDelete()
		{
			this.tapCloseCtrl.OnClose = null;
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].DeInit();
			}
			this.itemList.Clear();
			this.seqPool.Clear(false);
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickClose()
		{
			this.tapCloseCtrl.OnClose = null;
			GameApp.View.OpenView(ViewName.LoadingMainViewModule, null, 2, null, delegate(GameObject obj)
			{
				GameApp.View.GetViewModule(ViewName.LoadingMainViewModule).PlayShow(delegate
				{
					GameApp.View.CloseView(ViewName.BattleRogueDungeonViewModule, null);
					GameApp.View.CloseView(ViewName.BattleRogueDungeonResultViewModule, null);
					if (GameApp.View.IsOpened(ViewName.RoundSelectSkillViewModule))
					{
						GameApp.View.CloseView(ViewName.RoundSelectSkillViewModule, null);
					}
					if (GameApp.View.IsOpened(ViewName.SpecialChallengesViewModule))
					{
						GameApp.View.CloseView(ViewName.SpecialChallengesViewModule, null);
					}
					if (GameApp.View.IsOpened(ViewName.GameEventBoxViewModule))
					{
						GameApp.View.CloseView(ViewName.GameEventBoxViewModule, null);
					}
					if (GameApp.View.IsOpened(ViewName.SlotTrainViewModule))
					{
						GameApp.View.CloseView(ViewName.SlotTrainViewModule, null);
					}
					if (GameApp.View.IsOpened(ViewName.GameEventAngelViewModule))
					{
						GameApp.View.CloseView(ViewName.GameEventAngelViewModule, null);
					}
					if (GameApp.View.IsOpened(ViewName.GameEventDemonViewModule))
					{
						GameApp.View.CloseView(ViewName.GameEventDemonViewModule, null);
					}
					GameApp.State.ActiveState(StateName.MainState);
				});
			});
		}

		private Sequence PlayRewardScale(UIItem item, int index)
		{
			Sequence sequence = this.seqPool.Get();
			if (item == null)
			{
				return sequence;
			}
			item.transform.localScale = Vector3.zero;
			TweenSettingsExtensions.AppendInterval(sequence, (float)index * 0.05f);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(item.transform, Vector3.one * 1.1f, 0.15f));
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(item.transform, Vector3.one, 0.05f));
			return sequence;
		}

		public GameObject winObj;

		public GameObject failObj;

		public GameObject itemParent;

		public GameObject copyItem;

		public TapToCloseCtrl tapCloseCtrl;

		public CustomText textPass;

		public GameObject noRewardObj;

		private RogueDungeonDataModule mDataModule;

		private List<UIItem> itemList = new List<UIItem>();

		private SequencePool seqPool = new SequencePool();
	}
}
