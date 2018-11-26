using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightGive;
using TMPro;

public class StoryManager : SingletonMonoBehaviour<StoryManager>
{
	[SerializeField]
	private StoryBase[] m_stories;
	[SerializeField]
	private float m_changeDuration;
	[SerializeField]
	private Material m_changeMat;
	[SerializeField]
	private StoryText m_storyText;
	[SerializeField]
	private TextMeshProUGUI m_textName;
	[SerializeField]
	private string m_nextSceneName;

	private bool m_isClose = false;
	private bool m_isChangeScene;
	private int m_textNo = 0;
	private int m_storyNo = 0;
	private int m_nextStoryNo = 0;
	private float m_timeCnt = 0.0f;
	private float m_storyViewTimeCnt = 0.0f;

	private void Start()
	{

		//AudioManager.Instance.PlayBGM(AudioName.BGM_STORY);
		m_textNo = 0;
		m_stories[m_storyNo].gameObject.SetActive(true);
		OnCheckNextText();
	}

	private void Update()
	{
		StoryTransition();
		m_storyViewTimeCnt += Time.deltaTime;
	}

	void StoryTransition()
	{
		if (!m_isChangeScene)
			return;

		if (m_isClose)
		{
			//マスクがかかってだんだん暗くなっていく
			m_timeCnt += Time.deltaTime;
			m_changeMat.SetFloat("_Cutoff", Mathf.Clamp01(m_timeCnt / m_changeDuration));
			if (m_timeCnt >= m_changeDuration)
			{
				//完全にマスクで覆われている
				m_isClose = false;
				m_timeCnt = 0.0f;
				m_stories[m_storyNo].gameObject.SetActive(false);
				m_stories[m_nextStoryNo].gameObject.SetActive(true);
				m_storyNo = m_nextStoryNo;
			}
		}
		else
		{
			//マスクが外れてだんだん見えてくる
			m_timeCnt += Time.deltaTime;
			m_changeMat.SetFloat("_Cutoff", 1.0f - Mathf.Clamp01(m_timeCnt / m_changeDuration));
			if (m_timeCnt >= m_changeDuration)
			{
				m_timeCnt = 0.0f;
				m_isChangeScene = false;
				m_changeMat.SetFloat("_Cutoff", 0.0f);
				//m_storyText.SetText(m_stories[m_storyNo].displayTextInfos[m_textNo].displayText);
				OnCheckNextText();
			}
		}
	}


	public void OnCheckNextText()
	{
		if (!m_storyText.isDisplayComplete || m_isChangeScene)
			return;

		if (TransitionManager.Instance.isSceneTransitionProgress)
			return;


		if (m_textNo >= m_stories[m_storyNo].displayTextInfos.Length)
		{
			NextStory();
			return;
		}

		if (m_stories[m_storyNo].displayTextInfos[m_textNo].eventAct != null)
		{
			m_stories[m_storyNo].displayTextInfos[m_textNo].eventAct.Invoke();
		}

		m_textName.text = m_stories[m_storyNo].displayTextInfos[m_textNo].nameText;
		m_storyText.SetText(m_stories[m_storyNo].displayTextInfos[m_textNo].displayText);
		m_storyText.isFast = false;
		m_textNo++;
	}

	void NextStory()
	{
		m_textNo = 0;
		if ((m_storyNo + 1) >= m_stories.Length)
		{
			TransitionManager.Instance.LoadScene(m_nextSceneName);
			return;
		}

		ChangeStory(m_storyNo + 1);
	}

	void ChangeStory(int _changeStoryNo)
	{
		m_nextStoryNo = _changeStoryNo;
		m_isChangeScene = true;
		m_isClose = true;
	}

	public void OnButtonDownSkip()
	{
		TransitionManager.Instance.LoadScene(m_nextSceneName);
	}
}
