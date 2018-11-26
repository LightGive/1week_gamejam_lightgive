using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDebug : MonoBehaviour
{
	[SerializeField]
	private string[] randomPlayerName;


	void Start()
	{

	}

	void Update()
	{

	}

	public void OnButtonDownCheckRanking()
	{
		//StartCoroutine(LeaderboardManager.Instance.GetJsonData((dataList) =>
		//{
		//	for (int i = 0; i < dataList.results.Count; i++)
		//	{
		//		var data = SaveManager.Instance.JsonToSaveData(dataList.results[i].json);
		//	}
		//}));
	}

	public void OnButtonDownDeletePlayerPrefs()
	{
		PlayerPrefs.DeleteAll();
	}

	public void OnButtonDownAddRandomData(int _createDataCount)
	{
		for (int i = 0; i < _createDataCount; i++)
		{
			var randomRankingData = new RankingData();
			randomRankingData.arrivalStage = Random.Range(0, 4);
			randomRankingData.rankingName = randomPlayerName[Random.Range(0, randomPlayerName.Length)];
			randomRankingData.playerStatus.level = Random.Range(1, 10) * 10;
			randomRankingData.slimeNum = Random.Range(0, 100);
			randomRankingData.slimeType = (SlimeType)Random.Range(0, (int)SlimeType.Max);
			randomRankingData.score = GameManager.Instance.CalcScore(randomRankingData.arrivalStage, randomRankingData.playerStatus.level, randomRankingData.slimeNum);

			var json = SaveManager.Instance.SaveDataToJson(randomRankingData);
			StartCoroutine(LeaderboardManager.Instance.SendScoreUncheck(randomRankingData.rankingName, randomRankingData.score, json));
		}
	}
}
