using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIEventProgressCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.nodeItem.gameObject.SetActiveSafe(false);
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			this.totalDay = dataModule.CurrentChapter.TotalStage;
		}

		protected override void OnDeInit()
		{
			this.progressList.Clear();
			this.nodeItems.Clear();
			this.fillInItems.Clear();
			this.templist.Clear();
		}

		public void SetShow(bool isShow)
		{
			if (isShow)
			{
				this.PlayShowAni();
				return;
			}
			this.PlayHideAni();
		}

		public void SetData(List<GameEventProgressData> progressDataList)
		{
			this.progressList.Clear();
			this.progressList.AddRange(progressDataList);
			this.CreateNodes();
			for (int i = 0; i < this.nodeItems.Count; i++)
			{
				this.nodeItems[i].Init();
			}
			for (int j = 0; j < this.fillInItems.Count; j++)
			{
				this.fillInItems[j].Init();
			}
		}

		private void CreateNodes()
		{
			if (this.pathNode.Count < 5)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				Vector3 position = this.pathNode[i + 1].transform.position;
				GameObject gameObject = this.nodeParent;
				if (i == 3)
				{
					gameObject = this.topParent;
				}
				UIEventProgressNodeItem component = this.CreateNode(position, gameObject).GetComponent<UIEventProgressNodeItem>();
				this.nodeItems.Add(component);
			}
			for (int j = 0; j < 3; j++)
			{
				Vector3 position2 = this.pathNode[this.pathNode.Count - 1].transform.position;
				GameObject gameObject2 = this.CreateNode(position2, this.fillInParent);
				gameObject2.SetActiveSafe(false);
				UIEventProgressNodeItem component2 = gameObject2.GetComponent<UIEventProgressNodeItem>();
				this.fillInItems.Add(component2);
			}
		}

		private GameObject CreateNode(Vector3 position, GameObject parent)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.nodeItem);
			gameObject.SetParentNormal(parent, false);
			gameObject.transform.position = position;
			gameObject.SetActiveSafe(true);
			return gameObject;
		}

		private List<GameEventProgressData> GetShowList()
		{
			List<GameEventProgressData> list = new List<GameEventProgressData>();
			for (int i = 0; i < this.progressList.Count; i++)
			{
				GameEventProgressData gameEventProgressData = this.progressList[i];
				if (gameEventProgressData.stage > this.currentDay)
				{
					if (list.Count < 3)
					{
						if (gameEventProgressData.stage > this.currentDay && list.Count == 0 && i != 0)
						{
							list.Add(this.progressList[i - 1]);
							list.Add(gameEventProgressData);
						}
						else
						{
							list.Add(gameEventProgressData);
							if (list.Count == 3)
							{
								this.endDay = gameEventProgressData.stage;
							}
						}
					}
					else if (gameEventProgressData.type == GameEventType.BattleElite || gameEventProgressData.type == GameEventType.BattleBoss)
					{
						list.Add(gameEventProgressData);
						break;
					}
				}
			}
			if (this.progressList.Count > 0)
			{
				if (list.Count == 0)
				{
					list.Add(this.progressList[this.progressList.Count - 1]);
				}
				while (list.Count >= 0 && list.Count < 4)
				{
					GameEventProgressData gameEventProgressData2 = list[0];
					int num = this.progressList.IndexOf(gameEventProgressData2);
					if (num - 1 >= 0 && num - 1 < this.progressList.Count)
					{
						GameEventProgressData gameEventProgressData3 = this.progressList[num - 1];
						list.Insert(0, gameEventProgressData3);
					}
					else
					{
						list.Insert(0, gameEventProgressData2);
					}
				}
			}
			return list;
		}

		public void SetDay(int day)
		{
			this.currentDay = day;
			if (this.currentDay >= this.endDay && this.currentDay != this.totalDay)
			{
				List<GameEventProgressData> showList = this.GetShowList();
				if (showList.Count == 0)
				{
					return;
				}
				if (this.isSetData)
				{
					this.startDay = showList[0].stage;
					for (int i = 0; i < this.fillInItems.Count; i++)
					{
						this.fillInItems[i].SetData(showList[i], i);
					}
					this.templist.Clear();
					this.templist.AddRange(this.nodeItems);
					this.templist.RemoveAt(this.templist.Count - 1);
					if (this.templist[this.templist.Count - 1].GetData().stage == this.fillInItems[0].GetData().stage)
					{
						for (int j = 1; j < this.fillInItems.Count; j++)
						{
							this.templist.Add(this.fillInItems[j]);
						}
					}
					else
					{
						this.templist.AddRange(this.fillInItems);
					}
					this.ItemSetDay(this.currentDay, this.templist, true);
					this.StartAnimation();
					return;
				}
				this.isSetData = true;
				for (int k = 0; k < this.nodeItems.Count; k++)
				{
					this.nodeItems[k].SetData(showList[k], k);
				}
			}
			if (!this.isSetData && this.currentDay == this.totalDay)
			{
				List<GameEventProgressData> showList2 = this.GetShowList();
				if (showList2.Count == 0)
				{
					return;
				}
				this.isSetData = true;
				for (int l = 0; l < this.nodeItems.Count; l++)
				{
					this.nodeItems[l].SetData(showList2[l], l);
				}
			}
			this.ItemSetDay(day, this.nodeItems, true);
		}

		private void ItemSetDay(int day, List<UIEventProgressNodeItem> list, bool isAni = true)
		{
			for (int i = 0; i < list.Count; i++)
			{
				UIEventProgressNodeItem uieventProgressNodeItem = null;
				if (i + 1 < list.Count)
				{
					uieventProgressNodeItem = list[i + 1];
				}
				int num = ((uieventProgressNodeItem != null && uieventProgressNodeItem.GetData() != null) ? uieventProgressNodeItem.GetData().stage : day);
				list[i].SetDay(day, num, isAni);
			}
		}

		private void StartAnimation()
		{
			if (this.isHide)
			{
				return;
			}
			if (this.progressSeq != null && TweenExtensions.IsPlaying(this.progressSeq))
			{
				return;
			}
			if (this.templist[0].GetData().stage == this.startDay)
			{
				this.AnimationFinish();
				return;
			}
			this.progressSeq = DOTween.Sequence();
			float num = 0.5f;
			for (int i = 0; i < this.templist.Count; i++)
			{
				this.templist[i].gameObject.SetActiveSafe(true);
				this.templist[i].HideArrow();
				if (i == 0)
				{
					TweenSettingsExtensions.Append(this.progressSeq, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMoveX(this.templist[i].transform, this.pathNode[0].transform.position.x, num, false), 1));
					TweenSettingsExtensions.Join(this.progressSeq, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions46.DOFade(this.templist[i].canvasGroup, 0f, num), 1));
				}
				else
				{
					Vector3 position = this.templist[i - 1].transform.position;
					TweenSettingsExtensions.Join(this.progressSeq, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMoveX(this.templist[i].transform, position.x, num, false), 1));
				}
				if (i != this.templist.Count - 1)
				{
					this.templist[i].ShowSlider();
				}
			}
			TweenSettingsExtensions.AppendCallback(this.progressSeq, delegate
			{
				this.templist[0].gameObject.SetActiveSafe(false);
				this.templist.RemoveAt(0);
				this.StartAnimation();
			});
		}

		private void AnimationFinish()
		{
			List<GameEventProgressData> showList = this.GetShowList();
			for (int i = 0; i < this.nodeItems.Count; i++)
			{
				UIEventProgressNodeItem uieventProgressNodeItem = this.nodeItems[i];
				uieventProgressNodeItem.gameObject.SetActiveSafe(true);
				uieventProgressNodeItem.canvasGroup.alpha = 1f;
				uieventProgressNodeItem.SetData(showList[i], i);
				uieventProgressNodeItem.ResetInitPos();
				if (i == this.nodeItems.Count - 2 && this.progressList.Count - 2 >= 0 && this.progressList.Count - 2 < this.progressList.Count && uieventProgressNodeItem.GetData().stage == this.progressList[this.progressList.Count - 2].stage)
				{
					this.pointObj.SetActiveSafe(false);
					uieventProgressNodeItem.ShowSlider();
				}
			}
			this.ItemSetDay(this.currentDay, this.nodeItems, false);
			for (int j = 0; j < this.fillInItems.Count; j++)
			{
				this.fillInItems[j].gameObject.SetActiveSafe(false);
				this.fillInItems[j].canvasGroup.alpha = 1f;
				this.fillInItems[j].ResetInitPos();
			}
			this.ItemSetDay(this.currentDay, this.fillInItems, false);
		}

		private void PlayShowAni()
		{
			this.isHide = false;
			if (this.progressSeq != null)
			{
				TweenExtensions.Pause<Sequence>(this.progressSeq);
			}
			Sequence sequence = DOTween.Sequence();
			this.child.anchoredPosition = new Vector2(this.child.anchoredPosition.x, 300f);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.child, -20f, 0.3f, false)), ShortcutExtensions46.DOAnchorPosY(this.child, 0f, 0.2f, false));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				if (this.progressSeq != null)
				{
					TweenExtensions.Play<Sequence>(this.progressSeq);
				}
			});
		}

		private void PlayHideAni()
		{
			this.isHide = true;
			if (this.progressSeq != null)
			{
				TweenExtensions.Pause<Sequence>(this.progressSeq);
			}
			Sequence sequence = DOTween.Sequence();
			this.child.anchoredPosition = Vector2.zero;
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosY(this.child, -20f, 0.2f, false)), ShortcutExtensions46.DOAnchorPosY(this.child, 300f, 0.3f, false));
		}

		public List<GameObject> pathNode;

		public GameObject nodeItem;

		public GameObject nodeParent;

		public GameObject fillInParent;

		public GameObject topParent;

		public RectTransform child;

		public GameObject pointObj;

		private List<GameEventProgressData> progressList = new List<GameEventProgressData>();

		private List<UIEventProgressNodeItem> nodeItems = new List<UIEventProgressNodeItem>();

		private List<UIEventProgressNodeItem> fillInItems = new List<UIEventProgressNodeItem>();

		private List<UIEventProgressNodeItem> templist = new List<UIEventProgressNodeItem>();

		private int currentDay;

		private int endDay;

		private int startDay;

		private int totalDay;

		private Sequence progressSeq;

		private bool isHide;

		private bool isSetData;

		public const int ShowCount = 3;
	}
}
