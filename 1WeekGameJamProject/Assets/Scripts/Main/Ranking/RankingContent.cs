using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingContent : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_textRank;
	[SerializeField]
	private TextMeshProUGUI m_textPlayerName;
	[SerializeField]
	private TextMeshProUGUI m_textScore;

	public void SetRankingData(int _rank, RankingData _data)
	{
		m_textPlayerName.text = _data.rankingName;
		m_textScore.text = _data.score.ToString("0");
		m_textRank.text = _rank.ToString("0");
	}
}
