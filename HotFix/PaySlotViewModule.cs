using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class PaySlotViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.copyItem.SetActiveSafe(false);
			this.ShowMask(false);
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			PaySlotViewModule.OpenData openData = data as PaySlotViewModule.OpenData;
			if (openData != null)
			{
				this.mOpenData = openData;
			}
			if (this.mOpenData == null)
			{
				return;
			}
			BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
			if (playerData == null || !playerData.Chips.IsDataValid())
			{
				this.OnClickClose();
				return;
			}
			this.CreateItems();
			this.Refresh();
			this.PlayAnimation();
			GameApp.Sound.PlayClip(124, 1f);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			for (int i = 0; i < this.paySlotItems.Count; i++)
			{
				if (this.paySlotItems[i] != null)
				{
					this.paySlotItems[i].OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
		}

		public override void OnClose()
		{
			for (int i = 0; i < this.paySlotItems.Count; i++)
			{
				this.paySlotItems[i].OnHide();
			}
		}

		public override void OnDelete()
		{
			this.sequencePool.Clear(false);
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			for (int i = 0; i < this.paySlotItems.Count; i++)
			{
				UIPaySlotItem uipaySlotItem = this.paySlotItems[i];
				if (uipaySlotItem)
				{
					uipaySlotItem.DeInit();
				}
			}
			this.paySlotItems.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void CreateItems()
		{
			if (this.mOpenData == null)
			{
				return;
			}
			ChapterMiniGame_paySlotBase chapterMiniGame_paySlotBase = GameApp.Table.GetManager().GetChapterMiniGame_paySlotBase(this.mOpenData.slotId);
			if (chapterMiniGame_paySlotBase == null)
			{
				HLog.LogError(string.Format("[ChapterMiniGame_paySlotBase] not found id={0}", this.mOpenData.slotId));
				return;
			}
			List<int[]> list = new List<int[]>();
			list.Add(chapterMiniGame_paySlotBase.first);
			list.Add(chapterMiniGame_paySlotBase.second);
			list.Add(chapterMiniGame_paySlotBase.third);
			for (int i = 0; i < list.Count; i++)
			{
				int[] array = list[i];
				if (array.Length < 3)
				{
					return;
				}
				int num = array[0];
				int num2 = array[1];
				int num3 = array[2];
				UIPaySlotItem uipaySlotItem;
				if (i < this.paySlotItems.Count)
				{
					uipaySlotItem = this.paySlotItems[i];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
					gameObject.SetParentNormal(this.nodeParent, false);
					uipaySlotItem = gameObject.GetComponent<UIPaySlotItem>();
					uipaySlotItem.Init();
					this.paySlotItems.Add(uipaySlotItem);
				}
				uipaySlotItem.gameObject.SetActiveSafe(true);
				uipaySlotItem.SetData(this.mOpenData.seed, num3, num, num2, i, new Action<bool>(this.ShowMask));
			}
		}

		private void Refresh()
		{
			BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
			if (playerData == null)
			{
				return;
			}
			this.textCurrency.text = playerData.Chips.mVariable.ToString();
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.PaySlotViewModule, null);
			EventCloseMiniGameUI eventCloseMiniGameUI = new EventCloseMiniGameUI();
			eventCloseMiniGameUI.SetData(MiniGameType.PaySlot, null);
			GameApp.Event.DispatchNow(this, 375, eventCloseMiniGameUI);
		}

		private void PlayAnimation()
		{
			float time = Time.time;
			this.titleObj.AddComponent<EnterScaleAnimationCtrl>().PlayShowAnimation(time, 0, 100025);
			this.currencyObj.AddComponent<EnterScaleAnimationCtrl>().PlayShowAnimation(time, 0, 100025);
			for (int i = 0; i < this.paySlotItems.Count; i++)
			{
				this.paySlotItems[i].gameObject.AddComponent<EnterScaleAnimationCtrl>().PlayShowAnimation(time, i, 100025);
			}
			Sequence sequence = this.sequencePool.Get();
			RectTransform component = this.buttonClose.GetComponent<RectTransform>();
			component.anchoredPosition = new Vector2(-800f, component.anchoredPosition.y);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(component, 82f, 0.2f, false)), ShortcutExtensions46.DOAnchorPosX(component, 72f, 0.1f, false));
		}

		private void ShowMask(bool isShow)
		{
			this.clickMask.SetActiveSafe(isShow);
			this.Refresh();
		}

		[GameTestMethod("事件小游戏", "赌博机", "", 312)]
		private static void OpenPaySlot()
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			PaySlotViewModule.OpenData openData = new PaySlotViewModule.OpenData();
			openData.seed = random.Next();
			openData.slotId = 1;
			GameApp.View.OpenView(ViewName.PaySlotViewModule, openData, 1, null, null);
		}

		public CustomText textCurrency;

		public GameObject nodeParent;

		public GameObject copyItem;

		public CustomButton buttonClose;

		public GameObject titleObj;

		public GameObject currencyObj;

		public GameObject clickMask;

		private List<UIPaySlotItem> paySlotItems = new List<UIPaySlotItem>();

		private PaySlotViewModule.OpenData mOpenData;

		private SequencePool sequencePool = new SequencePool();

		public class OpenData
		{
			public int seed;

			public int slotId;
		}
	}
}
