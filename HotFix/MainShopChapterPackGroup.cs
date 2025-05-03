using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class MainShopChapterPackGroup : MainShopPackGroupBase
	{
		public int CurIndex
		{
			get
			{
				return this.curIndex;
			}
			set
			{
				this.curIndex = value;
				this.UpdateBtnArrow();
			}
		}

		private void UpdateBtnArrow()
		{
			if (this.dataList.Count <= 1)
			{
				this.btnArrowLeft.gameObject.SetActive(false);
				this.btnArrowRight.gameObject.SetActive(false);
				return;
			}
			if (this.curIndex <= 0)
			{
				this.btnArrowLeft.gameObject.SetActive(false);
				this.btnArrowRight.gameObject.SetActive(true);
				return;
			}
			if (this.curIndex >= this.dataList.Count - 1)
			{
				this.btnArrowLeft.gameObject.SetActive(true);
				this.btnArrowRight.gameObject.SetActive(false);
				return;
			}
			this.btnArrowLeft.gameObject.SetActive(true);
			this.btnArrowRight.gameObject.SetActive(true);
		}

		protected override void OnInit()
		{
			this.curItem.Init();
			this.oldItem.Init();
			this.btnArrowLeft.onClick.AddListener(delegate
			{
				if (this.CurIndex > 0)
				{
					int num = this.CurIndex;
					int num2 = this.CurIndex;
					this.CurIndex = num2 - 1;
					this.PlayShow(this.CurIndex, num);
				}
			});
			this.btnArrowRight.onClick.AddListener(delegate
			{
				if (this.CurIndex < this.dataList.Count - 1)
				{
					int num3 = this.CurIndex;
					int num4 = this.CurIndex;
					this.CurIndex = num4 + 1;
					this.PlayShow(this.CurIndex, num3);
				}
			});
		}

		private void PlayShow(int cur, int old)
		{
			this.isLockUpdate = true;
			DOTween.Kill(this, false);
			this.oldItem.gameObject.SetActive(true);
			this.curItem.SetData(this.dataList[cur]);
			this.oldItem.SetData(this.dataList[old]);
			Vector2 anchoredPosition = this.centerPosHolder.anchoredPosition;
			this.oldItemRectTrans.anchoredPosition = anchoredPosition;
			Vector2 vector = anchoredPosition;
			vector.x += (float)((cur > old) ? 1080 : (-1080));
			this.curItemRectTrans.anchoredPosition = vector;
			Vector2 vector2 = anchoredPosition;
			vector2.x -= (float)((cur > old) ? 1080 : (-1080));
			TweenSettingsExtensions.Append(this.seqPool.Get(), ShortcutExtensions46.DOAnchorPos(this.curItemRectTrans, anchoredPosition, 0.3f, false));
			Sequence sequence = this.seqPool.Get();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPos(this.oldItemRectTrans, vector2, 0.3f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.isLockUpdate = false;
				this.oldItemRectTrans.gameObject.SetActive(false);
				if (this.needUpdate)
				{
					this.UpdateContent();
				}
			});
		}

		protected override void OnDeInit()
		{
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 1;
			subPriority = 0;
		}

		public override void UpdateContent()
		{
			if (this.isLockUpdate)
			{
				this.needUpdate = true;
				return;
			}
			this.dataList.Clear();
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			int id = GameApp.Data.GetDataModule(DataName.ChapterDataModule).CurrentChapter.id;
			IList<IAP_ChapterPacks> allElements = GameApp.Table.GetManager().GetIAP_ChapterPacksModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				if (!dataModule.Common.IsLimitCount(allElements[i].id) && id >= allElements[i].chapterId)
				{
					this.dataList.Add(allElements[i]);
					if (this.CurIndex < 0 && id == allElements[i].chapterId)
					{
						this.CurIndex = this.dataList.Count - 1;
					}
				}
			}
			if (this.dataList.Count == 0)
			{
				base.gameObject.SetActive(false);
				return;
			}
			base.gameObject.SetActive(true);
			this.dataList.Sort((IAP_ChapterPacks a, IAP_ChapterPacks b) => a.orderId.CompareTo(b.orderId));
			if (this.CurIndex < 0 || this.CurIndex >= this.dataList.Count)
			{
				this.CurIndex = this.dataList.Count - 1;
			}
			if (this.CurIndex < 0)
			{
				base.gameObject.SetActive(false);
				return;
			}
			this.UpdateBtnArrow();
			this.curItem.gameObject.SetActive(true);
			this.oldItem.gameObject.SetActive(false);
			this.curItemRectTrans.anchoredPosition = this.centerPosHolder.anchoredPosition;
			this.curItem.SetData(this.dataList[this.CurIndex]);
		}

		public override int PlayAnimation(float startTime, int index)
		{
			this.btnArrowLeft.gameObject.SetActive(false);
			this.btnArrowRight.gameObject.SetActive(false);
			this.titleFg.gameObject.AddComponent<EnterMoveXAnimationCtrl>().PlayShowAnimation(startTime, index, 10024);
			this.curItem.gameObject.AddComponent<EnterMoveXAnimationCtrl>().PlayShowAnimation(startTime, index + 1, 10024).onCompleted = new Action(this.UpdateBtnArrow);
			return index + 2;
		}

		public CustomButton btnArrowLeft;

		public CustomButton btnArrowRight;

		public MainShopChapterPackItem curItem;

		public MainShopChapterPackItem oldItem;

		public RectTransform centerPosHolder;

		public RectTransform curItemRectTrans;

		public RectTransform oldItemRectTrans;

		private int curIndex = -1;

		private List<IAP_ChapterPacks> dataList = new List<IAP_ChapterPacks>();

		private SequencePool seqPool = new SequencePool();

		private bool isLockUpdate;

		private bool needUpdate;
	}
}
