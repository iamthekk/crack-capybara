using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Relic;
using Server;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIRelicSpecialChildNode : CustomBehaviour
	{
		public void SetData(RelicData data)
		{
			this.m_data = data;
		}

		protected override void OnInit()
		{
			this.m_table = GameApp.Table.GetManager().GetRelic_relicModelInstance().GetElementById(this.m_data.m_id);
			this.m_btn.onClick.AddListener(new UnityAction(this.OnClickBtn));
			this.m_activeBt.onClick.AddListener(new UnityAction(this.OnClickActiveBt));
			this.OnRefreshUI();
		}

		protected override void OnDeInit()
		{
			this.m_btn.onClick.RemoveAllListeners();
			this.m_activeBt.onClick.RemoveAllListeners();
		}

		private void OnClickBtn()
		{
			RelicDetailsViewModule.OpenData openData = new RelicDetailsViewModule.OpenData();
			openData.m_relicData = this.m_data;
			openData.m_isShowBt = this.m_state == UIRelicSpecialChildNode.State.Active;
			GameApp.View.OpenView(ViewName.RelicDetailsViewModule, openData, 1, null, null);
		}

		private void OnClickActiveBt()
		{
			if (this.m_state != UIRelicSpecialChildNode.State.CanActive)
			{
				return;
			}
			bool flag = false;
			if (GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)this.m_table.id)) > 0L)
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			NetworkUtils.Relic.DoRelicActiveRequest(this.m_data.m_id, delegate(bool isOK, RelicActiveResponse response)
			{
				if (!isOK)
				{
					return;
				}
				if (this.m_animator != null)
				{
					this.m_animator.ResetTrigger("beforeActive");
				}
				if (this.m_animator != null)
				{
					this.m_animator.ResetTrigger("afterActive");
				}
				if (this.m_animator != null)
				{
					this.m_animator.SetTrigger("active");
				}
			});
		}

		private UIRelicSpecialChildNode.State MathfState()
		{
			UIRelicSpecialChildNode.State state;
			if (!this.m_data.m_active)
			{
				state = UIRelicSpecialChildNode.State.UnActive;
				if (GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)this.m_table.id)) > 0L)
				{
					state = UIRelicSpecialChildNode.State.CanActive;
				}
			}
			else
			{
				state = UIRelicSpecialChildNode.State.Active;
			}
			return state;
		}

		public void OnRefreshUI()
		{
			string atlasPath = GameApp.Table.GetAtlasPath(this.m_table.iconAtlasID);
			if (this.m_bg != null)
			{
				this.m_bg.SetImage(atlasPath, this.m_table.bgName);
			}
			if (this.m_baseBg != null)
			{
				this.m_baseBg.SetImage(atlasPath, this.m_table.baseName);
			}
			if (this.m_icon != null)
			{
				this.m_icon.SetImage(atlasPath, this.m_table.iconName);
			}
			this.m_state = this.MathfState();
			switch (this.m_state)
			{
			case UIRelicSpecialChildNode.State.UnActive:
				if (this.m_activeBt != null)
				{
					this.m_activeBt.gameObject.SetActive(false);
				}
				if (this.m_grays != null)
				{
					this.m_grays.SetUIGray();
				}
				if (this.m_animator != null)
				{
					this.m_animator.SetTrigger("beforeActive");
					return;
				}
				break;
			case UIRelicSpecialChildNode.State.CanActive:
				if (this.m_activeBt != null)
				{
					this.m_activeBt.gameObject.SetActive(true);
				}
				if (this.m_grays != null)
				{
					this.m_grays.SetUIGray();
				}
				if (this.m_animator != null)
				{
					this.m_animator.SetTrigger("beforeActive");
					return;
				}
				break;
			case UIRelicSpecialChildNode.State.Active:
				if (this.m_activeBt != null)
				{
					this.m_activeBt.gameObject.SetActive(false);
				}
				if (this.m_grays != null)
				{
					this.m_grays.Recovery();
				}
				if (this.m_animator != null)
				{
					this.m_animator.SetTrigger("afterActive");
					return;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public RelicData m_data;

		public UIRelicSpecialChildNode.State m_state;

		[SerializeField]
		private CustomButton m_btn;

		[SerializeField]
		private CustomImage m_bg;

		[SerializeField]
		private CustomImage m_baseBg;

		[SerializeField]
		private CustomImage m_icon;

		[SerializeField]
		private CustomButton m_activeBt;

		[SerializeField]
		private UIGrays m_grays;

		[SerializeField]
		private Animator m_animator;

		private Relic_relic m_table;

		public enum State
		{
			UnActive,
			CanActive,
			Active
		}
	}
}
