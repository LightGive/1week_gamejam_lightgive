using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData : ISerializationCallbackReceiver
{
	//セーブデータが一度でも変更されたかどうか
	public bool isChangeData;

	public SettingData settingData;
	public RankingData rankingData;
	public UserData userData;

	/// <summary>
	/// コンストラクタ。データを初期化した時の初期値になる
	/// </summary>
	public SaveData()
	{
		//ユーザーデータ初期化
		userData = new UserData();
		settingData = new SettingData();
		rankingData = new RankingData();
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
/// ユーザーデータ
/// </summary>
[System.Serializable]
public class UserData
{
	public string playerName;
	public int totalKill;
	public int clearCount;

	public UserData()
	{
		totalKill = 0;
		clearCount = 0;
		playerName = "名無しさん";
	}
}

/// <summary>
/// 設定のデータ
/// </summary>
[System.Serializable]
public class SettingData
{
	public SettingVelocityGuide settingVelocityGuide;

	public SettingData()
	{
		settingVelocityGuide = SettingVelocityGuide.Arrow;
	}
}

/// <summary>
/// ランキングのデータ
/// </summary>
[System.Serializable]
public class RankingData
{
	public string rankingName;
	public int arrivalStage;
	public int slimeLevel;
	public int score = 0;
	public SlimeType slimeType;

	public RankingData()
	{
		rankingName = "名無しのごんべえ";
		arrivalStage = 1;
		slimeLevel = 10;
		slimeType = SlimeType.Normal;
		score = 0;
	}
}