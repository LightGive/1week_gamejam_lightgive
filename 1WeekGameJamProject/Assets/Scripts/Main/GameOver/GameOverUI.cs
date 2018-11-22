using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
	[SerializeField]
	private RankingWindow m_rankingWindow;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			m_rankingWindow.GetRankingStart();
		}
	}

	public void OnTimelineStartCountStage()
	{
	}
}
