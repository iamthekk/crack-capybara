using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIEventButtonsCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.rectTransform = base.GetComponent<RectTransform>();
			this.initPos = this.rectTransform.anchoredPosition;
			this.nextDayButtonCtrl.Init();
			this.oneButtonCtrl.Init();
			this.battleButtonCtrl.Init();
			for (int i = 0; i < this.buttonItems.Count; i++)
			{
				UIGameEventButtonItem uigameEventButtonItem = this.buttonItems[i];
				uigameEventButtonItem.m_onClick = new Action<int>(this.OnClickButton);
				uigameEventButtonItem.Init();
				uigameEventButtonItem.gameObject.SetActiveSafe(false);
			}
			this.nextDayButtonCtrl.gameObject.SetActiveSafe(false);
			this.oneButtonCtrl.gameObject.SetActiveSafe(false);
			this.battleButtonCtrl.gameObject.SetActiveSafe(false);
			this.ShowHand(false);
		}

		protected override void OnDeInit()
		{
			this.nextDayButtonCtrl.DeInit();
			this.oneButtonCtrl.DeInit();
			this.battleButtonCtrl.DeInit();
			this.sequencePool.Clear(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (((this.nextDayButtonCtrl.gameObject.activeSelf && this.nextDayButtonCtrl.IsUnLock()) || (this.oneButtonCtrl.gameObject.activeSelf && this.oneButtonCtrl.IsUnLock()) || (this.battleButtonCtrl.gameObject.activeSelf && this.battleButtonCtrl.IsUnLock())) && GameApp.View.IsLayerTop(925))
			{
				this.timer += deltaTime;
				if (this.timer >= 3f)
				{
					this.ShowHand(true);
				}
			}
		}

		public void RefreshButton(GameEventUIData data, Action onClick, Action aniFinish)
		{
			if (data == null)
			{
				return;
			}
			this.uiData = data;
			this.onClickNextDay = onClick;
			List<GameEventUIButtonData> buttons = this.uiData.GetButtons();
			this.battleButtonCtrl.gameObject.SetActiveSafe(false);
			if (buttons.Count >= 2)
			{
				this.battleButtonCtrl.gameObject.SetActiveSafe(false);
				this.oneButtonCtrl.gameObject.SetActiveSafe(false);
				for (int i = 0; i < this.buttonItems.Count; i++)
				{
					UIGameEventButtonItem uigameEventButtonItem = this.buttonItems[i];
					if (i < buttons.Count)
					{
						uigameEventButtonItem.Refresh(buttons[i]);
						GameTGATools.Ins.SetStageButtonContent(i, buttons[i].GetTGAString());
					}
				}
				this.ShowButtons(aniFinish);
				return;
			}
			if (buttons.Count != 1)
			{
				this.buttonsObj.SetActiveSafe(false);
				this.oneButtonCtrl.gameObject.SetActiveSafe(false);
				this.battleButtonCtrl.gameObject.SetActiveSafe(false);
				if (!this.nextDayButtonCtrl.gameObject.activeSelf)
				{
					this.ShowNextDayButton(aniFinish);
				}
				else if (aniFinish != null)
				{
					aniFinish();
				}
				this.nextDayButtonCtrl.SetData(new Action(this.OnClickNextDay));
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("UIGameEvent_NextDay");
				this.nextDayButtonCtrl.SetText(infoByID);
				GameTGATools.Ins.SetStageButtonContent(0, Singleton<LanguageManager>.Instance.GetInfoByID(2, "UIGameEvent_NextDay"));
				this.nextDayButtonCtrl.SetLock(this.isMoveToNpc);
				return;
			}
			if (buttons[0].isBattleButton)
			{
				this.oneButtonCtrl.gameObject.SetActiveSafe(false);
				this.buttonsObj.SetActiveSafe(false);
				if (!this.battleButtonCtrl.gameObject.activeSelf)
				{
					this.ShowBattleButton(aniFinish);
				}
				else if (aniFinish != null)
				{
					aniFinish();
				}
				this.battleButtonCtrl.SetData(new Action(this.OnClickOneButton));
				this.battleButtonCtrl.SetText(buttons[0].GetInfo());
				GameTGATools.Ins.SetStageButtonContent(0, buttons[0].GetTGAString());
				this.battleButtonCtrl.SetLock(this.isMoveToNpc);
				return;
			}
			this.battleButtonCtrl.gameObject.SetActiveSafe(false);
			this.buttonsObj.SetActiveSafe(false);
			if (!this.oneButtonCtrl.gameObject.activeSelf)
			{
				this.ShowOneButton(aniFinish);
			}
			else if (aniFinish != null)
			{
				aniFinish();
			}
			this.oneButtonCtrl.SetData(new Action(this.OnClickOneButton));
			this.oneButtonCtrl.SetText(buttons[0].GetInfo());
			GameTGATools.Ins.SetStageButtonContent(0, buttons[0].GetTGAString());
			this.oneButtonCtrl.SetLock(this.isMoveToNpc);
		}

		private void OnClickButton(int index)
		{
			if (this.isShowAni)
			{
				return;
			}
			if (GameApp.View.GetViewModule(ViewName.GameEventViewModule).IsClickEnabled() && this.uiData != null)
			{
				List<GameEventUIButtonData> buttons = this.uiData.GetButtons();
				if (index < buttons.Count)
				{
					EventArgUserSelected eventArgUserSelected = new EventArgUserSelected();
					eventArgUserSelected.buttonData = buttons[index];
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_UserSelected, eventArgUserSelected);
					this.HideButtons();
				}
			}
		}

		private void OnClickNextDay()
		{
			if (this.isShowAni)
			{
				return;
			}
			Action action = this.onClickNextDay;
			if (action != null)
			{
				action();
			}
			this.ShowHand(false);
		}

		private void OnClickOneButton()
		{
			if (this.isShowAni)
			{
				return;
			}
			if (GameApp.View.GetViewModule(ViewName.GameEventViewModule).IsClickEnabled() && this.uiData != null)
			{
				List<GameEventUIButtonData> buttons = this.uiData.GetButtons();
				if (buttons.Count > 0)
				{
					EventArgUserSelected eventArgUserSelected = new EventArgUserSelected();
					eventArgUserSelected.buttonData = buttons[0];
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_UserSelected, eventArgUserSelected);
					if (this.oneButtonCtrl.gameObject.activeSelf)
					{
						this.oneButtonCtrl.SetLock(true);
					}
					else if (this.battleButtonCtrl.gameObject.activeSelf)
					{
						this.battleButtonCtrl.SetLock(true);
					}
				}
			}
			this.ShowHand(false);
		}

		public void ShowButtons(Action aniFinish)
		{
			if (this.uiData == null)
			{
				return;
			}
			if (this.uiData.GetButtons().Count >= 2)
			{
				this.isShowAni = true;
				Sequence sequence = this.sequencePool.Get();
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.rectTransform, -300f, 0.2f, false));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.nextDayButtonCtrl.gameObject.SetActiveSafe(false);
					this.nextDayButtonCtrl.SetLock(this.isMoveToNpc);
					this.buttonsObj.SetActiveSafe(true);
					for (int i = 0; i < this.buttonItems.Count; i++)
					{
						this.buttonItems[i].gameObject.SetActive(true);
					}
				});
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.rectTransform, this.initPos.y, 0.2f, false));
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					for (int j = 0; j < this.buttonItems.Count; j++)
					{
						UIGameEventButtonItem uigameEventButtonItem = this.buttonItems[j];
						uigameEventButtonItem.Rebuild();
						uigameEventButtonItem.transform.localScale = Vector3.one;
					}
					this.isShowAni = false;
					Action aniFinish3 = aniFinish;
					if (aniFinish3 == null)
					{
						return;
					}
					aniFinish3();
				});
				return;
			}
			Action aniFinish2 = aniFinish;
			if (aniFinish2 == null)
			{
				return;
			}
			aniFinish2();
		}

		private void HideButtons()
		{
			this.buttonsObj.SetActiveSafe(false);
		}

		private void ShowOneButton(Action aniFinish)
		{
			this.isShowAni = true;
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.rectTransform, -300f, 0.2f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.oneButtonCtrl.gameObject.SetActiveSafe(true);
				this.nextDayButtonCtrl.gameObject.SetActiveSafe(false);
				this.nextDayButtonCtrl.SetLock(this.isMoveToNpc);
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.rectTransform, this.initPos.y, 0.2f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.isShowAni = false;
				Action aniFinish2 = aniFinish;
				if (aniFinish2 != null)
				{
					aniFinish2();
				}
				this.ShowHand(false);
			});
		}

		private void ShowBattleButton(Action aniFinish)
		{
			this.isShowAni = true;
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.rectTransform, -300f, 0.2f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.battleButtonCtrl.gameObject.SetActiveSafe(true);
				this.nextDayButtonCtrl.gameObject.SetActiveSafe(false);
				this.nextDayButtonCtrl.SetLock(this.isMoveToNpc);
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.rectTransform, this.initPos.y, 0.2f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.isShowAni = false;
				Action aniFinish2 = aniFinish;
				if (aniFinish2 != null)
				{
					aniFinish2();
				}
				this.ShowHand(false);
			});
		}

		private void ShowNextDayButton(Action aniFinish)
		{
			this.isShowAni = true;
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.rectTransform, -300f, 0.2f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.nextDayButtonCtrl.gameObject.SetActiveSafe(true);
			});
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.rectTransform, this.initPos.y, 0.2f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.isShowAni = false;
				Action aniFinish2 = aniFinish;
				if (aniFinish2 != null)
				{
					aniFinish2();
				}
				this.ShowHand(false);
			});
		}

		public void ShakeButton()
		{
			if (this.isShowAni)
			{
				return;
			}
			for (int i = 0; i < this.buttonItems.Count; i++)
			{
				if (this.buttonItems[i].gameObject.activeSelf)
				{
					this.buttonItems[i].ShakeButton();
				}
			}
			GameApp.Sound.PlayClip(56, 1f);
		}

		public void SetNextDayButtonDown()
		{
			this.nextDayButtonCtrl.SetLock(true);
		}

		public void SetNextDayButtonUp()
		{
			this.nextDayButtonCtrl.SetLock(false);
		}

		public bool IsNextDayButton()
		{
			return this.uiData != null && !this.uiData.IsShowButton();
		}

		public void SetNextDayFood(int food, int beginFood)
		{
			this.nextDayButtonCtrl.SetFood(food, beginFood);
		}

		public void SetNextDayHp(long hp, long maxHp)
		{
			this.nextDayButtonCtrl.SetHp(hp, maxHp);
		}

		public void SetMoveToNpc(bool isMoveTo)
		{
			this.isMoveToNpc = isMoveTo;
			this.oneButtonCtrl.SetLock(isMoveTo);
			this.battleButtonCtrl.SetLock(true);
			this.nextDayButtonCtrl.SetLock(isMoveTo);
		}

		public void ChangeInfo(string info)
		{
			if (this.oneButtonCtrl.gameObject.activeSelf)
			{
				this.oneButtonCtrl.SetText(info);
				return;
			}
			if (this.battleButtonCtrl.gameObject.activeSelf)
			{
				this.battleButtonCtrl.SetText(info);
			}
		}

		public void ChangeDefaultInfo()
		{
			if (this.uiData == null)
			{
				return;
			}
			List<GameEventUIButtonData> buttons = this.uiData.GetButtons();
			string text = "";
			if (buttons.Count > 0)
			{
				text = buttons[0].GetInfo();
			}
			if (this.oneButtonCtrl.gameObject.activeSelf)
			{
				this.oneButtonCtrl.SetText(text);
				return;
			}
			if (this.battleButtonCtrl.gameObject.activeSelf)
			{
				this.battleButtonCtrl.SetText(text);
			}
		}

		private void ShowHand(bool isShow)
		{
			this.handObj.SetActiveSafe(isShow);
			this.timer = 0f;
		}

		public List<UIGameEventButtonItem> buttonItems;

		public GameObject buttonsObj;

		public UINextDayButtonCtrl nextDayButtonCtrl;

		public UIOneButtonCtrl oneButtonCtrl;

		public UIBattleButtonCtrl battleButtonCtrl;

		public GameObject handObj;

		private GameEventUIData uiData;

		private SequencePool sequencePool = new SequencePool();

		private bool isShowAni;

		private Action onClickNextDay;

		private RectTransform rectTransform;

		private Vector2 initPos;

		private float timer;

		private bool isMoveToNpc;

		private const float DownY = -300f;

		private const float GuideInterval = 3f;
	}
}
