
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightGive;

public class SceneMain : SingletonMonoBehaviour<SceneMain>
{
	[SerializeField]
	private MainCamera m_mainCamera;
	[SerializeField]
	private UIController m_uiController;
	[SerializeField]
	private GameOverUI m_gameOverUI;
	[SerializeField]
	private Slime m_slime;
	[SerializeField]
	private float m_damageTime = 10.0f;


	private bool m_isGameStart = false;
	private bool m_isGameOver = false;
	private float m_timeCnt;

	public MainCamera mainCamera
	{
		get { return m_mainCamera; }
		set { m_mainCamera = value; }
	}
	public Vector3 randomPos
	{
		get { return new Vector3(Random.Range(-140, 140), Random.Range(-40, 40)); }
	}

	public bool isGameOver { get { return m_isGameOver; } }
	public bool isGameStart { get { return m_isGameStart; } }
	public UIController uIController { get { return m_uiController; } }
	public Slime slime { get { return m_slime; } }


	protected override void Awake()
	{
		isDontDestroy = false;
		base.Awake();

	}

	private void Start()
	{
		SimpleSoundManager.Instance.PlayBGM(SoundNameBGM.Main2);
	}

	private void Update()
	{
		if (m_isGameOver)
			return;

		if (!m_isGameStart)
		{
			CheckGameStart();
			return;
		}

		TimeCount();
	}


	void CheckGameStart()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GameStart();
		}
	}

	void TimeCount()
	{
		m_timeCnt = Mathf.Clamp(m_timeCnt - Time.deltaTime, 0.0f, m_damageTime);
		m_uiController.SetTime(m_damageTime, m_timeCnt);

		if (m_timeCnt <= 0.0f)
		{
			TransitionManager.Instance.Flash(0.0f, 0.2f);
			DamageCheck();
		}
	}

	/// <summary>
	/// ダメージを受けるかのチェック
	/// </summary>
	void DamageCheck()
	{
		if (m_slime.level % 10 == 0)
		{
			m_timeCnt = m_damageTime;
		}
		else
		{
			GameOver();
		}
	}

	public void GameOver()
	{
		SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.Thunder);
		SimpleSoundManager.Instance.StopBGM();
		m_gameOverUI.gameObject.SetActive(true);
		m_isGameOver = true;
	}

	public void GameStart()
	{
		m_timeCnt = m_damageTime;
		m_isGameOver = false;
		m_isGameStart = true;
	}
}
