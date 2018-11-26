using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingWindow : MonoBehaviour
{
	[SerializeField]
	private RankingContent m_rankingContentPref;
	[SerializeField]
	private Transform m_rankingView;
	[SerializeField]
	private GameObject m_loadingScreen;

	private bool m_isSendComplete = false;
	private bool m_isLoadComplete = false;
	private List<RankingData> m_rankingDataList = new List<RankingData>();

	public void GetRankingStart()
	{
		var jsonData = SaveManager.Instance.SaveDataToJson(SaveManager.Instance.saveData.rankingData);
		StartCoroutine(LeaderboardManager.Instance.SendScore
					   (SaveManager.Instance.saveData.rankingData.rankingName,
						SaveManager.Instance.saveData.rankingData.score,
						jsonData, () =>
		{
			ShowRanking();
		}));
	}

	void ShowRanking()
	{
		m_loadingScreen.SetActive(true);
		StartCoroutine(LeaderboardManager.Instance.GetScoreList(30, (score) =>
	   {
		   for (int i = 0; i < score.results.Count; i++)
		   {
			   if (score.results[i].score <= 0)
				   continue;

			   var rd = SaveManager.Instance.JsonToSaveData(score.results[i].jsonData);
			   var rc = Instantiate(m_rankingContentPref, m_rankingView);
			   rc.SetRankingData(i + 1, rd, rd.objectId == LeaderboardManager.Instance.saveObjectId);
		   }
		   m_loadingScreen.SetActive(false);
	   }));



		//m_rankingDataList.Sort((a, b) => b.score - a.score);

		//int myIdx = -1;
		//for (int i = 0; i < m_rankingDataList.Count; i++)
		//{
		//	var c = m_rankingDataList[i];
		//	var rc = Instantiate(m_rankingContentPref, m_rankingView);
		//	rc.SetRankingData(i + 1, m_rankingDataList[i]);

		//	//if (c.objectId == LeaderboardManager.Instance.objectID)
		//	//myIdx = i;
		//}
	}
}
