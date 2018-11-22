using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoryText : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_text;
	[SerializeField]
	private float m_normalInterval = 0.05f;
	[SerializeField]
	private float m_fastInterval = 0.01f;
	[SerializeField, Range(0, 1)]
	private float m_textVolume;

	private int m_currentStringNo = 0;
	private bool m_isActive = false;
	private bool m_isFast = false;
	private bool m_isDisplayComplete = false;
	private float m_timeCnt = 0.0f;
	private string m_tmpText = "";

	private float interval
	{
		get { return (m_isFast) ? m_fastInterval : m_normalInterval; }
	}
	public bool isFast
	{
		set { m_isFast = value; }
	}
	public bool isDisplayComplete
	{
		get { return m_isDisplayComplete; }
	}

	private void Start()
	{
		m_isDisplayComplete = true;
	}

	private void Update()
	{
		if (!m_isActive)
			return;

		m_timeCnt += Time.deltaTime;
		if (m_timeCnt <= interval)
			return;

		var str = m_text.text;
		m_text.text = str + m_tmpText[m_currentStringNo].ToString();
		m_timeCnt = 0.0f;
		m_currentStringNo++;
		SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.TextDisplay, m_textVolume);

		if (m_tmpText.Length <= m_currentStringNo)
		{
			m_isActive = false;
			m_isDisplayComplete = true;
			m_text.text = m_tmpText;
		}
	}

	public void SetText(string _text)
	{
		m_isDisplayComplete = false;
		m_isActive = true;
		m_tmpText = _text;
		m_currentStringNo = 0;
		m_text.text = "";
	}
}
