using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIGuildRacePageBattle : UIGuildRacePageBase
	{
		public GuildRaceBattleController BattleCtrl
		{
			get
			{
				return GuildRaceBattleController.Instance;
			}
		}

		protected override void GuildUI_OnInit()
		{
			this.mItemPool = LocalUnityObjctPool.Create(this.ScrollView.gameObject);
			this.GuildRaceItem_Battle0.SetActive(false);
			this.mItemPool.CreateCache("GuildRaceItem_Battle0", this.GuildRaceItem_Battle0.gameObject);
			this.GuildRaceItem_Battle1.SetActive(false);
			this.mItemPool.CreateCache("GuildRaceItem_Battle1", this.GuildRaceItem_Battle1.gameObject);
			this.GuildRaceItem_Battle2.SetActive(false);
			this.mItemPool.CreateCache("GuildRaceItem_Battle2", this.GuildRaceItem_Battle2.gameObject);
		}

		protected override void GuildUI_OnUnInit()
		{
			this.UnRegAllUIList();
		}

		private void UnRegAllUIList()
		{
			for (int i = 0; i < this.ItemUIList.Count; i++)
			{
				this.ItemUIList[i].DeInit();
			}
			this.ItemUIList.Clear();
		}

		protected override void GuildUI_OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (base.gameObject == null || !base.gameObject.activeSelf)
			{
				return;
			}
			if (this.BattleCtrl != null && this.mBattleProcess != this.BattleCtrl.BattleProcess)
			{
				this.mBattleProcess = this.BattleCtrl.BattleProcess;
			}
			if (this.mBattleProcess != null)
			{
				if (this.mBattleProcess.CurBattle != this.mCurrentBattle)
				{
					this.mCurrentBattle = this.mBattleProcess.CurBattle;
					this.RefreshCurrentShowList();
					if (this.CurrentBattleUI != null)
					{
						this.CurrentBattleUI.RefreshOnBattleChange(this.mBattleProcess);
					}
				}
				if (this.CurrentBattleUI != null)
				{
					this.CurrentBattleUI.RefreshCurBattle(this.mBattleProcess);
				}
			}
			if (this.mScrollContentToPosY > 0f)
			{
				Vector2 anchoredPosition = this.ScrollContent.anchoredPosition;
				float num = Mathf.Abs(anchoredPosition.y - this.mScrollContentToPosY);
				if (num < 1f)
				{
					anchoredPosition.y = this.mScrollContentToPosY;
					this.mScrollContentToPosY = -1f;
				}
				else
				{
					float num2 = 15f / num;
					num2 = Mathf.Clamp(num2, 0.05f, 1f);
					anchoredPosition.y = Mathf.Lerp(anchoredPosition.y, this.mScrollContentToPosY, num2);
				}
				this.ScrollContent.anchoredPosition = anchoredPosition;
				if (this.ScrollView.verticalNormalizedPosition <= 0f)
				{
					this.mScrollContentToPosY = -1f;
				}
			}
		}

		public override void RefreshUI()
		{
			if (this.BattleCtrl != null && this.BattleCtrl.IsCanGetCurDayBattleRecord())
			{
				this.mBattleProcess = this.BattleCtrl.BattleProcess;
				if (this.mBattleProcess != null)
				{
					this.mCurrentBattle = this.mBattleProcess.CurBattle;
				}
				this.RefreshCurrentShowList();
				if (this.CurrentBattleUI != null)
				{
					this.CurrentBattleUI.RefreshOnBattleChange(this.mBattleProcess);
				}
			}
			else
			{
				this.mBattleProcess = null;
			}
			this.Text_Guild1.text = this.BattleCtrl.GuildName1;
			this.Text_Guild2.text = this.BattleCtrl.GuildName2;
		}

		private void RefreshCurrentShowList()
		{
			this.mItemPool.Collect("GuildRaceItem_Battle0");
			this.mItemPool.Collect("GuildRaceItem_Battle1");
			this.mItemPool.Collect("GuildRaceItem_Battle2");
			if (this.mBattleProcess == null)
			{
				return;
			}
			float num = (float)(this.LayoutGroup.padding.top + this.LayoutGroup.padding.bottom);
			float num2 = 0f;
			float[] array = new float[] { 0f, 0f, 144f, 325f, 325f };
			this.CurrentBattleUI = null;
			List<GuildRaceBattleProcess.UserPK> pklist = this.mBattleProcess.PKList;
			int curUserPKIndex = this.mBattleProcess.CurUserPKIndex;
			for (int i = 0; i < pklist.Count; i++)
			{
				GuildRaceBattleProcess.UserPK userPK = pklist[i];
				if (userPK != null)
				{
					UIGuildRacePageBattleItem uiguildRacePageBattleItem;
					if (curUserPKIndex > i || this.mBattleProcess.AllBattleOver)
					{
						GameObject gameObject = this.mItemPool.DeQueue("GuildRaceItem_Battle0");
						gameObject.transform.SetParentNormal(this.ScrollContent, false);
						uiguildRacePageBattleItem = this.GetUI(gameObject.GetInstanceID());
						if (uiguildRacePageBattleItem == null)
						{
							uiguildRacePageBattleItem = gameObject.GetComponent<UIGuildRacePageBattleItem_Completed>();
							this.AddUI(uiguildRacePageBattleItem, gameObject);
						}
					}
					else if (curUserPKIndex == i)
					{
						GameObject gameObject = this.mItemPool.DeQueue("GuildRaceItem_Battle1");
						gameObject.transform.SetParentNormal(this.ScrollContent, false);
						uiguildRacePageBattleItem = this.GetUI(gameObject.GetInstanceID());
						if (uiguildRacePageBattleItem == null)
						{
							uiguildRacePageBattleItem = gameObject.GetComponent<UIGuildRacePageBattleItem_Doing>();
							this.AddUI(uiguildRacePageBattleItem, gameObject);
						}
						this.CurrentBattleUI = uiguildRacePageBattleItem as UIGuildRacePageBattleItem_Doing;
						if (i < array.Length)
						{
							num2 = array[i];
						}
						else
						{
							num2 = array[array.Length - 1];
						}
					}
					else
					{
						GameObject gameObject = this.mItemPool.DeQueue("GuildRaceItem_Battle2");
						gameObject.transform.SetParentNormal(this.ScrollContent, false);
						uiguildRacePageBattleItem = this.GetUI(gameObject.GetInstanceID());
						if (uiguildRacePageBattleItem == null)
						{
							uiguildRacePageBattleItem = gameObject.GetComponent<UIGuildRacePageBattleItem_Wait>();
							this.AddUI(uiguildRacePageBattleItem, gameObject);
						}
					}
					uiguildRacePageBattleItem.SetData(userPK.Record);
					uiguildRacePageBattleItem.RefreshUI();
					if (this.BattleCtrl != null)
					{
						uiguildRacePageBattleItem.SetGuildName(this.BattleCtrl.GuildName1, this.BattleCtrl.GuildName2);
					}
					num += uiguildRacePageBattleItem.Size_Y();
					num += this.LayoutGroup.spacing;
				}
			}
			Vector2 sizeDelta = this.ScrollContent.sizeDelta;
			sizeDelta.y = num;
			this.ScrollContent.sizeDelta = sizeDelta;
			this.mScrollContentToPosY = num2;
		}

		private UIGuildRacePageBattleItem GetUI(int instanceid)
		{
			UIGuildRacePageBattleItem uiguildRacePageBattleItem;
			if (this.mUIDic.TryGetValue(instanceid, out uiguildRacePageBattleItem))
			{
				return uiguildRacePageBattleItem;
			}
			return null;
		}

		private void AddUI(UIGuildRacePageBattleItem item, GameObject itemobj)
		{
			if (item == null || itemobj == null)
			{
				return;
			}
			item.Init();
			this.mUIDic[itemobj.GetInstanceID()] = item;
			this.ItemUIList.Add(item);
		}

		private const string Prefab0 = "GuildRaceItem_Battle0";

		private const string Prefab1 = "GuildRaceItem_Battle1";

		private const string Prefab2 = "GuildRaceItem_Battle2";

		public UIGuildRacePageBattleItem_Completed GuildRaceItem_Battle0;

		public UIGuildRacePageBattleItem_Doing GuildRaceItem_Battle1;

		public UIGuildRacePageBattleItem_Wait GuildRaceItem_Battle2;

		public CustomText Text_Guild1;

		public CustomText Text_Guild2;

		public ScrollRect ScrollView;

		public RectTransform ScrollContent;

		public VerticalLayoutGroup LayoutGroup;

		private GuildRaceBattleProcess mBattleProcess;

		private GuildRaceBattleProcess.UserBattle mCurrentBattle;

		private Dictionary<int, UIGuildRacePageBattleItem> mUIDic = new Dictionary<int, UIGuildRacePageBattleItem>();

		private List<UIGuildRacePageBattleItem> ItemUIList = new List<UIGuildRacePageBattleItem>();

		[HideInInspector]
		public UIGuildRacePageBattleItem_Doing CurrentBattleUI;

		private LocalUnityObjctPool mItemPool;

		private float mScrollContentToPosY;
	}
}
