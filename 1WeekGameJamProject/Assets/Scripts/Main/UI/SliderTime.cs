using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderTime : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_textTime;
	[SerializeField]
	private Slider m_slider;

	public void ChangeValue(float _val)
	{
		m_textTime.text = (_val * 60.0f).ToString("F2");
	}
}
