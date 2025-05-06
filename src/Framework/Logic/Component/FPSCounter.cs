using System;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class FPSCounter : MonoBehaviour
	{
		private void Start()
		{
			this.m_FpsNextPeriod = Time.realtimeSinceStartup + 0.2f;
		}

		private void Update()
		{
			this.m_CurrentFps += (Time.unscaledDeltaTime - this.m_CurrentFps) * 0.1f;
		}

		private void OnGUI()
		{
			this.myStyle.fontStyle = this.fontStyle;
			this.myStyle.fontSize = this.fontSize;
			float num = 1f / this.m_CurrentFps;
			GUI.Label(new Rect(this.PositionX, this.PositionY, this.Width, this.Height), string.Format("<color=#00ff00> FPS：{0:0.} </color>", num), this.myStyle);
		}

		private const string display = "<color=#00ff00> FPS：{0:0.} </color>";

		private const float fpsMeasurePeriod = 0.2f;

		private int m_FpsAccumulator;

		private float m_FpsNextPeriod = 0.1f;

		private float m_CurrentFps;

		public FontStyle fontStyle;

		public int fontSize = 40;

		public float PositionX = 10f;

		public float PositionY = 150f;

		private readonly float Width = 200f;

		private readonly float Height = 200f;

		private GUIStyle myStyle = new GUIStyle();
	}
}
