using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneTitle : LightGive.SingletonMonoBehaviour<SceneTitle>
{
	[SerializeField]
	private GameObject[] m_screens;

	[SerializeField]
	private GameObject[] m_slimes;

	private bool m_isChangeScreen;
	private ScreenType m_preScreenType = ScreenType.Start;
	private ScreenType m_nowScreenType = ScreenType.Start;

	protected override void Awake()
	{
		base.Awake();
		m_isChangeScreen = false;
		//ChangeScreen(ScreenType.Start);

		//m_buttonContinue.SetActive(SaveManager.Instance.saveData.isChangeData);
	}

	private void Start()
	{
		SimpleSoundManager.Instance.PlayBGM(SoundNameBGM.Title);

		for (int i = 0; i < SaveManager.Instance.saveData.userData.isRelease.Length; i++)
		{
			m_slimes[i].SetActive(SaveManager.Instance.saveData.userData.isRelease[i]);
		}
	}

	/// <summary>
	/// 戻るボタンを押した時
	/// </summary>
	public void OnButtonDownBack()
	{
		ChangeScreen(m_preScreenType);
	}



	/// <summary>
	/// タイトルシーン内でシーン変更
	/// </summary>
	/// <param name="_screenType">Screen type.</param>
	public void ChangeScreen(ScreenType _screenType)
	{
		if (m_isChangeScreen)
			return;

		m_isChangeScreen = true;

		TransitionManager.Instance.StartTransitonEffect(0.5f, TransitionManager.EffectType.Custom, Color.black, () =>
		  {
			  m_preScreenType = m_nowScreenType;
			  m_nowScreenType = _screenType;
			  for (int i = 0; i < m_screens.Length; i++)
			  {
				  m_screens[i].SetActive(i == (int)_screenType);
			  }
			  m_isChangeScreen = false;
		  });
	}

	public enum ScreenType
	{
		Start = 0,
		InputName = 1,
		IsPlayTutorial = 2,
		Setting = 3,
	}
}