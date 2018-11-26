using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCountUp : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_textScore;
	[SerializeField]
	private float m_countUpSpeed;

	private bool m_isStart = false;
	private bool m_isComplete = false;
	private int m_targetScore = 0;
	private float m_scoreTimeCount;

	void Update()
	{
		if (!m_isStart || m_isComplete)
			return;

		m_scoreTimeCount += Time.deltaTime * m_countUpSpeed;
		m_textScore.text = Mathf.FloorToInt(m_scoreTimeCount).ToString("0");

		if (m_scoreTimeCount > m_targetScore)
		{
			m_textScore.text = m_targetScore.ToString("0");
			m_isComplete = true;
		}
	}

	public void StartCountUp(int _targetScore)
	{
		m_scoreTimeCount = 0.0f;
		m_targetScore = _targetScore;
		m_isStart = true;
	}
}
