using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIEquipMergeAttributeNode : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public void PlayAnimator(string trigger)
		{
		}

		public void SetAttributeIcon(string attributeName)
		{
			if (this.m_icon == null)
			{
				return;
			}
			Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(attributeName);
			if (elementById == null)
			{
				this.m_icon.gameObject.SetActive(false);
				return;
			}
			this.m_icon.gameObject.SetActive(true);
			this.m_icon.SetImage(elementById.iconAtlasID, elementById.iconName);
		}

		public void SetNameTxt(string info)
		{
			if (this.m_name == null)
			{
				return;
			}
			this.m_name.text = info;
		}

		public void SetFromTxt(string info)
		{
		}

		public void SetToTxt(string info)
		{
			if (this.m_toTxt == null)
			{
				return;
			}
			this.m_toTxt.text = info;
		}

		[SerializeField]
		private CustomImage m_icon;

		[SerializeField]
		private CustomText m_name;

		[SerializeField]
		private CustomText m_toTxt;
	}
}
