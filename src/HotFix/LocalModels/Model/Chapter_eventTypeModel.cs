﻿using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Chapter_eventTypeModel : BaseLocalModel
	{
		public Chapter_eventType GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Chapter_eventType> GetAllElements()
		{
			return this.modelImpl.GetAllElement();
		}

		public override void Initialise(string name, byte[] assetBytes)
		{
			base.Initialise(name, assetBytes);
			if (assetBytes == null)
			{
				return;
			}
			this.modelImpl.Initialise(name, assetBytes);
		}

		public override void DeInitialise()
		{
			this.modelImpl.DeInitialise();
			base.DeInitialise();
		}

		public static readonly string fileName = "Chapter_eventType";

		private Chapter_eventTypeModelImpl modelImpl = new Chapter_eventTypeModelImpl();
	}
}
