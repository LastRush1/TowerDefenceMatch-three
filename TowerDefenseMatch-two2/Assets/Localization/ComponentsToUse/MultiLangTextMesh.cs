/*SimpleLocalizator plugin
 * Developed by NightLord189 (nldevelop.com)*/

using UnityEngine;
using TMPro;


namespace SimpleLocalizator {
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class MultiLangTextMesh : MultiLangTextBase {
		TextMeshProUGUI _text;
		TextMeshProUGUI text {
			get {
				if (_text == null)
					_text = GetComponent<TextMeshProUGUI> ();
				return _text;
			}
		}

		protected override void VisualizeString(string str) {
            text.text = str;
		}
	}
}