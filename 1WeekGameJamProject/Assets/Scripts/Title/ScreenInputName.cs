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

		//ここで重複しているかの判定
		if (realName == "")
		{
			return;
		}

		SaveManager.Instance.saveData.userData.playerName = realName;
		SaveManager.Instance.saveData.isChangeData = true;
		SaveManager.Instance.Save();
		TransitionManager.Instance.LoadScene(SceneName.Main);
	}
}
