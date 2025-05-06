using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildLevelDetailItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public void Refresh(GuildShareDataEx.LevelChangeData data)
		{
			if (data == null)
			{
				return;
			}
			this.textInfo.text = data.info;
			this.textLastNum.text = data.lastNum.ToString();
			this.textCurrentNum.text = data.currentNum.ToString();
			if (data.lv == 1)
			{
				this.imageArrow.SetActiveSafe(false);
				this.textCurrentNoArrow.text = data.currentNum.ToString();
			}
			else
			{
				this.imageArrow.SetActiveSafe(true);
				this.textCurrentNoArrow.text = "";
			}
			bool flag = data.lastNum == 0 && data.currentNum == 0;
			this.arrowObj.SetActiveSafe(!flag);
		}

		public void ResetAni()
		{
			if (this.ani == null)
			{
				return;
			}
			this.ani.Play("UIGuildLevelDetailItem_Show");
			this.ani.Update(0f);
			this.ani.enabled = false;
		}

		public void ShowAni(int index)
		{
			this.ResetAni();
			DelayCall.Instance.CallOnce(75 * index, new DelayCall.CallAction(this.OnShowAni));
		}

		private void OnShowAni()
		{
			if (this.ani == null)
			{
				return;
			}
			this.ani.enabled = true;
		}

		public void ClearAni()
		{
			if (this.ani == null)
			{
				return;
			}
			string text = "UIGuildLevelDetailItem_Show";
			AnimationClip[] animationClips = this.ani.runtimeAnimatorController.animationClips;
			float num = 0f;
			for (int i = 0; i < animationClips.Length; i++)
			{
				if (animationClips[i].name.Equals(text))
				{
					num = animationClips[i].length;
					break;
				}
			}
			this.ani.Play(text);
			this.ani.Update(num);
			this.ani.enabled = false;
		}

		public CustomText textInfo;

		public CustomText textLastNum;

		public CustomText textCurrentNum;

		public GameObject arrowObj;

		public GameObject imageArrow;

		public CustomText textCurrentNoArrow;

		public Animator ani;
	}
}
