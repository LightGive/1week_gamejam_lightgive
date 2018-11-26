using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenStart : MonoBehaviour
{
	/// <summary>
	/// はじめるのボタンを押した
	/// </summary>
	public void OnButtonDownStoryMode()
	{
		if (!SaveManager.Instance.saveData.isChangeData)
		{
			SceneTitle.Instance.ChangeScreen(SceneTitle.ScreenType.InputName);
		}
		else
		{
			TransitionManager.Instance.LoadScene(SceneName.Main);
			GameManager.Instance.ResetStatus();
			GameManager.Instance.stageNo = 1;
		}
	}

	/// <summary>
	/// 設定のボタンを押した
	/// </summary>
	public void OnButtonDownSetting()
	{
		SceneTitle.Instance.ChangeScreen(SceneTitle.ScreenType.Setting);

	}
}
