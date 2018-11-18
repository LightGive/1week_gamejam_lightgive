using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : ISerializationCallbackReceiver
{
	public int diff;
	public int atk;
	public string name;

	/// <summary>
	/// コンストラクタ。データを初期化した時の初期値になる
	/// </summary>
	public SaveData()
	{
		diff = 5;
		atk = 5;
		name = "名無しさん";
	}

	public void OnAfterDeserialize()
	{
	}

	public void OnBeforeSerialize()
	{
	}
}
