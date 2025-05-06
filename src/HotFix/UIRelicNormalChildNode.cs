using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Relic;
using Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIRelicNormalChildNode : CustomBehaviour
	{
		public void SetData(RelicData data)
		{
			this.m_data = data;
		}

		protected override void OnInit()
		{
			this.m_table = GameApp.Table.GetManager().GetRelic_relicModelInstance().GetElementById(this.m_data.m_id);
			this.m_starGroup.SetData(this.m_table.star);
			this.m_starGroup.Init();
			this.m_btn.onClick.AddListener(new UnityAction(this.OnClickBtn));
			this.m_activeBt.onClick.AddListener(new UnityAction(this.OnClickActiveBt));
			this.OnRefreshUI();
		}

		protected override void OnDeInit()
		{
			this.m_btn.onClick.RemoveAllListeners();
			this.m_activeBt.onClick.RemoveAllListeners();
			this.m_starGroup.DeInit();
		}

		private void OnClickBtn()
		{
			RelicDetailsViewModule.OpenData openData = new RelicDetailsViewModule.OpenData();
			openData.m_relicData = this.m_data;
			openData.m_isShowBt = this.m_state == UIRelicNormalChildNode.State.Active;
			GameApp.View.OpenView(ViewName.RelicDetailsViewModule, openData, 1, null, null);
		}

		private void OnClickActiveBt()
		{
			if (this.m_state != UIRelicNormalChildNode.State.CanActive)
			{
				return;
			}
			bool flag = false;
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			if (dataModule.GetItemDataCountByid((ulong)((long)this.m_table.id)) > 0L)
			{
				flag = true;
			}
			else if (dataModule.GetItemDataCountByid((ulong)((long)this.m_table.unlockCostID)) >= (long)this.m_table.unlockCostNumber)
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

		private UIRelicNormalChildNode.State MathfState()
		{
			UIRelicNormalChildNode.State state;
			if (!this.m_data.m_active)
			{
				state = UIRelicNormalChildNode.State.UnActive;
				PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
				if (dataModule.GetItemDataCountByid((ulong)((long)this.m_table.id)) > 0L)
				{
					state = UIRelicNormalChildNode.State.CanActive;
				}
				else if (dataModule.GetItemDataCountByid((ulong)((long)this.m_table.unlockCostID)) >= (long)this.m_table.unlockCostNumber)
				{
					state = UIRelicNormalChildNode.State.CanActive;
				}
			}
			else
			{
				state = UIRelicNormalChildNode.State.Active;
			}
			return state;
		}

		public void OnRefreshUI()
		{
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
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
			long itemDataCountByid = dataModule.GetItemDataCountByid((ulong)((long)this.m_table.unlockCostID));
			this.m_state = this.MathfState();
			switch (this.m_state)
			{
			case UIRelicNormalChildNode.State.UnActive:
				if (this.m_starGroup != null)
				{
					this.m_starGroup.SetActive(false);
				}
				if (this.m_sliderTxt != null)
				{
					this.m_sliderTxt.text = string.Format("{0}/{1}", itemDataCountByid, this.m_table.unlockCostNumber);
				}
				if (this.m_slider != null)
				{
					this.m_slider.value = (float)itemDataCountByid * 1f / (float)this.m_table.unlockCostNumber;
					this.m_slider.gameObject.SetActive(true);
				}
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
				}
				if (this.m_redPointObj != null)
				{
					this.m_redPointObj.SetActive(false);
					return;
				}
				break;
			case UIRelicNormalChildNode.State.CanActive:
				if (this.m_starGroup != null)
				{
					this.m_starGroup.SetActive(false);
				}
				if (this.m_slider != null)
				{
					this.m_slider.gameObject.SetActive(false);
				}
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
				}
				if (this.m_redPointObj != null)
				{
					this.m_redPointObj.SetActive(false);
					return;
				}
				break;
			case UIRelicNormalChildNode.State.Active:
				if (this.m_starGroup != null)
				{
					this.m_starGroup.SetCount(this.m_data.m_quality);
					this.m_starGroup.SetActive(true);
				}
				if (this.m_sliderTxt != null)
				{
					this.m_sliderTxt.text = string.Format("{0}/{1}", itemDataCountByid, this.m_table.unlockCostNumber);
				}
				if (this.m_slider != null)
				{
					this.m_slider.value = (float)itemDataCountByid * 1f / (float)this.m_table.unlockCostNumber;
					this.m_slider.gameObject.SetActive(true);
				}
				if (this.m_redPointObj != null)
				{
					RelicDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.RelicDataModule);
					this.m_redPointObj.SetActive(dataModule2.IsShowRedPointObj(this.m_data));
				}
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

		public UIRelicNormalChildNode.State m_state;

		[SerializeField]
		private CustomButton m_btn;

		[SerializeField]
		private CustomImage m_bg;

		[SerializeField]
		private CustomImage m_baseBg;

		[SerializeField]
		private CustomImage m_icon;

		[SerializeField]
		private Slider m_slider;

		[SerializeField]
		private CustomText m_sliderTxt;

		[SerializeField]
		private UIRelicStar m_starGroup;

		[SerializeField]
		private CustomButton m_activeBt;

		[SerializeField]
		private UIGrays m_grays;

		[SerializeField]
		private Animator m_animator;

		[SerializeField]
		private GameObject m_redPointObj;

		private Relic_relic m_table;

		public enum State
		{
			UnActive,
			CanActive,
			Active
		}
	}
}
