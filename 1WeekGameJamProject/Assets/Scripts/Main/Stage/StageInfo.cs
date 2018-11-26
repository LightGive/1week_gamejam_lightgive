using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInfo : MonoBehaviour
{
	[SerializeField]
	private EnemyAnchor[] m_enemyaAnchors;
	[SerializeField]
	private Vector2 m_slimeCreatePos;
	[SerializeField]
	private float m_generateTimeInterval;

	public Vector2 slimeCreatePos { get { return m_slimeCreatePos; } }
	public EnemyAnchor[] enemyAnchors { get { return m_enemyaAnchors; } }
	public float generateTimeInterval { get { return m_generateTimeInterval; } }

	void Start()
	{

	}

	void Update()
	{

	}
}
