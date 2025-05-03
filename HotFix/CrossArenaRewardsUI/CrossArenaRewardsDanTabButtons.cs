using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.CrossArenaRewardsUI
{
	public class CrossArenaRewardsDanTabButtons : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Prefab_Button.SetActive(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(List<CrossArenaRewards> list)
		{
			this.mDataList.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				this.mDataList.Add(list[i].Dan);
			}
		}

		public void RefreshUI()
		{
			if (base.gameObject == null)
			{
				return;
			}
			if (this.mDataList.Count != this.Buttons.Count)
			{
				RectTransform content = this.Scroll.content;
				content.DestroyChildren();
				for (int i = 0; i < this.mDataList.Count; i++)
				{
					int num = this.mDataList[i];
					GameObject gameObject = Object.Instantiate<GameObject>(this.Prefab_Button, content);
					gameObject.name = "Button_Dan" + num.ToString();
					gameObject.SetActive(true);
					CrossArenaRewardsDanTabButtons.DanTabButton danTabButton = new CrossArenaRewardsDanTabButtons.DanTabButton();
					danTabButton.SetGameObject(gameObject);
					danTabButton.SetDan(num);
					danTabButton.OnClick = new Action<int>(this.SwitchDan);
					this.Buttons.Add(danTabButton);
				}
			}
		}

		public void SwitchDan(int dan)
		{
			this.SelectDan = dan;
			for (int i = 0; i < this.Buttons.Count; i++)
			{
				CrossArenaRewardsDanTabButtons.DanTabButton danTabButton = this.Buttons[i];
				danTabButton.SetAsSel(danTabButton.Dan == dan);
			}
			Action<int> onSwitchDan = this.OnSwitchDan;
			if (onSwitchDan == null)
			{
				return;
			}
			onSwitchDan(dan);
		}

		public GameObject Prefab_Button;

		public ScrollRect Scroll;

		public List<CrossArenaRewardsDanTabButtons.DanTabButton> Buttons = new List<CrossArenaRewardsDanTabButtons.DanTabButton>();

		private List<int> mDataList = new List<int>();

		public int SelectDan;

		public Action<int> OnSwitchDan;

		public class DanTabButton
		{
			public int Dan { get; private set; }

			public bool IsSelected
			{
				get
				{
					return this.Button_Choose != null && this.Button_Choose.IsSelected;
				}
			}

			public void SetGameObject(GameObject go)
			{
				this.GObj = go;
				this.Button_Choose = this.GObj.GetComponent<CustomChooseButton>();
				this.Button_Choose.onClick.RemoveListener(new UnityAction(this.OnClickThis));
				this.Button_Choose.onClick.AddListener(new UnityAction(this.OnClickThis));
			}

			public void SetDan(int dan)
			{
				this.Dan = dan;
				string crossArenaDanName = CrossArenaDataModule.GetCrossArenaDanName(dan);
				this.Button_Choose.SetText(crossArenaDanName);
			}

			private void OnClickThis()
			{
				Action<int> onClick = this.OnClick;
				if (onClick == null)
				{
					return;
				}
				onClick(this.Dan);
			}

			public void SetAsSel(bool sel)
			{
				this.Button_Choose.SetSelect(sel);
			}

			public GameObject GObj;

			public CustomChooseButton Button_Choose;

			public Action<int> OnClick;
		}
	}
}
