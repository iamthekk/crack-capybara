using System;
using System.Runtime.CompilerServices;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class PushGiftPopCtrlBase : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
		}

		protected override void OnDeInit()
		{
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			PushGiftItemBase pushGiftItemBase = this.itemBase;
			if (pushGiftItemBase == null)
			{
				return;
			}
			pushGiftItemBase.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public void OnHide()
		{
			for (int i = 0; i < this.itemParent.transform.childCount; i++)
			{
				this.itemParent.transform.GetChild(i).gameObject.SetActiveSafe(false);
			}
		}

		public void SetData(PushGiftData data, Action onClose)
		{
			PushGiftPopCtrlBase.<SetData>d__12 <SetData>d__;
			<SetData>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<SetData>d__.<>4__this = this;
			<SetData>d__.data = data;
			<SetData>d__.onClose = onClose;
			<SetData>d__.<>1__state = -1;
			<SetData>d__.<>t__builder.Start<PushGiftPopCtrlBase.<SetData>d__12>(ref <SetData>d__);
		}

		private void OnClickClose()
		{
			Action onClose = this.OnClose;
			if (onClose == null)
			{
				return;
			}
			onClose();
		}

		[SerializeField]
		private CustomText textTitle;

		[SerializeField]
		private CustomButton buttonClose;

		[SerializeField]
		private CustomLanguageText textClick2Buy;

		[SerializeField]
		private GameObject itemParent;

		private string prefabPath = "Assets/_Resources/Prefab/UI/PushGift/";

		private Action OnClose;

		private PushGiftData _pushGiftData;

		private PushGiftItemBase itemBase;
	}
}
