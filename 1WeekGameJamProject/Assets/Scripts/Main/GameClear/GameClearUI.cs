using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameClearUI : MonoBehaviour
{
	[SerializeField]
	private StatusUI m_statusUI;
	[SerializeField]
	private TextMeshProUGUI m_textStatusPoint;

	private int m_statusPoint;

	public void ShowStageClear()
	{
		SimpleSoundManager.Instance.StopBGM();
		m_statusPoint = Mathf.FloorToInt((SceneMain.Instance.mainSlime.level - SceneMain.Instance.preLevel) / 10);
		m_textStatusPoint.text = m_statusPoint.ToString("0") + "P";

		SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.StageClear);
		m_statusUI.SetStatus(SceneMain.Instance.mainSlime.playerStatus);
		this.gameObject.SetActive(true);
	}

	public void OnButtonDownNextStage()
	{
		GameManager.Instance.stageNo++;
		TransitionManager.Instance.LoadScene(SceneName.Main);
	}

	public void OnButtonDownStatusPoint(int _pointNo)
	{
		if (m_statusPoint <= 0)
			return;

		switch (_pointNo)
		{
			case 0:
				SceneMain.Instance.mainSlime.playerStatus.atk++;
				break;
			case 1:
				SceneMain.Instance.mainSlime.playerStatus.def++;
				break;
			case 2:
				SceneMain.Instance.mainSlime.playerStatus.spd++;
				break;
		}

		m_statusPoint--;
		m_textStatusPoint.text = m_statusPoint.ToString("0") + "P";
		m_statusUI.SetStatus(SceneMain.Instance.mainSlime.playerStatus);
	}
}
