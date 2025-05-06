using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework.Logic.Modules;
using Framework.ResourcesModule;
using UnityEngine;
using UnityEngine.Audio;

namespace Framework.SoundModule
{
	public class AudioManager : MonoBehaviour
	{
		public void OnInit()
		{
			this.lastCheckTime = Time.time;
			this.InitBackgroundAudioSources();
		}

		public void SetMasterVolume(float volume)
		{
			if (this.m_audioMixer == null)
			{
				return;
			}
			this.m_audioMixer.SetFloat("MasterVolume", volume);
		}

		public void PlaySoundEffect(string path, float volume = 1f)
		{
			if (!this.m_isSoundEffectOpen)
			{
				return;
			}
			this.PlaySoundEffectNew(path, volume);
		}

		public async Task PlaySoundEffectNew(string path, float volume = 1f)
		{
			if (this.m_isSoundEffectOpen)
			{
				await this.PlayAudioClip(path, volume, new Func<AudioSource>(this.GetAvailableAudioSource));
			}
		}

		public void SetSoundEffectOpen(bool open)
		{
			this.m_isSoundEffectOpen = open;
			if (!open)
			{
				this.StopAllSoundEffect();
			}
		}

		public bool IsSoundEffectOpen()
		{
			return this.m_isSoundEffectOpen;
		}

		public void SetSoundEffectVolume(float volume)
		{
			if (this.m_audioMixer == null)
			{
				return;
			}
			if (this.m_soundEffectMixerGroup == null)
			{
				return;
			}
			this.m_audioMixer.SetFloat("SoundEffectVolume", volume);
		}

		public void StopAllSoundEffect()
		{
			foreach (ValueTuple<AudioSource, float> valueTuple in this.audioSources)
			{
				AudioSource item = valueTuple.Item1;
				if (item && item.isPlaying)
				{
					item.Stop();
				}
			}
		}

		public void StopSoundEffect(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return;
			}
			ValueTuple<AudioClip, float> valueTuple;
			if (this.soundEffects.TryGetValue(path, out valueTuple))
			{
				AudioClip item = valueTuple.Item1;
				foreach (ValueTuple<AudioSource, float> valueTuple2 in this.audioSources)
				{
					AudioSource item2 = valueTuple2.Item1;
					if (item2 && item2.isPlaying && item2.clip.name == item.name)
					{
						item2.Stop();
						break;
					}
				}
			}
		}

		public AudioMixerGroup GetAudioMixerGroup(SoundName souncname)
		{
			if (souncname == SoundName.Background)
			{
				return this.m_backgroundMixerGroup;
			}
			if (souncname != SoundName.SoundEffect)
			{
				return null;
			}
			return this.m_soundEffectMixerGroup;
		}

		public async Task PlayBackground(string path, float volume = 1f)
		{
			if (!string.IsNullOrEmpty(path))
			{
				if (!this.backgroundMusicSource)
				{
					this.backgroundMusicSource = base.gameObject.AddComponent<AudioSource>();
					this.backgroundMusicSource.loop = true;
				}
				this.currentBackgroundMusicPath = path;
				await this.PlayAudioClip(path, volume, new Func<AudioSource>(this.GetBackgroundMusicSource));
			}
		}

		public void StopBackground()
		{
			if (this.backgroundMusicSource)
			{
				this.backgroundMusicSource.Stop();
				this.backgroundMusicSource.clip = null;
				this.currentBackgroundMusicPath = null;
			}
		}

		public void PauseBackground()
		{
			if (this.backgroundMusicSource)
			{
				this.backgroundMusicSource.Pause();
			}
		}

		public void UnPauseBackground()
		{
			if (this.backgroundMusicSource)
			{
				this.backgroundMusicSource.UnPause();
			}
		}

		public void SetBackgroundVolume(float volume)
		{
			if (this.m_audioMixer == null)
			{
				return;
			}
			if (this.m_backgroundMixerGroup == null)
			{
				return;
			}
			this.m_audioMixer.SetFloat("MusicVolume", volume);
		}

		public void OnDeInit()
		{
			if (this.backgroundMusicSource)
			{
				Object.Destroy(this.backgroundMusicSource);
			}
			foreach (KeyValuePair<string, ValueTuple<AudioClip, float>> keyValuePair in this.soundEffects)
			{
				this.m_resourcesManager.Release<AudioClip>(keyValuePair.Value.Item1);
			}
			this.soundEffects.Clear();
			foreach (ValueTuple<AudioSource, float> valueTuple in this.audioSources)
			{
				Object.Destroy(valueTuple.Item1);
			}
			this.audioSources.Clear();
		}

		private void InitBackgroundAudioSources()
		{
			if (!this.backgroundMusicSource)
			{
				this.backgroundMusicSource = this.CreateAudioSource(true, this.m_backgroundMixerGroup);
			}
		}

		private void Update()
		{
			float time = Time.time;
			if (time - this.lastCheckTime > 5f)
			{
				this.lastCheckTime = time;
				this.CheckAudioClip(time);
				this.CheckAudioSources(time);
			}
		}

		private void CheckAudioClip(float currentTime)
		{
			if (this.isNeedClear)
			{
				this.keysToRemove.Clear();
				this.isNeedClear = false;
			}
			bool flag = false;
			foreach (KeyValuePair<string, ValueTuple<AudioClip, float>> keyValuePair in this.soundEffects)
			{
				if (currentTime - keyValuePair.Value.Item2 > this.unusedAudioClipLifeTime)
				{
					if (keyValuePair.Key != this.currentBackgroundMusicPath)
					{
						this.isNeedClear = true;
						if (this.IsDebug)
						{
							Debug.Log("释放音频资源：".ToColor(DebugColor.OrangeRed) + keyValuePair.Value.Item1.name);
						}
						this.m_resourcesManager.Release<AudioClip>(keyValuePair.Value.Item1);
						this.keysToRemove.Add(keyValuePair.Key);
					}
					else
					{
						flag = true;
					}
				}
			}
			ValueTuple<AudioClip, float> valueTuple;
			if (flag && this.soundEffects.TryGetValue(this.currentBackgroundMusicPath, out valueTuple))
			{
				valueTuple.Item2 = currentTime;
				this.soundEffects[this.currentBackgroundMusicPath] = valueTuple;
				if (this.IsDebug)
				{
					Debug.Log("刷新背景音乐播放时间：".ToColor(DebugColor.Violet) + this.currentBackgroundMusicPath + ":" + currentTime.ToString());
				}
			}
			foreach (string text in this.keysToRemove)
			{
				this.soundEffects.Remove(text);
			}
		}

		private void CheckAudioSources(float currentTime)
		{
			for (int i = this.audioSources.Count - 1; i >= 0; i--)
			{
				ValueTuple<AudioSource, float> valueTuple = this.audioSources[i];
				if (currentTime - valueTuple.Item2 <= this.unusedAudioSourceLifeTime)
				{
					break;
				}
				if (this.IsDebug)
				{
					Debug.Log("释放不用的音源组件：".ToColor(DebugColor.Orange));
				}
				Object.Destroy(valueTuple.Item1);
				this.audioSources.RemoveAt(i);
			}
		}

		private AudioSource GetAvailableAudioSource()
		{
			int i = 0;
			int count = this.audioSources.Count;
			while (i < count)
			{
				ValueTuple<AudioSource, float> valueTuple = this.audioSources[i];
				if (!valueTuple.Item1.isPlaying)
				{
					valueTuple.Item2 = Time.time;
					this.audioSources[i] = valueTuple;
					return valueTuple.Item1;
				}
				i++;
			}
			if (this.audioSources.Count < this.maxAudioSources)
			{
				AudioSource audioSource = this.CreateAudioSource(false, this.m_soundEffectMixerGroup);
				this.audioSources.Add(new ValueTuple<AudioSource, float>(audioSource, Time.time));
				return audioSource;
			}
			if (this.IsDebug)
			{
				Debug.Log("音源组件数量不够：".ToColor(DebugColor.Yellow) + this.maxAudioSources.ToString());
			}
			return null;
		}

		private async Task PlayAudioClip(string path, float volume, Func<AudioSource> getAudioSource)
		{
			if (!string.IsNullOrEmpty(path))
			{
				ValueTuple<AudioClip, float> valueTuple;
				AudioClip audioClip;
				if (!this.soundEffects.TryGetValue(path, out valueTuple))
				{
					audioClip = await this.LoadAndCacheAudioClip(path);
					if (audioClip == null)
					{
						HLog.LogError("AudioClip is null:", path);
						return;
					}
					valueTuple = new ValueTuple<AudioClip, float>(audioClip, -1f);
					this.soundEffects[path] = valueTuple;
				}
				else
				{
					audioClip = valueTuple.Item1;
					if (this.IsDebug)
					{
						Debug.Log("播放音频：".ToColor(DebugColor.Green) + audioClip.name + ":" + Time.time.ToString());
					}
				}
				if (Time.time - valueTuple.Item2 >= this.echoThreshold)
				{
					AudioSource audioSource = ((getAudioSource != null) ? getAudioSource() : null);
					if (audioSource)
					{
						valueTuple.Item2 = Time.time;
						this.soundEffects[path] = valueTuple;
						audioSource.clip = audioClip;
						audioSource.volume = volume;
						audioSource.Play();
					}
				}
				else if (this.IsDebug)
				{
					Debug.Log("时间间隔过短：".ToColor(DebugColor.Red) + audioClip.name + ":" + (Time.time - valueTuple.Item2).ToString());
				}
			}
		}

		private Task<AudioClip> LoadAndCacheAudioClip(string path)
		{
			AudioManager.<LoadAndCacheAudioClip>d__44 <LoadAndCacheAudioClip>d__;
			<LoadAndCacheAudioClip>d__.<>t__builder = AsyncTaskMethodBuilder<AudioClip>.Create();
			<LoadAndCacheAudioClip>d__.<>4__this = this;
			<LoadAndCacheAudioClip>d__.path = path;
			<LoadAndCacheAudioClip>d__.<>1__state = -1;
			<LoadAndCacheAudioClip>d__.<>t__builder.Start<AudioManager.<LoadAndCacheAudioClip>d__44>(ref <LoadAndCacheAudioClip>d__);
			return <LoadAndCacheAudioClip>d__.<>t__builder.Task;
		}

		private AudioSource CreateAudioSource(bool loop, AudioMixerGroup outputGroup)
		{
			AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
			audioSource.loop = loop;
			audioSource.outputAudioMixerGroup = outputGroup;
			return audioSource;
		}

		private AudioSource GetBackgroundMusicSource()
		{
			return this.backgroundMusicSource;
		}

		private void RefreshBackgroundMusicPlayTime(float currentTime)
		{
		}

		[SerializeField]
		[Header("Setting")]
		private ResourcesManager m_resourcesManager;

		[Header("音频开关")]
		public bool OnOff;

		[Header("调试开关")]
		[SerializeField]
		private bool IsDebug;

		[SerializeField]
		[Header("音效设置")]
		private int maxAudioSources = 30;

		[SerializeField]
		private float echoThreshold = 0.015f;

		[SerializeField]
		private float unusedAudioClipLifeTime = 180f;

		[SerializeField]
		private float unusedAudioSourceLifeTime = 60f;

		[Header("AudioMixer")]
		public AudioMixer m_audioMixer;

		public AudioMixerGroup m_backgroundMixerGroup;

		public AudioMixerGroup m_soundEffectMixerGroup;

		private const float checkTime = 5f;

		[TupleElementNames(new string[] { "audioClip", "lastPlayedTime" })]
		private Dictionary<string, ValueTuple<AudioClip, float>> soundEffects = new Dictionary<string, ValueTuple<AudioClip, float>>();

		[TupleElementNames(new string[] { "audioSource", "lastPlayedTime" })]
		private List<ValueTuple<AudioSource, float>> audioSources = new List<ValueTuple<AudioSource, float>>();

		private AudioSource backgroundMusicSource;

		private bool m_isSoundEffectOpen = true;

		private float lastCheckTime;

		private List<string> keysToRemove = new List<string>();

		private bool isNeedClear;

		private const string m_soundEffectVolumeName = "SoundEffectVolume";

		private const string m_backgroundVolumeName = "MusicVolume";

		private const string m_audioMixerVolumeName = "MasterVolume";

		private string currentBackgroundMusicPath;
	}
}
