using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Framework.Logic.AssetReference
{
	[Serializable]
	public class AssetReferenceMaterial : AssetReferenceT<Material>
	{
		public AssetReferenceMaterial(string guid)
			: base(guid)
		{
		}
	}
}
