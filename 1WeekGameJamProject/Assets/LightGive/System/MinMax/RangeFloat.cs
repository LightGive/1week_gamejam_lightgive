using UnityEngine;

[System.Serializable]
public struct RangeFloat : IRange<float>
{
	public RangeFloat(float i_value)
	{
		m_minValue = i_value;
		m_maxValue = i_value;
	}

	public RangeFloat(float i_min, float i_max)
	{
		m_minValue = i_min;
		m_maxValue = Mathf.Max(i_min, i_max);
	}

	[SerializeField]
	private float m_minValue;
	[SerializeField]
	private float m_maxValue;

	public float MinValue
	{
		get { return m_minValue; }
		set
		{
			m_minValue = Mathf.Min(value, m_maxValue);
		}
	}

	public float MaxValue
	{
		get { return m_maxValue; }
		set
		{
			m_maxValue = Mathf.Max(value, m_minValue);
		}
	}

	public float MidValue
	{
		get
		{
			return m_minValue + (m_maxValue - m_minValue) / 2.0f;
		}
	}

	public float Length
	{
		get
		{
			return Mathf.Abs(m_maxValue - m_minValue);
		}
	}

	public float RandomValue
	{
		get
		{
			return m_minValue < m_maxValue ? Random.Range(m_minValue, m_maxValue) : m_minValue;
		}
	}
}