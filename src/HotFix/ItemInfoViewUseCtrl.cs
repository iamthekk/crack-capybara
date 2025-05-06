using System;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class ItemInfoViewUseCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
			this.Button_Reduce.onClick.AddListener(new UnityAction(this.OnClickReduceBt));
			this.Button_Add.onClick.AddListener(new UnityAction(this.OnClickAddBt));
			this.Button_Use.onClick.AddListener(new UnityAction(this.OnClickUseBt));
		}

		public void SetData(PropData prop)
		{
			this.m_propData = prop;
			this.m_tableData = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)this.m_propData.id);
		}

		public void Open(Action onButtonUseClick)
		{
			if (this.m_isOpen)
			{
				return;
			}
			this.onUseClick = onButtonUseClick;
			this.InitRewardData();
			this.m_useIndex = 0;
			ItemType itemType = this.m_tableData.GetItemType();
			if (itemType != ItemType.eTimePack)
			{
				if (itemType == ItemType.eHeroePieces)
				{
					string[] array = this.m_tableData.itemTypeParam[0].Split('|', StringSplitOptions.None);
					int num;
					if (!int.TryParse(array[0], out num))
					{
						HLog.LogError("ItemInfoUseCtrl.Open split[0]:" + array[0] + " is not a int value.");
						return;
					}
					this.m_cardinalnum = num;
					this.m_currentCount = (int)this.m_propData.count / this.m_cardinalnum;
					this.m_maxCount = (int)this.m_propData.count / this.m_cardinalnum;
				}
			}
			else
			{
				this.m_currentCount = (int)this.m_propData.count;
				this.m_maxCount = (int)this.m_propData.count;
			}
			this.m_slider.minValue = (float)this.m_minCount;
			this.m_slider.maxValue = (float)this.m_maxCount;
			this.SetSliderValue(this.m_currentCount);
		}

		private bool IsShowUse()
		{
			return this.m_currentCount > this.m_minCount;
		}

		public bool IsOpen
		{
			get
			{
				return this.m_isOpen;
			}
		}

		public bool IsCanShow
		{
			get
			{
				ItemType itemType = this.m_tableData.GetItemType();
				return itemType == ItemType.eTimePack || itemType == ItemType.eHeroePieces;
			}
		}

		private void InitRewardData()
		{
			ItemType itemType = this.m_tableData.GetItemType();
			if (itemType != ItemType.eTimePack)
			{
				if (itemType != ItemType.eHeroePieces)
				{
					return;
				}
				this.m_currencyNode.SetActive(false);
				return;
			}
			else
			{
				this.m_currencyNode.SetActive(true);
				string[] itemTypeParam = this.m_tableData.itemTypeParam;
				if (itemTypeParam.Length != 1)
				{
					HLog.LogError(string.Format("ItemInfoUseCtrl.Open args.Length:{0} != 1.", itemTypeParam.Length));
					return;
				}
				string[] array = itemTypeParam[0].Split(',', StringSplitOptions.None);
				if (array.Length != 2)
				{
					HLog.LogError(string.Format("ItemInfoUseCtrl.Open split.Length:{0} != 2.", array.Length));
					return;
				}
				int num;
				if (!int.TryParse(array[0], out num))
				{
					HLog.LogError("ItemInfoUseCtrl.Open split[0]:" + array[0] + " is not a int value.");
					return;
				}
				int num2;
				if (!int.TryParse(array[1], out num2))
				{
					HLog.LogError("ItemInfoUseCtrl.Open split[1]:" + array[1] + " is not a int value.");
					return;
				}
				this.m_allTime = num2;
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(num);
				string atlasPath = GameApp.Table.GetAtlasPath(elementById.atlasID);
				this.Image_Icon.SetImage(atlasPath, elementById.icon);
				int goldLevel = GameApp.Data.GetDataModule(DataName.MainCityDataModule).m_goldLevel;
				MainLevelReward_AFKreward elementById2 = GameApp.Table.GetManager().GetMainLevelReward_AFKrewardModelInstance().GetElementById(goldLevel);
				switch (num)
				{
				case 1:
					this.m_perTime = Singleton<GameConfig>.Instance.Hangup_Gold_Duration;
					this.m_perTimeReward = elementById2.HangGold;
					return;
				case 2:
					return;
				case 3:
					this.m_perTime = Singleton<GameConfig>.Instance.Hangup_Dust_Duration;
					this.m_perTimeReward = elementById2.HangDust;
					return;
				case 4:
					this.m_perTime = Singleton<GameConfig>.Instance.Hangup_HeroExp_Duration;
					this.m_perTimeReward = elementById2.HangHeroExp;
					return;
				default:
					return;
				}
			}
		}

		private void UpdateSliderInfo()
		{
			this.Text_SliderCount.text = string.Format("<color=#F76952>{0}</color>/{1}", this.m_currentCount, this.m_maxCount);
			if (this.m_perTime <= 0)
			{
				return;
			}
			int num = this.m_allTime / this.m_perTime * this.m_perTimeReward * this.m_currentCount;
			this.Text_Count.text = DxxTools.FormatNumber((long)num);
			this.UpdateUseButton();
		}

		private void OnSliderValueChanged(float value)
		{
			this.m_currentCount = Utility.Math.Clamp((int)value, this.m_minCount, this.m_maxCount);
			this.UpdateSliderInfo();
		}

		private void SetSliderValue(int count)
		{
			if (this.m_maxCount <= 0)
			{
				this.SetSliderForceEvent(0);
				return;
			}
			this.SetSliderForceEvent(count);
		}

		private void SetSliderForceEvent(int count)
		{
			this.m_slider.SetValueWithoutNotify((float)count);
			Slider.SliderEvent onValueChanged = this.m_slider.onValueChanged;
			if (onValueChanged == null)
			{
				return;
			}
			onValueChanged.Invoke((float)count);
		}

		private void UpdateUseButton()
		{
			bool flag = this.IsShowUse();
			if (flag)
			{
				this.Gray_Button_Use.Recovery();
			}
			else
			{
				this.Gray_Button_Use.SetUIGray();
			}
			this.Button_Use.enabled = flag;
		}

		private void OnClickAddBt()
		{
			if (this.m_currentCount == this.m_maxCount)
			{
				return;
			}
			this.m_currentCount++;
			this.SetSliderValue(this.m_currentCount);
		}

		private void OnClickReduceBt()
		{
			if (this.m_currentCount == this.m_minCount)
			{
				return;
			}
			this.m_currentCount--;
			this.SetSliderValue(this.m_currentCount);
		}

		private void OnClickUseBt()
		{
			Action action = this.onUseClick;
			if (action == null)
			{
				return;
			}
			action();
		}

		public int GetUseCount()
		{
			ItemType itemType = this.m_tableData.GetItemType();
			if (itemType == ItemType.eTimePack)
			{
				return this.m_currentCount;
			}
			if (itemType != ItemType.eHeroePieces)
			{
				return this.m_currentCount;
			}
			return this.m_currentCount * this.m_cardinalnum;
		}

		public int GetUseIndex()
		{
			return this.m_useIndex;
		}

		public virtual void Close()
		{
			if (!this.m_isOpen)
			{
				return;
			}
			this.m_isOpen = false;
			this.onUseClick = null;
		}

		protected override void OnDeInit()
		{
			this.m_slider.onValueChanged.RemoveAllListeners();
			this.Button_Reduce.onClick.RemoveAllListeners();
			this.Button_Add.onClick.RemoveAllListeners();
			this.Button_Use.onClick.RemoveAllListeners();
			this.m_propData = null;
		}

		public GameObject m_currencyNode;

		public CustomImage Image_Icon;

		public CustomText Text_Count;

		public CustomButton Button_Reduce;

		public CustomButton Button_Add;

		public Slider m_slider;

		public CustomText Text_SliderCount;

		public CustomButton Button_Use;

		public UIGrays Gray_Button_Use;

		private bool m_isOpen;

		private PropData m_propData;

		private Action onUseClick;

		private int m_currentCount;

		private int m_maxCount;

		private int m_minCount;

		private int m_cardinalnum;

		private int m_allTime;

		private int m_perTime;

		private int m_perTimeReward;

		private int m_useIndex;

		private Item_Item m_tableData;
	}
}
