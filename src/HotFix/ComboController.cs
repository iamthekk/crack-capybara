using System;
using DG.Tweening;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class ComboController : RuntimeBehaviour
	{
		protected override void OnInit()
		{
			ComponentRegister component = base.gameObject.GetComponent<ComponentRegister>();
			this.Text_Combo = component.GetGameObject("Text_Combo").GetComponent<CustomText>();
			this.Show(false);
		}

		protected override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.m_seqPool.Clear(false);
		}

		public void SetCombo(int value)
		{
			this.Text_Combo.text = value.ToString();
			this.m_seqPool.Clear(false);
			if (value == 0)
			{
				return;
			}
			Sequence sequence = this.m_seqPool.Get();
			Transform transform = this.Text_Combo.transform;
			transform.localScale = Vector3.one;
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(transform, Vector3.one * 1.8f, 0.15f)), ShortcutExtensions.DOScale(transform, Vector3.one * 1f, 0.15f));
		}

		public void Show(bool value)
		{
			base.gameObject.SetActive(value);
			this.Text_Combo.text = "0";
		}

		public CustomText Text_Combo;

		private SequencePool m_seqPool = new SequencePool();
	}
}
