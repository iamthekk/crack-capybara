using System;
using Dxx.Chat;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.ChatUI.ChatDonationUI
{
	public class ChatDonationCell_Progress : ChatProxy.ChatProxy_BaseBehaviour
	{
		protected override void ChatUI_OnInit()
		{
			if (this.Image_Fill != null)
			{
				this.Image_Fill.fillAmount = 0f;
			}
			if (this.Image_Fill2 != null)
			{
				this.Image_Fill2.fillAmount = 0f;
			}
			if (this.Text_Progress != null)
			{
				this.Text_Progress.text = "";
			}
		}

		protected override void ChatUI_OnUnInit()
		{
		}

		public void SetData(int cur, int cur2, int max)
		{
			if (max < 1)
			{
				max = 1;
			}
			this.Text_Progress.text = string.Format("{0} / {1}", cur, max);
			this.Image_Fill.fillAmount = Mathf.Clamp01((float)cur / (float)max);
			if (this.Image_Fill2 != null)
			{
				this.Image_Fill2.fillAmount = Mathf.Clamp01((float)cur2 / (float)max);
			}
		}

		public Image Image_Fill;

		public Image Image_Fill2;

		public CustomText Text_Progress;
	}
}
