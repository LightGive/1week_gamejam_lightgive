using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DisplayNumberSprite : MonoBehaviour
{
	[SerializeField]
	private Sprite[] m_spriteNumbers;
	[SerializeField]
	private Sprite[] m_spriteOperator;
	[SerializeField]
	private SpriteRenderer[] m_spriteRenderers;
	[SerializeField]
	private SpriteRenderer m_spriteRendererOpe;
	[SerializeField]
	private Vector3 m_offset;


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			var num = Random.Range(0, 99);
			Debug.Log(num);
			SetNumber(Operator.Add, num);
		}
	}

	public void SetDelete()
	{
		for (int i = 0; i < m_spriteRenderers.Length; i++)
		{
			m_spriteRenderers[i].gameObject.SetActive(false);
		}
	}

	public void SetNumber(Operator _ope, int _num)
	{
		//演算子表示
		m_spriteRendererOpe.sprite = m_spriteOperator[(int)_ope];

		var digit = _num;

		//要素数0には１桁目の値が格納
		List<int> number = new List<int>();
		while (digit != 0)
		{
			_num = digit % 10;
			digit = digit / 10;
			number.Add(_num);
		}

		//表示
		for (int i = 0; i < m_spriteRenderers.Length; i++)
		{
			if (number.Count <= i)
			{
				m_spriteRenderers[i].gameObject.SetActive(false);
			}
			else
			{
				m_spriteRenderers[i].gameObject.SetActive(true);
				m_spriteRenderers[i].sprite = m_spriteNumbers[number[i]];
			}
		}

		//座標を変更
		for (int i = 0; i < number.Count + 1; i++)
		{
			var center = (m_offset.x * (number.Count + 1)) * 0.5f;
			if (i == 0)
			{
				m_spriteRendererOpe.transform.localPosition = (m_offset * i) - new Vector3(center, 0.0f, 0.0f);
			}
			else
			{
				m_spriteRenderers[(number.Count - 1) - (i - 1)].transform.localPosition = (m_offset * i) - new Vector3(center, 0.0f, 0.0f);
			}
		}

	}
}
