using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class MapWeatherCtrl
	{
		public void Init(GameObject obj)
		{
			this.gameObject = obj;
			ComponentRegister component = this.gameObject.GetComponent<ComponentRegister>();
			this.effHeavyRain = component.GetGameObject("RainBig");
			this.effLightRain = component.GetGameObject("RainSmall");
			this.effThrunder = component.GetGameObject("Thrunder");
			this.effWind = component.GetGameObject("Wind");
			this.darkMask = component.GetGameObject("DarkMask");
			this.effectDic.Add(EventWeather.HeavyRain, this.effHeavyRain);
			this.effectDic.Add(EventWeather.LightRain, this.effLightRain);
			this.effectDic.Add(EventWeather.StrongWind, this.effWind);
			this.ShowWeather(EventWeather.None);
			this.effThrunder.SetActiveSafe(false);
			this.thrunderFXArr = this.effThrunder.GetComponentsInChildren<ParticleSystem>();
			this.ShowDarkMask(false);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_Weather, new HandlerEvent(this.OnEventChangeWeather));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_EnterNewStage, new HandlerEvent(this.OnEventEnterNewStage));
		}

		public void DeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_Weather, new HandlerEvent(this.OnEventChangeWeather));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_EnterNewStage, new HandlerEvent(this.OnEventEnterNewStage));
			this.effectDic.Clear();
			this.thrunderFXArr = null;
		}

		private void OnEventChangeWeather(object sender, int type, BaseEventArgs args)
		{
			EventArgWeather eventArgWeather = args as EventArgWeather;
			if (eventArgWeather != null && eventArgWeather.data != null)
			{
				ValueTuple<int, bool, bool>? valueTuple = GameEventItemData.ToWeatherData(eventArgWeather.data.param);
				if (valueTuple != null)
				{
					this.currentWeather = (EventWeather)valueTuple.Value.Item1;
					this.endStage = eventArgWeather.data.endStage;
					this.ShowWeather(this.currentWeather);
					this.ShowDarkMask(valueTuple.Value.Item2);
					EventWeather eventWeather = this.currentWeather;
					if (eventWeather == EventWeather.HeavyRain)
					{
						this.effThrunder.SetActiveSafe(true);
						this.ShowThrunder();
						return;
					}
					if (eventWeather != EventWeather.StrongWind)
					{
						return;
					}
					EventArgsLong eventArgsLong = new EventArgsLong();
					eventArgsLong.SetData(2L);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ChangeWaves, eventArgsLong);
				}
			}
		}

		private void OnEventEnterNewStage(object sender, int type, BaseEventArgs args)
		{
			EventArgsInt eventArgsInt = args as EventArgsInt;
			if (eventArgsInt != null && this.endStage - eventArgsInt.Value < 0)
			{
				this.HideWeather();
			}
		}

		private void ShowWeather(EventWeather weather)
		{
			foreach (EventWeather eventWeather in this.effectDic.Keys)
			{
				this.effectDic[eventWeather].SetActiveSafe(eventWeather == weather);
			}
		}

		private void HideWeather()
		{
			foreach (EventWeather eventWeather in this.effectDic.Keys)
			{
				this.effectDic[eventWeather].SetActiveSafe(false);
			}
			this.effThrunder.SetActiveSafe(false);
			if (this.currentWeather == EventWeather.StrongWind)
			{
				EventArgsLong eventArgsLong = new EventArgsLong();
				eventArgsLong.SetData(1L);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ChangeWaves, eventArgsLong);
			}
			this.ShowDarkMask(false);
		}

		private void ShowThrunder()
		{
			if (this.thrunderFXArr == null)
			{
				return;
			}
			if (this.effectDic == null)
			{
				return;
			}
			GameObject gameObject;
			if (this.effectDic.TryGetValue(EventWeather.HeavyRain, out gameObject) && gameObject.activeSelf)
			{
				for (int i = 0; i < this.thrunderFXArr.Length; i++)
				{
					this.thrunderFXArr[i].Play();
				}
				float num = Utility.Math.Random(this.Thrunder_Random_Time_Min, this.Thrunder_Random_Time_Max);
				DelayCall.Instance.CallOnce((int)(num * 1000f), delegate
				{
					if (this.gameObject == null)
					{
						return;
					}
					this.ShowThrunder();
				});
			}
		}

		private void ShowDarkMask(bool isShow)
		{
			this.darkMask.SetActiveSafe(isShow);
		}

		private GameObject gameObject;

		private GameObject effHeavyRain;

		private GameObject effLightRain;

		private GameObject effThrunder;

		private GameObject effWind;

		private GameObject darkMask;

		private Dictionary<EventWeather, GameObject> effectDic = new Dictionary<EventWeather, GameObject>();

		private float Thrunder_Random_Time_Min = 1f;

		private float Thrunder_Random_Time_Max = 3f;

		private ParticleSystem[] thrunderFXArr;

		private int endStage;

		private EventWeather currentWeather;
	}
}
