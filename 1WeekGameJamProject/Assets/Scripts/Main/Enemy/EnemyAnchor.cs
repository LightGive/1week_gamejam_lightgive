using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnchor : MonoBehaviour
{
	[SerializeField]
	private Animator m_movementAnim;
	[SerializeField]
	private Enemy m_enemy;
	[SerializeField]
	private float m_enemyNotCreateRad;

	public Animator movementAnim { get { return m_movementAnim; } }
	public Enemy enemy { get { return m_enemy; } }

	void Start()
	{

	}

	void Update()
	{

	}

	public void OnChangeMovementState(Enemy.MovementState _state)
	{
		m_enemy.movementState = _state;
	}

	public void OnChangeMoveDir(Enemy.MoveDirection _dir)
	{
		m_enemy.dir = _dir;
	}

	public bool IsCreate(Vector2 _slimePos)
	{
		var dis = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), _slimePos);
		return (dis > m_enemyNotCreateRad) && !enemy.isActive;
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, m_enemyNotCreateRad);
	}
}
