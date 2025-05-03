using System;
using Framework;
using Framework.Logic.Platform;
using Framework.Platfrom;
using UnityEngine;

namespace HotFix
{
	public class QualityManager : Singleton<QualityManager>
	{
		public void InitQuality()
		{
			int @int = PlayerPrefsExpand.GetInt("Save_Quality");
			QualityManager.QualityType qualityType;
			if (@int == 0)
			{
				qualityType = (QualityManager.QualityType)Singleton<PlatformHelper>.Instance.GetQualityByDevice();
			}
			else
			{
				qualityType = (QualityManager.QualityType)@int;
			}
			this.ChangeQuality(qualityType);
		}

		public void NextQuality()
		{
			int num = PlayerPrefsExpand.GetInt("Save_Quality");
			num++;
			if (num == 4)
			{
				num = 1;
			}
			this.ChangeQuality((QualityManager.QualityType)num);
		}

		private void ChangeQuality(QualityManager.QualityType type)
		{
			PlayerPrefsExpand.SetInt("Save_Quality", (int)type);
			if (this.m_currentQuality != null)
			{
				this.m_currentQuality.Exit();
				this.m_currentQuality = null;
			}
			switch (type)
			{
			case QualityManager.QualityType.eLow:
				this.m_currentQuality = new QualityLow();
				break;
			case QualityManager.QualityType.eMiddle:
				this.m_currentQuality = new QualityMiddle();
				break;
			case QualityManager.QualityType.eHigh:
				this.m_currentQuality = new QualityHigh();
				break;
			default:
				HLog.LogError("品质设置错误");
				this.m_currentQuality = new QualityMiddle();
				break;
			}
			this.m_currentQuality.Enter();
		}

		public QualityManager.QualityType GetCurrentQuality()
		{
			return (QualityManager.QualityType)PlayerPrefsExpand.GetInt("Save_Quality");
		}

		public float GetDefaultCameraRatio(bool inBattle = true)
		{
			float num = 0.5625f;
			float num2 = (float)Screen.width / (float)Screen.height;
			if (inBattle && num2 > num)
			{
				return 1f;
			}
			return num / num2;
		}

		private const string PlayerPrefsQualityString = "Save_Quality";

		private QualityBase m_currentQuality;

		public enum QualityWidthType
		{
			e480,
			e720,
			e1080,
			Default
		}

		public enum QualityType
		{
			eLow = 1,
			eMiddle,
			eHigh,
			eEnd
		}
	}
}
