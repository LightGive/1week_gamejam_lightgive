using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
	[SerializeField]
	private Slider m_sliderTime;
	[SerializeField]
	private SliderExp m_sliderExp;
	[SerializeField]
	private TextMeshProUGUI m_textTime;
	[SerializeField]
	private TextMeshProUGUI m_textSlimeExp;
	[SerializeField]
	private TextMeshProUGUI m_textLevel;

	public TextMeshProUGUI textLevel { get { return m_textLevel; } }

	public SliderExp sliderExp { get { return m_sliderExp; } }

	public void SetTime(float _maxTime, float _nowTime)
	{
		m_sliderTime.value = Mathf.Clamp01(_nowTime / _maxTime);
	}

}
