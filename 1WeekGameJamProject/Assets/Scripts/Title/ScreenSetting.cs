using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScreenSetting : MonoBehaviour
{
	[SerializeField]
	private Slider m_sliderBgm;
	[SerializeField]
	private Slider m_sliderSe;
	[SerializeField]
	private TextMeshProUGUI m_textBGMVolume;
	[SerializeField]
	private TextMeshProUGUI m_textSEVolume;

	private void OnEnable()
	{
		m_sliderBgm.value = SimpleSoundManager.Instance.volumeBgm * 10;
		m_sliderSe.value = SimpleSoundManager.Instance.volumeSe * 10;
	}

	public void OnChangeValueSEVolume(float _val)
	{
		m_textSEVolume.text = (_val * 10.0f).ToString("0") + "％";
		SimpleSoundManager.Instance.volumeSe = _val * 0.1f;
	}

	public void OnChangeValueBGMVolume(float _val)
	{
		m_textBGMVolume.text = (_val * 10.0f).ToString("0") + "％";
		SimpleSoundManager.Instance.volumeBgm = _val * 0.1f;
	}
}
