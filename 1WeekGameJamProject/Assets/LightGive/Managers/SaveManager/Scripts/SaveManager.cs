using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SaveManager : LightGive.SingletonMonoBehaviour<SaveManager>
{
	private const string SaveKey = "SaveKey";
	private const string EmptySaveData = "";

	/// <summary>
	/// 一番最初以外は変更不可。保存できるスロット数。
	/// スロット数を多くすると初期化などに時間がかかってしまう。ので触る時は注意。
	/// 0以下は設定しないで下さい。
	/// </summary>
	public const int MaxSlotCount = 5;

	/// <summary>
	/// 起動直後にロードを行うか
	/// </summary>
	[SerializeField]
	private bool m_isAwakeLoad;

	/// <summary>
	/// ログを表示するか
	/// </summary>
	[SerializeField]
	private bool m_isCheckLog;

	private SaveData m_saveData;
	public SaveData saveData
	{
		get
		{
			return m_saveData;
		}

		set
		{
			m_saveData = value;
		}
	}

	protected override void Awake()
	{
		isDontDestroy = true;
		base.Awake();

		m_saveData = new SaveData();
		if (m_isAwakeLoad) { Load(); }
	}


	public void Save(int _saveSlot = 0)
	{
		string jsonText = SaveDataToJson(m_saveData);
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			//WebGLの時はPlayerPrefsを使用
			PlayerPrefs.SetString(SaveKey + _saveSlot.ToString("0"), StringEncryptor.Encrypt(jsonText));
		}
		else
		{
			File.WriteAllText(GetSaveFilePath(_saveSlot), StringEncryptor.Encrypt(jsonText));
		}

		if (m_isCheckLog) { Debug.Log(_saveSlot.ToString("0") + "番のスロットに現在のデータを保存しました。"); }
	}


	public void Load(int _saveSlot = 0)
	{
		m_saveData = JsonToSaveData(GetJson(_saveSlot));
		if (m_isCheckLog) { Debug.Log(_saveSlot.ToString("0") + "番のスロットからデータをロードしました。"); }
	}

	public void Delete(int _saveSlot = 0)
	{
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			PlayerPrefs.DeleteKey(SaveKey + _saveSlot.ToString("0"));
			if (m_isCheckLog) { Debug.Log("<color=red>" + _saveSlot.ToString("0") + "番のスロットのデータを削除しました。</color>"); }
		}
		else
		{
			string filePath = GetSaveFilePath(_saveSlot);
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
				if (m_isCheckLog) { Debug.Log("<color=red>" + _saveSlot.ToString("0") + "番のスロットのデータを削除しました。</color>"); }
			}
			else
			{
				if (m_isCheckLog) { Debug.Log(_saveSlot.ToString("0") + "番のスロットにはデータが存在しませんでした。"); }
			}
		}
	}

	public void DeleteAllSlot()
	{
		for (int i = 0; i < MaxSlotCount; i++)
		{
			Delete(i);
		}
	}

	public SaveData JsonToSaveData(string _jsonData)
	{
		return JsonUtility.FromJson<SaveData>(_jsonData);
	}

	public string SaveDataToJson(SaveData _data)
	{
		return JsonUtility.ToJson(_data);
	}

	public string GetJson(int _saveSlot = 0)
	{
		string jsonText = "";
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			//WebGLの場合はPlayerPrefsを使用する
			jsonText = StringEncryptor.Decrypt(PlayerPrefs.GetString(SaveKey + _saveSlot.ToString("0"), EmptySaveData));
			if (jsonText == EmptySaveData)
			{
				//初期化したデータを入れておく
				jsonText = SaveDataToJson(new SaveData());
			}
		}
		else
		{
			string filePath = GetSaveFilePath(_saveSlot);
			if (File.Exists(filePath))
			{
				jsonText = StringEncryptor.Decrypt(File.ReadAllText(filePath));
			}
			else
			{
				jsonText = JsonUtility.ToJson(new SaveData());
			}
		}

		return jsonText;
	}

	private string GetSaveFilePath(int _saveSlot = 0)
	{
		string filePath = "SaveData";
#if UNITY_EDITOR
		filePath += "_Slot" + _saveSlot.ToString("0") + ".json";
#else
		filePath = Application.persistentDataPath + "/" + filePath + "_Slot" + _saveSlot.ToString("0");
#endif
		return filePath;
	}
}