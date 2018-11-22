using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DisplayNumberSprite : MonoBehaviour
{
	[SerializeField]
	private Sprite[] m_spriteNumbers;
	[SerializeField]
	private SpriteRenderer[] m_spriteRenderers;
	[SerializeField]
	private Vector3 m_offset;

	public void SetDelete()
	{
		for (int i = 0; i < m_spriteRenderers.Length; i++)
		{
			m_spriteRenderers[i].gameObject.SetActive(false);
		}
	}

	public void SetNumber(int _num)
	{
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
			if (number.Count <= i && i != number.Count - 1)
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
		for (int i = 0; i < number.Count; i++)
		{
			var center = (m_offset.x * (number.Count + 1)) * 0.5f;
			m_spriteRenderers[(number.Count - 1) - (i - 1)].transform.localPosition = (m_offset * i) - new Vector3(center, 0.0f, 0.0f);
		}
	}
}
