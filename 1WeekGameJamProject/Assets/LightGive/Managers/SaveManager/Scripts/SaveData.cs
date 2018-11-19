using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : ISerializationCallbackReceiver
{
	//セーブデータが一度でも変更されたかどうか
	public bool isChangeData;

	public UserData userData;

	/// <summary>
	/// コンストラクタ。データを初期化した時の初期値になる
	/// </summary>
	public SaveData()
	{
		//ユーザーデータ初期化
		userData = new UserData();
		isChangeData = false;
	}

	public void OnAfterDeserialize()
	{
	}

	public void OnBeforeSerialize()
	{
	}
}

/// <summary>
/// ユーザーから取得するデータ
/// </summary>
[System.Serializable]
public class UserData
{
	public string playerName;
	public int score;

	public UserData()
	{
		playerName = "名無しさん";
	}
}