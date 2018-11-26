using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightGive;


public class GameManager : SingletonMonoBehaviour<GameManager>
{
	private const int StagePoint = 2000;
	private const int SlimeLevelPoint = 500;
	private const int SlimeNumPoint = 100;

	public int stageNo;
	public int slimeLevel;
	public int slimeNum;

	public PlayerStatus slimeStatus;
	public bool isTutorial { get { return stageNo == 0; } }
	private bool m_isDebugTime;

	protected override void Awake()
	{
		isDontDestroy = true;
		base.Awake();
	}

	private void Update()
	{

	}

	public void ResetStatus()
	{
		slimeStatus = new PlayerStatus(isTutorial);
	}

	public int CalcScore(int _stageNo, int _slimeLevel, int _slimeNum)
	{
		return (_stageNo + StagePoint) + (_slimeLevel * SlimeLevelPoint) + (_slimeNum * SlimeNumPoint);
	}
}
