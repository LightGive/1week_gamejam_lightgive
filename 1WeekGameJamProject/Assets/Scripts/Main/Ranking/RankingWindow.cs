using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingWindow : MonoBehaviour
{
	[SerializeField]
	private RankingContent m_rankingContentPref;
	[SerializeField]
	private Transform m_rankingView;

	private bool m_isSendComplete = false;
	private bool m_isLoadComplete = false;
	private List<RankingData> m_rankingDataList = new List<RankingData>();

	public void GetRankingStart()
	{
		var jsonData = SaveManager.Instance.SaveDataToJson(SaveManager.Instance.saveData.rankingData);

		//ランキングを送信する
		StartCoroutine(LeaderboardManager.Instance.SendSaveData(jsonData, () =>
		{
			m_isSendComplete = true;

			//送信に成功した時、ランキングの受信をする
			StartCoroutine(LeaderboardManager.Instance.GetJsonData((result) =>
			{
				for (int i = 0; i < result.results.Count; i++)
				{
					var c = result.results[i].json;
					var d = SaveManager.Instance.JsonToSaveData(c);
					m_rankingDataList.Add(d);
				}

				m_isLoadComplete = result.results.Count > 0;

				if (m_isLoadComplete)
					ShowRanking();

			}));
		}));

	}

	void ShowRanking()
	{
		m_rankingDataList.Sort((a, b) => a.score - b.score);
		for (int i = 0; i < m_rankingDataList.Count; i++)
		{
			var c = Instantiate(m_rankingContentPref, m_rankingView);
			c.SetRankingData(i + 1, m_rankingDataList[i]);
		}
	}
}
