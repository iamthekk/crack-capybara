using System;
using DG.Tweening;
using Framework.Logic;

namespace HotFix
{
	public class DeviceVibration
	{
		public static void SetOpen(bool value)
		{
			DeviceVibration.m_isOpen = value;
		}

		public static void PlayVibration(DeviceVibration.VibrationType type)
		{
			if (!DeviceVibration.m_isOpen)
			{
				return;
			}
			switch (type)
			{
			case DeviceVibration.VibrationType.Light:
				Utility.Vibration.VibrateAndroid(10L);
				return;
			case DeviceVibration.VibrationType.Middle:
				Utility.Vibration.VibratePop();
				return;
			case DeviceVibration.VibrationType.Heavy:
				Utility.Vibration.VibratePeek();
				return;
			case DeviceVibration.VibrationType.Warning:
			{
				DOTween.Kill("VibrationTweenId", false);
				Utility.Vibration.Vibrate();
				Sequence sequence = DOTween.Sequence();
				sequence.id = "VibrationTweenId";
				TweenSettingsExtensions.AppendInterval(sequence, 1f);
				TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(Utility.Vibration.Vibrate));
				TweenSettingsExtensions.AppendInterval(sequence, 1f);
				TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(Utility.Vibration.Vibrate));
				return;
			}
			default:
				return;
			}
		}

		private const string VibrationTweenId = "VibrationTweenId";

		private static bool m_isOpen = true;

		public enum VibrationType
		{
			Light,
			Middle,
			Heavy,
			Warning
		}
	}
}
