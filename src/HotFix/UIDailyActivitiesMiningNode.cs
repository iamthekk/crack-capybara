using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Proto.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIDailyActivitiesMiningNode : UIDailyActivitiesNodeBase
	{
		public override FunctionID FunctionOpenID
		{
			get
			{
				return FunctionID.Mining;
			}
		}

		public override UIDailyActivitiesType DailyType
		{
			get
			{
				return UIDailyActivitiesType.Mining;
			}
		}

		protected override void OnInit()
		{
			this.isInit = true;
			this.mDataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
			this.buttonNode.onClick.AddListener(new UnityAction(this.OnClickNode));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_Mining_CloseUI, new HandlerEvent(this.OnCloseMining));
			RedPointController.Instance.RegRecordChange("DailyActivity.ChallengeTag.Mining", new Action<RedNodeListenData>(base.OnRedPointChange));
		}

		protected override void OnDeInit()
		{
			this.isInit = false;
			this.buttonNode.onClick.RemoveListener(new UnityAction(this.OnClickNode));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Mining_CloseUI, new HandlerEvent(this.OnCloseMining));
			RedPointController.Instance.UnRegRecordChange("DailyActivity.ChallengeTag.Mining", new Action<RedNodeListenData>(base.OnRedPointChange));
		}

		protected override void OnShow()
		{
			if (this.mDataModule.MiningInfo == null)
			{
				return;
			}
			this.textFloor.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimining_num", new object[] { this.mDataModule.MiningInfo.Stage });
			UserTicket ticket = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicket(UserTicketKind.Mining);
			if (ticket != null)
			{
				this.textItemNum.text = string.Format("{0}/{1}", ticket.NewNum, ticket.RevertLimit);
			}
			else
			{
				this.textItemNum.text = "0/0";
			}
			DelayCall.Instance.CallOnce(10, delegate
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.desRT);
			});
		}

		protected override void OnHide()
		{
		}

		private void OnClickNode()
		{
			if (this.mIsLock)
			{
				return;
			}
			GameApp.View.OpenView(ViewName.MiningViewModule, null, 1, null, null);
		}

		private void OnCloseMining(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnShow();
		}

		[SerializeField]
		private CustomText textFloor;

		[SerializeField]
		private CustomText textItemNum;

		[SerializeField]
		private CustomButton buttonNode;

		[SerializeField]
		private RectTransform desRT;

		private MiningDataModule mDataModule;

		private bool isInit;
	}
}
