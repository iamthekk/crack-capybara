using System;
using UnityEngine;

namespace Framework.Logic.Modules
{
	[RequireComponent(typeof(AudioSource))]
	public class SoundNameController : MonoBehaviour
	{
		private void Awake()
		{
			if (this.m_audioSource == null)
			{
				this.m_audioSource = base.gameObject.GetComponent<AudioSource>();
			}
			if (this.m_audioSource != null && GameApp.Sound != null)
			{
				this.m_audioSource.outputAudioMixerGroup = GameApp.Sound.GetAudioMixerGroup(this.m_soundName);
			}
		}

		public AudioSource m_audioSource;

		public SoundName m_soundName = SoundName.SoundEffect;
	}
}
