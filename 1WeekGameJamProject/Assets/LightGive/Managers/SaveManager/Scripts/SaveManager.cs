using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : LightGive.SingletonMonoBehaviour<SaveManager>
{
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
		string jsonText = JsonUtility.ToJson(m_saveData);
		File.WriteAllText(GetSaveFilePath(_saveSlot), StringEncryptor.Encrypt(jsonText));

		if (m_isCheckLog) { Debug.Log(_saveSlot.ToString("0") + "番のスロットに現在のデータを保存しました。"); }
	}

	public void Load(int _saveSlot = 0)
	{
		m_saveData = JsonUtility.FromJson<SaveData>(GetJson(_saveSlot));

		if (m_isCheckLog) { Debug.Log(_saveSlot.ToString("0") + "番のスロットからデータをロードしました。"); }
	}


	public void Delete(int _saveSlot = 0)
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

	public void DeleteAllSlot()
	{
	}

	public string GetJson(int _saveSlot = 0)
	{
		string filePath = GetSaveFilePath(_saveSlot);
		string jsonText = "";

		if (File.Exists(filePath))
		{
			jsonText = StringEncryptor.Decrypt(File.ReadAllText(filePath));
		}
		else
		{
			jsonText = JsonUtility.ToJson(new SaveData());
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