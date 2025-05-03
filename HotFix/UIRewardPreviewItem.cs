using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIRewardPreviewItem : CustomBehaviour
	{
		public int Index { get; private set; }

		protected override void OnInit()
		{
			this.copyItem.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.itemList.Count; i++)
			{
				this.itemList[i].DeInit();
			}
			this.itemList.Clear();
		}

		public void SetData(int index, ChapterActivity_ActvTurntableReward cfg)
		{
			this.Index = index;
			ChapterActivityWheelDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule);
			if (cfg.special > 0)
			{
				this.imageBg.sprite = this.spriteRegister.GetSprite("special");
			}
			else
			{
				this.imageBg.sprite = this.spriteRegister.GetSprite("normal");
			}
			this.textProgress.text = DxxTools.FormatNumber((long)cfg.score);
			ChapterActivity_ActvTurntableReward currentReward = dataModule.GetCurrentReward();
			if (currentReward != null)
			{
				if (currentReward.id > cfg.id)
				{
					this.sliderProgress.value = 1f;
				}
				else if (currentReward.id < cfg.id)
				{
					this.sliderProgress.value = 0f;
				}
				else
				{
					ValueTuple<int, int> rewardProgress = dataModule.GetRewardProgress();
					float num = (float)rewardProgress.Item1 / (float)rewardProgress.Item2;
					if (num > 1f)
					{
						num = 1f;
					}
					this.sliderProgress.value = num;
				}
				this.finishObj.SetActiveSafe(currentReward.id > cfg.id);
			}
			else
			{
				this.sliderProgress.value = 1f;
				this.finishObj.SetActiveSafe(true);
			}
			List<ItemData> list = cfg.freeReward.ToItemDataList();
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
					gameObject.SetParentNormal(this.parent, false);
					uiitem = gameObject.GetComponent<UIItem>();
					uiitem.Init();
					this.itemList.Add(uiitem);
				}
				uiitem.gameObject.SetActiveSafe(true);
				uiitem.SetData(list[j].ToPropData());
				uiitem.OnRefresh();
			}
		}

		public void SetData(int index, ChapterActivity_ChapterObj cfg, ChapterActivityData chapterActivityData)
		{
			this.Index = index;
			if (cfg.special > 0)
			{
				this.imageBg.sprite = this.spriteRegister.GetSprite("special");
			}
			else
			{
				this.imageBg.sprite = this.spriteRegister.GetSprite("normal");
			}
			this.textProgress.text = DxxTools.FormatNumber((long)cfg.score);
			ChapterActivity_ChapterObj chapterActivity_ChapterObj = null;
			if (chapterActivityData != null)
			{
				chapterActivity_ChapterObj = chapterActivityData.CurrentProgress;
			}
			if (chapterActivity_ChapterObj != null)
			{
				if ((ulong)chapterActivityData.TotalScore >= (ulong)((long)chapterActivity_ChapterObj.score))
				{
					this.sliderProgress.value = 1f;
					this.finishObj.SetActiveSafe(true);
				}
				else
				{
					this.sliderProgress.value = 0f;
					if (chapterActivity_ChapterObj.id > cfg.id)
					{
						this.sliderProgress.value = 1f;
					}
					else if (chapterActivity_ChapterObj.id < cfg.id)
					{
						this.sliderProgress.value = 0f;
					}
					else
					{
						float num = chapterActivityData.TotalScore / (float)chapterActivity_ChapterObj.score;
						if (num > 1f)
						{
							num = 1f;
						}
						this.sliderProgress.value = num;
					}
					this.finishObj.SetActiveSafe(chapterActivity_ChapterObj.id > cfg.id);
				}
			}
			else
			{
				this.sliderProgress.value = 1f;
				this.finishObj.SetActiveSafe(true);
			}
			List<ItemData> list = cfg.reward.ToItemDataList();
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
					gameObject.SetParentNormal(this.parent, false);
					uiitem = gameObject.GetComponent<UIItem>();
					uiitem.Init();
					this.itemList.Add(uiitem);
				}
				uiitem.gameObject.SetActiveSafe(true);
				uiitem.SetData(list[j].ToPropData());
				uiitem.OnRefresh();
			}
		}

		public CustomImage imageBg;

		public SpriteRegister spriteRegister;

		public GameObject parent;

		public GameObject copyItem;

		public Slider sliderProgress;

		public CustomText textProgress;

		public GameObject finishObj;

		private List<UIItem> itemList = new List<UIItem>();
	}
}
