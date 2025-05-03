using System;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIDailyActivitiesDungeonNode : UIDailyActivitiesNodeBase
	{
		public override FunctionID FunctionOpenID
		{
			get
			{
				switch (this.dungeonID)
				{
				case DungeonID.DragonsLair:
					return FunctionID.Main_Activity_DragonsLair;
				case DungeonID.AstralTree:
					return FunctionID.Main_Activity_AstralTree;
				case DungeonID.SwordIsland:
					return FunctionID.Main_Activity_SwordIsland;
				case DungeonID.DeepSeaRuins:
					return FunctionID.Main_Activity_DeepSeaRuins;
				default:
					return FunctionID.None;
				}
			}
		}

		public override UIDailyActivitiesType DailyType
		{
			get
			{
				switch (this.dungeonID)
				{
				case DungeonID.DragonsLair:
					return UIDailyActivitiesType.DragonLair;
				case DungeonID.AstralTree:
					return UIDailyActivitiesType.AstralTree;
				case DungeonID.SwordIsland:
					return UIDailyActivitiesType.SwordIsland;
				case DungeonID.DeepSeaRuins:
					return UIDailyActivitiesType.DeepSeaRuins;
				default:
					return UIDailyActivitiesType.Wait;
				}
			}
		}

		protected override void OnInit()
		{
			this.nodeButton.onClick.AddListener(new UnityAction(this.OnClickNode));
			this.dungeonDataModule = GameApp.Data.GetDataModule(DataName.DungeonDataModule);
			this.ticketDataModule = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			switch (this.dungeonID)
			{
			case DungeonID.DragonsLair:
				RedPointController.Instance.RegRecordChange("DailyActivity.DungeonTag.DragonLair", new Action<RedNodeListenData>(base.OnRedPointChange));
				return;
			case DungeonID.AstralTree:
				RedPointController.Instance.RegRecordChange("DailyActivity.DungeonTag.AstralTree", new Action<RedNodeListenData>(base.OnRedPointChange));
				return;
			case DungeonID.SwordIsland:
				RedPointController.Instance.RegRecordChange("DailyActivity.DungeonTag.SwordIsland", new Action<RedNodeListenData>(base.OnRedPointChange));
				return;
			case DungeonID.DeepSeaRuins:
				RedPointController.Instance.RegRecordChange("DailyActivity.DungeonTag.DeepSeaRuins", new Action<RedNodeListenData>(base.OnRedPointChange));
				return;
			default:
				return;
			}
		}

		protected override void OnDeInit()
		{
			this.nodeButton.onClick.RemoveListener(new UnityAction(this.OnClickNode));
			switch (this.dungeonID)
			{
			case DungeonID.DragonsLair:
				RedPointController.Instance.UnRegRecordChange("DailyActivity.DungeonTag.DragonLair", new Action<RedNodeListenData>(base.OnRedPointChange));
				return;
			case DungeonID.AstralTree:
				RedPointController.Instance.UnRegRecordChange("DailyActivity.DungeonTag.AstralTree", new Action<RedNodeListenData>(base.OnRedPointChange));
				return;
			case DungeonID.SwordIsland:
				RedPointController.Instance.UnRegRecordChange("DailyActivity.DungeonTag.SwordIsland", new Action<RedNodeListenData>(base.OnRedPointChange));
				return;
			case DungeonID.DeepSeaRuins:
				RedPointController.Instance.UnRegRecordChange("DailyActivity.DungeonTag.DeepSeaRuins", new Action<RedNodeListenData>(base.OnRedPointChange));
				return;
			default:
				return;
			}
		}

		protected override void OnShow()
		{
			Dungeon_DungeonBase elementById = GameApp.Table.GetManager().GetDungeon_DungeonBaseModelInstance().GetElementById((int)this.dungeonID);
			if (elementById == null)
			{
				HLog.LogError(string.Format("Table [Dungeon_DungeonBase] not found id={0}", (int)this.dungeonID));
				return;
			}
			uint currentLevel = this.dungeonDataModule.GetCurrentLevel((int)this.dungeonID);
			this.textDifficult.text = currentLevel.ToString();
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.name);
			UserTicketKind ticketKind = this.dungeonDataModule.GetTicketKind((int)this.dungeonID);
			UserTicket ticket = this.ticketDataModule.GetTicket(ticketKind);
			if (ticket != null)
			{
				this.textChallengeTime.text = ticket.NewNum.ToString();
			}
			else
			{
				this.textChallengeTime.text = "0";
			}
			Item_Item elementById2 = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(elementById.keyID);
			if (elementById2 != null)
			{
				string atlasPath = GameApp.Table.GetAtlasPath(elementById2.atlasID);
				this.imageTicket.SetImage(atlasPath, elementById2.icon);
			}
			Item_Item elementById3 = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(elementById.showDropItemId);
			if (elementById3 != null)
			{
				string atlasPath2 = GameApp.Table.GetAtlasPath(elementById3.atlasID);
				this.imageShowIcon.SetImage(atlasPath2, elementById3.icon);
			}
			this.textDes.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.desId);
			DelayCall.Instance.CallOnce(10, delegate
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.desTrans);
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
			if (!GameApp.View.IsOpened(ViewName.DungeonViewModule))
			{
				GameApp.View.OpenView(ViewName.DungeonViewModule, this.dungeonID, 1, null, null);
			}
		}

		[SerializeField]
		private DungeonID dungeonID;

		[SerializeField]
		private CustomText textTitle;

		[SerializeField]
		private CustomText textDifficult;

		[SerializeField]
		private CustomText textChallengeTime;

		[SerializeField]
		private CustomImage imageTicket;

		[SerializeField]
		private CustomButton nodeButton;

		[SerializeField]
		private CustomImage imageShowIcon;

		[SerializeField]
		private CustomText textDes;

		[SerializeField]
		private RectTransform desTrans;

		private DungeonDataModule dungeonDataModule;

		private TicketDataModule ticketDataModule;
	}
}
