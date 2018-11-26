using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenIsPlayingTutorial : MonoBehaviour
{
	public void OnButtonDownYes()
	{
		GameManager.Instance.stageNo = 0;
		GameStart();
	}

	public void OnButtonDownNo()
	{
		GameManager.Instance.stageNo = 1;
		GameStart();
	}

	void GameStart()
	{
		GameManager.Instance.ResetStatus();
		TransitionManager.Instance.LoadScene(SceneName.Main);
	}
}
