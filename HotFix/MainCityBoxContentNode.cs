using System;
using DG.Tweening;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class MainCityBoxContentNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_tfBoxNode = this.BoxNode.transform as RectTransform;
			this.m_button.onClick.AddListener(new UnityAction(this.OnClickButton));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.m_onClick = null;
			if (this.m_button != null)
			{
				this.m_button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
			}
		}

		public void SetAsHasBox(bool has)
		{
			this.BoxNode.SetActive(has);
		}

		public void PlayBoxScaleToShow(SequencePool m_seqPool)
		{
			this.SetAsHasBox(true);
			if (m_seqPool == null || this.m_tfBoxNode == null)
			{
				return;
			}
			DxxTools.UI.DoScaleAnim(m_seqPool.Get(), this.m_tfBoxNode, 0f, 1f, (float)this.m_index * 0.03f, 0.2f, 0);
		}

		public void SetIndex(int index)
		{
			this.m_index = index;
		}

		public void SetOnClick(Action<MainCityBoxContentNode> onClick)
		{
			this.m_onClick = onClick;
		}

		public void RefreshUI(MainCityBoxData boxData)
		{
			this.m_data = boxData;
			base.gameObject.transform.localScale = Vector3.one;
			GameApp.Table.GetManager().GetBox_boxBaseModelInstance().GetElementById(this.m_data.m_quality);
			this.m_text.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6511);
			this.m_animator.Play("NodeRun", 0, 0f);
		}

		public void PlayScale()
		{
			base.gameObject.transform.localScale = Vector3.zero;
			ShortcutExtensions.DOScale(base.gameObject.transform, Vector3.one, 0.1f);
		}

		private void OnClickButton()
		{
			if (this.m_onClick == null)
			{
				return;
			}
			this.m_onClick(this);
		}

		[Header("底板")]
		public GameObject BgNode;

		public CustomText TextNoBox;

		public CustomImage ImageNoBox;

		[Header("宝箱")]
		public GameObject BoxNode;

		public CustomButton m_button;

		public CustomImage m_icon;

		public CustomText m_text;

		public Animator m_animator;

		private RectTransform m_tfBoxNode;

		public int m_index;

		public MainCityBoxData m_data;

		public Action<MainCityBoxContentNode> m_onClick;
	}
}
