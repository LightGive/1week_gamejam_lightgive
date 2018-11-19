using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainArrow : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer m_spRenderer;
	[SerializeField]
	private float m_arrowWidh;
	[SerializeField]
	private RangeFloat m_rangeArrowLength;

	public float lerp { get; set; }
	public bool isActive { get; set; }

	private void Awake()
	{
		SetActive(false);
	}

	public void SetActive(bool _isActive)
	{
		isActive = _isActive;
		m_spRenderer.gameObject.SetActive(_isActive);
	}

	public void ChangeArrowLengh(float _val, Vector2 _vec)
	{
		lerp = Mathf.Clamp01(_val);
		m_spRenderer.size = new Vector2(m_arrowWidh, Mathf.Lerp(m_rangeArrowLength.MinValue, m_rangeArrowLength.MaxValue, _val));
		transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Atan2(_vec.y, _vec.x) * Mathf.Rad2Deg + 90.0f);
	}
}
