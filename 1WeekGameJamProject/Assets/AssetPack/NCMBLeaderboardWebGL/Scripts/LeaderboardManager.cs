using NCMBRest;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

[RequireComponent(typeof(NCMBRestController))]
public class LeaderboardManager : LightGive.SingletonMonoBehaviour<LeaderboardManager>
{
	private const string SaveKeyObjectId = "ObjectId";
	private const string DataStoreClassName = "Leaderboard";

	private NCMBRestController ncmbRestController;

	protected override void Awake()
	{
		isDontDestroy = true;
		base.Awake();
		ncmbRestController = GetComponent<NCMBRestController>();
	}

	/// <summary>
	/// セーブデータを送信する。
	/// 新規で送信するかレコード編集か判定する
	/// </summary>
	/// <returns>The save data.</returns>
	/// <param name="_jsonData">Json data.</param>
	public IEnumerator SendSaveData(string _jsonData, UnityAction _callback = null)
	{
		//スコアを既に送っているかの判定
		if (PlayerPrefs.HasKey(SaveKeyObjectId))
		{
			//既に送っているのでレコード編集
			yield return PutSaveData(_jsonData, PlayerPrefs.GetString(SaveKeyObjectId));
			if (_callback != null)
				_callback.Invoke();
		}
		else
		{
			//まだ送ってないので新規レコード登録
			yield return SendSaveDataUncheck(_jsonData);
			if (_callback != null)
				_callback.Invoke();
		}
	}


	/// <summary>
	/// レコードの有る無しに関わらずデータを新規で登録する
	/// </summary>
	/// <returns>The score uncheck.</returns>
	/// <param name="_jsonData">Json data.</param>
	public IEnumerator SendSaveDataUncheck(string _jsonData)
	{
		Debug.Log(_jsonData);

		//レコードの新規作成//
		IEnumerator postScoreCoroutine = PostSaveData(_jsonData);
		yield return postScoreCoroutine;
		var objectId = (string)postScoreCoroutine.Current;

		//ObjectIdを保存
		PlayerPrefs.SetString(SaveKeyObjectId, objectId);
	}

	/// <summary>
	/// 既送信してあるセーブデータのレコードを取得する
	/// </summary>
	/// <returns>The save data.</returns>
	/// <param name="_jsonData">Json data.</param>
	/// <param name="_objectId">Object identifier.</param>
	private IEnumerator PutSaveData(string _jsonData, string _objectId)
	{
		NCMBDataStoreParamSet paramSet = new NCMBDataStoreParamSet(new JsonData(_jsonData));

		IEnumerator coroutine = ncmbRestController.Call(
			NCMBRestController.RequestType.PUT, "classes/" + DataStoreClassName + "/" + _objectId, paramSet,
			(erroCode) =>
			{
				if (erroCode == 404)
				{
					Debug.Log("レコードID：" + _objectId + "が見つからなかったため、新規レコードを作成します");
					StartCoroutine(SendSaveDataUncheck(_jsonData));
				}
			});

		yield return StartCoroutine(coroutine);
		JsonUtility.FromJsonOverwrite((string)coroutine.Current, paramSet);
		yield return paramSet.objectId;
	}

	/// <summary>
	/// セーブデータのレコードを新規登録する
	/// </summary>
	/// <returns>The save data.</returns>
	/// <param name="_jsonData">Json data.</param>
	private IEnumerator PostSaveData(string _jsonData)
	{
		NCMBDataStoreParamSet paramSet = new NCMBDataStoreParamSet(new JsonData(_jsonData));
		IEnumerator coroutine = ncmbRestController.Call(NCMBRestController.RequestType.POST, "classes/" + DataStoreClassName, paramSet);
		yield return StartCoroutine(coroutine);
		JsonUtility.FromJsonOverwrite((string)coroutine.Current, paramSet);
		yield return paramSet.objectId;
	}


	public IEnumerator GetJsonData(UnityAction<JsonDataList> callback)
	{
		NCMBDataStoreParamSet paramSet = new NCMBDataStoreParamSet();
		paramSet.Limit = int.MaxValue;

		IEnumerator coroutine = ncmbRestController.Call(NCMBRestController.RequestType.GET, "classes/" + DataStoreClassName, paramSet);
		yield return StartCoroutine(coroutine);
		string jsonStr = (string)coroutine.Current;
		JsonDataList saveDates = JsonUtility.FromJson<JsonDataList>(jsonStr);
		if (saveDates.results.Count == 0)
		{
			Debug.Log("データがありません");
		}
		else
		{
			Debug.Log(saveDates.results.Count.ToString() + "件データを取得しました");
		}
		callback(saveDates);
	}

	[System.Serializable]
	public class JsonDataList
	{
		public List<JsonData> results;
	}

	[System.Serializable]
	public class JsonData
	{
		public string json;
		public JsonData(string _json)
		{
			json = _json;
		}
	}
}