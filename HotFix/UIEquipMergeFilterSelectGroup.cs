using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIEquipMergeFilterSelectGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_currentSelectIndex = -1;
			this.m_maskClose.onClick.AddListener(new UnityAction(this.OnClickMaskClose));
			this.m_buttonCtrls[0].SetData(0);
			this.m_buttonCtrls[0].m_onClickBt = new Action<UIEquipMergeFilterSelectNode>(this.OnClickBt);
			this.m_buttonCtrls[0].Init();
			for (int i = 1; i < 5; i++)
			{
				this.m_buttonCtrls[i].SetData(i);
				this.m_buttonCtrls[i].m_onClickBt = new Action<UIEquipMergeFilterSelectNode>(this.OnClickBt);
				this.m_buttonCtrls[i].Init();
				Equip_equipType elementById = GameApp.Table.GetManager().GetEquip_equipTypeModelInstance().GetElementById(i);
				this.m_buttonCtrls[i].SetIcon(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.iconName);
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			if (this.m_maskClose != null)
			{
				this.m_maskClose.onClick.RemoveListener(new UnityAction(this.OnClickMaskClose));
			}
			this.m_onChangeSelect = null;
			for (int i = 0; i < this.m_buttonCtrls.Count; i++)
			{
				UIEquipMergeFilterSelectNode uiequipMergeFilterSelectNode = this.m_buttonCtrls[i];
				if (!(uiequipMergeFilterSelectNode == null))
				{
					uiequipMergeFilterSelectNode.DeInit();
				}
			}
			this.m_buttonCtrls.Clear();
		}

		public void SetSelect(int index)
		{
			for (int i = 0; i < this.m_buttonCtrls.Count; i++)
			{
				UIEquipMergeFilterSelectNode uiequipMergeFilterSelectNode = this.m_buttonCtrls[i];
				if (!(uiequipMergeFilterSelectNode == null))
				{
					if (uiequipMergeFilterSelectNode.m_index == index)
					{
						uiequipMergeFilterSelectNode.SetSelect(true);
						if (this.m_currentSelectIndex != index && this.m_onChangeSelect != null)
						{
							this.m_onChangeSelect(index);
						}
						this.m_currentSelectIndex = index;
					}
					else
					{
						uiequipMergeFilterSelectNode.SetSelect(false);
					}
				}
			}
		}

		private void OnClickMaskClose()
		{
			base.SetActive(false);
			if (this.m_onClose != null)
			{
				this.m_onClose(this.m_currentSelectIndex);
			}
		}

		private void OnClickBt(UIEquipMergeFilterSelectNode obj)
		{
			this.SetSelect(obj.m_index);
			this.OnClickMaskClose();
		}

		[SerializeField]
		private CustomButton m_maskClose;

		[SerializeField]
		private List<UIEquipMergeFilterSelectNode> m_buttonCtrls = new List<UIEquipMergeFilterSelectNode>();

		[SerializeField]
		private int m_currentSelectIndex = -1;

		public Action<int> m_onChangeSelect;

		public Action<int> m_onClose;
	}
}
