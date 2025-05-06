using System;
using Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HotFix
{
	public class HyperlinkHandler : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		private void Awake()
		{
			if (this.textMeshPro == null)
			{
				this.textMeshPro = base.GetComponent<TMP_Text>();
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.textMeshPro == null)
			{
				return;
			}
			int num = TMP_TextUtilities.FindIntersectingLink(this.textMeshPro, Input.mousePosition, GameApp.View.UICamera);
			if (num != -1)
			{
				TMP_LinkInfo tmp_LinkInfo = this.textMeshPro.textInfo.linkInfo[num];
				string linkID = tmp_LinkInfo.GetLinkID();
				string text = "";
				if (linkID.StartsWith("[copy]"))
				{
					GUIUtility.systemCopyBuffer = linkID.Substring("[copy]".Length, linkID.Length - "[copy]".Length);
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("30017"));
				}
				else if (linkID.StartsWith("[copy&open]"))
				{
					string[] array = linkID.Substring("[copy&open]".Length, linkID.Length - "[copy&open]".Length).Split('|', StringSplitOptions.None);
					if (array.Length >= 1)
					{
						text = array[0];
					}
					if (array.Length >= 2)
					{
						GUIUtility.systemCopyBuffer = array[1];
						GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("30017"));
					}
				}
				else
				{
					text = linkID;
				}
				if (!string.IsNullOrEmpty(text))
				{
					Application.OpenURL(text);
				}
			}
		}

		public const string CopyFlag = "[copy]";

		public const string CopyAndOpenFlag = "[copy&open]";

		public TMP_Text textMeshPro;
	}
}
