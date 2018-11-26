using NCMBRest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(NCMBRestController))]
public class LeaderboardManager : LightGive.SingletonMonoBehaviour<LeaderboardManager>
{
	private NCMBRestController ncmbRestController;

	private static readonly string PLAYERNAME = "PlayerName";
	private static readonly string OBJECT_ID = "ObjectId";
	private static readonly string HIGH_SCORE = "HighScore";
	private static readonly string DATASTORE_CLASSNAME = "Leaderboard"; //スコアを保存するデータストア名//

	public string saveObjectId { get { return PlayerPrefs.GetString(OBJECT_ID, ""); } }

	protected override void Awake()
	{
		isDontDestroy = true;
		base.Awake();
		ncmbRestController = GetComponent<NCMBRestController>();
	}

	public IEnumerator SendScore(string _playerName, int _score, string _jsonData, UnityAction _act = null)
	{
		//過去のスコアがあるか//
		if (PlayerPrefs.HasKey(OBJECT_ID))
		{
			//そのスコアはハイスコアか//
			if (_score > PlayerPrefs.GetInt(HIGH_SCORE))
			{
				//レコードの更新//
				yield return PutScore(_playerName, _score, _jsonData, PlayerPrefs.GetString(OBJECT_ID));
				//ローカルのハイスコアを更新//
				PlayerPrefs.SetInt(HIGH_SCORE, _score);
				PlayerPrefs.SetString(PLAYERNAME, _playerName);

				if (_act != null)
					_act.Invoke();

				yield break;
			}
			else
			{
				Debug.Log("ハイスコアが更新されていないため、スコアを送信しませんでした。");
				if (_act != null)
					_act.Invoke();

				yield break;
			}
		}

		//チェックせずに送る
		yield return SendScoreUncheck(_playerName, _score, _jsonData);

		if (_act != null)
		{
			_act.Invoke();
		}
	}

	public IEnumerator SendScoreUncheck(string _playerName, int _score, string _jsonData)
	{
		//レコードの新規作成//
		IEnumerator postScoreCoroutine = PostScore(_playerName, _score, _jsonData);

		yield return postScoreCoroutine;

		string objectId = (string)postScoreCoroutine.Current;

		PlayerPrefs.SetString(OBJECT_ID, objectId);//ObjectIdを保存//
		PlayerPrefs.SetInt(HIGH_SCORE, _score);//ローカルのハイスコアを保存//
		PlayerPrefs.SetString(PLAYERNAME, _playerName);//プレイヤーネームを保存 名前を変えたときのチェック用//
		SaveManager.Instance.saveData.rankingData.objectId = objectId;
	}

	private IEnumerator PostScore(string _playerName, int _score, string _jsonData)
	{
		Debug.Log(_playerName + "のスコア" + _score + "を新規投稿します。");

		ScoreData scoreData = new ScoreData(_playerName, _score, _jsonData);
		NCMBDataStoreParamSet paramSet = new NCMBDataStoreParamSet(scoreData);

		IEnumerator coroutine = ncmbRestController.Call(NCMBRestController.RequestType.POST, "classes/" + DATASTORE_CLASSNAME, paramSet);

		yield return StartCoroutine(coroutine);

		JsonUtility.FromJsonOverwrite((string)coroutine.Current, paramSet);

		yield return paramSet.objectId;
	}

	private IEnumerator PutScore(string _playerName, int _score, string _jsonData, string objectId)
	{
		string formerPlayerName = PlayerPrefs.GetString(PLAYERNAME);

		if (formerPlayerName != _playerName)
		{
			Debug.Log("プレイヤー名が " + formerPlayerName + " から " + _playerName + " に変更されました");
			PlayerPrefs.SetString(PLAYERNAME, _playerName);
		}

		Debug.Log(_playerName + "のスコア" + _score + "を更新します。レコードのID：" + objectId);

		ScoreData scoreData = new ScoreData(_playerName, _score, _jsonData);
		NCMBDataStoreParamSet paramSet = new NCMBDataStoreParamSet(scoreData);

		IEnumerator coroutine = ncmbRestController.Call(
			NCMBRestController.RequestType.PUT, "classes/" + DATASTORE_CLASSNAME + "/" + objectId, paramSet,
			(erroCode) =>
			{
				if (erroCode == 404)
				{
					Debug.Log("レコードID：" + objectId + "が見つからなかったため、新規レコードを作成します");
					StartCoroutine(SendScoreUncheck(_playerName, _score, _jsonData));
				}
			}

			);

		yield return StartCoroutine(coroutine);

		JsonUtility.FromJsonOverwrite((string)coroutine.Current, paramSet);

		yield return paramSet.objectId;
	}

	public IEnumerator GetScoreList(int num, UnityAction<ScoreDatas> callback)
	{
		Debug.Log("Get Data");
		NCMBDataStoreParamSet paramSet = new NCMBDataStoreParamSet();
		paramSet.Limit = num;
		paramSet.SortColumn = "-score";

		IEnumerator coroutine = ncmbRestController.Call(NCMBRestController.RequestType.GET, "classes/" + DATASTORE_CLASSNAME, paramSet);

		yield return StartCoroutine(coroutine);

		string jsonStr = (string)coroutine.Current;

		//取得したjsonをScoreDatasとして展開//
		ScoreDatas scores = JsonUtility.FromJson<ScoreDatas>(jsonStr);

		if (scores.results.Count == 0)
		{
			Debug.Log("no data");
		}

		callback(scores);
	}

	public IEnumerator GetScoreListByStr(int num, UnityAction<string> callback)
	{
		yield return GetScoreList(num, (scores) =>
		{
			string str = string.Empty;

			int i = 1;

			foreach (ScoreData s in scores.results)
			{
				str += i + ": " + s.playerName + ": " + s.score.ToString() + "\n";
				i++;
			}

			callback(str);
		});
	}

	public void ClearLocalData()
	{
		PlayerPrefs.DeleteKey(PLAYERNAME);
		PlayerPrefs.DeleteKey(OBJECT_ID);
		PlayerPrefs.DeleteKey(HIGH_SCORE);
		Debug.Log("ローカルのハイスコアとObjectIdが削除されました");
	}

	[Serializable]
	public class ScoreDatas
	{
		public List<ScoreData> results;
	}

	[Serializable]
	public class ScoreData
	{
		public ScoreData(string playerName, int score, string _jsonData)
		{
			this.playerName = playerName;
			this.score = score;
			this.jsonData = _jsonData;
		}

		public string playerName;
		public string jsonData;
		public int score;
	}
}