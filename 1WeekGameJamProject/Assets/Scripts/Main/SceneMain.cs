using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightGive;

public class SceneMain : SingletonMonoBehaviour<SceneMain>
{
	[SerializeField]
	private MainCamera m_mainCamera;

	private float m_timeCnt;

	private void Update()
	{
		m_timeCnt += Time.deltaTime;
	}

	public MainCamera mainCamera
	{
		get { return m_mainCamera; }
		set { m_mainCamera = value; }
	}

	public Vector3 randomPos
	{
		get { return new Vector3(Random.Range(-140, 140), Random.Range(-40, 40)); }
	}
}
