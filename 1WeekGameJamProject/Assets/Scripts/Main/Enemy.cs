using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField]
	protected DisplayNumberSprite m_numberSpriteDisplay;
	[SerializeField]
	protected Operator m_ope;
	[SerializeField]
	protected int m_num;

	public Operator ope { get { return m_ope; } }
	public int num
	{
		get { return m_num; }
		set { m_num = value; }
	}

	private void Start()
	{
		m_numberSpriteDisplay.SetNumber(m_ope, m_num);
	}

	public void Hit()
	{
		transform.position = SceneMain.Instance.randomPos;
	}
}