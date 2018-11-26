using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTalk : MonoBehaviour
{
	[SerializeField]
	private List<TalkOrder> m_talkInfoList;
	[SerializeField]
	private StoryText m_talkText;

	private int m_textNo = 0;
	private int m_talkNo = 0;
	public int textNo { get { return m_textNo; } set { m_textNo = value; } }
	public int talkNo { get { return m_talkNo; } set { m_talkNo = value; } }

	void Start()
	{
		OnCheckNextText();
	}

	void Update()
	{

	}

	public void OnCheckNextText()
	{
		if (!m_talkText.isDisplayComplete)
			return;
		if (m_textNo >= m_talkInfoList[m_talkNo].talkInfo.Count)
		{
			//終了時のイベントアクションがあれば実行してから終わる
			if (m_talkInfoList[m_talkNo].completeEvent != null)
				m_talkInfoList[m_talkNo].completeEvent.Invoke();

			CloseWindow();
			return;
		}

		if (m_talkInfoList[m_talkNo].talkInfo[m_textNo].eventAct != null)
		{
			m_talkInfoList[m_talkNo].talkInfo[m_textNo].eventAct.Invoke();
		}

		m_talkText.SetText(m_talkInfoList[m_talkNo].talkInfo[m_textNo].displayText);
		m_talkText.isFast = false;
		m_textNo++;
	}

	public void ShowWindow()
	{
		m_talkNo++;
		m_textNo = 0;

		//開始時のイベントアクションがあれば実行してから終わる
		if (m_talkInfoList[m_talkNo].startEvent != null)
			m_talkInfoList[m_talkNo].startEvent.Invoke();

		OnCheckNextText();
		this.gameObject.SetActive(true);
	}

	void CloseWindow()
	{
		this.gameObject.SetActive(false);
	}

	public void OnPlayBGM()
	{
		SimpleSoundManager.Instance.PlayBGM(SoundNameBGM.Tutorial);
	}

	public void OnStopBGM()
	{
		SimpleSoundManager.Instance.StopBGM();
	}

	public void CreateEnemy()
	{
		SceneMain.Instance.GenerateEnemy();
	}

	public void OnChangeFixedSlime(bool _isFixed)
	{
		SceneMain.Instance.isFixedSlime = _isFixed;
	}

	public void OnShowStatus()
	{
		SceneMain.Instance.uIController.statusUI.gameObject.SetActive(true);
	}

	public void OnShowExpSlider()
	{
		SceneMain.Instance.uIController.sliderExp.gameObject.SetActive(true);
	}

	public void OnShowTopUI()
	{
		SceneMain.Instance.uIController.topUI.gameObject.SetActive(true);
	}

	public void OnStartGame()
	{
		SceneMain.Instance.TutorialGameStart();
	}

	public void OnTutorialComplete()
	{
		GameManager.Instance.stageNo = 1;
		GameManager.Instance.ResetStatus();
		TransitionManager.Instance.LoadScene(SceneName.Main);
	}

	public void OnTutorialReStart()
	{
		SceneMain.Instance.SetTime(30);
		SceneMain.Instance.mainSlime.playerStatus = new PlayerStatus(true);
		SceneMain.Instance.uIController.sliderExp.SetLevel();
	}

	public void OnTutorialReStartToStart()
	{
		SceneMain.Instance.TutorialGameStart();
	}

	[System.Serializable]
	public class TalkOrder
	{
		public UnityEvent startEvent;
		public List<TalkInfo> talkInfo;
		public UnityEvent completeEvent;
	}

	[System.Serializable]
	public class TalkInfo
	{
		[Multiline]
		public string displayText;
		public UnityEvent eventAct;
	}
}