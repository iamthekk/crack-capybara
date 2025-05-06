using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Habby.Mail.Data;

namespace HotFix
{
	public class MailItemRewardNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_item.Init();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			if (this.m_item != null)
			{
				this.m_item.DeInit();
			}
		}

		public void RefreshUI(MailReward data, bool isRead, int index)
		{
			if (this.m_item != null)
			{
				this.m_item.SetData(data.ToItemData().ToPropData());
				this.m_item.OnRefresh();
			}
			if (this.m_grays != null)
			{
				if (isRead)
				{
					this.m_grays.SetUIGray();
					return;
				}
				this.m_grays.Recovery();
			}
		}

		public UIGrays m_grays;

		public UIItem m_item;
	}
}
