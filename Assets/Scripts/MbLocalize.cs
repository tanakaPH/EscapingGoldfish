/*
端末言語によって日本語・英語を切り替える
  
■　 使い方
Image または Text コンポーネントが割あたっているゲームオブジェクトにD&Dして、
日本語＆英語　テキスト・画像　を割り当てればOKです。
動的に変更されているテキストに関しては、別途処理が必要ですが、基本的にこのクラス内と同じ処理を行えばOKです。
  
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MbLocalize : MonoBehaviour
{

	void Start()
	{
		SetLanguageUI();
	}


	[SerializeField, Header("■　日本語＆英語用のSpriteを入れてください。")]
	Sprite m_image_jp;
	[SerializeField]
	Sprite m_image_en;

	[SerializeField, Header("■　日本語＆英語用のテキストを入れてください。"), TextArea(1, 4)]
	string m_text_jp;
	[SerializeField, TextArea(1, 4)]
	string m_text_en;

	void SetLanguageUI()
	{
		SpriteRenderer spR = GetComponent<SpriteRenderer>();
		Text txt = GetComponent<Text>();

		switch (PlayerPrefs.GetString("Language", ""))
		{
			case "Japanese":
				if (spR) spR.sprite = m_image_jp;
				if (txt) txt.text = m_text_jp;
				break;
			case "English":
				if (spR) spR.sprite = m_image_en;
				if (txt) txt.text = m_text_en;
				break;
			default:// デフォルトでは英語に
				if (spR) spR.sprite = m_image_en;
				if (txt) txt.text = m_text_en;
				break;
		}
	}
}