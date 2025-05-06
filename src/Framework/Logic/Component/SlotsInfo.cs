using System;
using Spine.Unity;
using UnityEngine;

namespace Framework.Logic.Component
{
	[Serializable]
	public struct SlotsInfo
	{
		[Header("换装")]
		public Sprite 图片;

		[SpineSlot("", "", false, true, false)]
		public string SlotName;

		[SpineAttachment(true, false, false, "SlotName", "", "baseSkinName", true, false)]
		public string SkinKey;
	}
}
