
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightGive;

public class SceneMain : SingletonMonoBehaviour<SceneMain>
{
	[SerializeField]
	private StageInfo[] m_stagePref;
	[SerializeField]
	private Slime[] m_slimeObjects;
	[SerializeField]
	private MainCamera m_mainCamera;
	[SerializeField]
	private Transform m_stageAnchor;
	[SerializeField]
	private UIController m_uiController;
	[SerializeField]
	private GameObject m_startWindowObj;
	[SerializeField]
	private TextDisplay m_startWindowTextDisplay;
	[SerializeField]
	private GameOverUI m_gameOverUI;
	[SerializeField]
	private GameClearUI m_gameClearUI;
	[SerializeField]
	private GameObject m_tutorialUI;
	[SerializeField]
	private GameObject m_dragonTalkUI;
	[SerializeField]
	private FlatFX m_flatFx;
	[SerializeField]
	private float m_damageTime = 10.0f;

	private Slime m_mainSlime;
	private SlimeType m_slimeType;
	private bool m_isGameStart = false;
	private bool m_isGameOver = false;
	private bool m_isFixedSlime = false;
	private bool m_isGenerate = false;
	private int m_slimeNum = 0;
	private float m_timeCnt;
	private float m_generateTimeCnt = 0.0f;

	public MainCamera mainCamera
	{
		get { return m_mainCamera; }
		set { m_mainCamera = value; }
	}
	public Vector3 randomPos
	{
		get { return new Vector3(Random.Range(-140, 140), Random.Range(-40, 40)); }
	}


	public StageInfo stageInfo { get; set; }
	public bool isGenerate { get { return m_isGenerate; } set { m_isGenerate = value; } }
	public bool isGameOver { get { return m_isGameOver; } }
	public bool isGameStart { get { return m_isGameStart; } set { m_isGameStart = value; } }
	public bool isFixedSlime { get { return m_isFixedSlime; } set { m_isFixedSlime = value; } }
	public int slimeNum { get { return m_slimeNum; } }
	public int preLevel { get; set; }
	public UIController uIController { get { return m_uiController; } }
	public Slime mainSlime { get { return m_mainSlime; } }
	public FlatFX flatFx { get { return m_flatFx; } }


	protected override void Awake()
	{
		isDontDestroy = false;
		base.Awake();

		//開始演出中は引っ張れないようにする
		m_isFixedSlime = true;
		preLevel = GameManager.Instance.slimeStatus.level;

		if (GameManager.Instance.isTutorial)
		{
			m_tutorialUI.SetActive(true);
			m_timeCnt = 30.0f;
			DisplayTime();
		}
		else if (GameManager.Instance.stageNo == 4)
		{
			m_dragonTalkUI.SetActive(true);
			m_timeCnt = m_damageTime;
			DisplayTime();
		}
		else
		{
			m_tutorialUI.SetActive(false);
			m_timeCnt = m_damageTime;
			DisplayTime();
			StartCoroutine(_GameStartProduction());
		}

		//ステージ生成
		stageInfo = Instantiate(m_stagePref[GameManager.Instance.stageNo], m_stageAnchor);

		//使用するスライムをアクティブにするか
		for (int i = 0; i < m_slimeObjects.Length; i++)
		{
			if (i == (int)SaveManager.Instance.saveData.userData.slimeType)
			{
				m_slimeObjects[i].gameObject.SetActive(true);
				m_mainSlime = m_slimeObjects[i];
			}
			else
			{
				m_slimeObjects[i].gameObject.SetActive(false);
			}
		}
		m_mainSlime.playerStatus = GameManager.Instance.slimeStatus;
		m_uiController.statusUI.SetStatus(m_mainSlime.playerStatus);
		m_uiController.sliderExp.SetLevel();

		m_mainSlime.transform.position = stageInfo.slimeCreatePos;

		for (int i = 0; i < stageInfo.enemyAnchors.Length; i++)
		{
			stageInfo.enemyAnchors[i].enemy.SetActive(false);
		}
	}

	private void Start()
	{
		SimpleSoundManager.Instance.StopBGM();

		if (GameManager.Instance.stageNo == 0)
		{
			SimpleSoundManager.Instance.PlayBGM(SoundNameBGM.Tutorial);
		}
	}

	private void Update()
	{
		if (m_isGameOver)
			return;

		if (!m_isGameStart)
			return;

		TimeCount();
		EnemyGenerateCheck();
	}

	private IEnumerator _GameStartProduction()
	{
		yield return new WaitForSeconds(1.0f);
		m_startWindowObj.SetActive(true);
		SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.GameStartJingle);
		yield return new WaitForSeconds(0.5f);
		m_startWindowTextDisplay.SetText("STAGE " + (GameManager.Instance.stageNo).ToString());
		yield return new WaitForSeconds(1.0f);
		m_startWindowTextDisplay.SetText("Are you ready?");
		yield return new WaitForSeconds(2.0f);
		m_startWindowTextDisplay.SetText("3");
		yield return new WaitForSeconds(1.0f);
		m_startWindowTextDisplay.SetText("2");
		yield return new WaitForSeconds(1.0f);
		m_startWindowTextDisplay.SetText("1");
		yield return new WaitForSeconds(1.0f);
		m_startWindowObj.SetActive(false);
		GameStart();
	}

	void EnemyGenerateCheck()
	{
		if (!m_isGenerate)
			return;
		m_generateTimeCnt += Time.deltaTime;

		if (m_generateTimeCnt > stageInfo.generateTimeInterval)
		{
			GenerateEnemy();
			m_generateTimeCnt = 0.0f;
		}
	}

	public void GenerateEnemy()
	{
		List<Enemy> createEnemys = new List<Enemy>();
		for (int i = 0; i < stageInfo.enemyAnchors.Length; i++)
		{
			if (stageInfo.enemyAnchors[i].IsCreate(m_mainSlime.positionVec2))
			{
				createEnemys.Add(stageInfo.enemyAnchors[i].enemy);
			}
		}

		if (createEnemys.Count <= 0)
		{
		}
		else
		{
			var ranIdx = Random.Range(0, createEnemys.Count);
			createEnemys[ranIdx].SetActive(true);
		}
	}

	void TimeCount()
	{
		m_timeCnt = Mathf.Clamp(m_timeCnt - Time.deltaTime, 0.0f, m_damageTime);
		DisplayTime();

		if (m_timeCnt <= 0.0f)
		{
			TransitionManager.Instance.Flash(0.0f, 0.2f);
			SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.Thunder);

			DamageCheck();
		}
	}

	void DisplayTime()
	{
		m_uiController.sliderTime.value = Mathf.Clamp01(m_timeCnt / m_damageTime);
	}

	/// <summary>
	/// ダメージを受けるかのチェック
	/// </summary>
	void DamageCheck()
	{
		if (m_mainSlime.level % 10 == 0)
		{
			if (GameManager.Instance.isTutorial)
			{
				TutorialManager.Instance.Success10Lvel();
				return;
			}
			StageClear();
			m_timeCnt = m_damageTime;
			m_isGameStart = false;
		}
		else
		{
			if (GameManager.Instance.isTutorial)
			{
				TutorialManager.Instance.Failed10Lvel();
				return;
			}
			GameOver();
		}
	}

	public void StageClear()
	{
		m_gameClearUI.ShowStageClear();
	}

	public void GameOver()
	{
		SimpleSoundManager.Instance.StopBGM();
		m_isFixedSlime = true;

		//スコア計算
		var score = GameManager.Instance.CalcScore(GameManager.Instance.stageNo, mainSlime.level, m_slimeNum);
		if (SaveManager.Instance.saveData.userData.highScore < score)
		{
			SaveManager.Instance.saveData.userData.highScore = score;
		}

		m_gameOverUI.gameObject.SetActive(true);
		m_gameOverUI.ShowGameOverUI(m_slimeType, GameManager.Instance.stageNo, m_slimeNum, score);
		m_isGameOver = true;
	}

	public void TutorialGameStart()
	{
		m_isGameOver = false;
		m_isGameStart = true;
		m_isGenerate = true;
	}

	public void GameStart()
	{
		SimpleSoundManager.Instance.PlayBGM(SoundNameBGM.Main2);
		m_timeCnt = m_damageTime;
		m_isGameOver = false;
		m_isGameStart = true;
		m_isGenerate = true;
		m_isFixedSlime = false;
	}

	public void SetTime(float _time)
	{
		m_timeCnt = _time;
		DisplayTime();

	}
}
