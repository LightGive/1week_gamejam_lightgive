using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightGive;

public class TutorialManager : SingletonMonoBehaviour<TutorialManager>
{
	[SerializeField]
	private TutorialTalk m_tutorialTalk;

	private bool m_isFirstKill;

	void Start()
	{
		m_isFirstKill = true;
	}

	void Update()
	{

	}

	public void EnemyDead()
	{
		if (m_isFirstKill)
		{
			m_isFirstKill = false;
			m_tutorialTalk.ShowWindow();
		}
	}

	public void Success10Lvel()
	{
		SceneMain.Instance.isGameStart = false;
		m_tutorialTalk.talkNo = 2;
		m_tutorialTalk.ShowWindow();
	}
	public void Failed10Lvel()
	{
		SceneMain.Instance.isGameStart = false;
		m_tutorialTalk.talkNo = 1;
		m_tutorialTalk.ShowWindow();

	}
}
