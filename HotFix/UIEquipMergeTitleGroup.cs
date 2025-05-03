using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIEquipMergeTitleGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			if (this.m_oneClickMergeBt != null)
			{
				this.m_oneClickMergeBt.onClick.AddListener(new UnityAction(this.OnClickOneClickMergeBt));
			}
			if (this.m_filterBt != null)
			{
				this.m_filterBt.onClick.AddListener(new UnityAction(this.OnClickFilterBt));
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			if (this.m_oneClickMergeBt != null)
			{
				this.m_oneClickMergeBt.onClick.RemoveListener(new UnityAction(this.OnClickOneClickMergeBt));
			}
			if (this.m_filterBt != null)
			{
				this.m_filterBt.onClick.RemoveListener(new UnityAction(this.OnClickFilterBt));
			}
			this.m_onClickOneClickMergeBt = null;
			this.m_onClickFilterBt = null;
		}

		private void OnClickOneClickMergeBt()
		{
			if (this.m_onClickOneClickMergeBt == null)
			{
				return;
			}
			this.m_onClickOneClickMergeBt(this);
		}

		private void OnClickFilterBt()
		{
			if (this.m_onClickFilterBt == null)
			{
				return;
			}
			this.m_onClickFilterBt(this);
		}

		public void SetEquipType(int equipType)
		{
			EquipType equipType2 = (EquipType)equipType;
			int num;
			bool flag = !int.TryParse(equipType2.ToString(), out num);
			if (this.m_filterBtIconTxt != null)
			{
				this.m_filterBtIconTxt.SetActive(!flag);
			}
			if (this.m_filterBtIcon != null)
			{
				if (flag)
				{
					Equip_equipType equip_equipType = GameApp.Table.GetManager().GetEquip_equipType(equipType);
					string atlasPath = GameApp.Table.GetAtlasPath(equip_equipType.atlasID);
					this.m_filterBtIcon.SetImage(atlasPath, equip_equipType.iconName);
				}
				if (this.m_filterBtIcon != null)
				{
					this.m_filterBtIcon.gameObject.SetActive(flag);
				}
			}
		}

		public void SetUnfold(bool isUnfold)
		{
			if (this.m_filterUp != null)
			{
				this.m_filterUp.SetActive(!isUnfold);
			}
			if (this.m_filterDown != null)
			{
				this.m_filterDown.SetActive(isUnfold);
			}
		}

		public void SetFilterActive(bool active)
		{
			if (this.m_filter == null)
			{
				return;
			}
			this.m_filter.SetActive(active);
		}

		public void SetOneClickBtGray(bool isGray)
		{
			if (this.m_oneClickMergeBtGray == null)
			{
				return;
			}
			if (isGray)
			{
				this.m_oneClickMergeBtGray.SetUIGray();
				return;
			}
			this.m_oneClickMergeBtGray.Recovery();
		}

		[SerializeField]
		private CustomButton m_oneClickMergeBt;

		[SerializeField]
		private UIGrays m_oneClickMergeBtGray;

		[SerializeField]
		private CustomButton m_filterBt;

		[SerializeField]
		private CustomImage m_filterBtIcon;

		[SerializeField]
		private GameObject m_filterBtIconTxt;

		[SerializeField]
		private GameObject m_filterUp;

		[SerializeField]
		private GameObject m_filterDown;

		[SerializeField]
		private GameObject m_filter;

		public Action<UIEquipMergeTitleGroup> m_onClickOneClickMergeBt;

		public Action<UIEquipMergeTitleGroup> m_onClickFilterBt;
	}
}
