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
	public SlimeType slimeType;
	public string playerName;
	public int highScore;
	public int totalKill;
	public int clearCount;
	public bool[] isRelease;

	public UserData()
	{
		isRelease = new bool[(int)SlimeType.Max]
		{
			true,
			false,
			false,
			false,
			false
		};

		highScore = 0;
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
	public string objectId;
	public string rankingName;
	public int arrivalStage;
	public int score;
	public int slimeNum;
	public SlimeType slimeType;
	public PlayerStatus playerStatus;

	public RankingData()
	{
		objectId = "";
		playerStatus = new PlayerStatus(false);
		rankingName = "名無しのごんべえ";
		slimeType = SlimeType.Normal;
		arrivalStage = 1;
		slimeNum = 0;
		score = 0;
	}
}