using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.UI;
using Proto.Pay;
using UnityEngine;

namespace HotFix
{
	public class IAPBattlePassCellNormal : IAPBattlePassBaseCell
	{
		public IAPBattlePassData Data
		{
			get
			{
				return this.mData;
			}
		}

		public int ID
		{
			get
			{
				if (this.mData == null)
				{
					return 0;
				}
				return this.mData.ID;
			}
		}

		public int Score
		{
			get
			{
				if (this.mData == null)
				{
					return 0;
				}
				return this.mData.Score;
			}
		}

		public BattlePassType Type
		{
			get
			{
				if (this.mData == null)
				{
					return BattlePassType.Normal;
				}
				return this.mData.Type;
			}
		}

		protected override void OnInit()
		{
			this.mDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.ObjFreeHasRewards.SetActive(false);
			this.ObjPayHasRewards.SetActive(false);
			this.PrefabItem.SetActive(false);
			this.ObjMask.SetActive(false);
			this.BuyScoreUI.Init();
			this.isShowAni = false;
		}

		protected override void OnDeInit()
		{
			IAPBattlePassCellBuyScoreUI buyScoreUI = this.BuyScoreUI;
			if (buyScoreUI != null)
			{
				buyScoreUI.DeInit();
			}
			this.DelInitItems();
		}

		private void DelInitItems()
		{
			if (this.ItemForFree != null)
			{
				GameObject gameObject = this.ItemForFree.gameObject;
				this.ItemForFree.DeInit();
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
			}
			this.ItemForFree = null;
			for (int i = 0; i < this.ItemsForPay.Count; i++)
			{
				GameObject gameObject2 = this.ItemsForPay[i].gameObject;
				this.ItemsForPay[i].DeInit();
				if (gameObject2 != null)
				{
					Object.Destroy(gameObject2);
				}
			}
			this.ItemsForPay.Clear();
		}

		public override void SetData(IAPBattlePassData data, int index, Action onClickReward)
		{
			this.mData = data;
			this.ShowScore = this.mData.Score;
			this.Index = index;
			this.OnClickReward = onClickReward;
		}

		public override void SetStatus(int curscore, bool freehaveget, bool payhaveget)
		{
			this.mCurUserScore = curscore;
			this.mFreeHaveGet = freehaveget;
			this.mPayHaveGet = payhaveget;
		}

		public override void RefreshUI(SequencePool m_seqPool)
		{
			if (this.mData == null || base.gameObject == null)
			{
				return;
			}
			bool hasBuy = this.mDataModule.BattlePass.HasBuy;
			this.ObjMask.SetActive(this.mData.Score > this.mCurUserScore);
			bool flag = this.Score > this.mCurUserScore;
			bool flag2 = this.Score > this.mCurUserScore || !hasBuy;
			if (this.ItemForFree == null)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.PrefabItem, this.TFItemForFreeRoot);
				gameObject.name = "freeitem";
				this.ItemForFree = gameObject.GetComponent<IAPBattlePassCellItem>();
				this.ItemForFree.Item.SetCountShowType(UIItem.CountShowType.MissOne);
				gameObject.SetActive(true);
				Transform transform = gameObject.transform;
				if (m_seqPool != null && !this.isShowAni)
				{
					transform.localScale = Vector3.zero;
					TweenSettingsExtensions.Append(m_seqPool.Get(), ShortcutExtensions.DOScale(transform, 1f, 0.3f));
				}
				else
				{
					transform.localScale = Vector3.one;
				}
			}
			bool flag3 = !this.mFreeHaveGet && this.mData.Score <= this.mCurUserScore;
			this.ItemForFree.SetData(this.mData.FreeReward);
			this.ItemForFree.Init();
			this.ItemForFree.OnRefresh();
			this.ItemForFree.SetBattlePassState(this.mFreeHaveGet, flag, this.mFreeHaveGet, flag3);
			this.ItemForFree.Item.onClick = new Action<UIItem, PropData, object>(this.OnClickItemFree);
			bool flag4 = !this.mPayHaveGet && this.mData.Score <= this.mCurUserScore && hasBuy;
			List<PropData> payRewards = this.mData.PayRewards;
			if (this.ItemsForPay.Count != payRewards.Count)
			{
				for (int i = 0; i < this.ItemsForPay.Count; i++)
				{
					GameObject gameObject2 = this.ItemsForPay[i].gameObject;
					this.ItemsForPay[i].DeInit();
					if (gameObject2 != null)
					{
						Object.Destroy(gameObject2);
					}
				}
				this.ItemsForPay.Clear();
				for (int j = 0; j < payRewards.Count; j++)
				{
					GameObject gameObject3 = Object.Instantiate<GameObject>(this.PrefabItem, this.TFItemForPayRoot);
					gameObject3.name = "payitem_" + j.ToString();
					IAPBattlePassCellItem component = gameObject3.GetComponent<IAPBattlePassCellItem>();
					component.Item.SetCountShowType(UIItem.CountShowType.MissOne);
					this.ItemsForPay.Add(component);
					gameObject3.SetActive(true);
					Transform transform2 = gameObject3.transform;
					if (m_seqPool != null && !this.isShowAni)
					{
						transform2.localScale = Vector3.zero;
						TweenSettingsExtensions.Append(m_seqPool.Get(), ShortcutExtensions.DOScale(transform2, 1f, 0.3f));
					}
					else
					{
						transform2.localScale = Vector3.one;
					}
				}
			}
			for (int k = 0; k < payRewards.Count; k++)
			{
				IAPBattlePassCellItem iapbattlePassCellItem = this.ItemsForPay[k];
				iapbattlePassCellItem.SetData(payRewards[k]);
				iapbattlePassCellItem.Init();
				iapbattlePassCellItem.OnRefresh();
				iapbattlePassCellItem.SetBattlePassState(this.mPayHaveGet, flag2, this.mPayHaveGet, flag4);
				iapbattlePassCellItem.Item.onClick = new Action<UIItem, PropData, object>(this.OnClickItemPay);
			}
			this.TextScore.text = this.mData.Level.ToString();
			int currentScore = this.mDataModule.BattlePass.CurrentScore;
			if (this.mData.Score <= currentScore)
			{
				this.BuyScoreUI.OnBuyScore = null;
				this.BuyScoreUI.SetData(null, 0, 0);
			}
			else if (this.mData.LastScore <= currentScore)
			{
				if (this.mDataModule.BattlePass.IsOpenBuyScore())
				{
					int num = this.mData.Score - currentScore;
					float battlePass_DiamonScoreRate = Singleton<GameConfig>.Instance.BattlePass_DiamonScoreRate;
					int num2 = Mathf.CeilToInt((float)num * battlePass_DiamonScoreRate);
					this.BuyScoreUI.SetData(this.mData, num, num2);
					this.BuyScoreUI.OnBuyScore = new Action(this.OnBuyScore);
					base.transform.SetAsLastSibling();
				}
				else
				{
					this.BuyScoreUI.OnBuyScore = null;
					this.BuyScoreUI.SetData(null, 0, 0);
				}
			}
			else
			{
				this.BuyScoreUI.OnBuyScore = null;
				this.BuyScoreUI.SetData(null, 0, 0);
			}
			this.RefreshProgressUI();
			this.isShowAni = true;
		}

		private void RefreshProgressUI()
		{
			float height = this.RTFProgressBG.rect.height;
			float num = ((this.mCurUserScore >= this.mData.Score) ? 0f : height);
			Utility.SetBottom(this.RTFProgressFront, num);
		}

		public bool IfCanGetReward(bool showtips = true)
		{
			IAPBattlePass battlePass = this.mDataModule.BattlePass;
			if (battlePass == null)
			{
				return false;
			}
			bool flag;
			bool flag2;
			battlePass.GetBattlePassRewardGet(this.mData.ID, out flag, out flag2);
			if (flag && flag2)
			{
				return false;
			}
			if (flag)
			{
				if (battlePass.HasBuy)
				{
					return true;
				}
				if (showtips)
				{
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("uibattlepass_unlock_tip"));
				}
				return false;
			}
			else
			{
				if (battlePass.CurrentScore >= this.mData.Score)
				{
					return true;
				}
				if (showtips)
				{
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("uibattlepass_noscore"));
				}
				return false;
			}
		}

		public bool IfCanBuyScore()
		{
			IAPBattlePass battlePass = this.mDataModule.BattlePass;
			if (battlePass == null)
			{
				return false;
			}
			if (this.mDataModule.BattlePass.IsOpenBuyScore())
			{
				IAPBattlePassData lastData = battlePass.GetLastData(this.mData.ID);
				if (lastData == null || battlePass.CurrentScore >= lastData.Score)
				{
					return true;
				}
			}
			return false;
		}

		public bool ShowBuyScore(bool showtips)
		{
			IAPBattlePass battlePass = this.mDataModule.BattlePass;
			if (battlePass == null)
			{
				return false;
			}
			int scoreneed = this.mData.Score - battlePass.CurrentScore;
			float battlePass_DiamonScoreRate = Singleton<GameConfig>.Instance.BattlePass_DiamonScoreRate;
			int num = Mathf.CeilToInt((float)scoreneed * battlePass_DiamonScoreRate);
			if (DxxTools.UI.CheckCurrencyAndShowTips(2, num, showtips))
			{
				DxxTools.UI.OpenPopCommon(Singleton<LanguageManager>.Instance.GetInfoByID("uibattlepass_buy_tip", new object[] { num, scoreneed }), delegate(int result)
				{
					if (result == 1)
					{
						this.BuyScore(scoreneed);
					}
				});
				return true;
			}
			return false;
		}

		private void OnClickItemPay(UIItem item, PropData data, object obj)
		{
			if (this.mDataModule == null || this.mDataModule.BattlePass == null || this.mDataModule.BattlePass.BattlePassID == 0)
			{
				return;
			}
			if (!this.mDataModule.BattlePass.HasBuy)
			{
				if (this.OnClickGotoBuy != null)
				{
					this.OnClickGotoBuy();
				}
				return;
			}
			IAPBattlePass battlePass = this.mDataModule.BattlePass;
			if (battlePass == null)
			{
				return;
			}
			bool flag;
			bool flag2;
			battlePass.GetBattlePassRewardGet(this.mData.ID, out flag, out flag2);
			if (flag2)
			{
				item.OnBtnItemClick(item, data, obj);
				return;
			}
			if (this.IfCanGetReward(false))
			{
				this.OnTryGetRewards();
				return;
			}
			if (this.IfCanBuyScore())
			{
				this.ShowBuyScore(false);
				return;
			}
		}

		private void OnClickItemFree(UIItem item, PropData data, object obj)
		{
			IAPBattlePass battlePass = this.mDataModule.BattlePass;
			if (battlePass == null)
			{
				return;
			}
			bool flag;
			bool flag2;
			battlePass.GetBattlePassRewardGet(this.mData.ID, out flag, out flag2);
			if (flag)
			{
				item.OnBtnItemClick(item, data, obj);
				return;
			}
			if (this.IfCanGetReward(false))
			{
				this.OnTryGetRewards();
				return;
			}
			if (this.IfCanBuyScore())
			{
				this.ShowBuyScore(false);
				return;
			}
		}

		private void OnBuyScore()
		{
			if (this.IfCanBuyScore())
			{
				this.ShowBuyScore(true);
			}
		}

		protected virtual void OnTryGetRewards()
		{
			Action onClickReward = this.OnClickReward;
			if (onClickReward == null)
			{
				return;
			}
			onClickReward();
		}

		private void BuyScore(int scoreneed)
		{
			if (base.gameObject == null)
			{
				return;
			}
			if (this.mData == null)
			{
				return;
			}
			NetworkUtils.Purchase.BattlePassBuyScoreRequest((uint)scoreneed, delegate(bool result, BattlePassChangeScoreResponse resp)
			{
			});
		}

		public GameObject PrefabItem;

		[Header("免费物品")]
		public Transform TFItemForFreeRoot;

		[NonSerialized]
		public IAPBattlePassCellItem ItemForFree;

		public GameObject ObjFreeHasRewards;

		[Header("付费物品")]
		public Transform TFItemForPayRoot;

		[NonSerialized]
		public List<IAPBattlePassCellItem> ItemsForPay = new List<IAPBattlePassCellItem>();

		public GameObject ObjPayHasRewards;

		[Header("积分")]
		public GameObject ObjScore;

		public CustomText TextScore;

		[Header("进度")]
		public GameObject ObjProgress;

		public RectTransform RTFProgressFront;

		public RectTransform RTFProgressBG;

		[Header("遮罩")]
		public GameObject ObjMask;

		[Header("购买积分")]
		public IAPBattlePassCellBuyScoreUI BuyScoreUI;

		[Label]
		public int ShowScore;

		private IAPDataModule mDataModule;

		private IAPBattlePassData mData;

		private int mCurUserScore;

		private bool mFreeHaveGet;

		private bool mPayHaveGet;

		private bool isShowAni;

		private Action OnClickReward;
	}
}
