using System;
using DG.Tweening;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Chapter;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIMainPageTabNodeBattle : UIBaseMainPageTabNode
	{
		public override float ElementMax
		{
			get
			{
				return 150f;
			}
		}

		protected override void OnInit()
		{
			base.OnInit();
			if (this.m_other == null)
			{
				return;
			}
			if (this.m_btn_battle != null)
			{
				this.m_btn_battle.onClick.AddListener(new UnityAction(this.OnClickBattleBt));
			}
			if (this.m_redNode_battle != null)
			{
				this.m_redNode_battle.SetType(240);
				this.m_redNode_battle.Value = 0;
			}
			this.reClickAnimationTarget = this.m_iconParent_battle;
			this.m_iconParent_battleAnimatorListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			this.OnLanguageChange();
		}

		protected override void PlayUpdate(bool isFinished, float progress)
		{
			base.PlayUpdate(isFinished, progress);
			if (this.m_other != null)
			{
				Utility.SetTop(this.m_other, Mathf.Lerp(this.m_fromBtnTop, this.m_toBtnTop, progress));
				Utility.SetBottom(this.m_other, Mathf.Lerp(this.m_fromBtnBottom, this.m_toBtnBottom, progress));
			}
			if (this.m_iconParent != null)
			{
				this.m_iconParent_battle.anchoredPosition = Vector2.Lerp(this.m_fromIconPos, this.m_toIconPos, progress);
				this.m_iconParent_battle.localScale = Vector2.Lerp(this.m_fromIconScale, this.m_toIconScale, progress);
			}
		}

		public override void OnLanguageChange()
		{
			if (this.m_pageNameTxt != null)
			{
				this.m_pageNameTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(base.m_languageID);
			}
			if (this.m_pageNameTxt_battle != null)
			{
				this.m_pageNameTxt_battle.text = Singleton<LanguageManager>.Instance.GetInfoByID(base.m_languageID);
			}
		}

		protected override void OnDeInit()
		{
			base.OnDeInit();
			if (this.m_btn_battle != null)
			{
				this.m_btn_battle.onClick.RemoveListener(new UnityAction(this.OnClickBattleBt));
			}
			if (this.m_iconParent_battleAnimatorListen != null)
			{
				this.m_iconParent_battleAnimatorListen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
		}

		public override void OnSelect(bool isSelect, bool isLerp = false)
		{
			if (!this.isSelected || !isSelect)
			{
				if (this.reClickAnimationTarget != null)
				{
					ShortcutExtensions.DOKill(this.reClickAnimationTarget, false);
				}
				if (this.m_other != null)
				{
					this.m_other.gameObject.SetActive(isSelect);
				}
				if (isSelect)
				{
					if (!string.IsNullOrEmpty(base.m_redPointName))
					{
						RedPointController.Instance.ClickRecord(base.m_redPointName);
					}
					if (this.m_default != null)
					{
						this.m_default.gameObject.SetActive(false);
					}
					if (this.m_other != null)
					{
						this.m_other.gameObject.SetActive(true);
					}
					if (this.m_bgUnselect_battle != null)
					{
						this.m_bgUnselect_battle.gameObject.SetActive(false);
					}
					if (this.m_bgselect_battle != null)
					{
						this.m_bgselect_battle.gameObject.SetActive(true);
					}
					if (this.m_activeObj != null)
					{
						this.m_activeObj.SetActive(true);
					}
					if (!isLerp)
					{
						if (this.m_other != null)
						{
							Utility.SetTop(this.m_other, this.BtnTopMax);
							Utility.SetBottom(this.m_other, this.BtnBottomMax);
						}
						this.m_iconParent_battle.anchoredPosition = this.IconPosMax;
						this.m_iconParent_battle.localScale = this.IconScaleMax;
						this.m_iconParent_battleAnimator.SetTrigger("OnSelect");
					}
					else
					{
						this.m_fromBtnTop = this.BtnTopMin;
						this.m_toBtnTop = this.BtnTopMax;
						this.m_fromBtnBottom = this.BtnBottomMin;
						this.m_toBtnBottom = this.BtnBottomMax;
						this.m_fromIconScale = this.m_iconParent_battle.localScale;
						this.m_toIconScale = this.IconScaleMax;
						this.m_fromIconPos = this.m_iconParent_battle.anchoredPosition;
						this.m_toIconPos = this.IconPosMax;
						this.m_iconParent_battleAnimator.SetTrigger("Select");
					}
				}
				else
				{
					if (this.m_bgUnselect_battle != null)
					{
						this.m_bgUnselect_battle.gameObject.SetActive(true);
					}
					if (this.m_bgselect_battle != null)
					{
						this.m_bgselect_battle.gameObject.SetActive(false);
					}
					if (this.m_activeObj != null)
					{
						this.m_activeObj.SetActive(false);
					}
					if (!isLerp)
					{
						if (this.m_default != null)
						{
							this.m_default.gameObject.SetActive(true);
						}
						if (this.m_other != null)
						{
							this.m_other.gameObject.SetActive(false);
						}
						if (this.m_other != null)
						{
							Utility.SetTop(this.m_other, this.BtnTopMin);
							Utility.SetBottom(this.m_other, this.BtnBottomMin);
						}
						this.m_iconParent_battle.anchoredPosition = this.IconPosMin;
						this.m_iconParent_battle.localScale = this.IconScaleMin;
					}
					else
					{
						if (this.m_other != null)
						{
							this.m_other.gameObject.SetActive(true);
						}
						this.m_fromBtnTop = this.BtnTopMax;
						this.m_toBtnTop = this.BtnTopMin;
						this.m_fromBtnBottom = this.BtnBottomMax;
						this.m_toBtnBottom = this.BtnBottomMin;
						this.m_fromIconScale = this.m_iconParent_battle.localScale;
						this.m_toIconScale = this.IconScaleMin;
						this.m_fromIconPos = this.m_iconParent_battle.anchoredPosition;
						this.m_toIconPos = this.IconPosMin;
						this.m_iconParent_battleAnimator.SetTrigger("UnSelect");
					}
				}
				base.OnSelect(isSelect, isLerp);
				return;
			}
			if (base.m_isPlaying)
			{
				return;
			}
			this.PlayReClickAnimation(this.IconScaleMax);
		}

		public override void OnRefreshFunctionOpenState()
		{
			bool flag = base.IsLock();
			base.SetLock(flag);
		}

		protected override void OnRedPointChange(RedNodeListenData obj)
		{
			base.OnRedPointChange(obj);
		}

		private void OnAnimatorListen(GameObject arg1, string arg2)
		{
			if (this.m_default != null)
			{
				this.m_default.gameObject.SetActive(true);
			}
			if (this.m_other != null)
			{
				this.m_other.gameObject.SetActive(false);
			}
		}

		private void OnClickBattleBt()
		{
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			if ((ulong)GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.UserLife).NewNum < (ulong)((long)dataModule.CurrentChapter.CostEnergy))
			{
				EventArgsString eventArgsString = new EventArgsString();
				eventArgsString.SetData("体力不足");
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddTextTipNode, eventArgsString);
				return;
			}
			NetworkUtils.Chapter.DoStartChapterRequest(dataModule.ChapterID, delegate(bool isOk, StartChapterResponse res)
			{
				if (isOk)
				{
					GameApp.View.OpenView(ViewName.LoadingBattleViewModule, null, 2, null, delegate(GameObject obj)
					{
						GameApp.View.GetViewModule(ViewName.LoadingBattleViewModule).PlayShow(delegate
						{
							EventArgsGameDataEnter instance = Singleton<EventArgsGameDataEnter>.Instance;
							instance.SetData(GameModel.Chapter, null);
							GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameData_GameEnter, instance);
							GameApp.State.ActiveState(StateName.BattleChapterState);
						});
					});
				}
			});
		}

		[Header("Other")]
		[SerializeField]
		private Button m_btn_battle;

		[SerializeField]
		private RectTransform m_unlockNode_battle;

		[SerializeField]
		private RectTransform m_iconParent_battle;

		[SerializeField]
		private Animator m_iconParent_battleAnimator;

		[SerializeField]
		private AnimatorListen m_iconParent_battleAnimatorListen;

		[SerializeField]
		private RedNodeOneCtrl m_redNode_battle;

		[SerializeField]
		private GameObject m_activeObj;

		[SerializeField]
		private CustomText m_pageNameTxt_battle;

		[SerializeField]
		private RectTransform m_bgUnselect_battle;

		[SerializeField]
		private RectTransform m_bgselect_battle;

		private float m_fromBtnTop;

		private float m_toBtnTop;

		private float BtnTopMin;

		private float BtnTopMax;

		private float m_fromBtnBottom;

		private float m_toBtnBottom;

		private float BtnBottomMin;

		private float BtnBottomMax;

		private Vector2 m_fromIconPos;

		private Vector2 m_toIconPos;

		private Vector2 IconPosMin = new Vector2(0f, 0f);

		private Vector2 IconPosMax = new Vector2(0f, 50f);

		private Vector3 m_fromIconScale;

		private Vector3 m_toIconScale;

		private Vector3 IconScaleMin = new Vector3(1f, 1f, 1f);

		private Vector3 IconScaleMax = Vector3.one * 1.25f;

		private bool m_isSelect;
	}
}
