using System;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIMainButton_Gift : BaseUIMainButton
	{
		private string ServerClockKey
		{
			get
			{
				return string.Format("iap_gift_mainbutton_{0}", this.id);
			}
		}

		public void SetData(int idVal)
		{
			this.id = idVal;
		}

		public override bool IsShow()
		{
			return Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.FirstTopUp, false);
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 4;
			IAP_PushPacks iap_PushPacks = this.pushPackTable;
			subPriority = ((iap_PushPacks != null) ? iap_PushPacks.priority : 0);
		}

		protected override void OnInit()
		{
			this.pushPackTable = GameApp.Table.GetManager().GetIAP_PushPacksModelInstance().GetElementById(this.id);
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.button.onClick.AddListener(new UnityAction(this.OnClickButton));
			string atlasPath = GameApp.Table.GetAtlasPath(this.pushPackTable.iconAtlasID);
			this.iconImage.SetImage(atlasPath, this.pushPackTable.iconName);
			this.RefreshNameTxt();
			this.InitRefreshTime();
			this.effectObj.SetActive(this.pushPackTable.isEffect != 0);
			this.redNodePath = this.GetRedNodePath();
			RedPointController.Instance.RegRecordChange(this.redNodePath, new Action<RedNodeListenData>(this.OnRedPointChange));
			if (this.pushPackTable.packType == 7)
			{
				this.redNode.gameObject.SetActiveSafe(this.iapDataModule.MeetingGift.IsRedPoint());
			}
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			long endTimeTick = this.GetEndTimeTick();
			if (endTimeTick > 0L && endTimeTick > serverTimestamp)
			{
				DxxTools.UI.RemoveServerTimeClockCallback(this.ServerClockKey);
				DxxTools.UI.AddServerTimeCallback(this.ServerClockKey, new Action(this.OnServerClockTimeOut), endTimeTick, 0);
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.iapShopTimeCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnDeInit()
		{
			RedPointController.Instance.UnRegRecordChange(this.redNodePath, new Action<RedNodeListenData>(this.OnRedPointChange));
			this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
			this.DeInitRefreshTime();
			DxxTools.UI.RemoveServerTimeClockCallback(this.ServerClockKey);
		}

		public override void OnRefresh()
		{
			this.OnRefreshAnimation();
		}

		public override void OnLanguageChange()
		{
			this.RefreshNameTxt();
		}

		private void OnClickButton()
		{
			if (this.pushPackTable == null)
			{
				return;
			}
			switch (this.pushPackTable.packType)
			{
			case 1:
				GameApp.View.OpenView(ViewName.GiftFirstViewModule, null, 1, null, null);
				return;
			case 2:
				GameApp.View.OpenView(ViewName.GiftOpenServerViewModule, null, 1, null, null);
				return;
			case 3:
				GameApp.View.OpenView(ViewName.GiftLimitedTimeViewModule, null, 1, null, null);
				return;
			case 4:
			{
				IAPChapterGift.Data data = GameApp.Data.GetDataModule(DataName.IAPDataModule).ChapterGift.GetData(this.id);
				if (data != null)
				{
					GiftChapterViewModule.OpenData openData = new GiftChapterViewModule.OpenData
					{
						m_giftData = data
					};
					GameApp.View.OpenView(ViewName.GiftChapterViewModule, openData, 1, null, null);
					return;
				}
				break;
			}
			case 5:
			case 6:
				break;
			case 7:
				GameApp.View.OpenView(ViewName.MeetingGiftViewModule, null, 1, null, null);
				break;
			default:
				return;
			}
		}

		private void RefreshNameTxt()
		{
			this.nameText.text = Singleton<LanguageManager>.Instance.GetInfoByID("uigiftrecharge_title");
		}

		private void OnRedPointChange(RedNodeListenData data)
		{
			string text = ((this.redNode.count > 0) ? "Shake" : "Idle");
			this.animator.SetTrigger(text);
			this.redNode.gameObject.SetActiveSafe(data.m_count > 0);
		}

		public override void OnRefreshAnimation()
		{
		}

		private void InitRefreshTime()
		{
			this.iapShopTimeCtrl.OnRefreshText += this.OnRefreshTimeText;
			this.iapShopTimeCtrl.Init();
			long num;
			string text;
			if (this.GetRefreshTimeInfo(out num, out text))
			{
				this.iapShopTimeCtrl.Play();
			}
		}

		private void DeInitRefreshTime()
		{
			this.iapShopTimeCtrl.DeInit();
		}

		private string OnRefreshTimeText(IAPShopTimeCtrl arg)
		{
			long num;
			string text;
			if (this.GetRefreshTimeInfo(out num, out text))
			{
				return text;
			}
			return string.Empty;
		}

		private IAPShopTimeCtrl.State OnChangeStateNextTime(IAPShopTimeCtrl arg)
		{
			if (this.pushPackTable.packType != 7)
			{
				return IAPShopTimeCtrl.State.Load;
			}
			if (this.iapDataModule.MeetingGift.GetUnBuyGift() == null)
			{
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshLimitTimeLeftButton, null);
				return IAPShopTimeCtrl.State.Load;
			}
			if (this.iapDataModule.MeetingGift.EndTime - DxxTools.Time.ServerTimestamp <= 0L)
			{
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshLimitTimeLeftButton, null);
				return IAPShopTimeCtrl.State.Load;
			}
			return IAPShopTimeCtrl.State.Show;
		}

		private bool GetRefreshTimeInfo(out long timeValue, out string timeString)
		{
			timeValue = 0L;
			timeString = string.Empty;
			switch (this.pushPackTable.packType)
			{
			default:
				return false;
			case 2:
				timeValue = this.iapDataModule.OpenServerGift.GetLastTime();
				break;
			case 3:
				timeValue = this.iapDataModule.LimitedTimeGift.GetLastTime();
				break;
			case 4:
			{
				IAPChapterGift.Data data = this.iapDataModule.ChapterGift.GetData(this.id);
				if (data == null)
				{
					return false;
				}
				timeValue = data.GetLastTime();
				break;
			}
			case 7:
			{
				long lastTime = this.iapDataModule.MeetingGift.GetLastTime();
				if (lastTime < 0L)
				{
					return false;
				}
				timeValue = lastTime;
				break;
			}
			}
			timeString = Singleton<LanguageManager>.Instance.GetTime((timeValue < 0L) ? 0L : timeValue);
			return true;
		}

		private long GetEndTimeTick()
		{
			long num = 0L;
			switch (this.pushPackTable.packType)
			{
			case 2:
				if (!this.iapDataModule.OpenServerGift.TryFinishedTime(out num))
				{
					num = 0L;
				}
				break;
			case 3:
			{
				long num2;
				if (!this.iapDataModule.LimitedTimeGift.TryTime(out num2, out num))
				{
					num = 0L;
				}
				break;
			}
			case 4:
			{
				IAPChapterGift.Data data = this.iapDataModule.ChapterGift.GetData(this.id);
				if (data != null && !data.TryFinishedTime(out num))
				{
					num = 0L;
				}
				break;
			}
			}
			return num;
		}

		private string GetRedNodePath()
		{
			string text = string.Empty;
			if (this.pushPackTable == null)
			{
				return text;
			}
			switch (this.pushPackTable.packType)
			{
			case 1:
				text = "IAPGift.First";
				break;
			case 2:
				text = "IAPGift.OpenServer";
				break;
			case 3:
				text = "IAPGift.Limited";
				break;
			case 4:
				text = RedPointController.Instance.GetChapterGiftPath(this.id);
				break;
			case 7:
				text = "IAPGift.Meeting";
				break;
			}
			return text;
		}

		private void OnServerClockTimeOut()
		{
			ViewName viewName = (ViewName)0;
			switch (this.pushPackTable.packType)
			{
			case 1:
				viewName = ViewName.GiftFirstViewModule;
				break;
			case 2:
				viewName = ViewName.GiftOpenServerViewModule;
				break;
			case 3:
				viewName = ViewName.GiftLimitedTimeViewModule;
				break;
			case 4:
				viewName = ViewName.GiftChapterViewModule;
				break;
			}
			if (GameApp.View.IsOpened(viewName))
			{
				GameApp.View.CloseView(viewName, null);
			}
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshLimitTimeLeftButton, null);
		}

		[SerializeField]
		private CustomButton button;

		[SerializeField]
		private CustomImage iconImage;

		[SerializeField]
		private GameObject effectObj;

		[SerializeField]
		private CustomText nameText;

		[SerializeField]
		private RedNodeOneCtrl redNode;

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private IAPShopTimeCtrl iapShopTimeCtrl;

		private string redNodePath;

		private int id;

		private IAP_PushPacks pushPackTable;

		private IAPDataModule iapDataModule;
	}
}
