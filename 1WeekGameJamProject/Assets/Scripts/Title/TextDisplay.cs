using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_text;
	[SerializeField, Range(0.001f, 0.3f)]
	private float m_interval = 0.05f;

	private int m_currentStringNo = 0;
	private float m_timeCnt = 0.0f;
	private string m_tmpText = "";
	private bool m_isActive = false;

	public TextMeshProUGUI text { get { return m_text; } }

	public void SetText(string _text)
	{
		m_isActive = true;
		m_tmpText = _text;
		m_currentStringNo = 0;
		m_text.text = "";
	}

	public void ClearText()
	{
		m_isActive = false;
		m_tmpText = "";
		m_text.text = "";
	}

	void Update()
	{
		if (!m_isActive)
			return;
		m_timeCnt += Time.deltaTime;
		if (m_timeCnt <= m_interval)
			return;

		var str = m_text.text;
		m_text.text = str + m_tmpText[m_currentStringNo].ToString();
		m_timeCnt = 0.0f;
		m_currentStringNo++;
		SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.TextDisplay);

		if (m_tmpText.Length <= m_currentStringNo)
		{
			m_isActive = false;
			m_text.text = m_tmpText;
		}
	}
}
