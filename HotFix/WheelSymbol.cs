using System;
using System.Collections.Generic;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class WheelSymbol : MonoBehaviour
	{
		private void Start()
		{
		}

		public void SetData(WheelSymbolData data)
		{
			this.data = data;
			this.areaAngle = data.areaAngle;
			this.areaStyle = data.cfg.planeColor;
			this.UpdateView();
			this.rewardItem.SetData(data);
		}

		private void UpdateView()
		{
			float num = this.areaAngle / 360f;
			if (this.imgArea != null)
			{
				this.imgArea.fillAmount = num;
			}
			if (this.imgAreaLineL != null)
			{
				this.imgAreaLineL.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
			if (this.imgAreaLineR != null)
			{
				this.imgAreaLineR.transform.localRotation = Quaternion.Euler(0f, 0f, -this.areaAngle);
			}
			if (this.rewardItem != null)
			{
				this.rewardItem.transform.localRotation = Quaternion.Euler(0f, 0f, -this.areaAngle * 0.5f);
			}
			if (this.imgAreaStyleList.Count >= this.areaStyle)
			{
				this.imgArea.sprite = this.imgAreaStyleList[this.areaStyle - 1];
			}
		}

		[Range(1f, 360f)]
		public float areaAngle = 45f;

		public CustomImage imgArea;

		public CustomImage imgAreaLineL;

		public CustomImage imgAreaLineR;

		public WheelSymbolReward rewardItem;

		[Range(1f, 2f)]
		public int areaStyle = 1;

		public List<Sprite> imgAreaStyleList = new List<Sprite>();

		private WheelSymbolData data;
	}
}
