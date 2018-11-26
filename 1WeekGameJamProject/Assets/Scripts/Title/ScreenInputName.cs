using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScreenInputName : MonoBehaviour
{
	[SerializeField]
	private TMP_InputField m_inputText;

	public void OnButtonDownStart()
	{
		var realName = m_inputText.text.Replace('\n', ' ');

		if (realName == "")
		{
			return;
		}

		SaveManager.Instance.saveData.userData.playerName = realName;
		SaveManager.Instance.saveData.isChangeData = true;
		SaveManager.Instance.Save();
		var jsonData = SaveManager.Instance.SaveDataToJson(SaveManager.Instance.saveData.rankingData);
		StartCoroutine(LeaderboardManager.Instance.SendScoreUncheck(SaveManager.Instance.saveData.userData.playerName, 0, jsonData));
		SceneTitle.Instance.ChangeScreen(SceneTitle.ScreenType.IsPlayTutorial);
	}

	public void OnInputTextChange()
	{
		SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.Cursor);
	}
}
