using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.Modules;
using Framework.Logic.Tools;
using Framework.Platfrom;
using Framework.ResourcesModule;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Framework.SoundModule
{
	public class SoundManager : MonoBehaviour
	{
		public void OnInit()
		{
			if (this.m_soundNodePrefab == null)
			{
				return;
			}
			for (int i = 0; i < this.m_soundEffectPoolCount; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.m_soundNodePrefab, base.transform);
				AudioSource component = gameObject.GetComponent<AudioSource>();
				component.outputAudioMixerGroup = this.m_soundEffectMixerGroup;
				int instanceID = gameObject.GetInstanceID();
				this.m_audioSources[instanceID] = component;
				this.m_inIDs.Add(instanceID);
				component.gameObject.SetActive(true);
				component.enabled = false;
			}
		}

		public void OnDeInit()
		{
			this.StopBackground();
			foreach (KeyValuePair<int, AudioSource> keyValuePair in this.m_audioSources)
			{
				if (!(keyValuePair.Value == null))
				{
					if (keyValuePair.Value.isPlaying)
					{
						keyValuePair.Value.Stop();
					}
					if (keyValuePair.Value.clip != null)
					{
						this.m_resourcesManager.Release<AudioClip>(keyValuePair.Value.clip);
					}
					keyValuePair.Value.clip = null;
					Object.Destroy(keyValuePair.Value.gameObject);
				}
			}
			this.m_audioSources.Clear();
			this.m_inIDs.Clear();
			this.m_outIDs.Clear();
		}

		public void SetMasterVolume(float volume)
		{
			if (this.m_audioMixer == null)
			{
				return;
			}
			this.m_audioMixer.SetFloat("MasterVolume", volume);
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

		public void PlayBackground(string path, float volume = 1f)
		{
			SoundManager.<PlayBackground>d__22 <PlayBackground>d__;
			<PlayBackground>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<PlayBackground>d__.<>4__this = this;
			<PlayBackground>d__.path = path;
			<PlayBackground>d__.volume = volume;
			<PlayBackground>d__.<>1__state = -1;
			<PlayBackground>d__.<>t__builder.Start<SoundManager.<PlayBackground>d__22>(ref <PlayBackground>d__);
		}

		public void StopBackground()
		{
			if (this.m_backgroundAudioSource == null)
			{
				return;
			}
			if (this.m_backgroundAudioSource.clip == null)
			{
				return;
			}
			this.m_backgroundAudioSource.Stop();
			this.m_resourcesManager.Release<AudioClip>(this.m_backgroundAudioSource.clip);
			this.m_backgroundAudioSource.clip = null;
		}

		public void PauseBackground()
		{
			if (this.m_backgroundAudioSource == null)
			{
				return;
			}
			this.m_backgroundAudioSource.Pause();
		}

		public void UnPauseBackground()
		{
			if (this.m_backgroundAudioSource == null)
			{
				return;
			}
			this.m_backgroundAudioSource.UnPause();
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
			this.m_audioMixer.SetFloat(this.m_backgroundVolumeName, volume);
		}

		public void SetSoundEffectOpen(bool open)
		{
			this.m_isSoundEffectOpen = open;
		}

		public bool IsSoundEffectOpen()
		{
			return this.m_isSoundEffectOpen;
		}

		private async Task<AudioClip> GetSoundEffectClip(string path)
		{
			AudioClip audioClip;
			if (!this.m_asynsSoundEffects.TryGetValue(path, out audioClip))
			{
				AsyncOperationHandle<AudioClip> asyHandler = GameApp.Resources.LoadAssetAsync<AudioClip>(path);
				await asyHandler.Task;
				if (asyHandler.Status != 1)
				{
					HLog.LogError("CheckPrefab Load Prefab is null! path = " + path + " ");
					return null;
				}
				this.m_asynsSoundEffects[path] = asyHandler.Result;
				asyHandler = default(AsyncOperationHandle<AudioClip>);
			}
			return this.m_asynsSoundEffects[path];
		}

		public void ClearSoundEffects()
		{
			if (this.m_asynsSoundEffects != null)
			{
				this.m_asynsSoundEffects.Clear();
			}
		}

		public async void PlaySoundEffect(string path, float volume = 1f)
		{
			if (this.m_isSoundEffectOpen)
			{
				if (!string.IsNullOrEmpty(path))
				{
					if (this.m_soundEffectCurrentFrame != Time.frameCount)
					{
						this.m_soundEffectCurrentFrame = Time.frameCount;
						this.m_soundEffectCurrentFrameCount = 0;
					}
					this.m_soundEffectCurrentFrameCount++;
					if (this.m_soundEffectCurrentFrameCount <= this.m_soundEffectFrameCount)
					{
						AudioSource audioSource = this.Out();
						if (!(audioSource == null))
						{
							Task<AudioClip> handler = this.GetSoundEffectClip(path);
							await handler;
							if (!(audioSource == null))
							{
								audioSource.clip = handler.Result;
								audioSource.enabled = true;
								audioSource.volume = volume;
								audioSource.Play();
								await TaskExpand.Delay((int)((audioSource.clip.length + 0.1f) * 1000f));
								if (audioSource != null)
								{
									audioSource.Stop();
									audioSource.enabled = false;
									audioSource.clip = null;
									this.Put(audioSource);
								}
							}
						}
					}
				}
			}
		}

		public Task PlaySoundEffectNew(string path, float volume, FrameworkTaskOutValue<GameObject> outValue)
		{
			SoundManager.<PlaySoundEffectNew>d__32 <PlaySoundEffectNew>d__;
			<PlaySoundEffectNew>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<PlaySoundEffectNew>d__.<>4__this = this;
			<PlaySoundEffectNew>d__.path = path;
			<PlaySoundEffectNew>d__.volume = volume;
			<PlaySoundEffectNew>d__.outValue = outValue;
			<PlaySoundEffectNew>d__.<>1__state = -1;
			<PlaySoundEffectNew>d__.<>t__builder.Start<SoundManager.<PlaySoundEffectNew>d__32>(ref <PlaySoundEffectNew>d__);
			return <PlaySoundEffectNew>d__.<>t__builder.Task;
		}

		public void StopSoundEffectNew(GameObject obj)
		{
			if (obj)
			{
				this.StopSoundEffectNew(obj.GetInstanceID());
			}
		}

		public void StopSoundEffectNew(int instanceId)
		{
			AudioSource audioSource;
			if (this.m_audioSources.TryGetValue(instanceId, out audioSource) && this.m_outIDs.Contains(instanceId))
			{
				audioSource.Stop();
				audioSource.enabled = false;
				if (audioSource.clip != null)
				{
					this.m_resourcesManager.Release<AudioClip>(audioSource.clip);
				}
				audioSource.clip = null;
				this.Put(audioSource);
			}
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
			this.m_audioMixer.SetFloat(this.m_soundEffectVolumeName, volume);
		}

		private AudioSource Out()
		{
			int count = this.m_inIDs.Count;
			if (count <= 0)
			{
				return null;
			}
			int num = this.m_inIDs[count - 1];
			AudioSource audioSource;
			this.m_audioSources.TryGetValue(num, out audioSource);
			if (audioSource == null)
			{
				return null;
			}
			this.m_inIDs.RemoveAt(count - 1);
			this.m_outIDs.Add(num);
			return audioSource;
		}

		private void Put(AudioSource audioSource)
		{
			if (audioSource == null)
			{
				return;
			}
			int instanceID = audioSource.gameObject.GetInstanceID();
			this.m_outIDs.Remove(instanceID);
			this.m_inIDs.Add(instanceID);
		}

		[Header("Setting")]
		[SerializeField]
		private ResourcesManager m_resourcesManager;

		[Header("音频开关")]
		public bool OnOff;

		[Header("AudioMixer")]
		public AudioMixer m_audioMixer;

		[Header("Background")]
		public AudioSource m_backgroundAudioSource;

		public AudioMixerGroup m_backgroundMixerGroup;

		public string m_backgroundVolumeName = string.Empty;

		[Header("SoundEffect")]
		public GameObject m_soundNodePrefab;

		public AudioMixerGroup m_soundEffectMixerGroup;

		public string m_soundEffectVolumeName = string.Empty;

		[Space(10f)]
		public int m_soundEffectPoolCount = 30;

		public int m_soundEffectFrameCount = 10;

		[SerializeField]
		[Label]
		private int m_soundEffectCurrentFrame;

		[SerializeField]
		[Label]
		private int m_soundEffectCurrentFrameCount;

		private Dictionary<int, AudioSource> m_audioSources = new Dictionary<int, AudioSource>();

		private List<int> m_inIDs = new List<int>();

		private List<int> m_outIDs = new List<int>();

		private bool m_isSoundEffectOpen = true;

		private Dictionary<string, AudioClip> m_asynsSoundEffects = new Dictionary<string, AudioClip>();
	}
}
