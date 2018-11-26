using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightGive;

public class Enemy : MonoBehaviour
{
	private const string AnimParamMovementState = "MovementState";

	[SerializeField]
	protected GameObject m_uiObject;
	[SerializeField]
	protected Animator m_anim;
	[SerializeField]
	protected DisplayNumberSprite m_numberSpriteDisplay;
	[SerializeField]
	protected SpriteParamChanger m_sprite;
	[SerializeField]
	protected SpriteRenderer m_spriteRenderer;
	[SerializeField]
	protected EnemyHitPointBar m_hitPointBar;
	[SerializeField]
	protected EnemyAnchor m_enemyAnchor;
	[SerializeField]
	protected EnemyStatus m_status;
	[SerializeField]
	protected float m_appearTime = 2.0f;
	[SerializeField]
	protected float m_reflectVelocity = 1.0f;
	[SerializeField]
	protected bool m_isInvisible = false;


	protected MovementState m_movementState;
	private float m_movementTime = 0.0f;
	private MoveDirection m_dir = MoveDirection.Right;
	private bool m_isActive = false;

	public Vector2 positionVec2 { get { return new Vector2(transform.position.x, transform.position.y); } }
	public bool isActive
	{
		get { return m_isActive; }
	}
	public MoveDirection dir
	{
		get { return m_dir; }
		set
		{
			m_dir = value;
			m_spriteRenderer.flipX = (m_dir == MoveDirection.Left);
		}
	}

	public MovementState movementState
	{
		get { return m_movementState; }
		set
		{
			m_movementState = value;
			m_anim.SetInteger(AnimParamMovementState, (int)m_movementState);
		}
	}


	protected virtual void Start()
	{
		m_status.hp = m_status.maxHp;
		m_numberSpriteDisplay.SetNumber(m_status.level);
	}

	protected virtual void Update()
	{
		switch (m_movementState)
		{
			case MovementState.Appear:
				AppearUpdate();
				break;
			case MovementState.Idle:
				IdleUpdate();
				break;
			case MovementState.Move:
				MoveUpdate();
				break;
		}
	}

	public void Init()
	{
		m_status.hp = m_status.maxHp;
		m_numberSpriteDisplay.SetNumber(m_status.level);
	}

	public void SetActive(bool _isActive)
	{
		if (_isActive)
		{
			m_sprite.WhiteColor = 0.0f;
			m_status.hp = m_status.maxHp;
			m_hitPointBar.SetBarLerp((float)m_status.hp / m_status.maxHp);
			m_isInvisible = false;
		}

		m_isActive = _isActive;
		m_enemyAnchor.movementAnim.Rebind();
		gameObject.SetActive(_isActive);
	}

	public void Hit(int _damage, Vector2 _hitPos)
	{
		if (SceneMain.Instance.isGameOver)
			return;
		if (m_isInvisible)
			return;
		var vec = (_hitPos - positionVec2).normalized * m_reflectVelocity;
		SceneMain.Instance.mainSlime.AddForce(vec);

		m_status.hp = Mathf.Clamp(m_status.hp - _damage, 0, m_status.maxHp);
		m_hitPointBar.SetBarLerp((float)m_status.hp / m_status.maxHp);
		if (m_status.hp <= 0)
		{
			m_isInvisible = true;
			m_status.hp = m_status.maxHp;
			StartCoroutine(_DeadProduction());
			SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.EnemyDead);

			//transform.position = SceneMain.Instance.randomPos;
			SceneMain.Instance.mainSlime.AddExp(m_status.level * 100);
		}
		else
		{
			StartCoroutine(this.DelayMethod(0.0f, PlayEffect, _hitPos, 1));
			StartCoroutine(this.DelayMethod(0.2f, PlayEffect, _hitPos, 1));


			SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.EnemyDamage);
			m_isInvisible = true;
			StartCoroutine(_DamageProduction());
		}
	}

	public void Appear()
	{

	}

	public virtual void AppearUpdate()
	{
		m_movementTime += Time.deltaTime;
		if (m_movementTime > m_appearTime)
		{

		}
	}

	public virtual void IdleUpdate()
	{
		m_movementTime += Time.deltaTime;
	}

	public virtual void MoveUpdate()
	{
		m_movementTime += Time.deltaTime;
	}

	public virtual void Dead()
	{
	}

	void PlayEffect(Vector2 _hitPos, int _effectNo)
	{
		SceneMain.Instance.flatFx.AddEffect(_hitPos, _effectNo);
	}

	/// <summary>
	/// ダメージ演出
	/// </summary>
	/// <returns>The production.</returns>
	private IEnumerator _DamageProduction()
	{
		m_sprite.WhiteColor = 1.0f;
		yield return new WaitForSeconds(0.1f);
		m_sprite.WhiteColor = 0.0f;
		yield return new WaitForSeconds(0.1f);
		m_sprite.WhiteColor = 1.0f;
		yield return new WaitForSeconds(0.1f);
		m_sprite.WhiteColor = 0.0f;
		yield return new WaitForSeconds(0.1f);

		m_isInvisible = false;
	}

	public void ChangeDirection(int _dir)
	{

	}

	private IEnumerator _DeadProduction()
	{
		m_sprite.WhiteColor = 1.0f;
		yield return new WaitForSeconds(0.1f);
		m_sprite.WhiteColor = 0.0f;
		yield return new WaitForSeconds(0.1f);
		m_sprite.WhiteColor = 1.0f;
		StartCoroutine(this.DelayMethod(0.0f, PlayEffect, new Vector2(transform.position.x, transform.position.y), 2));
		yield return new WaitForSeconds(0.2f);
		Dead();
		SetActive(false);
	}

	[System.Serializable]
	public class EnemyStatus
	{
		public int level;
		public int hp;
		public int maxHp;
	}

	public enum MovementState
	{
		Appear = 0,
		Idle = 1,
		Move = 2,
		Attack = 3,
	}

	public enum MoveDirection
	{
		Right = 1,
		Left = -1
	}
}