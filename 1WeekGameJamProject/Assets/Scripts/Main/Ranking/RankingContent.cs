using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RankingContent : MonoBehaviour
{
	[SerializeField]
	private Image m_background;
	[SerializeField]
	private TextMeshProUGUI m_textRank;
	[SerializeField]
	private TextMeshProUGUI m_textPlayerName;
	[SerializeField]
	private TextMeshProUGUI m_textScore;
	[SerializeField]
	private TextMeshProUGUI m_textSlimeLevel;

	public void SetRankingData(int _rank, RankingData _data, bool _isMyContent = false)
	{
		m_textPlayerName.text = _data.rankingName;
		m_textScore.text = _data.score.ToString("0");
		m_textRank.text = _rank.ToString("0");
		m_textSlimeLevel.text = "Lv." + _data.playerStatus.level.ToString("0");
		m_background.gameObject.SetActive(_isMyContent);
	}
}
