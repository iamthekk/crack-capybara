using System;
using Dxx.Guild;
using Framework.Logic.UI;

namespace HotFix.GuildUI
{
	public class GuildPositionChangeButton : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.Button.m_onClick = new Action(this.OnChangePosition);
		}

		protected override void GuildUI_OnUnInit()
		{
			this.Button.m_onClick = null;
		}

		public void SetPosition(GuildPositionType currentpos, GuildPositionType targetposition)
		{
			this.CurPositionType = currentpos;
			this.PositionType = targetposition;
			this.RefreshPositionCount();
		}

		public void RefreshPositionCount()
		{
			int currentPositionCount = this.GetCurrentPositionCount();
			int maxPositionCount = this.GetMaxPositionCount();
			string text = "";
			GuildPositionType positionType = this.PositionType;
			if (positionType != GuildPositionType.VicePresident)
			{
				if (positionType == GuildPositionType.Manager)
				{
					text = GuildProxy.Language.GetInfoByID2("400025", currentPositionCount, maxPositionCount);
				}
			}
			else
			{
				text = GuildProxy.Language.GetInfoByID2("400023", currentPositionCount, maxPositionCount);
			}
			if (this.Text_PositionCount != null)
			{
				this.Text_PositionCount.text = text;
			}
		}

		public int GetMaxPositionCount()
		{
			return base.SDK.GuildInfo.GuildData.GetPositionMaxCount(this.PositionType);
		}

		public int GetCurrentPositionCount()
		{
			return base.SDK.GuildInfo.GetMemberCountByPosition(this.PositionType);
		}

		private void OnChangePosition()
		{
			Action<GuildPositionType> onSetPosition = this.OnSetPosition;
			if (onSetPosition == null)
			{
				return;
			}
			onSetPosition(this.PositionType);
		}

		public CustomButton Button;

		public CustomText Text_PositionCount;

		public GuildPositionType CurPositionType;

		public GuildPositionType PositionType;

		public Action<GuildPositionType> OnSetPosition;
	}
}
