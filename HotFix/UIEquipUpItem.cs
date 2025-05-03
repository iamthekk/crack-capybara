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
	public class UIEquipUpItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_btn.onClick.AddListener(new UnityAction(this.OnClickBt));
			if (this.m_icon != null)
			{
				Equip_equipType elementById = GameApp.Table.GetManager().GetEquip_equipTypeModelInstance().GetElementById((int)this.m_equipType);
				string atlasPath = GameApp.Table.GetAtlasPath(elementById.atlasID);
				this.m_icon.SetImage(atlasPath, elementById.iconName);
			}
			this.PlayAndObjAnimator();
		}

		protected override void OnDeInit()
		{
			if (this.m_btn != null)
			{
				this.m_btn.onClick.RemoveListener(new UnityAction(this.OnClickBt));
			}
		}

		private void OnClickBt()
		{
			EquipSelectorViewModule.OpenData openData = new EquipSelectorViewModule.OpenData();
			openData.m_equiptype = this.m_equipType;
			GameApp.View.OpenView(ViewName.EquipSelectorViewModule, openData, 1, null, null);
		}

		public void PlayAndObjAnimator()
		{
			if (this.m_andObjAnimator != null)
			{
				this.m_andObjAnimator.Play("Run", 0, 0f);
			}
		}

		public void SetActiveAndObj(bool active)
		{
			if (this.m_andObjAnimator == null)
			{
				return;
			}
			this.m_andObjAnimator.gameObject.SetActive(active);
		}

		[SerializeField]
		private CustomButton m_btn;

		[SerializeField]
		private CustomImage m_icon;

		[SerializeField]
		private Animator m_andObjAnimator;

		[SerializeField]
		public EquipType m_equipType;

		[SerializeField]
		public int m_equipTypeIndex = 1;
	}
}
