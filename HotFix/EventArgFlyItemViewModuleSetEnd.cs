using System;
using System.Collections.Generic;
using Framework.EventSystem;
using UnityEngine;

namespace HotFix
{
	public class EventArgFlyItemViewModuleSetEnd : BaseEventArgs
	{
		public void SetData(FlyItemModel model, CurrencyType currencyType, List<Vector3> positions)
		{
			this.m_transforms.Clear();
			this.m_model = model;
			this.m_currencyType = currencyType;
			this.m_positions = positions;
			this.m_type = FlyItemType.Currency;
		}

		public void SetData(FlyItemModel model, CurrencyType currencyType, List<Transform> transforms)
		{
			this.m_positions.Clear();
			this.m_model = model;
			this.m_currencyType = currencyType;
			this.m_type = FlyItemType.Currency;
			this.m_transforms = transforms;
		}

		public void SetData(FlyItemModel model, FlyItemOtherType currencyType, List<Vector3> positions)
		{
			this.m_transforms.Clear();
			this.m_model = model;
			this.m_otherType = currencyType;
			this.m_positions = positions;
			this.m_type = FlyItemType.Other;
		}

		public void SetData(FlyItemModel model, FlyItemOtherType currencyType, List<Transform> transforms)
		{
			this.m_positions.Clear();
			this.m_model = model;
			this.m_otherType = currencyType;
			this.m_type = FlyItemType.Other;
			this.m_transforms = transforms;
		}

		public override void Clear()
		{
		}

		public List<Vector3> m_positions = new List<Vector3>();

		public List<Transform> m_transforms = new List<Transform>();

		public FlyItemModel m_model;

		public CurrencyType m_currencyType;

		public FlyItemOtherType m_otherType;

		public FlyItemType m_type;
	}
}
