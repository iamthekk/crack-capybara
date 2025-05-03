using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class IAPFundViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.btnList.Add(this.btnBattlePass);
			this.btnList.Add(this.btnTalent);
			this.btnList.Add(this.btnTower);
			this.btnList.Add(this.btnDungeon);
			this.buttonBack.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.btnBattlePass.onClick.AddListener(new UnityAction(this.OnClickBattlePass));
			this.btnTalent.onClick.AddListener(new UnityAction(this.OnClickTalent));
			this.btnTower.onClick.AddListener(new UnityAction(this.OnClickTower));
			this.btnDungeon.onClick.AddListener(new UnityAction(this.OnClickDungeon));
			RedPointController.Instance.RegRecordChange("IAPRechargeGift.Fund.BattlePass", new Action<RedNodeListenData>(this.OnRedPointBattlePass));
			RedPointController.Instance.RegRecordChange("IAPRechargeGift.Fund.TalentFund", new Action<RedNodeListenData>(this.OnRedPointTalent));
			RedPointController.Instance.RegRecordChange("IAPRechargeGift.Fund.TowerFund", new Action<RedNodeListenData>(this.OnRedPointTower));
			RedPointController.Instance.RegRecordChange("IAPRechargeGift.Fund.RogueDungeonFund", new Action<RedNodeListenData>(this.OnRedPointDungeon));
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			this.CheckShowTag();
			IAPFundViewModule.FundType fundType = ((data is IAPFundViewModule.FundType) ? ((IAPFundViewModule.FundType)data) : IAPFundViewModule.FundType.BattlePass);
			this._curFundType = fundType;
			GameTGAExtend.OnViewOpen("IAPFundViewModule" + this._curFundType.ToString());
			switch (fundType)
			{
			case IAPFundViewModule.FundType.BattlePass:
				this.OnClickBattlePass();
				return;
			case IAPFundViewModule.FundType.Talent:
				this.OnClickTalent();
				return;
			case IAPFundViewModule.FundType.Tower:
				this.OnClickTower();
				return;
			case IAPFundViewModule.FundType.RogueDungeon:
				this.OnClickDungeon();
				return;
			default:
				return;
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			foreach (IAPFundCtrlBase iapfundCtrlBase in this.fundDic.Values)
			{
				iapfundCtrlBase.OnHide();
			}
			GameTGAExtend.OnViewClose("IAPFundViewModule" + this._curFundType.ToString());
		}

		public override void OnDelete()
		{
			this.btnList.Clear();
			this.buttonBack.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.btnBattlePass.onClick.RemoveListener(new UnityAction(this.OnClickBattlePass));
			this.btnTalent.onClick.RemoveListener(new UnityAction(this.OnClickTalent));
			this.btnTower.onClick.RemoveListener(new UnityAction(this.OnClickTower));
			this.btnDungeon.onClick.RemoveListener(new UnityAction(this.OnClickDungeon));
			foreach (IAPFundCtrlBase iapfundCtrlBase in this.fundDic.Values)
			{
				iapfundCtrlBase.DeInit();
			}
			RedPointController.Instance.UnRegRecordChange("IAPRechargeGift.Fund.BattlePass", new Action<RedNodeListenData>(this.OnRedPointBattlePass));
			RedPointController.Instance.UnRegRecordChange("IAPRechargeGift.Fund.TalentFund", new Action<RedNodeListenData>(this.OnRedPointTalent));
			RedPointController.Instance.UnRegRecordChange("IAPRechargeGift.Fund.TowerFund", new Action<RedNodeListenData>(this.OnRedPointTower));
			RedPointController.Instance.UnRegRecordChange("IAPRechargeGift.Fund.RogueDungeonFund", new Action<RedNodeListenData>(this.OnRedPointDungeon));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void Refresh(IAPFundViewModule.FundType type)
		{
			IAPFundCtrlBase iapfundCtrlBase;
			if (this.fundDic.TryGetValue(type, out iapfundCtrlBase))
			{
				if (iapfundCtrlBase)
				{
					iapfundCtrlBase.OnShow();
				}
			}
			else
			{
				string text = "";
				switch (type)
				{
				case IAPFundViewModule.FundType.BattlePass:
					text = "Assets/_Resources/Prefab/UI/IAPFund/IAPFundBattlePassNode.prefab";
					break;
				case IAPFundViewModule.FundType.Talent:
					text = "Assets/_Resources/Prefab/UI/IAPFund/IAPFundTalentNode.prefab";
					break;
				case IAPFundViewModule.FundType.Tower:
					text = "Assets/_Resources/Prefab/UI/IAPFund/IAPFundTowerNode.prefab";
					break;
				case IAPFundViewModule.FundType.RogueDungeon:
					text = "Assets/_Resources/Prefab/UI/IAPFund/IAPFundRogueDungeonNode.prefab";
					break;
				}
				this.LoadNode(type, text);
			}
			if (this._curFundType != type)
			{
				GameTGAExtend.OnViewClose("IAPFundViewModule" + this._curFundType.ToString());
				this._curFundType = type;
				GameTGAExtend.OnViewOpen("IAPFundViewModule" + this._curFundType.ToString());
			}
		}

		private Task LoadNode(IAPFundViewModule.FundType type, string path)
		{
			IAPFundViewModule.<LoadNode>d__26 <LoadNode>d__;
			<LoadNode>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadNode>d__.<>4__this = this;
			<LoadNode>d__.type = type;
			<LoadNode>d__.path = path;
			<LoadNode>d__.<>1__state = -1;
			<LoadNode>d__.<>t__builder.Start<IAPFundViewModule.<LoadNode>d__26>(ref <LoadNode>d__);
			return <LoadNode>d__.<>t__builder.Task;
		}

		private void SetButtonSelect(CustomChooseButton btn)
		{
			for (int i = 0; i < this.btnList.Count; i++)
			{
				if (this.btnList[i].Equals(btn))
				{
					this.btnList[i].SetSelect(true);
				}
				else
				{
					this.btnList[i].SetSelect(false);
				}
			}
			foreach (IAPFundCtrlBase iapfundCtrlBase in this.fundDic.Values)
			{
				iapfundCtrlBase.OnHide();
			}
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.IAPFundViewModule, null);
		}

		private void OnClickBattlePass()
		{
			this.SetButtonSelect(this.btnBattlePass);
			this.Refresh(IAPFundViewModule.FundType.BattlePass);
		}

		private void OnClickTalent()
		{
			this.SetButtonSelect(this.btnTalent);
			this.Refresh(IAPFundViewModule.FundType.Talent);
		}

		private void OnClickTower()
		{
			this.SetButtonSelect(this.btnTower);
			this.Refresh(IAPFundViewModule.FundType.Tower);
		}

		private void OnClickDungeon()
		{
			this.SetButtonSelect(this.btnDungeon);
			this.Refresh(IAPFundViewModule.FundType.RogueDungeon);
		}

		private void OnRedPointBattlePass(RedNodeListenData redData)
		{
			if (this.redNodeBattlePass == null)
			{
				return;
			}
			this.redNodeBattlePass.gameObject.SetActive(redData.m_count > 0);
		}

		private void OnRedPointTalent(RedNodeListenData redData)
		{
			if (this.redNodeTalent == null)
			{
				return;
			}
			this.redNodeTalent.gameObject.SetActive(redData.m_count > 0);
		}

		private void OnRedPointTower(RedNodeListenData redData)
		{
			if (this.redNodeTower == null)
			{
				return;
			}
			this.redNodeTower.gameObject.SetActive(redData.m_count > 0);
		}

		private void OnRedPointDungeon(RedNodeListenData redData)
		{
			if (this.redNodeDungeon == null)
			{
				return;
			}
			this.redNodeDungeon.gameObject.SetActive(redData.m_count > 0);
		}

		private void CheckShowTag()
		{
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			IAPBattlePass battlePass = dataModule.BattlePass;
			bool flag = battlePass != null && !battlePass.IsAllEnd();
			this.btnBattlePass.gameObject.SetActiveSafe(flag);
			IAPLevelFundGroup currentFundGroup = dataModule.LevelFund.GetCurrentFundGroup(IAPLevelFundGroupKind.TalentLevel);
			this.btnTalent.gameObject.SetActiveSafe(currentFundGroup != null);
			IAPLevelFundGroup currentFundGroup2 = dataModule.LevelFund.GetCurrentFundGroup(IAPLevelFundGroupKind.TowerLevel);
			this.btnTower.gameObject.SetActiveSafe(currentFundGroup2 != null);
			IAPLevelFundGroup currentFundGroup3 = dataModule.LevelFund.GetCurrentFundGroup(IAPLevelFundGroupKind.RogueDungeonFloor);
			this.btnDungeon.gameObject.SetActiveSafe(currentFundGroup3 != null);
		}

		public GameObject nodeParent;

		public CustomButton buttonBack;

		public CustomChooseButton btnBattlePass;

		public CustomChooseButton btnTalent;

		public CustomChooseButton btnTower;

		public CustomChooseButton btnDungeon;

		public GameObject redNodeBattlePass;

		public GameObject redNodeTalent;

		public GameObject redNodeTower;

		public GameObject redNodeDungeon;

		private List<CustomChooseButton> btnList = new List<CustomChooseButton>();

		private Dictionary<IAPFundViewModule.FundType, IAPFundCtrlBase> fundDic = new Dictionary<IAPFundViewModule.FundType, IAPFundCtrlBase>();

		public const string FUND_BATTLEPASS_PATH = "Assets/_Resources/Prefab/UI/IAPFund/IAPFundBattlePassNode.prefab";

		public const string FUND_TALENT_PATH = "Assets/_Resources/Prefab/UI/IAPFund/IAPFundTalentNode.prefab";

		public const string FUND_TOWER_PATH = "Assets/_Resources/Prefab/UI/IAPFund/IAPFundTowerNode.prefab";

		public const string FUND_DUNGEON_PATH = "Assets/_Resources/Prefab/UI/IAPFund/IAPFundRogueDungeonNode.prefab";

		private IAPFundViewModule.FundType _curFundType;

		public enum FundType
		{
			BattlePass,
			Talent,
			Tower,
			RogueDungeon
		}
	}
}
