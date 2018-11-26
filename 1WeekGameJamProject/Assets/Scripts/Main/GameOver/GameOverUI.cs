using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
	[SerializeField]
	private RankingWindow m_rankingWindow;
	[SerializeField]
	private TextMeshProUGUI m_textStageNo;
	[SerializeField]
	private TextMeshProUGUI m_textSlimeLevel;
	[SerializeField]
	private TextMeshProUGUI m_textSlimeNum;
	[SerializeField]
	private TextMeshProUGUI m_textScore;
	[SerializeField]
	private TextMeshProUGUI m_textHighScore;

	public void ShowGameOverUI(SlimeType _type, int _stageNo, int _slimeNum, int _score)
	{

		if (_score > SaveManager.Instance.saveData.rankingData.score)
		{
			SaveManager.Instance.saveData.rankingData.objectId = LeaderboardManager.Instance.saveObjectId;
			SaveManager.Instance.saveData.rankingData.arrivalStage = _stageNo;
			SaveManager.Instance.saveData.rankingData.slimeType = _type;
			SaveManager.Instance.saveData.rankingData.slimeNum = _slimeNum;
			SaveManager.Instance.saveData.rankingData.rankingName = SaveManager.Instance.saveData.userData.playerName;
			SaveManager.Instance.saveData.rankingData.score = _score;
			SaveManager.Instance.saveData.rankingData.playerStatus = SceneMain.Instance.mainSlime.playerStatus;
			SaveManager.Instance.Save();
		}

		m_textStageNo.text = _stageNo.ToString("0");
		m_textSlimeLevel.text = SceneMain.Instance.mainSlime.playerStatus.level.ToString("0");
		m_textSlimeNum.text = _slimeNum.ToString("0");
		m_textScore.text = _score.ToString("0");
		m_textHighScore.text = SaveManager.Instance.saveData.rankingData.score.ToString("0");

		SimpleSoundManager.Instance.PlayBGM(SoundNameBGM.GameOver, true, 1.0f, 3.0f);
	}

	public void ShowRanking()
	{
		m_rankingWindow.GetRankingStart();
	}

	public void OnButtonDownRetry()
	{
		GameManager.Instance.ResetStatus();
		TransitionManager.Instance.LoadScene(SceneName.Main);
	}

	public void OnButtonDownTitle()
	{
		TransitionManager.Instance.LoadScene(SceneName.Title);
	}
}
