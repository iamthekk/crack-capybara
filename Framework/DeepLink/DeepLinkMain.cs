using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Framework.DeepLink
{
	public class DeepLinkMain : MonoBehaviour
	{
		private void Awake()
		{
			Application.deepLinkActivated += this.OnDeepLinkActivated;
			if (!string.IsNullOrEmpty(Application.absoluteURL))
			{
				this.OnDeepLinkActivated(Application.absoluteURL);
			}
		}

		private void OnDeepLinkActivated(string url)
		{
			string absolutePath = new Uri(url).AbsolutePath;
			if (string.IsNullOrEmpty(absolutePath))
			{
				return;
			}
			string text = absolutePath.Substring(absolutePath.LastIndexOf('/') + 1);
			this.DeepLinkParam = text;
		}

		public void UpdateDeepLink(string json)
		{
			if ((string.IsNullOrEmpty(this.DeepLinkParam) || string.IsNullOrEmpty(Application.absoluteURL)) && !string.IsNullOrEmpty(json))
			{
				try
				{
					Dictionary<string, string> dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
					if (dictionary != null)
					{
						foreach (KeyValuePair<string, string> keyValuePair in dictionary)
						{
						}
						if (dictionary.ContainsKey("url"))
						{
							this.OnDeepLinkActivated(dictionary["url"]);
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		[HideInInspector]
		public string DeepLinkParam;
	}
}
