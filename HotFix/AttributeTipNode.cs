using System;
using System.Collections;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class AttributeTipNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			if (this.m_listen != null)
			{
				this.m_listen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
		}

		protected override void OnDeInit()
		{
			base.StopCoroutine("PlayImpl");
			if (this.m_listen != null)
			{
				this.m_listen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
		}

		public void SetData(string attributeKey, long value)
		{
			Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(attributeKey);
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.LanguageId);
			this.m_txt.text = string.Format("{0} +{1}", infoByID, value);
		}

		public void Play(float delayTime = 0f)
		{
			this.delayTimer = delayTime;
			base.transform.localScale = Vector3.zero;
			base.StartCoroutine("PlayImpl");
		}

		private IEnumerator PlayImpl()
		{
			yield return new WaitForSeconds(this.delayTimer);
			base.transform.localScale = Vector3.one;
			this.m_animator.Play("AttributeTipNode");
			yield break;
		}

		private void OnAnimatorListen(GameObject gameObject, string eventParameter)
		{
			if (string.Equals(eventParameter, "Run"))
			{
				this.m_isPlaying = true;
			}
			if (string.Equals(eventParameter, "End"))
			{
				this.m_isPlaying = false;
				Action<AttributeTipNode> onFinished = this.m_onFinished;
				if (onFinished == null)
				{
					return;
				}
				onFinished(this);
			}
		}

		public float flySpace = 65f;

		public float intervalTime = 0.1f;

		public Animator m_animator;

		public AnimatorListen m_listen;

		public CustomText m_txt;

		public Action<AttributeTipNode> m_onFinished;

		public bool m_isPlaying;

		private float delayTimer;
	}
}
