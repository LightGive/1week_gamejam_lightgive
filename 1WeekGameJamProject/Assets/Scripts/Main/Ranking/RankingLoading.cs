using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingLoading : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_textLoading;
	[SerializeField]
	private float m_interval = 0.8f;
	[SerializeField]
	private string[] m_loadingTexts;

	private float m_timeCnt = 0.0f;
	private int m_idx = 0;

	private void OnEnable()
	{
		m_textLoading.text = m_loadingTexts[0];
		m_timeCnt = 0.0f;
		m_idx = 0;
	}

	void Update()
	{
		m_timeCnt += Time.deltaTime;
		if (m_timeCnt > m_interval)
		{
			m_textLoading.text = m_loadingTexts[m_idx];
			m_timeCnt = 0.0f;

			m_idx++;
			if (m_idx >= m_loadingTexts.Length)
				m_idx = 0;
		}
	}
}
