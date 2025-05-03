using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class SocialityRankLoadNode : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetActiveNextTxt(bool active)
		{
			if (this.m_nextTxt == null)
			{
				return;
			}
			this.m_nextTxt.SetActive(active);
		}

		public void SetActiveFinishedTxt(bool active)
		{
			if (this.m_finishedTxt == null)
			{
				return;
			}
			this.m_finishedTxt.SetActive(active);
		}

		public void SetActiveLoading(bool active)
		{
			if (this.m_loadingImage == null)
			{
				return;
			}
			this.m_loadingImage.SetActive(active);
		}

		public GameObject m_nextTxt;

		public GameObject m_finishedTxt;

		public GameObject m_loadingImage;
	}
}
