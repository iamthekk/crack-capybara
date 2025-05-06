using System;
using Framework.Logic.Component;
using Framework.Platfrom;
using UnityEngine;

namespace HotFix
{
	public class CarnivalItemBaseCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.SetOpen(false);
		}

		protected override void OnDeInit()
		{
		}

		public virtual void SetOpen(CarnivalItemBaseCtrl.BoxState eState)
		{
		}

		public virtual void SetOpen(bool open)
		{
		}

		public virtual void SetEquipInfo(PropData data)
		{
		}

		public virtual void SetEquipClickCallBack(Action<UIItem, PropData, object> clickCallBack)
		{
		}

		public virtual void SetActiveText(string textInfo)
		{
		}

		public async void PlayAnimation(bool get)
		{
			await TaskExpand.Delay(500);
			if (this._animator != null)
			{
				if (get)
				{
					this._animator.Play("CanGet");
				}
				else
				{
					this._animator.Play("Idle");
				}
			}
		}

		[SerializeField]
		private Animator _animator;

		private const string AnimationName_Idle = "Idle";

		private const string AnimationName_CanGet = "CanGet";

		public enum BoxState
		{
			State_Gray,
			State_Normal,
			State_Opened
		}
	}
}
