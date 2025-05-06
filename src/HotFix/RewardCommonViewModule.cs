using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.Component;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using Google.Protobuf.Collections;
using Proto.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class RewardCommonViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.ButtonTapToClose.OnClose = new Action(this.OnClickCloseRewardCommonView);
			Vector3 localScale = this.m_item.transform.localScale;
			this.ItemWidth = localScale.x * ((RectTransform)this.m_item.transform).sizeDelta.x + this.itemSpacingX;
			this.ItemHeight = localScale.y * ((RectTransform)this.m_item.transform).sizeDelta.y + this.itemSpacingY;
			this.m_pool = LocalUnityObjctPool.Create(base.gameObject);
			this.m_pool.CreateCache(this.PoolCacheName, this.m_item);
			this.m_item.SetActive(false);
			this.m_Prefab_Item.SetActive(false);
			this.m_Prefab_Hero.SetActive(false);
			this.m_Prefab_Equip.SetActive(false);
			if (this.m_firstEnterListen != null)
			{
				this.m_firstEnterListen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListenForModelAnimatorListen));
			}
		}

		public override void OnOpen(object data)
		{
			GameApp.Sound.PlayClip(50, 1f);
			this.m_scrollContent.anchoredPosition = Vector2.zero;
			this.canClose = false;
			DelayCall.Instance.CallOnce(200, delegate
			{
				this.canClose = true;
			});
			if (data == null)
			{
				HLog.LogError("RewardCommonViewModule data == null");
				return;
			}
			this.m_data = (RewardCommonData)data;
			if (this.m_data == null)
			{
				HLog.LogError("RewardCommonViewModule m_data == null");
				return;
			}
			if (this.m_defaultTitle != null)
			{
				string text = this.m_data.m_title;
				if (!this.m_data.m_hideTitle && string.IsNullOrEmpty(text))
				{
					text = Singleton<LanguageManager>.Instance.GetInfoByID("Common_Reward");
				}
				this.m_defaultTitle.gameObject.SetActive(!this.m_data.m_hideTitle);
				this.m_defaultTitle.SetText(text);
			}
			this.m_isFly = this.m_data.m_isFly;
			this.m_firstEnterAnimator.Play("RewardCommonShow");
			this.ShowReward();
			if (this.coroutineAutoCloseTrigger != null)
			{
				base.StopCoroutine(this.coroutineAutoCloseTrigger);
			}
			this.coroutineAutoCloseTrigger = base.StartCoroutine(this.AutoCloseTrigger());
		}

		private IEnumerator AutoCloseTrigger()
		{
			if (this.m_data.m_isInSweep)
			{
				yield return new WaitForSeconds(this.m_data.m_autoCloseTime);
				this.OnClickCloseRewardCommonView();
			}
			yield break;
		}

		public void ShowReward()
		{
			List<RewardCommonViewModule.RewardItem> list = new List<RewardCommonViewModule.RewardItem>();
			List<ItemData> list2 = this.m_data.list;
			if (list2 != null && list2.Count > 0)
			{
				for (int i = 0; i < list2.Count; i++)
				{
					list.Add(new RewardCommonViewModule.RewardItem(list2[i]));
				}
			}
			List<EquipData> epuipList = this.m_data.epuipList;
			if (epuipList != null && epuipList.Count > 0)
			{
				for (int j = 0; j < epuipList.Count; j++)
				{
					list.Add(new RewardCommonViewModule.RewardItem(epuipList[j]));
				}
			}
			this.m_rewarditemList = new List<RewardCommonViewModule.RewardItem>();
			this.m_rewarditemList.AddRange(list);
			int count = list.Count;
			Vector2 vector;
			int num;
			float num2;
			this.AutoCalcViewSize(count, out vector, out num, out num2);
			this.m_itemList = this.m_data.list;
			Vector3 localScale = this.m_item.transform.localScale;
			for (int k = 0; k < count; k++)
			{
				int num3 = k;
				GameObject gameObject = this.m_pool.DeQueue(this.PoolCacheName);
				RewardCommonViewModule.UIRewardItemCtrl uirewardItemCtrl = new RewardCommonViewModule.UIRewardItemCtrl();
				uirewardItemCtrl.SetGameObject(gameObject);
				uirewardItemCtrl.SetPrefab(this.m_Prefab_Item, this.m_Prefab_Equip);
				uirewardItemCtrl.SetParent(this.m_scrollRect.content);
				gameObject.transform.localScale = localScale;
				float num4 = (float)(num3 / 5) * -this.ItemHeight + vector.y;
				float num5 = (float)(num3 % 5) * this.ItemWidth;
				uirewardItemCtrl.SetPos(new Vector2(vector.x + num5, num4));
				uirewardItemCtrl.SetShowData(this.m_rewarditemList[num3]);
				this.m_uilist.Add(uirewardItemCtrl);
			}
			this.m_seqPool.Clear(false);
			for (int l = 0; l < this.m_uilist.Count; l++)
			{
				RewardCommonViewModule.UIRewardItemCtrl uirewardItemCtrl2 = this.m_uilist[l];
				if (!(uirewardItemCtrl2.RTF == null))
				{
					DxxTools.UI.DoScaleAnim(this.m_seqPool.Get(), uirewardItemCtrl2.RTF, 0f, localScale.x, 0.02f * (float)l, 0.2f, 0);
				}
			}
		}

		private void AutoCalcViewSize(int count, out Vector2 start, out int line, out float bgHeight)
		{
			float x = this.m_scrollRect.content.sizeDelta.x;
			RectTransform rectTransform = this.m_scrollImage.rectTransform;
			float y = rectTransform.offsetMin.y;
			float num = -rectTransform.offsetMax.y;
			line = Utility.Math.GetLine(count, 5);
			float num2;
			if (count > 10)
			{
				num2 = this.itemPaddingTop + this.ItemHeight * (float)line + this.itemPaddingBottom;
				bgHeight = this.itemPaddingTop + this.ItemHeight * 2.5f + y + num;
			}
			else if (count > 5)
			{
				num2 = this.itemPaddingTop + this.ItemHeight * 2f + this.itemPaddingBottom;
				bgHeight = num2 + y + num;
			}
			else
			{
				num2 = this.itemPaddingTop + this.ItemHeight * 1f + this.itemPaddingBottom;
				bgHeight = num2 + y + num;
			}
			float num3;
			if (count < 5)
			{
				num3 = -((float)(count - 1) / 2f * this.ItemWidth);
			}
			else
			{
				num3 = -2f * this.ItemWidth;
			}
			float num4 = (float)(line - 1) * this.ItemHeight / 2f;
			this.m_imgBgRt.sizeDelta = new Vector2(this.m_imgBgRt.sizeDelta.x, bgHeight);
			this.m_scrollRect.content.sizeDelta = new Vector2(x, num2);
			bool flag = line > 2;
			this.m_scrollRect.enabled = flag;
			this.m_scrollImage.raycastTarget = flag;
			if (flag)
			{
				line = 2;
			}
			start = new Vector2(num3, num4);
		}

		private void OnClickCloseRewardCommonView()
		{
			if (!this.canClose)
			{
				return;
			}
			if (this.m_isFly)
			{
				GameApp.View.FlyItemDatas(FlyItemModel.Default, this.m_itemList, null, null);
			}
			GameApp.View.CloseView(ViewName.RewardCommonViewModule, null);
		}

		public override void OnClose()
		{
			if (this.m_data != null && this.m_data.OnClose != null)
			{
				this.m_data.OnClose();
			}
			this.m_seqPool.Clear(false);
			for (int i = 0; i < this.m_uilist.Count; i++)
			{
				this.m_uilist[i].OnDelThis();
			}
			this.m_uilist.Clear();
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_Close_Common_Reward, null);
			this.m_pool.Collect(this.PoolCacheName);
		}

		public override void OnDelete()
		{
			if (this.m_firstEnterListen != null)
			{
				this.m_firstEnterListen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListenForModelAnimatorListen));
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void DeinitItems()
		{
		}

		private void OnAnimatorListenForModelAnimatorListen(GameObject obj, string listenName)
		{
		}

		[GameTestMethod("弹窗", "测试获取到装备", "", 0)]
		private static void OnTestEquipShow()
		{
			new List<EquipData>();
			EquipDataModule dataModule = GameApp.Data.GetDataModule(DataName.EquipDataModule);
			List<EquipData> list = new List<EquipData>();
			list.AddRange(dataModule.GetEquipDatas(false, false));
			DxxTools.UI.OpenRewardCommonEquips(list, null, Singleton<LanguageManager>.Instance.GetInfoByID("1478"));
		}

		[GameTestMethod("界面", "打开奖励界面", "", 0)]
		private static void OnTest()
		{
			RepeatedField<RewardDto> repeatedField = new RepeatedField<RewardDto>();
			repeatedField.Add(new RewardDto
			{
				ConfigId = 1U,
				Count = 150UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 2U,
				Count = 88UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 201U,
				Count = 200UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 110103U,
				Count = 1UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 110405U,
				Count = 1UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 120210U,
				Count = 1UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 330201U,
				Count = 1UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 330301U,
				Count = 1UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 330601U,
				Count = 1UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 340402U,
				Count = 10UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 1U,
				Count = 150UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 2U,
				Count = 88UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 201U,
				Count = 200UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 110103U,
				Count = 1UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 110405U,
				Count = 1UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 120210U,
				Count = 1UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 330201U,
				Count = 1UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 330301U,
				Count = 1UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 330601U,
				Count = 1UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 340402U,
				Count = 10UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 351002U,
				Count = 1UL,
				Type = 0U
			});
			repeatedField.Add(new RewardDto
			{
				ConfigId = 362002U,
				Count = 15UL,
				Type = 0U
			});
			DxxTools.UI.OpenRewardCommon(repeatedField, null, true);
		}

		private const int LineCount = 5;

		private const int MaxLine = 2;

		private float ItemWidth;

		private float ItemHeight;

		[SerializeField]
		private float itemPaddingTop;

		[SerializeField]
		private float itemPaddingBottom;

		[SerializeField]
		private float itemSpacingX = 18f;

		[SerializeField]
		private float itemSpacingY = 18f;

		private const int MaxScrollCount = 10;

		[Header("标题")]
		public UIBgText m_defaultTitle;

		[Header("奖励物品")]
		public GameObject m_item;

		public GameObject m_Prefab_Item;

		public GameObject m_Prefab_Hero;

		public GameObject m_Prefab_Equip;

		[Header("内容区域")]
		public RectTransform m_imgBgRt;

		public RectTransform m_infoNode;

		public RectTransform m_scrollContent;

		public CustomScrollRect m_scrollRect;

		public Image m_scrollImage;

		[Header("其他")]
		public TapToCloseCtrl ButtonTapToClose;

		public AnimatorListen m_firstEnterListen;

		public Animator m_firstEnterAnimator;

		private RewardCommonData m_data;

		[Label]
		public string PoolCacheName = "reward";

		private LocalUnityObjctPool m_pool;

		private List<RewardCommonViewModule.UIRewardItemCtrl> m_uilist = new List<RewardCommonViewModule.UIRewardItemCtrl>();

		private List<RewardCommonViewModule.RewardItem> m_rewarditemList;

		private SequencePool m_seqPool = new SequencePool();

		private List<ItemData> m_itemList;

		[HideInInspector]
		public bool m_isFly = true;

		private bool canClose;

		private Coroutine coroutineAutoCloseTrigger;

		public class RewardItem
		{
			public RewardItem(ItemData item)
			{
				this.Item = item;
			}

			public RewardItem(EquipData equip)
			{
				this.Equip = equip;
			}

			public ItemData Item;

			public EquipData Equip;
		}

		public class UIRewardItemCtrl
		{
			public RectTransform RTF
			{
				get
				{
					return this.m_RTF;
				}
			}

			public void SetGameObject(GameObject obj)
			{
				this.m_GObj = obj;
				this.m_RTF = this.m_GObj.transform as RectTransform;
			}

			public void SetPrefab(GameObject prefabitem, GameObject prefabequip)
			{
				this.Prefab_Item = prefabitem;
				this.Prefab_Equip = prefabequip;
			}

			public void SetParent(RectTransform rtf)
			{
				this.m_RTF.SetParentNormal(rtf, false);
			}

			public void SetPos(Vector2 pos)
			{
				this.m_RTF.anchoredPosition = pos;
			}

			public void OnDelThis()
			{
				if (this.m_UIItemCtrl != null)
				{
					this.m_UIItemCtrl.DeInit();
					if (this.m_UIItemCtrl.gameObject != null)
					{
						Object.Destroy(this.m_UIItemCtrl.gameObject);
					}
					this.m_UIItemCtrl = null;
				}
				if (this.m_UIEquipCtrl != null)
				{
					this.m_UIEquipCtrl.DeInit();
					if (this.m_UIEquipCtrl.gameObject != null)
					{
						Object.Destroy(this.m_UIEquipCtrl.gameObject);
					}
					this.m_UIEquipCtrl = null;
				}
			}

			public void SetShowData(RewardCommonViewModule.RewardItem ritem)
			{
				if (ritem.Item != null)
				{
					this.CreateItem();
					this.ActiveItem(this.m_UIItemCtrl);
					if (this.m_UIItemCtrl != null)
					{
						this.m_UIItemCtrl.SetData(ritem.Item.ToPropData());
						this.m_UIItemCtrl.OnRefresh();
						return;
					}
				}
				else if (ritem.Equip != null)
				{
					this.CreateEquip();
					this.ActiveItem(this.m_UIEquipCtrl);
					if (this.m_UIEquipCtrl != null)
					{
						this.m_UIEquipCtrl.RefreshData(ritem.Equip);
						return;
					}
				}
				else
				{
					this.ActiveItem(null);
				}
			}

			private void ActiveItem(CustomBehaviour obj)
			{
				if (this.m_UIItemCtrl != null)
				{
					this.m_UIItemCtrl.SetActive(this.m_UIItemCtrl == obj);
				}
				if (this.m_UIEquipCtrl != null)
				{
					this.m_UIEquipCtrl.SetActive(this.m_UIEquipCtrl == obj);
				}
			}

			public void CreateItem()
			{
				if (this.m_UIItemCtrl != null || this.Prefab_Item == null)
				{
					return;
				}
				GameObject gameObject = Object.Instantiate<GameObject>(this.Prefab_Item, this.m_RTF);
				if (gameObject != null)
				{
					((RectTransform)gameObject.transform).anchoredPosition = Vector3.zero;
					this.m_UIItemCtrl = gameObject.GetComponent<UIItem>();
					this.m_UIItemCtrl.Init();
					this.m_UIItemCtrl.onClick = new Action<UIItem, PropData, object>(this.OnBtnItemClick);
					this.m_UIItemCtrl.SetActive(true);
				}
			}

			private void OnBtnItemClick(UIItem item, PropData data, object openData)
			{
				float num = this.m_RTF.rect.height * this.m_RTF.localScale.y * 0.5f + 10f;
				DxxTools.UI.OnItemClick(item, data, openData, item.transform.position, num);
			}

			public void CreateEquip()
			{
				if (this.m_UIEquipCtrl != null || this.Prefab_Equip == null)
				{
					return;
				}
				GameObject gameObject = Object.Instantiate<GameObject>(this.Prefab_Equip, this.m_RTF);
				if (gameObject != null)
				{
					((RectTransform)gameObject.transform).anchoredPosition = Vector3.zero;
					this.m_UIEquipCtrl = gameObject.GetComponent<UIHeroEquipItem>();
					this.m_UIEquipCtrl.Init();
					this.m_UIEquipCtrl.SetUpLevelActive(false);
					this.m_UIEquipCtrl.SetRedNodeActive(false);
					this.m_UIEquipCtrl.SetLockActive(false);
					this.m_UIEquipCtrl.SetActive(true);
				}
			}

			private GameObject m_GObj;

			private RectTransform m_RTF;

			private UIItem m_UIItemCtrl;

			private UIHeroEquipItem m_UIEquipCtrl;

			public GameObject Prefab_Item;

			public GameObject Prefab_Equip;
		}
	}
}
