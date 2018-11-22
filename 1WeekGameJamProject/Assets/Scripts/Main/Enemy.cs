using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LightGive;

public class Enemy : MonoBehaviour
{
	[SerializeField]
	private Animator m_anim;
	[SerializeField]
	protected DisplayNumberSprite m_numberSpriteDisplay;
	[SerializeField]
	private SpriteParamChanger m_sprite;
	[SerializeField]
	private EnemyHitPointBar m_hitPointBar;
	[SerializeField]
	private EnemyStatus m_status;
	[SerializeField]
	private bool m_isInvisible = false;


	private void Start()
	{
		m_status.hp = m_status.maxHp;
		m_numberSpriteDisplay.SetNumber(m_status.level);
	}

	public void Hit(int _damage)
	{
		if (m_isInvisible)
			return;

		m_status.hp = Mathf.Clamp(m_status.hp - _damage, 0, m_status.maxHp);
		m_hitPointBar.SetBarLerp((float)m_status.hp / m_status.maxHp);
		if (m_status.hp <= 0)
		{
			m_status.hp = m_status.maxHp;

			SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.EnemyDead);
			transform.position = SceneMain.Instance.randomPos;
			SceneMain.Instance.slime.AddExp(m_status.exp);
		}
		else
		{
			SimpleSoundManager.Instance.PlaySE_2D(SoundNameSE.EnemyDamage);
			m_isInvisible = true;
			StartCoroutine(_DamageProduction());
		}
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

	[System.Serializable]
	public class EnemyStatus
	{
		public int level;
		public int hp;
		public int maxHp;
		public int exp;
	}

}