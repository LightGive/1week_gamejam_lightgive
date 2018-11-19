using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDebug : MonoBehaviour
{
	void Start()
	{

	}

	void Update()
	{

	}

	public void OnButtonDownCheckRanking()
	{
		StartCoroutine(LeaderboardManager.Instance.GetRankingData((dataList) =>
		{
			for (int i = 0; i < dataList.results.Count; i++)
			{
				var data = SaveManager.Instance.JsonToSaveData(dataList.results[i].json);
				Debug.Log(data.userData.score.ToString());
			}
		}));
	}

	public void OnButtonDownAddRandomData(int _createDataCount)
	{
		for (int i = 0; i < _createDataCount; i++)
		{
			var randomSaveData = new SaveData();
			randomSaveData.userData.score = Random.Range(0, 10000);
			var json = SaveManager.Instance.SaveDataToJson(randomSaveData);
			StartCoroutine(LeaderboardManager.Instance.SendSaveDataUncheck(json));
		}
	}
}
